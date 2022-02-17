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
                            <h1>Hola <strong>Félix</strong>, gracias por unirte a <strong>Nextweb</strong></h1>
                            <div class="box box01">
                                <div class="step-progress-wrap mb-5">
                                    <ul class="step-progress">
                                        <li class="step-progress__circle done active">
                                            <span class="step-progress__text">Datos básicos</span>
                                        </li>
                                        <li class="step-progress__bar active"></li>
                                        <li class="step-progress__circle done active">
                                            <span class="step-progress__text">Datos personales</span>
                                        </li>
                                        <li class="step-progress__bar active"></li>
                                        <li class="step-progress__circle done active">
                                            <span class="step-progress__text">Tus intereses</span>
                                        </li>
                                        <li class="step-progress__bar active"></li>
                                        <li class="step-progress__circle active">
                                            <span class="step-progress__text current">Red de contactos</span>
                                        </li>
                                    </ul>
                                </div>
                                <div class="grupo-personas mb-5">
                                    <div class="resource-list usuarios con-borde">
                                        <article class="resource resource-grupo">
                                            <div class="user-miniatura">
                                                <div class="imagen-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <div class="imagen">
                                                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="nombre-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <p class="nombre">María Elena Alvarado</p>
                                                        <p class="nombre-completo">María Elena Alvarado · Equipo GNOSS</p>
                                                    </a>
                                                </div>
                                                <div class="acciones-usuario-wrap">
                                                    <ul class="no-list-style">
                                                        <li>
                                                            <a class="btn btn-outline-grey" onclick="AccionPerfil_Seguir(this, 'http://pruebas.gnoss.net/comunidad/gnoss-developers-community/perfil/pruebadew1/follow')">
                                                                <span class="material-icons">person_add_alt_1</span>
                                                                <span>Seguir</span>
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </article>
                                        <article class="resource resource-grupo">
                                            <div class="user-miniatura">
                                                <div class="imagen-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <div class="imagen">
                                                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario-2.jpg)"></span>
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="nombre-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <p class="nombre">Tomás Torres</p>
                                                        <p class="nombre-completo">Tomás Torres García · Equipo GNOSS</p>
                                                    </a>
                                                </div>
                                                <div class="acciones-usuario-wrap">
                                                    <ul class="no-list-style">
                                                        <li>
                                                            <a class="btn btn-outline-grey" onclick="AccionPerfil_Seguir(this, 'http://pruebas.gnoss.net/comunidad/gnoss-developers-community/perfil/pruebadew1/follow')">
                                                                <span class="material-icons">person_add_alt_1</span>
                                                                <span>Seguir</span>
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </article>
                                        <article class="resource resource-grupo">
                                            <div class="user-miniatura">
                                                <div class="imagen-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <div class="imagen">
                                                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="nombre-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <p class="nombre">María Elena Alvarado</p>
                                                        <p class="nombre-completo">María Elena Alvarado · Equipo GNOSS</p>
                                                    </a>
                                                </div>
                                                <div class="acciones-usuario-wrap">
                                                    <ul class="no-list-style">
                                                        <li>
                                                            <a class="btn btn-outline-grey" onclick="AccionPerfil_Seguir(this, 'http://pruebas.gnoss.net/comunidad/gnoss-developers-community/perfil/pruebadew1/follow')">
                                                                <span class="material-icons">person_add_alt_1</span>
                                                                <span>Seguir</span>
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </article>
                                        <article class="resource resource-grupo">
                                            <div class="user-miniatura">
                                                <div class="imagen-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <div class="imagen">
                                                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="nombre-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <p class="nombre">María Elena Alvarado</p>
                                                        <p class="nombre-completo">María Elena Alvarado · Equipo GNOSS</p>
                                                    </a>
                                                </div>
                                                <div class="acciones-usuario-wrap">
                                                    <ul class="no-list-style">
                                                        <li>
                                                            <a class="btn btn-outline-grey" onclick="AccionPerfil_Seguir(this, 'http://pruebas.gnoss.net/comunidad/gnoss-developers-community/perfil/pruebadew1/follow')">
                                                                <span class="material-icons">person_add_alt_1</span>
                                                                <span>Seguir</span>
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </article>
                                        <article class="resource resource-grupo">
                                            <div class="user-miniatura">
                                                <div class="imagen-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <div class="imagen">
                                                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario-2.jpg)"></span>
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="nombre-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <p class="nombre">Tomás Torres</p>
                                                        <p class="nombre-completo">Tomás Torres García · Equipo GNOSS</p>
                                                    </a>
                                                </div>
                                                <div class="acciones-usuario-wrap">
                                                    <ul class="no-list-style">
                                                        <li>
                                                            <a class="btn btn-outline-grey" onclick="AccionPerfil_Seguir(this, 'http://pruebas.gnoss.net/comunidad/gnoss-developers-community/perfil/pruebadew1/follow')">
                                                                <span class="material-icons">person_add_alt_1</span>
                                                                <span>Seguir</span>
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </article>
                                        <article class="resource resource-grupo">
                                            <div class="user-miniatura">
                                                <div class="imagen-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <div class="imagen">
                                                            <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                                                        </div>
                                                    </a>
                                                </div>
                                                <div class="nombre-usuario-wrap">
                                                    <a href="./fichaPerfil.php">
                                                        <p class="nombre">María Elena Alvarado</p>
                                                        <p class="nombre-completo">María Elena Alvarado · Equipo GNOSS</p>
                                                    </a>
                                                </div>
                                                <div class="acciones-usuario-wrap">
                                                    <ul class="no-list-style">
                                                        <li>
                                                            <a class="btn btn-outline-grey" onclick="AccionPerfil_Seguir(this, 'http://pruebas.gnoss.net/comunidad/gnoss-developers-community/perfil/pruebadew1/follow')">
                                                                <span class="material-icons">person_add_alt_1</span>
                                                                <span>Seguir</span>
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </article>
                                    </div>
                                </div>

                                <div class="actionButtons">
                                    <div class="mb-3">
                                        <input type="submit" value="Guardar" class="btn btn-primary principal submit uppercase">
                                    </div>
                                    <div>
                                        <span>¿Ya eres usuario de GNOSS?</span>
                                        <a class="link" href="" onclick="CargarFormLoginRegistro('http://pruebas.gnoss.net/comunidad/gnoss-developers-community/hazte-miembro')">Iniciar sesión</a>
                                    </div>
                                </div>
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