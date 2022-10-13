using System;
using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.ROsLinked
{

    public class ROLinked
    {
        // ?s ?title ?abstract ?issued ?type ?roType ?roTypeTitle ?origin group_concat(distinct ?clKnowledgeArea;separator=',') as ?gckarea"

        public string entityID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string fecha { get; set; }
        public string roType { get; set; }
        public string type { get; set; }
        public string roTypeTitle { get; set; }
        public bool origin { get; set; }
        public bool isValidated { get; set; }
        public List<string> terms { get; set; }
        public List<string> idsGnoss { get; set; }
        public string url { get; set; }
    }

    public enum TypeRO
    {
        RO,
        Document
    }
    public class ResTypeRo
    {
        public TypeRO typeRO { get; set; }
        public string type { get; set; }
        public string longType { get; set; }
    }
}
