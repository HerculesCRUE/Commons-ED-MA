<div class="acciones-listado acciones-listado-gestion">
    <div class="wrap">
        <div class="checkAllGestionWrapper" id="checkAllGestionWrapper">
            <div class="custom-control custom-checkbox">
                <input type="checkbox" class="custom-control-input" id="checkAllResources">
                <label class="custom-control-label" for="checkAllResources">
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
                    <a href="javascript: void(0)" class="item-dropdown checkAllRead">Leído</a>
                    <a href="javascript: void(0)" class="item-dropdown checkAllNonRead">No leído</a>
                </div>
            </div>
        </div>
        <div class="acciones">
            <ul class="no-list-style d-flex align-items-center">
                    <li>
                        <a class="btn btn-outline-grey" href="javascript: void(0);">
                        <span class="texto">Publicar</span>
                        <span class="material-icons">lock_open</span>
                    </a>
                </li>
                <li>
                    <a class="btn btn-outline-grey" href="javascript: void(0);">
                        <span class="texto">Despublicar</span>
                        <span class="material-icons">lock</span>
                    </a>
                </li>
                <li>
                    <a class="btn btn-outline-grey" href="javascript: void(0);">
                        <span class="texto">Eliminar</span>
                        <span class="material-icons">delete</span>
                    </a>
                </li>
                <li>
                    <div class="dropdown">
                        <a href="#" class="dropdown-toggle btn btn-outline-grey" role="button" id="dropdownEnriquecimiento" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            Enriquecimiento
                        </a>
                        <div class="dropdown-menu basic-dropdown dropdown-menu-right" aria-labelledby="dropdownEnriquecimiento">
                            <ul class="no-list-style">
                                <li>
                                    <a class="item-dropdown" href="javascript: void(0);">
                                        <span class="texto">Aceptar</span>
                                        <span class="material-icons-outlined">thumb_up_alt</span>
                                    </a>
                                </li>
                                <li>
                                    <a class="item-dropdown" href="javascript: void(0);">
                                        <span class="texto">Rechazar</span>
                                        <span class="material-icons-outlined">thumb_down</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="wrap">
        <div class="ordenar dropdown dropdown-select">
            <a class="dropdown-toggle" data-toggle="dropdown">
                <span class="material-icons">sort</span>
                <span class="texto">Ordenar</span>
            </a>
            <div class="dropdown-menu basic-dropdown dropdown-menu-right">
                <a href="javascript: void(0)" class="item-dropdown">Fecha más reciente</a>
                <a href="javascript: void(0)" class="item-dropdown">Fecha más antigua</a>
                <a href="javascript: void(0)" class="item-dropdown">Relevancia</a>
            </div>
        </div>
        <div class="buscar">
            <form method="post" action="http://devlariojaturismo.gnoss.com/ver-y-hacer?default;rdf:type=destination&harmonise:region=logro%C3%B1o%20y%20alrededores">
                <fieldset>
                    <legend>Buscar</legend>
                    <div class="finderUtils">
                        <div class="group finderSection">
                            <label for="finderSection">Encontrar</label>
                            <input type="text" class="not-outline finderSectionText autocompletar" autocomplete="off" placeholder="Buscar...">
                            <input type="hidden" class="inpt_parametros">
                        </div>
                    </div>
                </fieldset>
            </form>
        </div>
    </div>
</div>