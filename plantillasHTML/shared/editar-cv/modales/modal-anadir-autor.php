<div id="modal-anadir-autor" class="modal modal-top fade" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <p class="modal-title"><span class="material-icons">person_add</span>Añadir personal investigador</p>
                <span class="material-icons cerrar" data-dismiss="modal" aria-label="Close">close</span>
            </div>
            <div class="modal-body">
                <form class="formulario-edicion formulario-principal" style="display: block;">
                    <div class="custom-form-row">
                        <div class="form-group full-group">
                            <label class="control-label d-block">Buscar por firma</label>
                            <input placeholder="Introduce una o varias firmas separadas por comas" type="text" name="" id="" class="form-control not-outline">
                            <a href="javascript: void(0);" class="btn btn-outline-grey">Validar</a>
                        </div>
                    </div>
                    <div class="custom-form-row resultados" style="display: block;">
                        <div class="form-group full-group">
                            <div class="simple-collapse">
                                <label class="control-label d-block">Luis Pérez</label>
                                <div class="user-miniatura">
                                    <div class="custom-control custom-checkbox">
                                        <input type="checkbox" id="user-1" name="user-1" class="custom-control-input">
                                        <label class="custom-control-label" for="user-1"></label>
                                    </div>
                                    <div class="imagen-usuario-wrap">
                                        <a href="javascript: void(0);">
                                            <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                                                <div class="imagen">
                                                    <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                </div>
                                            <?php else: ?>
                                                <div class="imagen sinImagen">
                                                    <span class="material-icons">person</span>
                                                </div>
                                            <?php endif ?>
                                        </a>
                                    </div>
                                    <div class="nombre-usuario-wrap">
                                        <a href="./fichaPerfil.php">
                                            <p class="nombre">Luis Pérez García</p>
                                            <p class="nombre-completo">Dpto. Informática · 0000-0002-6463-3216</p>
                                        </a>
                                    </div>
                                    <div class="coincidencia-wrap">
                                        <p class="label">Coincidencia</p>
                                        <p class="numResultado" style="color: green;">90%</p>
                                    </div>
                                </div>
                                <a href="#groupCollapse1" data-toggle="collapse" aria-expanded="true" class="collapse-toggle collapsed">Más resultados</a>
                                <div id="groupCollapse1" class="collapse-wrap collapse">
                                    <div class="user-miniatura">
                                        <div class="custom-control custom-checkbox">
                                            <input type="checkbox" id="user-3" name="user-3" class="custom-control-input">
                                            <label class="custom-control-label" for="user-3"></label>
                                        </div>
                                        <div class="imagen-usuario-wrap">
                                            <a href="javascript: void(0);">
                                                <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                                                    <div class="imagen">
                                                        <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                    </div>
                                                <?php else: ?>
                                                    <div class="imagen sinImagen">
                                                        <span class="material-icons">person</span>
                                                    </div>
                                                <?php endif ?>
                                            </a>
                                        </div>
                                        <div class="nombre-usuario-wrap">
                                            <a href="./fichaPerfil.php">
                                                <p class="nombre">Luis Pérez García</p>
                                                <p class="nombre-completo">Dpto. Informática · 0000-0002-6463-3216</p>
                                            </a>
                                        </div>
                                        <div class="coincidencia-wrap">
                                            <p class="label">Coincidencia</p>
                                            <p class="numResultado" style="color: green;">90%</p>
                                        </div>
                                    </div>
                                    <div class="form-actions">
                                        <a href="javascript: void(0);" class="form-buscar">Buscar</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group full-group">
                            <div class="simple-collapse">
                                <label class="control-label d-block">L. García</label>
                                <div class="user-miniatura">
                                    <div class="custom-control custom-checkbox">
                                        <input type="checkbox" id="user-2" name="user-2" class="custom-control-input">
                                        <label class="custom-control-label" for="user-2"></label>
                                    </div>
                                    <div class="imagen-usuario-wrap">
                                        <a href="javascript: void(0);">
                                            <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                                                <div class="imagen">
                                                    <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                </div>
                                            <?php else: ?>
                                                <div class="imagen sinImagen">
                                                    <span class="material-icons">person</span>
                                                </div>
                                            <?php endif ?>
                                        </a>
                                    </div>
                                    <div class="nombre-usuario-wrap">
                                        <a href="./fichaPerfil.php">
                                            <p class="nombre">Lorenzo García Sánchez</p>
                                            <p class="nombre-completo">Dpto. Informática · 0000-0002-6463-3216</p>
                                        </a>
                                    </div>
                                    <div class="coincidencia-wrap">
                                        <p class="label">Coincidencia</p>
                                        <p class="numResultado" style="color: orange;">60%</p>
                                    </div>
                                </div>
                                <a href="#groupCollapse2" data-toggle="collapse" aria-expanded="true" class="collapse-toggle collapsed">Más resultados</a>
                                <div id="groupCollapse2" class="collapse-wrap collapse">
                                    <div class="user-miniatura">
                                        <div class="custom-control custom-checkbox">
                                            <input type="checkbox" id="user-4" name="user-4" class="custom-control-input">
                                            <label class="custom-control-label" for="user-4"></label>
                                        </div>
                                        <div class="imagen-usuario-wrap">
                                            <a href="javascript: void(0);">
                                                <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                                                    <div class="imagen">
                                                        <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                    </div>
                                                <?php else: ?>
                                                    <div class="imagen sinImagen">
                                                        <span class="material-icons">person</span>
                                                    </div>
                                                <?php endif ?>
                                            </a>
                                        </div>
                                        <div class="nombre-usuario-wrap">
                                            <a href="./fichaPerfil.php">
                                                <p class="nombre">Javier Lluis Martínez García</p>
                                                <p class="nombre-completo">Dpto. Informática · 0000-0002-6463-3216</p>
                                            </a>
                                        </div>
                                        <div class="coincidencia-wrap">
                                            <p class="label">Coincidencia</p>
                                            <p class="numResultado" style="color: orange;">55%</p>
                                        </div>
                                    </div>
                                    <div class="user-miniatura">
                                        <div class="custom-control custom-checkbox">
                                            <input type="checkbox" id="user-5" name="user-5" class="custom-control-input">
                                            <label class="custom-control-label" for="user-5"></label>
                                        </div>
                                        <div class="imagen-usuario-wrap">
                                            <a href="javascript: void(0);">
                                                <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                                                    <div class="imagen">
                                                        <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                    </div>
                                                <?php else: ?>
                                                    <div class="imagen sinImagen">
                                                        <span class="material-icons">person</span>
                                                    </div>
                                                <?php endif ?>
                                            </a>
                                        </div>
                                        <div class="nombre-usuario-wrap">
                                            <a href="./fichaPerfil.php">
                                                <p class="nombre">Luis Pérez García</p>
                                                <p class="nombre-completo">Dpto. Informática · 0000-0002-6463-3216</p>
                                            </a>
                                        </div>
                                        <div class="coincidencia-wrap">
                                            <p class="label">Coincidencia</p>
                                            <p class="numResultado" style="color: red;">20%</p>
                                        </div>
                                    </div>
                                    <div class="form-actions">
                                        <a href="javascript: void(0);" class="form-buscar">Buscar</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group full-group">
                            <label class="control-label d-block">X. Lee</label>
                            <div class="user-miniatura">
                                <div class="imagen-usuario-wrap">
                                    <a href="javascript: void(0);">
                                        <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                                            <div class="imagen">
                                                <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                            </div>
                                        <?php else: ?>
                                            <div class="imagen sinImagen">
                                                <span class="material-icons">person</span>
                                            </div>
                                        <?php endif ?>
                                    </a>
                                </div>
                                <div class="nombre-usuario-wrap">
                                    <a href="./fichaPerfil.php">
                                        <p class="nombre">Ninguna sugerencia</p>
                                    </a>
                                </div>
                                <div class="coincidencia-wrap">
                                    <a href="javascript: void(0);" class="form-buscar">Buscar</a>
                                </div>
                            </div>
                        </div>
                        <div class="form-actions">
                            <a href="javascript: void(0);" class="btn btn-primary uppercase" data-dismiss="modal">Añadir seleccionados</a>
                        </div>
                    </div>
                </form>
                <form class="formulario-edicion formulario-codigo" style="display: none;">
                    <legend>Buscar por</legend>
                    <div class="custom-form-row">
                        <div class="form-group full-group">
                            <label class="control-label d-block">Código ORCID</label>
                            <input placeholder="Introduce el código ORCID" type="text" name="" id="" class="form-control not-outline">
                            <a href="javascript: void(0);" class="btn btn-outline-grey">Validar</a>
                        </div>
                    </div>
                    <a href="javascript: void(0);" class="form-autor">O teclea los datos de X. Lee</a>
                    <div class="botones-wrap">
                        <a href="javascript: void(0);" class="volver form-principal">
                            <span class="material-icons">keyboard_arrow_left</span>
                            Volver a la pantalla anterior
                        </a>
                    </div>
                </form>
                <form class="formulario-edicion formulario-autor" style="display: none;">
                    <div class="custom-form-row">
                        <div class="form-group expand-2">
                            <label class="control-label d-block">Código ORCID</label>
                            <input placeholder="Introduce el código ORCID" type="text" name="" id="" class="form-control not-outline">
                        </div>
                    </div>
                    <div class="custom-form-row">
                        <div class="form-group full-group">
                            <label class="control-label d-block">Nombre</label>
                            <input placeholder="Introduce el nombre" type="text" name="" id="" class="form-control not-outline">
                        </div>
                    </div>
                    <div class="custom-form-row">
                        <div class="form-group full-group">
                            <label class="control-label d-block">Primer apellido</label>
                            <input placeholder="Introduce el primer apellido" type="text" name="" id="" class="form-control not-outline">
                        </div>
                    </div>
                    <div class="custom-form-row">
                        <div class="form-group full-group">
                            <label class="control-label d-block">Segundo apellido</label>
                            <input placeholder="Introduce el segundo apellido" type="text" name="" id="" class="form-control not-outline">
                        </div>
                    </div>
                    <div class="custom-form-row">
                        <div class="form-group expand-2">
                            <label class="control-label d-block">Posición</label>
                            <input placeholder="Introduce la posición" type="text" name="" id="" value="3" class="form-control not-outline" disabled>
                        </div>
                    </div>
                    <div class="custom-form-row">
                        <div class="form-group full-group">
                            <label class="control-label d-block">Firma</label>
                            <input placeholder="Introduce la firma" type="text" name="" id="" value="X. Lee" class="form-control not-outline" disabled>
                        </div>
                    </div>
                    <div class="form-actions">
                        <a class="btn btn-primary uppercase" data-dismiss="modal">Guardar</a>
                        <a href="javascript: void(0);" class="volver form-buscar">
                            <span class="material-icons">keyboard_arrow_left</span>
                            Volver a la pantalla anterior
                        </a>
                    </div>
                </form>
            </div>
        </div>
    </div>
</div>