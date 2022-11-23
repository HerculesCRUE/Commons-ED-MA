using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Table("BaseRecursosProyecto")]
    [Serializable]
    public partial class BaseRecursosProyecto
    {
        [Column(Order = 0)]
        public Guid BaseRecursosID { get; set; }

        [Column(Order = 1)]
        public Guid OrganizacionID { get; set; }

        [Column(Order = 2)]
        public Guid ProyectoID { get; set; }
    }
}
