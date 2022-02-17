<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Personalizaciones de comunidades GNOSS";
    $invitado = false;
    $comunidad = true;
    $nombreComunidad = "Hércules";
    $listado = true;
    $imagenUsuario = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Búsqueda</title>

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
                                <a href="./home.php">Home</a>
                            </li>
                            <li>
                                Personas
                            </li>
                        </ul>
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
                                    <h1>Recursos <span class="numResultados">50</span></h1>
                                </div>
                                <?php include './shared/acciones/acciones-listado-compactado.php'; ?>
                                <?php include './shared/filtros-listado.php'; ?>
                            </div>
                            <?php include './shared/listado-recursos.php'; ?>
                            <?php include './shared/cargar-resultados.php'; ?>
						</div>
					</div>
				</div>
			</div>
		</main>
        <?php include './shared/footer.php'; ?>
        <?php include './shared/modales-y-sidebars.php'; ?>

</html>