<div class="contenido-cv">
    <div class="tab-paneles-cv">
        <ul class="nav nav-tabs" id="navegacion-cv" role="tablist">
            <li class="nav-item" role="presentation">
                <a class="nav-link active" id="identificacion-tab" data-toggle="tab" href="#identificacion-panel" role="tab" aria-controls="identificacion-panel" aria-selected="true">Datos de identificación</a>
            </li>
            <li class="nav-item" role="presentation">
                <a class="nav-link" id="situacion-tab" data-toggle="tab" href="#situacion-panel" role="tab" aria-controls="situacion-panel" aria-selected="true">Situación profesional</a>
            </li>
            <li class="nav-item" role="presentation">
                <a class="nav-link" id="formacion-tab" data-toggle="tab" href="#formacion-panel" role="tab" aria-controls="formacion-panel" aria-selected="true">Formación recibida</a>
            </li>
            <li class="nav-item" role="presentation">
                <a class="nav-link" id="actividad-tab" data-toggle="tab" href="#actividad-panel" role="tab" aria-controls="actividad-panel" aria-selected="true">Actividad docente</a>
            </li>
            <li class="nav-item" role="presentation">
                <a class="nav-link " id="experiencia-tab" data-toggle="tab" href="#experiencia-panel" role="tab" aria-controls="experiencia-panel" aria-selected="true">Experiencia científica y tecnológica</a>
            </li>
            <li class="nav-item" role="presentation">
                <a class="nav-link" id="resumen-tab" data-toggle="tab" href="#resumen-panel" role="tab" aria-controls="resumen-panel" aria-selected="true">Resumen de texto libre</a>
            </li>
        </ul>
        <div class="tab-content" id="paneles-cv">
            <div class="tab-pane fade show active" id="identificacion-panel" role="tabpanel" aria-labelledby="identificacion-tab">
                <?php include './shared/editar-cv/tabs/identificacion.php'; ?>
            </div>
            <div class="tab-pane fade" id="situacion-panel" role="tabpanel" aria-labelledby="situacion-tab">
                <?php include './shared/editar-cv/tabs/situacion.php'; ?>
            </div>
            <div class="tab-pane fade" id="formacion-panel" role="tabpanel" aria-labelledby="formacion-tab">
                <?php include './shared/editar-cv/tabs/formacion.php'; ?>
            </div>
            <div class="tab-pane fade" id="actividad-panel" role="tabpanel" aria-labelledby="actividad-tab">
                <?php include './shared/editar-cv/tabs/actividad.php'; ?>
            </div>
            <div class="tab-pane fade " id="experiencia-panel" role="tabpanel" aria-labelledby="experiencia-tab">
                <?php include './shared/editar-cv/tabs/experiencia.php'; ?>
            </div>
            <div class="tab-pane fade" id="resumen-panel" role="tabpanel" aria-labelledby="resumen-tab">
                <?php include './shared/editar-cv/tabs/resumen.php'; ?>
            </div>
        </div>
    </div>
</div>