<div id="modal-enviar" class="modal modal-top fade modal-edicion" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <p class="modal-title"><span class="material-icons">send</span>Enviar</p>
                <span class="material-icons cerrar" data-dismiss="modal" aria-label="Close">close</span>
            </div>
            <div class="modal-body">
                <div class="formulario-edicion formulario-enviar">
                    <div class="form-group mb-1">
                        <label class="control-label">Añadir destinatarios</label>
                        <div id="panDesplegableDestinatarios">
                            <ul class="nav nav-tabs" id="myTab" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" id="miembros-tab" data-toggle="tab" href="#miembros" role="tab" aria-controls="miembros" aria-selected="true">Miembros de la comunidad</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="cuentas-correo-tab" data-toggle="tab" href="#cuentas-correo" role="tab" aria-controls="cuentas-correo" aria-selected="false">Cuentas de correo</a>
                                </li>
                            </ul>
                            <div class="tab-content">

                                <div class="tab-pane fade active show" id="miembros" role="tabpanel" aria-labelledby="miembros-tab">
                                    <div class="filtroRapido">
                                        <input type="text" placeholder="Buscar por nombre y/o apellido" onkeydown="if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {return false;}}" id="txtFiltro" class="form-control ac_input" autocomplete="off">
                                        <span class="icon material-icons">search</span>
                                    </div>
                                </div>

                                <div class="tab-pane fade" id="cuentas-correo" role="tabpanel" aria-labelledby="cuentas-correo-tab">
                                    <div class="autocompletar autocompletar-tags form-group">
                                        <div class="input-wrap form-sticky-button">
                                            <input type="text" placeholder="Introduce los correos separados por comas y haz clic en añadir" id="txtTags" class=" form-control" autocomplete="off">
                                            <a href="" id="anadir-tag" class="btn btn-primary uppercase">Añadir</a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="panContenedorInvitados" class="mb-4 panContenedorInvitados">
                        <!-- mostar este span cuando NO haya destinatarios -->
                        <span class="no-destinatarios texto-gris-claro d-block mb-3">Ningún destinatario añadido...</span>
                        <!-- mostar este span cuando SI haya destinatarios -->
                        <span class="tag-list lista-destinatarios">
                            <div class="tag">
                                <div class="tag-wrap">
                                    <span class="tag-text">usuario@dominio.com</span>
                                    <span class="tag-remove material-icons">close</span>
                                </div>
                            </div>
                            <div class="tag">
                                <div class="tag-wrap">
                                    <span class="tag-text">otrousuario@otrodominio.com</span>
                                    <span class="tag-remove material-icons">close</span>
                                </div>
                            </div>
                        </span>
                    </div>

                    <div class="form-group mb-4">
                        <label class="control-label d-block">Idioma</label>
                        <select id="ddlIdioma" class="js-select2 form-control pmd-select2">
                            <option value="es" selected="selected">Español</option>
                            <option value="en">English</option>
                            <option value="pt">Português</option>
                            <option value="ca">Català</option>
                            <option value="eu">Euskara</option>
                            <option value="gl">Galego</option>
                            <option value="fr">Français</option>
                            <option value="de">Deutsch</option>
                            <option value="it">Italiano</option>
                        </select>
                    </div>

                    <div class="form-group mb-4">
                        <label class="control-label d-block">Mensaje (opcional)</label>
                        <textarea placeholder="Si lo deseas, puedes incluir una nota personal en el correo de envío del enlace" class="cke mensajes form-control" id="txtNotas"></textarea>
                    </div>

                    <div class="form-group mb-4">
                        <label class="control-label">Enlace a</label>
                        <div class="recurso-enviar-wrap">
                            <div class="header-resource">
                                <div class="title-wrap">
                                    <p class="title">Extensión MVC a Comunidades del ecosistema: Innovación a tener en cuenta</p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-actions">
                        <a href="" class="btn btn-primary">Enviar</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>