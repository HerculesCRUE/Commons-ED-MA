using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
        }

        public Guid UsuarioID { get; set; }

        [Required]
        [StringLength(12)]
        public string Login { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        public bool? EstaBloqueado { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreCorto { get; set; }

        public short? Version { get; set; }

        public DateTime? FechaCambioPassword { get; set; }

        public short Validado { get; set; }
    }
}
