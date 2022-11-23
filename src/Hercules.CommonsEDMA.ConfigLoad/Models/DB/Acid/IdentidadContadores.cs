using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("IdentidadContadores")]
    public partial class IdentidadContadores
    {
        [Key]
        public Guid IdentidadID { get; set; }

        public int NumeroVisitas { get; set; }

        public int NumeroDescargas { get; set; }
    }
}
