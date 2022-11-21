//JS común a todas las fichas con buscadores
//Buscador personalizado
var buscadorPersonalizado = {
	nombreelemento: null,
	contenedor: null,
	filtro: null,
	orden:null,
	footer:"footer",
	article:"article",
	orders:null,
	init: function (nombreelemento, contenedor, filtro, orden, parametrosadicionales, urlcomunidad, idcomunidad, urlPush = "", callback = () => {}) {
		this.nombreelemento = nombreelemento;
		this.contenedor = contenedor;
		this.filtro = filtro;
		this.orden = orden;
		this.config();

		if (urlPush != "" && urlPush != null) {
			history.pushState('', 'New URL: ', ResetearURL());
			history.pushState('','',"?" + urlPush.join("&"))
		} else {
			history.pushState('', 'New URL: ', ResetearURL());
		}
		urlComunidad = urlcomunidad;
		urlCargarAccionesRecursos = urlcomunidad+'/load-resource-actions';
		panFacetas = 'panFacetas';
		panResultados = 'panResultados';
		numResultadosBusq = 'panNumResultados';
		panFiltrosPulgarcito = 'panListadoFiltros';
		updResultados = 'panResultados';
		divFiltros = 'panFiltros';
		ubicacionBusqueda = 'Meta';
		grafo = idcomunidad.toLowerCase();
		adminVePersonas = 'False';
		tipoBusqeda = 0;
		idNavegadorBusqueda = 'panNavegador';
		ordenPorDefecto = 'gnoss:hasfechapublicacion';
		ordenEnSearch = 'gnoss:relevancia';
		filtroContexto = "";
		tiempoEsperaResultados = 0;
		suplementoFiltros = '';
		primeraCargaDeFacetas = false;
		// TODO ¿De donde viene PestanyaActualID? Da error al hacer scroll y cargar resultados en proyectos. Con esta porción de código elimino el bug.
		if (parametrosadicionales.includes('PestanyaActualID=')) {
			parametrosadicionales = parametrosadicionales.split('|')[1];
		}
		parametros_adiccionales = parametrosadicionales;
		mostrarFacetas = true;
		mostrarCajaBusqueda = true;
		// FiltrarPorFacetas("");
		FiltrarPorFacetas(ObtenerHash2());
		MontarResultadosScroll.init(this.footer, this.article, callback());
		MontarResultadosScroll.CargarResultadosScroll = function (data) {
			var htmlRespuesta = document.createElement("div");
			htmlRespuesta.innerHTML = data;
			$(htmlRespuesta).find('article').each(function () {
				$('#panResultados article').last().after(this);
			});
			comportamientoVerMasVerMenosTags.init();
			enlazarFacetasBusqueda();
			if (callback && typeof(callback) === "function") {
				callback();
			}
		}
		return;
	},
	setScrollVars: function(footer, article) {
		this.footer = footer
		this.article  = article
	},
	resetScrollVars: function() {
		this.footer = "footer"
		this.article  = "article"
	},
	config: function (callback = () => {}) {
		var that = this;
		$('.searcherTitle').remove();
		$('.searcherFacets').remove();
		$('.searcherResults').remove();
		var ordersHtml="";
		if(this.orders!=null)
		{
			var txtOrderDefecto="";
			var htmlEnlacesOrder="";
			this.orders.forEach((order, i) => {
				if(i==0)
				{
					txtOrderDefecto=order.name;
					htmlEnlacesOrder+=`
								<a class="item-dropdown activeView" data-orderby="${order.orderby}" data-order="orden|desc">
																	<span class="material-icons">swap_vert</span>
																	<span class="texto">${order.name}</span> 
																</a>`;
				}else
				{
					htmlEnlacesOrder+=`
								<a class="item-dropdown" data-orderby="${order.orderby}" data-order="orden|desc">
																	<span class="material-icons">swap_vert</span>
																	<span class="texto">${order.name}</span> 
																</a>`;
				}
				
			})
			
			
			ordersHtml=`
						<div class="acciones-listado">
											<div class="wrap">
												<!-- Filtros de ordenación -->
													<div id="panel-orderBy" class="ordenar dropdown dropdown-select">
														<a class="dropdown-toggle active" data-toggle="dropdown" aria-expanded="false">
															<span class="material-icons">swap_vert</span>
															<span class="texto">${txtOrderDefecto}</span>
														</a>

														<div class="dropdown-menu basic-dropdown dropdown-menu-right" style="will-change: transform;">
															${htmlEnlacesOrder}	
														</div>
													</div>
											</div>
										</div>`;
		}
		
		var hmltBuscador = `
							<div class="col col-12 col-xl-3 col-facetas col-lateral izquierda searcherFacets">
								<div class="wrapCol">
									<div class="header-facetas">
										<p>Filtros</p>
										<a href="javascript: void(0);" class="cerrar">
											<span class="material-icons">close</span>
										</a>
									</div>
									<div id="panFacetas" class="facetas-wrap pmd-scrollbar mCustomScrollbar " data-mcs-theme="minimal-dark"></div>
								</div>
							</div>
							<div class="col col-12 col-xl-9 col-contenido derecha searcherResults">
								<div class="wrapCol">

									<div class="header-contenido">
										<!-- Número de resultados -->
										<div class="h1-container">
											<h1>${that.nombreelemento} <span id="panNumResultados" class="numResultados"></span></h1>
										</div>		
										${ordersHtml}
										<!-- Etiquetas de filtros -->
										<div class="etiquetas" id="panFiltros">
											<ul class="facetedSearch tags" id="panListadoFiltros">
												<li class="borrarFiltros-wrap">
													<a class="borrarFiltros" href="#" onclick="event.preventDefault();LimpiarFiltros();">Borrar</a>
												</li>
											</ul>
										</div>
									</div>
									<div class="col-buscador">
										<form method="post" id="buscadorPersonalizadoSearchForm" action="javascript:filtrarSearch()">
											<fieldset style="display: block">
												<legend class="nota">facetas</legend>
												<div class="finderUtils" id="divCajaBusqueda">
													<div class="group finderSection">
														<label for="finderSection" class="">Encontrar</label>
														<input type="text" class="not-outline finderSectionText autocompletar ac_input" autocomplete="off" placeholder="Buscar en sección">
														<input title="Encontrar" type="button" class="findAction" id="inputLupa">
														<input type="hidden" value="" class="inpt_urlPaginaActual">
														<input type="hidden" value="sioc_t:Tag|foaf:firstName" class="inpt_facetasBusqPag">
														<input type="hidden" class="inpt_parametros">
														<a href="javascript: void(0);" class="btn-filtrar-movil">
															<span class="material-icons">filter_list</span>
														</a>
													</div>
												</div>
											</fieldset>
										</form>
									</div>
									<!-- Resultados -->
									<div class="resource-list listView resource-list-buscador">
										<div id="panResultados" class="resource-list-wrap">
										</div>
									</div>
								</div>
							</div>`;
		$(this.contenedor).append(hmltBuscador);
		accionDropdownSelect.init();
		callback();
	}
}


/** 
 * Clase para trabajar en las gráficas de los colaboradores en las diferentes fichas
 */
class CargarGraficaProjectoObj {
    /**
     * Constructor
     * @param curi, uri para las gráficas
     */
    constructor (curi) {

	    this.dataCB = {}
	    this.idContenedorCB = ""
	    this.typesOcultar = ["relation_project", "relation_document"]
	    this.showRelation = true
	    this.url = ""

        this.url = url_servicio_externo + curi
    }

    /**
     * Método para actualizar las gráficas de colaboradores
     * Llama a la función externa de "AjustarGraficaArania"
     */
    actualizarGraficaColaboradores = () => {
        AjustarGraficaArania(this.dataCB, this.idContenedorCB, this.typesOcultar, this.showRelation)
    }

    /**
     * Método para Cargar las gráficas de colaboradores
     * @param pIdGrupo, Id del grupo
     * @param parametros, Parámetros de búsqueda
     * @param idContenedor, Id del contenedor sobre el que se va a pintar la gráfica
     * @param mostrarCargando, Indica si se va a mostrar el efecto "cargando la página"
     */
    CargarGraficaColaboradores = (arg, parametros, idContenedor, mostrarCargando = false) => {
        // var url = servicioExtermpBaseUrl + "servicioexterno/Hercules/DatosGraficaColaboradoresGrupo";
        // if (depuracion) {
        //     url = localUrlBase + "Hercules/DatosGraficaColaboradoresGrupo";
        // }
        
        var self = this
        arg.pParametros = parametros
        arg.pMax = $('#numColaboradores').val()
        $('#' + idContenedor).empty()
        if (mostrarCargando) {
            MostrarUpdateProgress()
        }

        this.typesOcultar = ["relation_project", "relation_document"]

        $.get(this.url, arg, function (data) {
            // Establecer los valores en la variable externa
            self.dataCB = data
            self.idContenedorCB = idContenedor

            self.actualizarGraficaColaboradores()
            if (mostrarCargando) {
                OcultarUpdateProgress()
            }
        })

    }

	/**
	 * Función a la que se llama para seleccionar qué elementos de las relaciones mostrar
	 * @param type, Indica si cierto tipo de elemento de las relaciones en la gráfica se debe ocultar o no
     * @param id, Id del html que ha lanzado la acción para darle el efecto de "tachado"
	 */
    actualizarTypesOcultar = (type, id) => {

        if (this.typesOcultar.includes(type)) {
            this.typesOcultar.splice(this.typesOcultar.indexOf(type), 1)
            document.getElementById(id).classList.add('tachado')
        } else {
            this.typesOcultar.push(type)
            document.getElementById(id).classList.remove('tachado')
        }
        this.actualizarGraficaColaboradores()
    }

}


function PintarGraficaPublicaciones(data,idContenedor) {	
	$('#'+idContenedor+'_aux').remove();
	$('#'+idContenedor).append($('<canvas id="'+idContenedor+'_aux" class="js-chart"></canvas>'));
	var ctx = document.getElementById(idContenedor+'_aux');
	data.options={
		scales: {
		  y1: {
			type: 'linear',
			display: true,
			position: 'left',
			title: {
              text: "Publicaciones",
              display: true,
              color: "red",
            },
		  },
		  y2: {
			type: 'linear',
			display: true,
			position: 'right',
			title: {
              text: "Citas",
              display: true,
              color: "red",
            },
		  },
		},		
		scale:{
			ticks:{
				precision:0
			}
		},
		maintainAspectRatio: false
	}

	// Sugested max number - Set the max value into the graphic y axis
	var itemsY1 = [];
	var itemsY2 = [];
	data.data.datasets.filter(e => e.yAxisID == "y1").forEach((e, index) => {
		// Inicialize the array
		if (itemsY1.length == 0) {
			for (n = 0; n < e.data.length; n++)
			{
				itemsY1[n] = 0;
				itemsY2[n] = 0;
			}
		}
		e.data.forEach((itm, i) => {
			itemsY1[i] = itemsY1[i] + itm;
		})
	});
	// Get the max number for y1
	var maxDataY1 = Math.max(...itemsY1);
	// Set the option
	data.options.scales.y1.suggestedMax = maxDataY1 + 1;

    
	// Sugested max number into y2 axis
	data.data.datasets.filter(e => e.yAxisID == "y2").forEach((e, index) => {
		e.data.forEach((itm, i) => {
			itemsY2[i] = itemsY2[i] + itm;
		})
	});

	// Get the max number for y2
	var maxDataY2 = Math.max(...itemsY2);
	// Set the option
	data.options.scales.y2.suggestedMax = maxDataY2 + 1;


	var parent = ctx.parentElement;
	var height = parent.offsetHeight;
	ctx.setAttribute('height', 400);
	var myChart = new Chart(ctx, data);
}

function PintarGraficaProyectos(data,idContenedorAnios,idContenedorMiembros,idContenedorAmbito) {	
	$('#'+idContenedorAnios).empty();
	$('#'+idContenedorAnios).parent().css("height", 450);
	$('#'+idContenedorMiembros).empty();
	$('#'+idContenedorMiembros).parent().css("height", 166);
	$('#'+idContenedorAmbito).empty();
	$('#'+idContenedorAmbito).parent().css("height", 234);	
	
	//Gráfico de barras años
	var ctxBarrasAnios = document.getElementById(idContenedorAnios);
	data.graficaBarrasAnios.options={
		scale:{
			ticks:{
				precision:0
			}
		},
		maintainAspectRatio: false
	}

	// Get the max suggested number for the y axio in graficaBarrasAnios graphic
	var items = [];
	data.graficaBarrasAnios.data.datasets.forEach(e => {
		items = [...items, ...e.data];
	});
	var maxData = Math.max(...items);

	data.graficaBarrasAnios.options.scale.suggestedMax = maxData + 1;

	var myChartBarrasAnios = new Chart(ctxBarrasAnios, data.graficaBarrasAnios);
	
	
	//Gráfico de barras miembros
	var ctxBarrasMiembros = document.getElementById(idContenedorMiembros);
	data.graficaBarrasMiembros.options={
		scale:{
			ticks:{
				precision:0
			}
		},
		plugins: { legend: { display: false } },
		maintainAspectRatio: false
	}


	// Get the max suggested number for the y axio
	var items = [];
	data.graficaBarrasMiembros.data.datasets.forEach(e => {
		items = [...items, ...e.data];
	});
	var maxData = Math.max(...items);

	data.graficaBarrasMiembros.options.scale.suggestedMax = maxData + 1;



	var myChartBarrasMiembros = new Chart(ctxBarrasMiembros, data.graficaBarrasMiembros);
	
	
	//Gráfico de sectores ambito
	var ctxBarrasAmbito = document.getElementById(idContenedorAmbito);
	data.graficaSectoresAmbito.options={
		scale:{
			ticks:{
				precision:0
			}
		},
		plugins: { legend: { display: false } },
		maintainAspectRatio: false
	}


	items = [];
	data.graficaSectoresAmbito.data.datasets.forEach(e => {
	  items = [...items, ...e.data];
	});
	maxData = Math.max(...items);

	data.graficaSectoresAmbito.options.scale.suggestedMax = maxData + 1;

	var myBarrasAmbito = new Chart(ctxBarrasAmbito, data.graficaSectoresAmbito);
}


function AjustarGraficaArania(data,idContenedor,typesOcultar = [],showRelation = true, graficaToShow = "default") {

	if (typesOcultar.length > 0) {
		data = data.filter(e => e.selectable === true || typesOcultar.includes(e.data.type));
	}

	if (showRelation == false) {
		let ipEl = data.filter(e => e.data.type === "none" || e.data.type === "icon_ip" || e.data.type === "icon_project");
		if (ipEl.length > 0) {
			let id = ipEl[0].data.id;
			if (id != "")
			{
				data = data.filter(e => e.selectable === true || e.data.source === id || e.data.target === id);
			}
		}
	}

	if (graficaToShow === "default") {
		PintarGraficaArania(data, idContenedor);
	} else if (graficaToShow === "circle") {
		PintarGraficaAraniaVersionCircle(data, idContenedor);
	}
}


function zoomCyIn() {
	window.cy.zoom(window.cy.zoom() + 0.2)
}

function zoomCyOut() {
	window.cy.zoom(window.cy.zoom() - 0.2)
}


function PintarGraficaArania(data,idContenedor) {
	let currentData = [...data];

	// La primera vez se pinta de nuevo
	var repintar = false;
	if (window.cy && window.cy._private && window.cy._private.ready !== true) {
		repintar = true;
	}

	// Se crea el objeto
	$('#'+idContenedor).empty();

	// Inicializamos la variable de la gráfica
	var cy = window.cy = null
	try {
		// intentamos inicializar la gráfica
		cy = window.cy = cytoscape({
			// Contenedor
			container: document.getElementById(idContenedor),
			// Layout
			layout: {
				name: 'cose',
				idealEdgeLength: 100,
				nodeOverlap: 20,
				refresh: 20,
				fit:true,
				padding: 30,
				randomize: false,
				componentSpacing: 100,
				nodeRepulsion: 400000,
				edgeElasticity: 100,
				nestingFactor: 5,
				gravity: 80,
				numIter: 1000,
				initialTemp: 200,
				coolingFactor: 0.95,
				minTemp: 1.0

			},
			// Estilos
			style: [{
				"selector": "node",
				"style": {
					//"width": "mapData(score, 0, 10, 45, 90)",
					//"height": "mapData(score, 0, 10, 45, 90)",
					"content": "data(name)",
					"font-size": "12px",
					"font-family": 'Roboto',
					//"font-color": "#999999",
					"background-color": "#b2cff7",
					"text-outline-width": "0px",
					"overlay-padding": "6px",
					"z-index": "10"
				}
			}, {
				"selector": "edge",
				"style": {
					"curve-style": "haystack",
					"content": "data(name)",
					"font-size": "24px",
					"font-family": 'Roboto',
					//"font-color": "#999999",
					"background-color": "#b2cff7",
					"haystack-radius": "0.5",
					"opacity": "0.5",
					"line-color": "#E1E1E1",
					"width": "mapData(weight, 0, 10, 3, 13)",

					"overlay-padding": "1px",
					"z-index": "11"
				}
			}],
			// Zoom
			userZoomingEnabled: false,
			minZoom: 0.5,
			maxZoom: 2.0,
			// Datos
			elements: data
		});
	} catch (error) {
		// ocultamos el "update progress en el caso de que haya habido un error en la carga de la gráfica"
		OcultarUpdateProgress();
		let element = document.getElementById(idContenedor)
		element.classList.add("d-none")
		chartWrapEl = element.closest(".chart-wrap")
		if (chartWrapEl) {
			chartWrapEl.classList.add("d-none")
		}
		
	}

	//cy.nodes(ele=>ele._private.edges.length == 0 && ele._private.data.type !="icon_ip")
	var arrayNodes = [];

	if (cy) {	
		var nodos = cy.nodes();
		for (i = 0; i < cy.nodes().length; i++) { //starts loop
			arrayNodes.push(nodos[i]._private.data.name);                
			switch (nodos[i]._private.data.type) {
				case 'icon_ip':
					cy.nodes()[i].style({
						'background-color': '#b2cff7',
						'background-image': 'https://cdn.iconscout.com/icon/free/png-256/user-1912184-1617653.png',
						'background-fit': 'cover',
						'border-width': '0px',
						'border-color': '#b2cff7',
						'shape': 'ellipse'
					})                        
					break;
				case 'icon_member':
					cy.nodes()[i].style({
						'background-color': '#b2cff7',
						'border-width': '0px',
						'border-color': '#b2cff7',
						'shape': 'ellipse'
					})
					break;
				case 'icon_project':
					cy.nodes()[i].style({
						'background-color': '#b2cff7',
						'background-image': 'https://cdn.iconscout.com/icon/free/png-256/briefcase-446-460356.png',
						'background-width': '65%',
						'background-height': '65%',
						'border-width': '0px',
						'border-color': '#b2cff7',
						'shape': 'ellipse'
					})
					break;
				default:
					nodos[i].style({
						'border-width': '0px',
						'border-color': '#b2cff7',
						'background-color': '#b2cff7',
						'shape': 'ellipse'
					});
					break;
			}
		};
		cy.nodes(ele=>ele._private.edges.length == 0 && ele._private.data.type !="icon_ip").remove()
		var arrayEdges = [];
		var edges = cy.edges();

		for (i = 0; i < cy.edges().length; i++) { //starts loop
			var data=edges[i]._private.data.id.split('~');	
			arrayEdges.push(data[data.length-1]);
			edges[i]._private.data.name = "";
			switch (edges[i]._private.data.type) {
				case 'relation_document':
					edges[i].style({
						"line-color": "#f8abab"
					})
					break;
				case 'relation_project':
					edges[i].style({
						"line-color": "#b2cff7"
					})
					break;
				default:
					edges[i].style({
						"line-color": "#E1E1E1"
					})
					break;
			}
		};

		cy.on('click', 'node', function (e) {
			e = e.target;
			var indice = cy.nodes().indexOf(e);
			if (e._private.data.name === "") {
				e._private.data.name = arrayNodes[indice];
			}
			else {
				e._private.data.name = "";
			}
		})

		cy.on('click', 'edge', function (e) {
			e = e.target;
			var indice = cy.edges().indexOf(e);
			if (e._private.data.name === "") {
				e._private.data.name = arrayEdges[indice];
			}
			else {
				e._private.data.name = "";
			}
		});

		
		// Colocar los elementos huérfanos a la derecha
		cy.ready(function(event) {		
			if (repintar ) {
				PintarGraficaArania(currentData,idContenedor);
			}
		});
	}
}


function PintarGraficaAraniaVersionCircle(data,idContenedor) {
	let currentData = [...data];

	// Se repinta la primera vez
	var repintar = false;
	if (window.cy && window.cy._private && window.cy._private.ready !== true) {
		repintar = true;
	}

	// Se crea el objeto de la gráfica
	$('#'+idContenedor).empty();
	var cy = window.cy = cytoscape({
		// Contenedor
		container: document.getElementById(idContenedor),
		// Layout
		layout: {
			name: 'circle',
			idealEdgeLength: 100,
            nodeOverlap: 20,
            refresh: 20,
            padding: 30,
            randomize: false,
            componentSpacing: 100,
            nodeRepulsion: 400000,
            edgeElasticity: 100,
            nestingFactor: 5,
            gravity: 80,
            numIter: 1000,
            initialTemp: 200,
            coolingFactor: 0.95,
            minTemp: 1.0,
			fit:true,
		},
		// Estilos
		style: [{
			"selector": "node",
			"style": {
				"target-text-rotation": "auto",
				//"width": "mapData(score, 0, 10, 45, 90)",
				//"height": "mapData(score, 0, 10, 45, 90)",
				"content": "data(name)",
				"font-size": "12px",
				"font-family": 'Roboto',
				//"font-color": "#999999",
				"background-color": "#b2cff7",
				"text-outline-width": "0px",
				"overlay-padding": "6px",
				"z-index": "10"
			}
		}, {
			"selector": "edge",
			"style": {
				"curve-style": "haystack",
				"content": "",
				"font-size": "24px",
				"font-family": 'Roboto',
				//"font-color": "#999999",
				"background-color": "#b2cff7",
				"haystack-radius": "0.5",
				"opacity": "0.5",
				"line-color": "#E1E1E1",
				"width": "mapData(weight, 0, 10, 0, 10)",
				"overlay-padding": "1px",
				"z-index": "11"
			}
		}],
		// Datos
		elements: data
	});

	var arrayNodes = [];
	var nodos = cy.nodes();

	for (i = 0; i < cy.nodes().length; i++) { //starts loop
		arrayNodes.push(nodos[i]._private.data.name);                
		switch (nodos[i]._private.data.type) {
			case 'icon_ip':
				cy.nodes()[i].style({
					'background-color': '#b2cff7',
					'background-image': 'https://cdn.iconscout.com/icon/free/png-256/user-1912184-1617653.png',
					'background-fit': 'cover',
					'border-width': '0px',
					'border-color': '#b2cff7',
					'shape': 'ellipse'
				})                        
				break;
			case 'icon_member':
				cy.nodes()[i].style({
					'background-color': '#b2cff7',
					// 'background-image': 'https://cdn.iconscout.com/icon/free/png-256/user-1648810-1401302.png',
					'background-fit': 'cover',
					'border-width': '0px',
					'border-color': '#b2cff7',
					'shape': 'ellipse'
				})
				break;
			default:
				nodos[i].style({
					'border-width': '0px',
					'border-color': '#b2cff7',
					'background-color': '#b2cff7',
					'shape': 'ellipse'
				});
				break;
		}
	};

	var arrayEdges = [];
	var edges = cy.edges();

	for (i = 0; i < cy.edges().length; i++) { //starts loop
		var data=edges[i]._private.data.id.split('~');	
		arrayEdges.push(data[data.length-1]);
		edges[i]._private.data.name = "";
		switch (edges[i]._private.data.type) {
			case 'relation_document':
				edges[i].style({
					"line-color": "#f8abab"
				})
				break;
			case 'relation_project':
				edges[i].style({
					"line-color": "#b2cff7"
				})
				break;
			default:
				edges[i].style({
					"line-color": "#E1E1E1"
				})
				break;
		}
	}

	cy.on('click', 'node', function (e) {
		e = e.target;
		var indice = cy.nodes().indexOf(e);
		if (e._private.data.name === "") {
			e._private.data.name = arrayNodes[indice];
		}
		else {
			e._private.data.name = "";
		}
	})

	cy.on('click', 'edge', function (e) {
		e = e.target;
		var indice = cy.edges().indexOf(e);
		if (e._private.data.name === "") {
			e._private.data.name = arrayEdges[indice];
		}
		else {
			e._private.data.name = "";
		}
	});


	//Se puede calcular el centro de la circunferencia mediante 3 puntos que la forman serían por ejemplo las coord de los 3 primeros nodos	
	AX=	cy.nodes()[0]._private.position.x	
	AY=	cy.nodes()[0]._private.position.y	
	BX=	cy.nodes()[1]._private.position.x	
	BY=	cy.nodes()[1]._private.position.y	
	CX=	cy.nodes()[2]._private.position.x	
	CY=	cy.nodes()[2]._private.position.y	

	var yDelta_a = BY - AY 
	var xDelta_a = BX - AX; 
	var yDelta_b = CY - BY; 
	var xDelta_b = CX - BX;

	var aSlope = yDelta_a / xDelta_a; 
	var bSlope = yDelta_b / xDelta_b;

	//Este es el centro de la circunferencia(x,y)
	coordCentroCircunferenciaX = (aSlope*bSlope*(AY - CY) + bSlope*(AX + BX) - aSlope*(BX+CX) )/(2* (bSlope-aSlope));
	coordCentroCircunferenciaY = -1*(coordCentroCircunferenciaX- (AX+BX)/2)/aSlope +  (AY+BY)/2;	 
	

	// Obtener el investigador principal
	let ipEl = cy.nodes().filter(e => e._private.data.type === "none" || e._private.data.type === "icon_ip");
	if (ipEl.length > 0) {
		let id = ipEl[0]._private.data.id;
		
		ipEl[0]._private.position.x=coordCentroCircunferenciaX
		ipEl[0]._private.position.y=coordCentroCircunferenciaY
	} 

	let nodox,nodoy,angulo;


	//Calculo de angulo para todos los nodos
	for (i = 0; i< cy.nodes().length; i=i+1) { //starts loop
		let nodo=cy.nodes()[i];

		//calcular angulo inicial y rotacion
		nodox=nodo._private.position.x
		nodoy=nodo._private.position.y
		

		
		//3 puntos el centro d la circunferencia el nodo y un punto sobre el eje x 
		C = { x: coordCentroCircunferenciaX, y: coordCentroCircunferenciaY };
		A = { x: nodox, y: 0 };
		B = { x: nodox,y:nodoy  };


		//Esta funcion calcula en angulo que forman 3 puntos 
		function find_angle(A,B,C) {
			var AB = Math.sqrt(Math.pow(B.x-A.x,2)+ Math.pow(B.y-A.y,2));    
			var BC = Math.sqrt(Math.pow(B.x-C.x,2)+ Math.pow(B.y-C.y,2)); 
			var AC = Math.sqrt(Math.pow(C.x-A.x,2)+ Math.pow(C.y-A.y,2));
			return Math.acos((BC*BC+AB*AB-AC*AC) / (2*BC*AB)) * (180 / Math.PI);   
		}

		let angulo = Math.round (
			find_angle(A,B,C)
		)
					
		//it can be much better but it works	
		while (angulo > 90) angulo -= 90 
		
		//Dependiendo del cuadrante del circulo son necesarios ajustar los angulos
        if (nodoy <= coordCentroCircunferenciaY) {
			if (nodox >= coordCentroCircunferenciaX) {  //0-90
				angulo =- angulo
			}
			else { //270-360 este
			}     
		}
		else {	
			if (nodox >= coordCentroCircunferenciaX) { //90-180
				angulo =- angulo + 90	
			}
			else {
				if (angulo != 0) angulo -= 90 
			}
		}

		//if(i<=q2) angulo=-90
		//if(i>q2) angulo-=90;
		var nodolabel = cy.nodes()[i]

		variable = i > (cy.nodes().length/2) ? 'right' : 'left'

		cy.nodes()[i].style({
		    'text-rotation': `${angulo}deg`,
			'background-color': 'white',
			'border-width': '2px',
			'border-color': 'rgb(4,184,209)',
			'shape': 'ellipse'
		});

	}

	cy.ready(function(event) {

		if (repintar) {
			setTimeout(function(){ PintarGraficaArania(currentData,idContenedor); }, 1000);
		}
    });
}

function PintarGraficaAreasTematicas(data,idContenedor) {	
	$('#'+idContenedor).empty();
	// Porcentajes en parte inferior.
	data.options.scales.x.ticks.callback = function (value) { return value + "%" }
	var altura = data.data.labels.length * 50;
	if(altura==0)
	{
		altura=50;
	}
	$('#'+idContenedor).removeAttr("style");
	$('#'+idContenedor).css("height", altura + 50);
	$('#'+idContenedor).append($(`<canvas id="${idContenedor}_aux" class="js-chart" width="600" height="' + altura + '"></canvas>`));
	var ctx = document.getElementById(idContenedor+'_aux');
	var parent = ctx.parentElement;
	var width = parent.offsetWidth;
	ctx.setAttribute('width', width);
	var height = parent.offsetHeight;
	ctx.setAttribute('height', height);
	var myChart = new Chart(ctx, data);
}

/** Creamos la función filtrar search para que funcione el filtro de los buscadores
* 
* @param callback, función opcional para cuando se realice la función
*/
function filtrarSearch(callback = () => {}) {

	let input = document.getElementById('buscadorPersonalizadoSearchForm').getElementsByClassName('finderSectionText');
	let searchID = $('#buscadorPersonalizadoSearchForm').closest('.row').attr('id');
	let search = '';
	if(searchID === 'contenedorBuscadorPublicaciones' || searchID === 'contenedorBuscadorRelacionados' 
		|| searchID === 'contenedorBuscadorResearchObjects' || searchID === 'contenedorBuscadorResearchObjectsModal' || searchID === 'ofertaListPublicaciones')
	{
		search = 'searcherPublications';
	}
	else if(searchID === 'contenedorBuscadorProyectos' || searchID === 'ofertaListProyectos')
	{
		search = 'searcherProjects';
	}
	else if(searchID === 'contenedorBuscadorMiembros' || searchID === 'contenedorBuscadorMiembrosFuera' 
		|| searchID === 'contenedorBuscadorColaboradores' || searchID === 'contenedorBuscadorParticipantes' || searchID === 'ofertaListUsers')
	{
		search = 'searcherPersons';
	}
	else if(searchID === 'searchOwnOffers' || searchID === 'ofertaListOtri')
	{
		search = 'searcherOffers';
	}
	else if(searchID === 'ofertaListPII')
	{
		search = 'searcherPII';
	}

	let parameterVal = input[0].value;
	let filtro = search + "=" + parameterVal;
	input[0].value = "";

	var url = new URL(location.href);
	let params = url.searchParams;
	params.delete(search);
	params.append(search, parameterVal); 

	let resultsParamsArr = []
	params.forEach(function(value, key) {
		resultsParamsArr.push(key + '=' +  value)
	});
	
	history.pushState('','','?' + (resultsParamsArr.join('&')))
	FiltrarPorFacetas(ObtenerHash2(), () => {
		callback()
	});
}

//Sobreescribimos FiltrarPorFacetas para que coja el filtro por defecto (y el orden)
function FiltrarPorFacetas(filtro, callback = () => {}) {
	if (buscadorPersonalizado.filtro != null) {
		filtro += "|" + buscadorPersonalizado.filtro;
	}
	if (buscadorPersonalizado.orden != null) {
		filtro += "|ordenarPor=" + buscadorPersonalizado.orden;
	}
	if (typeof (accionFiltrado) != 'undefined') {
		accionFiltrado(ObtenerHash2());
	}
	callback()
	return FiltrarPorFacetasGenerico(filtro);
}

function VerFaceta(faceta, controlID) {
    if (document.getElementById(controlID + '_aux') == null) {
        $('#' + controlID).parent().html($('#' + controlID).parent().html() + '<div style="display:none;" id="' + controlID + '_aux' + '"></div>');
        $('#' + controlID + '_aux').html($('#' + controlID).html());

        var filtros = ObtenerHash2();

        if (typeof (filtroDePag) != 'undefined' && filtroDePag != '') {
            if (filtros != '') {
                filtros = filtroDePag + '|' + filtros;
            }
            else {
                filtros = filtroDePag;
            }
        }
		if (buscadorPersonalizado.filtro != null) {
			filtros += "|" + buscadorPersonalizado.filtro;
		}
		if (buscadorPersonalizado.orden != null) {
			filtros += "|ordenarPor=" + buscadorPersonalizado.orden;
		}
        MontarFacetas(filtros, false, -1, '#' + controlID, faceta + '|vermas');
    }
    else {
        var htmlAux = $('#' + controlID + '_aux').html();
        $('#' + controlID + '_aux').html($('#' + controlID).html());
        $('#' + controlID).html(htmlAux);
        if (enlazarJavascriptFacetas) {
            enlazarFacetasBusqueda();
        }
        else {
            enlazarFacetasNoBusqueda();
        }
        CompletadaCargaFacetas();
    }
    return false;
}

//Limpia los filtros y engancha el comportamineto del popup
function comportamientoCargaFacetasComunidad() {
	if (buscadorPersonalizado.filtro != null) {
		$('#panListadoFiltros a[name="' + buscadorPersonalizado.filtro + '"]').closest('li').remove();
		if ($('#panListadoFiltros li').length == 1) {
			$('#panListadoFiltros li').remove();
		}
	}
	comportamientoFacetasPopUp.init();
	plegarSubFacetas.init();
	comportamientoRangosFechas();
	comportamientoRangosNumeros();
	if ((typeof stepsCls != 'undefined') && (typeof CompletadaCargaFacetasCluster != 'undefined')) {
		CompletadaCargaFacetasCluster();
	}
}

// Cuando se filtra no hay que subir arriba
function SubirPagina() {

}

//Resetea la URL eliminando los parámetros
function ResetearURL() {
	var urlActual = window.location.href;
	if (urlActual.includes("?")) {
		urlActual = urlActual.split("?")[0];
	}
	return urlActual;
}

//Scroll
MontarResultadosScroll.peticionScrollResultados = function () {
	var defr = $.Deferred();
	//Realizamos la peticion
	if (this.pagActual == null) {
		this.pagActual = 1;
	}
	this.pagActual++;
	var filtros = ObtenerHash2().replace(/&/g, '|');
	filtros += "|" + buscadorPersonalizado.filtro;
	if (buscadorPersonalizado.orden != null) {
		filtros += "|ordenarPor=" + buscadorPersonalizado.orden;
	}


	if (typeof (filtroDePag) != 'undefined' && filtroDePag != '') {
		if (filtros != '') {
			filtros = filtroDePag + '|' + filtros;
		}
		else {
			filtros = filtroDePag;
		}
	}
	filtros += "|pagina=" + this.pagActual;
	var params = {};



    try {
		params['pUsarMasterParaLectura'] = bool_usarMasterParaLectura;
		params['pProyectoID'] = $('input.inpt_proyID').val();
		params['pEsUsuarioInvitado'] = $('input.inpt_bool_esUsuarioInvitado').val() == 'True';
		params['pIdentidadID'] = $('input.inpt_identidadID').val();
		params['pParametros'] = '' + filtros.replace('#', '');
		params['pLanguageCode'] = $('input.inpt_Idioma').val();
		params['pPrimeraCarga'] = false;
		params['pAdministradorVeTodasPersonas'] = false;
		params['pTipoBusqueda'] = tipoBusqeda;
		params['pNumeroParteResultados'] = 1;
		params['pGrafo'] = grafo;
		params['pFiltroContexto'] = filtroContexto;
		params['pParametros_adiccionales'] = parametros_adiccionales;
		params['cont'] = contResultados;
		$.post(obtenerUrl($('input.inpt_UrlServicioResultados').val()) + "/CargarResultados", params, function (response) {
			if (params['cont'] == contResultados) {
				var data = response
				if (response.Value != null) {
					data = response.Value;
				}
				defr.resolve(data);
			}
		}, "json");
	}
	catch(err) {

	}
	return defr;
}

$(function () {
	// Comportamiento Ver Más / Ver Menos
	comportamientoVerMasVerMenosTags.init();

	// Comportamiento cabecera de las fichas
	mostrarFichaCabeceraFixed.init();
	
	if (mostrarFichaCabeceraFixed.contenido.length < 1) return;
	const position = mostrarFichaCabeceraFixed.contenido.position().top;
	$(window).scroll(function (e) {
		var scroll = $(window).scrollTop();
		if(scroll >= position) {
			body.addClass('cabecera-ficha-fixed');
			return;
		} else {
			body.removeClass('cabecera-ficha-fixed');
			return;
		}
	});
});