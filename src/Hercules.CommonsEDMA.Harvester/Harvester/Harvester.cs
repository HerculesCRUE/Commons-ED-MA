using Harvester.Models;
using Newtonsoft.Json;
using OAI_PMH.Models.SGI.Organization;
using OAI_PMH.Models.SGI.PersonalData;
using OAI_PMH.Models.SGI.Project;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Harvester
{
    public class Harvester
    {
        public IHaversterServices HaversterServices;

        public Harvester(IHaversterServices haversterServices)
        {
            this.HaversterServices = haversterServices;
        }

        public void Harvest(ReadConfig pConfig, string pSet, string pFecha)
        {
            // Obtención de los IDs.
            HashSet<string> listaIdsSinRepetir = new ();

            List<IdentifierOAIPMH> idList = HaversterServices.ListIdentifiers(pFecha, pConfig, set: pSet);

            if (idList != null)
            {
                foreach (var id in idList)
                {
                    listaIdsSinRepetir.Add(id.Identifier);
                }
                List<string> listaIdsOrdenados = listaIdsSinRepetir.ToList();
                listaIdsOrdenados.Sort();

                // Guardado de los IDs.
                string directorio = pConfig.GetLogCargas() + $@"/{pSet}/pending/";
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                File.WriteAllLines(directorio + $@"{pSet}_{pFecha.Replace(":", "-")}.txt", listaIdsOrdenados);

                // Se actualiza la última fecha de carga.
                UpdateLastDate(pConfig, pSet, DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
            }
            else
            {
                // No se han podido obtener los IDs.
            }
        }

        /// <summary>
        /// Modifica el fichero con la última fecha.
        /// </summary>
        /// <param name="pConfig"></param>
        public void UpdateLastDate(ReadConfig pConfig, string pSet, string pFecha)
        {
            if (!Directory.Exists(pConfig.GetLastUpdateDateFolder()))
            {
                Directory.CreateDirectory(pConfig.GetLastUpdateDateFolder());
            }

            File.WriteAllText(pConfig.GetLastUpdateDateFolder() + $@"\\lastUpdateDate_{pSet}.txt", pFecha);
        }

        public void HarvestPRC(ReadConfig pConfig, string pSet, string pFecha)
        {
            // Obtención de los IDs.
            HashSet<string> listaIdsSinRepetir = new ();

            List<ListRecordsOAIPMH> idList = HaversterServices.ListRecords(pFecha, pConfig, set: pSet);

            if (idList != null && idList.Any())
            {
                foreach (var id in idList)
                {
                    listaIdsSinRepetir.Add(id.Identifier + "||" + id.Estado);
                }
                List<string> listaIdsOrdenados = listaIdsSinRepetir.ToList();
                listaIdsOrdenados.Sort();

                // Guardado de los IDs.
                string directorio = pConfig.GetLogCargas() + $@"/{pSet}/pending/";
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                File.WriteAllLines(directorio + $@"{pSet}_{pFecha.Replace(":", "-")}.txt", listaIdsOrdenados);

                // Se actualiza la última fecha de carga.
                UpdateLastDate(pConfig, pSet, DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));
            }
            else
            {
                // No se han podido obtener los IDs.
            }
        }
    }
}
