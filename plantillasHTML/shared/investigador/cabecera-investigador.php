<!-- AÑADIR CLASE cabecera-ficha -->
<div class="cabecera-ficha">
    <div class="ficha-upper-row">
        <div class="user-miniatura">
            <div class="imagen-usuario-wrap">
                <?php if (isset($imagenUsuario) && $imagenUsuario) : ?>
                    <div class="imagen">
                        <span style="background-image: url(theme/resources/imagenes-pre/foto-usuario.jpg)"></span>
                    </div>
                <?php else : ?>
                    <div class="imagen sinImagen">
                        <span class="material-icons">person</span>
                    </div>
                <?php endif ?>
            </div>
            <div class="nombre-usuario-wrap">
                <p class="nombre">Tim Berners-Lee</p>
                <p class="rol">Univ.-Prof. Dr.techn., MSc</p>
            </div>
        </div>
        <div class="acciones-ficha-wrap">
            <div class="info-resource">
                <span class="material-icons">article</span>
                <span id="numPublicacionesCab" about="http://gnoss.com/items/Person_77396ffc-5442-481e-8e11-80c022504c41_b35af76c-b40d-454f-8aaa-90a1392ccfb6" property="http://w3id.org/roh/publicationsNumber" class="nombre">169 Publicaciones &nbsp;</span>
            </div>
            <div class="info-resource">
                <span class="material-icons">tag</span>
                <span id="numPublicacionesCab" about="http://gnoss.com/items/Person_77396ffc-5442-481e-8e11-80c022504c41_b35af76c-b40d-454f-8aaa-90a1392ccfb6" property="http://w3id.org/roh/h-index" class="nombre">55 Índice H &nbsp;</span>
            </div>
            <ul class="no-list-style d-flex">
                <li>
                    <a class="btn btn-primary btn-seguir">
                        <span class="seguir">
                            <span class="material-icons">person_add_alt_1</span>
                            <span>Añadir al clúster</span>
                        </span>
                        <span class="no-seguir">
                            <span class="material-icons">person_remove_alt_1</span>
                            <span>Quitar del clúster</span>
                        </span>
                    </a>
                </li>
                <li>
                    <a class="btn btn-outline-grey d-none d-xl-flex">
                        <span class="material-icons-outlined">folder</span>
                        <span>Guardar en Espacio Personal</span>
                    </a>
                </li>
                <li>
                    <div class="acciones-recurso-listado">
                        <div class="dropdown">
                            <a href="#" class="dropdown-toggle btn btn-outline-grey no-flecha" role="button" id="dropdownMasOpciones" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <span class="material-icons">more_vert</span>
                            </a>
                            <div class="dropdown-menu basic-dropdown dropdown-icons dropdown-menu-right" aria-labelledby="dropdownMasOpciones">
                                <ul class="no-list-style">
                                    <li>
                                        <a class="item-dropdown">
                                            <span class="material-icons-outlined">folder</span>
                                            <span class="texto">Guardar en Espacio Personal</span>
                                        </a>
                                    </li>
                                    <li>
                                        <a class="item-dropdown">
                                            <span class="material-icons">send</span>
                                            <span class="texto">Enviar mensaje</span>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>
        </div>
    </div>
    <div class="description-wrap">
        <div class="desc">
            <p>
                Gerente de investigación e investigador senior en el campo de la interacción humano-computadora con enfoque en sistemas inteligentes interactivos, computación portátil, visualización, realidad virtual y aumentada. Fuerte experiencia en experiencia de usuario, metodología de evaluación UX, ciencia y tecnología de la información, ciencias de la computación, gráficos por computadora, métodos científicos. Capacidad para realizar investigaciones independientes. Capacidad para construir, capacitar y liderar equipos de investigación para lograr los niveles más altos de becas.
            </p>
            <p>
                E-learning occupies an increasingly prominent place in education. It provides the learner with a rich virtual network where he or she can exchange ideas and information and create synergies through interactions with other members of the network, whether fellow learners or teachers. Social network analysis (SNA) has proven extremely powerful at describing and analysing network behaviours in business, economics and medicine, but its application to e-learning has been relatively limited.
            </p>
        </div>
        <p class="moreResults showMore">
            <a href="javascript: void(0);" class="ver-mas" style="display: flex;">Ver más</a>
            <a href="javascript: void(0);" class="ver-menos" style="display: none;">Ver menos</a>
        </p>
    </div>
    <div class="info-investigador d-flex flex-wrap">
        <div class="bloque">
            <div class="label">Centro</div>
            <div class="data">Centro Investigación</div>
        </div>
        <div class="bloque">
            <div class="label">Departamento</div>
            <div class="data">Química</div>
        </div>
        <div class="bloque">
            <div class="label">Áreas de conocimiento</div>
            <div class="data">Química Orgánica</div>
        </div>
        <div class="bloque">
            <div class="label">Equipo de investigación</div>
            <div class="data">Bioconjugación de proteínas</div>
        </div>
    </div>
</div>