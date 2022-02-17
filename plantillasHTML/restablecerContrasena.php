<?php
// variables para que cambie la visualización, no implementar
$pageTitle = "Restablecer contraseña";
$clasePagina = "password-page";
$invitado = true;
$comunidad = true;
$listado = false;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Restablecer contraseña</title>

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
                            <h1>Restablecer contraseña</h1>
                            <div class="fieldset fieldset01">
                                <fieldset>
                                    <div class="fieldsetContent">
                                        <div class="form-group">
                                            <label for="usuario_mail" class="control-label">Correo electrónico / usuario</label>
                                            <input placeholder="Introduce tu correo electrónico o nombre de usuario" type="text" name="usuario_mail" id="usuario_mail" class="form-control not-outline">
                                        </div>
                                        <div class="form-group">
                                            <label for="txtNewPassword" class="control-label">Nueva contraseña</label>
                                            <input placeholder="Introduce tu contraseña" type="password" name="txtNewPassword" id="txtNewPassword" class="form-control not-outline">
                                        </div>
                                        <div class="form-group">
                                            <label for="txtConfirmedPassword" class="control-label">Confirmar nueva contraseña</label>
                                            <input placeholder="Confirma tu contraseña" type="password" name="txtConfirmedPassword" id="txtConfirmedPassword" class="form-control not-outline">
                                        </div>
                                        <p id="warning" style="display: none;">Bloq Mayús activado</p>
                                    </div>
                                    <div style="display: none;" id="lblErrorExterno" class="ok"></div>
                                </fieldset>
                            </div>
                            <div class="fieldset actionButtons fieldset02 actionButtons">
                                <fieldset>
                                    <div class="fieldsetContent">
                                        <input class="principal submit btn btn-primary" id="btnCambiarPassword" type="button" value="Guardar">
                                    </div>
                                </fieldset>
                            </div>
                            <div class="actionRegister">
                                <p>¿No solicitaste el cambio de contraseña?</p>
                                <p><a href="#">Pulsa este enlace para cancelar la solicitud</a></p>
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