<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Registro";
    $clasePagina = "registro";
    $invitado = true;
    $comunidad = true;
    $listado = false;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Registro</title>

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
                    <div id="col01" class="col">
                        <div class="form formularioRegistroUsuarios panel-centrado">
                            <h1>Hola <strong>Félix</strong>, gracias por unirte a <strong>Nextweb</strong></h1>
                            <div class="box box01">
                                <div class="step-progress-wrap mb-5">
                                    <ul class="step-progress">
                                        <li class="step-progress__circle done active">
                                            <span class="step-progress__text">Datos básicos</span>
                                        </li>
                                        <li class="step-progress__bar active"></li>
                                        <li class="step-progress__circle done active">
                                            <span class="step-progress__text">Datos personales</span>
                                        </li>
                                        <li class="step-progress__bar active"></li>
                                        <li class="step-progress__circle active">
                                            <span class="step-progress__text current">Tus intereses</span>
                                        </li>
                                        <li class="step-progress__bar"></li>
                                        <li class="step-progress__circle">
                                            <span class="step-progress__text">Red de contactos</span>
                                        </li>
                                    </ul>
                                </div>
                                <div class="form-group mb-5">
                                    <div id="panDesplegableSelCat">
                                        <ul class="nav nav-tabs" id="myTab" role="tablist">
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

                                                            <div class=" categoria-wrap">
                                                                <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">
                                                                    <div class="custom-control custom-checkbox themed little">
                                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">
                                                                        <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">Primaria</label>
                                                                    </div>
                                                                </div>
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
                            </div>
                            <div class="actionButtons">
                                <div class="mb-3">
                                    <input type="hidden" value="" name="urlOrigen">
                                    <input type="hidden" value="f1a1f955-fcf1-4ce7-ae4a-492fdc5c33ba" name="txtSeguridad">
                                    <input type="submit" value="Guardar y siguiente" class="btn btn-primary principal submit uppercase" onclick="Registro_EnviarPreferencias()">
                                </div>
                                <div>
                                    <span>¿Ya eres usuario de GNOSS?</span>
                                    <a class="link" href="" onclick="CargarFormLoginRegistro('http://pruebas.gnoss.net/comunidad/gnoss-developers-community/hazte-miembro')">Iniciar sesión</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <?php include './shared/footer.php'; ?>
    <?php include './shared/modales-y-sidebars.php'; ?>
</body>

</html>