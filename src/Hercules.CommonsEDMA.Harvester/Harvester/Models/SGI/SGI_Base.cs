using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Harvester;
using Harvester.Models.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OAI_PMH.Models.SGI
{
    public abstract class SGI_Base
    {
        internal string gnossId;

        /// <summary>
        /// Transforma un objeto a un recurso a cargar.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public abstract ComplexOntologyResource ToRecurso(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, bool pFusionarPersona = false, string pIdPersona = null);

        /// <summary>
        /// Transforma un objeto a un recurso a cargar.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <param name="pRabbitConf"></param>
        /// <param name="pFusionarPersona"></param>
        /// <param name="pIdPersona"></param>
        /// <returns></returns>
        public abstract void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss);

        /// <summary>
        /// Obtiene el ID en BBDD preguntando por el crisidentifier.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public abstract string ObtenerIDBBDD(ResourceApi pResourceApi);

        /// <summary>
        /// Carga/Modifica el recurso.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGrafo"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public string Cargar(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, string pIdGrafo, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, bool pPersona = false)
        {
            gnossId = ObtenerIDBBDD(pResourceApi);
            pResourceApi.ChangeOntoly(pIdGrafo);

            // Modificación.                
            ComplexOntologyResource resource;

            if (!string.IsNullOrEmpty(gnossId))
            {   
                if(pPersona)
                {
                    resource = ToRecurso(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas, pRabbitConf, true, gnossId);
                }
                else
                {
                    resource = ToRecurso(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas, pRabbitConf);
                }
                
                resource.GnossId = gnossId;

                int numIntentos = 0;
                while (!resource.Modified)
                {
                    numIntentos++;

                    if (numIntentos > 6)
                    {
                        break;
                    }
                    pResourceApi.ModifyComplexOntologyResource(resource, false, false);
                }                
            }
            else
            {
                // Carga.
                resource = ToRecurso(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas, pRabbitConf);

                int numIntentos = 0;
                while (!resource.Uploaded)
                {
                    numIntentos++;

                    if (numIntentos > 6)
                    {
                        break;
                    }
                    pResourceApi.LoadComplexSemanticResource(resource, false, false);
                }
                
                gnossId = resource.GnossId;
            }

            // Carga adicional.
            ToRecursoAdicional(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas, pRabbitConf, gnossId);

            // Inserción en la cola de Rabbit.
            switch (pIdGrafo)
            {
                case "person":
                    pRabbitConf.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.person, new HashSet<string>() { resource.GnossId }));
                    break;
                case "project":
                    pRabbitConf.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.project, new HashSet<string>() { resource.GnossId }));
                    break;
                case "group":
                    pRabbitConf.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.group, new HashSet<string>() { resource.GnossId }));
                    break;
                case "patent":
                    pRabbitConf.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.patent, new HashSet<string>() { resource.GnossId }));
                    break;
                case "organization":
                    pRabbitConf.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.organization, new HashSet<string>() { resource.GnossId }));
                    break;
                case "projectauthorization":
                    pRabbitConf.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.projectauthorization, new HashSet<string>() { resource.GnossId }));
                    break;
            }

            return resource.GnossId;
        }        

        /// <summary>
        /// Divide una lista.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pItems"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(List<T> pItems, int pSize)
        {
            for (int i = 0; i < pItems.Count; i += pSize)
            {
                yield return pItems.GetRange(i, Math.Min(pSize, pItems.Count - i));
            }
        }

        /// <summary>
        /// Resta dos fechas devolviendo los días, meses y años entre ellas.
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static Tuple<string, string, string> RestarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            int total = (fechaFin - fechaInicio).Days;
            int anios = total / 365;
            int meses = (total - (365 * anios)) / 30;
            int dias = total - (365 * anios) - (30 * meses);
            return new Tuple<string, string, string>(anios.ToString(), meses.ToString(), dias.ToString());
        }
    }
}
