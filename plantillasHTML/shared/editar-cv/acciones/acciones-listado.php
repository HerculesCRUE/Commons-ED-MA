<div class="acciones-listado acciones-listado-cv">
    <div class="wrap">
        <div class="checkAllCVWrapper" id="checkAllCVWrapper">
            <div class="custom-control custom-checkbox">
                <input type="checkbox" class="custom-control-input" id="checkAllCV">
                <label class="custom-control-label" for="checkAllCV">
                </label>
            </div>
            <div class="check-actions-wrap">
                <a href="javascript: void(0);" class="dropdown-toggle check-actions-toggle no-flecha" data-toggle="dropdown">
                    <span class="material-icons">
                        arrow_drop_down
                    </span>
                </a>
                <div class="dropdown-menu basic-dropdown check-actions" id="checkActions">
                    <a href="javascript: void(0)" class="item-dropdown checkall">Todo</a>
                    <a href="javascript: void(0)" class="item-dropdown decheckall">Nada</a>
                </div>
            </div>
        </div>
        <p>Selecciona una o varias publicaciones</p>
    </div>
    <div class="wrap">
        <ul class="no-list-style d-flex align-items-center">
            <li>
                <a class="btn btn-outline-grey" data-toggle="modal" data-target="#modal-anadir-datos-experiencia">
                    <span class="texto">Añadir</span>
                    <span class="material-icons">post_add</span>
                </a>
            </li>
        </ul>
        <div class="ordenar dropdown dropdown-select">
            <a class="dropdown-toggle" data-toggle="dropdown">
                <span class="material-icons">swap_vert</span>
                <span class="texto">Ordenar por fecha</span>
            </a>
            <div class="dropdown-menu basic-dropdown dropdown-menu-right">
                <a href="javascript: void(0)" class="item-dropdown"><span class="texto">Fecha más reciente</span></a>
                <a href="javascript: void(0)" class="item-dropdown"><span class="texto">Fecha más antigua</span></a>
                <a href="javascript: void(0)" class="item-dropdown"><span class="texto">Relevancia</span></a>
            </div>
        </div>
        <?php include './shared/editar-cv/acciones/buscador.php'; ?>
    </div>
</div>