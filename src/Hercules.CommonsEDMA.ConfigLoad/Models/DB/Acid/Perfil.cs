using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("Perfil")]
    public partial class Perfil
    {
        public Perfil()
        {
        }

        public Guid PerfilID { get; set; }

        [Required]
        [StringLength(300)]
        public string NombrePerfil { get; set; }

        [StringLength(255)]
        public string NombreOrganizacion { get; set; }

        public bool Eliminado { get; set; }

        [StringLength(50)]
        public string NombreCortoOrg { get; set; }

        [StringLength(50)]
        public string NombreCortoUsu { get; set; }

        public Guid? OrganizacionID { get; set; }

        public Guid? PersonaID { get; set; }

        public bool TieneTwitter { get; set; }

        [StringLength(15)]
        public string UsuarioTwitter { get; set; }

        [StringLength(1000)]
        public string TokenTwitter { get; set; }

        [StringLength(1000)]
        public string TokenSecretoTwitter { get; set; }

        public Guid? CurriculumPerfilID { get; set; }

        public int CaducidadResSusc { get; set; }

        public Guid? CurriculumID { get; set; }
    }
}
