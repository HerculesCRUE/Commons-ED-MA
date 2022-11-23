namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Usuario")]
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            OAuthToken = new HashSet<OAuthToken>();
            UsuarioConsumer = new HashSet<UsuarioConsumer>();
        }
        [Key]
        public Guid UsuarioID { get; set; }

        [Required]
        [StringLength(12)]
        public string Login { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OAuthToken> OAuthToken { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsuarioConsumer> UsuarioConsumer { get; set; }
    }
}
