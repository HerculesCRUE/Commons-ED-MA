<?php
// variables para que cambie la visualización, no implementar
$pageTitle = "Instrucciones enviadas";
$clasePagina = "password-page";
$invitado = true;
$comunidad = true;
$listado = false;
$min_height_content = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Login</title>

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
                    <div id="col02" class="col">
                        <div class="panel-centrado">
                            <h1>Instrucciones enviadas</h1>
                            <div class="fieldset">
                                <fieldset class="busquedaEstandar">
                                    <div class="fieldsetContent">
                                        <div class="texto-gris-medio">
                                            <p>Te hemos enviado un correo electrónico a la cuenta indicada con un instrucciones para restablecer tu contraseña</p>
                                            <p><a href="./home.php">Ir al inicio</a></p>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <?php include './shared/footer.php'; ?>
    <?php include './shared/modales-y-sidebars.php'; ?>

</body>

</html>