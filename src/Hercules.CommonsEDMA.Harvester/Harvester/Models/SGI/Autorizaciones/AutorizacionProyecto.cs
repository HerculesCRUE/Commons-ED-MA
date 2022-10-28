using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Harvester.Models.RabbitMQ;
using OAI_PMH.Models.SGI;
using OAI_PMH.Models.SGI.PersonalData;
using ProjectauthorizationOntology;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Harvester.Models.SGI.Autorizaciones
{
    public class Autorizacion : SGI_Base
    {
        /// <summary>
        /// Crea el objeto ComplexOntologyResource para ser cargado.
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
        public override ComplexOntologyResource ToRecurso(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, bool pFusionarPersona = false, string pIdPersona = null)
        {
            ProjectAuthorization autorizacion = CrearAutorizationOntology(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            pResourceApi.ChangeOntoly("projectauthorization");
            return autorizacion.ToGnossApiResource(pResourceApi, null);
        }

        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            // No necesario para esta clase.
            return string.Empty;
        }

        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {
            // No necesario para esta clase.
        }

        /// <summary>
        /// Obtiene los datos de las Autorizaciones del SGI.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pId"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public static Autorizacion GetAutorizacionSGI(IHarvesterServices pHarvesterServices, ReadConfig pConfig, string pId, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            // Obtención de datos en bruto.
            Autorizacion autorizacion = new();
            string xmlResult = pHarvesterServices.GetRecord(pId, pConfig);

            if (string.IsNullOrEmpty(xmlResult))
            {
                return null;
            }

            XmlSerializer xmlSerializer = new(typeof(Autorizacion));
            using (StringReader sr = new(xmlResult))
            {
                autorizacion = (Autorizacion)xmlSerializer.Deserialize(sr);
            }

            return autorizacion;
        }

        /// <summary>
        /// Crea el objeto ProjectAuthorization.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public ProjectAuthorization CrearAutorizationOntology(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            ProjectAuthorization autorizacion = new();
            autorizacion.Roh_crisIdentifier = this.entidadRef;
            autorizacion.Roh_title = this.tituloProyecto;
            autorizacion.IdRoh_owner = Persona.ObtenerPersonasBBDD(new HashSet<string>() { this.solicitanteRef }, pResourceApi)[this.solicitanteRef];
            return autorizacion;
        }

        public int id { get; set; }
        public string solicitanteRef { get; set; }
        public string tituloProyecto { get; set; }
        public string entidadRef { get; set; }
        public string responsableRef { get; set; }
        public string datosResponsable { get; set; }
    }
}
