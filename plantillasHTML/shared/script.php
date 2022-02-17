<!-- PERSONALIACION COMUNIDAD -->
<script src="http://code.jquery.com/jquery-3.5.1.js" integrity="sha256-QWo7LDvxbWT2tbbQ97B53yJnYU3WhH/C8ycbRAkjPDc=" crossorigin="anonymous"></script>
<script src="https://code.jquery.com/ui/1.12.0/jquery-ui.js" integrity="sha256-0YPKAwZP7Mp3ALMRVB2i8GXeEndvCq3eSl/WsAl1Ryk=" crossorigin="anonymous"></script>

<!-- BOOTSTRAP 4 + PROPELLER -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.12.9/umd/popper.min.js" integrity="sha384-ApNbgh9B+Y1QKtv3Rn7W3mgPxhU9K/ScQsAP7hUibX39j7fakFPskvXusvfa0b4Q" crossorigin="anonymous"></script>
<script src="./theme/libraries/bootstrap/bootstrap.min.js"></script>
<script src="./theme/libraries/propeller/js/propeller.js"></script>

<!-- AUTOCOMPLETE -->
<script type="text/javascript" src="./theme/libraries/autocomplete/jquery.autocomplete.js"></script>

<!-- LIGHTGALLERY -->
<script type="text/javascript" src="./theme/libraries/lightgallery/js/lightgallery-all.min.js"></script>

<!-- SCROLLMAGIC -->
<script type="text/javascript" src="./theme/libraries/scrollmagic/ScrollMagic.min.js"></script>
<script type="text/javascript" src="./theme/libraries/scrollmagic/debug.addIndicators.min.js"></script>

<!-- DATATABLES -->
<script type="text/javascript" src="./theme/libraries/datatables/datatables.min.js"></script>

<!-- JQUERY CUSTOM SCROLL BAR Version: 3.1.13 -->
<script type="text/javascript" src="./theme/libraries/customscroller/js/jquery.mCustomScrollbar.concat.min.js"></script>

<!-- JQUERY PROGRESS BAR TIME -->
<!-- DOCUMENTACION: https://www.jqueryscript.net/time-clock/Countdown-Timers-Bootstrap-4-Progressbar.html -->
<script type="text/javascript" src="./theme/libraries/progressBarTime/jquery.progressBarTimer.min.js"></script>

<!-- JQUERY PROGRESS BAR TIME -->
<!-- DOCUMENTACION: https://www.jqueryscript.net/time-clock/Countdown-Timers-Bootstrap-4-Progressbar.html -->
<script type="text/javascript" src="./theme/libraries/slidereveal/jquery.slidereveal.min.js"></script>

<!-- SELECT2 -->
<script type="text/javascript" src="./theme/libraries/select2/js/select2.min.js"></script>

<!-- TOASTR - https://www.jqueryscript.net/other/bootstrap-toasts-manager.html -->
<script type="text/javascript" src="./theme/libraries/toastr/toastr.min.js"></script>

<!-- CHARTS.JS -->
<script type="text/javascript" src="./theme/libraries/chart/chart.js"></script>

<!-- PERSONALIZACION TEMA -->
<script type="text/javascript" src="./theme/theme.js"></script>
<?php if(isset($comunidad) && $comunidad): ?>
    <?php if(isset($comunidadAlternativa) && $comunidadAlternativa): ?>

    <?php else: ?>
        <script type="text/javascript" src="./theme/community.js"></script>
    <?php endif; ?>
<?php endif; ?>