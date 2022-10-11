using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.OrganicStructure
{
    public class Clasificacion
    {
        public string Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string PadreId { get; set; }
    }
}
