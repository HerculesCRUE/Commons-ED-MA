using System;
using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Offer
{
    public class Offer
    {
        public string entityID { get; set; }
        public string name { get; set; }
        public string creatorId { get; set; }
        public string creatorName { get; set; }
        public string state { get; set; }
        public string date { get; set; }
        public List<string> tags { get; set; }
        public List<string> groups { get; set; }
        public Dictionary<string, string> areaProcedencia { get; set; }
        public Dictionary<string, string> sectorAplicacion { get; set; }
        public Dictionary<string, string> lineResearchs { get; set; }
        public Dictionary<Guid, UsersOffer> researchers { get; set; }
        public string matureState { get; set; }
        public string framingSector { get; set; }
        public FieldsHtmlOffer objectFieldsHtml { get; set; }
        public Dictionary<Guid, ProjectsOffer> projects { get; set; }
        public Dictionary<Guid, DocumentsOffer> documents { get; set; }
        public Dictionary<Guid, PIIOffer> pii { get; set; }
    }
    public class FieldsHtmlOffer
    {
        public string descripcion { get; set; }
        public string aplicaciones { get; set; }
        public string destinatarios { get; set; }
        public string ventajasBeneficios { get; set; }
        public string origen { get; set; }
        public string innovacion { get; set; }
        public string socios { get; set; }
        public string colaboracion { get; set; }
        public string observaciones { get; set; }
    }

    public class ProjectsOffer
{
        public string id { get; set; }
        public Guid shortId { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public string description { get; set; }
        public List<string> researchers { get; set; }
        public string[] dates { get; set; }
    }
    public class DocumentsOffer
    {
        public string id { get; set; }
        public Guid shortId { get; set; }
        public string name { get; set; }
        public string info { get; set; }
        public string description { get; set; }
        public string[] dates { get; set; }
        public List<string> researchers { get; set; }
    }
    public class PIIOffer
    {
        public string id { get; set; }
        public Guid shortId { get; set; }
        public string name { get; set; }
        public string organizacion { get; set; }
        public string description { get; set; }
        public string fecha { get; set; }
        public List<string> researchers { get; set; }
        public List<string> researchersIds { get; set; }
    }
    public class UsersOffer
    {
        public string id { get; set; }
        public Guid shortId { get; set; }
        public string name { get; set; }
        public int numPublicaciones { get; set; }
        public int numPublicacionesTotal { get; set; }
        public int ipNumber { get; set; }
        public string organization { get; set; }
        public string hasPosition { get; set; }
        public string departamento { get; set; }
        public List<string> groups { get; set; }
    }

}
