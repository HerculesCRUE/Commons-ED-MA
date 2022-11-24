using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid
{
    [Serializable]
    [Table("ProyectoUsuarioIdentidad")]
    public partial class ProyectoUsuarioIdentidad
    {
        [Column(Order = 0)]
        public Guid IdentidadID { get; set; }

        [Column(Order = 1)]
        public Guid UsuarioID { get; set; }

        [Column(Order = 2)]
        public Guid OrganizacionGnossID { get; set; }

        [Column(Order = 3)]
        public Guid ProyectoID { get; set; }

        public DateTime? FechaEntrada { get; set; }

        public int? Reputacion { get; set; }
    }
}
