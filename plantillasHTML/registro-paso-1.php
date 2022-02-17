<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Registro";
    $clasePagina = "registro";
    $invitado = true;
    $comunidad = true;
    $listado = false;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Registro</title>

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
                    <div id="col01" class="col">
                        <div class="form formularioRegistroUsuarios panel-centrado">
                            <h1>Hazte miembro de <strong>Nextweb</strong></h1>
                            <div class="box box01">
                                <div class="step-progress-wrap mb-5">
                                    <ul class="step-progress">
                                        <li class="step-progress__circle done active">
                                            <span class="step-progress__text current">Datos básicos</span>
                                        </li>
                                        <li class="step-progress__bar"></li>
                                        <li class="step-progress__circle">
                                            <span class="step-progress__text">Datos personales</span>
                                        </li>
                                        <li class="step-progress__bar"></li>
                                        <li class="step-progress__circle">
                                            <span class="step-progress__text">Tus intereses</span>
                                        </li>
                                        <li class="step-progress__bar"></li>
                                        <li class="step-progress__circle">
                                            <span class="step-progress__text">Red de contactos</span>
                                        </li>
                                    </ul>
                                </div>
                                <div class="form-group mb-3">
                                    <label class="control-label d-block">Nombre</label>
                                    <input placeholder="Introduce tu nombre" type="text" name="" id="" class="form-control not-outline">
                                </div>
                                <div class="form-group mb-3">
                                    <label class="control-label d-block">Apellidos</label>
                                    <input placeholder="Introduce tus apellidos" type="text" name="" id="" class="form-control not-outline">
                                </div>
                                <div class="form-group mb-3">
                                    <label class="control-label d-block">Correo electrónico</label>
                                    <input placeholder="Introduce tu correo eléctrónico" type="email" name="" id="" class="form-control not-outline">
                                </div>
                                <div class="form-group mb-3">
                                    <label class="control-label d-block">Contraseña</label>
                                    <input placeholder="Introduce tu contraseña" type="password" name="" id="" class="form-control not-outline">
                                    <small class="helper">De 6 a 12 caracteres: al menos una letra y un número</small>
                                </div>
                                <div id="divKodatosUsuario">
                                    <div class="ko">
                                        <p>Debes rellenar todos los campos marcados con *</p>
                                        <p>El correo electrónico no es válido</p>
                                        <p>La contraseña debe tener entre 6 y 12 caracteres, al menos una letra y un número, y no puede contener caracteres especiales, excepto: # _ $ *</p>
                                        <p>Debes establecer la fecha de nacimiento</p>
                                    </div>
                                </div>
                                <div class="custom-control custom-checkbox mb-4">
                                    <input type="checkbox" id="proteger-version" name="proteger-version" class="custom-control-input">
                                    <label class="custom-control-label texto-gris-medio" for="proteger-version">He leído y acepto la <a class="link" data-target="#modal-politica-privacidad" data-toggle="modal"> política de privacidad</a> de myGNOSS.</label>
                                </div>
                                <div id="divKoCondicionesUso">
                                    <div class="ko">
                                        <p>Debes leer y aceptar las condiciones de acceso y uso</p>
                                    </div>
                                </div>
                                <div class="actionButtons">
                                    <div class="mb-3">
                                        <input type="submit" value="Unirse" class="btn btn-primary principal submit uppercase" id="btnCrearCuenta" onclick="if(ComprobarDatosRegistro('18')) {return false;}">
                                    </div>
                                    <div>
                                        <span>¿Ya eres usuario de GNOSS?</span>
                                        <a class="link" href="" onclick="CargarFormLoginRegistro('http://pruebas.gnoss.net/comunidad/gnoss-developers-community/hazte-miembro')">Iniciar sesión</a>
                                    </div>
                                </div>
                            </div>
                            <div class="panel-centrado-separator"></div>
                            <div class="box box02">
                                <fieldset id="fsPrefieresRedesSociales">
                                    <p>O bien,</p>
                                    <ul class="lista-redes-sociales lista-redes-sociales--registro no-list-style">
                                        <li class="twitter">
                                            <a href="http://serviciospruebas.gnoss.net/login/logintwitter.aspx?token=%252btM2RDGm6hGfWlvP767TUpAjhjRmuzn9nxFyhMo%252bBRQ%253d&amp;proyectoID=e01d50a3-73e0-4707-a6c9-10e0ca7dd165&amp;urlOrigen=" title="Twitter">
                                                <i class="fab fa-twitter"></i>
                                                <span>Regístrate con Twitter</span>
                                            </a>
                                        </li>
                                        <li class="facebook">
                                            <a href="http://serviciospruebas.gnoss.net/login/loginfacebook.aspx?token=%252btM2RDGm6hGfWlvP767TUpAjhjRmuzn9nxFyhMo%252bBRQ%253d&amp;proyectoID=e01d50a3-73e0-4707-a6c9-10e0ca7dd165&amp;urlOrigen=" title="Facebook">
                                                <i class="fab fa-facebook-f"></i>
                                                <span>Regístrate con Facebook</span>
                                            </a>
                                        </li>
                                        <li class="google">
                                            <a href="#" title="google">
                                                <i class="google-icon"></i>
                                                <span>Regístrate con Google</span>
                                            </a>
                                        </li>
                                    </ul>
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