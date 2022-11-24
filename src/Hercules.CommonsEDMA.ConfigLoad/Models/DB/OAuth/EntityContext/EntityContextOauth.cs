using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth.EntityContext
{
    public class EntityContextOauth : DbContext
    {
        private ConfigService mConfigService;
        /// <summary>
        /// Constructor internal, para obtener un objeto EntityContext, llamar al método ObtenerEntityContext del BaseAD
        /// </summary>
        public EntityContextOauth(DbContextOptions<EntityContextOauth> dbContextOptions, ConfigService configService)
            : base(dbContextOptions)
        {
            mConfigService = configService;
        }

        public virtual DbSet<ConsumerData> ConsumerData { get; set; }
        public virtual DbSet<OAuthConsumer> OAuthConsumer { get; set; }
        public virtual DbSet<OAuthToken> OAuthToken { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioConsumer> UsuarioConsumer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConsumerData>(entity =>
            {
                entity.Property(e => e.ConsumerId).ValueGeneratedNever();

                entity.HasOne(d => d.OAuthConsumer)
                    .WithOne(p => p.ConsumerData)
                    .HasForeignKey<ConsumerData>(d => d.ConsumerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsumerData_OAuthConsumer");
            });

            modelBuilder.Entity<OAuthConsumer>(entity =>
            {
                entity.HasKey(e => e.ConsumerId)
                    .HasName("PK_dbo.OAuthConsumer");
            });

            modelBuilder.Entity<OAuthToken>(entity =>
            {
                entity.HasKey(e => e.TokenId)
                    .HasName("PK_dbo.OAuthToken");

                entity.HasOne(d => d.OAuthConsumer)
                    .WithMany(p => p.OAuthToken)
                    .HasForeignKey(d => d.ConsumerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OAuthConsumer_OAuthToken");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.OAuthToken)
                    .HasForeignKey(d => d.UsuarioID)
                    .HasConstraintName("FK_OAuthToken_Usuario");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.UsuarioID).ValueGeneratedNever();
            });

            modelBuilder.Entity<UsuarioConsumer>(entity =>
            {
                entity.HasKey(e => e.ConsumerId)
                    .HasName("PK_Usuario_Consumer");

                entity.Property(e => e.ConsumerId).ValueGeneratedNever();

                entity.HasOne(d => d.OAuthConsumer)
                    .WithOne(p => p.UsuarioConsumer)
                    .HasForeignKey<UsuarioConsumer>(d => d.ConsumerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Consumer_OAuthConsumer");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.UsuarioConsumer)
                    .HasForeignKey(d => d.UsuarioID)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario_Consumer_Usuario");
            });
        }

    }
}
