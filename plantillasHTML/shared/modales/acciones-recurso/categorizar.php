<div id="modal-categorizar" class="modal modal-top fade modal-edicion" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <p class="modal-title"><span class="material-icons">folder_open</span>Categorizar</p>
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
                                <a class="nav-link active" id="ver-arbol-tab" data-toggle="tab" href="#ver-arbol" role="tab" aria-controls="ver-arbol" aria-selected="true">Árbol</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="ver-lista-tab" data-toggle="tab" href="#ver-lista" role="tab" aria-controls="ver-lista" aria-selected="false">Lista</a>
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
                                            <div class="custom-control custom-checkbox themed little primary">
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
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">
                                                        <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8696">Ecología</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="categoria-wrap">
                                                <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">
                                                        <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a43295c2">Ciencia</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="categoria-wrap">
                                                <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb">
                                                        <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb">Matemáticas</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 30882d37-a3ef-4351-95a0-643a00de7f51">
                                            <div class="custom-control custom-checkbox themed little primary">
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
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8692">
                                                        <label class="custom-control-label" for="arb_46a9196d-f95e-45a5-8dd4-84e2bdab8692">Universidad</label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="categoria-wrap">
                                                <div class="categoria e8a0549d-a7ed-458a-b3db-ead4a432123">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_e8a0549d-a7ed-458a-b3db-ead4a432123"">
                                                        <label class="custom-control-label" for="arb_e8a0549d-a7ed-458a-b3db-ead4a432123"">Bachillerato</label>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="categoria-wrap">
                                                <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">
                                                    <div class="custom-control custom-checkbox themed little primary">
                                                        <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">
                                                        <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb12">Primaria</label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-56c81b97ffcb1212">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb1212">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-56c81b97ffcb1212">Tecnología</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-333">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-333">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-333">Física</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-444">
                                            <div class="custom-control custom-checkbox themed little primary">
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
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4441">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4441">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4442">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4442">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4442">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4443">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4443">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4443">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4444">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4444">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4444">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4445">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4445">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4445">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4446">
                                            <div class="custom-control custom-checkbox themed little primary">
                                                <input type="checkbox" onclick="MarcarCatSelEditorTes(this);" class="custom-control-input" id="arb_372bc6ca-6f7a-4488-9c04-4446">
                                                <label class="custom-control-label" for="arb_372bc6ca-6f7a-4488-9c04-4446">Física cuántica</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="categoria-wrap">
                                        <div class="categoria 372bc6ca-6f7a-4488-9c04-4447">
                                            <div class="custom-control custom-checkbox themed little primary">
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
                    <div class="form-actions">
                        <a href="" class="btn btn-primary pmd-ripple-effect">Guardar</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>