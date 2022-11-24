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
    [Table("ParametroGeneral")]
    public partial class ParametroGeneral
    {
        [Column(Order = 0)]
        public Guid OrganizacionID { get; set; }

        [Column(Order = 1)]
        public Guid ProyectoID { get; set; }

        public float? UmbralSuficienciaEnMejora { get; set; }

        public float? DesviacionAdmitidaEnEvalua { get; set; }

        public short? MetaAutomatPropietarioPro { get; set; }

        [StringLength(4000)]
        public string AvisoLegal { get; set; }

        public bool WikiDisponible { get; set; }

        public bool BaseRecursosDisponible { get; set; }

        public bool CompartirRecursosPermitido { get; set; }

        public bool InvitacionesDisponibles { get; set; }

        public bool ServicioSuscripcionDisp { get; set; }

        public bool BlogsDisponibles { get; set; }

        public bool ForosDisponibles { get; set; }

        public bool EncuestasDisponibles { get; set; }

        public bool VotacionesDisponibles { get; set; }

        public bool ComentariosDisponibles { get; set; }

        public bool PreguntasDisponibles { get; set; }

        public bool DebatesDisponibles { get; set; }

        public bool BrightcoveDisponible { get; set; }

        [StringLength(100)]
        public string BrightcoveTokenWrite { get; set; }

        [StringLength(100)]
        public string BrightcoveTokenRead { get; set; }

        [StringLength(100)]
        public string BrightcoveReproductorID { get; set; }

        public byte[] LogoProyecto { get; set; }

        public string MensajeBienvenida { get; set; }

        public bool EntidadRevisadaObligatoria { get; set; }

        public float? UmbralDetPropietariosProc { get; set; }

        public float? UmbralDetPropietariosObj { get; set; }

        public float? UmbralDetPropietariosGF { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreDebilidadDafoProc { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreAmenazaDafoProc { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreFortalezaDafoProc { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreOportunidadDafoProc { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreDebilidadDafoObj { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreAmenazaDafoObj { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreFortalezaDafoObj { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreOportunidadDafoObj { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreDebilidadDafoGF { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreAmenazaDafoGF { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreFortalezaDafoGF { get; set; }

        [Required]
        [StringLength(1000)]
        public string NombreOportunidadDafoGF { get; set; }

        public byte[] ImagenHome { get; set; }

        [Required]
        [StringLength(255)]
        public string NombreImagenPeque { get; set; }

        public byte[] ImagenPersonalizadaPeque { get; set; }

        public bool PermitirRevisionManualPro { get; set; }

        public bool PermitirRevisionManualGF { get; set; }

        public bool PermitirRevisionManualObj { get; set; }

        public bool PermitirRevisionManualComp { get; set; }

        [StringLength(100)]
        public string RutaTema { get; set; }

        [StringLength(100)]
        public string RutaImagenesTema { get; set; }

        public bool PermitirCertificacionRec { get; set; }

        public string PoliticaCertificacion { get; set; }

        [StringLength(30)]
        public string CoordenadasHome { get; set; }

        public byte[] ImagenHomeGrande { get; set; }

        public bool DafoDisponible { get; set; }

        public bool PlantillaDisponible { get; set; }

        [StringLength(15)]
        public string CodigoGoogleAnalytics { get; set; }

        public bool VerVotaciones { get; set; }

        public int? VersionFotoImagenHomeGrande { get; set; }

        public int? VersionFotoImagenFondo { get; set; }

        public bool ClausulasRegistro { get; set; }

        [StringLength(10)]
        public string LicenciaPorDefecto { get; set; }

        public string MensajeLicenciaPorDefecto { get; set; }

        [StringLength(100)]
        public string BrightcoveFTP { get; set; }

        [StringLength(100)]
        public string BrightcoveFTPUser { get; set; }

        [StringLength(100)]
        public string BrightcoveFTPPass { get; set; }

        [StringLength(100)]
        public string BrightcovePublisherID { get; set; }

        public int? VersionCSS { get; set; }

        public int? VersionJS { get; set; }

        public bool OcultarPersonalizacion { get; set; }

        public string PestanyasDocSemanticos { get; set; }

        public bool PestanyaRecursosVisible { get; set; }

        public string ScriptBusqueda { get; set; }

        public bool ImagenRelacionadosMini { get; set; }

        [StringLength(30)]
        public string CoordenadasMosaico { get; set; }

        [StringLength(30)]
        public string CoordenadasSup { get; set; }

        public int? VersionFotoImagenMosaicoGrande { get; set; }

        public int? VersionFotoImagenSupGrande { get; set; }

        public bool EsBeta { get; set; }

        [StringLength(1000)]
        public string ScriptGoogleAnalytics { get; set; }

        public short NumeroRecursosRelacionados { get; set; }

        public bool GadgetsPieDisponibles { get; set; }

        public bool GadgetsCabeceraDisponibles { get; set; }

        public bool BiosCortas { get; set; }

        public bool RssDisponibles { get; set; }

        public bool RdfDisponibles { get; set; }

        public bool RegDidactalia { get; set; }

        public bool HomeVisible { get; set; }

        public bool CargasMasivasDisponibles { get; set; }

        public bool ComunidadGNOSS { get; set; }

        public bool IdiomasDisponibles { get; set; }

        [StringLength(50)]
        public string IdiomaDefecto { get; set; }

        public bool SupervisoresAdminGrupos { get; set; }

        public bool FechaNacimientoObligatoria { get; set; }

        public bool PrivacidadObligatoria { get; set; }

        public bool EventosDisponibles { get; set; }

        public bool SolicitarCoockieLogin { get; set; }

        [StringLength(100)]
        public string Copyright { get; set; }

        public bool CMSDisponible { get; set; }

        public int? VersionCSSWidget { get; set; }

        public bool InvitacionesPorContactoDisponibles { get; set; }

        public bool PermitirUsuNoLoginDescargDoc { get; set; }

        public short? TipoCabecera { get; set; }

        public short? TipoFichaRecurso { get; set; }

        public bool AvisoCookie { get; set; }

        public bool MostrarPersonasEnCatalogo { get; set; }

        public bool EnvioMensajesPermitido { get; set; }

        [StringLength(45)]
        public string EnlaceContactoPiePagina { get; set; }

        public bool TieneSitemapComunidad { get; set; }

        [StringLength(200)]
        public string UrlServicioFichas { get; set; }

        public bool PermitirVotacionesNegativas { get; set; }

        public bool MostrarAccionesEnListados { get; set; }

        public bool PalcoActivado { get; set; }

        public bool PermitirRecursosPrivados { get; set; }

        public short PlataformaVideoDisponible { get; set; }

        public string TOPIDCuenta { get; set; }

        public string TOPIDPlayer { get; set; }

        public string TOPPublisherID { get; set; }

        public string TOPFTPUser { get; set; }

        public string TOPFTPPass { get; set; }

        [StringLength(200)]
        public string UrlMappingCategorias { get; set; }

        public string AlgoritmoPersonasRecomendadas { get; set; }

        public string PropsMapaPerYOrg { get; set; }

        public bool ChatDisponible { get; set; }

        public short? VersionCSSAdmin { get; set; }

        public short? VersionJSAdmin { get; set; }
    }
}
