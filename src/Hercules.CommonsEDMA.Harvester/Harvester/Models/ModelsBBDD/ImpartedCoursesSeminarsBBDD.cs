using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Models.ModelsBBDD
{
    public class ImpartedCoursesSeminarsBBDD
    {
        public string crisIdentifiers { get; set; }
        public string title { get; set; }

        public string eventType { get; set; }
        public string eventTypeOther { get; set; }

        public string promotedByTitle { get; set; }
        public string promotedBy { get; set; }
        public string promotedByType { get; set; }
        public string promotedByTypeOther { get; set; }

        public DateTime? start { get; set; }
        public float? durationHours { get; set; }

        public string locality { get; set; }
        public string hasCountryName { get; set; }
        public string hasRegion { get; set; }

        public string goals { get; set; }
        public string hasLanguage { get; set; }
        public string isbn { get; set; }
        public string issn { get; set; }
        public bool correspondingAuthor { get; set; }
        public string doi { get; set; }
        public string handle { get; set; }
        public string pmid { get; set; }
        public List<Document> identifier { get; set; }
        public string targetProfile { get; set; }
        public string participationType { get; set; }
        public string participationTypeOther { get; set; }
    }

    public class Document
    {
        public string topic { get; set; }
        public string title { get; set; }
    }
}
