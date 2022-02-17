<div class="tab-paneles-alertas">
    <ul class="nav nav-tabs" id="navegacion-alertas" role="tablist">
        <li class="nav-item" role="presentation">
            <a class="nav-link active" id="incorporar-tab" data-toggle="tab" href="#incorporar-panel" role="tab" aria-controls="incorporar-panel" aria-selected="true">Incorporar a CV <span class="badge">12</span></a>
        </li>
        <li class="nav-item" role="presentation">
            <a class="nav-link" id="validar-tab" data-toggle="tab" href="#validar-panel" role="tab" aria-controls="validar-panel" aria-selected="true">Validar enriquecimiento <span class="badge">2</span></a>
        </li>
        <li class="nav-item" role="presentation">
            <a class="nav-link" id="notificaciones-tab" data-toggle="tab" href="#notificaciones-panel" role="tab" aria-controls="notificaciones-panel" aria-selected="true">Notificaciones Universidad <span class="badge">3</span></a>
        </li>
    </ul>
    <div class="tab-content" id="paneles-alertas">
        <div class="tab-pane fade show active" id="incorporar-panel" role="tabpanel" aria-labelledby="incorporar-tab">
            <?php include './shared/alertas/listado/listado-incorporar-cv.php'; ?>
        </div>
        <div class="tab-pane fade show" id="validar-panel" role="tabpanel" aria-labelledby="validar-tab">
        <?php include './shared/alertas/listado/listado-validar-enriquecimiento.php'; ?>
        </div>
        <div class="tab-pane fade show" id="notificaciones-panel" role="tabpanel" aria-labelledby="notificaciones-tab">
            <?php include './shared/alertas/listado/listado-notificaciones-universidad.php'; ?>
        </div>
    </div>
</div>