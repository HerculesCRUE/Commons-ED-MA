String.prototype.width = function (font) {
    var f = font || '12px arial',
        o = $('<div></div>')
            .text(this)
            .css({ 'position': 'absolute', 'float': 'left', 'white-space': 'nowrap', 'visibility': 'hidden', 'font': f })
            .appendTo($('body')),
        w = o.width();
    o.remove();
    return w;
}
var paginas = [];
var grupos = {};
var callbacks = {};
var resizeGraficas = [];
$(window).resize(function() {
    clearTimeout(this.resizeTimer);
    this.resizeTimer = setTimeout(function() {
        for (var i = 0; i < resizeGraficas.length; i++) {
            resizeGraficas[i]();
        }
    }, 150);
});
class Pagina {
    constructor(title, id, pContenedor, data, userId, pFiltroFacetas = "") {
        this.title = title;
        this.id = id;
        this.contenedor = pContenedor;
        this.data = data;
        this.graficasConfig = data.listaConfigGraficas || data;
        this.userId = userId;
        this.filtroFacetas = pFiltroFacetas;
        this.grupos = {};

    }
    pintarPagina(opcionesDropdown = []) {
        this.createEmptyPage();
        this.agrupar();
        this.createGraphs(opcionesDropdown);
    }
    createEmptyPage() {
        $(this.contenedor).empty();
        $(this.contenedor).attr('id', 'page_' + this.id);
        $('#panFacetas').attr('idfaceta', 'page_' + this.id);
        $('#panFacetas').addClass('containerFacetas');

        $('main').find('.modal-backdrop').remove();
        $('main').append(`
        <div class="modal-backdrop fade" style="pointer-events: none;"></div>
        `);
    }
    /**
     * Agrupa las graficas de la pagina en grupos
     */
    agrupar() {
        this.grupos = {};
        this.graficasConfig.forEach(grafica => {
            if (grafica.idGrupo) {
                if (this.grupos[grafica.idGrupo] == undefined) {
                    this.grupos[grafica.idGrupo] = [grafica]
                } else {
                    this.grupos[grafica.idGrupo].push(grafica);
                }
            } else {
                if (!grafica.id) {
                    grafica['id'] = grafica.idRecurso;
                }
                var id = 'grafica_' + grafica.id;
                var count = 0;
                while (this.grupos[id] != undefined) {
                    id += '_' + count;
                    count++;
                }
                this.grupos[id] = [grafica];
            }
        });
    }
    createGraphs(opcionesDropdown = []) {
        Object.entries(this.grupos).forEach(([idGrupo, grupo]) => {
            var contenedor = this.createContenedorGrafica(grupo[0]);
            grupo.forEach(grafica => {
                this.createGrafica(contenedor, grafica, opcionesDropdown);
            });
        });
    }
    createContenedorGrafica(grafica) {
        var ancho = "span" + grafica.anchura;
        var contenedor = $(`<article class="resource ${ancho}"></article>`);
        contenedor.append(`<div class="wrap"></div>`);
        contenedor.addClass('contenedorGrafica');
        contenedor.find(".wrap").append(`<div id="${grafica.id}"class="grafica show"></div>`);
        this.contenedor.append(contenedor[0]);
        return contenedor.find('.grafica')[0];
    }
    async createGrafica(contenedor, grafica, opcionesDropdown = []) {
        if (!grafica.idRecurso) {
            var graph = await getGrafica2(this.id, grafica.id, this.filtroFacetas);
        } else {
            var graph = await getGrafica2(grafica.idPagina, grafica.idGrafica, grafica.filtro, 50, grafica.escalas, grafica.titulo)
            graph.data.groupId = null;
        }
        graph.pintarGrafica(contenedor);
        graph.addZoomButton(contenedor);
        graph.addDescargaJpg(contenedor);
        graph.addDescargaCsv(contenedor);
        graph.dropdown = opcionesDropdown;
        opcionesDropdown.forEach(opcion => {
            graph.addDropdownOption(contenedor, opcion.elemento, opcion.identificador, opcion.comportamiento);
        });
    }
}
class GraficaBase {
    constructor(idPagina, idGrafica, data, filtroFacetas = "", titulo = null, tipo = null, hideLegend = false) {
        this.idPagina = idPagina;
        this.idGrafica = idGrafica;
        this.filtroFacetas = filtroFacetas;
        this.titulo = titulo || data.options.plugins.title.text;
        this.data = data;
        this.tipo = tipo;
        this.hideLegend = hideLegend;
        this.callbacks = {};
        this.plugins = {};
        this.globalPlugins = [];
        this.dropdown = [];
        this.accionesMapa = {
        };
    }
    addPlugin(nombre, plugin) {
        if (this.plugins[nombre] == undefined) {
            this.plugins[nombre] = plugin;
        } else {
            Object.entries(plugin).forEach(([key, value]) => {
                this.plugins[nombre][key] = value;
            });
        }
    }
    addCallback(nombre, callback) {
        this.callbacks[nombre] = callback;
    }
    callbacks(ctx) {
        if ($(ctx).hasClass("zoom")) {
            this.accionesMapa.zoom = () => { };
        }
        if (!$(ctx).hasClass("collapsed")) {
            this.accionesMapaCallBack(ctx);
        }

    }
    pluginsCallback() {
        if (this.data.plugins) {
            this.data.plugins.push(this.globalPlugins);
        } else {
            this.data.plugins = this.globalPlugins;
        }

        Object.entries(this.plugins).forEach(([nombre, plugin]) => {
            if (this.data.options) {
                this.data.options.plugins[nombre] = plugin;
            }
        });
    }
    accionesMapaCallBack(ctx) {
        if ($(ctx).context.className != "expand") {
            Object.entries(this.accionesMapa).forEach(([nombre, callback]) => {
                callback(ctx);
            });
        }
    }
    pintarGrafica(canvas, cambiarFont = true) {
        let fontSize = 16;
        if (this instanceof GraficaCircular) {
            canvas = canvas.canvas;
        }
        while (fontSize > 5 && this.titulo.width("bold " + fontSize + "px Helvetica") > $(canvas).parents(".grafica").width() - 200) {
            fontSize--;
        }
        this.data.options.plugins.title.font = {
            size: fontSize,
            style: 'bold'
        }

        this.addPlugin("legend", {
            onHover: (e) => {
                e.chart.canvas.style.cursor = 'pointer';
            },
            onLeave: (e) => {
                e.chart.canvas.style.cursor = 'default';
            }
        });
        if (this.hideLegend) {
            this.addPlugin("legend", { display: false });
        }
        this.globalPlugins.push({
            id: 'custom_canvas_background_color',
            beforeDraw: (chart) => {
                chart.ctx.save();
                chart.ctx.globalCompositeOperation = 'destination-over';
                chart.ctx.fillStyle = '#FFFFFF';
                chart.ctx.fillRect(0, 0, chart.width, chart.height);
                chart.ctx.restore();
            }
        });
        if ($(canvas).parents(".zoom").length == 0) {
            this.pluginsCallback();
        }

        // Título de la gráfica
        if (this.titulo) {
            this.data.options.plugins.title.text = this.titulo;
        }
        return new Chart(canvas, this.data);
    }
    addAccionMapa(key, accion) {
        this.accionesMapa[key] = accion;
    }
    addZoomButton(pContenedor) {
        var accionesMapa = pintarAccionesMapa(pContenedor);
        if (accionesMapa.find("div.zoom").length != 0) {
            return;
        }
        var zoom = $(`<div class="zoom"></div>`);
        accionesMapa.find('.wrap').append(zoom)
        zoom.append(`
                <a href="javascript:void(0);" data-toggle="modal">
                    <span class="material-icons">zoom_in</span>
                </a>
            `);

        zoom.unbind()
            .click(function (e) {
                if ($("#modal-ampliar-mapa").length == 0) {
                    $(".modal").first().parent().append(`
                        <div id="modal-ampliar-mapa" class="modal modal-top fade modal-ampliar-mapa" style="pointer-events:none" tabindex="-1" role="dialog">
                            <div class="modal-dialog" style="margin:50px" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <p class="modal-title"></p>
                                        <span class="material-icons cerrar cerrar-grafica" aria-label="Close">close</span>
                                    </div>
                                        <div class="modal-body">
                                        <div class="graph-container grafica show zoom" style="width:100%;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    `)

                } else {
                    $("#modal-ampliar-mapa").find('.graph-container').addClass('grafica zoom');
                }

                if ($(".modal-backdrop").length == 0) {
                    $(".modal").first().parent().append(`
                        <div class="modal-backdrop fade" style="pointer-events:none" tabindex="-1" role="dialog"></div>
                    `)
                }

                $('.modal-backdrop')
                    .unbind()
                    .click(cerrarModal);

                $('span.cerrar-grafica')
                    .unbind()
                    .click(cerrarModal);

                // Obtiene la gráfica seleccionada (en caso de menu) o la grafica del contenedor en casos normales.
                var grafica = $(this).parents(".acciones-mapa").parent().find(".grafica.show");
                $("#modal-ampliar-mapa").data("ctx", grafica);
                var data = grafica.data('grafica');
                var parent = $('#modal-ampliar-mapa').find('.graph-container');
                parent.removeClass('small horizontal vertical'); // se le quitan los estilos que podria tener
                var modalContent = $('#modal-ampliar-mapa').find('.modal-content');
                // Tamaño del contenedor (dejando 50px de margen arriba y abajo).
                modalContent.css({ height: 'calc(100vh - 100px)' });
                modalContent.parent().css({ maxWidth: '1310px' }); // El tamaño maximo del contendor de los articles.
                // Se revela el popup.
                $('#modal-ampliar-mapa').css('display', 'block');
                $('#modal-ampliar-mapa').css('pointer-events', 'none');
                $('.modal-backdrop').addClass('show');
                $('.modal-backdrop').css('pointer-events', 'auto');
                $('#modal-ampliar-mapa').addClass('show');
                $('#modal-ampliar-mapa').find('div.acciones-mapa').append(`
                <div class="wrap">
                </div>`);

                data.pintarGrafica(parent);
                let botonJPG = grafica.parents('div.wrap').find('.acciones-mapa .descargaJPG');
                data.addDropdownOption(parent, `<li><a class="item-dropdown descargaJPGzoom"><span class="material-icons">photo_camera</span><span class="texto-item-dropdown">Descargar JPG</span></a></li>`, 'descargaJPGzoom', function (e) {
                    botonJPG.click();
                });
                let botonCSV = grafica.parents('div.wrap').find('.acciones-mapa .descargaCSV');
                data.addDropdownOption(parent, `<li><a class="item-dropdown descargaCSVzoom"><span class="material-icons">file_copy</span><span class="texto-item-dropdown">Descargar CSV</span></a></li>`, 'descargaCSVzoom', function (e) {
                    botonCSV.click();
                });
                data.dropdown.forEach(opcion => {
                    let botonOpcion = grafica.parents('div.wrap').find('.acciones-mapa .' + opcion.identificador);
                    data.addDropdownOption(parent, opcion.elementozoom, opcion.identificador + 'zoom', function (e) {
                        cerrarModal();
                        botonOpcion.click();
                    });
                });
            });
    }
    /**
     * Añade un dropdown al contenedor de la gráfica.
     * @param {*} pContenedor es el contenedor de la gráfica.
     * @param {string} pTituloDropdown por defecto es 'Acciones'
     */
    addDropdownButton(pContenedor, pTituloDropdown = 'Acciones') {
        var accionesMapa = pintarAccionesMapa(pContenedor);
        if (accionesMapa.find("div.dropdown").length != 0) {
            return accionesMapa.find("div.dropdown");
        }
        var dropdown = $(`<div class="dropdown"></div>`);
        accionesMapa.find('.wrap').append(dropdown)
        dropdown.append(`
                <a href="javascript:void(0);" data-toggle="dropdown">
                    <span class="material-icons">more_vert</span>
                </a>
                <div class="dropdown-menu basic-dropdown dropdown-icons dropdown-menu-right">
                    <p class="dropdown-title">${pTituloDropdown}</p>
                    <ul class="no-list-style dropdownGrafica"></ul>
                </div>
            `);
        return dropdown;
    }
    /**
     * Añade una opción al dropdown de la gráfica.
     * @param {*} pContenedor es el contenedor de la gráfica.
     * @param {*} pOpcionDropdown es el &lt;li&gt;&lt;/li&gt; que se añadirá al dropdown
     */
    addDropdownOption(pContenedor, pOpcionDropdown, pIdentificador = "", pOnClick = null) {
        var listaDropdown = this.addDropdownButton(pContenedor)

        if ((pIdentificador != "" && $(listaDropdown).find('.' + pIdentificador).length != 0) || $(listaDropdown).find('ul').html().includes(pOpcionDropdown)) {
            return;
        }
        $(listaDropdown).find('ul').append(pOpcionDropdown);
        if (pOnClick != null) {
            $('.' + pIdentificador).unbind().click(pOnClick);
        }
    }
    addDescargaJpg(pContenedor, click = null) {
        var itemLista = $(`<li><a class="item-dropdown descargaJPG"><span class="material-icons">photo_camera</span><span class="texto-item-dropdown">Descargar JPG</span></a></li>`);
        this.addDropdownOption(pContenedor, itemLista, "descargaJPG");
        if (!click) {
            click = function (e) {// Obtención del chart usando el elemento canvas de graficas con scroll.
                var canvas = $(this).parents('div.acciones-mapa').parent().find('div.grafica.show');
                var grafica = canvas.data('grafica');
                if (grafica.tipo == "nodos") {
                    var cy = grafica.cy;
                    var image = cy.jpg(
                        {
                            full: true,
                            quality: 1,
                        }
                    );
                    var a = document.createElement('a');
                    a.href = image;

                    a.download = grafica.titulo + '.jpg';
                    a.click();

                } else {
                    var chart = Chart.getChart(canvas);
                    // Obtención del chart usando el elemento canvas de graficas sin scroll y de Chart.js
                    if (chart == null) {
                        canvas = $(this).parents('div.acciones-mapa').parent().find("div.grafica.show canvas");
                        chart = Chart.getChart(canvas);
                    }
                    var image = chart.toBase64Image('image/jpeg', 1);
                    // Creación del elemento para empezar la descarga.
                    var a = document.createElement('a');
                    a.href = image;
                    a.download = chart.config._config.options.plugins.title.text + '.jpg';
                    a.click();
                }
            }

        }
        itemLista.unbind().click(click);

    }
    addDescargaCsv(pContenedor) {
        var itemLista = $(`<li><a class="item-dropdown descargaCSV"><span class="material-icons">file_copy</span><span class="texto-item-dropdown">Descargar CSV</span></a></li>`);
        this.addDropdownOption(pContenedor, itemLista, "descargaCSV");
        itemLista.unbind().click(function (e) {
            var url = url_servicio_graphicengine + "GetCSVGrafica";
            var canvas = $(this).parents('div.acciones-mapa').parent().find('div.grafica.show');
            var grafica = canvas.data('grafica');
            if (grafica.filtroFacetas == null) {
                grafica.filtroFacetas = "";
            }
            url += "?pIdPagina=" + grafica.idPagina;
            url += "&pIdGrafica=" + grafica.idGrafica;
            url += "&pFiltroFacetas=" + encodeURIComponent(grafica.filtroFacetas);
            url += "&pLang=" + lang;
            var urlAux = url_servicio_graphicengine + "GetGrafica"; //"https://localhost:44352/GetGrafica"
            var argAux = {};
            argAux.pIdPagina = grafica.idPagina;
            argAux.pIdGrafica = grafica.idGrafica;
            argAux.pFiltroFacetas = decodeURIComponent(grafica.filtroFacetas);
            argAux.pLang = lang;
            $.get(urlAux, argAux, function (listaData) {
                if (!listaData.options) {
                    url += "&pTitulo=" + listaData.title;
                } else {
                    url += "&pTitulo=" + listaData.options.plugins.title.text;
                }
                document.location.href = url;
            });
        });
    }
    addToGroup(pContenedor) {
        if (this.data.groupId == null || $(pContenedor).hasClass("zoom") || $($(pContenedor).context).hasClass("expand")) {
            return pContenedor;
        }
        var numChildren = $(pContenedor).children().length;
        var accionesMapa = pintarAccionesMapa(pContenedor);

        if (!accionesMapa.hasClass("showAcciones")) {
            accionesMapa.addClass("showAcciones");
            accionesMapa.prepend(`
            <div class="toggleGraficas">
                <a href="javascript: void(0);" id="dropdownMasOpciones" data-toggle="dropdown">
                    <span class="material-icons">sync_alt</span>
                </a>
                <div class="dropdown-menu basic-dropdown dropdown-icons dropdown-menu-left" aria-labelledby="dropdownMasOpciones">
                    <p class="dropdown-title">Gráficas</p>
                    <ul class="no-list-style">
                    </ul>
                </div>
            </div>
            `);
        }
        var tipo = this.tipo;
        if (this.orientacion) {
            tipo = this.orientacion;
        }
        var first = true;
        if ($(pContenedor).attr("id") != this.idGrafica) {
            first = "";
            this.accionesMapaCallBack = function () { };
            var parent = $(pContenedor).parent();
            pContenedor = $(`<div order="${numChildren}" class="grafica ${tipo} hide " style="top:-9999px;left:-9999px;z-index:-1"></div>`)[0];
            parent.append(pContenedor);
        } else {
            $(pContenedor).removeAttr("id");
        }
        var opcionlista = $(`<a order="${numChildren}" value="grafica_${this.idPagina + "_" + this.idGrafica}" class="item-dropdown ${first ? "active" : ""}"></a>`)
        if (first) {
            accionesMapa.find('.toggleGraficas ul.no-list-style').prepend(opcionlista);
        } else {
            accionesMapa.find('.toggleGraficas ul.no-list-style').append(opcionlista);
        }
        opcionlista.append(`
        <span class="material-icons">${getIcon(this.tipo)}</span>
        <span class="texto">${this.titulo}</span>`);
        addComportamiento(opcionlista);
        return pContenedor;
        function getIcon(tipoGrafica) {
            switch (tipoGrafica) {
                case "nodos":
                    return "bubble_chart";
                case "circular":
                    return "pie_chart";
                case "horizontal":
                case "barras":
                    return "bar_chart";
                case "vertical":
                    return "align_horizontal_left";
            }
        }
        function addComportamiento(listItem) {
            listItem.unbind().click(function () {
                var list = $(this).parent();
                var active = list.find(".active");
                var activeGraph = $(list).parents(".acciones-mapa").parent().find(".grafica.show");
                var expand = $(list).parents(".acciones-mapa").find(".expand");
                active.removeClass("active");
                activeGraph.removeClass("show");
                activeGraph.addClass("hide");
                activeGraph.css('top', '-9999px');
                activeGraph.css('left', '-9999px');
                activeGraph.css('z-index', '-1');

                $(this).addClass("active");
                var idGrafica = $(this).attr("value");
                var grafica = $(list).parents(".acciones-mapa").parent().find("#" + idGrafica).parents("div.grafica");
                if (expand.length > 0) {
                    var scroll = grafica.find(".chartScroll");
                    if (scroll.hasClass("collapsed")) {
                        expand.addClass("collapsed");
                        expand.find("span").text("open_in_full");
                    } else {
                        expand.removeClass("collapsed");
                        expand.find("span").text("close_fullscreen");
                    }
                }
                var graph = grafica.data("grafica");
                if (graph instanceof GraficaNodos) {
                    grafica.find(".graficoNodos").empty();
                    graph.comportamientoNodos(grafica.find(".graficoNodos"));
                }
                grafica.removeClass("hide");
                grafica.addClass("show");
                grafica.attr('style', '');
            }
            );
        }
    }
}
class GraficaNodos extends GraficaBase {
    constructor(idPagina, idGrafica, data, filtroFacetas = "", titulo = null) {
        super(idPagina, idGrafica, data, filtroFacetas, "-", "nodos");
        this.titulo = titulo || data.title;
        this.cy = null;
    }
    pintarContenedores(pContenedor) {
        var ctx = $(`<div class="graficoNodos" id="grafica_${this.idPagina}_${this.idGrafica}"></div>`);

        $(pContenedor).append(`
            <p id="titulo_grafica_${this.idPagina}_${this.idGrafica}" style="z-index:4; text-align:center; margin-top: 0.60em; width: 100%; color: #666666; font-weight:bold; ">${this.titulo}</p>
            <div class="graph-controls">
                <ul class="no-list-style align-items-center">
                    <li class="control zoomin-control" id="zoomIn">
                        <span class="material-icons" >add</span>
                    </li>
                    <li class="control zoomout-control" style="margin-top:5px" id="zoomOut">
                        <span class="material-icons" >remove</span>
                    </li>
                </ul>
            </div> 
        `);
        $(pContenedor).append(ctx);
        return ctx[0];

    }
    comportamientoNodos(ctx) {
        var controls = $(ctx).parent().find(".graph-controls");
        this.data.container = ctx;
        this.cy = cytoscape(this.data);
        $(ctx).data("cy", this.cy);
        var arrayNodes = [];
        var nodos = this.cy.nodes();
        for (i = 0; i < this.cy.nodes().length; i++) {
            arrayNodes.push(nodos[i]._private.data.name);
        };

        var arrayEdges = [];
        var edges = this.cy.edges();
        for (i = 0; i < this.cy.edges().length; i++) {
            var data = edges[i]._private.data.id.split('~');
            arrayEdges.push(data[data.length - 1]);
            edges[i]._private.data.name = "";
        };
        var nodeGraph = this;
        this.cy.on('click', 'node', function (e) {
            e = e.target;
            var indice = nodeGraph.cy.nodes().indexOf(e);
            if (e._private.data.name === "") {
                e._private.data.name = arrayNodes[indice];
            } else {
                e._private.data.name = "";
            }
        })

        this.cy.on('click', 'edge', function (e) {
            e = e.target;
            var indice = nodeGraph.cy.edges().indexOf(e);
            if (e._private.data.name === "") {
                e._private.data.name = arrayEdges[indice];
            } else {
                e._private.data.name = "";
            }
        });
        $(controls.find("#zoomOut"))
            .unbind()
            .click(function (e) {
                var grafica = $(e.target).parents(".grafica").data("grafica");
                grafica.cy.zoom({
                    level: grafica.cy.zoom() / 1.2,
                    renderedPosition: { x: grafica.cy.width() / 2, y: grafica.cy.height() / 2 }
                });
            });
        $(controls.find("#zoomIn"))
            .unbind()
            .click(function (e) {
                var grafica = $(e.target).parents(".grafica").data("grafica");
                grafica.cy.zoom({
                    level: grafica.cy.zoom() * 1.2,
                    renderedPosition: { x: grafica.cy.width() / 2, y: grafica.cy.height() / 2 }
                });
            })

    }
    pintarGrafica(ctx) {
        ctx = super.addToGroup(ctx);
        super.callbacks(ctx);
        $(ctx).data("grafica", this);
        ctx = this.pintarContenedores(ctx);
        this.comportamientoNodos(ctx);

    }
}
class GraficaBarras extends GraficaBase {
    constructor(idPagina, idGrafica, data, filtroFacetas = "", titulo = null, escalas = null, barSize = 50, orientacion = "vertical", hideLegend = false) {
        super(idPagina, idGrafica, data, filtroFacetas, titulo, "barras", hideLegend);
        this.escalas = escalas;
        this.barSize = barSize;
        this.orientacion = orientacion;

    }
    pintarGrafica(ctx) {

        return super.pintarGrafica(ctx);
    }
    callbacks(ctx) {
        super.callbacks(ctx);
    }
    pintarContenedores(pContenedor) {

        pContenedor = super.addToGroup(pContenedor);

        $(pContenedor).data("grafica", this);
        var ctx = $(`<canvas id = "grafica_${this.idPagina}_${this.idGrafica}"></canvas>`)
        $(pContenedor).append(`
        <div class="chartWrapper" >
            <div class="chartScroll custom-css-scroll custom-css-scroll-proy">
                <div class="chartAreaWrapper">
                </div>
            </div>
        </div>
        `);
        $(pContenedor).find(".chartAreaWrapper").append(ctx);

        return ctx[0];

    }
    getCanvasSize() {
        // ESCALAMOS EL TAMAÑO DE LAS BARRAS

        var numBars = this.data.data.labels.length; // Número de barras.
        var canvasSize = (numBars * this.barSize) * 1.5; // Tamaño del canvas, el 1.5 representa el espacio entre las barras

        var barCount = 0;
        var stacks = []
        //Obtenemos el numero de barras que tendra la grafica por cada dataset
        this.data.data.datasets.forEach(function (dataset) {
            if (dataset.type == "bar") {
                if (dataset.stack != null) {
                    if (stacks.indexOf(dataset.stack) == -1) {
                        stacks.push(dataset.stack);
                        barCount++;
                    }
                } else {
                    barCount++;
                }
            }
        })
        if (barCount == 0) {
            barCount = 1;
        }
        var ancho = this.barSize / barCount;
        this.data.options.maintainAspectRatio = false;
        this.data.options.responsive = true;
        //esto modifica el tamaño de las barras 
        this.data.data.datasets.forEach((item) => {
            item['barThickness'] = ancho;
        })

        this.data.options.scale = {
            ticks: {
                precision: 0
            }
        }
        return canvasSize;

    }
    drawSmallChart(ctx) {

        var chartAreaWrapper = $(ctx).parents(".chartAreaWrapper");
        var scrollContainer = $(ctx).parents(".chartScroll");
        //$(ctx).parents(".grafica").addClass("small");

        chartAreaWrapper.css("width", "100%");
        chartAreaWrapper.css("height", "100%");
        scrollContainer.css("width", "100%");
        scrollContainer.css("height", "100%");


        scrollContainer[0].style.overflow = "hidden";


        super.pintarGrafica(ctx);

    }
    drawLegend(ctx) {
        //Se revela el modal de zoom
        if (this.hideLegend){
            return null
        }
        // Se comprueba si tiene eje principal/secundario.
        var scrollContainer = $(ctx).parents(".chartScroll")[0];
        var chartAreaWrapper = $(ctx).parents(".chartAreaWrapper")[0];
        // Leyenda con titulo y contenedor para datasets. 
        var legend = $(`<div class="chartLegend">
                <h4 id="legendTitle">${this.titulo}</h4>
                </div>`);
        $(ctx).parents(".chartWrapper").append(legend);
        var dataSetLabels = $(`<div class="dataSetLabels"></div>`)
        $(legend).append(dataSetLabels);

        // Por cada dataset que exista se creara un div con su nombre y color y se añade a dataSetLabels.
        var datasets = this.data.data.datasets;
        datasets.forEach((dataset, index) => {
            var labelContainer = $(`<div id="label-${index}" class="labelContainer" >
                    <div style="background-color: ${dataset.backgroundColor[0]};"></div>
                    <p class="dataSetLabel">${dataset.label}</p>
                    </div>`);

            labelContainer.unbind().click(function (e) {
                // Se obtiene el chart desde el canvas.
                var canvas = $(this).parents('div.chartWrapper').find('div.chartAreaWrapper canvas');
                var chart = Chart.getChart(canvas);
                // El ID del dataset está en el ID del contenedor del label.
                var id = $(this).attr('id').split('-')[1];
                var label = $(this).find('p.dataSetLabel');

                // Si el label no está tachado se tacha y oculta el dataset.
                if (label.css('text-decoration').indexOf("line-through") == -1) {
                    label.css("text-decoration", "line-through");
                    chart.setDatasetVisibility(id, false);
                } else {
                    label.css("text-decoration", "none");
                    chart.setDatasetVisibility(id, true);
                }

                try { // Hay problemas con el gráfico de líneas + grafico de barras stackeado, si falla se repinta el chart.
                    chart.update();
                } catch (e) {
                    chart.draw();
                }
            });
            $(dataSetLabels).prepend(labelContainer);
        });



        if (this.data.isDate) { //las graficas de fechas se mueven hasta el año mas reciente que se encuentra al final del scroll
            $(scrollContainer).animate({ scrollLeft: $(chartAreaWrapper).width() - $(scrollContainer).width() }, 6000);
            // Se detiene la animacion al hacer click en el scroll o al pulsar la rueda del raton.
            $(scrollContainer).mousedown((e) => {
                if (e.button == 1 || scrollContainer.clientHeight <= e.offsetY) {
                    $(scrollContainer).stop();
                }
            });
        }

        return legend;

    }
    reSizeLegend(myChart, mainAxis, secondaryAxis, legend) {

        $(legend).css("height", myChart.chartArea.top + "px");

        //si la leyenda falsa es mayor a la del canvas se añade la diferencia en margen para compensar
        //esto sucede cuando en el canvas la leyenda ocupa una fila pero en el div 2 o mas;
        if ($(legend).height() > myChart.chartArea.top) {
            //añadimos el margen
            myChart.canvas.style.marginTop = $(legend).height() - myChart.chartArea.top + 9 + "px";
            if (mainAxis) {
                mainAxis[0].style.marginTop = $(legend).height() - myChart.chartArea.top + 9 + "px";
            }
            if (secondaryAxis) {
                secondaryAxis[0].style.marginTop = $(legend).height() - myChart.chartArea.top + 9 + "px";
            }

        }
    }
    abrebiarGrafica(value) {

        const labels = this.chart.config.data.labels; // Obtención de los labels.
        if (value >= 0 && value < labels.length) {
            if (labels[value].length >= 40) {
                return labels[value].substring(0, 40) + "..."; // Se muestran solo los 40 primeros caractéres para que no se salga de la barra.
            }
            return labels[value];
        }
        return value;

    }



}
class GraficaHorizontal extends GraficaBarras {
    constructor(idPagina, idGrafica, data, filtroFacetas = "", titulo = null, escalas = null, barSize = 50, isAbr = false, hideLegend = false) {
        super(idPagina, idGrafica, data, filtroFacetas, titulo, escalas, barSize, "horizontal", hideLegend);
        this.isAbr = isAbr;
    }
    pintarGrafica(ctx) {
        $(ctx).addClass("horizontal");
        var canvas = super.pintarContenedores(ctx);
        this.drawChart(canvas);
        super.callbacks(ctx);
    }

    drawChart(ctx) {
        if (Chart.getChart(ctx) != null) {// en caso de que se ejecute esto despues de cambiar de pagina desde la anterior
            return;
        }
        var canvasSize = super.getCanvasSize();
        ctx.parentNode.style.height = canvasSize + 'px'; //se establece la altura del eje falso
        if (this.isAbr) {
            this.abreviar();
        }

        if (this.escalas) {
            //console.log(this.escalas);
            if (this.escalas.indexOf(",") > -1) {
                this.data.options.scales.x1['max'] = parseInt(this.escalas.split(",")[1]);
                this.data.options.scales.x2['max'] = parseInt(this.escalas.split(",")[0]);
            } else {
                this.data.options.scales.x1['max'] = parseInt(this.escalas);
            }
        }
        // Título de la gráfica
        if (this.titulo) {
            this.data.options.plugins.title.text = this.titulo;
        }
        //si la grafica es horizontal y su altura es menor a (310 si no esta en zoom, tamaño de ventana - 270 si esta en zoom ) o si es vertical y su ancho es menor a (su contenedor si no tiene zoom, 1110 si tiene zoom) no necesita scroll 
        if ((canvasSize < 318)) {
            super.drawSmallChart(ctx);
        } else { // a partir de aqui se prepara el scroll
            this.drawScrollChart(ctx);
        }
    }

    drawScrollChart(ctx) {
        var legend = super.drawLegend(ctx);
        var hasMainAxis = false; //eje superior en caso horizontal, izquierdo en vertical
        var hasSecondaryAxis = false; // eje inferior o derecho

        var myChart = super.pintarGrafica(ctx);
        Object.entries(this.data.options.scales).forEach((scale) => { // por cada escala que tenga data
            if ((scale[1].axis == "x")) { //se comprueba si tiene eje principal (top en caso de horizontal, left en caso de vertical)
                if (scale[1].position == "top") {
                    hasMainAxis = true;
                } else if (scale[1].position == "bottom") {//se comprueba si tiene eje secundario (bottom en caso de horizontal, right en caso de vertical)
                    hasSecondaryAxis = true;
                }
            }
        });

        //Se añade el eje principal al contenedor.
        if (hasMainAxis) {

            var mainAxis = $(`<canvas id="topAxis" class="myChartAxis"></canvas>`);
            $(legend).append(mainAxis);

        }
        //Se añade el eje secundario al contenedor.
        if (hasSecondaryAxis) {
            var secondaryAxis = $(`<canvas id="bottomAxis" class="myChartAxis"></canvas>`);
            $(ctx).parents(".chartWrapper").append(secondaryAxis);
        }

        // mover
        this.data.options.animation.onProgress = () => this.reDrawChart(myChart, mainAxis, secondaryAxis);

        // Resize
        let that = this;
        resizeGraficas.push(() => that.reDrawChart(myChart, mainAxis, secondaryAxis));
    }
    reDrawChart(myChart, mainAxis, secondaryAxis) {
        var legend = $(myChart.canvas).parents(".chartWrapper").find(".chartLegend")[0];

        // Se obtiene la escala del navegador (afecta cuando el usuario hace zoom).
        var scale = window.devicePixelRatio;
        // Preparamos el eje superior.
        super.reSizeLegend(myChart, mainAxis, secondaryAxis, legend);
        $(legend).css("width", $(myChart.canvas).parents(".chartAreaWrapper").width() + "px");
        if (secondaryAxis) {
            var start = myChart.chartArea.bottom;
            var end = start + myChart.height - myChart.chartArea.bottom;
            drawAxis(secondaryAxis[0].getContext("2d"), myChart, start, end);
        }
        if (mainAxis) {
            var end = myChart.chartArea.top;
            var start = end - 25;
            drawAxis(mainAxis[0].getContext("2d"), myChart, start, end);
        }

        function drawAxis(ctx, myChart, start, end) {
            ctx.canvas.width = myChart.width;// * scale;
            ctx.canvas.height = end - start;
            try {
                ctx.drawImage(myChart.canvas, 0, start * scale, myChart.width * scale, (end - start) * scale, 0, 0, myChart.width, end - start);
            } catch (e) {
                console.log(e);
            }
        }


    }
    abreviar() {
        // Se modifica la propiedad que usa Chart.js para obtener los labels de la gráfica.

        this.data.options.scales['y'] = {
            ticks: {
                callback: super.abrebiarGrafica,
                mirror: true,
                padding: -5,
                font: {
                    weight: "bold"
                },
                z: 1000
            }

        }
    }


}
class GraficaVertical extends GraficaBarras {
    constructor(idPagina, idGrafica, data, filtroFacetas = "", titulo = null, escalas = null, barSize = 50, hideLegend = false) {
        super(idPagina, idGrafica, data, filtroFacetas, titulo, escalas, barSize, "vertical", hideLegend);
        this.escalas = escalas;

    }
    pintarGrafica(ctx) {
        $(ctx).addClass("vertical");
        var canvas = super.pintarContenedores(ctx);
        this.drawChart(canvas); // meter metodo aqui
        super.callbacks(ctx);
    }

    drawChart(ctx) {
        if (Chart.getChart(ctx) != null) {// en caso de que se ejecute esto despues de cambiar de pagina desde la anterior
            return;
        }

        var canvasSize = super.getCanvasSize();
        ctx.parentNode.style.width = canvasSize + 'px'; //se establece la altura del eje falso
        if (this.escalas) {
            //console.log(this.escalas);
            //if contains ","
            if (this.escalas.indexOf(",") > -1) {

                this.data.options.scales.y1['max'] = parseInt(this.escalas.split(",")[1]);
                this.data.options.scales.y2['max'] = parseInt(this.escalas.split(",")[0]);
            } else {
                this.data.options.scales.y1['max'] = parseInt(this.escalas);
            }


        }
        // Título de la gráfica
        if (this.titulo) {
            this.data.options.plugins.title.text = this.titulo;
        }
        //si la grafica es horizontal y su altura es menor a (310 si no esta en zoom, tamaño de ventana - 270 si esta en zoom ) o si es vertical y su ancho es menor a (su contenedor si no tiene zoom, 1110 si tiene zoom) no necesita scroll 
        if ((canvasSize < $(ctx).parents(".chartWrapper").width())) {
            super.drawSmallChart(ctx);
        } else { // a partir de aqui se prepara el scroll
            this.drawScrollChart(ctx);
        }

    }
    drawScrollChart(ctx) {
        var legend = super.drawLegend(ctx);
        var hasMainAxis = false; //eje superior en caso horizontal, izquierdo en vertical
        var hasSecondaryAxis = false; // eje inferior o derecho
        super.addAccionMapa("expand", (ctx) => { this.addExpandButton(ctx) });
        var myChart = super.pintarGrafica(ctx);

        Object.entries(this.data.options.scales).forEach((scale) => {

            // por cada escala que tenga data
            if ((scale[1].axis == "y")) { //se comprueba si tiene eje principal (top en caso de horizontal, left en caso de vertical)
                if (scale[1].position == "left") {
                    hasMainAxis = true;
                } else if (scale[1].position == "right") {//se comprueba si tiene eje secundario (bottom en caso de horizontal, right en caso de vertical)
                    hasSecondaryAxis = true;
                }
            }
        });

        //Se añade el eje principal al contenedor.
        if (hasMainAxis) {

            var mainAxis = $(`<canvas id="leftAxis" class="myChartAxis"></canvas>`);
            $(legend).append(mainAxis);

        }
        //Se añade el eje secundario al contenedor.
        if (hasSecondaryAxis) {

            var secondaryAxis = $(`<canvas id="rightAxis" class="myChartAxis"></canvas>`);
            $(legend).append(secondaryAxis);
        }

        // Mover
        this.data.options.animation.onProgress = () => this.reDrawChart(myChart, mainAxis, secondaryAxis, legend);

        // Resize
        let that = this;
        resizeGraficas.push(() => that.reDrawChart(myChart, mainAxis, secondaryAxis, legend));
    }
    reDrawChart(myChart, mainAxis, secondaryAxis, legend) {

        // Se obtiene la escala del navegador (afecta cuando el usuario hace zoom).
        var scale = window.devicePixelRatio;
        // Preparamos el eje superior.
        super.reSizeLegend(myChart, mainAxis, secondaryAxis, legend);
        $(legend).css("width", $(myChart.canvas).parents(".chartWrapper").width() + "px");
        if (secondaryAxis) {
            var start = myChart.chartArea.right;
            var end = myChart.width;
            drawAxis(secondaryAxis[0].getContext("2d"), myChart, start, end);
        }
        if (mainAxis) {
            var end = myChart.chartArea.left;
            var start = 0;
            drawAxis(mainAxis[0].getContext("2d"), myChart, start, end);
        }

        function drawAxis(ctx, myChart, start, end) {
            ctx.canvas.width = end - start;
            ctx.canvas.height = myChart.height;
            try {
                ctx.drawImage(myChart.canvas, start * scale, 0, (end - start) * scale, myChart.height * scale, 0, 0, end - start, myChart.height);
            } catch (e) {
                console.log(e);
            }
        }

    }

    addExpandButton(pContenedor) {
        var accionesMapa = pintarAccionesMapa(pContenedor);
        if (accionesMapa.find(".expand").length != 0) {
            return;
        }
        var expand = $(`<div class="expand"></div>`);
        accionesMapa.find('.wrap').prepend(expand)
        expand.prepend(`
                <a href="javascript:void(0);" data-toggle="modal">
                    <span class="material-icons">close_fullscreen</span>
                </a>
            `);
        expand.unbind()
            .click({}, function (e) {
                var parent = $(this).parents('.acciones-mapa').parent().find(".grafica:not(.hide)");
                var grafica = parent.data("grafica");

                var canvas = parent.find(".chartAreaWrapper canvas");
                var myChart = Chart.getChart(canvas[0]);

                //plugin para que el color de fondo sea blanco.
                var plugin = {
                    id: 'custom_canvas_background_color',
                    beforeDraw: (chart) => {
                        chart.ctx.save();
                        chart.ctx.globalCompositeOperation = 'destination-over';
                        chart.ctx.fillStyle = '#FFFFFF';
                        chart.ctx.fillRect(0, 0, chart.width, chart.height);
                        chart.ctx.restore();
                    }
                };
                var isExpanded = !$(this).hasClass('collapsed');
                $(this).toggleClass('collapsed');
                if (isExpanded) {
                    $(this).find("span").text("open_in_full");

                    grafica.data.data.datasets.forEach(function (dataset) {
                        delete dataset['barThickness'];
                    });

                    //Destruyo el chart para que se redibuje con el nuevo tamaño
                    myChart.destroy();
                    canvas.parents(".chartScroll").addClass('collapsed');

                    canvas.parent().css('width', 'auto');
                    var chartWrapper = canvas.parents(".chartWrapper");
                    //Elimino la leyenda y los ejes
                    chartWrapper.find(".myChartAxis, .chartLegend").remove();
                    //Elimino el callback que llama a reDrawChart
                    delete myChart.config.options.animation['onProgress'];

                    var newChart = new Chart(canvas[0].getContext('2d'), {
                        type: 'bar',
                        data: grafica.data.data,
                        options: myChart.config.options,
                        plugins: [plugin]
                    });

                } else {
                    // cambio el icono
                    $(this).find("span").text("close_fullscreen");
                    canvas.parents(".chartScroll").removeClass('collapsed');

                    myChart.destroy();
                    parent.empty();

                    grafica.pintarGrafica(parent);


                }
            });
    }
}
class GraficaCircular extends GraficaBase {
    constructor(idPagina, idGrafica, data, filtroFacetas = "", titulo = null, hideLegend = false) {
        super(idPagina, idGrafica, data, filtroFacetas, titulo, "circular", hideLegend);
    }
    pintarGrafica(ctx) {
        this.data.options.responsive = true;
        this.data.options.maintainAspectRatio = false;
        $(ctx).css("height", "90%");
        $(ctx).data("grafica", this);
        var size = $(ctx).width();
        var canvas = $(`<canvas id = "grafica_${this.idPagina}_${this.idGrafica}" width = "${size}" height = "250" ></canvas>`)
        $(ctx).append(canvas);


        if (this.data.isPercentage) {
            this.data.options.plugins.tooltip = {
                callbacks: {
                    label: function (tooltipItem) {
                        var dataset = tooltipItem.dataset;
                        var total = dataset.data.reduce(function (previousValue, currentValue, currentIndex, array) {
                            return previousValue + currentValue;
                        });
                        var currentValue = tooltipItem.parsed;
                        var percentage = Math.floor(((currentValue / total) * 100) + 0.5);
                        return tooltipItem.label + ": " + currentValue + " (" + percentage + "%)";
                    }
                }
            }
        }

        if (this.data.data.datasets.length > 1) {
            var dataBack = {};
            super.addPlugin("legend",
                {
                    color: '#FFFFFF',
                    labels: {
                        generateLabels(chart) {
                            const data = chart.data;
                            if (data.labels.length && data.datasets.length) {
                                var outer = data.datasets[1].label.split("~~~")[1].split("---");
                                var outerLabels = []
                                var outerColors = []
                                outer.forEach(function (item, index) {
                                    outerLabels.push(item.split("|")[0]);
                                    outerColors.push(item.split("|")[1]);
                                });
                                data.labels = data.labels.filter(function (el) {
                                    return !outerLabels.includes(el);
                                });
                                data.labels = data.labels.concat(outerLabels);
                                return data.labels.map(function (label, i) {
                                    var meta = chart.getDatasetMeta(1);
                                    var style = meta.controller.getStyle(i);
                                    var isOuter = outerLabels.indexOf(label) != -1;
                                    if (!isOuter && !dataBack[i]) {
                                        var grupo = data.datasets[0].grupos[i];
                                        dataBack[i] = {
                                            "inner": data.datasets[1].data[i],
                                            "outer": {}

                                        };
                                        var itemsGrupo = 0;
                                        var grupoStart = -1;
                                        var indexTmp = 0;
                                        var grupoTmp = 0;
                                        for (var j = 0; j < data.datasets[0].grupos.length; j++) {
                                            if (data.datasets[0].grupos[j] != grupoTmp) {
                                                grupoTmp = data.datasets[0].grupos[j];
                                                indexTmp++;
                                            }
                                            if (indexTmp == i) {
                                                if (grupoStart == -1) {
                                                    grupoStart = j;
                                                }
                                                itemsGrupo++;
                                            }
                                        }
                                        for (var j = 0; j < itemsGrupo; j++) {
                                            dataBack[i].outer[j] = data.datasets[0].data[grupoStart + j];
                                        }
                                    }
                                    var hidden;
                                    var color = style.backgroundColor;
                                    if (isOuter) {
                                        hidden = false;
                                        color = outerColors[outerLabels.indexOf(label)];
                                    } else {
                                        hidden = isNaN(data.datasets[0].data[i]) || meta.data[i].hidden;
                                    }
                                    return {
                                        text: label,
                                        fillStyle: color,
                                        strokeStyle: color == "#FFFFFF" ? "#666666" : style.borderColor,
                                        lineWidth: style.borderWidth,
                                        hidden: hidden,
                                        index: i,
                                        grupos: data.datasets[0].grupos,
                                        data: dataBack[i]
                                    };
                                });
                            }
                            return [];
                        }
                    },
                    onClick: function (e, legendItem) {
                        const toggleMeta = (meta, index, numGrupo) => {
                            if (meta.data[index]) {
                                if (meta.data[index].hidden) {
                                    meta.data[index].hidden = false;
                                    this.chart.data.datasets[meta.index].data[index] = meta.index == 0 ? legendItem.data.outer[index >= numGrupo ? index % numGrupo : index] : legendItem.data.inner;
                                }
                                else {
                                    meta.data[index].hidden = true;
                                    this.chart.data.datasets[meta.index].data[index] = 0;
                                }

                            }
                        }
                        const innerMeta = this.chart.getDatasetMeta(1);
                        toggleMeta(innerMeta, legendItem.index);

                        const outerMeta = this.chart.getDatasetMeta(0);
                        var numItemsGrupo = 0;
                        var grupoStart = -1;
                        var indexTmp = 0;
                        var grupoTmp = 0;
                        for (var j = 0; j < legendItem.grupos.length; j++) {
                            if (grupoTmp != legendItem.grupos[j]) {
                                grupoTmp = legendItem.grupos[j];
                                indexTmp++;
                            }

                            if (indexTmp == legendItem.index) {
                                if (grupoStart == -1) {
                                    grupoStart = j;
                                }
                                numItemsGrupo++;
                            }
                        }
                        for (var j = 0; j < numItemsGrupo; j++) {
                            toggleMeta(outerMeta, grupoStart + j, numItemsGrupo);
                        }

                        this.chart.update();
                    }
                })
            super.addPlugin("tooltip", {
                callbacks: {
                    label: function (context) {
                        let prelabel = context.dataset.label.split('|')[context.dataIndex];
                        if (prelabel.includes('~~~')) prelabel = prelabel.split('~~~')[0];
                        let label = prelabel + ": ";
                        let valor = context.dataset.data[context.dataIndex];
                        return label + valor;
                    }
                }
            })
        }
        super.pintarGrafica(canvas[0].getContext("2d"), false);
        super.callbacks(ctx);
    }
}
async function getGrafica2(pIdPagina, pIdGrafica, pFiltroFacetas, barSize = 50, maxScales = null, pTitulo = null) {
    var url = url_servicio_graphicengine + "GetGrafica"; //"https://localhost:44352/GetGrafica"
    var arg = {};
    arg.pIdPagina = pIdPagina;
    arg.pIdGrafica = pIdGrafica;
    arg.pFiltroFacetas = pFiltroFacetas.includes("_filter") ? pFiltroFacetas.replace("_filter", "") : pFiltroFacetas;
    arg.pLang = lang; //"es";
    var data = await $.get(url, arg, function (data) { });

    if (data.isNodes) {
        return new GraficaNodos(pIdPagina, pIdGrafica, data, pFiltroFacetas, pTitulo);
    } else if (data.isVertical) {
        return new GraficaHorizontal(pIdPagina, pIdGrafica, data, pFiltroFacetas, pTitulo, maxScales, barSize, data.isAbr, data.hideLeyend);
    } else if (data.isHorizontal) {
        return new GraficaVertical(pIdPagina, pIdGrafica, data, pFiltroFacetas, pTitulo, maxScales, barSize, data.hideLeyend);
    } else {
        return new GraficaCircular(pIdPagina, pIdGrafica, data, pFiltroFacetas, pTitulo, data.hideLeyend);
    }

}
async function getPages(pContenedor, userId, pFiltroFacetas = "") {
    var url = url_servicio_graphicengine + "GetPaginasGraficas"; //"https://localhost:44352/GetPaginaGrafica";        
    var arg = {};
    arg.pLang = lang;
    arg.userId = $('.inpt_usuarioID').attr('value');
    // Petición para obtener los datos de la página.
    var listaData = await $.get(url, arg, function (listaData) {

    });

    for (let i = 0; i < listaData.length; i++) {
        $(".listadoMenuPaginas").append(`
                <li class="nav-item" id="${listaData[i].id}" num="${i}">
                    <a class="nav-link uppercase">${listaData[i].nombre}</a>
                </li>
            `);
        paginas.push(new Pagina(listaData[i].nombre, listaData[i].id, pContenedor, listaData[i], userId, pFiltroFacetas));
    }


    listaPaginas = listaData;
    return paginas;
}
async function getPagesUser(pContenedor = null, userId) {
    var url = url_servicio_graphicengine + "GetPaginasUsuario"; //"https://localhost:44352/GetPaginasUsuario"  
    var arg = {};
    arg.pUserId = userId;
    arg.pLang = lang;
    var listaData = await $.get(url, arg, function (listaData) { });
    for (let i = 0; i < listaData.length; i++) {
        url = url_servicio_graphicengine + "GetGraficasUser";
        arg = {};
        arg.pPageId = listaData[i].idRecurso;
        var paginaData = await $.get(url, arg, function (listaGraficas) { });
        paginas.push(new Pagina(listaData[i].titulo, listaData[i].idRecurso, pContenedor, paginaData, userId));
    }
    return paginas;

}
async function pintarGraficaIndividual(pContenedor, pIdPagina, idGrafica) {
    if (!$(pContenedor).hasClass("grafica")) {
        $(pContenedor).addClass("grafica");
    }
    var grafica = await getGrafica2(pIdPagina, idGrafica, "");
    grafica.data.groupId = null;
    grafica.pintarGrafica(pContenedor);
}


function comportamientos() {
    var menus = $(".toggleGraficas");
    menus.each((index, menu) => { // recorre todos los menus 

        var listItems = $(menu).find("ul").children(); // obtiene todos los items de un menu
        listItems.detach().sort(function (a, b) { // bordena los items
            return $(a).attr("order") < $(b).attr("order") ? -1 : 1;
        }).appendTo($(menu).find("ul")); // los agrega al menu
        var selectedID = $(menu).parents('div.acciones-mapa').parent().find("div.show.grafica").data("idGrafica"); //Obtiene la id de la grafica visible
        var paginaID = $(menu).parents('div.acciones-mapa').parent().find("div.show.grafica").data("idPagina"); //Obtiene la id de la pagina
        $(menu).find("a[value='" + "grafica_" + paginaID + "_" + selectedID + "']").addClass("active"); //Añade la clase active al item que esta visible
    });


    $(".toggleGraficas ul a")
        .unbind()
        .click(function (e) {
            // Establecemos la grafica seleccionada como activa en el menu
            $(this).parent().find(".active").removeClass("active");
            $(this).addClass("active");
            // enconntramos la grafica que esta siendo mostrada
            var parent = $(this).parents('div.acciones-mapa').parent();
            var shown = parent.find('div.grafica.show');



            // y la ocultamos
            shown.css('opacity', '0'); // display none csa problemas con redrawChart por que intenta modifica un elemento sin altura
            shown.css('position', 'absolute');
            // Muevo el div fuera de la página para que no cree espacios en blanco
            shown.css('left', '-9999px');
            shown.css('top', '-9999px');
            shown.css('z-index', '-1');

            shown.removeClass('show');
            shown.addClass('hide');

            // ahora buscamos la grafica que se quiere mostrar
            var selected = parent.find('#' + $(this).attr("value")).parents('div.hide');
            if (selected.length) { // si la grafica existe
                // la mostramos

                selected.css('display', 'flex');
                //selected.css('opacity', '1');
                selected.css('position', 'relative');
                selected.css('width', '100%');
                // Lo muevo de vuelta
                selected.css('left', '0px');
                selected.css('top', '0px');
                selected.css('z-index', '1');

                selected.removeClass('hide');
                selected.addClass('show');




                if (selected.attr('idgrafica').includes('nodes')) {
                    selected.parents('article').find('a#img').addClass('descargarcyto');
                    selected.parents('article').find('a#img').removeClass('descargar');
                } else {
                    selected.parents('article').find('a#img').addClass('descargar');
                    selected.parents('article').find('a#img').removeClass('descargarcyto');
                    comportamientos();
                }
            }
            shown = parent.find('div.grafica.show');
            if (shown.find(".chartScroll").hasClass("collapsed")) {
                parent.find(".expand span").text("open_in_full");
                parent.find(".expand").addClass("collapsed");
            } else {
                parent.find(".expand span").text("close_fullscreen");
                parent.find(".expand").removeClass("collapsed");

            }

        });

    $('div.labelContainer')
        .unbind()
        .click(function (e) {
            // Se obtiene el chart desde el canvas.
            var canvas = $(this).parents('div.chartWrapper').find('div.chartAreaWrapper canvas');
            var chart = Chart.getChart(canvas);
            // El ID del dataset está en el ID del contenedor del label.
            var id = $(this).attr('id').split('-')[1];
            var label = $(this).find('p.dataSetLabel');

            // Si el label no está tachado se tacha y oculta el dataset.
            if (label.css('text-decoration').indexOf("line-through") == -1) {
                label.css("text-decoration", "line-through");
                chart.setDatasetVisibility(id, false);
            } else {
                label.css("text-decoration", "none");
                chart.setDatasetVisibility(id, true);
            }

            try { // Hay problemas con el gráfico de líneas + grafico de barras stackeado, si falla se repinta el chart.
                chart.update();
            } catch (e) {
                chart.draw();
            }
        });

    $("div.expand")
        .unbind()
        .click(function (e) {
            var parent = $(this).parents('.acciones-mapa').parent()[0] || $(this).parents('.modal-body > .graph-container')[0];
            parent = $(parent);
            var canvas = parent.find('.show.grafica .chartAreaWrapper canvas')[0] || parent.find('.chartAreaWrapper canvas')[0];
            canvas = $(canvas);
            var myChart = Chart.getChart(canvas[0]);
            var data = myChart.data;
            var config = myChart.config;

            var idGrafica = canvas.attr('id').split('_')[2];
            var idPagina = canvas.attr('id').split('_')[1];
            if ($('div').hasClass('indicadoresPersonalizados')) {
                parent = $(this).parents('article > div.wrap');
                if (parent.length != 0) {
                    idGrafica = parent.find("div.grafica").attr("idGrafica");
                    idPagina = parent.find("div.grafica").attr("idPagina");
                    idGraficaActual = idGrafica;
                    idPaginaActual = idPagina;

                } else {
                    idPagina = idPaginaActual;
                    idGrafica = idGraficaActual;
                }
            }




            //plugin para que el color de fondo sea blanco.
            var plugin = {
                id: 'custom_canvas_background_color',
                beforeDraw: (chart) => {
                    chart.ctx.save();
                    chart.ctx.globalCompositeOperation = 'destination-over';
                    chart.ctx.fillStyle = '#FFFFFF';
                    chart.ctx.fillRect(0, 0, chart.width, chart.height);
                    chart.ctx.restore();
                }
            };
            var isExpanded = !$(this).hasClass('collapsed');
            $(this).toggleClass('collapsed');
            if (isExpanded) {
                $(this).find("span").text("open_in_full");

                data.datasets.forEach(function (dataset) {
                    delete dataset['barThickness'];
                });

                //Destruyo el chart para que se redibuje con el nuevo tamaño
                myChart.destroy();
                canvas.parents(".chartScroll").addClass('collapsed');
                canvas.parent().css('width', 'auto');
                var chartWrapper = canvas.parents(".chartWrapper");
                //Elimino la leyenda y los ejes
                chartWrapper.find(".myChartAxis, .chartLegend").remove();
                //Elimino el callback que llama a reDrawChart
                delete config.options.animation['onProgress'];

                //Remake the chart with the data obtained from the previous chart
                var newChart = new Chart(canvas[0].getContext('2d'), {
                    type: 'bar',
                    data: data,
                    options: config.options,
                    plugins: [plugin]
                });
                //getGrafica(idPagina, idGrafica, ObtenerHash2(), null, 50)
                idGraficaActual = idGrafica;
                idPaginaActual = idPagina;
            } else {
                // cambio el icono
                $(this).find("span").text("close_fullscreen");
                canvas.parents(".chartScroll").removeClass('collapsed');
                myChart.destroy();

                var filtro;
                var idPagina;
                if (!$('div').hasClass('indicadoresPersonalizados')) {
                    filtro = ObtenerHash2();
                    idPagina = idPaginaActual;
                } else {
                    filtro = (canvas).parents('div.grafica.show').attr("filtro");
                    idPagina = (canvas).parents('div.grafica.show').attr("idpagina") || idPaginaActual;
                    idGrafica = idGraficaActual;
                }
                $('#grafica_' + idPagina + '_' + idGrafica).attr('filtro', filtro);

                //obtenemos los datos y pintamos la grafica

                if (!$('div').hasClass('indicadoresPersonalizados')) {
                    getGrafica(idPagina, idGrafica, filtro, canvas[0], 50);
                } else {
                    //idGraficaActual = $(this).closest('article').find("div.show.grafica").attr("idrecurso");
                    var url = url_servicio_graphicengine + "GetGraficasUser"; //"https://localhost:44352/GetGraficasUser"
                    var arg = {};
                    arg.pPageId = idPaginaActual;
                    $.get(url, arg, function (listaData) {
                        listaData.forEach(data => {
                            if (data.idRecurso == idGraficaActual) {
                                tituloActual = data.titulo;
                            }
                        });
                        getGrafica(idPagina, idGrafica, filtro, canvas[0], 50, null, tituloActual)
                    });
                }
            }
        });

    for (const [key, callback] of Object.entries(callbacks)) {
        callback.zoom();
        callback.download()
        delete callbacks[key];

    };
}


function pintarAccionesMapa(pContenedor) {

    if ($(pContenedor).parent().find("div.acciones-mapa").length == 0) {

        var accionesMapa = (`<div class="wrap acciones-mapa"><div class="wrap"></div></div>`);
        $(pContenedor).parent().prepend(accionesMapa);
        $(pContenedor).parent().css("position", "relative");
        return $(pContenedor).parent().find("div.acciones-mapa");
    }
    else {
        return $(pContenedor).parent().find("div.acciones-mapa");
    }
}

function cerrarModal() {
    var ctx = $('#modal-ampliar-mapa').data("ctx");
    var grafica = ctx.data("grafica");
    if (grafica instanceof GraficaNodos) {

        ctx.find(".graficoNodos").empty();
        grafica.comportamientoNodos(ctx.find(".graficoNodos"));

    }
    $('#modal-ampliar-mapa').find('div.graph-container').empty();
    $('#modal-ampliar-mapa').find('div.acciones-mapa').empty();
    $('#modal-ampliar-mapa').removeClass('show');
    $('.modal-backdrop').removeClass('show');
    $('.modal-backdrop').css('pointer-events', 'none');
    $('#modal-ampliar-mapa').css('display', 'none');
    $('#modal-agregar-datos').removeClass('show');
    $('#modal-agregar-datos').css('display', 'none');
    $('#modal-admin-config').removeClass('show');
    $('#modal-admin-config').css('display', 'none');
    $('#labelTituloGraficaConfig').empty();
}
