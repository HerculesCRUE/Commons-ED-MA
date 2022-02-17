<div id="modal-anadir-item-cv" class="modal modal-top fade modal-alertas" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <p class="modal-title"><span class="material-icons">post_add</span>Añadir</p>
                <span class="material-icons cerrar" data-dismiss="modal" aria-label="Close">close</span>
            </div>
            <div class="modal-body">
                <form class="formulario-edicion formulario-alertas">
                    <p>El item se incorporará a tu CV</p>
                    <div class="form-group">
                        <label class="control-label">¿Deseas también hacerlo público y/o enviarlo a producción científica?</label>
                    </div>
                    <div class="custom-control custom-checkbox mb-4">
                        <input type="checkbox" id="solicitar-validacion" name="solicitar-validacion" class="custom-control-input">
                        <label class="custom-control-label texto-gris-claro" for="solicitar-validacion">Solicitar validación de la Universidad</label>
                    </div>
                    <div class="form-group mb-4">
                        <div class="image-uploader js-image-uploader">
                            <div class="image-uploader__preview">
                                <img class="image-uploader__img" alt="Imagen Usuario" src="">
                            </div>
                            <div class="image-uploader__drop-area">
                                <div class="image-uploader__icon">
                                    <span class="material-icons">backup</span>
                                </div>
                                <div class="image-uploader__info">
                                    <p><strong>Arrastra y suelta en la zona punteada los documentos acreditativos</strong></p>
                                    <p>Imágenes en formato .PNG o .JPG</p>
                                    <p>Peso máximo de las imágenes 250 kb</p>
                                </div>
                            </div>
                            <div class="image-uploader__error">
                                <p class="ko"></p>
                            </div>
                        </div>
                        <input type="file" class="image-uploader__input" accept="image/*" />
                    </div>
                    <div class="custom-control custom-checkbox mb-4">
                        <input type="checkbox" id="hacer-publico" name="hacer-publico" class="custom-control-input">
                        <label class="custom-control-label texto-gris-claro" for="hacer-publico">Hacerlo público</label>
                    </div>
                </form>
                <div class="form-actions">
                    <a class="btn btn-primary uppercase">Añadir</a>
                    <a class="btn btn-link uppercase" data-dismiss="modal">Cancelar</a>
                </div>
            </div>
        </div>
    </div>
</div>