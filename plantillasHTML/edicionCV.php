<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Título página";
    $clasePagina = "page-cv";
    $invitado = false;
    $comunidad = true;
    $nombreComunidad = "Hércules";
    $listado = false;
    $imagenUsuario = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Editar CV</title>

    <?php include './shared/style.php'; ?>
    <?php include './shared/script.php'; ?>    <meta name="apple-mobile-web-app-capable" content="yes">
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
                            Editar CV
                        </li>
                    </ul>
                </div>
                <div class="col col-12 col-contenido">
                    <div class="wrapCol">
                        <?php include './shared/editar-cv/cabecera-cv.php'; ?>
                        <?php include './shared/editar-cv/contenido-cv.php'; ?>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <?php include './shared/footer.php'; ?>
    <?php include './shared/modales-y-sidebars.php'; ?>
    <?php include './shared/modales/acciones-recurso/acciones-recurso.php'; ?></body>
    <?php include './shared/editar-cv/modales/modal-editar-datos-identificacion.php'; ?>
    <?php include './shared/editar-cv/modales/modal-anadir-datos-experiencia.php'; ?>
    <?php include './shared/editar-cv/modales/modal-editar-autor.php'; ?>
    <?php include './shared/editar-cv/modales/modal-anadir-autor.php'; ?>
    <?php include './shared/editar-cv/modales/modal-editar-tesauro.php'; ?>
    <?php include './shared/modales/gestion/modal-anadir-topicos.php'; ?>
</html>