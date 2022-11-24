namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth
{
    using Microsoft.EntityFrameworkCore;

    public partial class EntityModelOauth : DbContext
    {
        public EntityModelOauth()
            : base()
        {
        }

        public virtual DbSet<OAuthConsumer> OAuthConsumer { get; set; }
        public virtual DbSet<OAuthToken> OAuthToken { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioConsumer> UsuarioConsumer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<OAuthConsumer>()
                .HasOne(e => e.UsuarioConsumer)
                .WithOne(e => e.OAuthConsumer).IsRequired();

            modelBuilder.Entity<OAuthConsumer>()
                .HasMany(e => e.OAuthToken)
                .WithOne(e => e.OAuthConsumer).IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>()
                .HasMany(e => e.UsuarioConsumer)
                .WithOne(e => e.Usuario).IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
