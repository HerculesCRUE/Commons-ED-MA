<div class="col col-12 col-section-title font-weight-bold">
    <div class="wrapCol">
        <p>Evolución temporal publicaciones</p>
    </div>
</div>
<div class="col col-12 col-chart">
    <div class="wrapCol">
        <?php include './shared/investigador/charts/chart-publicaciones.php'; ?>
    </div>
</div>
<div class="col col-12 col-section-title font-weight-bold">
    <div class="wrapCol">
        <p>Listado de publicaciones</p>
    </div>
</div>
<div class="col col-12 col-xl-3 col-facetas col-lateral izquierda">
    <div class="wrapCol">
        <?php include './shared/facetas.php'; ?>
    </div>
</div>
<div class="col col-12 col-xl-9 col-contenido derecha">
    <div class="wrapCol">
        <div class="header-contenido">
            <div class="h1-container">
                <h1>Publicaciones <span class="numResultados">2400</span></h1>
            </div>
            <?php include './shared/acciones/acciones-listado-compactado.php'; ?>
            <?php include './shared/filtros-listado.php'; ?>
        </div>
        <?php include './shared/listado-recursos-publicaciones.php'; ?>
    </div>
</div>