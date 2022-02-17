<?php
    // variables para que cambie la visualización, no implementar
    $pageTitle = "Título página";
    $clasePagina = "fichaRecurso";
    $invitado = false;
    $comunidad = true;
    $nombreComunidad = "Hércules";
    $listado = false;
    $imagenUsuario = true;
?>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es" xmlns:og="http://ogp.me/ns#">

<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>Hércules - Ficha Recurso</title>

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
                    <div class="col col-12 col-breadcrumb">
                        <ul>
                            <li>
                                <a href="./home.php">Home</a>
                            </li>
                            <li>
                                <a href="javascript: void(0);">Tipo recurso</a>
                            </li>
                            <li>
                                Título recurso
                            </li>
                        </ul>
                    </div>
                    <div class="col col-12 col-xl-8 col-contenido izquierda">
                        <div class="wrapCol">
                            <div class="header-resource">
                                <div class="title-wrap">
                                    <h1 class="title">Extensión MVC a Comunidades del ecosistema: Detalles a tener en cuenta</h1>
                                </div>
                                <div class="upper-wrap">
                                    <?php include './shared/usuario/user-miniatura.php'; ?>
                                    <p class="fecha">Lun 22, jun. 2020</p>
                                </div>
                            </div>
                            <div class="acciones-recurso-wrapper">
                                <div class="izquierda">
                                    <div class="visualizacion-recurso">
                                        <span class="material-icons">visibility</span>
                                        <span class="number">135</span>
                                    </div>
                                    <div class="interacciones-recurso">
                                        <div class="likes">
                                            <span class="material-icons">thumb_up_alt</span>
                                            <span class="number">12</span>
                                        </div>
                                        <div class="dislikes">
                                            <span class="material-icons">thumb_down_alt</span>
                                            <span class="number">3</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="derecha">
                                    <ul class="acciones-recurso">
                                        <li class="destacada oculto">
                                            <a class="btn btn-primary" href="javascript: void(0);">
                                                <span class="material-icons">get_app</span>
                                                <span class="texto">PDF</span>
                                            </a>
                                        </li>
                                        <li class="destacada">
                                            <a class="btn btn-primary" href="javascript: void(0);">
                                                <span class="material-icons">forward</span>
                                                <span class="texto">Ir a la web</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a class="btn btn-outline-grey" href="./edicion.php">
                                                <span class="material-icons">mode_edit</span>
                                                <span class="texto">Editar</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a class="btn btn-outline-grey" href="javascript: void(0);" data-toggle="modal" data-target="#modal-enviar">
                                                <span class="material-icons">send</span>
                                                <span class="texto">Enviar</span>
                                            </a>
                                        </li>
                                        <li>
                                            <a class="btn btn-outline-grey" href="javascript: void(0);" data-toggle="modal" data-target="#modal-etiquetar">
                                                <span class="material-icons">label</span>
                                                <span class="texto">Etiquetar</span>
                                            </a>
                                        </li>
                                        <li>
                                            <?php include './shared/acciones/acciones-recurso.php' ?>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="contenido">
                                <div class="grupo grupo-principal">
                                    <p><strong>La colaboración público-privada se está mostrando como una de las soluciones más eficaces para abordar los grandes temas</strong> en los que debemos avanzar como sociedad. Este tipo de alianzas es fundamental en áreas como la educación, donde la digitalización ha cobrado especial importancia durante el último año.
                                    </p>
                                    <p><strong>Con el fin de impulsar alianzas público-privadas</strong> e iniciativas conjuntas en los países socios de la<strong> Cooperación Española, este lunes se celebra la I Mesa de Empresa y Cooperación en Educación Digital,</strong> en la que ha participado Didactalia.
                                    </p>
                                    <p>La sesión reunirá a expertos del sector público junto a un amplio grupo de directivos de las empresas que más innovan en educación. El objetivo es compartir puntos de interés y potenciar acciones capaces de transformar digitalmente la enseñanza en países en desarrollo.
                                    </p>
                                    <p>La iniciativa cuenta <strong>con el apoyo de E-duc@ de la Agencia Española de Cooperación Internacional para el Desarrollo (AECID).</strong> La información sobre el evento, no presencial, y que podrá seguirse en streaming a las 17.00 del lunes 25 de enero está ya disponible en la web de la Casa de América.
                                    </p>
                                    <p>Desde el ámbito público, la Mesa contará con la participación de Ángeles Moreno Bau, secretaria de Estado de Cooperación Internacional; Xiana Margarida Méndez, secretaria de Estado de Comercio; Carme Artigas Brugal, secretaria de Estado de Transformación Digital; Alejandro Tiana, secretario de Estado de Educación; y Magdy Martínez Solimán, director de la Agencia Española de Cooperación Internacional (AECID).
                                    </p>
                                    <p>Además de Didactalia, en el evento participarán otras empresas destacadas del sector como Telefónica, Fundación Telefónica, ProFruturo, Hispasat, Fundación Orange, Fundación Vodafone, Microsoft, Ibérica, Escudo Web, Grupo Mondragón, McGraw Hill, OTBInnova, Planeta, Santillana, SM, Rubio, ISDI, Grupo-AE, Siemens-Gamesa. Rocío Millá, Fundación Mapfre, Fundación Banco Santander, Santander Universidades, Anaya, BBVA y Coverwallet.
                                    </p>
                                </div>
                            </div>
                            <div class="info-recurso">
                                <div class="group autor">
                                    <p class="group-title">Autor:</p>
                                    <ul>
                                        <li>
                                            <a href="">Autor del recurso</a>
                                        </li>
                                    </ul>
                                    <span class="btn btn-outline-grey">
                                        <span class="material-icons">person_add</span>
                                        <span>Añadir</span>
                                    </span>
                                </div>
                                <div class="group categorias">
                                    <p class="group-title">
                                        Categorías:
                                    </p>
                                    <ul id="listCat">
                                        <li><a><span>8-10 años/2º Ciclo Primaria</span></a></li>
                                        <li><a><span>Experiencia educativa</span></a></li>
                                        <li><a><span>12-14 años/1º-2º ESO</span></a></li>
                                    </ul>
                                </div>
                                <div class="group etiquetas">
                                    <p class="group-title">
                                        Etiquetas:
                                    </p>
                                    <ul id="listTags">
                                        <li><a><span>barbara petchenik</span></a></li>
                                        <li><a><span>día internacional de la mujer y la niña en la ciencia</span></a></li>
                                        <li><a><span>cartografía</span></a></li>
                                    </ul>
                                </div>
                                <div class="group licencia">
                                    <p class="group-title">
                                        Licencias:
                                    </p>
                                    <ul id="listTags" class="listTags">
                                        <li><a><span>barbara petchenik</span></a></li>
                                    </ul>
                                </div>
                                <div class="group redes-sociales">
                                    <p class="group-title">
                                        Compartir en:
                                    </p>
                                    <ul class="lista-redes-sociales no-list-style">
                                        <li class="twitter">
                                            <a href="#" title="Twitter"><i class="fab fa-twitter"></i></a>
                                        </li>
                                        <li class="facebook">
                                            <a href="#" title="Facebook"><i class="fab fa-facebook-f"></i></a>
                                        </li>
                                        <li class="linkedin">
                                            <a href="#" title="Linkedin"><i class="fab fa-linkedin-in"></i></a>
                                        </li>
                                        <li class="whatsapp">
                                            <a href="#" title="whatsapp"><i class="fab fa-whatsapp"></i></a>
                                        </li>
                                        <li class="reddit">
                                            <a href="#" title="reddit"><i class="fab fa-reddit"></i></a>
                                        </li>
                                        <li class="blogger">
                                            <a href="#" title="blogger"><i class="fab fa-blogger-b"></i></a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <?php include './shared/comentarios/comentariosFicha.php' ?>
                        </div>
                    </div>
                    <div class="col col-12 col-xl-4 col-contexto col-lateral derecha">
                        <?php include './shared/relacionados/grupos-relacionados.php'; ?>
                        <?php include './shared/relacionados/grupos-vinculados.php'; ?>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <?php include './shared/footer.php'; ?>
    <?php include './shared/modales-y-sidebars.php'; ?>
    <?php include './shared/modales/acciones-recurso/acciones-recurso.php'; ?></body>

</html>