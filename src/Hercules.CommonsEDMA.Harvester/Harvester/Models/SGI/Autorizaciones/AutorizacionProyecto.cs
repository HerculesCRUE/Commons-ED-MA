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
        public override ComplexOntologyResource ToRecurso(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, bool pFusionarPersona = false, string pIdPersona = null)
        {
            ProjectAuthorization autorizacion = CrearAutorizationOntology(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            pResourceApi.ChangeOntoly("projectauthorization");
            return autorizacion.ToGnossApiResource(pResourceApi, null);
        }

        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            return string.Empty;
        }

        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {
        }

        public static Autorizacion GetAutorizacionSGI(IHarvesterServices pHarvesterServices, ReadConfig pConfig, string pId, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            // Obtención de datos en bruto.
            Autorizacion autorizacion = new Autorizacion();
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

        public ProjectAuthorization CrearAutorizationOntology(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            ProjectAuthorization autorizacion = new ProjectAuthorization();
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
