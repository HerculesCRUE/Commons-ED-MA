<div class="contenido-ficha">
    <div class="tab-paneles-ficha">
        <div class="tabs">
            <ul class="nav nav-tabs" id="navegacion-recurso" role="tablist">
                <li class="nav-item" role="presentation">
                    <a class="nav-link active" id="referencias-tab" data-toggle="tab" href="#referencias-panel" role="tab" aria-controls="referencias-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">Referencias</div>
                            <div class="data">25</div>
                        </div>
                    </a>
                </li>
                <li class="nav-item" role="presentation">
                    <a class="nav-link" id="publicaciones-relacionadas-tab" data-toggle="tab" href="#publicaciones-relacionadas-panel" role="tab" aria-controls="publicaciones-relacionadas-panel" aria-selected="true">
                        <div class="bloque">
                            <div class="label">Publicaciones relacionadas</div>
                            <div class="data">100</div>
                        </div>
                    </a>
                </li>
            </ul>
        </div>
        <div class="tab-content" id="paneles-recurso">
            <div class="tab-pane fade show active" id="referencias-panel" role="tabpanel" aria-labelledby="referencias-tab">
                <div class="row">
                    <?php include './shared/grafico/tabs/referencias.php'; ?>
                </div>
            </div>
            <div class="tab-pane fade" id="publicaciones-relacionadas-panel" role="tabpanel" aria-labelledby="publicaciones-relacionadas-tab">
                <div class="row">
                    <?php include './shared/publicacion/tabs/publicaciones-relacionadas.php'; ?>
                </div>
            </div>
        </div>
    </div>
</div>