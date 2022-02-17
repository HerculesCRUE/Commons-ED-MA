<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Login";
    $clasePagina = "login";
    $invitado = true;
    $comunidad = true;
    $listado = false;
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

<body class="<?php include './shared/clasesBody.php'; ?> conBackground" cz-shortcut-listen="true">

    <?php
    include './shared/formsGnoss.php';
    include './shared/cabecera.php';
    ?>

    <main role="main" style="background-image: url('./theme/resources/imagenes-pre/background-login.jpg'); background-size: cover;">
        <div class="container">
            <div class="row">
                <div id="col01" class="col">
                    <div class="form formtheme01 formularioRegistroUsuarios panel-centrado">
                        <h1>Inicia sesión en <strong>Hércules</strong></h1>
                        <div class="box box01">
                            <form method="post" id="formPaginaLogin" action="http://serviciosdevcotec.gnoss.com/login/login.aspx?token=6PF60A7A6x2ROXveot2yJXjkFI2Bkf9kF0gxq9pmZZA%253d&amp;proyectoID=90cd1c8e-aaeb-480d-ae36-95db17f46033">
                                <div class="fieldset fieldset01">
                                    <fieldset>
                                        <div class="fieldsetContent">
                                            <div class="form-group">
                                                <label for="usuario_Login" class="control-label">Correo electrónico</label>
                                                <input placeholder="Introduce tu correo electrónico" type="text" name="usuario" id="usuario_Login" class="form-control not-outline">
                                            </div>
                                            <div class="form-group">
                                                <label for="password_login" class="control-label">Contraseña</label>
                                                <input placeholder="Introduce tu contraseña" type="password" name="password" id="password_login" class="form-control not-outline">
                                            </div>

                                                <div id="loginError" class="error-wrap">
                                                    <div class="ko">
                                                        <p id="mensajeError">El usuario o la contraseña son incorrectos. </p>
                                                    </div>
                                                </div>
                                                <div id="loginErrorAutenticacionExterna" class="error-wrap">
                                                    <div class="ko">
                                                        <p>Se ha producido un error en el servicio de autenticación. </p>
                                                    </div>
                                                </div>
                                                <div id="logintwice" class="error-wrap">
                                                    <div class="ko">
                                                        <p id="mensajeErrorLoginTwice">El usuario ya está logueado desde otro navegador. Desconéctese de los otros navegadores o espere a que termine la sesión automáticamente (20 minutos desde el último acceso). </p>
                                                    </div>
                                                </div>

                                            </div>
                                        </fieldset>
                                    </div>
                                    <div class="fieldset actionButtons fieldset03 actionButtons">
                                        <fieldset>
                                            <div class="fieldsetContent">
                                                <a class="olvidaste-password " href="./olvideContrasena.php">¿No puedes iniciar sesión?</a>
                                                <input class="principal submit btn btn-primary" type="submit" onclick="if(validarCampos()){MostrarUpdateProgress();}else{ mostrarErrores('generico'); return false; }" title="Iniciar sesión" value="Iniciar sesión">
                                            </div>
                                        </fieldset>
                                    </div>
                                </form>
                                <p class="actionRegister">Si todavía no tienes cuenta <a href="./registro-paso-1.php">Regístrate</a></p>

                                <script type="text/javascript">
                                    $(document).ready(function() {
                                        if (ObtenerHash() == '#error') {
                                            mostrarErrores('generico');
                                        } else if (ObtenerHash().indexOf('&') > 0) {
                                            var mensajeError = ObtenerHash().split('&')[1];
                                            if (mensajeError != '') {
                                                mostrarErrores('mensaje', mensajeError);
                                            }
                                        } else if (document.location.href.endsWith('logintwice')) {
                                            mostrarErrores('logintwice');
                                        }
                                        if (ObtenerHash() == '#errorAutenticacionExterna') {
                                            $('#loginErrorAutenticacionExterna .ko').show();
                                        }

                                        $('#formPaginaLogin').prop('action', $('#inpt_UrlLogin').val());
                                        $('#usuario_Login').focus();
                                        $('#usuario_Login').keydown(function(event) {
                                            if (event.which || event.keyCode) {
                                                if ((event.which == 13) || (event.keyCode == 13)) {
                                                    return false;
                                                }
                                            }
                                        });
                                        $('#password_login').keydown(function(event) {
                                            if (event.which || event.keyCode) {
                                                if ((event.which == 13) || (event.keyCode == 13)) {
                                                    if ($('#usuario_Login').val() == "" || $('#password_login').val() == "") {
                                                        $('#loginError .ko').css('display', 'block');
                                                        return false;
                                                    }
                                                }
                                            }
                                        });
                                    });

                                    function validarCampos() {
                                        return ($('#usuario_Login').val() != '' && $('#password_login').val() != '')
                                    }

                                    function mostrarErrores(error, mensajeError = "") {
                                        $('.ko').hide();
                                        var password = $('#usuario_Login').parent();
                                        var email = $('#password_login').parent();

                                        password.addClass('invalid');
                                        email.addClass('invalid');

                                        switch (error) {
                                            case 'generico':
                                                $('#loginError .ko').show();
                                                break;
                                            case 'mensaje':
                                                $('#loginError .ko').show();
                                                $('#mensajeError').text(mensajeError);
                                                break;
                                            case 'logintwice':
                                                $('#logintwice .ko').show();
                                                break;
                                            case 'autenticacion':
                                                $('#loginErrorAutenticacionExterna .ko').show();
                                                break;
                                            default:
                                                break;
                                        }

                                        return false;
                                    }
                                </script>
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