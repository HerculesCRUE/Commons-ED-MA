<div id="modal-crear-ro" class="modal modal-top modal-wide fade" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <p class="modal-title"><span class="material-icons">edit</span>Crear / Editar RO de código</p>
                <span class="material-icons cerrar" data-dismiss="modal" aria-label="Close">close</span>
            </div>
            <div class="modal-body">
                <form class="formulario-edicion formulario-edicion-ro">
                    <div class="actions">
                        <div class="dropdown">
                            <a class="dropdown-toggle btn btn-outline-grey no-flecha" data-toggle="dropdown" aria-expanded="false">
                                <span class="texto">Plegar todas</span>
                                <span class="material-icons">keyboard_arrow_down</span>
                            </a>
                            <div class="dropdown-menu basic-dropdown dropdown-menu-right" style="will-change: transform;">
                                <a href="javascript: void(0)" class="item-dropdown expandAll" data-target=".collapse-toggle">Plegar todas</a>
                                <a href="javascript: void(0)" class="item-dropdown collapseAll" data-target=".collapse-toggle">Desplegar todas</a>
                            </div>
                        </div>
                    </div>
                    <!-- DATOS BÁSICOS -->
                    <div class="simple-collapse">
                        <a class="collapse-toggle" data-toggle="collapse" href="#collapse-datos-basicos" role="button" aria-expanded="true" aria-controls="collapse-datos-basicos">
                            Datos básicos
                        </a>
                        <div class="collapse show" id="collapse-datos-basicos">
                            <div class="simple-collapse-content">
                                <div class="custom-form-row">
                                    <div class="form-group full-group">
                                        <label class="control-label d-block">Título</label>
                                        <input placeholder="Introduce el titulo" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                </div>
                                <div class="custom-form-row">
                                    <div class="form-group full-group">
                                        <label class="control-label d-block">Descripción</label>
                                        <input placeholder="Introduce una descripción" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                </div>
                                <div class="custom-form-row">
                                    <div class="form-group full-group">
                                        <label class="control-label d-block">URL del repositorio de código</label>
                                        <input placeholder="Introduce la url del repositorio del código" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- END DATOS BÁSICOS -->

                    <!-- INFORMACIÓN -->
                    <div class="simple-collapse">
                        <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-informacion" role="button" aria-expanded="true" aria-controls="collapse-informacion">
                            Información
                        </a>
                        <div class="collapse" id="collapse-informacion">
                            <div class="simple-collapse-content">
                                <div class="custom-form-row">
                                    <div class="form-group ">
                                        <label class="control-label d-block">Lenguajes de programación</label>
                                        <select id="" name="" class="js-select2" data-select-search="true">
                                            <option>Selecciona lenguajes de programación</option>
                                            <option value="Phyton">Phyton</option>
                                            <option value="C#">C#</option>
                                        </select>
                                    </div>
                                    <div class="form-group ">
                                        <label class="control-label d-block">Licencia</label>
                                        <select id="" name="" class="js-select2" data-select-search="true">
                                            <option>Selecciona la licencia</option>
                                            <option value="Pago">Pago</option>
                                            <option value="Gratuito">Gratuito</option>
                                        </select>
                                    </div>
                                    <div class="form-group ">
                                        <label class="control-label d-block">Fecha última actualización</label>
                                        <input title="Fecha de la publicación" type="text" class="ac_input datepicker form-control not-outline" placeholder="Introduce la fecha de publicación" name="" id="" autocomplete="off">
                                    </div>
                                </div>
                                <div class="custom-form-row">
                                    <div class="form-group">
                                        <label class="control-label d-block">Número de branches</label>
                                        <input placeholder="Introduce el número de branches" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label d-block">Número de packages</label>
                                        <input placeholder="Introduce el número de packages" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label d-block">Número de forks</label>
                                        <input placeholder="Introduce el número de forks" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                </div>
                                <div class="custom-form-row">
                                    <div class="form-group">
                                        <label class="control-label d-block">Número de issues</label>
                                        <input placeholder="Introduce el número de issues" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                    <div class="form-group">
                                        <label class="control-label d-block">Número de documento</label>
                                        <input placeholder="Introduce el número de documento" type="text" name="" id="" class="form-control not-outline">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- END INFORMACIÓN -->
                </form>
                <div class="form-actions">
                    <a class="btn btn-primary">GUARDAR</a>
                </div>
            </div>
        </div>
    </div>
</div>