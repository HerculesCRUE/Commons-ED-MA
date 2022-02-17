<div class="resource-list usuarios con-borde">
    <article class="resource resource-grupo">
        <div class="user-miniatura">
            <div class="imagen-usuario-wrap">
                <a href="./fichaPerfil.php">
                    <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                        <div class="imagen">
                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario-3.jpg)"></span>
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
                    <p class="nombre">Roberto Alvarado</p>
                    <p class="nombre-completo">Roberto Alvarado García · Equipo DIDACTALIA</p>
                </a>
            </div>
        </div>
    </article>
    <article class="resource resource-grupo">
        <div class="user-miniatura">
            <div class="imagen-usuario-wrap">
                <a href="./fichaPerfil.php">
                    <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
                        <div class="imagen">
                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario-2.jpg)"></span>
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
                    <p class="nombre">Tomás Torres</p>
                    <p class="nombre-completo">Tomás Torres García · Equipo GNOSS</p>
                </a>
            </div>
        </div>
    </article>
    <article class="resource resource-grupo">
        <div class="user-miniatura">
            <div class="imagen-usuario-wrap">
                <a href="./fichaPerfil.php">
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
                    <p class="nombre">María Elena Alvarado</p>
                    <p class="nombre-completo">María Elena Alvarado · Equipo GNOSS</p>
                </a>
            </div>
        </div>
    </article>
    <a href="./listadoGrupos.php" class="ver-mas ver-mas-icono con-icono-after">Ver todos</a>
</div>