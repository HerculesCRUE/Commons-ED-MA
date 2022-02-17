<?php
    // no es invitado
    if(empty($invitado)){
        include './shared/menuLateral.php';
        include './shared/menuLateralUsuario.php';
        // include './shared/menuLateralMetabuscador.php';
        include './shared/modales/gestion/modal-invitar-comunidad.php';
        include './shared/modales/gestion/modal-gestionar-suscripciones.php';
        include './shared/modales/gestion/modal-recibir-newsletter.php';
        include './shared/modales/gestion/modal-abandonar-comunidad.php';

        if(!empty($listado)){
            include './shared/modalResultados.php';
            include './shared/modalResultadosPaginado.php';
        }
    }
    include './shared/modales/modales-legales/politica-privacidad.php';
    include './shared/modales/modales-legales/politica-cookies.php';
    include './shared/modales/modales-legales/condiciones-uso.php';
?>

<div id="mascaraBlanca">
    <div class="wrap popup">
        <div class="preloader-wrapper active">
            <div class="spinner-layer spinner-blue-only">
            <div class="circle-clipper left">
                <div class="circle"></div>
            </div><div class="gap-patch">
                <div class="circle"></div>
            </div><div class="circle-clipper right">
                <div class="circle"></div>
            </div>
            </div>
        </div>
    </div>
</div>