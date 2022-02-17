<!-- AÑADIR CLASE contenido-ficha -->
<div class="contenido-ficha">
    <div class="tab-paneles-ficha">
        <div class="tabs">
            <ul class="nav nav-tabs" id="navegacion-recurso" role="tablist">
                <li class="nav-item" role="presentation">
                    <a class="nav-link active" id="publicaciones-tab" data-toggle="tab" href="#publicaciones-panel" role="tab" aria-controls="publicaciones-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">Publicaciones</div>
                            <div class="data">2540</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="temas-tab" data-toggle="tab" href="#temas-panel" role="tab" aria-controls="temas-panel" aria-selected="false">
                        <div class="bloque">
                            <div class="label">Temas de investigación</div>
                            <div class="data">254</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="colaboradores-tab" data-toggle="tab" href="#colaboradores-panel" role="tab" aria-controls="colaboradores-panel" aria-selected="false">
                        <div class="bloque">
                            <div class="label">Red colaboradores</div>
                            <div class="data">60</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="proyectos-tab" data-toggle="tab" href="#proyectos-panel" role="tab" aria-controls="proyectos-panel" aria-selected="false">
                        <div class="bloque">
                            <div class="label">Proyectos</div>
                            <div class="data">32</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="ros-tab" data-toggle="tab" href="#ros-panel" role="tab" aria-controls="ros-panel" aria-selected="false">
                        <div class="bloque">
                            <div class="label">ROs</div>
                            <div class="data">632</div>
                        </div>
                    </a>
                </li>
            </ul>
        </div>
        <div class="tab-content" id="paneles-recurso">
            <div class="tab-pane fade show active" id="publicaciones-panel" role="tabpanel" aria-labelledby="publicaciones-tab">
                <div class="row">
                    <?php include './shared/investigador/tabs/publicaciones.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="temas-panel" role="tabpanel" aria-labelledby="temas-tab">
                <div class="row">
                    <?php include './shared/investigador/tabs/temas.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="colaboradores-panel" role="tabpanel" aria-labelledby="colaboradores-tab">
                <div class="row">
                    <?php include './shared/investigador/tabs/colaboradores.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="proyectos-panel" role="tabpanel" aria-labelledby="proyectos-tab">
                <div class="row">
                </div>
            </div>
            <div class="tab-pane fade" id="ros-panel" role="tabpanel" aria-labelledby="ros-tab">
                <div class="row">
                </div>
            </div>
        </div>
    </div>
</div>