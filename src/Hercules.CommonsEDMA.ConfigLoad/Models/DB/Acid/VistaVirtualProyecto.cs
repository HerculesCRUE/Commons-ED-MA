using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{

    [Serializable]
    [Table("VistaVirtualProyecto")]
    public partial class VistaVirtualProyecto
    {
        [Column(Order = 0)]
        public Guid OrganizacionID { get; set; }

        [Column(Order = 1)]
        public Guid ProyectoID { get; set; }

        [Column(Order = 2)]
        public Guid PersonalizacionID { get; set; }

        public virtual VistaVirtualPersonalizacion VistaVirtualPersonalizacion { get; set; }
    }
}
