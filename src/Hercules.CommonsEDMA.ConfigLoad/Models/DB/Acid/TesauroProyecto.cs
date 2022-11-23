using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("TesauroProyecto")]
    public partial class TesauroProyecto
    {
        [Column(Order = 0)]
        public Guid TesauroID { get; set; }

        [Column(Order = 1)]
        public Guid OrganizacionID { get; set; }

        [Column(Order = 2)]
        public Guid ProyectoID { get; set; }

        [StringLength(50)]
        public string IdiomaDefecto { get; set; }
    }
}
