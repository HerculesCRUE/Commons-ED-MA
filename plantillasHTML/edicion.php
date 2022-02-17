<?php
// variables para que cambie la visualización, no implementar
$pageTitle = "Edicion recuros";
$clasePagina = "edicionRecurso";
$invitado = false;
$comunidad = true;
$nombreComunidad = "Hércules";
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
                                    <span class="material-icons">mode_edit</span>Nuevo Recurso
                                </p>
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
                                        <input placeholder="Título del recurso" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                    <div class="form-group mb-5 edit-descripcion">
                                        <label class="control-label d-block">Descripción</label>
                                        <textarea id="txtDescripcion" placeholder="Descripción del recurso" class="cke form-control recursos" cols="20" rows="2"></textarea>
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
                                                <div class="tag" title="Educación">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Educación</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Educación">
                                                </div>
                                                <div class="tag" title="Deportes">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Deportes</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Deportes">
                                                </div>
                                                <div class="tag" title="Educación especial">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Educación especial</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Educación especial">
                                                </div>
                                            </span>

                                            <label class="control-label d-block mb-2">Etiquetas propuestas / sugeridas</label>
                                            <span class="tag-list sugerencias">
                                                <div class="tag" title="Educación">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Educación</span>
                                                        <span class="tag-remove material-icons">add</span>
                                                    </div>
                                                    <input type="hidden" value="Educación">
                                                </div>
                                                <div class="tag" title="Deportes">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Deportes</span>
                                                        <span class="tag-remove material-icons">add</span>
                                                    </div>
                                                    <input type="hidden" value="Deportes">
                                                </div>
                                                <div class="tag" title="Educación especial">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Educación especial</span>
                                                        <span class="tag-remove material-icons">add</span>
                                                    </div>
                                                    <input type="hidden" value="Educación especial">
                                                </div>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="form-group edit-categorias">
                                        <label class="control-label d-block">Guardar en</label>
                                        <div id="panDesplegableSelCat">
                                            <ul class="nav nav-tabs grey" id="myTab" role="tablist">
                                                <li class="nav-item">
                                                    <a class="nav-link active" id="ver-arbol-tab" data-toggle="tab" href="#ver-arbol" role="tab" aria-controls="ver-arbol" aria-selected="true">Árbol</a>
                                                </li>
                                                <li class="nav-item">
                                                    <a class="nav-link" id="ver-list-tab" data-toggle="tab" href="#ver-lista" role="tab" aria-controls="ver-lista" aria-selected="false">Lista</a>
                                                </li>
                                            </ul>
                                            <div class="tab-content">

                                                <div class="tab-pane fade show active" id="ver-arbol" role="tabpanel" aria-labelledby="ver-arbol-tab">
                                                    <div class="divTesArbol divCategorias clearfix">
                                                        <div class="buscador-categorias">
                                                            <div class="form-group">
                                                                <input class="filtroRapido form-control not-outline" placeholder="Busca una categoría" type="text" onkeydown="javascript:if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {return false;}}" onkeyup="javascript:MVCFiltrarListaSelCatArbol(this, 'panDesplegableSelCat');">
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 30882d37-a3ef-4351-95a0-643a00de7f50">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_30882d37-a3ef-4351-95a0-643a00de7f50">
                                                                    <label class="custom-control-label" for="arb_30882d37-a3ef-4351-95a0-643a00de7f50">Temas</label>
                                                                </div>
                                                            </div>
                                                            <!--  pintar esto solo cuando tenga hijos -->
                                                            <div class="boton-desplegar">
                                                                <span class="material-icons">keyboard_arrow_down</span>
                                                            </div>
                                                            <!--  -->
                                                            <div class="panHijos">
                                                                <div class="categoria-wrap">
                                                                    <div class="categoria 46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                        <div class="custom-control custom-checkbox themed little">
                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                            <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">Ecología</label>
                                                                        </div>
                                                                    </div>
                                                                    <!--  pintar esto solo cuando tenga hijos -->
                                                                    <div class="boton-desplegar">
                                                                        <span class="material-icons">keyboard_arrow_down</span>
                                                                    </div>
                                                                    <!--  -->
                                                                    <div class="panHijos">
                                                                        <div class="categoria-wrap">
                                                                            <div class="categoria 46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                                <div class="custom-control custom-checkbox themed little">
                                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                                    <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">Universidad</label>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="categoria-wrap">
                                                                            <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                                <div class="custom-control custom-checkbox themed little">
                                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                                    <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">Ciencia</label>
                                                                                </div>
                                                                            </div>
                                                                            <!--  pintar esto solo cuando tenga hijos -->
                                                                            <div class="boton-desplegar">
                                                                                <span class="material-icons">keyboard_arrow_down</span>
                                                                            </div>
                                                                            <!--  -->
                                                                            <div class="panHijos">
                                                                                <div class="categoria-wrap">
                                                                                    <div class="categoria 46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                                        <div class="custom-control custom-checkbox themed little">
                                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                                            <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">Ecología</label>
                                                                                        </div>
                                                                                    </div>
                                                                                    <!--  pintar esto solo cuando tenga hijos -->
                                                                                    <div class="boton-desplegar">
                                                                                        <span class="material-icons">keyboard_arrow_down</span>
                                                                                    </div>
                                                                                    <!--  -->
                                                                                    <div class="panHijos">
                                                                                        <div class="categoria-wrap">
                                                                                            <div class="categoria 46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                                                <div class="custom-control custom-checkbox themed little">
                                                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                                                                    <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">Universidad</label>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                        <div class="categoria-wrap">
                                                                                            <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                                                <div class="custom-control custom-checkbox themed little">
                                                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                                                    <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">Ciencia</label>
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="categoria-wrap">
                                                                                    <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                                        <div class="custom-control custom-checkbox themed little">
                                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                                            <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">Ciencia</label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="categoria-wrap">
                                                                                    <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb">
                                                                                        <div class="custom-control custom-checkbox themed little">
                                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb">
                                                                                            <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb">Matemáticas</label>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="categoria-wrap">
                                                                    <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                        <div class="custom-control custom-checkbox themed little">
                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                                            <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">Ciencia</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="categoria-wrap">
                                                                    <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb">
                                                                        <div class="custom-control custom-checkbox themed little">
                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb">
                                                                            <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb">Matemáticas</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="categoria-wrap">
                                                            <div class="categoria 30882d37-a3ef-4351-95a0-643a00de7f51">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_30882d37-a3ef-4351-95a0-643a00de7f51">
                                                                    <label class="custom-control-label" for="arb_30882d37-a3ef-4351-95a0-643a00de7f51">Nivel académico</label>
                                                                </div>
                                                            </div>
                                                            <!--  pintar esto solo cuando tenga hijos -->
                                                            <div class="boton-desplegar">
                                                                <span class="material-icons">keyboard_arrow_down</span>
                                                            </div>
                                                            <!--  -->
                                                            <div class="panHijos">
                                                                <div class="categoria-wrap">
                                                                    <div class="categoria 46a9196d-f95e-45a5-8dd4-84e2bdab8692">
                                                                        <div class="custom-control custom-checkbox themed little">
                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8692">
                                                                            <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8692">Universidad</label>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="categoria-wrap">
                                                                    <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a432123">
                                                                        <div class="custom-control custom-checkbox themed little">
                                                                            <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a432123">
                                                                            <label class=" custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a432123">Bachillerato</label>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">Primaria</label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb1212">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb1212">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb1212">Tecnología</label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-333">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-333">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-333">Física</label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-444">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-444">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-444">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="tab-pane fade" id="ver-lista" role="tabpanel" aria-labelledby="ver-lista-tab">
                                                    <div class="divTesLista divCategorias clearfix">
                                                        <div class="buscador-categorias">
                                                            <div class="form-group">
                                                                <input class="filtroRapido form-control not-outline" placeholder="Busca una categoría" type="text" onkeydown="javascript:if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {return false;}}" onkeyup="javascript:MVCFiltrarListaSelCat(this, 'panDesplegableSelCat');">
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-4441">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4441">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4441">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-4442">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4442">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4442">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-4443">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4443">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4443">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-4444">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4444">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4444">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-4445">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4445">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4445">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-4446">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4446">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4446">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="categoria-wrap">
                                                            <div class="categoria 372bc6ca-6f7a-4488-9c04-4447">
                                                                <div class="custom-control custom-checkbox themed little">
                                                                    <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4447">
                                                                    <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4447">Física cuántica</label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <input type="hidden" id="txtHackCatTesSel" class="hackCatTesSel" value="c5f38eb9-3d3c-4dd5-8fb7-0c771cb4a044,e9292906-a378-477f-b01e-c6c005be201e,">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset>
                                    <div class="form-section-title mb-4 uppercase">
                                        <span class="">Privacidad y seguridad</span>
                                    </div>
                                    <div class="form-group mb-4 edit-permisos-edicion">
                                        <label class="control-label d-block mb-3">Personas que pueden EDITAR el recurso</label>
                                        <div class="custom-control themed little custom-radio">
                                            <input type="radio" id="permisos-edicion-solo-yo" name="permisos-edicion" class="custom-control-input" checked>
                                            <label class="custom-control-label" for="permisos-edicion-solo-yo">Sólo yo</label>
                                        </div>
                                        <div class="custom-control themed little custom-radio">
                                            <input type="radio" id="permisos-edicion-otros" name="permisos-edicion" class="custom-control-input">
                                            <label class="custom-control-label" for="permisos-edicion-otros">Personas y/o grupos específicos</label>
                                        </div>
                                        <div id="divContDespEdit" class="form-group mb-4 pl-5">
                                            <div class="input-wrap">
                                                <input type="text" placeholder="Introduce una persona y/o grupo" class="form-control filtroFaceta filtroFacetaSelectUsuRec ac_input" autocomplete="off">
                                            </div>
                                            <span class="tag-list mb-4 mt-3">
                                                <div class="tag" title="Carlos">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Carlos</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Carlos">
                                                </div>
                                                <div class="tag" title="Javier Martínez">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Javier Martínez</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Javier Martínez">
                                                </div>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="form-group mb-4 edit-permisos-ver">
                                        <label class="control-label d-block mb-3">Personas que pueden VER el recurso</label>
                                        <div class="custom-control themed little custom-radio">
                                            <input type="radio" id="permisos-ver-cualquiera" name="permisos-ver" class="custom-control-input" checked>
                                            <label class="custom-control-label" for="permisos-ver-cualquiera">Cualquier miembro de la comunidad</label>
                                        </div>
                                        <div class="custom-control themed little custom-radio">
                                            <input type="radio" id="permisos-ver-editores" name="permisos-ver" class="custom-control-input">
                                            <label class="custom-control-label" for="permisos-ver-editores">Solo editores</label>
                                        </div>
                                        <div class="custom-control themed little custom-radio">
                                            <input type="radio" id="permisos-ver-otros" name="permisos-ver" class="custom-control-input">
                                            <label class="custom-control-label" for="permisos-ver-otros">Personas y/o grupos específicos</label>
                                        </div>

                                        <div id="divContDespLect" class="form-group mb-4 pl-5">
                                            <div class="input-wrap">
                                                <input type="text" placeholder="Introduce una persona y/o grupo" class="form-control filtroFaceta filtroFacetaSelectUsuRec ac_input" autocomplete="off">
                                            </div>
                                            <span class="tag-list mb-4 mt-3">
                                                <div class="tag" title="Carlos">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Carlos</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Carlos">
                                                </div>
                                                <div class="tag" title="Javier Martínez">
                                                    <div class="tag-wrap">
                                                        <span class="tag-text">Javier Martínez</span>
                                                        <span class="tag-remove material-icons">close</span>
                                                    </div>
                                                    <input type="hidden" value="Javier Martínez">
                                                </div>
                                            </span>
                                        </div>

                                    </div>
                                    <div class="form-group mb-4 edit-proteger-version">
                                        <label class="control-label d-block mb-3">Proteger versión</label>
                                        <div class="custom-control little custom-checkbox">
                                            <input type="checkbox" id="proteger-version" name="proteger-version" class="custom-control-input">
                                            <label class="custom-control-label" for="proteger-version">Esta versión no podrá eliminarse por otros editores.</label>
                                        </div>
                                    </div>
                                    <div class="form-group mb-5 edit-compartir">
                                        <label class="control-label d-block mb-3">Compartir</label>
                                        <div class="custom-control little custom-checkbox">
                                            <input type="checkbox" id="permisos-compartir" name="permisos-compartir" class="custom-control-input">
                                            <label class="custom-control-label" for="permisos-compartir">Este recurso se podrá compartir en las distintas comunidades.</label>
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset>
                                    <div class="form-section-title mb-4 uppercase">
                                        <span class="">Propiedad intelectual</span>
                                    </div>
                                    <div class="form-group mb-4 edit-autor-recurso">
                                        <label class="control-label d-block mb-3">¿Eres el autor de este recurso?</label>
                                        <div class="custom-control themed little custom-radio">
                                            <input type="radio" id="autor-recurso-si" name="autor-recurso" class="custom-control-input" checked>
                                            <label class="custom-control-label" for="autor-recurso-si">Sí</label>
                                            <div class="configuracion-autor mt-4">
                                                <div class="form-group mb-3">
                                                    <label class="control-label d-block">GNOSS ofrece la posibilidad a sus usuarios de que licencien los contenidos que aportan y de los que son autores bajo una licencia Creative Commons. Con una licencia de Creative Commons, mantienes tus derechos de autor pero permites a otras personas copiar y distribuir tu obra, siempre y cuando reconozcan la correspondiente autoría y solamente bajo las condiciones que especifiques aquí.</label>
                                                </div>
                                                <div class="form-group mb-4 edit-permisos-usos">
                                                    <label class="control-label d-block mb-3">¿Quieres permitir usos comerciales de tu obra?</label>
                                                    <div class="custom-control themed little custom-radio">
                                                        <input type="radio" id="permisos-usos-no" name="permisos-usos" class="custom-control-input" checked>
                                                        <label class="custom-control-label" for="permisos-usos-no">No</label>
                                                    </div>
                                                    <div class="custom-control themed little custom-radio">
                                                        <input type="radio" id="permisos-usos-si" name="permisos-usos" class="custom-control-input">
                                                        <label class="custom-control-label" for="permisos-usos-si">Sí</label>
                                                    </div>
                                                </div>
                                                <div class="form-group mb-4 edit-permisos-modificacion">
                                                    <label class="control-label d-block mb-3">¿Quieres permitir usos comerciales de tu obra?</label>
                                                    <div class="custom-control themed little custom-radio">
                                                        <input type="radio" id="permisos-modificacion-no" name="permisos-modificacion" class="custom-control-input" checked>
                                                        <label class="custom-control-label" for="permisos-modificacion-no">No</label>
                                                    </div>
                                                    <div class="custom-control themed little custom-radio">
                                                        <input type="radio" id="permisos-modificacion-si" name="permisos-modificacion" class="custom-control-input">
                                                        <label class="custom-control-label" for="permisos-modificacion-si">Sí</label>
                                                    </div>
                                                    <div class="custom-control themed little custom-radio">
                                                        <input type="radio" id="permisos-modificacion-si-mientras" name="permisos-modificacion" class="custom-control-input">
                                                        <label class="custom-control-label" for="permisos-modificacion-si-mientras">Sí, mientras se comparta de la misma manera</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="custom-control themed little custom-radio mb-4">
                                            <input type="radio" id="autor-recurso-no" name="autor-recurso" class="custom-control-input">
                                            <label class="custom-control-label" for="autor-recurso-no">No</label>
                                        </div>
                                        <div class="form-group mb-4 anadir-autor pl-5">
                                            <input class="submit btn btn-grey uppercase mb-4" id="lbAgregarAutores" type="button" onclick="BtnAgregarAutores_Click()" value="Añadir otro autor">
                                            <div id="fielAutores">
                                                <label class="control-label" for="autor-recurso-no">Otros autores</label>
                                                <div class="input-wrap">
                                                    <input type="text" id="txtAutores" placeholder="Introduce un autor" class="txtAutocomplete ac_input form-control" value="" autocomplete="off">
                                                    <input type="hidden" id="txtAutores_Hack">
                                                </div>
                                                <span class="tag-list mb-4 mt-3">
                                                    <div class="tag" title="Carlos">
                                                        <div class="tag-wrap">
                                                            <span class="tag-text">Carlos</span>
                                                            <span class="tag-remove material-icons">close</span>
                                                        </div>
                                                        <input type="hidden" value="Carlos">
                                                    </div>
                                                    <div class="tag" title="Javier Martínez">
                                                        <div class="tag-wrap">
                                                            <span class="tag-text">Javier Martínez</span>
                                                            <span class="tag-remove material-icons">close</span>
                                                        </div>
                                                        <input type="hidden" value="Javier Martínez">
                                                    </div>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

</html>