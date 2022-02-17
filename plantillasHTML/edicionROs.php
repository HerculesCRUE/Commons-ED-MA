<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Personalizaciones de comunidades GNOSS";
	$clasePagina = "page-ros";
    $invitado = false;
    $comunidad = true;
    $listado = false;
    $nombreComunidad = "Hércules";
    $imagenUsuario = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Edición ROs</title>

    <?php include './shared/style.php';    ?>
    <?php include './shared/script.php'; ?>

    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="robots" content="noindex,nofollow">
</head>

<body class="<?php include './shared/clasesBody.php'; ?>" cz-shortcut-listen="true">

    <?php
    include './shared/formsGnoss.php';
    include './shared/cabecera.php';
    ?>

    <main role="main">
        <div class="container">
            <div class="row-content">
                <div class="row">
                    <div class="col col-12 col-breadcrumb">
                        <ul>
                            <li>
                                <a href="./home.php">Home</a>
                            </li>
                            <li>
                                CV
                            </li>
                        </ul>
                    </div>
                    <div class="col col-12 col-buscador">
                        <?php include './shared/buscadores/buscadorSeccion.php'; ?>
                    </div>
                    <div class="col col-12 col-xl-8 col-contenido izquierda">
                        <div class="wrapCol">
                            <div class="header-contenido">
                                <div class="h1-container">
                                    <h1>Por donde empezar</h1>
								</div>
							</div>
                            <p>Te ayudamos a gestionar los resultados de tu investigación</p>
                            <div class="page-section">
                                <span class="material-icons">backup</span>
                                <h2>Importa tu CVN</h2>
                                <p>Si dispones de tu CV en formato CVN lo puedes importar con un solo click</p>
                                <div class="image-uploader js-image-uploader">
                                    <div class="image-uploader__preview">
                                        <img class="image-uploader__img" alt="Imagen" src="">
                                    </div>
                                    <div class="image-uploader__drop-area">
                                        <div class="image-uploader__icon">
                                            <span class="material-icons">backup</span>
                                        </div>
                                        <div class="image-uploader__info">
                                            <p><strong>Arrastra y suelta en la zona punteada una foto para tu perfil</strong></p>
                                            <p>Imágenes en formato .PNG o .JPG</p>
                                            <p>Peso máximo de las imágenes 250 kb</p>
                                        </div>
                                    </div>
                                    <div class="image-uploader__error">
                                        <p class="ko"></p>
                                    </div>
                                </div>
                                <input type="file" class="image-uploader__input" accept="image/*" />
                            </div>
                            <div class="page-section">
                                <span class="material-icons">settings_input_component</span>
                                <h2>Conéctate con tus redes de investigación</h2>
                                <p>Vincula tus cuentas SCOPUS, WoS, … para que podamos recolectar tus publicaciones automáticamente</p>
                                <a href="javascript: void(0);">
                                    Vincular cuenta
                                    <span class="material-icons">navigate_next</span>
                                </a>
                            </div>
                            <div class="page-section">
                                <span class="material-icons">manage_accounts</span>
                                <h2>Editar CV</h2>
                                <p>Datos personales, Actividad docente, Experiencia científica y Actividad científica</p>
                                <a href="javascript: void(0);">
                                    Editar CV
                                    <span class="material-icons">navigate_next</span>
                                </a>
                            </div>
						</div>
					</div>
					<div class="col col-12 col-xl-4 col-lateral col-contexto derecha">
                        <aside id="menuLateralUsuarioClonado" class="menuLateral usuario clonado" role="navigation">
                            <div class="header">
                                <div class="wrap-header">
                                    <div class="usuarioWrapper">
                                        <div class="nombre">
                                            <span>Miguel Ángel Sicilia</span> - <span>Universidad de Alcalá</span>
                                        </div>
                                    </div>
                                    <span class="material-icons cerrar">close</span>
                                    <div class="cabeceraUsuario">
                                        <div class="imagen-usuario" style="background-image: url('theme/resources/imagenes-pre/foto-usuario-2.jpg')">
                                        </div>
                                        <div class="nombre-usuario">
                                            <p class="nombre">Miguel Ángel Sicilia</p>
                                            <p class="nombre-completo">
                                                <span>Universidad de Alcalá</span>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="body">
                                <div class="menuUsuario ">
                                    <div class="group-collapse resultados">
                                        <a href="#trabajo" data-toggle="collapse" aria-expanded="true">Resultados de la investigación</a>
                                        <div id="trabajo" class="collapse show">
                                            <ul class="pmd-sidebar-nav">
                                                <li class="liPublicacion">
                                                    <a href="javascript: void(0);">Publicaciones <span class="numResultados">256</span></a>
                                                </li>
                                                <li class="liComentarios">
                                                    <a href="javascript: void(0);">Pendiente de validacion <span class="numResultados">100</span></a>
                                                </li>
                                                <li class="liOtros">
                                                    <a href="javascript: void(0);">Otros resultados <span class="numResultados">110</span></a>
                                                </li>
                                                <li class="liAnadir">
                                                    <a href="javascript: void(0);" data-target="#modal-anadir-ro" data-toggle="modal">Añadir resultado de investigación</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="group-collapse consultar">
                                        <a href="#consultar" data-toggle="collapse" aria-expanded="true">Consultar</a>
                                        <div id="consultar" class="collapse show">
                                            <ul class="pmd-sidebar-nav">
                                                <li class="liNotificaciones nuevos">
                                                    <a href="./alertas.php">Alertas</a>
                                                </li>
                                                <li class="liComentarios">
                                                    <a href="javascript:void(0);">Mi trayectoria personal</a>
                                                </li>
                                                <li class="liContactos">
                                                    <a href="javascript:void(0);">Espacio personal</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <div class="group-collapse espacio">
                                        <a href="#gestionar" data-toggle="collapse" aria-expanded="true">Gestionar</a>
                                        <div id="gestionar" class="collapse show">
                                            <ul class="pmd-sidebar-nav">
                                                <li class="liContribuciones">
                                                    <a href="./edicionCV.php">Editar CV</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Importar CV</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Exportar CV</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Actividad Científica</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Convocatorias, solicitudes y proyectos</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Grupos de investigación</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Solicitudes de comisión de ética</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Propiedad industrial e intelectual</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Empresas de base tecnológica</a>
                                                </li>
                                                <li>
                                                    <a href="javascript:void(0);">Configurar cuenta</a>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </aside>
					</div>
				</div>
			</div>
		</main>
        <?php include './shared/footer.php'; ?>
        <?php include './shared/modales-y-sidebars.php'; ?>
		<?php include './shared/modales/gestion/modal-anadir-ro.php'; ?>
        <?php include './shared/modales/gestion/modal-crear-ro.php'; ?>
	</body>
</html>