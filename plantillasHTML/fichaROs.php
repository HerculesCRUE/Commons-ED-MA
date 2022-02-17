<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Ficha Publicación";
    $clasePagina = "fichaRecurso fichaRecurso-publicacion";
    $invitado = false;
    $comunidad = true;
    $nombreComunidad = 'Hércules';
    $listado = false;
    $imagenUsuario = false;
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
            <div class="row">
                <div class="col col-12 col-breadcrumb">
                    <ul>
                        <li>
                            <a href="./home.php">Home</a>
                        </li>
                        <li>
                            <a href="javascript: void(0);">ROs</a>
                        </li>
                        <li>
                            Social Network Analysis in E-Learning Environments: A Preliminary Systematic Review.
                        </li>
                    </ul>
                </div>
                <div class="col col-12 col-xl-12 col-contenido">
                    <div class="wrapCol">
                        <?php include './shared/ros/cabecera-ros.php' ?>
                        <?php include './shared/ros/contenido-ros.php' ?>
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