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
    [Table("VistaVirtualPersonalizacion")]
    public partial class VistaVirtualPersonalizacion
    {
        [Key]
        public Guid PersonalizacionID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VistaVirtualProyecto> VistaVirtualProyecto { get; set; }
    }
}
