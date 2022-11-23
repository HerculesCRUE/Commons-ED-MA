using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("Persona")]
    public partial class Persona
    {
        public Persona()
        {
        }
        public Guid PersonaID { get; set; }

        public Guid? UsuarioID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Apellidos { get; set; }

        public short? TipoDocumentoAcreditativo { get; set; }

        [StringLength(100)]
        public string ValorDocumentoAcreditativo { get; set; }

        public byte[] Foto { get; set; }

        [StringLength(1)]
        public string Sexo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public Guid? PaisPersonalID { get; set; }

        public Guid? ProvinciaPersonalID { get; set; }

        [StringLength(255)]
        public string ProvinciaPersonal { get; set; }

        [StringLength(255)]
        public string LocalidadPersonal { get; set; }

        [StringLength(15)]
        public string CPPersonal { get; set; }

        [StringLength(1000)]
        public string DireccionPersonal { get; set; }

        [StringLength(13)]
        public string TelefonoPersonal { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string EmailTutor { get; set; }
        public Guid? EstadoCivilID { get; set; }

        public Guid? TitulacionID { get; set; }

        public short? Hijos { get; set; }

        public bool EsBuscable { get; set; }

        public bool EsBuscableExternos { get; set; }

        public bool Eliminado { get; set; }

        [StringLength(30)]
        public string CoordenadasFoto { get; set; }

        [Required]
        [StringLength(5)]
        public string Idioma { get; set; }

        public int? VersionFoto { get; set; }

        public DateTime? FechaNotificacionCorreccion { get; set; }

        public short EstadoCorreccion { get; set; }

        public DateTime? FechaAnadidaFoto { get; set; }
    }
}
