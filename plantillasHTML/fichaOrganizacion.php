<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Ficha perfil";
    $clasePagina = "fichaPerfil";
    $invitado = false;
    $comunidad = true;
    $listado = false;
    $imagenUsuario = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Ficha Recurso</title>

    <?php include './shared/style.php'; ?>
    <?php include './shared/script.php'; ?>
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta name="robots" content="noindex,nofollow">
</head>

<body class="<?php include './shared/clasesBody.php'; ?>" cz-shortcut-listen="true">

    <?php
    include './shared/formsGnoss.php';
    include './shared/cabecera.php';
    ?>

    <main role="main">
        <div class="container">
            <div class="row-content">
                <div class="row">
                    <div class="col col-12 col-breadcrumb">
                        <ul>
                            <li>
                                <a href="./home.php">Home</a>
                            </li>
                            <li>
                                <a href="javascript: void(0);">Personas y organizaciones</a>
                            </li>
                            <li>
                                Ana Moreno
                            </li>
                        </ul>
                    </div>
                    <div class="col col-12 col-xl-8 col-contenido izquierda">
                        <div class="wrapCol">
                            <div class="contenido">
                                <div class="">
                                    <?php include './shared/organizacion/perfil-cabecera.php' ?>
                                </div>
                                <div class="">
                                    <ul class="nav nav-tabs grey big-no-padding" id="myTab" role="tablist">
                                        <li class="nav-item">
                                            <a class="nav-link active" id="tab-1" data-toggle="tab" href="#tab-pane-1" role="tab" aria-controls="tab-pane-1" aria-selected="true">Actividad reciente</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" id="tab-2" data-toggle="tab" href="#tab-pane-2" role="tab" aria-controls="tab-pane-2" aria-selected="false">Integrantes 47</a>
                                        </li>
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane fade show active" id="tab-pane-1" role="tabpanel" aria-labelledby="ane-1">
                                            <?php include './shared/listado-recursos.php'; ?>
                                        </div>
                                        <div class="tab-pane fade" id="tab-pane-2" role="tabpanel" aria-labelledby="pane-2">
                                            <?php include './shared/listado-recursos-personas.php'; ?>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col col-12 col-xl-4 col-contexto col-lateral derecha">
                        <?php include './shared/organizacion/perfil-lateral.php'; ?>
                        <?php include './shared/relacionados/grupo-ultimas-contribuciones.php'; ?>
                        <?php include './shared/relacionados/grupo-ultimos-seguidos.php'; ?>
                        <?php include './shared/relacionados/grupo-administradores.php'; ?>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <?php include './shared/footer.php'; ?>
    <?php include './shared/modales-y-sidebars.php'; ?>
    <?php include './shared/modales/acciones-recurso/acciones-recurso.php'; ?>
</body>

</html>