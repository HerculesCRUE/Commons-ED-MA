<div class="user-miniatura">
    <div class="imagen-wrap">
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
    <div class="nombre-wrap">
        <a href="javascript: void(0);">
            <p class="nombre">María Elena Alvarado</p>
            <p class="nombre-completo">María Elena Alvarado · Equipo GNOSS</p>
        </a>
    </div>
</div>