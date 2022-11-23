namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OAuthConsumer")]
    public partial class OAuthConsumer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OAuthConsumer()
        {
            OAuthToken = new HashSet<OAuthToken>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConsumerId { get; set; }

        [Required]
        [StringLength(50)]
        public string ConsumerKey { get; set; }

        [Required]
        [StringLength(50)]
        public string ConsumerSecret { get; set; }

        [StringLength(4000)]
        public string Callback { get; set; }

        public int VerificationCodeFormat { get; set; }

        public int VerificationCodeLength { get; set; }

        public virtual ConsumerData ConsumerData { get; set; }

        public virtual UsuarioConsumer UsuarioConsumer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OAuthToken> OAuthToken { get; set; }

        
    }
}
