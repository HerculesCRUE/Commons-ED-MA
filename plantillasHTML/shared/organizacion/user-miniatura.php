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
        <a href="./fichaOrganizacion.php">
            <p class="nombre">Equipo GNOSS</p>
            <p class="localizacion">Logroño - La Rioja - España</p>
        </a>
    </div>
</div>