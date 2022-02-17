<div class="perfil-lateral">
    <div class="perfil-lateral-imagen">
        <?php if(isset($imagenUsuario) && $imagenUsuario): ?>
            <img src="./theme/resources/imagenes-pre/foto-usuario.jpg">
        <?php endif; ?>
    </div>
    <div class="perfil-lateral-contenido">
        <div class="perfil-lateral-user-miniatura user-miniatura">
            <div class="nombre-usuario-wrap">
                <p class="nombre">María Elena Alvarado</p>
                <p class="nombre-completo">María Elena Alvarado · Equipo GNOSS</p>
                <p class="localizacion">Logroño - La Rioja - España</p>
            </div>
        </div>
        <div class="perfil-lateral-redes-sociales redes-sociales">
            <p class="group-title">
                Seguir en:
            </p>
            <ul class="lista-redes-sociales no-list-style">
                <li class="twitter">
                    <a href="#" title="Twitter"><i class="fab fa-twitter"></i></a>
                </li>
                <li class="facebook">
                    <a href="#" title="Facebook"><i class="fab fa-facebook-f"></i></a>
                </li>
                <li class="linkedin">
                    <a href="#" title="Linkedin"><i class="fab fa-linkedin-in"></i></a>
                </li>
            </ul>
        </div>
        <div class="perfil-lateral-resumen-usuario resumen-usuario">
            <div class="bloque">
                <div class="titulo">Contribuciones</div>
                <div class="contenido">
                    <a href="./listadoConFiltros.php">17</a>
                </div>
            </div>
            <div class="bloque">
                <div class="titulo">Siguiendo a</div>
                <div class="contenido">
                    <!-- Este no sé si llevará enlace, si no sería sustituir el span por un a  -->
                    <span>35</span>
                </div>
            </div>
            <div class="bloque">
                <div class="titulo">Seguidores</div>
                <div class="contenido">
                    <!-- Este no sé si llevará enlace, si no sería sustituir el span por un a  -->
                    <span>183</span>
                </div>
            </div>
        </div>
    </div>
</div>
