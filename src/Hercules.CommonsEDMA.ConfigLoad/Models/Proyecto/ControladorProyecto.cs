using Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid.EntityContext;
using Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth.EntityContext;
using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.Proyecto
{
    public static class ControladorProyecto
    {
        public static DB.Acid.Proyecto CrearNuevoProyecto(EntityContextAcid entityContextAcid, ConfigService configService)
        {
            if (entityContextAcid.Proyecto.FirstOrDefault(x => x.NombreCorto == configService.ObtenerNombreCortoComunidad()) != null)
            {
                Console.WriteLine("El proyecto ya existe");
                return entityContextAcid.Proyecto.FirstOrDefault(x => x.NombreCorto == configService.ObtenerNombreCortoComunidad());
            }

            //Creamos Fila Usuario
            DB.Acid.Usuario filaUsuario = entityContextAcid.Usuario.FirstOrDefault(x => x.Login == configService.ObtenerLoginAdmin());
            if (filaUsuario == null)
            {
                Console.WriteLine("El usuario no existe");
                return null;
            }

            Console.WriteLine("Creamos el proyecto...");

            //Creamos Fila Proyecto
            DB.Acid.Proyecto filaProyecto = AnyadirProyecto(entityContextAcid, configService);

            //Creamos Fila Persona
            DB.Acid.Persona filaPersona = entityContextAcid.Persona.FirstOrDefault(x => x.UsuarioID == filaUsuario.UsuarioID);

            //Creamos Fila Perfil
            DB.Acid.Perfil filaPerfil = entityContextAcid.Perfil.FirstOrDefault(x => x.PersonaID == filaPersona.PersonaID);

            //Creamos Filas para el administrador del proyecto
            #region Administrador del proyecto

            //Creamos la fila ProyectoRolusuario
            DB.Acid.ProyectoRolUsuario filaProyectoRolUsuario = AnyadirProyectoRolUsuario(filaProyecto, filaUsuario, entityContextAcid, configService);

            //Creamos la fila Identidad
            DB.Acid.Identidad filaIdentidad = AnyadirIdentidad(filaProyecto, filaPerfil, entityContextAcid, configService);

            //Creamos la fila ProyectoUsuarioIdentidad
            DB.Acid.ProyectoUsuarioIdentidad filaProyectoUsuarioIdentidad = AnyadirProyectoUsuarioIdentidad(filaProyecto, filaUsuario, filaIdentidad, entityContextAcid, configService);

            //Creamos la fila HistoricoProyectoUsuario
            DB.Acid.HistoricoProyectoUsuario filaHistoricoProyectoUsuario = AnyadirHistoricoProyectoUsuario(filaProyecto, filaUsuario, filaIdentidad, entityContextAcid, configService);

            //Creamos la fila AdministradorProyecto
            DB.Acid.AdministradorProyecto filaAdministradorProyecto = AnyadirAdministradorProyecto(filaProyecto, filaUsuario, entityContextAcid, configService);

            #endregion

            //Creamos la fila ParametroGeneral
            DB.Acid.ParametroGeneral filaParametroGeneral = AnyadirParametroGeneral(filaProyecto, filaUsuario, entityContextAcid, configService);

            //Creamos la fila Tesauro
            DB.Acid.Tesauro filaTesauro = new DB.Acid.Tesauro() { TesauroID = Guid.NewGuid() };
            entityContextAcid.Tesauro.Add(filaTesauro);

            //Creamos la fila TesauroProyecto
            DB.Acid.TesauroProyecto filaTesauroProyecto = AnyadirTesauroProyecto(filaProyecto, filaTesauro, entityContextAcid, configService);

            //Creamos la fila BaseRecursos
            DB.Acid.BaseRecursos filaBaseRecursos = new DB.Acid.BaseRecursos() { BaseRecursosID = Guid.NewGuid() };
            entityContextAcid.BaseRecursos.Add(filaBaseRecursos);

            //Creamos la fila BaseRecursosProyecto
            DB.Acid.BaseRecursosProyecto filaBaseRecursosProyecto = AnyadirBaseRecursosProyecto(filaProyecto, filaBaseRecursos, entityContextAcid, configService);

            //Creamos las filas ParametroProyecto
            DB.Acid.ParametroProyecto filaParametroProyectoAdministracionSemanticaPermitido = new DB.Acid.ParametroProyecto() { OrganizacionID = filaProyecto.OrganizacionID, ProyectoID = filaProyecto.ProyectoID, Parametro = "AdministracionSemanticaPermitido", Valor = "1" };
            DB.Acid.ParametroProyecto filaParametroProyectoAdministracionPaginasPermitido = new DB.Acid.ParametroProyecto() { OrganizacionID = filaProyecto.OrganizacionID, ProyectoID = filaProyecto.ProyectoID, Parametro = "AdministracionPaginasPermitido", Valor = "1" };
            DB.Acid.ParametroProyecto filaParametroProyectoAdministracionVistasPermitido = new DB.Acid.ParametroProyecto() { OrganizacionID = filaProyecto.OrganizacionID, ProyectoID = filaProyecto.ProyectoID, Parametro = "AdministracionVistasPermitido", Valor = "1" };
            DB.Acid.ParametroProyecto filaParametroProyectoAdministracionDesarrolladoresPermitido = new DB.Acid.ParametroProyecto() { OrganizacionID = filaProyecto.OrganizacionID, ProyectoID = filaProyecto.ProyectoID, Parametro = "AdministracionDesarrolladoresPermitido", Valor = "1" };
            DB.Acid.ParametroProyecto filaParametroProyectoSinNombreCortoEnURL = new DB.Acid.ParametroProyecto() { OrganizacionID = filaProyecto.OrganizacionID, ProyectoID = filaProyecto.ProyectoID, Parametro = "ProyectoSinNombreCortoEnURL", Valor = "1" };
            entityContextAcid.ParametroProyecto.Add(filaParametroProyectoAdministracionSemanticaPermitido);
            entityContextAcid.ParametroProyecto.Add(filaParametroProyectoAdministracionPaginasPermitido);
            entityContextAcid.ParametroProyecto.Add(filaParametroProyectoAdministracionVistasPermitido);
            entityContextAcid.ParametroProyecto.Add(filaParametroProyectoAdministracionDesarrolladoresPermitido);
            entityContextAcid.ParametroProyecto.Add(filaParametroProyectoSinNombreCortoEnURL);

            //Cramos la fila VistaVirtualPersonalizacion
            DB.Acid.VistaVirtualPersonalizacion filaVistaVirtualPersonalizacion = new DB.Acid.VistaVirtualPersonalizacion() { PersonalizacionID=Guid.NewGuid()};
            entityContextAcid.VistaVirtualPersonalizacion.Add(filaVistaVirtualPersonalizacion);

            //Cramos la fila VistaVirtualProyecto
            DB.Acid.VistaVirtualProyecto filaVistaVirtualProyecto = new DB.Acid.VistaVirtualProyecto();
            filaVistaVirtualProyecto.PersonalizacionID = filaVistaVirtualPersonalizacion.PersonalizacionID;
            filaVistaVirtualProyecto.ProyectoID = filaProyecto.ProyectoID;
            filaVistaVirtualProyecto.OrganizacionID = filaProyecto.OrganizacionID;
            entityContextAcid.VistaVirtualProyecto.Add(filaVistaVirtualProyecto);

            entityContextAcid.SaveChanges();

            Console.WriteLine("Ya se ha creado el proyecto");
            return filaProyecto;
        }


        private static DB.Acid.Proyecto AnyadirProyecto(EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.Proyecto filaProyecto = new DB.Acid.Proyecto();
            filaProyecto.ProyectoID = new Guid("b836078b-78a0-4939-b809-3f2ccf4e5c01");
            filaProyecto.OrganizacionID = new Guid("11111111-1111-1111-1111-111111111111");
            filaProyecto.Nombre = configService.ObtenerNombreCortoComunidad();
            filaProyecto.NombreCorto = configService.ObtenerNombreCortoComunidad();
            filaProyecto.Descripcion = string.Empty;
            filaProyecto.Estado = 3;
            filaProyecto.EsProyectoDestacado = false;
            filaProyecto.FechaFin = null;
            filaProyecto.FechaInicio = DateTime.UtcNow;
            filaProyecto.NumeroArticulos = 0;
            filaProyecto.NumeroDafos = 0;
            filaProyecto.NumeroForos = 0;
            filaProyecto.NumeroMiembros = 0;
            filaProyecto.NumeroOrgRegistradas = 0;
            filaProyecto.NumeroRecursos = 0;
            filaProyecto.NumeroPreguntas = 0;
            filaProyecto.NumeroDebates = 0;
            filaProyecto.TipoAcceso = 0;
            filaProyecto.TipoProyecto = 1;
            filaProyecto.TieneTwitter = false;
            filaProyecto.EnviarTwitterComentario = false;
            filaProyecto.EnviarTwitterNuevaCat = false;
            filaProyecto.EnviarTwitterNuevaPolitCert = false;
            filaProyecto.EnviarTwitterNuevoAdmin = false;
            filaProyecto.EnviarTwitterNuevoTipoDoc = false;
            filaProyecto.TagTwitterGnoss = true;
            filaProyecto.Tags = "hercules";
            filaProyecto.URLPropia = configService.ObtenerUrlPropia();
            entityContextAcid.Proyecto.Add(filaProyecto);
            return filaProyecto;

        }

        private static DB.Acid.ProyectoRolUsuario AnyadirProyectoRolUsuario(DB.Acid.Proyecto filaProyecto, DB.Acid.Usuario filaUsuario, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.ProyectoRolUsuario proyectoRolUsuario = new DB.Acid.ProyectoRolUsuario();
            proyectoRolUsuario.OrganizacionGnossID = filaProyecto.OrganizacionID;
            proyectoRolUsuario.ProyectoID = filaProyecto.ProyectoID;
            proyectoRolUsuario.UsuarioID = filaUsuario.UsuarioID;
            proyectoRolUsuario.RolPermitido = "FFFFFFFFFFFFFFFF";
            proyectoRolUsuario.RolDenegado = "0000000000000000";
            proyectoRolUsuario.EstaBloqueado = false;
            entityContextAcid.ProyectoRolUsuario.Add(proyectoRolUsuario);
            return proyectoRolUsuario;
        }
        private static DB.Acid.Identidad AnyadirIdentidad(DB.Acid.Proyecto filaProyecto, DB.Acid.Perfil filaPerfil, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.Identidad filaIdentidad = new DB.Acid.Identidad();
            filaIdentidad.IdentidadID = Guid.NewGuid();
            filaIdentidad.FechaAlta = DateTime.UtcNow;
            filaIdentidad.FechaBaja = null;
            filaIdentidad.OrganizacionID = filaProyecto.OrganizacionID;
            filaIdentidad.ProyectoID = filaProyecto.ProyectoID;
            filaIdentidad.PerfilID = filaPerfil.PerfilID;
            filaIdentidad.NumConnexiones = 0;
            filaIdentidad.RecibirNewsLetter = true;
            filaIdentidad.MostrarBienvenida = true;
            filaIdentidad.DiasUltActualizacion = 0;
            filaIdentidad.ValorAbsoluto = 1;
            filaIdentidad.Rank = 0;
            filaIdentidad.ActualizaHome = true;
            filaIdentidad.ActivoEnComunidad = true;
            filaIdentidad.Tipo = 0;
            filaIdentidad.NombreCortoIdentidad = configService.ObtenerLoginAdmin();
            filaIdentidad.Foto = "sinfoto";
            entityContextAcid.Identidad.Add(filaIdentidad);

            DB.Acid.IdentidadContadores filaIdentidadContadores = new DB.Acid.IdentidadContadores();
            filaIdentidadContadores.IdentidadID = filaIdentidad.IdentidadID;
            filaIdentidadContadores.NumeroDescargas = 0;
            filaIdentidadContadores.NumeroVisitas = 0;
            entityContextAcid.IdentidadContadores.Add(filaIdentidadContadores);
            return filaIdentidad;
        }
        private static DB.Acid.ProyectoUsuarioIdentidad AnyadirProyectoUsuarioIdentidad(DB.Acid.Proyecto filaProyecto, DB.Acid.Usuario filaUsuario, DB.Acid.Identidad filaIdentidad, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.ProyectoUsuarioIdentidad filaProyectoUsuarioIdentidad = new DB.Acid.ProyectoUsuarioIdentidad();
            filaProyectoUsuarioIdentidad.UsuarioID = filaUsuario.UsuarioID;
            filaProyectoUsuarioIdentidad.FechaEntrada = DateTime.UtcNow;
            filaProyectoUsuarioIdentidad.IdentidadID = filaIdentidad.IdentidadID;
            filaProyectoUsuarioIdentidad.OrganizacionGnossID = filaProyecto.OrganizacionID;
            filaProyectoUsuarioIdentidad.ProyectoID = filaProyecto.ProyectoID;
            filaProyectoUsuarioIdentidad.Reputacion = 0;
            entityContextAcid.ProyectoUsuarioIdentidad.Add(filaProyectoUsuarioIdentidad);
            return filaProyectoUsuarioIdentidad;
        }

        private static DB.Acid.HistoricoProyectoUsuario AnyadirHistoricoProyectoUsuario(DB.Acid.Proyecto filaProyecto, DB.Acid.Usuario filaUsuario, DB.Acid.Identidad filaIdentidad, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.HistoricoProyectoUsuario filaHistoricoProyectoUsuario = new DB.Acid.HistoricoProyectoUsuario();
            filaHistoricoProyectoUsuario.IdentidadID = filaIdentidad.IdentidadID;
            filaHistoricoProyectoUsuario.UsuarioID = filaUsuario.UsuarioID;
            filaHistoricoProyectoUsuario.OrganizacionGnossID = filaProyecto.OrganizacionID;
            filaHistoricoProyectoUsuario.ProyectoID = filaProyecto.ProyectoID;
            filaHistoricoProyectoUsuario.FechaEntrada = DateTime.UtcNow;
            filaHistoricoProyectoUsuario.FechaSalida = null;
            entityContextAcid.HistoricoProyectoUsuario.Add(filaHistoricoProyectoUsuario);
            return filaHistoricoProyectoUsuario;
        }

        private static DB.Acid.AdministradorProyecto AnyadirAdministradorProyecto(DB.Acid.Proyecto filaProyecto, DB.Acid.Usuario filaUsuario, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.AdministradorProyecto filaAdministradorProyecto = new DB.Acid.AdministradorProyecto();
            filaAdministradorProyecto.OrganizacionID = filaProyecto.OrganizacionID;
            filaAdministradorProyecto.ProyectoID = filaProyecto.ProyectoID;
            filaAdministradorProyecto.UsuarioID = filaUsuario.UsuarioID;
            filaAdministradorProyecto.Tipo = 0;
            entityContextAcid.AdministradorProyecto.Add(filaAdministradorProyecto);
            return filaAdministradorProyecto;
        }

        private static DB.Acid.ParametroGeneral AnyadirParametroGeneral(DB.Acid.Proyecto filaProyecto, DB.Acid.Usuario filaUsuario, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.ParametroGeneral filaParametroGeneral = new DB.Acid.ParametroGeneral();
            filaParametroGeneral.OrganizacionID = filaProyecto.OrganizacionID;
            filaParametroGeneral.ProyectoID = filaProyecto.ProyectoID;
            filaParametroGeneral.AvisoLegal = null;
            filaParametroGeneral.MensajeBienvenida = null;
            filaParametroGeneral.WikiDisponible = true;
            filaParametroGeneral.BaseRecursosDisponible = true;
            filaParametroGeneral.CompartirRecursosPermitido = true;
            filaParametroGeneral.ServicioSuscripcionDisp = true;
            filaParametroGeneral.EntidadRevisadaObligatoria = false;
            filaParametroGeneral.InvitacionesDisponibles = false;
            filaParametroGeneral.BlogsDisponibles = false;
            filaParametroGeneral.ForosDisponibles = false;
            filaParametroGeneral.EncuestasDisponibles = false;
            filaParametroGeneral.VotacionesDisponibles = true;
            filaParametroGeneral.PlataformaVideoDisponible = 0;
            filaParametroGeneral.VerVotaciones = false;
            filaParametroGeneral.LogoProyecto = null;
            filaParametroGeneral.NombreAmenazaDafoGF = "Amenaza";
            filaParametroGeneral.NombreAmenazaDafoObj = "Amenaza";
            filaParametroGeneral.NombreAmenazaDafoProc = "Amenaza";
            filaParametroGeneral.NombreDebilidadDafoGF = "Debilidad";
            filaParametroGeneral.NombreDebilidadDafoObj = "Debilidad";
            filaParametroGeneral.NombreDebilidadDafoProc = "Debilidad";
            filaParametroGeneral.NombreFortalezaDafoGF = "Fortaleza";
            filaParametroGeneral.NombreFortalezaDafoObj = "Fortaleza";
            filaParametroGeneral.NombreFortalezaDafoProc = "Fortaleza";
            filaParametroGeneral.NombreOportunidadDafoGF = "Oportunidad";
            filaParametroGeneral.NombreOportunidadDafoObj = "Oportunidad";
            filaParametroGeneral.NombreOportunidadDafoProc = "Oportunidad";
            filaParametroGeneral.DesviacionAdmitidaEnEvalua = 0;
            filaParametroGeneral.MetaAutomatPropietarioPro = 0;
            filaParametroGeneral.UmbralDetPropietariosGF = 20;
            filaParametroGeneral.UmbralDetPropietariosObj = 20;
            filaParametroGeneral.UmbralDetPropietariosProc = 20;
            filaParametroGeneral.UmbralSuficienciaEnMejora = 20;
            filaParametroGeneral.PoliticaCertificacion = null;
            filaParametroGeneral.PermitirCertificacionRec = false;
            filaParametroGeneral.PermitirRevisionManualComp = false;
            filaParametroGeneral.PermitirRevisionManualGF = false;
            filaParametroGeneral.PermitirRevisionManualObj = false;
            filaParametroGeneral.PermitirRevisionManualPro = false;
            filaParametroGeneral.CoordenadasHome = null;
            filaParametroGeneral.ImagenHomeGrande = null;
            filaParametroGeneral.ImagenHome = null;
            filaParametroGeneral.ImagenPersonalizadaPeque = null;
            filaParametroGeneral.RutaImagenesTema = null;
            filaParametroGeneral.RutaTema = null;
            filaParametroGeneral.ComentariosDisponibles = true;
            filaParametroGeneral.NombreImagenPeque = "peque";
            filaParametroGeneral.DafoDisponible = false;
            filaParametroGeneral.PlantillaDisponible = false;
            filaParametroGeneral.PreguntasDisponibles = false;
            filaParametroGeneral.EncuestasDisponibles = false;
            filaParametroGeneral.DebatesDisponibles = false;
            filaParametroGeneral.ClausulasRegistro = false;
            filaParametroGeneral.OcultarPersonalizacion = false;
            filaParametroGeneral.RdfDisponibles = false;
            filaParametroGeneral.RssDisponibles = false;
            filaParametroGeneral.CargasMasivasDisponibles = false;
            filaParametroGeneral.InvitacionesPorContactoDisponibles = true;
            filaParametroGeneral.PermitirRecursosPrivados = true;
            filaParametroGeneral.ComunidadGNOSS = true;
            filaParametroGeneral.IdiomaDefecto = "es";
            filaParametroGeneral.NumeroRecursosRelacionados = 5;
            filaParametroGeneral.FechaNacimientoObligatoria = true;
            filaParametroGeneral.SolicitarCoockieLogin = true;
            filaParametroGeneral.RdfDisponibles = true;
            filaParametroGeneral.RssDisponibles = true;
            filaParametroGeneral.LogoProyecto = null;
            filaParametroGeneral.CMSDisponible = true;
            filaParametroGeneral.PlantillaDisponible = true;
            entityContextAcid.ParametroGeneral.Add(filaParametroGeneral);
            return filaParametroGeneral;
        }

        private static DB.Acid.TesauroProyecto AnyadirTesauroProyecto(DB.Acid.Proyecto filaProyecto, DB.Acid.Tesauro filaTesauro, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.TesauroProyecto filaTesauroProyecto = new DB.Acid.TesauroProyecto();
            filaTesauroProyecto.OrganizacionID = filaProyecto.OrganizacionID;
            filaTesauroProyecto.ProyectoID = filaProyecto.ProyectoID;
            filaTesauroProyecto.TesauroID = filaTesauro.TesauroID;
            entityContextAcid.TesauroProyecto.Add(filaTesauroProyecto);
            return filaTesauroProyecto;
        }

        private static DB.Acid.BaseRecursosProyecto AnyadirBaseRecursosProyecto(DB.Acid.Proyecto filaProyecto, DB.Acid.BaseRecursos filaBaseRecursos, EntityContextAcid entityContextAcid, ConfigService configService)
        {
            DB.Acid.BaseRecursosProyecto filaBaseRecursosProyecto = new DB.Acid.BaseRecursosProyecto();
            filaBaseRecursosProyecto.BaseRecursosID = filaBaseRecursos.BaseRecursosID;
            filaBaseRecursosProyecto.ProyectoID = filaProyecto.ProyectoID;
            filaBaseRecursosProyecto.OrganizacionID = filaProyecto.OrganizacionID;
            entityContextAcid.BaseRecursosProyecto.Add(filaBaseRecursosProyecto);
            return filaBaseRecursosProyecto;
        }
    }
}
