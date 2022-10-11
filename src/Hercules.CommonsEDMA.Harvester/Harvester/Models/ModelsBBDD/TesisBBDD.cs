using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Models.ModelsBBDD
{
    public class TesisBBDD
    {
        public string idGnoss { get; set; }
        public string crisIdentifier { get; set; }
        public string title { get; set; }
        public DateTime? issued { get; set; }

        public string studentNick { get; set; }
        public string studentName { get; set; }
        public string studentFirstSurname { get; set; }
        public string studentSecondSurname { get; set; }

        public string promotedByTitle { get; set; }
        public string promotedBy { get; set; }
        public string promotedByType { get; set; }
        public string promotedByTypeOther { get; set; }

        public string locality { get; set; }
        public string hasCountryName { get; set; }
        public string hasRegion { get; set; }

        public string projectCharacterType { get; set; }
        public string projectCharacterTypeOther { get; set; }

        public List<Codirector> codirector { get; set; }

        public string qualification { get; set; }
        public bool? qualityMention { get; set; }
        public DateTime? qualityMentionDate { get; set; }

        public bool? europeanDoctorate { get; set; }
        public DateTime? europeanDoctorateDate { get; set; }

        public List<string> freeTextKeyword { get; set; }
    }

    public class Codirector
    {
        public int comment { get; set; }
        public string nick { get; set; }
        public string firstName { get; set; }
        public string familyName { get; set; }
        public string secondFamilyName { get; set; }
    }
}
