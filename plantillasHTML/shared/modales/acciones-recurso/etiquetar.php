<div id="modal-etiquetar" class="modal modal-top fade modal-edicion" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <p class="modal-title"><span class="material-icons">label</span>Etiquetar</p>
                <span class="material-icons cerrar" data-dismiss="modal" aria-label="Close">close</span>
            </div>
            <div class="modal-body">
                <div class="formulario-edicion">
                    <div class="autocompletar autocompletar-tags form-group">
                        <div class="input-wrap form-sticky-button">
                            <input type="text" placeholder="Introduce una etiqueta y pulsa AÑADIR" id="txtTags" class=" form-control" autocomplete="off">
                            <a href="" id="anadir-tag" class="btn btn-primary uppercase">Añadir</a>
                        </div>
                        <input type="hidden" id="txtTags_Hack" value="">
                        <span class="tag-list">
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
                    </div>
                    <div class="form-actions">
                        <a href="" class="btn btn-primary">Guardar</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>