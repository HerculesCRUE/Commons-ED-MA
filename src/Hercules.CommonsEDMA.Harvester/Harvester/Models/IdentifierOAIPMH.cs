using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Models
{
    public class IdentifierOAIPMH
    {
        public string Identifier { get; set; }
        public DateTime Date { get; set; }
        public string Set { get; set; }
        public bool Deleted { get; set; }
    }
}
