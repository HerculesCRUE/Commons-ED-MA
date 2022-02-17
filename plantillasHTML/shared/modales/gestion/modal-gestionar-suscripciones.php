<div id="modal-gestionar-suscripciones" class="modal modal-top fade" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <p class="modal-title"><span class="material-icons">folder_open</span>Suscripciones</p>
                <span class="material-icons cerrar" data-dismiss="modal" aria-label="Close">close</span>
            </div>
            <div class="modal-body">
                <div class="formulario-edicion">
                    <div class="form-group">
                        <label class="control-label">Categorizar</label>
                    </div>
                    <div id="panDesplegableSelCat">
                        <ul class="nav nav-tabs" id="myTab" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="ver-arbol-suscripciones-tab" data-toggle="tab" href="#ver-arbol-suscripciones" role="tab" aria-controls="ver-arbol-suscripciones" aria-selected="true">Árbol</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="ver-lista-suscripciones-tab" data-toggle="tab" href="#ver-lista-suscripciones" role="tab" aria-controls="ver-lista-suscripciones" aria-selected="false">Lista</a>
                            </li>
                        </ul>
                        <div class="tab-content">

                            <div class="tab-pane fade show active" id="ver-arbol-suscripciones" role="tabpanel" aria-labelledby="ver-arbol-suscripciones-tab">
                                <div class="divTesArbol divCategorias clearfix">
                                    <div class="buscador-categorias">
                                        <div class="form-group">
                                            <input class="filtroRapido form-control not-outline" placeholder="Busca una categoría" type="text" onkeydown="javascript:if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {return false;}}" onkeyup="javascript:MVCFiltrarListaSelCatArbol(this, 'panDesplegableSelCat');">
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 30882d37-a3ef-4351-95a0-643a00de7f52">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_30882d37-a3ef-4351-95a0-643a00de7f52">
                                                <label class="custom-control-label" for="arb_30882d37-a3ef-4351-95a0-643a00de7f52">Temas</label>
                                            </div>
                                        </div>
                                        <!--  pintar esto solo cuando tenga hijos -->
                                        <div class="boton-desplegar">
                                            <span class="material-icons">keyboard_arrow_down</span>
                                        </div>
                                        <!--  -->
                                        <div class="panHijos">
                                            <div class="categoria-wrap">
                                                <div class="categoria 46a9196d-f95e-45a5-8dd4-84e2bdab8697">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8697">
                                                        <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8697">Ecología</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="categoria-wrap">
                                                <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a43295c3">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c3">
                                                        <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c3">Ciencia</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="categoria-wrap">
                                                <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcc">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcc">
                                                        <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcc">Matemáticas</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 30882d37-a3ef-4351-95a0-643a00de7f53">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_30882d37-a3ef-4351-95a0-643a00de7f53">
                                                <label class="custom-control-label" for="arb_30882d37-a3ef-4351-95a0-643a00de7f53">Nivel académico</label>
                                            </div>
                                        </div>
                                        <!--  pintar esto solo cuando tenga hijos -->
                                        <div class="boton-desplegar">
                                            <span class="material-icons">keyboard_arrow_down</span>
                                        </div>
                                        <!--  -->
                                        <div class="panHijos">
                                            <div class="categoria-wrap">
                                                <div class="categoria 46a9196d-f95e-45a5-8dd4-84e2bdab8693">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8693">
                                                        <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8693">Universidad</label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="categoria-wrap">
                                                <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a432124">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a432124"">
                                                        <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a432124"">Bachillerato</label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="categoria-wrap">
                                                <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcc13">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcc13">
                                                        <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcc13">Primaria</label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcc1313">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcc1313">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcc1313">Tecnología</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-334">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-334">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-334">Física</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-445">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-445">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-445">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <div class="tab-pane fade" id="ver-lista-suscripciones" role="tabpanel" aria-labelledby="ver-lista-suscripciones-tab">
                                <div class="divTesLista divCategorias clearfix">
                                    <div class="buscador-categorias">
                                        <div class="form-group">
                                            <input class="filtroRapido form-control not-outline" placeholder="Busca una categoría" type="text" onkeydown="javascript:if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {return false;}}" onkeyup="javascript:MVCFiltrarListaSelCat(this, 'panDesplegableSelCat');">
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4451">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4451">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4451">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4452">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4452">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4452">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4453">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4453">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4453">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4454">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4454">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4454">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4455">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4455">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4455">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4456">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4456">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4456">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4457">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4457">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4457">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <input type="hidden" id="txtHackCatTesSel" class="hackCatTesSel" value="c5f38eb9-3d3c-4dd5-8fb7-0c771cb4a044,e9292906-a378-477f-b01e-c6c005be201e,">
                            </div>
                        </div>
                    </div>
                    <div class="form-actions">
                        <a href="" class="btn btn-primary pmd-ripple-effect">Guardar</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>