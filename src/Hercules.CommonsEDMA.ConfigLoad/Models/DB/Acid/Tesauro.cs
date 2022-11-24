using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("Tesauro")]
    public partial class Tesauro
    {
        public Tesauro()
        {
        }

        public Guid TesauroID { get; set; }
    }
}
