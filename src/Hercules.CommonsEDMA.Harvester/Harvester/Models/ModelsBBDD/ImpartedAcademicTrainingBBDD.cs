using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Models.ModelsBBDD
{
    public class ImpartedAcademicTrainingBBDD
    {
        public string owner { get; set; } // Person
        public string cvnCode { get; set; }
        public string crisIdentifier { get; set; }
        public string title { get; set; }
        public string degreeType { get; set; } // DegreeType (Secundaria)
        public string teaches { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string promotedByTitle { get; set; }
        public string promotedBy { get; set; } // Organization
        public string promotedByType { get; set; } // OrganizationType (Secundaria)
        public string promotedByTypeOther { get; set; }
        public string center { get; set; }
        public string locality { get; set; }
        public string hasCountryName { get; set; } // Feature (Secundaria)
        public string hasRegion { get; set; } // Feature (Secundaria)
        public string teachingType { get; set; } // TeachingType (Secundaria)
        public float numberECTSHours { get; set; }
        public float frequency { get; set; }
        public string programType { get; set; } // ProgramType (Secundaria)
        public string programTypeOther { get; set; }
        public string modalityTeachingType { get; set; } // ModalityTeachingType (Secundaria)
        public string modalityTeachingTypeOther { get; set; }
        public string department { get; set; }
        public string courseType { get; set; } // CourseType (Secundaria)
        public string courseTypeOther { get; set; }
        public string course { get; set; }
        public string hoursCreditsECTSType { get; set; } // HoursCreditsECTSType (Secundaria)
        public string hasLanguage { get; set; } // Language (Secundaria)
        public string competencies { get; set; }
        public string professionalCategory { get; set; }
        public string qualification { get; set; }
        public string maxQualification { get; set; }
        public string evaluatedByTitle { get; set; } 
        public string evaluatedBy { get; set; } // Organization
        public string evaluatedByType { get; set; } // OrganizationType (Secundaria)
        public string evaluatedByTypeOther { get; set; } 
        public string evaluatedByLocality { get; set; }
        public string evaluatedByHasCountryName { get; set; } // Feature (Secundaria)
        public string evaluatedByHasRegion { get; set; } // Feature (Secundaria)
        public string financedByTitle { get; set; }
        public string financedBy { get; set; } // Organization
        public string financedByType { get; set; } // OrganizationType (Secundaria)
        public string financedByTypeOther { get; set; } 
        public string financedByHasCountryName { get; set; } // Feature (Secundaria)
        public string financedByHasRegion { get; set; } // Feature (Secundaria)
        public string callType { get; set; } // CallType (Secundaria)
        public string callTypeOther { get; set; }
        public string geographicFocus { get; set; } // EventGeographicRegion (Secundaria)
        public string geographicFocusOther { get; set; }
    }
}
