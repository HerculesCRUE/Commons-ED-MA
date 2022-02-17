<aside id="menuLateralComunidad" class="menuLateral comunidad" role="navigation">
    <div class="header">
        <div class="wrap-header">
            <div class="menu-logo-wrapper">
                <div class="menu-toggle">
                    <a href="javascript: void(0);" class="cerrar texto-blanco">
                        <span class="material-icons">menu</span>
                    </a>
                </div>
                <div class="logo-wrapper">
                    <a href="./home.php" class="texto-blanco">
                        <?= isset($nombreComunidad) ? $nombreComunidad : 'Nombre  comunidad comunidadcomunidad comunidadcomunidad' ?>
                    </a>
                </div>
            </div>
        </div>
        <span class="material-icons cerrar">close</span>
    </div>

    <div class="body pmd-scrollbar mCustomScrollbar" data-mcs-theme="minimal-dark">
        <div id="community-menu-movil">
            <p class="title">Navegación</p>
            <ul class="nav pmd-sidebar-nav">
                <li class="">
                    <a href="./listado.php">Home</a>
                </li>
                <li class="">
                    <a class="./listado.php" href="#">Índice</a>
                </li>
                <li class="active">
                    <a href="./listado.php">Recursos</a>
                </li>
                <li class="">
                    <a href="./listado.php">Debates</a>
                </li>
                <li>
                    <a href="./listado.php">Preguntas</a>
                </li>
                <li>
                    <a href="./listado.php">Encuestas</a>
                </li>
                <li>
                    <a href="./listadoPersonas.php">Personas y organizaciones</a>
                </li>
            </ul>
        </div>
        </ul>
    </div>
</aside>