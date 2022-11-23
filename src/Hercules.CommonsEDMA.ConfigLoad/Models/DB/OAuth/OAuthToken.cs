namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OAuthToken")]
    public partial class OAuthToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenId { get; set; }

        [Required]
        [StringLength(50)]
        public string Token { get; set; }

        [Required]
        [StringLength(50)]
        public string TokenSecret { get; set; }

        public int State { get; set; }

        public DateTime IssueDate { get; set; }

        public int ConsumerId { get; set; }

        public Guid? UsuarioID { get; set; }

        public string Scope { get; set; }

        [StringLength(4000)]
        public string RequestTokenVerifier { get; set; }

        [StringLength(4000)]
        public string RequestTokenCallback { get; set; }

        [StringLength(4000)]
        public string ConsumerVersion { get; set; }

        public virtual OAuthConsumer OAuthConsumer { get; set; }

        public virtual Usuario Usuario { get; set; }     
    }
}
