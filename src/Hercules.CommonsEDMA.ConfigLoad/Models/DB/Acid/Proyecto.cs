using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("Proyecto")]
    public partial class Proyecto
    {
        public Proyecto()
        {
        }

        [Column(Order = 0)]
        public Guid OrganizacionID { get; set; }

        [Column(Order = 1)]
        public Guid ProyectoID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public short TipoProyecto { get; set; }

        public short TipoAcceso { get; set; }

        public int? NumeroRecursos { get; set; }

        public int? NumeroPreguntas { get; set; }

        public int? NumeroDebates { get; set; }

        public int? NumeroMiembros { get; set; }

        public int? NumeroOrgRegistradas { get; set; }

        public int? NumeroArticulos { get; set; }

        public int? NumeroDafos { get; set; }

        public int? NumeroForos { get; set; }

        public Guid? ProyectoSuperiorID { get; set; }

        public bool EsProyectoDestacado { get; set; }

        [StringLength(250)]
        public string URLPropia { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreCorto { get; set; }

        public short Estado { get; set; }

        public bool TieneTwitter { get; set; }

        [StringLength(50)]
        public string TagTwitter { get; set; }

        [StringLength(15)]
        public string UsuarioTwitter { get; set; }

        [StringLength(1000)]
        public string TokenTwitter { get; set; }

        [StringLength(1000)]
        public string TokenSecretoTwitter { get; set; }

        public bool EnviarTwitterComentario { get; set; }

        public bool EnviarTwitterNuevaCat { get; set; }

        public bool EnviarTwitterNuevoAdmin { get; set; }

        public bool EnviarTwitterNuevaPolitCert { get; set; }

        public bool EnviarTwitterNuevoTipoDoc { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TablaBaseProyectoID { get; set; }

        public Guid? ProcesoVinculadoID { get; set; }

        public string Tags { get; set; }

        public bool TagTwitterGnoss { get; set; }

        [StringLength(1000)]
        public string NombrePresentacion { get; set; }

        public virtual ICollection<AdministradorProyecto> AdministradorProyecto { get; set; }
    }
}
