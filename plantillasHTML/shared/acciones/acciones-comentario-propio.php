<div class="comentario-acciones">
    <div class="wrap">
        <div>
            <span class="btn btn-outline-grey" onclick="Comentario_EditarComentario('http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a/edit', '2c0aa2b0-798e-4080-a602-0592f27e758a');">
                <span class="material-icons">mode_edit</span>Editar
            </span>
        </div>
        <div>
            <span class="btn btn-outline-grey" onclick="Comentario_EliminarComentario('http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a/delete', '2c0aa2b0-798e-4080-a602-0592f27e758a');">
                <span class="material-icons">delete</span>Eliminar<span class="icono"></span>
            </span>
        </div>
        <div>
            <span class="btn btn-outline-grey" onclick="Comentario_ResponderComentario('http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a/reply', '2c0aa2b0-798e-4080-a602-0592f27e758a');">
                <span class="material-icons">reply</span>Responder
            </span>
        </div>
    </div>
    <div class="wrap-movil">
        <div class="dropdown no-flecha">
            <a href="#" class="dropdown-toggle no-flecha" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                <span class="material-icons">more_vert</span>
            </a>
            <div class="dropdown-menu basic-dropdown dropdown-icons dropdown-menu-right">
                <p class="dropdown-title">Acciones</p>
                <span class="material-icons cerrar-dropdown">close</span>
                <ul class="no-list-style">
                    <li>
                        <span class="item-dropdown" onclick="Comentario_EditarComentario('http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a/reply', '2c0aa2b0-798e-4080-a602-0592f27e758a');">
                            <span class="material-icons">mode_edit</span>
                            <span class="texto">Editar</span>
                        </span>
                    </li>
                    <li>
                        <span class="item-dropdown" onclick="Comentario_EliminarComentario('http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a/reply', '2c0aa2b0-798e-4080-a602-0592f27e758a');">
                            <span class="material-icons">delete</span>
                            <span class="texto">Eliminar</span>
                        </span>
                    </li>
                    <li>
                        <span class="item-dropdown" onclick="Comentario_ResponderComentario('http://pruebas.didactalia.net/comunidad/comedu/recurso/recurso-tipo-archivo-adjunto/31e3ca8b-e40f-43f9-9364-b25c22187145/comentario/2c0aa2b0-798e-4080-a602-0592f27e758a/reply', '2c0aa2b0-798e-4080-a602-0592f27e758a');">
                            <span class="material-icons">reply</span>
                            <span class="texto">Responder</span>
                        </span>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</div>