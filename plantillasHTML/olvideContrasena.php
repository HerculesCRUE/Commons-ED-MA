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
    <title>Hércules -  ¿Olvidaste la contraseña?</title>

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
                            <h1>¿Olvidaste tu contraseña?</h1>
                            <div class="fieldset">
                                <fieldset class="busquedaEstandar">
                                    <div class="fieldsetContent">
                                        <div class="texto-gris-medio">
                                            <p>Si olvidaste tu contraseña puedes obtener una nueva. Para ello introduce tu correo electrónico o nombre de usuario y te enviaremos un enlace a tu cuenta de correo donde podrás cambiar tu contraseña.</p>
                                            <p>Si no ves este correo electrónico en tu bandeja de entrada puede ser porque se haya filtrado como SPAM. Revisa por favor tu carpeta de correo no deseado y asegúrate de marcar como "no es spam" el correo recibido.</p>
                                        </div>
                                        <div class="form-group">
                                            <label for="userLogin" class="control-label">Correo electrónico:</label>
                                            <input placeholder="Introduce tu correo electrónico" type="text" value="" id="userLogin" class="form-control">
                                        </div>
                                        <div id="lblError" class="error-wrap">
                                            <p class="ko"></p>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="fieldset actionButtons">
                                <fieldset>
                                    <div class="fieldsetContent">
                                        <input type="button" value="Enviar" class="submit principal btn btn-primary" id="btnEnviar">
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </div>
                <script type="text/javascript" language="javascript">
                    $(document).ready(function() {
                        $('#btnEnviar').click(function() {
                            var errorDisplay = $('#lblError p');
                            if ($('#userLogin').val() == "") {
                                errorDisplay.html('Introduzca su nombre de usuario');
                                errorDisplay.show();
                            } else {
                                MostrarUpdateProgress();
                                errorDisplay.hide();
                                var dataPost = {
                                    User: $('#userLogin').val()
                                }

                                GnossPeticionAjax('http://devcotec.gnoss.com/comunidad/cotec/olvide-password/change-request', dataPost, true).fail(function() {
                                    errorDisplay.html('No es posible cambiar la contrase&#241;a de un usuario que todav&#237;a no tiene acceso a GNOSS.');
                                    errorDisplay.show();
                                    OcultarUpdateProgress();
                                });
                            }
                        })
                    });
            </script>
        </div>
    </main>

    <?php include './shared/footer.php'; ?>
    <?php include './shared/modales-y-sidebars.php'; ?>

</body>

</html>