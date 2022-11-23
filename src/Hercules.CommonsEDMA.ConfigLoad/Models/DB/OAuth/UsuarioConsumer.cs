namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UsuarioConsumer")]
    public partial class UsuarioConsumer
    {
        public Guid UsuarioID { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConsumerId { get; set; }

        public Guid ProyectoID { get; set; }

        public virtual OAuthConsumer OAuthConsumer { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
