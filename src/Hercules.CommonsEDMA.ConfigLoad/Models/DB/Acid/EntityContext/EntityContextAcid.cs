using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid.EntityContext
{
    public partial class EntityContextAcid : DbContext
    {
        private ConfigService mConfigService;

        /// <summary>
        /// Constructor internal, para obtener un objeto EntityContext, llamar al m�todo ObtenerEntityContext del BaseAD
        /// </summary>
        public EntityContextAcid(DbContextOptions<EntityContextAcid> dbContextOptions, ConfigService configService)
            : base(dbContextOptions)
        {
            mConfigService = configService;
        }
        public virtual DbSet<BaseRecursosProyecto> BaseRecursosProyecto { get; set; }
        public virtual DbSet<BaseRecursos> BaseRecursos { get; set; }
        public virtual DbSet<TesauroProyecto> TesauroProyecto { get; set; }
        public virtual DbSet<Tesauro> Tesauro { get; set; }
        public virtual DbSet<AdministradorProyecto> AdministradorProyecto { get; set; }
        public virtual DbSet<HistoricoProyectoUsuario> HistoricoProyectoUsuario { get; set; }
        public virtual DbSet<ProyectoUsuarioIdentidad> ProyectoUsuarioIdentidad { get; set; }

        public virtual DbSet<ProyectoRolUsuario> ProyectoRolUsuario { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }

        public virtual DbSet<Identidad> Identidad { get; set; }
        public virtual DbSet<IdentidadContadores> IdentidadContadores { get; set; }
        public virtual DbSet<Perfil> Perfil { get; set; }

        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Proyecto> Proyecto { get; set; }
        public virtual DbSet<ParametroGeneral> ParametroGeneral { get; set; }
        public virtual DbSet<ParametroProyecto> ParametroProyecto { get; set; }
        public virtual DbSet<VistaVirtualPersonalizacion> VistaVirtualPersonalizacion { get; set; }
        public virtual DbSet<VistaVirtualProyecto> VistaVirtualProyecto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ParametroGeneral>()
                .HasKey(c => new { c.OrganizacionID, c.ProyectoID });

            modelBuilder.Entity<ParametroGeneral>()
                .Property(e => e.CoordenadasHome)
                .IsFixedLength();

            modelBuilder.Entity<ParametroGeneral>()
                .Property(e => e.CoordenadasMosaico)
                .IsFixedLength();

            modelBuilder.Entity<ParametroGeneral>()
                .Property(e => e.CoordenadasSup)
                .IsFixedLength();

            modelBuilder.Entity<ParametroGeneral>()
                .Property(e => e.EnlaceContactoPiePagina)
                .IsUnicode(false);

            modelBuilder.Entity<ParametroProyecto>()
                .HasKey(c => new { c.OrganizacionID, c.ProyectoID, c.Parametro });

            modelBuilder.Entity<VistaVirtualProyecto>()
             .HasKey(c => new { c.OrganizacionID, c.ProyectoID, c.PersonalizacionID });

            modelBuilder.Entity<Proyecto>()
                .HasKey(c => new { c.OrganizacionID, c.ProyectoID });

            modelBuilder.Entity<Proyecto>()
                .HasMany(e => e.AdministradorProyecto)
                .WithOne(e => e.Proyecto)
                .IsRequired()
                .HasForeignKey(e => new { e.OrganizacionID, e.ProyectoID });

            modelBuilder.Entity<VistaVirtualPersonalizacion>()
                .HasMany(e => e.VistaVirtualProyecto)
                .WithOne(e => e.VistaVirtualPersonalizacion)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AdministradorProyecto>()
                .HasKey(c => new { c.OrganizacionID, c.ProyectoID, c.UsuarioID, c.Tipo });

            modelBuilder.Entity<BaseRecursosProyecto>()
                .HasKey(c => new { c.BaseRecursosID, c.OrganizacionID, c.ProyectoID });

            modelBuilder.Entity<HistoricoProyectoUsuario>()
                .HasKey(c => new { c.UsuarioID, c.OrganizacionGnossID, c.ProyectoID, c.IdentidadID, c.FechaEntrada });

            modelBuilder.Entity<ProyectoRolUsuario>()
                .HasKey(c => new { c.OrganizacionGnossID, c.ProyectoID, c.UsuarioID });

            modelBuilder.Entity<ProyectoUsuarioIdentidad>()
                .HasKey(c => new { c.IdentidadID, c.UsuarioID, c.OrganizacionGnossID, c.ProyectoID });

            modelBuilder.Entity<TesauroProyecto>()
                .HasKey(c => new { c.TesauroID, c.OrganizacionID, c.ProyectoID });
        }
    }
}
