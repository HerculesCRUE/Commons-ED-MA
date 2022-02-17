<div class="row community-row">
    <div class="container community-menu-wrapper">
        <a href="javascript: void(0);" data-target="menuLateralComunidad" id="menuLateralComunidadTrigger" class=" texto-blanco">
            <span class="material-icons">menu</span>
        </a>
        <div class="page-name-wrapper">
            <?php
                if (isset($nombreComunidad)){
                    print "<span class='page-name'>$nombreComunidad</span>";
                }else{
                    print "<span class='page-name'>Nombre comunidad</span>";
                }
            ?>
        </div>
        <div id="community-menu" class="">
            <ul class="">
                <li class="active">
                    <a href="./listadoPublicaciones.php">Publicaciones</a>
                </li>
                <li>
                    <a href="./listadoInvestigadores.php">Investigadores</a>
                </li>
                <li>
                    <a href="./listado.php">Centro de investigación</a>
                </li>
                <li>
                    <a href="./listado.php">Proyectos</a>
                </li>
                <li>
                    <a href="./listado.php">ROs</a>
                </li>
            </ul>
        </div>
    </div>
    <?php include './shared/menuLateralComunidad.php'; ?>
</div>