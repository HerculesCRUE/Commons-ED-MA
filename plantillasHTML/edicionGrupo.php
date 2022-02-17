<?php
// variables para que cambie la visualización, no implementar
$pageTitle = "Edicion recursos";
$clasePagina = "edicionRecurso edicionGrupo";
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
    <title>Hércules - Editar recurso</title>

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
                                    <span class="material-icons">mode_edit</span>Editar perfil / grupo
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
                                    <div class="form-group mb-5 edit-titulo">
                                        <label class="control-label d-block">Título</label>
                                        <input placeholder="Introduce un título para el grupo" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                    <div class="form-group mb-5 edit-descripcion">
                                        <label class="control-label d-block">Descripción</label>
                                        <textarea id="txtDescripcion" placeholder="Contenido del recurso..." class="cke form-control recursos" cols="20" rows="2"></textarea>
                                    </div>
                                    <div class="form-group mb-5 edit-etiquetas">
                                        <label class="control-label d-block">Etiquetas</label>
                                        <div class="autocompletar autocompletar-tags form-group">
                                            <div class="input-wrap form-sticky-button">
                                                <input type="text" placeholder="Introduce una etiqueta y pulsa AÑADIR" id="txtTags" class=" form-control" autocomplete="off">
                                                <a href="" id="anadir-tag" class="btn btn-grey uppercase">Añadir</a>
                                            </div>
                                            <input type="hidden" id="txtTags_Hack" value="">
                                            <span class="tag-list mb-4">
                                                <div class="tag" title="Nombre Apellido">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Nombre Apellido</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Nombre Apellido">
                                                </div>
                                                <div class="tag" title="Grupo">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Grupo</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Grupo">
                                                </div>
                                                <div class="tag" title="usuario@dominio.com">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">usuario@dominio.com</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="usuario@dominio.com">
                                                </div>
                                            </span>

                                            <label class="control-label d-block mb-2">Etiquetas propuestas / sugeridas</label>
                                            <span class="tag-list sugerencias">
                                                <div class="tag" title="Etiqueta">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Etiqueta</span>
                                                        <span class="tag-remove material-icons">add</span>
                                                    </div>
                                                    <input type="hidden" value="Etiqueta">
                                                </div>
                                                <div class="tag" title="Etiqueta">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Etiqueta</span>
                                                        <span class="tag-remove material-icons">add</span>
                                                    </div>
                                                    <input type="hidden" value="Etiqueta">
                                                </div>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="form-group mb-5 edit-miembros">
                                        <label class="control-label d-block">Añadir miembros</label>
                                        <div class="autocompletar autocompletar-miembros form-group">
                                            <div class="input-wrap form-sticky-button">
                                                <input type="text" placeholder="Introduce nombre y/o apellidos" id="txtMiembros" class="input-miembros form-control" autocomplete="off">
                                                <span class="icon material-icons">search</span>
                                                <a href="" id="anadir-miembro" class="btn btn-grey uppercase">Añadir</a>
                                            </div>
                                            <input type="hidden" id="txtMiembros_Hack" value="">
                                            <span class="tag-list mb-4">
                                                <div class="tag" title="Nombre Apellido">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Nombre Apellido</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Nombre Apellido">
                                                </div>
                                                <div class="tag" title="Nombre Apellido">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Nombre Apellido</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Nombre Apellido">
                                                </div>
                                                <div class="tag" title="Nombre Apellido">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Nombre Apellido</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Nombre Apellido">
                                                </div>
                                            </span>
                                        </div>
                                    </div>
                                    <button type="submit" class="btn btn-primary uppercase">Guardar</button>
                                    <button type="submit" class="btn btn-link">Cancelar</button>
                                </fieldset>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

</html>