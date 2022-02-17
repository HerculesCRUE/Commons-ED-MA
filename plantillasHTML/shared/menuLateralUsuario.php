<aside id="menuLateralUsuario" class="menuLateral usuario" role="navigation">
    <div class="header">
        <div class="wrap-header">
            <div class="usuarioWrapper">
                <div class="nombre">
                    <span>Félix Tuesta</span> - <span>Equipo GNOSS</span>
                </div>
            </div>
            <span class="material-icons cerrar">close</span>
            <div class="cabeceraUsuario">
                <div class="imagen-usuario" style="background-image: url('theme/resources/imagenes-pre/foto-usuario-2.jpg')">
                </div>
                <div class="nombre-usuario">
                    <p class="nombre">Félix Tuesta</p>
                    <p class="nombre-completo">
                        <span>Félix Tuesta Sesma</span> - <span>Equipo GNOSS</span>
                    </p>
                </div>
            </div>
        </div>
    </div>

    <div class="body pmd-scrollbar mCustomScrollbar" data-mcs-theme="minimal-dark">
        <div class="menuUsuario ">
            <div class="group-collapse navegacion">
                <a href="#navegacion" data-toggle="collapse" aria-expanded="true">Navegación</a>
                <div id="navegacion" class="collapse show">
                    <ul class="pmd-sidebar-nav">
                        <li class="liInicio active">
                            <a href="javascript: void(0);">Inicio</a>
                        </li>
                        <li class="liCluster">
                            <a href="javascript: void(0);">Crear clúster</a>
                        </li>
                        <li class="liIndicadores">
                            <a href="javascript: void(0);">Indicadores</a>
                        </li>
                        <?php if($invitado): ?>
                            <li class="liIniciarSesion">
                                <a href="javascript: void(0);">Iniciar sesión</a>
                            </li>
                        <?php else: ?>
                            <li class="liNotificaciones nuevos">
                                <a href="./alertas.php">Alertas</a>
                            </li>
                        <?php endif; ?>
                    </ul>
                </div>
            </div>
            <div class="group-collapse espacio">
                <a href="#espacio" data-toggle="collapse" aria-expanded="true">Mi espacio personal</a>
                <div id="espacio" class="collapse show">
                    <ul class="pmd-sidebar-nav">
                        <li class="liContribuciones">
                            <a href="./listado.php">Contribuciones</a>
                        </li>
                        <li class="liPublicaciones">
                            <a href="./listado.php">Recursos / Guardados</a>
                        </li>
                        <li class="liFavoritos">
                            <a href="./listado.php">Favoritos</a>
                        </li>
                        <li class="liBorradores">
                            <a href="./listado.php">Borradores</a>
                        </li>
                        <li class="ver">
                            <a href="./listado.php">Ver todas</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="group-collapse comunidades">
                <a href="#comunidades" data-toggle="collapse" aria-expanded="true">Mis comunidades</a>
                <div id="comunidades" class="collapse show">
                    <ul class="pmd-sidebar-nav">
                        <li><a href="./listado.php">Next Web</a></li>
                        <li><a href="./listadoComunidadAlternativa.php">Gestión de proyectos GNOSS</a></li>
                        <li><a href="./listadoComunidadAlternativa.php">Soporte GNOSS Preproduccion</a></li>
                        <li><a href="./listadoComunidadAlternativa.php">Gnoss Academy</a></li>
                        <li class="ver">
                            <a href="javascript: void(0);">Ver todas</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="group-collapse identidades">
                <a href="#identidades" data-toggle="collapse" aria-expanded="true">Mis identidades</a>
                <div id="identidades" class="collapse show">
                    <ul class="pmd-sidebar-nav">
                        <li>
                            <a href="javascript: void(0);">Equipo GNOSS</a>
                        </li>
                        <li>
                            <a href="javascript: void(0);">Didactalia</a>
                        </li>
                        <li>
                            <a href="javascript: void(0);">Configuración</a>
                        </li>
                        <li class="ver">
                            <a href="javascript: void(0);">Ver todas</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="group-collapse gestionar">
                <a href="#gestionar" data-toggle="collapse" aria-expanded="true">Gestionar</a>
                <div id="gestionar" class="collapse show">
                    <ul class="pmd-sidebar-nav">
                        <li class="liInvitarComunidad">
                            <a class="invitar-comunidad-toggle" data-target="#modal-invitar-comunidad" data-toggle="modal" href="#">Invitar a comunidad</a>
                        </li>
                        <li class="liGestionarSuscripciones">
                            <a class="gestionar-suscripciones-toggle" data-target="#modal-gestionar-suscripciones" data-toggle="modal" href="#">Gestionar suscripciones</a>
                        </li>
                        <li class="liRecibirNewsletter">
                            <a class="recibir-newsletter-toggle" data-target="#modal-recibir-newsletter" data-toggle="modal" href="#">Recibir newsletter</a>
                        </li>
                        <li class="liAbandonarComunidad">
                            <a class="abandonar-comunidad-toggle" data-target="#modal-abandonar-comunidad" data-toggle="modal" href="#">Abandonar comunidad</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="group-collapse desconectar">
                <div id="desconectar-usuario" class="collapse show">
                    <ul class="pmd-sidebar-nav">
                        <li>
                            <a class="desconectar-usuario bold">Desconectar</a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</aside>
<div class="pmd-sidebar-overlay" data-rel="menuLateralUsuario"></div>