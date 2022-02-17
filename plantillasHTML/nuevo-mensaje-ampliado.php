<?php
// variables para que cambie la visualización, no implementar
$pageTitle = "Edicion recuros";
$clasePagina = "edicionRecurso nuevoMensaje";
$invitado = false;
$comunidad = true;
$nombreComunidad = "Next Web: web 3.0, web semántica y el futuro de internet";
$listado = false;
$imagenUsuario = true;
$min_height_content = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Nuevo mensaje</title>

    <?php include './shared/style.php'; ?>
    <?php include './shared/script.php'; ?>
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="robots" content="noindex,nofollow">
</head>

<body class="<?php include './shared/clasesBody.php'; ?> page-modal" cz-shortcut-listen="true">

    <?php
    include './shared/formsGnoss.php';
    include './shared/cabecera.php';
    ?>

    <main role="main">
        <div class="container">
            <div class="row-content">
                <div class="row">
                    <div class="col col-12 header-tipo-modal texto-blanco">
                        <div class="container">
                            <div class="modal-header">
                                <p class="modal-title">
                                    <span class="material-icons">mode_edit</span>Mensaje nuevo
                                </p>
                                <a href="./ficha.php" class="cerrar texto-blanco">
                                    <span class="material-icons">close</span>
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="col col-12 col-edicion">
                        <div class="wrapCol container">
                            <form class="formulario-edicion background-blanco">
                                <fieldset>
                                    <div class="form-group mb-0 edit-descripcion">
                                        <label class="control-label d-block">Descripción</label>
                                        <textarea id="txtDescripcion" placeholder="Descripción del mensaje" class="cke form-control " cols="20" rows="6"></textarea>
                                    </div>
                                    <div id="panRespuesta" class="respuesta-mensaje">
                                        <div class="comentario" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a" resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
                                            <div class="bloque-comentario">
                                                <div class="col-titulo">
                                                    <p>En respuesta a</p>
                                                </div>
                                                <div class="col-comentario">
                                                    <div class="comentario-header">
                                                        <div class="user-miniatura">
                                                            <div class="nombre-usuario-wrap">
                                                                <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                                                    <span class="nombre" property="foaf:name">Esteban Ramírez</span>
                                                                </a>
                                                            </div>
                                                        </div>
                                                        <div class="fecha-publicacion">
                                                            <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 34 minutos</span>
                                                        </div>
                                                    </div>
                                                    <div class="comentario-body">
                                                        <div property="sioc:content" class="comentario-contenido">
                                                            <p></p>
                                                            <p>Vestibulum turpis arcu, elementum ac tellus ut, tincidunt venenatis mauris. Sed sed nisl quis arcu rhoncus congue.</p>

                                                            <p>Nam libero nulla, eleifend quis urna quis, commodo lacinia quam. In tristique vel urna elementum malesuada. Etiam viverra non libero sit amet consectetur. Cras dolor dui, fringilla laoreet mi a, tristique ultricies erat. In odio metus, laoreet at ante eget, semper viverra justo. In nec laoreet nisi. Praesent ac tincidunt diam.</p>

                                                            <p>Suspendisse potenti. Vivamu&nbsp;ac erat aliquam, convallis ligula quis, accumsan metus. Integer molestie mollis semper. In semper nisi quis egestas fringilla. Nullam sit amet diam quis libero semper luctus quis quis velit.</p>
                                                            <p></p>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-usuario">
                                                    <?php include './shared/usuario/user-miniatura-solo-imagen.php' ?>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                                <a href="javascript: void(0);" class="btn btn-primary uppercase">Enviar</a>
                                <a href="javascript: void(0);" class="btn btn-link uppercase">Guardar borrador</a>
                                <a href="javascript: void(0);" class="btn btn-link uppercase">Cancelar</a>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

</html>