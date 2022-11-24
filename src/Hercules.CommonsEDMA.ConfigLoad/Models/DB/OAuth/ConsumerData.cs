namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ConsumerData")]
    public partial class ConsumerData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ConsumerId { get; set; }

        [Required]
        [StringLength(2000)]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        [StringLength(2000)]
        public string UrlOrigen { get; set; }

        public DateTime FechaAlta { get; set; }

        public virtual OAuthConsumer OAuthConsumer { get; set; }
    }
}
