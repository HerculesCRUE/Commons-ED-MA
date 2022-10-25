using Hercules.CommonsEDMA.Desnormalizador.Models;
using Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores;
using Hercules.CommonsEDMA.Desnormalizador.Models.Services;
using Gnoss.ApiWrapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Hercules.CommonsEDMA.Desnormalizador.Program;

namespace Hercules.CommonsEDMA.Desnormalizador
{
    public class Worker : BackgroundService
    {
        private readonly ConfigService _configService;
        private readonly RabbitServiceReaderDenormalizer _rabbitServiceReaderDenormalizer;
        private readonly string _directoryPendingCV;
        private readonly string _directoryPending;
        private readonly string _directoryError;

        /// <summary>
        /// Contructor.
        /// </summary>
        public Worker(ConfigService configService, RabbitServiceReaderDenormalizer rabbitServiceReaderDenormalizer)
        {
            _configService = configService;
            _rabbitServiceReaderDenormalizer = rabbitServiceReaderDenormalizer;

            _directoryPendingCV = _configService.GetRutaDirectorioEscritura() + "pendingcv/";
            _directoryPending = _configService.GetRutaDirectorioEscritura() + "pending/";
            _directoryError = _configService.GetRutaDirectorioEscritura() + "error/";
            if (!Directory.Exists(_directoryPendingCV))
            {
                Directory.CreateDirectory(_directoryPendingCV);
            }
            if (!Directory.Exists(_directoryPending))
            {
                Directory.CreateDirectory(_directoryPending);
            }
            if (!Directory.Exists(_directoryError))
            {
                Directory.CreateDirectory(_directoryError);
            }
        }

        /// <summary>
        /// Tarea asincrona.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenToQueue();            

            ProcessItemsPendingCV();

            ProcessItemsPending();

            ProcessItemsError();

            ProcessComplete();
        }


        /// <summary>
        /// Permite lanzar la escucha después de leer. 
        /// Contiene un sleep de 30 segundos.
        /// </summary>
        private void OnShutDown()
        {
            Thread.Sleep(30000);
            ListenToQueue();
        }

        /// <summary>
        /// Lectura de una cola Rabbit.
        /// </summary>
        private void ListenToQueue()
        {
            _rabbitServiceReaderDenormalizer.ListenToQueue(new RabbitServiceReaderDenormalizer.ReceivedDelegate(ProcessItem), new RabbitServiceReaderDenormalizer.ShutDownDelegate(OnShutDown), _configService.GetDenormalizerQueueRabbit());
        }

        /// <summary>
        /// Ejecuta un procesado completo del desnormalizador
        /// </summary>
        private void ProcessComplete()
        {
            string denormalizerCronExpression = _configService.GetDenormalizerCronExpression();
            bool ejecutado = false;
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {

                        var expression = new CronExpression(denormalizerCronExpression);
                        DateTimeOffset? time = expression.GetTimeAfter(DateTimeOffset.UtcNow);
                        if (!ejecutado)
                        {
                            ActualizadorEDMA.DesnormalizarTodo(_configService);
                            ejecutado = true;
                        }
                        if (time.HasValue)
                        {
                            Thread.Sleep((time.Value.UtcDateTime - DateTimeOffset.UtcNow));
                            ActualizadorEDMA.DesnormalizarTodo(_configService);
                        }
                    }
                    catch (Exception ex)
                    {
                        FileLogger.Log(ex);
                        Thread.Sleep(60000);
                    }
                }
            }).Start();
        }

        /// <summary>
        /// Procesa los ficheros generados al leer la cola de rabit para generar datos del CV
        /// </summary>
        private void ProcessItemsPendingCV()
        {
            new Thread(() =>
            {
                while (true)
                {
                    //Ficheros para procesar
                    HashSet<string> filesToProcess = new HashSet<string>();
                    try
                    {
                        Thread.Sleep(1000);
                        List<string> files = Directory.GetFiles(_directoryPendingCV).OrderBy(x => x).ToList();

                        if (files.Count > 0)
                        {
                            //Items máximos a procesar
                            int maxItems = 500;

                            //Item para agrupar los IDs
                            Dictionary<DenormalizerItemQueue.ItemType, HashSet<string>> agrupados = new Dictionary<DenormalizerItemQueue.ItemType, HashSet<string>>();
                            foreach (string file in files)
                            {
                                filesToProcess.Add(file);
                                DenormalizerItemQueue denormalizerItemQueue = JsonConvert.DeserializeObject<DenormalizerItemQueue>(File.ReadAllText(file));
                                if (!agrupados.ContainsKey(denormalizerItemQueue._itemType))
                                {
                                    agrupados[denormalizerItemQueue._itemType] = new HashSet<string>();
                                }
                                agrupados[denormalizerItemQueue._itemType].UnionWith(denormalizerItemQueue._items);
                                if (agrupados.Where(x => x.Value.Count > maxItems).Count() > 0)
                                {
                                    break;
                                }
                            }

                            foreach (DenormalizerItemQueue.ItemType type in agrupados.Keys)
                            {
                                List<List<string>> listLists = ActualizadorBase.SplitList(agrupados[type].ToList(), maxItems).ToList();
                                foreach (List<string> listIn in listLists)
                                {
                                    switch (type)
                                    {
                                        case DenormalizerItemQueue.ItemType.person:
                                            ActualizadorEDMA.DesnormalizarDatosCVPersonas(pPersons: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.group:
                                            ActualizadorEDMA.DesnormalizarDatosCVGrupos(pGroups: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.project:
                                            ActualizadorEDMA.DesnormalizarDatosCVProyectos(pProjects: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.document:
                                            ActualizadorEDMA.DesnormalizarDatosCVDocumento(pDocuments: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.researchobject:
                                            ActualizadorEDMA.DesnormalizarDatosCVResearchObject(pROs: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.patent:
                                            ActualizadorEDMA.DesnormalizarDatosCVPatentes(pPatents: listIn);
                                            break;

                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        FileLogger.Log(ex);
                    }
                    finally
                    {
                        foreach (string file in filesToProcess)
                        {
                            System.IO.File.Move(file, file.Replace(_directoryPendingCV, _directoryPending));
                        }
                    }

                }
            }).Start();
        }

        /// <summary>
        /// Procesa los ficheros generados al leer la cola de rabit para generar datos
        /// </summary>
        private void ProcessItemsPending()
        {
            new Thread(() =>
            {
                while (true)
                {
                    //Ficheros para procesar
                    HashSet<string> filesToProcess = new HashSet<string>();
                    try
                    {
                        Thread.Sleep(1000);
                        List<string> files = System.IO.Directory.GetFiles(_directoryPending).OrderBy(x => x).ToList();

                        if (files.Count > 0)
                        {
                            //Items máximos a procesar
                            int maxItems = 500;

                            //Item para agrupar los IDs
                            Dictionary<DenormalizerItemQueue.ItemType, HashSet<string>> agrupados = new Dictionary<DenormalizerItemQueue.ItemType, HashSet<string>>();
                            foreach (string file in files)
                            {
                                filesToProcess.Add(file);
                                DenormalizerItemQueue denormalizerItemQueue = JsonConvert.DeserializeObject<DenormalizerItemQueue>(File.ReadAllText(file));
                                if (!agrupados.ContainsKey(denormalizerItemQueue._itemType))
                                {
                                    agrupados[denormalizerItemQueue._itemType] = new HashSet<string>();
                                }
                                agrupados[denormalizerItemQueue._itemType].UnionWith(denormalizerItemQueue._items);
                                if (agrupados.Where(x => x.Value.Count > maxItems).Count() > 0)
                                {
                                    break;
                                }
                            }

                            foreach (DenormalizerItemQueue.ItemType type in agrupados.Keys)
                            {
                                List<List<string>> listLists = ActualizadorBase.SplitList(agrupados[type].ToList(), maxItems).ToList();
                                foreach (List<string> listIn in listLists)
                                {
                                    switch (type)
                                    {
                                        case DenormalizerItemQueue.ItemType.person:
                                            ActualizadorEDMA.DesnormalizarDatosPersonas(pPersons: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.group:
                                            ActualizadorEDMA.DesnormalizarDatosGrupos(pGroups: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.project:
                                            ActualizadorEDMA.DesnormalizarDatosProyectos(pProjects: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.document:
                                            ActualizadorEDMA.DesnormalizarDatosDocumento(pDocuments: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.researchobject:
                                            ActualizadorEDMA.DesnormalizarDatosResearchObject(pROs: listIn);
                                            break;
                                        case DenormalizerItemQueue.ItemType.patent:
                                            ActualizadorEDMA.DesnormalizarDatosPatentes(pPatents: listIn);
                                            break;

                                    }
                                }
                            }

                            foreach (string file in filesToProcess)
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        foreach (string file in filesToProcess)
                        {
                            File.Move(file, file.Replace(_directoryPending, _directoryError));
                        }
                        FileLogger.Log(ex);
                    }

                }
            }).Start();
        }

        /// <summary>
        /// Procesa los ficheros que han dado error al leer la cola de rabit para generar datos
        /// </summary>
        private void ProcessItemsError()
        {
            new Thread(() =>
            {
                while (true)
                {
                    //Ficheros para procesar
                    HashSet<string> filesToProcess = new HashSet<string>();
                    try
                    {
                        Thread.Sleep(1000);
                        List<string> files = System.IO.Directory.GetFiles(_directoryError).OrderBy(x => x).ToList();

                        if (files.Count > 0)
                        {
                            //Items máximos a procesar
                            int maxItems = 500;
                            foreach (string file in files)
                            {
                                try
                                {
                                    DenormalizerItemQueue denormalizerItemQueue = JsonConvert.DeserializeObject<DenormalizerItemQueue>(File.ReadAllText(file));
                                    List<List<string>> listLists = ActualizadorBase.SplitList(denormalizerItemQueue._items.ToList(), maxItems).ToList();
                                    foreach (List<string> listIn in listLists)
                                    {
                                        switch (denormalizerItemQueue._itemType)
                                        {
                                            case DenormalizerItemQueue.ItemType.person:
                                                ActualizadorEDMA.DesnormalizarDatosPersonas(pPersons: listIn);
                                                break;
                                            case DenormalizerItemQueue.ItemType.group:
                                                ActualizadorEDMA.DesnormalizarDatosGrupos(pGroups: listIn);
                                                break;
                                            case DenormalizerItemQueue.ItemType.project:
                                                ActualizadorEDMA.DesnormalizarDatosProyectos(pProjects: listIn);
                                                break;
                                            case DenormalizerItemQueue.ItemType.document:
                                                ActualizadorEDMA.DesnormalizarDatosDocumento(pDocuments: listIn);
                                                break;
                                            case DenormalizerItemQueue.ItemType.researchobject:
                                                ActualizadorEDMA.DesnormalizarDatosResearchObject(pROs: listIn);
                                                break;
                                            case DenormalizerItemQueue.ItemType.patent:
                                                ActualizadorEDMA.DesnormalizarDatosPatentes(pPatents: listIn);
                                                break;

                                        }
                                    }
                                    File.Delete(file);
                                }
                                catch(Exception ex)
                                {
                                    FileLogger.Log(ex);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        FileLogger.Log(ex);
                    }

                }
            }).Start();
        }


        /// <summary>
        /// Procesa una cola de rabbit
        /// </summary>
        /// <param name="pMessage">item</param>
        /// <returns></returns>
        public bool ProcessItem(string pMessage)
        {
            DenormalizerItemQueue denormalizerItemQueue = JsonConvert.DeserializeObject<DenormalizerItemQueue>(pMessage);
            File.WriteAllText(_directoryPendingCV + DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid(), JsonConvert.SerializeObject(denormalizerItemQueue));
            return true;
        }

    }
}
