<?php
// variables para que cambie la visualización, no implementar
$pageTitle = "Mensajes";
$clasePagina = "fichaMensaje";
$invitado = false;
$comunidad = true;
$listado = false;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Ficha - Mensaje</title>

    <?php include './shared/style.php'; ?>
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
                                <a href="./actividadReciente.php">Home</a>
                            </li>
                            <li>
                                <a href="javascript: void(0);">Mensajes</a>
                            </li>
                            <li>
                                Asunto del mensaje
                            </li>
                        </ul>
                    </div>
                    <div class="col col-12 col-xl-8 col-contenido izquierda">
                        <div class="wrapCol">
                            <div class="header-mensaje mensaje-principal">
                                <div class="h1-container">
                                    <h1>RE: Asunto del mensaje</h1>
                                </div>
                                <div class="upper-wrap">
                                    <?php include './shared/usuario/user-miniatura.php'; ?>
                                    <p class="fecha">Lun 22, jun. 2020</p>
                                </div>
                            </div>
                            <div class="destinatarios-mensaje">
                                <div class="wrap">
                                    <p>Para</p>
                                    <div class="destinatarios-wrap">
                                        <div class="lista-destinatarios">
                                            <span class="destinatario">Ricardo Alonso</span>
                                            <span class="destinatario">Félix Tuesta Valer</span>
                                            <span class="destinatario">Mario Tuesta</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="contenido contenido-mensaje">
                                <p>Hola Félix:</p>
                                <p>Me gusta la operativa en móvil. Hace saber cómo se navega desde ahí a Hércules y al resto de comunidades y también cómo se verá la mensajería en PC.</p>
                                <p>¿Podrías avazanzar en esto junto con el metabuscador en comunidad y Hércules para la reunión de mañana martes?</p>
                                <p>Gracias,</p>
                                <p>Elena</p>
                            </div>
                            <div class="respuestas-mensaje">
                                <h2 class="">Respuestas</h2>
                                <div class="respuesta">
                                    <div class="header-mensaje">
                                        <div class="upper-wrap">
                                            <?php include './shared/usuario/user-miniatura.php'; ?>
                                            <p class="fecha">Lun 23, mar. 2220</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="acciones-mensaje">
                                    <div class="wrap">
                                        <ul class="acciones-recurso no-list-style">
                                            <li>
                                                <a href="javascript: void(0);" rel="nofollow" title="Responder" class="btn btn-outline-grey responder">
                                                    <span class="material-icons">reply</span>
                                                    <span>Responder</span>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="javascript: void(0);" rel="nofollow" title="Responder a todos" class="btn btn-outline-grey responder-todos">
                                                    <span class="material-icons">reply_all</span>
                                                    <span>Responder a todos</span>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="javascript: void(0);" rel="nofollow" title="Reenviar" class="btn btn-outline-grey reenviar">
                                                    <span class="material-icons">forward</span>
                                                    <span>Reenviar</span>
                                                </a>
                                            </li>
                                            <li>
                                                <a href="javascript: void(0);" rel="nofollow" title="Borrar" class="btn btn-outline-grey borrar">
                                                    <span class="material-icons">delete</span>
                                                    <span>Eliminar</span>
                                                </a>
                                            </li>
                                            <li>
                                                <div class="dropdown">
                                                    <a href="#" class="dropdown-toggle btn btn-outline-grey no-flecha" role="button" id="dropdownMasOpciones" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="material-icons">more_vert</span></a>
                                                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMasOpciones" style="position: absolute; transform: translate3d(26px, 21px, 0px); top: 0px; left: 0px; will-change: transform;">
                                                        <ul class="no-list-style">
                                                            <li>
                                                                <a href="javascript: void(0);" rel="nofollow" class="dropdown-item">Imprimir</a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript: void(0);" rel="nofollow" class="dropdown-item">Eliminar</a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript: void(0);" rel="nofollow" class="dropdown-item">Copiar</a>
                                                            </li>
                                                            <li>
                                                                <a href="javascript: void(0);" rel="nofollow" class="dropdown-item">Cambiar</a>
                                                            </li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="contenido contenido-mensaje">
                                    <p>Hola Félix:</p>
                                    <p>Me gusta la operativa en móvil. Hace saber cómo se navega desde ahí a myGNOSS y al resto de comunidades y también cómo se verá la mensajería en PC.</p>
                                    <p>¿Podrías avazanzar en esto junto con el metabuscador en comunidad y myGNOSS para la reunión de mañana martes?</p>
                                    <p>Gracias,</p>
                                    <p>Elena</p>
                                </div>
                                <div class="respuestas-mensaje">
                                    <h2 class="">Respuestas</h2>
                                    <div class="respuesta">
                                        <div class="header-mensaje">
                                            <div class="upper-wrap">
                                                <?php include './shared/usuario/user-miniatura.php'; ?>
                                                <p class="fecha">Lun 23, mar. 2220</p>
                                            </div>
                                        </div>
                                        <div class="contenido contenido-mensaje">
                                            <p>Ricardo, acabo de subir las pantallas de la versión móvil de la mensajería a GNOSS. Te he enviado un mensaje pero en el autocompletado sólo apareces como "Ricardo Alonso Maturana - Equipo GNOSS" y creo que esa cuenta no mantienes...</p>
                                            <p>Este es el enlace a las pantallas,</p>
                                            <p><a href="https://redprivada.gnoss.com/comunidad/personalizacionesgnoss/recurso/presentacion-mensajeria-mygnoss-version-movil/481fac43-4362-4184-87ad-4bfe658bfe19">enlace</a></p>
                                        </div>
                                    </div>
                                    <div class="respuesta">
                                        <div class="header-mensaje">
                                            <div class="upper-wrap">
                                                <?php include './shared/usuario/user-miniatura.php'; ?>
                                                <p class="fecha">Lun 23, mar. 2220</p>
                                            </div>
                                        </div>
                                        <div class="contenido contenido-mensaje">
                                            <p>Buenos días Félix: habíamos quedado en ver la mensajería, ¿cuándo es posible? Gcs!</p>
                                        </div>
                                    </div>
                                    <div class="respuesta">
                                        <div class="header-mensaje">
                                            <div class="upper-wrap">
                                                <?php include './shared/usuario/user-miniatura.php'; ?>
                                                <p class="fecha">Lun 23, mar. 2220</p>
                                            </div>
                                        </div>
                                        <div class="contenido contenido-mensaje">
                                            <p>buenos días Ricardo,</p>
                                            <p>mañana te lo paso...</p>
                                            <p>Félix</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col col-12 col-xl-4 col-contexto col-lateral derecha">
                        <?php include './shared/relacionados/grupos-relacionados-mensajes.php'; ?>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <?php include './shared/footer.php'; ?>
    <?php include './shared/modales-y-sidebars.php'; ?>

</body>

</html>