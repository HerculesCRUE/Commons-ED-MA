using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("HistoricoProyectoUsuario")]
    public partial class HistoricoProyectoUsuario
    {
        [Column(Order = 0)]
        public Guid UsuarioID { get; set; }

        [Column(Order = 1)]
        public Guid OrganizacionGnossID { get; set; }

        [Column(Order = 2)]
        public Guid ProyectoID { get; set; }

        [Column(Order = 3)]
        public Guid IdentidadID { get; set; }

        [Column(Order = 4)]
        public DateTime FechaEntrada { get; set; }

        public DateTime? FechaSalida { get; set; }
    }
}
