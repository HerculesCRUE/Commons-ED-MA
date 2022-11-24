using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Table("Identidad")]
    [Serializable]
    public partial class Identidad
    {
        public Identidad()
        {
        }
        public Guid IdentidadID { get; set; }

        public Guid PerfilID { get; set; }

        public Guid OrganizacionID { get; set; }

        public Guid ProyectoID { get; set; }

        public Guid? CurriculumID { get; set; }

        public DateTime FechaAlta { get; set; }

        public DateTime? FechaBaja { get; set; }

        public int NumConnexiones { get; set; }

        public short Tipo { get; set; }

        [Required]
        [StringLength(300)]
        public string NombreCortoIdentidad { get; set; }

        public DateTime? FechaExpulsion { get; set; }

        public bool RecibirNewsLetter { get; set; }

        public double? Rank { get; set; }

        public bool MostrarBienvenida { get; set; }

        public int DiasUltActualizacion { get; set; }

        public double ValorAbsoluto { get; set; }

        public bool ActivoEnComunidad { get; set; }

        public bool ActualizaHome { get; set; }

        [StringLength(200)]
        public string Foto { get; set; }
    }
}
