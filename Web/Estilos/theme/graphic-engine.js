$(document).ready(function () {
    metricas.init();
});

// Año máximo y mínimo para las facetas
var minYear;
var maxYear;
// Página seleccionada.
var idPaginaActual = "";
var tituloPaginaActual;
var ordenPaginaActual;
// Gráfica seleccionada.
var idGraficaActual = "";
var tituloActual;
var tamanioActual;
var ordenActual;
var escalaActual;
// Lista de páginas.
var paginas;
var numPagina = 0;
var isPersonalized = false;
var opcionesDropdown = [];
const { jsPDF } = window.jspdf;

var metricas = {
    // Inicializar
    init: async function () {
        // Estilos bugfix
        $('.block').addClass('no-cms-style');

        if ($('div').hasClass('indicadoresPersonalizados')) {
            isPersonalized = true;
            opcionEditar = {};
            opcionEditar.elemento = `
                <li>
                    <a class="item-dropdown editargrafica" data-toggle="modal" data-target="#modal-editargrafica">
                        <span class="material-icons">edit</span>
                        <span class="texto">${metricas.GetText("EDITAR_GRAFICA")}</span>
                    </a>
                </li>
            `;
            opcionEditar.elementozoom = `
                <li>
                    <a class="item-dropdown editargraficazoom" data-toggle="modal" data-target="#modal-editargrafica">
                        <span class="material-icons">edit</span>
                        <span class="texto">${metricas.GetText("EDITAR_GRAFICA")}</span>
                    </a>
                </li>
            `;
            opcionEditar.identificador = "editargrafica";
            opcionEditar.comportamiento = function (e) {
                // Limpia los campos.
                $("#labelTituloGrafica").val("");
                $("#idSelectorOrden").empty();
                $("#idSelectorTamanyo").val("11").change();
                $("#labelEscalaGrafica").val("");
                $("#labelEscalaSecundariaGrafica").val("");
                idGraficaActual = $(this).closest('article').find("div.show.grafica").attr('id');
                // Leer gráficas de esta página
                var url = url_servicio_graphicengine + "GetGraficasUser"; //"https://localhost:44352/GetGraficasUser"
                var arg = {};
                arg.pPageId = paginas[numPagina].id;
                var orden = 1;
                // Petición para obtener los datos de la página.
                $.get(url, arg, function (listaData) {
                    listaData.forEach(data => {
                        if (data.idRecurso == idGraficaActual) {
                            tituloActual = data.titulo;
                            tamanioActual = data.anchura;
                            ordenActual = data.orden;
                            escalaActual = data.escalas;
                        }
                        $('#idSelectorOrden').append(`
                            <option value="${orden}">${orden}</option>    
                        `)
                        orden++;
                    });
                    // Rellena los campos
                    $("#labelTituloGrafica").val(tituloActual);
                    $("#idSelectorTamanyo").val(tamanioActual).change();
                    $("#idSelectorOrden").val(ordenActual).change();
                    if (!escalaActual) {
                        $("#escalaPrimaria").hide();
                        $("#escalaSecundaria").hide();
                    } else if (escalaActual.includes(',')) {
                        $("#escalaPrimaria").show();
                        $("#escalaSecundaria").show();
                        $("#labelEscalaGrafica").val(escalaActual.split(',')[0]);
                        $("#labelEscalaSecundariaGrafica").val(escalaActual.split(',')[1]);
                    } else {
                        $("#escalaPrimaria").show();
                        $("#escalaSecundaria").hide();
                        $("#labelEscalaGrafica").val(escalaActual);
                    }
                });
            }
            opcionesDropdown.push(opcionEditar);
            opcionEliminar = {};
            opcionEliminar.elemento = `
                <li>
                    <a class="item-dropdown eliminargrafica" data-toggle="modal" data-target="#modal-eliminar">
                        <span class="material-icons">delete</span>
                        <span class="texto">${metricas.GetText("ELIMINAR_GRAFICA")}</span>
                    </a>
                </li>
            `;
            opcionEliminar.elementozoom = `
                <li>
                    <a class="item-dropdown eliminargraficazoom" data-toggle="modal" data-target="#modal-eliminar">
                        <span class="material-icons">delete</span>
                        <span class="texto">${metricas.GetText("ELIMINAR_GRAFICA")}</span>
                    </a>
                </li>
            `;
            opcionEliminar.identificador = "eliminargrafica";
            opcionEliminar.comportamiento = function (e) {
                idGraficaActual = $(this).closest('article').find("div.show.grafica").attr('id');
            }
            opcionesDropdown.push(opcionEliminar);
            await this.getPagesPersonalized();
            this.engancharComportamientos();
            return;
        }

        // Esto impide que las facetas se apliquen a la primera pagina al recargar desde otra pagina
        if (ObtenerHash2().includes('~~~')) {
            // Este codigo se incluye para no borrar las facetas al quitar una
            var splitHash = ObtenerHash2().split('~~~');
            numPagina = ObtenerHash2().split('~~~')[1];
            history.pushState('', 'New URL: ', '?' + splitHash[0]);
        } else if (performance.navigation.type == performance.navigation.TYPE_RELOAD && ObtenerHash2()) {
            // Si se recarga la pagina con filtros, se eliminan estos para que no se apliquen a la 2a pagina por error
            history.pushState('', 'New URL: ', '?');
        }
        opcionGuardar = {};
        opcionGuardar.elemento = `
            <li>
                <a class="item-dropdown guardar">
                    <span class="material-icons">assessment</span>
                    <span class="texto">${metricas.GetText("GUARDAR_GRAFICA")}</span>
                </a>
            </li>
        `;
        opcionGuardar.elementozoom = `
            <li>
                <a class="item-dropdown guardarzoom">
                    <span class="material-icons">assessment</span>
                    <span class="texto">${metricas.GetText("GUARDAR_GRAFICA")}</span>
                </a>
            </li>
        `;
        opcionGuardar.identificador = "guardar";
        opcionGuardar.comportamiento = function (e) {
            // Lipia los campos.
            $("#labelTituloGrafica").val("");
            $("#idSelectorPagina").empty();
            $("#labelTituloPagina").val("");
            $("#idSelectorTamanyo").val("11").change();

            // Obtiene el ID de la gráfica seleccionada.
            idGraficaActual = $(this).closest('article').find("div.show.grafica").data('grafica').idGrafica;

            // Leer paginas de usuario

            var idUsuario = $('.inpt_usuarioID').attr('value');
            var url = url_servicio_graphicengine + "GetPaginasUsuario"; //"https://localhost:44352/GetPaginasUsuario"
            var arg = {};
            arg.pUserId = idUsuario;

            // Petición para obtener los datos de la página.
            $.get(url, arg, function (listaData) {
                listaData.forEach(data => {
                    $('#idSelectorPagina').append(`
                        <option idPagina="${data.idRecurso}">${data.titulo}</option>    
                    `)
                });

                var canvas = $(this).parents('div.wrap').find('div.grafica.show canvas') || $(this).parents('div.wrap').find('div.chartAreaWrapper canvas');
                var parent = $('#modal-agregar-datos').find('.graph-container');
                var pIdGrafica = (canvas).parents('div.grafica').attr('id');
                var ctx;

                parent.css("height", "calc(100vh-100px)");

                $('#modal-agregar-datos').css('display', 'block');
                $('#modal-agregar-datos').css('pointer-events', 'none');
                $('.modal-backdrop').addClass('show');
                $('.modal-backdrop').css('pointer-events', 'auto');
                $('#modal-agregar-datos').addClass('show');

                if ($('#idSelectorPagina').children().length == 0) {
                    $("#createPageRadio").prop("checked", true);
                    $("#selectPageRadio").parent().hide();
                    $('#selectPage').hide();
                    $('#createPage').show();
                } else {
                    $("#selectPageRadio").prop("checked", true);
                    $("#selectPageRadio").parent().show();
                    $('#selectPage').show();
                    $('#createPage').hide();
                }

            });
        };
        opcionesDropdown.push(opcionGuardar);
        paginas = await getPages($(".resource-list.graphView .resource-list-wrap"), $('.inpt_usuarioID').attr('value'), ObtenerHash2());
        this.engancharComportamientosAdmin(true);
        paginas[numPagina].pintarPagina(opcionesDropdown);
        $(`li[num="${numPagina}"] a`).addClass('active');
        this.crearFacetas(paginas[numPagina]);
        this.engancharComportamientos();
        
        return;
    },
    // Facetas
    crearFacetas: function (pPagina) {
        // Si ya existe alguna faceta, no creo nada.
        if ($("div.facetedSearch").length != 0) {
            return;
        }

        // Creo los contenedores
        pPagina.data.listaIdsFacetas.forEach(function (item, index, array) {
            $('div.containerFacetas').append(`
                <div class='facetedSearch'>
                    <div class='box' idfaceta="${item.includes('(((') ? item.split('(((')[0] : item}" reciproca="${item.includes('(((') ? item.split('(((')[1] : ''}"></div>
                </div>
            `);
        });

        // Completo los contenedores
        $('div.box').each(function () {
            metricas.getFaceta(pPagina.data.id, $(this).attr('idfaceta'), pPagina.filtroFacetas);
        });

        // Preparo las etiquetas
        $("#panListadoFiltros").children().remove();
        var filtros = decodeURIComponent(ObtenerHash2());
        filtros = filtros.replaceAll(" & ", "|||");
        var filtrosArray = filtros.split('&');
        for (let i = 0; i < filtrosArray.length; i++) {
            let filtro = filtrosArray[i].replace("|||", " & ");
            let nombre;
            if (filtro === "" || !filtro) {
                continue;
            }
            if (filtro.split('=')[1].includes('@')) {
                nombre = filtro.split('=')[1].split('@')[0].replaceAll("'", "");
                $(".borrarFiltros-wrap").remove();
                $("#panListadoFiltros").append(`
                    <li class="Categoria" filtro="${filtro}">
                        <span>${nombre == 'true' ? 'Sí' : (nombre == 'false' ? 'No' : (nombre.includes('(((') ? nombre.split('(((')[0] : nombre))}</span>
                        <a rel="nofollow" class="remove faceta" name="search=Categoria" href="javascript:;">eliminar</a>
                    </li>
                    <li class="borrarFiltros-wrap">
                        <a class="borrarFiltros" href="javascript:;">Borrar</a>
                    </li>
                `);
            } else if (filtro.split('=')[1].includes('http://')) {
                // Agregado la clase oculto para procesarlo después de que se carguen las facetas.
                $(".borrarFiltros-wrap").remove();
                $("#panListadoFiltros").append(`
                    <li class="Categoria oculto" filtro="${filtro}">
                        <span>${filtro}</span>
                        <a rel="nofollow" class="remove faceta" name="search=Categoria" href="javascript:;">eliminar</a>
                    </li>
                    <li class="borrarFiltros-wrap">
                        <a class="borrarFiltros" href="javascript:;">Borrar</a>
                    </li>
                `);
            } else {
                nombre = filtro.split('=')[1].replaceAll("'", "");
                if (nombre === "lastyear") {
                    nombre = this.GetText("ULTIMO_ANIO");
                } else if (nombre === "fiveyears") {
                    nombre = this.GetText("ULTIMOS_CINCO_ANIOS");
                }
                $(".borrarFiltros-wrap").remove();
                $("#panListadoFiltros").append(`
                    <li class="Categoria" filtro="${filtro}">
                        <span>${nombre == 'true' ? 'Sí' : (nombre == 'false' ? 'No' : (nombre.includes('(((') ? nombre.split('(((')[0] : nombre))}</span>
                        <a rel="nofollow" class="remove faceta" name="search=Categoria" href="javascript:;">eliminar</a>
                    </li>
                    <li class="borrarFiltros-wrap">
                        <a class="borrarFiltros" href="javascript:;">Borrar</a>
                    </li>
                `);
            }
        }
    },
    getFaceta: function (pIdPagina, pIdFaceta, pFiltroFacetas) {
        var that = this;
        var url = url_servicio_graphicengine + "GetFaceta"; //"https://localhost:44352/GetFaceta"
        var arg = {};

        arg.pIdPagina = pIdPagina;
        arg.pIdFaceta = pIdFaceta;
        arg.pFiltroFacetas = pFiltroFacetas;
        arg.pLang = lang;

        // Año máximo y mínimo para las facetas de años.
        minYear = 10000;
        maxYear = 0;

        // Petición para obtener los datos de las gráficas.

        $.get(url, arg, function (data) {
            if ($("div[idFaceta='" + pIdFaceta + "']").find("*").length > 0) {
                return;
            }

            var numItemsPintados = 0;
            if (data.isDate) {
                $('div[idfaceta="' + data.id + '"]').append(`
                <span class="faceta-title">${data.nombre}</span>
                <span class="facet-arrow"></span>
                <div class="faceta-date-range">
                <div id="gmd_ci_daterange" class="ui-slider ui-corner-all ui-slider-horizontal ui-widget ui-widget-content">
                    <div class="ui-slider-range ui-corner-all ui-widget-header"></div>
                        <span tabindex="0" class="ui-slider-handle ui-corner-all ui-state-default" style="left: 0%;"></span>
                        <span tabindex="1" class="ui-slider-handle ui-corner-all ui-state-default" style="left: 100%;"></span>
                    </div>
                <div class="d-flex" id="inputs_rango"></div>
                    <a name="gmd_ci:date" class="searchButton">Aplicar</a>
                    <ul class="no-list-style">
                        <li>
                            <a href="javascript: void(0);" id="fiveyears">Últimos cinco años</a>
                        </li>
                        <li>
                            <a href="javascript: void(0);" id="lastyear">Último año</a>
                        </li>
                        <li>
                            <a href="javascript: void(0);" id="allyears">Todos</a>
                        </li>
                    </ul>
                </div>
                `);
            } else if (data.tesauro) {
                $('div[idfaceta="' + data.id + '"]').append(`
                    <span class="faceta-title">${data.nombre}</span>
                    <ul class="listadoFacetas">
                        ${metricas.pintarTesauro(data.items)}
                    </ul>
                    <p class="moreResults"><a class="no-close open-popup-link open-popup-link-tesauro" href="#" data-toggle="modal" faceta="0" data-target="#modal-tesauro">Ver todos</a></p>
                `);
            } else {
                $('div[idfaceta="' + data.id + '"]').append(`
                    <span class="faceta-title">${data.nombre}</span>
                    <span class="facet-arrow"></span><ul class="listadoFacetas"></ul>
                `);
                if (data.verTodos) {
                    $('div[idfaceta="' + data.id + '"]').append(`
                        <p class="moreResults"><a class="no-close open-popup-link open-popup-link-resultados" href="#" data-toggle="modal" faceta="6" data-target="#modal-resultados">Ver todos</a></p>
                    `);
                }
            }

            data.items.forEach(function (item, index, array) {
                // Límite de los ítems de las facetas para mostrar.
                if (numItemsPintados == data.numeroItemsFaceta) {
                    return;
                }

                var filtros = decodeURIComponent(ObtenerHash2());
                var filtrosArray = filtros.split('&');
                var contieneFiltro = false;

                for (var i = 0; i < filtrosArray.length; i++) {
                    if (filtrosArray[i] != '') {
                        if (filtrosArray[i] == item.filtro) {
                            contieneFiltro = true;
                        }
                    }
                }

                if (data.isDate) {
                    minYear = Math.min(minYear, item.nombre);
                    maxYear = Math.max(maxYear, item.nombre);
                } else if (!data.tesauro) {
                    $('div[idfaceta="' + data.id + '"] .listadoFacetas').append(`
                        <li>
                            <a href="javascript: void(0);" class="faceta filtroMetrica" filtro="${item.filtro}">
                                <span class="textoFaceta">${item.nombre == 'true' ? 'Sí' : (item.nombre == 'false' ? 'No' : item.nombre)}</span>
                                <span class="num-resultados">(${item.numero})</span>
                            </a>
                        </li>
                    `);
                    if (filtros.includes(item.filtro)) {
                        // Negrita
                        $('li').find(`[filtro='${item.filtro}']`).addClass("applied");
                    }
                } else {
                    // Negrita del tesauro.
                    for (var i = 0; i < filtrosArray.length; i++) {
                        $(`a[filtro="${filtrosArray[i]}"]`).addClass("applied");
                    }
                }
                numItemsPintados++;
            });
            if (data.isDate) {
                if (minYear == 10000 && maxYear == 0) {
                    minYear = new Date().getFullYear();
                    maxYear = minYear;
                }
                $('div[idfaceta="' + data.id + '"] #inputs_rango').append(`
                    <input title="Año" type="number" min="${minYear}" max="${maxYear}" autocomplete="off" class="filtroFacetaFecha hasDatepicker minVal" placeholder="${minYear}" value="${minYear}" name="gmd_ci_datef1" id="gmd_ci_datef1">
                    <input title="Año" type="number" min="${minYear}" max="${maxYear}" autocomplete="off" class="filtroFacetaFecha hasDatepicker maxVal" placeholder="${maxYear}" value="${maxYear}" name="gmd_ci_datef2" id="gmd_ci_datef2">
                `)
            }
            that.corregirFiltros();
            that.engancharComportamientos();
        });
    },
    // Administrar gráficas
    engancharComportamientosAdmin: function (addOption = false) {
        var url = url_servicio_graphicengine + "IsAdmin"; //"https://localhost:44352/IsAdmin";
        var arg = {};
        arg.pLang = lang;
        arg.pUserId = $('.inpt_usuarioID').attr('value');

        var isAdmin = false;
        $.get(url, arg, function (data) {
            isAdmin = data;
        }).done(function () {
            if (isAdmin) {
                opcionAdmin = {};
                opcionAdmin.elemento = `
                    <li>
                        <a class="item-dropdown admin-config">
                            <span class="material-icons">settings</span>
                            <span class="text">${metricas.GetText("CONFIGURACION")}</span>
                        </a>
                    </li>
                `;
                opcionAdmin.elementozoom = `
                    <li>
                        <a class="item-dropdown admin-configzoom">
                            <span class="material-icons">settings</span>
                            <span class="text">${metricas.GetText("CONFIGURACION")}</span>
                        </a>
                    </li>
                `;
                opcionAdmin.identificador = "admin-config";
                opcionAdmin.comportamiento = function() {
                    $('#modal-admin-config').css('display', 'block');
                    $('#modal-admin-config').css('pointer-events', 'none');
                    $('.modal-backdrop').addClass('show');
                    $('.modal-backdrop').css('pointer-events', 'auto');
                    $('#modal-admin-config').addClass('show');
        
                    var url = url_servicio_graphicengine + "ObtenerGraficaConfig"; //"https://localhost:44352/GetConfiguracion"
                    var args = {}
                    args.pLang = lang;
                    args.pUserId = $('.inpt_usuarioID').attr('value');
                    args.pGraphicId = $(this).closest('article').find('.grafica.show').data('grafica').idGrafica;
                    args.pPageID = paginas[numPagina].id;
                    var numGraficas = $(this).closest('article').parent().find('article').length;
                    var idGrafica = $(this).closest('article').find('.grafica').data('grafica').idGrafica;
                    var idBloque = '';
                    if (args.pGraphicId != idGrafica) {
                        idBloque = idGrafica;
                        idGrafica = args.pGraphicId;
                    }
                    $("#modal-admin-config #idSelectorOrden").empty();
                    for (var i = 0; i < numGraficas; i++) {
                        $("#modal-admin-config #idSelectorOrden").append(`
                            <option value="${i + 1}">${i + 1}</option>
                        `)
                    }
                    var parent = $(this).parents('article');
                    var index = parent.index();
                    $("#idSelectorOrden").val(index + 1).change();
                    $.get(url, args, function (listaData) {
                        $("#modal-admin-config #labelTituloGraficaConfig").val(listaData.nombre[lang]);
                        $("#modal-admin-config #idSelectorTamanyoConfig").val(listaData.anchura).change();
                        $("#modal-admin-config #btnGuardarGraficaConfig").attr('idgrafica', idGrafica);
                        if (idBloque != '') {
                            $("#modal-admin-config #btnGuardarGraficaConfig").attr('idbloque', idBloque);
                        }
                })};
                if (addOption) {
                    opcionesDropdown.push(opcionAdmin);
                }
                if ($(".pageOptions .admin-page").length == 0) {
                    $('.pageOptions').append(`
                        <div class="admin-page">
                            <span class="material-icons btn-download-page">manage_accounts</span>
                            <p>${metricas.GetText("ADMINISTRAR_GRAFICAS")}</p>
                        </div>
                    `);
                }
                $('div.admin-page')
                    .unbind()
                    .click(function (e) {
                        metricas.clearPage();
                        metricas.createAdminPage();
                });
            }
        });
    },
    createAdminPage: function () {
        var url = url_servicio_graphicengine + "ObtenerConfigs"; //"https://localhost:44352/ObtenerConfigs";
        var arg = {};
        arg.pLang = lang;
        arg.pUserId = $('.inpt_usuarioID').attr('value');
        if ($("table.tablaAdmin").length != 0) {
            return;
        }
        $("main div.row-content").append(`
            <table class="tablaAdmin">
                <tbody>
                    <tr>
                        <th>Fichero</th>
                        <th>Subir</th>
                    </tr>
                </tbody>
            </table>
        `);
        $.get(url, arg, function (data) {
            data.forEach(function (jsonName, index) {
                jsonName = jsonName.substring(1);
                var link = url_servicio_graphicengine + "DescargarConfig";
                link += "?pLang=" + lang;
                link += "&pConfig=" + jsonName;
                link += "&pUserId=" + $('.inpt_usuarioID').attr('value');
                $("table.tablaAdmin tbody").append(`
                    <tr id="${index}">
                        <td><a href="${link}" id="jsonName">${jsonName}</a></td> 
                        <td class="subir">
                            <input type="file">
                            <a class="btn btn-primary subir">Subir</a>   
                        </td>
                    </tr>
                `)
            });
            metricas.engancharComportamientos();
            $(`div.download-page`).unbind();
            $('.admin-page').css("color", "var(--c-primario)");
            $('a.nav-link.active').removeClass('active');
        });
    },
    // Indicadores personales
    getPagesPersonalized: async function () {
        var userId = $('.inpt_usuarioID').attr('value');
        if (userId == "ffffffff-ffff-ffff-ffff-ffffffffffff") { // Sin usuario
            $('div.row-content').append(`
                <div class="container">
                    <div class="row-content">
                        <div class="row">
                            <div class="col">
                                <div class="form panel-centrado">
                                    <h1>${metricas.GetText("NO_HAS_INICIADO_SESION")}</h1>
                                    <p>${metricas.GetText("DESCRIPCION_NO_HAS_INICIADO_SESION")}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            `);
        } else {
            var paginas = await getPagesUser($(".resource-list-wrap")[0], userId);
            // Petición para obtener los datos de la página.
            if (paginas.length == 0) {
                $('div.row-content').append(`
                        <div class="container">
                            <div class="row-content">
                                <div class="row">
                                    <div class="col">
                                        <div class="formz panel-centrado">
                                            <h1>${metricas.GetText("NO_TIENES_INDICADORES")}</h1>
                                            <p>${metricas.GetText("COMIENZA_A_CREAR_INDICADORES")}</p>
                                            <p><a href="${metricas.GetText("URL_INDICADORES")}">${metricas.GetText("LA_PAGINA_DE_INDICADORES")}</a>.</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `);
            } else {
                for (let i = 0; i < paginas.length; i++) {
                    $(".listadoMenuPaginas").append(`
                    <li class="nav-item" id="${paginas[i].id}" num="${i}">
                        <a class="nav-link ${i == 0 ? "active" : ""} uppercase">${paginas[i].title}</a>
                    </li>
                `);
                }
                $('.topbar-container').css('display', 'flex');
                await paginas[0].pintarPagina(opcionesDropdown);
            }
        }
    },
    // Comportamientos
    engancharComportamientos: function (cyto = null) {
        var that = this;
        numPagina = $('.listadoMenuPaginas').find('a.active').parent().attr('num');

        // Comportamientos facetas



        $(".faceta-date-range .ui-slider").slider({
            range: true,
            min: minYear,
            max: maxYear,
            values: [minYear, maxYear],
            slide: function (event, ui) {
                $("#gmd_ci_datef1").val(ui.values[0]);
                $("#gmd_ci_datef2").val(ui.values[1]);
            }
        });

        $(".faceta-date-range").find("input.filtroFacetaFecha").on("input", function (event, ui) {
            var valores = $(".faceta-date-range .ui-slider").slider("option", "values");

            if ($(this).attr("id") === "gmd_ci_datef1") {
                valores[0] = this.value;
            } else {
                valores[1] = this.value;
            }
            $(".faceta-date-range .ui-slider").slider("values", valores);
        });

        $('.containerFacetas a.filtroMetrica,.listadoTesauro a.filtroMetrica, .indice-lista .faceta')
            .unbind()
            .click(function (e) {
                var filtroActual = $(this).attr('filtro');
                if ($(this).parents('.box').attr('reciproca')) {
                    filtroActual = filtroActual + '(((' + $(this).parents('.box').attr('reciproca');
                }
                var filtros = decodeURIComponent(ObtenerHash2());
                var filtrosArray = filtros.split('&');
                filtros = '';
                var contieneFiltro = false;
                for (var i = 0; i < filtrosArray.length; i++) {
                    if (filtrosArray[i] != '') {
                        if (filtrosArray[i] == filtroActual) {
                            contieneFiltro = true;
                        } else {
                            filtros += filtrosArray[i] + '&';
                        }

                    }
                }
                if (!contieneFiltro) {
                    filtros += filtroActual;
                }

                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                e.preventDefault();

                location.reload();
        });

        $('a.remove.faceta')
            .unbind()
            .click(function (e) {
                var filtroActual = $(this).parent().attr('filtro');
                var filtros = decodeURIComponent(ObtenerHash2());
                filtros = filtros.replaceAll(" & ", "|||");
                var filtrosArray = filtros.split('&');
                filtros = '';
                var contieneFiltro = false;
                for (var i = 0; i < filtrosArray.length; i++) {
                    let filtro = filtrosArray[i].replace("|||", " & ");
                    if (filtro != '') {
                        if (filtro == filtroActual) {
                            contieneFiltro = true;
                        } else {
                            filtros += filtro + '&';
                        }
                    }
                }
                if (!contieneFiltro) {
                    filtros += filtroActual;
                }
                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                e.preventDefault();

                location.reload();
        });

        $('.borrarFiltros')
            .unbind()
            .click(function (e) {
                history.pushState('', 'New URL: ', '?');
                e.preventDefault();
                location.reload();
        });

        $('#fiveyears')
            .unbind()
            .click(function (e) {
                var filtro = $(this).parent().parent().parent().parent().attr('idfaceta');
                var filtroActual = `${filtro}=fiveyears`;
                var filtros = decodeURIComponent(ObtenerHash2());
                var filtrosArray = filtros.split('&');
                filtros = '';
                for (var i = 0; i < filtrosArray.length; i++) {
                    if (filtrosArray[i] != '') {
                        filtros += filtrosArray[i] + '&';
                    }
                }
                // Borrar filtros año anteriores
                var reg = new RegExp(filtro + "=[0-9]*-[0-9]*");
                if (filtros.includes(filtro)) {
                    filtros = filtros.replace(reg, "");
                    filtros = filtros.replace(filtro + "=lastyear", "");
                    filtros = filtros.replace(filtro + "=fiveyears", "");
                }
                filtros += filtroActual;
                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                e.preventDefault();

                location.reload();
        });

        $('#lastyear')
            .unbind()
            .click(function (e) {
                var filtro = $(this).parent().parent().parent().parent().attr('idfaceta');
                var filtroActual = `${filtro}=lastyear`;
                var filtros = decodeURIComponent(ObtenerHash2());
                var filtrosArray = filtros.split('&');
                filtros = '';
                for (var i = 0; i < filtrosArray.length; i++) {
                    if (filtrosArray[i] != '') {
                        filtros += filtrosArray[i] + '&';
                    }
                }
                // Borrar filtros año anteriores
                var reg = new RegExp(filtro + "=[0-9]*-[0-9]*");
                if (filtros.includes(filtro)) {
                    filtros = filtros.replace(reg, "");
                    filtros = filtros.replace(filtro + "=lastyear", "");
                    filtros = filtros.replace(filtro + "=fiveyears", "");
                }
                filtros += filtroActual;

                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                e.preventDefault();

                location.reload();
        });

        $('#allyears')
            .unbind()
            .click(function (e) {
                var filtro = $(this).parent().parent().parent().parent().attr('idfaceta');
                var filtros = decodeURIComponent(ObtenerHash2());
                var filtrosArray = filtros.split('&');
                filtros = '';
                for (var i = 0; i < filtrosArray.length; i++) {
                    if (filtrosArray[i] != '') {
                        filtros += filtrosArray[i] + '&';
                    }
                }
                // Borrar filtros año anteriores
                var reg = new RegExp(filtro + "=[0-9]*-[0-9]*");
                if (filtros.includes(filtro)) {
                    filtros = filtros.replace(reg, "");
                    filtros = filtros.replace(filtro + "=lastyear", "");
                    filtros = filtros.replace(filtro + "=fiveyears", "");
                }

                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                e.preventDefault();

                location.reload();
        });

        $('.faceta-date-range a.searchButton')
            .unbind()
            .click(function (e) {
                var min, max;
                // Cojo el valor del input y si no tiene le pongo el placeholder
                $("#gmd_ci_datef1").val() === '' ? min = $("#gmd_ci_datef1").attr("placeholder") : min = $("#gmd_ci_datef1").val();
                $("#gmd_ci_datef2").val() === '' ? max = $("#gmd_ci_datef2").attr("placeholder") : max = $("#gmd_ci_datef2").val();
                var filtro = $(this).parent().parent().attr('idfaceta');
                var filtroActual = `${filtro}=${min}-${max}`;
                var filtros = decodeURIComponent(ObtenerHash2());
                var filtrosArray = filtros.split('&');
                filtros = '';
                for (var i = 0; i < filtrosArray.length; i++) {
                    if (filtrosArray[i] != '') {
                        filtros += filtrosArray[i] + '&';
                    }
                }
                // Borrar filtros año anteriores
                var reg = new RegExp(filtro + "=[0-9]*-[0-9]*");
                if (filtros.includes(filtro)) {
                    filtros = filtros.replace(reg, "");
                    filtros = filtros.replace(filtro + "=lastyear", "");
                    filtros = filtros.replace(filtro + "=fiveyears", "");
                }
                filtros += filtroActual;

                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                e.preventDefault();

                location.reload();
        });

        // Comportamiento admin
        $('table.tablaAdmin td.subir a.btn.subir')
            .unbind()
            .click(function (e) {
                var url = url_servicio_graphicengine + "SubirConfig";
                var formData = new FormData();
                formData.append('pConfigName', $(this).closest('tr').find('a#jsonName').text());
                formData.append('pUserID', $('.inpt_usuarioID').attr('value'));
                formData.append('pLang', lang);
                formData.append('pConfigFile', $(this).parent().find('input[type=file]')[0].files[0]);

                $.ajax({
                    url: url,
                    type: 'POST',
                    data: formData,
                    cache: false,
                    processData: false,
                    enctype: 'multipart/form-data',
                    contentType: false,
                    success: function (response) {
                        if (response) {
                            mostrarNotificacion('success', 'Configuración subida correctamente');
                            location.reload();
                        } else {
                            mostrarNotificacion('error', 'Error al subir la configuración');
                        }
                    },
                    error: function (response) {
                        mostrarNotificacion('error', 'Error al subir la configuración');
                    }

                });
            });

        $('table.tablaAdmin a.btn.editar')
            .unbind()
            .click(function (e) {
                $('#modal-editar-configuracion').css('display', 'block');
                $('#modal-editar-configuracion').css('pointer-events', 'none');
                $('.modal-backdrop').addClass('show');
                $('.modal-backdrop').css('pointer-events', 'auto');
                $('#modal-editar-configuracion').addClass('show');
                var url = url_servicio_graphicengine + "ObtenerPaginaConfig";
                var arg = {};

                arg.pUserId = $('.inpt_usuarioID').attr('value');
                //arg.pageID = $(this).closest('tr').id;
                arg.pConfig = $(this).closest('tr').find('a#jsonName').text();
                arg.pLang = lang;
                $.get(url, arg, function (data) {
                    $('#labelTituloPaginaEditar').val(data.nombre);
                    var numPaginas = $(".tablaAdmin").find('tr').length - 1;
                    $('#idSelectorOrdenPagina').empty();
                    for (var i = 0; i < numPaginas; i++) {
                        $('#idSelectorOrdenPagina').append(`
                            <option value="${i + 1}">${i + 1}</option>
                        `)
                    }

                    $('#idSelectorOrdenPagina').val(data.orden).change();

                });
                url = url_servicio_graphicengine + "ObtenerGraficasConfig";

                $.get(url, arg, function (data) {
                    data.forEach(function (grafica, index, array) {
                        $("#modal-editar-configuracion").find('.modal-body').append(`
                        <div class="custom-form-row">
                            <div class="simple-collapse">
                                <a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-${index}">grafica ${index}</a>
                                <div id="collapse-${index}" class="collapse">
                                    <div class="form-group full-group disabled ">
                                        <label class="control-label d-block"></label>
                                        <input id="labelTituloGrafica" onfocus="" type="text" class="form-control not-outline">
                                    </div>
                                    <div class="form-group full-group disabled">
                                        <label class="control-label d-block">Anchura</label>
                                        <select id="idSelectorTamanyoEditar" class="js-select2 select2-hidden-accessible" dependency="" data-select-search="true" tabindex="-1" aria-hidden="true">
                                            <option value="11">100%</option>
                                            <option value="34">75%</option>
                                            <option value="23">66%</option>
                                            <option value="12">50%</option>
                                            <option value="13">33%</option>
                                            <option value="14">25%</option>
                                        </select>
                                    </div>
                                    <div class="form-group full-group disabled">
                                        <label class="control-label d-block">Orden</label>
                                        <select id="idSelectorOrden" class="js-select2 select2-hidden-accessible" dependency="" data-select-search="true" tabindex="-1" aria-hidden="true">
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>`);

                        $("collapse" + index).find('#labelTituloGrafica').val(grafica.options.plugins.title);
                        var numGraficas = data.length;
                        for (var i = 0; i < numGraficas; i++) {
                            $("collapse-${index}").find('#idSelectorOrden').append(`
                            <option value="${i + 1}">${i + 1}</option>
                        `)
                        }
                        $("collapse-${index}").find('#idSelectorOrden').val(index).change();

                    });


                });

                var orden = "yo que se añadir luego ";
                $('#idSelectorOrdenPagina').empty().append(`
                    <option value="${orden}">${orden}</option>    
                `)
                $('#idSelectorOrden').empty().append(`
                    <option value="${orden}">${orden}</option>    
                `)
            });

        $('div.edit-page')
            .unbind()
            .click(function (e) {
                // Limpia los campos.
                $("#labelTituloPagina").val("");
                $("#idSelectorOrdenPg").empty();
                // Leer páginas
                var url = url_servicio_graphicengine + "GetPaginasUsuario"; //"https://localhost:44352/GetPaginasUsuario"
                var arg = {};
                arg.pUserId = $('.inpt_usuarioID').attr('value');
                var orden = 1;
                // Petición para obtener los datos de la página.
                $.get(url, arg, function (listaData) {
                    listaData.forEach(data => {
                        if (data.idRecurso == paginas[numPagina].id) {
                            tituloPaginaActual = data.titulo;
                            ordenPaginaActual = data.orden;
                        }
                        $('#idSelectorOrdenPg').append(`
                            <option value="${orden}">${orden}</option>    
                        `)
                        orden++;
                    });
                    // Rellena los campos
                    $("#labelTituloPagina").val(tituloPaginaActual);
                    $("#idSelectorOrdenPg").val(ordenPaginaActual).change();
                });
            });

        $(`div.download-page`)
            .unbind()
            .click(function (e) {

                MostrarUpdateProgress();
                $(".acciones-mapa").hide();
                $(".Categoria a").hide();
                $(".borrarFiltros-wrap").hide();
                var htmlsource = $("div.col-contenido ")[0] || $("div.resource-list-wrap")[0];
                html2canvas(htmlsource, { scale: 2, scrolly: -window.scrolly }).then(function (canvas) {
                    var img = canvas.toDataURL();
                    var orientation = canvas.height > canvas.width ? 'portrait' : 'landscape';
                    var doc = new jsPDF(orientation, "mm", [(canvas.height + 200) / 4, (canvas.width + 200) / 4]);
                    doc.addImage(img, 'jpeg', 25, 25, canvas.width / 4, canvas.height / 4);
                    doc.save('Indicadores.pdf');
                    OcultarUpdateProgress();
                    $(".acciones-mapa").show();
                    $(".Categoria a").show();
                    $(".borrarFiltros-wrap").show();
                });
            });

        $('a.eliminar')
            .unbind()
            .click(function (e) {
                // Leer paginas de usuario
                var idUsuario = $('.inpt_usuarioID').attr('value');
                var idPagina = paginas[numPagina].id;
                var idGrafica = idGraficaActual;
                var url = url_servicio_graphicengine + "BorrarGrafica"; //"https://localhost:44352/BorrarGrafica"
                var arg = {};
                arg.pUserId = idUsuario;
                arg.pPageID = idPagina;
                arg.pGraphicID = idGrafica;

                // Petición para eliminar la gráfica.
                MostrarUpdateProgress();
                $.get(url, arg, function (listaData) {
                    location.reload();
                });
            });

        $('a.eliminarpg')
            .unbind()
            .click(function (e) {
                // Leer paginas de usuario
                var idUsuario = $('.inpt_usuarioID').attr('value');
                var idPagina = paginas[numPagina].id;
                var url = url_servicio_graphicengine + "BorrarPagina"; //"https://localhost:44352/BorrarPagina"
                var arg = {};
                arg.pUserId = idUsuario;
                arg.pPageID = idPagina;

                MostrarUpdateProgress();
                // Petición para eliminar la página.
                $.get(url, arg, function (listaData) {
                    location.reload();
                });
            });
        $('#btnGuardarEditPagina')
            .unbind()
            .click(function (e) {
                //TODO translate
                var formTituloPagina = $('#labelTituloPagina').val();
                if (!formTituloPagina) {
                    mostrarNotificacion("warning", "Introduce el titulo de la pagina por favor");
                    return;
                }

                MostrarUpdateProgress();
                var user = $('.inpt_usuarioID').attr('value');
                var url = url_servicio_graphicengine + "EditarNombrePagina"; //"https://localhost:44352/EditarNombrePagina"
                var arg = {};
                arg.pUserId = user;
                arg.pPageID = paginas[numPagina].id;
                arg.pNewTitle = $('#labelTituloPagina').val();
                arg.pOldTitle = tituloPaginaActual;

                $.get(url, arg, function () {
                    var urlOrd = url_servicio_graphicengine + "EditarOrdenPagina"; //"https://localhost:44352/EditarOrdenPagina"
                    var argOrd = {};
                    argOrd.pUserId = user;
                    argOrd.pPageID = paginas[numPagina].id;
                    argOrd.pNewOrder = $('#idSelectorOrdenPg option:selected').val();

                    argOrd.pOldOrder = ordenPaginaActual;

                    $.get(urlOrd, argOrd, function () {
                        location.reload();
                    });
                });
            });

        $('#btnGuardarEditGrafica')
            .unbind()
            .click(function (e) {
                //TODO translate
                var formTitle = $('#labelTituloGrafica').val();
                if (!formTitle) {
                    mostrarNotificacion("warning", "Introduce el titulo por favor");
                    return;
                }

                MostrarUpdateProgress();
                var user = $('.inpt_usuarioID').attr('value');
                var url = url_servicio_graphicengine + "EditarNombreGrafica"; //"https://localhost:44352/EditarNombreGrafica"
                var arg = {};
                arg.pUserId = $('.inpt_usuarioID').attr('value');
                arg.pPageID = paginas[numPagina].id;
                arg.pGraphicID = idGraficaActual;
                arg.pNewTitle = $('#labelTituloGrafica').val();
                arg.pOldTitle = tituloActual;

                $.get(url, arg, function () {
                    var urlOrd = url_servicio_graphicengine + "EditarOrdenGrafica"; //"https://localhost:44352/EditarOrdenGrafica"
                    var argOrd = {};
                    argOrd.pUserId = user;
                    argOrd.pPageID = paginas[numPagina].id;
                    argOrd.pGraphicID = idGraficaActual;
                    argOrd.pNewOrder = $('#idSelectorOrden option:selected').val();
                    argOrd.pOldOrder = ordenActual;

                    $.get(urlOrd, argOrd, function () {
                        var urlAnch = url_servicio_graphicengine + "EditarAnchuraGrafica"; //"https://localhost:44352/EditarAnchuraGrafica"
                        var argAnch = {};
                        argAnch.pUserId = user;
                        argAnch.pPageID = paginas[numPagina].id;
                        argAnch.pGraphicID = idGraficaActual;
                        argAnch.pNewWidth = $('#idSelectorTamanyo option:selected').val();
                        argAnch.pOldWidth = tamanioActual;

                        $.get(urlAnch, argAnch, function () {
                            var urlScal = url_servicio_graphicengine + "EditarEscalasGrafica"; //"https://localhost:44352/EditarEscalasGrafica"
                            var argScal = {};
                            argScal.pUserId = user;
                            argScal.pPageID = paginas[numPagina].id;
                            argScal.pGraphicID = idGraficaActual;
                            var escalas = $('#labelEscalaGrafica').val() == '' ? "" : $('#labelEscalaSecundariaGrafica').val() == '' ? parseInt($('#labelEscalaGrafica').val()) : parseInt($('#labelEscalaGrafica').val()) + "," + parseInt($('#labelEscalaSecundariaGrafica').val());
                            argScal.pNewScales = escalas;
                            argScal.pOldScales = escalaActual;

                            $.get(urlScal, argScal, function () {
                                location.reload();
                            });
                        });
                    });
                });
            });

        $('#btnGuardarGrafica')
            .unbind()
            .click(function (e) {

                //TODO translate
                var formTitle = $('#labelTituloGrafica').val();
                if (!formTitle) {
                    mostrarNotificacion("warning", "Introduce el titulo de la grafica por favor");
                    return;
                }
                var formCreatePageRadio = $('#createPageRadio').is(':checked');
                if (formCreatePageRadio) {
                    var formCreatePage = $('#labelTituloPagina').val();
                    if (!formCreatePage) {
                        mostrarNotificacion("warning", "Introduce el titulo de la pagina por favor");
                        return;
                    }
                }

                var grafica = $("#grafica_" + paginas[numPagina].id + "_" + idGraficaActual);
                var chart = Chart.getChart(grafica.attr('id'));
                if (chart) {
                    var scales = chart.scales;
                } else {
                    var scales = "";
                }
                var isHorizontal = grafica.parents('div.grafica').data('grafica').orientacion == 'horizontal';
                var max = [];

                for (scale in scales) {
                    if (scales[scale].axis == 'y' && !isHorizontal) {
                        max.push(scales[scale].max);
                    } else if (scales[scale].axis == 'x' && isHorizontal) {
                        max.push(scales[scale].max);
                    }
                }
                var escalas = max[0];
                if (max.length > 1) {
                    escalas = max[0] + "," + max[1];
                }


                var url = url_servicio_graphicengine + "GuardarGrafica"; //"https://localhost:44352/GuardarGrafica"
                var arg = {};
                arg.pTitulo = $('#labelTituloGrafica').val();
                arg.pAnchura = $('#idSelectorTamanyo option:selected').val();
                arg.pIdPaginaGrafica = paginas[numPagina].id;
                arg.pIdGrafica = idGraficaActual;
                arg.pFiltros = ObtenerHash2();
                arg.pUserId = $('.inpt_usuarioID').attr('value');
                arg.pEscalas = escalas;

                if ($("#selectPage").is(":visible")) {
                    arg.pIdRecursoPagina = $('#idSelectorPagina option:selected').attr("idPagina");
                } else {
                    arg.pIdRecursoPagina = "";
                }

                if ($("#createPage").is(":visible")) {
                    arg.pTituloPagina = $('#labelTituloPagina').val();
                } else {
                    arg.pTituloPagina = "";
                }

                // Petición para obtener los datos de la página.
                MostrarUpdateProgress();
                $.get(url, arg, function (data) {
                    if (data) {
                        mostrarNotificacion("success", "Grafica guardada correctamente");
                    } else {
                        mostrarNotificacion("error", "Error al guardar la grafica");
                    }
                }).done(function () {
                    OcultarUpdateProgress();
                    cerrarModal();
                });
            });

        $("#btnGuardarGraficaConfig")
            .unbind()
            .click(function (e) {
                var url = url_servicio_graphicengine + "EditarConfig"; //"https://localhost:44352/GuardarGraficaConfig"
                var arg = {};
                arg.pUserId = $('.inpt_usuarioID').attr('value');
                arg.pPageId = paginas[numPagina].id;
                arg.pGraphicId = $(this).attr('idgrafica');
                arg.pLang = lang
                arg.pGraphicName = $('#labelTituloGraficaConfig').val();
                arg.pGraphicWidth = $('#idSelectorTamanyoConfig option:selected').val();
                arg.pGraphicOrder = $('#idSelectorOrden').val();
                if ($(this).attr('idbloque')) {
                    arg.pBlockId = $(this).attr('idbloque');
                }
                $.get(url, arg, function (data) {
                    if (data) {
                        mostrarNotificacion("success", "Grafica guardada correctamente");
                    } else {
                        mostrarNotificacion("error", "Error al guardar la grafica");
                    }
                    location.reload();
                }).fail(function (data) {
                    mostrarNotificacion("error", "Error de servidor al guardar la grafica");
                });
            });

        $('#createPageRadio')
            .unbind()
            .change(function () {
                if (this.checked) {
                    $('#selectPage').hide();
                    $('#createPage').show();
                }
            });

        $('#selectPageRadio')
            .unbind()
            .change(function () {
                if (this.checked) {
                    $('#selectPage').show();
                    $('#createPage').hide();
                }
            });

        // Modales
        $('.modal-backdrop')
            .unbind()
            .click(cerrarModal);
        $('span.cerrar-grafica')
            .unbind()
            .click(cerrarModal);

        function cerrarModal() {
            $('#modal-ampliar-mapa').find('div.graph-container').empty();
            $('#modal-ampliar-mapa').removeClass('show');
            $('.modal-backdrop').removeClass('show');
            $('.modal-backdrop').css('pointer-events', 'none');
            $('#modal-ampliar-mapa').css('display', 'none');
            $('#modal-agregar-datos').removeClass('show');
            $('#modal-agregar-datos').css('display', 'none');
            $('#modal-admin-config').removeClass('show');
            $('#modal-admin-config').css('display', 'none');
        }

        // Cambiar pagina
        $(".listadoMenuPaginas li.nav-item")
            .unbind()
            .click(function (e) {
                var numero = $(this).attr("num");
                numPagina = numero;
                if (isPersonalized) {
                    $(this).parents('ul').find('a.active').removeClass('active');
                    $(this).find('a').addClass('active');
                    metricas.clearPage();
                    if (paginas[numero].data.length != 0) {
                        paginas[numero].pintarPagina(opcionesDropdown);
                    } else {
                        if ($('div.row-content').find('div.sin-graficas').length == 0) {
                            $('div.row-content').append(`
                            <div class="sin-graficas">
                                <div class="container">
                                    <div class="row-content">
                                        <div class="row">
                                            <div class="col">
                                                <div class="form panel-centrado">
                                                    <h1>${metricas.GetText("NO_HAY_GRAFICAS")}</h1>
                                                    <p>${metricas.GetText("PUEDES")} <a href="#" onclick="$('.delete-page').click()">${metricas.GetText("BORRAR_LA_PAGINA")}</a> ${metricas.GetText("O")} <a href="${metricas.GetText("URL_INDICADORES")}">${metricas.GetText("ANIADIR_NUEVAS_GRAFICAS")}</a>.</p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            `);
                        }
                    }
                    metricas.engancharComportamientos();
                } else {
                    $('.admin-page').removeAttr('style');
                    $(this).parents('ul').find('a.active').removeClass('active');
                    $(this).find('a').addClass('active');
                    metricas.clearPage();
                    paginas[numero].filtroFacetas = "";
                    paginas[numero].pintarPagina(opcionesDropdown);
                    metricas.crearFacetas(paginas[numero]);
                    metricas.engancharComportamientos();
                    metricas.engancharComportamientosAdmin();
                }
            });

        plegarSubFacetas.init();
        comportamientoFacetasPopUp.init();

        // Agrega el enganche sin sobreescribir la función.
        $('#panFacetas .open-popup-link-tesauro').unbind('.clicktesauro').bind("click.clicktesauro", (function (event) {
            that.engancharComportamientos();
            $(".modal-body .buscador-coleccion .action-buttons-resultados").remove();
        }));

        $('#panFacetas .open-popup-link-resultados').unbind().click(function (event) {
            $('#modal-resultados').show();
            $(".indice-lista.no-letra").html('');
            event.preventDefault();
            $('#modal-resultados .modal-dialog .modal-content .modal-title').text($($(this).closest('.box')).find('.faceta-title').text());
            comportamientoFacetasPopUp.cargarFaceta($(this).closest('.box').attr('idfaceta'));
            that.engancharComportamientos();
        });
    },
    // Utils
    GetText: function (id, param1, param2, param3, param4) {
        if ($('#' + id).length) {
            var txt = $('#' + id).val();
            if (param1 != null) {
                txt = txt.replace("PARAM1", param1);
            }
            if (param2 != null) {
                txt = txt.replace("PARAM2", param1);
            }
            if (param3 != null) {
                txt = txt.replace("PARAM3", param1);
            }
            if (param4 != null) {
                txt = txt.replace("PARAM4", param1);
            }
            return txt;
        } else {
            return id;
        }
    },
    corregirFiltros: function () {
        // Permite pintar el filtro del tesauro con el nombre del nivel correspondiente.
        $("#panListadoFiltros").each(function () {
            $("#panListadoFiltros").find('li').each(function () {
                if ($(this).hasClass("oculto")) {
                    var valor = $(this).find('span').text();
                    var nombre = $(`a[filtro="${valor}"]`).attr("title");
                    if (nombre != null) {
                        $(this).find('span').text(nombre);
                        $(this).removeClass('oculto');
                    }
                }
            });
        });
    },
    pintarTesauro: function (pData) {
        var etiqueta = "";
        var hijos = "";

        if (pData.length > 0) {

            pData.forEach(function (item, index, array) {
                hijos += metricas.pintarTesauro(item);
            });

            return hijos;

        } else {

            // Si tiene hijos, los pinta llamando a la propia función recursivamente.
            if (pData.childsTesauro.length > 0) {

                etiqueta += `<ul>`;
                pData.childsTesauro.forEach(function (item, index, array) {
                    hijos += metricas.pintarTesauro(item);
                });
                etiqueta += `${hijos}</ul>`;

                return `<li>
                            <a rel="nofollow" href="javascript: void(0);" class="faceta filtroMetrica con-subfaceta ocultarSubFaceta ocultarSubFaceta" filtro="${pData.filtro}" title="${pData.nombre}">
                                <span class="desplegarSubFaceta"><span class="material-icons">add</span></span>
                                <span class="textoFaceta">${pData.nombre == 'true' ? 'Sí' : (pData.nombre == 'false' ? 'No' : pData.nombre)}</span>
                                <span class="num-resultados">(${pData.numero})</span>                          
                            </a>
                            ${etiqueta}
                        </li>`;

            } else {

                return `<li>
                            <a rel="nofollow" href="javascript: void(0);" class="faceta filtroMetrica ocultarSubFaceta ocultarSubFaceta" filtro="${pData.filtro}" title="${pData.nombre}">
                                <span class="textoFaceta">${pData.nombre == 'true' ? 'Sí' : (pData.nombre == 'false' ? 'No' : pData.nombre)}</span>
                                <span class="num-resultados">(${pData.numero})</span>                          
                            </a>
                        </li>`;
            }
        }
    },
    clearPage: function () {
        if (isPersonalized) {
            $('canvas').each(function () {
                Chart.getChart(this)?.destroy();
            });
            $(window).unbind('resize');
            $('.resource-list-wrap').empty();
        } else {
            $('canvas').each(function () {
                Chart.getChart(this)?.destroy();
            });
            $(window).unbind('resize');
            $('#panFacetas').empty();
            $('#panListadoFiltros').empty();
            $('.resource-list-wrap').empty();
            $('table.tablaAdmin').remove();
            history.pushState('', 'New URL: ', '?');
        }

    },
}

comportamientoFacetasPopUp.cargarFaceta = function (pIdFaceta) {
    var that = this;
    var url = url_servicio_graphicengine + "GetFaceta"; //"https://localhost:44352/GetFaceta"
    var arg = {};
    numPagina = $('.listadoMenuPaginas').find('a.active').parent().attr('num');
    arg.pIdPagina = paginas[numPagina].id;
    arg.pIdFaceta = pIdFaceta;
    arg.pFiltroFacetas = ObtenerHash2();
    arg.pLang = lang;
    arg.pGetAll = true;
    that.textoActual = '';
    that.paginaActual = 1;
    // Petición para obtener los datos de las gráficas.
    $.get(url, arg, function (data) {

        $('.buscador-coleccion .buscar .texto').keyup(function () {
            that.textoActual = that.eliminarAcentos($(this).val());
            that.paginaActual = 1;
            that.buscarFacetas();
            $('.indice-lista .faceta')
                .unbind()
                .click(function (e) {
                    var filtroActual = $(this).attr('filtro');
                    var filtros = decodeURIComponent(ObtenerHash2());
                    var filtrosArray = filtros.split('&');
                    filtros = '';
                    var contieneFiltro = false;
                    for (var i = 0; i < filtrosArray.length; i++) {
                        if (filtrosArray[i] != '') {
                            if (filtrosArray[i] == filtroActual) {
                                contieneFiltro = true;
                            } else {
                                filtros += filtrosArray[i] + '&';
                            }

                        }
                    }
                    if (!contieneFiltro) {
                        filtros += filtroActual;
                    } else {
                        location.reload();
                    }

                    history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                    e.preventDefault();

                    location.reload();
                });
        });

        that.arrayTotales = new Array(data.items.length);
        var i = 0;
        data.items.forEach(function (item, index, array) {
            that.arrayTotales[i] = new Array(2);
            that.arrayTotales[i][0] = that.eliminarAcentos(item.nombre.toLowerCase());
            that.arrayTotales[i][1] = $(`<a href="javascript: void(0);" class="faceta filtroMetrica" filtro="${item.filtro}">
                                <span class="textoFaceta">${item.nombre == 'true' ? 'Sí' : (item.nombre == 'false' ? 'No' : item.nombre)}</span>
                                <span class="num-resultados">(${item.numero})</span>
                            </a>`);
            i++;
        });

        //Ordena por orden alfabético
        that.arrayTotales = that.arrayTotales.sort(function (a, b) {
            if (a[0] > b[0]) return 1;
            if (a[0] < b[0]) return -1;
            return 0;
        });

        that.paginaActual = 1;

        $(".modal-body .buscador-coleccion .action-buttons-resultados").remove();
        $(".modal-body .buscador-coleccion").append($('<div></div>').attr('class', 'action-buttons-resultados'));
        $(".modal-body .buscador-coleccion .action-buttons-resultados").append($('<ul></ul>').attr('class', 'no-list-style'));

        $(".modal-body .buscador-coleccion .action-buttons-resultados .no-list-style").append($('<li></li>').attr('class', 'js-anterior-facetas-modal'));
        $(".modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-anterior-facetas-modal").append($('<span></span>').attr('class', 'material-icons').text('navigate_before'));
        $(".modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-anterior-facetas-modal").append($('<span></span>').attr('class', 'texto').text('Anteriores'));

        $(".modal-body .buscador-coleccion .action-buttons-resultados .no-list-style").append($('<li></li>').attr('class', 'js-siguiente-facetas-modal'));
        $(".modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-siguiente-facetas-modal").append($('<span></span>').attr('class', 'texto').text('Siguientes'));
        $(".modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-siguiente-facetas-modal").append($('<span></span>').attr('class', 'material-icons').text('navigate_next'));


        $('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-anterior-facetas-modal .texto').click(function () {
            if (!that.buscando && that.paginaActual > 1) {
                that.buscando = true;
                that.paginaActual--;
                var hacerPeticion = true;
                $('.indice-lista ul').animate({
                    marginLeft: 30,
                    opacity: 0
                }, 200, function () {
                    if (hacerPeticion) {
                        that.buscarFacetas();
                        $('.indice-lista .faceta')
                            .unbind()
                            .click(function (e) {
                                var filtroActual = $(this).attr('filtro');
                                var filtros = decodeURIComponent(ObtenerHash2());
                                var filtrosArray = filtros.split('&');
                                filtros = '';
                                var contieneFiltro = false;
                                for (var i = 0; i < filtrosArray.length; i++) {
                                    if (filtrosArray[i] != '') {
                                        if (filtrosArray[i] == filtroActual) {
                                            contieneFiltro = true;
                                        } else {
                                            filtros += filtrosArray[i] + '&';
                                        }

                                    }
                                }
                                if (!contieneFiltro) {
                                    filtros += filtroActual;
                                } else {
                                    location.reload();
                                }

                                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                                e.preventDefault();

                                location.reload();
                            });
                        hacerPeticion = false;
                    }
                    $('.indice-lista ul').css({ marginLeft: -30 });
                    $('.indice-lista ul').animate({
                        marginLeft: 20,
                        opacity: 1
                    }, 200, function () {
                        // Left Animation complete.                         
                    });
                });
            }
        });

        $('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-siguiente-facetas-modal .texto').click(function () {
            if (!that.buscando && !that.fin) {
                that.buscando = true;
                that.paginaActual++;
                var hacerPeticion = true;
                $('.indice-lista ul').animate({
                    marginLeft: -30,
                    opacity: 0
                }, 200, function () {
                    if (hacerPeticion) {
                        that.buscarFacetas();
                        $('.indice-lista .faceta')
                            .unbind()
                            .click(function (e) {
                                var filtroActual = $(this).attr('filtro');
                                var filtros = decodeURIComponent(ObtenerHash2());
                                var filtrosArray = filtros.split('&');
                                filtros = '';
                                var contieneFiltro = false;
                                for (var i = 0; i < filtrosArray.length; i++) {
                                    if (filtrosArray[i] != '') {
                                        if (filtrosArray[i] == filtroActual) {
                                            contieneFiltro = true;
                                        } else {
                                            filtros += filtrosArray[i] + '&';
                                        }

                                    }
                                }
                                if (!contieneFiltro) {
                                    filtros += filtroActual;
                                } else {
                                    location.reload();
                                }

                                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                                e.preventDefault();

                                location.reload();
                            });
                        hacerPeticion = false;
                    }
                    $('.indice-lista ul').css({ marginLeft: 30 });
                    $('.indice-lista ul').animate({
                        marginLeft: 20,
                        opacity: 1
                    }, 200, function () {
                        // Right Animation complete.                            
                    });
                });
            }
        });
        that.buscarFacetas();
        $('.indice-lista .faceta')
            .unbind()
            .click(function (e) {
                var filtroActual = $(this).attr('filtro');
                var filtros = decodeURIComponent(ObtenerHash2());
                var filtrosArray = filtros.split('&');
                filtros = '';
                var contieneFiltro = false;
                for (var i = 0; i < filtrosArray.length; i++) {
                    if (filtrosArray[i] != '') {
                        if (filtrosArray[i] == filtroActual) {
                            contieneFiltro = true;
                        } else {
                            filtros += filtrosArray[i] + '&';
                        }

                    }
                }
                if (!contieneFiltro) {
                    filtros += filtroActual;
                } else {
                    location.reload();
                }

                history.pushState('', 'New URL: ' + filtros, '?' + filtros + '~~~' + numPagina);
                e.preventDefault();

                location.reload();
            });
    });
};