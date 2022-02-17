<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

<!-- MATERIAL ICONS -->
<link type="text/css" rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons|Material+Icons+Outlined" media="all" />

<!-- FONT AWESOME -->
<link href="https://use.fontawesome.com/releases/v5.0.8/css/all.css" rel="stylesheet">

<!-- BOOTSTRAP 4 + PROPELLER -->
<link rel="stylesheet" href="./theme/libraries/bootstrap/bootstrap.min.css">
<link rel="stylesheet" href="./theme/libraries/propeller/css/propeller.min.css" media="screen" type="text/css" >

<!-- LIGHTGALLERY -->
<link type="text/css" rel="stylesheet" href="./theme/libraries/lightgallery/css/lightgallery.min.css">

<!-- JQUERY CUSTOM SCROLL BAR Version: 3.1.13 -->
<link type="text/css" rel="stylesheet" href="./theme/libraries/customscroller/css/jquery.mCustomScrollbar.min.css">

<!-- PRELOADER MATERIALIZE -->
<link type="text/css" rel="stylesheet" href="./theme/libraries/spinner-material/spinner-material.css">

<!-- DATATABLES -->
<link type="text/css" rel="stylesheet" href="./theme/libraries/datatables/datatables.min.css"/>

<!-- SELECT2 -->
<link type="text/css" rel="stylesheet" href="./theme/libraries/select2/css/select2.min.css"/>

<!-- TOASTR - https://www.jqueryscript.net/other/bootstrap-toasts-manager.html -->
<link type="text/css" rel="stylesheet" href="./theme/libraries/toastr/toastr.min.css"/>

<!-- DATEPICKER -->
<link type="text/css" rel="stylesheet" href="./theme/libraries/jqueryUI/jquery-ui.min.css"/>

<!-- PERSONALIZACION TEMA -->
<link type="text/css" rel="stylesheet" href="./theme/theme.css">

<!-- PERSONALIZACION COMUNIDAD -->
<?php if(isset($comunidad) && $comunidad): ?>
    <?php if(isset($comunidadAlternativa) && $comunidadAlternativa): ?>
            <link type="text/css" rel="stylesheet" href="./theme/communityAlternativa.css">
    <?php else: ?>
            <link type="text/css" rel="stylesheet" href="./theme/community.css">
    <?php endif; ?>
<?php endif; ?>
