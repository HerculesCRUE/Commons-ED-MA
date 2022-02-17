<div class="contenido-ficha">
    <div class="tab-paneles-ficha">
        <div class="tabs">
            <ul class="nav nav-tabs" id="navegacion-recurso" role="tablist">
                <li class="nav-item" role="presentation">
                    <a class="nav-link active" id="informacion-tab" data-toggle="tab" href="#informacion-panel" role="tab" aria-controls="informacion-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">Información</div>
                            <div class="data"></div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="citado-tab" data-toggle="tab" href="#citado-panel" role="tab" aria-controls="citado-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">Citado por</div>
                            <div class="data">240</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="area-tematicas-tab" data-toggle="tab" href="#area-tematicas-panel" role="tab" aria-controls="area-tematicas-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">Área temáticas de las citas</div>
                            <div class="data">60</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="ros-relacionados-tab" data-toggle="tab" href="#ros-relacionados-panel" role="tab" aria-controls="ros-relacionados-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">ROs relacionados</div>
                            <div class="data">20</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="anotaciones-tab" data-toggle="tab" href="#anotaciones-panel" role="tab" aria-controls="anotaciones-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">Anotaciones</div>
                            <div class="data">20</div>
                        </div>
                    </a>
                </li>
            </ul>
        </div>
        <div class="tab-content" id="paneles-recurso">
            <div class="tab-pane fade show active" id="informacion-panel" role="tabpanel" aria-labelledby="informacion-tab">
                <div class="row">
                    <?php include './shared/ros/tabs/informacion.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="citado-panel" role="tabpanel" aria-labelledby="citado-tab">
                <div class="row">
                    <?php include './shared/publicacion/tabs/referencias.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="area-tematicas-panel" role="tabpanel" aria-labelledby="area-tematicas-tab">
                <div class="row">
                    <?php include './shared/ros/tabs/area-tematicas.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="ros-relacionados-panel" role="tabpanel" aria-labelledby="ros-relacionados-tab">
                <div class="row">
                    <?php include './shared/ros/tabs/ros-relacionados.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="anotaciones-panel" role="tabpanel" aria-labelledby="anotaciones-tab">
                <div class="row">
                    <?php include './shared/ros/tabs/anotaciones.php'; ?>
                </div>
            </div>
        </div>
    </div>
</div>