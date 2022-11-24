using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("ProyectoRolUsuario")]
    public partial class ProyectoRolUsuario
    {
        public ProyectoRolUsuario()
        {
        }

        [Column(Order = 0)]
        public Guid OrganizacionGnossID { get; set; }

        [Column(Order = 1)]
        public Guid ProyectoID { get; set; }

        [Column(Order = 2)]
        public Guid UsuarioID { get; set; }

        [Required]
        [StringLength(16)]
        public string RolPermitido { get; set; }

        [StringLength(16)]
        public string RolDenegado { get; set; }

        public bool EstaBloqueado { get; set; }
    }
}
