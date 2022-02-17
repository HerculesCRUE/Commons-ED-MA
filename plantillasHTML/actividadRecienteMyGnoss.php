<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Actividad reciente";
    $invitado = false;
    $comunidad = true;
    $imagenUsuario = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Actividad reciente</title>

    <?php include './shared/style.php';    ?>
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
                                <a href="./actividadReciente.php">Home</a>
                            </li>
                            <li>
                                Actividad reciente
                            </li>
                        </ul>
                    </div>
                    <div class="col col-12 col-buscador">
                        <?php include './shared/buscadores/buscadorSeccion.php'; ?>
                    </div>
                    <div class="col col-12 col-xl-9 col-contenido izquierda">
                        <div class="wrapCol">
                            <div class="header-contenido">
                                <div class="h1-container">
                                    <h1>Actividad reciente</h1>
                                </div>
                            </div>
                            <h2 class="section-title">Recursos</h2>
                            <?php include './shared/listado-recursos.php'; ?>
                        </div>
                    </div>
                    <div class="col col-12 col-xl-3 col-contexto col-lateral derecha">
                        <?php include './shared/relacionados/grupo-comunidades.php'; ?>
                        <?php include './shared/relacionados/grupo-personas.php'; ?>
					</div>
				</div>
			</div>
		</main>

        <?php include './shared/footer.php'; ?>
        <?php include './shared/modales-y-sidebars.php'; ?>

</html>