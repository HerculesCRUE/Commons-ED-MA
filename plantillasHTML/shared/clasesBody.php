<?php

    $clasesBody = isset($clasePagina) ? $clasePagina : "";

    if(!empty($invitado)) {
        $clasesBody = $clasesBody . " invitado";
    }else{
        $clasesBody = $clasesBody . " usuarioConectado";
    }

    if(!empty($comunidad)) {
        $clasesBody = $clasesBody . " comunidad";
    }

    if(!empty($listado)) {
        $clasesBody = $clasesBody . " listado";
    }

    if(!empty($min_height_content) && $min_height_content) {
        $clasesBody = $clasesBody . " no-min-height-content";
    }

    echo $clasesBody;

?>