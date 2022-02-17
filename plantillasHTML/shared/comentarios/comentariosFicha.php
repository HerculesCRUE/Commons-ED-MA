<div class="resource-comentarios" id="comentarios">
    <div class="comentarios-header">
        <h2 class="seccion-title"><span class="material-icons">mode_comment</span>Comentarios</h2>
    </div>
    <div id="panComentario" class="escribir-comentario">
        <div class="comentario">
            <div class="bloque-comentario">
                <div class="col-usuario">
                    <?php include './shared/usuario/user-miniatura-solo-imagen.php' ?>
                </div>
                <div class="col-comentario">
                    <div class="form-group">
                        <!-- Dejo el textarea visible, cuando se implemente el cke editor habrá que ocultarlo -->
                        <textarea placeholder="Deja aquí tu comentario" class="form-control not-outline cke" id="txtNuevoComentario_31e3ca8b-e40f-43f9-9364-b25c22187145" rows="2" cols="20"></textarea>
                    </div>
                    <div class="accion-comentario">
                        <a class="btn btn-outline-grey" onclick="Comentario_CrearComentario('http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment', '31e3ca8b-e40f-43f9-9364-b25c22187145');">Publicar un comentario</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="panComentarios" class="lista-comentarios">
        <div class="comentario" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a"  resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
            <div class="bloque-comentario">
                <div class="col-comentario">
                    <div class="comentario-header">
                        <div class="user-miniatura">
                            <div class="nombre-usuario-wrap">
                                <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                    <span class="nombre" property="foaf:name">Esteban Ramírez</span>
                                </a>
                            </div>
                        </div>
                        <div class="fecha-publicacion">
                            <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 34 minutos</span>
                        </div>
                    </div>
                    <div class="comentario-body">
                        <div property="sioc:content" class="comentario-contenido">
                            <p></p>
                            <p>Vestibulum turpis arcu, elementum ac tellus ut, tincidunt venenatis mauris. Sed sed nisl quis arcu rhoncus congue.</p>

                            <p>Nam libero nulla, eleifend quis urna quis, commodo lacinia quam. In tristique vel urna elementum malesuada. Etiam viverra non libero sit amet consectetur. Cras dolor dui, fringilla laoreet mi a, tristique ultricies erat. In odio metus, laoreet at ante eget, semper viverra justo. In nec laoreet nisi. Praesent ac tincidunt diam.</p>

                            <p>Suspendisse potenti. Vivamu&nbsp;ac erat aliquam, convallis ligula quis, accumsan metus. Integer molestie mollis semper. In semper nisi quis egestas fringilla. Nullam sit amet diam quis libero semper luctus quis quis velit.</p>
                            <p></p>
                        </div>
                    </div>
                    <?php include './shared/acciones/acciones-comentario.php'; ?>
                </div>
                <div class="col-usuario">
                    <?php include './shared/usuario/user-miniatura-solo-imagen.php' ?>
                </div>
            </div>
            <div class="bloque-respuestas">
                <div class="comentario-respuestas">
                    <div class="comentario" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a"  resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
                        <div class="bloque-comentario">
                            <div class="col-comentario">
                                <div class="comentario-header">
                                    <div class="user-miniatura">
                                        <div class="nombre-usuario-wrap">
                                            <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                                <span class="nombre" property="foaf:name">Andrés Jiménez</span>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="fecha-publicacion">
                                        <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 10 minutos</span>
                                    </div>
                                </div>
                                <div class="comentario-body">
                                    <div property="sioc:content" class="comentario-contenido">
                                        <p></p>
                                        <p>Vestibulum turpis arcu, elementum ac tellus ut, u rhoncus congue.</p>
                                        <p></p>
                                    </div>
                                </div>
                                <?php include './shared/acciones/acciones-comentario.php'; ?>
                            </div>
                            <div class="col-usuario">
                                <?php include './shared/usuario/user-miniatura-solo-imagen.php' ?>
                            </div>
                        </div>
                    </div>
                    <div class="comentario usuario-actual" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a"  resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
                        <div class="bloque-comentario">
                            <div class="col-comentario">
                                <div class="comentario-header">
                                    <div class="user-miniatura">
                                        <div class="nombre-usuario-wrap">
                                            <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                                <span class="nombre" property="foaf:name">Yo</span>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="fecha-publicacion">
                                        <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 10 minutos</span>
                                    </div>
                                </div>
                                <div class="comentario-body">
                                    <div property="sioc:content" class="comentario-contenido">
                                        <p></p>
                                        <p>Suspendisse potenti. Vivamu&nbsp;ac erat aliquam, convallis ligula quis, accumsan metus. Integer molestie mollis semper. In semper nisi quis egestas fringilla. Nullam sit amet diam quis libero semper luctus quis quis velit.</p>
                                        <p></p>
                                    </div>
                                </div>
                                <?php include './shared/acciones/acciones-comentario-propio.php'; ?>
                            </div>
                            <div class="col-usuario">
                                <?php include './shared/usuario/user-miniatura-solo-imagen-2.php' ?>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>        <div class="comentario" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a"  resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
            <div class="bloque-comentario">
                <div class="col-comentario">
                    <div class="comentario-header">
                        <div class="user-miniatura">
                            <div class="nombre-usuario-wrap">
                                <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                    <span class="nombre" property="foaf:name">Esteban Ramírez</span>
                                </a>
                            </div>
                        </div>
                        <div class="fecha-publicacion">
                            <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 34 minutos</span>
                        </div>
                    </div>
                    <div class="comentario-body">
                        <div property="sioc:content" class="comentario-contenido">
                            <p></p>
                            <p>Vestibulum turpis arcu, elementum ac tellus ut, tincidunt venenatis mauris. Sed sed nisl quis arcu rhoncus congue.</p>

                            <p>Nam libero nulla, eleifend quis urna quis, commodo lacinia quam. In tristique vel urna elementum malesuada. Etiam viverra non libero sit amet consectetur. Cras dolor dui, fringilla laoreet mi a, tristique ultricies erat. In odio metus, laoreet at ante eget, semper viverra justo. In nec laoreet nisi. Praesent ac tincidunt diam.</p>

                            <p>Suspendisse potenti. Vivamu&nbsp;ac erat aliquam, convallis ligula quis, accumsan metus. Integer molestie mollis semper. In semper nisi quis egestas fringilla. Nullam sit amet diam quis libero semper luctus quis quis velit.</p>
                            <p></p>
                        </div>
                    </div>
                    <?php include './shared/acciones/acciones-comentario.php'; ?>
                </div>
                <div class="col-usuario">
                    <?php include './shared/usuario/user-miniatura-solo-imagen.php' ?>
                </div>
            </div>
            <div class="bloque-respuestas">
                <div class="comentario-respuestas">
                    <div class="comentario" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a"  resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
                        <div class="bloque-comentario">
                            <div class="col-comentario">
                                <div class="comentario-header">
                                    <div class="user-miniatura">
                                        <div class="nombre-usuario-wrap">
                                            <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                                <span class="nombre" property="foaf:name">Andrés Jiménez</span>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="fecha-publicacion">
                                        <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 10 minutos</span>
                                    </div>
                                </div>
                                <div class="comentario-body">
                                    <div property="sioc:content" class="comentario-contenido">
                                        <p></p>
                                        <p>Vestibulum turpis arcu, elementum ac tellus ut, u rhoncus congue.</p>
                                        <p></p>
                                    </div>
                                </div>
                                <?php include './shared/acciones/acciones-comentario.php'; ?>
                            </div>
                            <div class="col-usuario">
                                <?php include './shared/usuario/user-miniatura-solo-imagen.php' ?>
                            </div>
                        </div>
                    </div>
                    <div class="comentario usuario-actual" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a"  resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
                        <div class="bloque-comentario">
                            <div class="col-comentario">
                                <div class="comentario-header">
                                    <div class="user-miniatura">
                                        <div class="nombre-usuario-wrap">
                                            <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                                <span class="nombre" property="foaf:name">Yo</span>
                                            </a>
                                        </div>
                                    </div>
                                    <div class="fecha-publicacion">
                                        <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 10 minutos</span>
                                    </div>
                                </div>
                                <div class="comentario-body">
                                    <div property="sioc:content" class="comentario-contenido">
                                        <p></p>
                                        <p>Suspendisse potenti. Vivamu&nbsp;ac erat aliquam, convallis ligula quis, accumsan metus. Integer molestie mollis semper. In semper nisi quis egestas fringilla. Nullam sit amet diam quis libero semper luctus quis quis velit.</p>
                                        <p></p>
                                    </div>
                                </div>
                                <?php include './shared/acciones/acciones-comentario-propio.php'; ?>
                            </div>
                            <div class="col-usuario">
                                <?php include './shared/usuario/user-miniatura-solo-imagen-2.php' ?>
                            </div>
                        </div>
                        <div class="bloque-respuestas">
                            <div class="comentario-respuestas">
                                <div class="comentario" rel="sioc:has_discussion" id="2c0aa2b0-798e-4080-a602-0592f27e758a"  resource="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a" typeof="sioc_t:Comment" about="http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/create-comment">
                                    <div class="bloque-comentario">
                                        <div class="col-comentario">
                                            <div class="comentario-header">
                                                <div class="user-miniatura">
                                                    <div class="nombre-usuario-wrap">
                                                        <a typeof="foaf:Person" href="http://pruebas.didactalia.net/comunidad/comedu/perfil/Consultor-GNOSS1">
                                                            <span class="nombre" property="foaf:name">Andrés Jiménez</span>
                                                        </a>
                                                    </div>
                                                </div>
                                                <div class="fecha-publicacion">
                                                    <span content="2021-03-01T10:56:44+01:00" property="dcterms:created">hace 10 minutos</span>
                                                </div>
                                            </div>
                                            <div class="comentario-body">
                                                <div property="sioc:content" class="comentario-contenido">
                                                    <p></p>
                                                    <p>Vestibulum turpis arcu, elementum ac tellus ut, u rhoncus congue.</p>
                                                    <p></p>
                                                </div>
                                            </div>
                                            <?php include './shared/acciones/acciones-comentario.php'; ?>
                                        </div>
                                        <div class="col-usuario">
                                            <?php include './shared/usuario/user-miniatura-solo-imagen.php' ?>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</div>
