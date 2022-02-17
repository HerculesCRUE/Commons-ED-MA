<div class="chart-wrap">
    <div class="acciones-grafico">
        <div class="periodo dropdown dropdown-select">
            <a href="#" class="dropdown-toggle btn btn-outline-grey" role="button" id="dropdownPeriodo" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                Periodo temporal:
                <span class="texto">Todos</span>
            </a>
            <div class="dropdown-menu basic-dropdown dropdown-menu-left" aria-labelledby="dropdownPeriodo">
                <p class="dropdown-title">Personalizado</p>
                <div class="faceta-date-range">
                    <div id="gmd_ci_daterange" class="ui-slider ui-corner-all ui-slider-horizontal ui-widget ui-widget-content">
                        <div class="ui-slider-range ui-corner-all ui-widget-header"></div>
                        <span tabindex="0" class="ui-slider-handle ui-corner-all ui-state-default"></span>
                        <span tabindex="0" class="ui-slider-handle ui-corner-all ui-state-default"></span>
                    </div>
                    <div class="d-flex">
                        <input title="Año" type="number" min="0" max="2021" autocomplete="off" class="filtroFacetaFecha hasDatepicker" placeholder="Año inicio" name="gmd_ci_datef1" id="gmd_ci_datef1" onchange="actualizarValoresSlider(this,0)">
                        <input title="Año" type="number" min="0" max="2021" autocomplete="off" class="filtroFacetaFecha hasDatepicker" placeholder="Año fin" name="gmd_ci_datef2" id="gmd_ci_datef2" onchange="actualizarValoresSlider(this,1)">
                        <a name="gmd_ci:date" class="searchButton">Aplicar</a>
                    </div>
                    <ul class="no-list-style">
                        <li>
                            <a href="javascript: void(0);" class="item-dropdown">
                                <span class="textoAlternativo">Periodo temporal:</span>
                                <span class="texto">Últimos cinco años</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript: void(0);" class="item-dropdown">
                                <span class="textoAlternativo">Periodo temporal:</span>
                                <span class="texto">Últimos año</span>
                            </a>
                        </li>
                        <li>
                            <a href="javascript: void(0);" class="item-dropdown activeView">
                                <span class="textoAlternativo">Periodo temporal:</span>
                                <span class="texto">Todos</span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="vista-red dropdown dropdown-select">
            <a href="#" class="dropdown-toggle btn btn-outline-grey" role="button" id="dropdownVistaRed" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                Vista red:
                <span class="texto">Item 1</span>
            </a>
            <div class="dropdown-menu basic-dropdown dropdown-menu-right" aria-labelledby="dropdownVistaRed">
                <ul class="no-list-style">
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown activeView">
                            <span class="textoAlternativo">Vista red:</span>
                            <span class="texto">Item 1</span>
                        </a>
                    </li>
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown">
                            <span class="textoAlternativo">Vista red:</span>
                            <span class="texto">Item 2</span>
                        </a>
                    </li>
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown">
                            <span class="textoAlternativo">Vista red:</span>
                            <span class="texto">Item 3</span>
                        </a>
                    </li>
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown">
                            <span class="textoAlternativo">Vista red:</span>
                            <span class="texto">Item 4</span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
        <div class="temas dropdown dropdown-select">
            <a href="#" class="dropdown-toggle btn btn-outline-grey" role="button" id="dropdownTemas" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                Temas:
                <span class="texto">Item 1</span>
            </a>
            <div class="dropdown-menu basic-dropdown dropdown-menu-right" aria-labelledby="dropdownTemas">
                <ul class="no-list-style">
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown activeView">
                            <span class="textoAlternativo">Temas:</span>
                            <span class="texto">Item 1</span>
                        </a>
                    </li>
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown">
                            <span class="textoAlternativo">Temas:</span>
                            <span class="texto">Item 2</span>
                        </a>
                    </li>
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown">
                            <span class="textoAlternativo">Temas:</span>
                            <span class="texto">Item 3</span>
                        </a>
                    </li>
                    <li>
                        <a href="javascript: void(0);" rel="nofollow" class="item-dropdown">
                            <span class="textoAlternativo">Temas:</span>
                            <span class="texto">Item 4</span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="contenido-grafico container">
        <div class="row">
            <div class="col col-12 col-md-8">
                <div class="grafico-wrap">
                    <p>Gráfico 1</p>
                    <div class="graph-controls">
                        <ul class="no-list-style d-flex align-items-center">
                            <li class="control remove-control">
                                <span class="material-icons">remove</span>
                            </li>
                            <li class="control text-control">
                                10 nodos
                            </li>
                            <li class="control add-control">
                                <span class="material-icons">add</span>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col col-12 col-md-4">
                <div class="grafico-wrap">
                    <p>Gráfico 2</p>
                </div>
                <div class="grafico-wrap">
                    <p>Gráfico 3</p>
                </div>
            </div>
        </div>
    </div>
</div>