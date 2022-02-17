<div class="row upper-row">
    <div class="col col01">
        <div class="menu-logo-wrapper">

            <?php if(!$invitado): ?>
                <div class="menu-toggle">
                    <a href="javascript: void(0);" data-target="menuLateral" id="menuLateralTrigger" class="texto-blanco">
                        <span class="material-icons">apps</span>
                    </a>
                </div>
            <?php endif; ?>

            <div class="logo-wrapper" >
                <a href="./home.php" class="texto-blanco">
                    Hércules
                    <!-- <img class="logo-corporativo" src="./theme/resources/logotipos/logognossblanco.png"> -->
                </a>
            </div>
        </div>
    </div>
    <div class="col col02">
        <div class="col-buscador">
            <?php include './shared/buscadores/buscadorSeccion.php'; ?>
        </div>
    </div>
    <div class="col col03">
        <ul>
            <?php if($invitado): ?>

                <li class="iniciar-sesion">
                    <a href="./login.php" class="texto-blanco">Iniciar sesión</a>
                </li>

            <?php else: ?>

                <li class="nuevo nuevo-mensaje">
                    <a href="javascript: void(0);" class="btn btn-outline-primary btn-round">
                        <span class="material-icons">mail_outline</span>
                        <span>Redactar</span>
                    </a>
                </li>
                <li class="usuario">
                    <span id="menuLateralUsuarioTrigger" class="texto-blanco">
                        <?php include './shared/usuario/user-miniatura.php'; ?>
                    </span>
                </li>
                <li class="buscar">
                    <a href="javascript: void(0);" class="texto-blanco">
                        <span class="material-icons">search</span>
                    </a>
                </li>

            <?php endif; ?>
        </ul>
    </div>
</div>