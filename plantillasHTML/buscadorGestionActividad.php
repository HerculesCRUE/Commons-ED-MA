<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Personalizaciones de comunidades GNOSS";
    $invitado = false;
    $comunidad = true;
    $listado = true;
    $nombreComunidad = "Hercules";
    $imagenUsuario = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">
	<head>
		<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
		<title>Hercules - Búsqueda</title>

        <?php include './shared/style.php';	?>
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
					<div class="col col-12 col-xl-3 col-facetas col-lateral izquierda">
						<div class="wrapCol">
							<?php include './shared/facetas.php'; ?>
						</div>
					</div>
					<div class="col col-12 col-xl-9 col-contenido derecha">
                        <div class="wrapCol">
                            <div class="header-contenido">
                                <div class="h1-container">
                                    <h1>Gestión actividad científica <span class="numResultados">110</span></h1>
								</div>
								<div class="dropdown anadir-ro">
									<a href="#" class="dropdown-toggle btn btn-outline-grey" role="button" id="dropdownAnadirRo" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
										<span class="material-icons">post_add</span>
										Añadir resultado de investigación
									</a>
									<div class="dropdown-menu basic-dropdown dropdown-menu-right" aria-labelledby="dropdownAnadirRo">
										<ul class="no-list-style">
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons">code</span>
													<span class="texto">Código</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons-outlined">fact_check</span>
													<span class="texto">Protocolos</span>
												</a>
											</li>
										</ul>
										<ul class="no-list-style background-gris">
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons">grid_on</span>
													<span class="texto">Dataset</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons">collections</span>
													<span class="texto">Presentación</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons-outlined">poll</span>
													<span class="texto">Gráficos</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons">insert_drive_file</span>
													<span class="texto">Docs</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons">link</span>
													<span class="texto">Enlaces</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons-outlined">videocam</span>
													<span class="texto">Video</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons-outlined">satellite</span>
													<span class="texto">Poster</span>
												</a>
											</li>
											<li>
												<a href="javascript: void(0);">
													<span class="material-icons-outlined">book</span>
													<span class="texto">Lesson</span>
												</a>
											</li>
										</ul>
									</div>
								</div>
								<?php include './shared/filtros-listado.php'; ?>
							</div>
							<?php include './shared/acciones/acciones-listado-gestion.php'; ?>
                            <?php include './shared/listado-recursos-gestion-actividad.php'; ?>
                            <?php include './shared/cargar-resultados.php'; ?>
						</div>
					</div>
				</div>
			</div>
		</main>

        <?php include './shared/footer.php'; ?>
        <?php include './shared/modales-y-sidebars.php'; ?>
		<?php include './shared/modales/gestion/modal-enviar-produccion-cientifica.php'; ?>
		<?php include './shared/modales/gestion/modal-anadir-area-conocimiento.php'; ?>
		<?php include './shared/modales/gestion/modal-anadir-topicos.php'; ?>

	</body>
</html>