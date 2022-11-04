$(document).ready(function () {	
	comportamientoFacetasPopUp.init();	
	if ($('.resource-list-buscador').length) {
        MontarResultadosScroll.init('footer', 'article');
        MontarResultadosScroll.CargarResultadosScroll = function (data) {
            var htmlRespuesta = document.createElement("div");
            htmlRespuesta.innerHTML = data;
            $(htmlRespuesta).find('article').each(function () {
                $('#panResultados article').last().after(this);
            });
         comportamientoVerMasVerMenosTags.init();
		 enlazarFacetasBusqueda();
        }
	}
	montarTooltipCode.init();
	cargarCVId.init();
    comportamientoMenuLateral.init();
});
var cvUrl = ""

function GetText(id, param1, param2, param3, param4) {
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
}

var cargarCVId = {
	CVId:null,
	init: function () {
		this.loadCVId();
		this.printCVId();
		this.printCVIdHomeEd();
	},
	loadCVId: function(){
		var that=this;
		var keyCache='cv_'+lang+'_'+$('#inpt_usuarioID').val();
		this.CVId=getCacheWithExpiry(keyCache);
		if(this.CVId==null)
		{
			var usuarioID=$('#inpt_usuarioID').val();
			if(usuarioID!='ffffffff-ffff-ffff-ffff-ffffffffffff')
			{
				var urlGetCVUrl = url_servicio_editorcv+'EdicionCV/GetCVUrl?userID='+usuarioID+ "&lang=" + lang;
				$.get(urlGetCVUrl, null, function(data) {
					that.CVId=data;
					cvUrl = data;
					that.printCVId();
					that.printCVIdHomeEd();
					setCacheWithExpiry(keyCache,data,60000);
				});
			}
		}
	},
	printCVId: function(){
		if(this.CVId!=null && this.CVId!='')
		{
			$('#menuLateralUsuario .hasCV').show();
			$('#menuLateralUsuario li.liEditarCV a').attr('href',this.CVId);
            $('#curriculumvitae li.liOtros a.editcv').attr('href',this.CVId);
		}
	},
	printCVIdHomeEd: function(){
		if(this.CVId!=null && this.CVId!='')
		{
			$('#menuLateralUsuarioClonado #curriculumvitae a.editcv').attr('href',this.CVId);
			$('#menuLateralUsuarioClonado #trabajo a.editcvPub').attr('href',this.CVId + '?tab=http://w3id.org/roh/scientificActivity');
			$('#menuLateralUsuarioClonado #trabajo a.editcvPV').attr('href',this.CVId + '?tab=http://w3id.org/roh/scientificActivity');
			$('#menuLateralUsuarioClonado #trabajo a.editcvOR').attr('href',this.CVId + '?tab=http://w3id.org/roh/researchObject');
		}
	}
}

function enlazarFiltrosBusqueda() {
    // Cambiado por nuevo Front
    /*$('.limpiarfiltros')
    .unbind()
    .click(function (e) {
        LimpiarFiltros();
        e.preventDefault();
    });*/

    // Permitir borrar filtros de búsqueda
    $("#divFiltros")
    .unbind()
    .on('click', '.limpiarfiltros', function (event) {
            LimpiarFiltros();
            event.preventDefault();
    });
    $("#panFiltros")
        .unbind()
        .on('click', '.limpiarfiltros', function (event) {
            LimpiarFiltros();
            event.preventDefault();
    });
          
    $('.panelOrdenContenedor select.filtro')
    .unbind()
        .change(function (e) {            
        AgregarFiltro('ordenarPor', $(this).val(), true);
    });

    // Configurar la selección de ordenación de los resultados al pulsar en "Ordenado por"    
    $("#panel-orderBy a.item-dropdown")
        // En ordenación, no mostraba el icono seleccionado ya que lo "desmontaba".
        .unbind('.ordenar')
        .bind('click.ordenar',function (e) {                    
            var orderBy = $(this).attr("data-orderBy");
            var filtroOrder = $(this).attr("data-order");
            const concatFilterAndOrder = orderBy + "|" + filtroOrder.split("|")[1];
            AgregarFiltro('ordenarPor', concatFilterAndOrder, true);
    });

    // Orden Ascedente o Descedente
    $('#panel-orderAscDesc a.item-dropdown')
        .click(function (e) {
            var filtro = $(this).attr("data-order");
            AgregarFiltro(filtro.split('|')[0], filtro.split('|')[1], false);
            //e.preventDefault();
        });

    $('.panelOrdenContenedor a.filtroV2')
    .unbind()
    .click(function (e) {
        var filtro = $(this).attr("name");
        AgregarFiltro(filtro.split('-')[0], filtro.split('-')[1], false);
        e.preventDefault();
    });
    $('.paginadorResultados a.filtro')
    .unbind()
    .click(function (e) {
        var filtro = $(this).attr("name");
        AgregarFiltro(filtro.split('|')[0], filtro.split('|')[1], false);

        if (typeof searchAnalitics != 'undefined') {
            searchAnalitics.pageSearch(filtro.split('|')[1]);
        }
        e.preventDefault();
    });
}

function comportamientoCargaFacetasComunidad() {	
	comportamientoFacetasPopUp.init();	
	plegarSubFacetas.init();
	comportamientoRangosFechas();
	comportamientoRangosNumeros();
}

var setFilter = function(element) {
	// actualizarValoresSlider(self, pos);
	// Get the element data
	let theId = element.id;
	let filterFinalPart1 = $(element).data("filterpart1");
	let filterFinalPart2 = $(element).data("filterpart2");
	let facekey = $(element).data("facekey");
	let inputname1 = $(element).data("inputname1");
	let inputname2 = $(element).data("inputname2");

	let minDate = $("#" + inputname1).val();
	let maxDate = Number.parseInt($("#" + inputname2).val()) + 1;

	// Set the url for the filter
	let filterString = filterFinalPart1 + minDate + '0000-' + maxDate + '0000&' + filterFinalPart2;
	// window.location = filterString;

	// Set name attr
	$(element).parent().find(".searchButton").attr("name", facekey + '=' + minDate + '0000-' + maxDate + '0000');
	// Set href attr
	$(element).parent().find(".searchButton").attr("href", filterString);
	setFilterButtons(element);
}

var setFilterNumbers = function(element) {
	// Get the element data
	let theId = element.id;
	let filterFinalPart1 = $(element).data("filterpart1");
	let filterFinalPart2 = $(element).data("filterpart2");
	let facekey = $(element).data("facekey");
	let inputname1 = $(element).data("inputname1");
	let inputname2 = $(element).data("inputname2");
	let minDate = $("#" + inputname1).val();
	let maxDate = $("#" + inputname2).val();
	// Set the url for the filter
	let filterString = filterFinalPart1 + minDate + '-' + maxDate + '&' + filterFinalPart2;
	// Set name attr
	$(element).parent().find(".searchButton").attr("name", facekey + '=' + minDate + '-' + maxDate);
	// Set href attr
	$(element).parent().find(".searchButton").attr("href", filterString);
}

var setFilterButtons = function(element) {
	let filterFinalPart1 = $(element).data("filterpart1");
	let filterFinalPart2 = $(element).data("filterpart2");
	let facekey = $(element).data("facekey");

	let actYear = new Date().getFullYear();
	let maxYear = actYear + 1;
	let min5year = actYear - 4;
	let lastYear = actYear;

	// Set the url for the filter
	let filterString = filterFinalPart1 + min5year + '0000-' + maxYear + '0000&' + filterFinalPart2;

	// LAST 5 YEARS
	// Set name attr
	$(element).parent().find(".last5Years").attr("name", facekey + '=' + min5year + '0000-' + maxYear + '0000');
	// Set href attr
	$(element).parent().find(".last5Years").attr("href", filterString);

	// Set the url for the filter
	let filterStringAY = filterFinalPart1 + lastYear + '0000-' + maxYear + '0000&' + filterFinalPart2;

	// LAST YEAR
	// Set name attr
	$(element).parent().find(".lastYear").attr("name", facekey + '=' + lastYear + '0000-' + maxYear + '0000');
	// Set href attr
	$(element).parent().find(".lastYear").attr("href", filterStringAY);

	// Set the url for the filter
	let filterStringActY = filterFinalPart1 + '&' + filterFinalPart2;

	// ALL YEARS
	// Set name attr
	$(element).parent().find(".allYears").attr("name", facekey+ '=');
	// Set href attr
	$(element).parent().find(".allYears").attr("href", filterStringActY);
}

var changeSliderVals = function(sldrEl, el) {
	var value = el.value;
	totalVal = $(sldrEl).slider( "option", "values" );
	if (el.classList.contains('minVal')) {
		totalVal[0] = value;
	} else if(el.classList.contains('maxVal')) {
		totalVal[1] = value;
	}
	$(sldrEl).slider("values", totalVal);
}

function comportamientoRangosFechas()
{
	// Inicialite all facetas general range
	$(".faceta-general-range .ui-slider").each((i, e) => {
		$(e).slider({
			range: true,
			min: $(e).data('minnumber'),
			max: $(e).data('maxnumber'),
			values: [$(e).data('minnumber'), $(e).data('maxnumber')],
			slide: function(event, ui) {
				$("#" + $(e).data('inputname1')).val(ui.values[0]);
				$("#" + $(e).data('inputname2')).val(ui.values[1]);
			}
		});

		$(e).off("slidechange").on( "slidechange", function( event, ui ) {
			setFilter(e);
		});


		$(e).parent().find("input.filtroFacetaFecha").off("input").on( "input", function( event, ui ) {
			setFilter(e);
			changeSliderVals(e, this);
		});

		setFilter(e);
	});

	$('.faceta-general-range a.searchButton').unbind().click(function (e) {
		AgregarFaceta($(this).attr("name"),true);
		// Quitar el panel de filtrado para móvil para visualizar resultados correctamente
		$(body).removeClass("facetas-abiertas");
		e.preventDefault();
	});

	$('.faceta-general-range a.last5Years, .faceta-general-range a.lastYear, .faceta-general-range a.allYears').unbind().click(function (e) {
		AgregarFaceta($(this).attr("name"),true);
		// Quitar el panel de filtrado para móvil para visualizar resultados correctamente
		$(body).removeClass("facetas-abiertas");
		e.preventDefault();
	});
}

function comportamientoRangosNumeros()
{
	// Inicialite all facetas general range
	$(".faceta-general-number-range .ui-slider").each((i, e) => {
		$(e).slider({
			range: true,
			min: $(e).data('minnumber'),
			max: $(e).data('maxnumber'),
			values: [$(e).data('minnumber'), $(e).data('maxnumber')],
			slide: function(event, ui) {
				$("#" + $(e).data('inputname1')).val(ui.values[0]);
				$("#" + $(e).data('inputname2')).val(ui.values[1]);
		}
		});
		$(e).off("slidechange").on( "slidechange", function( event, ui ) {
			setFilterNumbers(e);
		});
		
		$(e).parent().find("input.filtroFacetaFecha").off("input").on( "input", function( event, ui ) {
			setFilterNumbers(e);
			changeSliderVals(e, this);
		});

		setFilterNumbers(e);
	});



	$('.faceta-general-number-range a.searchButton').unbind().click(function (e) {
		AgregarFaceta($(this).attr("name"),true);
		// Quitar el panel de filtrado para móvil para visualizar resultados correctamente
		$(body).removeClass("facetas-abiertas");
		e.preventDefault();
	});
}
	

function CompletadaCargaRecursosComunidad()
{	
	comportamientoVerMasVerMenos.init();
	comportamientoVerMasVerMenosTags.init();
	enlazarFacetasBusqueda();
	//montarTooltip.init();
	tooltipsAccionesRecursos.init();
	contarLineasDescripcion.init();
	
	// Engancha el filtro de persona a la minificha.
	$('.list-wrap.authors a.faceta')
	.unbind()
	.click(function (e) {
	    AgregarFaceta($(this).attr("name"));
        // Quitar el panel de filtrado para móvil para visualizar resultados correctamente
        $(body).removeClass("facetas-abiertas");
	    e.preventDefault();
	});
	
	if ((typeof CompletadaCargaRecursosCluster != 'undefined')) {
		CompletadaCargaRecursosCluster();
	}
	
	if ((typeof CompletadaCargaRecursosInvestigadoresOfertas != 'undefined')) {
		CompletadaCargaRecursosInvestigadoresOfertas();
	}
	
	if ((typeof CompletadaCargaRecursosProyectosOfertas != 'undefined')) {
		CompletadaCargaRecursosProyectosOfertas();
	}

	if ((typeof CompletadaCargaRecursosPublicacionesOfertas != 'undefined')) {
		CompletadaCargaRecursosPublicacionesOfertas();
	}

	if ((typeof CompletadaCargaRecursosDocumentosROsVincular != 'undefined')) {
		CompletadaCargaRecursosDocumentosROsVincular();
	}
	
	if ((typeof CompletadaCargaRecursosSimilitud != 'undefined')) {
		CompletadaCargaRecursosSimilitud();
	}
	
	if ((typeof CompletadaCargaRecursosPIIOfertas != 'undefined')) {
		CompletadaCargaRecursosPIIOfertas();
	}
	
	if ((typeof gOTecnEndLoadedResources != 'undefined')) {
		gOTecnEndLoadedResources();
	}
	tooltipsImpactFactor();
}

comportamientoFacetasPopUp.numPaginas=2,
comportamientoFacetasPopUp.numResultadosPagina=10,
comportamientoFacetasPopUp.init= function () {
	this.config();
	this.IndiceFacetaActual = 0;
};
comportamientoFacetasPopUp.config= function () {
	//- -> :
	//--- -> @@@
	//1º Nombre de la faceta
	//2º Titulo del buscador ES
	//3º Titulo del buscador EN
	//4º True para ordenar por orden alfabético, False para utilizar el orden por defecto
	var that = this;
	this.facetasConPopUp = [
	['bibo:authorList@@@rdf:member@@@foaf:name', 'Busca por nombre o apellido de la persona', "Search by person's name or surname", true],
	['vivo:hasPublicationVenue', 'Busca por nombre de la revista', "Search by journal's name", true],
	['bibo:authorList@@@rdf:member@@@roh:hasKnowledgeArea@@@roh:categoryNode', 'Busca por tópicos', "Search by topics", true],
	['vivo:departmentOrSchool@@@dc:title', 'Busca por departamento', "Search by department", true],
	['vivo:relates@@@roh:title','Busca por grupos de investigación','Search by research group',true],
	['roh:lineResearch','Busca por línea de investigación','Search by research line',true],
	['vivo:hasPublicationVenue@@@roh:title','Busca por revista','Search by journal',true]
	
	];
	//TODO textos
	for (i = 0; i < this.facetasConPopUp.length; i++) {
		var faceta = this.facetasConPopUp[i][0];
		var facetaSinCaracteres = faceta.replace(/\@@@/g, '---').replace(/\:/g, '_');
		var enlaceVerTodos = `<a class="no-close open-popup-link" href="#" data-toggle="modal" faceta="${i}" data-target="#modal-resultados">Ver todos</a>`;
		if (configuracion.idioma == 'en') 
		{
			enlaceVerTodos = `<a class="no-close open-popup-link" href="#" data-toggle="modal" faceta="${i}" data-target="#modal-resultados">See all</a>`;
		}
		if ($('#panFacetas #' + facetaSinCaracteres + ' .moreResults').length > 0) {
			if ($('#panFacetas #' + facetaSinCaracteres + ' .moreResults .open-popup-link ').length == 0) {
				$('#panFacetas #' + facetaSinCaracteres + ' .moreResults').html(enlaceVerTodos);
			} 
		}
	}		
	
	$('#panFacetas .open-popup-link').unbind().click(function(event) 
	{		
		$(".indice-lista.no-letra").html('');
		event.preventDefault();
		$('#modal-resultados .modal-dialog .modal-content .modal-title').text($($(this).closest('.box')).find('.faceta-title').text());
		that.IndiceFacetaActual = parseInt($(this).attr('faceta'));
		that.cargarFaceta();
	});	

	this.facetasConPopUpCategorias = [
		['roh:hasKnowledgeArea@@@roh:categoryNode', 'Busca por nombre de la categoría', "Search by category name", true],
		['vivo:hasResearchArea@@@roh:categoryNode', 'Busca por nombre de la categoría', "Search by category name", true]//Categorias
        ];
	for (i = 0; i < this.facetasConPopUpCategorias.length; i++) {
		var faceta = this.facetasConPopUpCategorias[i][0];
		var facetaSinCaracteres = faceta.replace(/\@@@/g, '---').replace(/\:/g, '_');
		var enlaceVerTodos = `<p class="moreResults"><a class="no-close open-popup-link open-popup-link-tesauro" href="#" data-toggle="modal" faceta="${i}" data-target="#modal-tesauro">Ver todos</a></p>`;
		if (configuracion.idioma == 'en') 
		{
			enlaceVerTodos =`<p class="moreResults"><a class="no-close open-popup-link open-popup-link-tesauro" href="#" data-toggle="modal" faceta="${i}" data-target="#modal-tesauro">See all</a></p>`;
		}
		$('#panFacetas #' + facetaSinCaracteres+' .moreResults').remove();
		$('#panFacetas #' + facetaSinCaracteres).append(enlaceVerTodos);
	}			
	
	$('#panFacetas .open-popup-link-tesauro').unbind().click(function(event) 
	{		
		$("#modal-tesauro input.texto").val('');
		event.preventDefault();		
		$('#modal-tesauro .modal-title').text($($(this).closest('.box')).find('.faceta-title').text());		
		if( $('input.inpt_Idioma').val()=='es')
		{
			$('#modal-tesauro .buscador-coleccion .buscar .texto').attr('placeholder',that.facetasConPopUpCategorias[0][1]);
		}else
		{
			$('#modal-tesauro .buscador-coleccion .buscar .texto').attr('placeholder',that.facetasConPopUpCategorias[0][2]);
		}	
		$('#modal-tesauro ul.listadoTesauro').html($(this).closest('.box').find('ul.listadoFacetas').html());
		$('#modal-tesauro ul.listadoTesauro .material-icons').html('expand_more');
		$('#modal-tesauro ul.listadoTesauro .num-resultados').addClass('textoFaceta');
		$('#modal-tesauro ul.listadoTesauro').addClass('facetedSearch');
		$('#modal-tesauro ul.listadoTesauro a').attr('data-dismiss','modal');
		$('#modal-tesauro ul.listadoTesauro a.applied').addClass('selected');
		plegarSubFacetas.init();
		enlazarFacetasBusqueda();
		operativaFormularioTesauro.init();
				
	});	

	//Autocompletar tesauros		
	$('#modal-tesauro input.texto').off('keyup').on('keyup', function (e) {		
		var txt=$(this).val();
		var lista = $('#modal-tesauro ul.listadoTesauro').find('li');
		lista.each(function(indice) {
			var item = $(this);
			var enlaceItem = item.children('a');
			var itemText = enlaceItem.text();
			item.removeClass('oculto');
			if (itemText.toLowerCase().indexOf(txt.toLowerCase()) < 0) {
				item.addClass('oculto');
			} else {
				item.removeHighlight().highlight(txt);
				item.parents('.oculto').removeClass('oculto');
			}
		});	
	});
	
	
};
comportamientoFacetasPopUp.eliminarAcentos= function (texto) {
	var ts = '';
	for (var i = 0; i < texto.length; i++) {
		var c = texto.charCodeAt(i);
		if (c >= 224 && c <= 230) { ts += 'a'; }
		else if (c >= 232 && c <= 235) { ts += 'e'; }
		else if (c >= 236 && c <= 239) { ts += 'i'; }
		else if (c >= 242 && c <= 246) { ts += 'o'; }
		else if (c >= 249 && c <= 252) { ts += 'u'; }
		else { ts += texto.charAt(i); }
	}
	return ts;
};
comportamientoFacetasPopUp.cargarFaceta= function () {
	var that = this;
	var FacetaActual = that.facetasConPopUp[that.IndiceFacetaActual][0];
	var facetaSinCaracteres = FacetaActual.replace(/\@@@/g, '---').replace(/\:/g, '--');
	this.paginaActual = 1;
	this.textoActual = '';
	this.fin = true;
	this.buscando = false;
	this.arrayTotales = null;

	if( $('input.inpt_Idioma').val()=='es')
	{
		$('.buscador-coleccion .buscar .texto').attr('placeholder',that.facetasConPopUp[that.IndiceFacetaActual][1]);
	}else
	{
		$('.buscador-coleccion .buscar .texto').attr('placeholder',that.facetasConPopUp[that.IndiceFacetaActual][2]);
	}
	$('.mfp-content h2').text($('#panFacetas div[faceta=' + facetaSinCaracteres + '] h2 ').text());
	this.textoActual = '';
	$(".indice-lista.no-letra").html('');
	
	var metodo = 'CargarFacetas';
	var params = {};
	params['pProyectoID'] = $('input.inpt_proyID').val();
	params['pEstaEnProyecto'] = $('input.inpt_bool_estaEnProyecto').val() == 'True';
	params['pEsUsuarioInvitado'] = $('input.inpt_bool_esUsuarioInvitado').val() == 'True';
	params['pIdentidadID'] = $('input.inpt_identidadID').val();
	//params['pParametros'] = '' + replaceAll(replaceAll(replaceAll(ObtenerHash2().replace(/&/g, '|').replace('#', ''), '%', '%25'), '#', '%23'), '+', "%2B");
	
	var filtros = ObtenerHash2();
    filtros = replaceAll(filtros, '%26', '---AMPERSAND---');
    filtros = decodeURIComponent(filtros);
    filtros = replaceAll(filtros, '---AMPERSAND---', '%26');
	filtros = replaceAll(filtros, '&', '|');
	params['pParametros']=filtros;
	
	
	if(typeof filtroPersonalizado !== 'undefined' && filtroPersonalizado!=null && filtroPersonalizado!='')
	{
		 params['pParametros'] +=filtroPersonalizado;
	}
	
	if(typeof buscadorPersonalizado !== 'undefined' && buscadorPersonalizado.filtro!=null && buscadorPersonalizado.filtro!='')
	{
		 params['pParametros'] +='|'+buscadorPersonalizado.filtro;
	}
	
	
	params['pLanguageCode'] = $('input.inpt_Idioma').val();
	params['pPrimeraCarga'] = false;
	params['pAdministradorVeTodasPersonas'] = false;
	params['pTipoBusqueda'] = tipoBusqeda;
	params['pGrafo'] = grafo;
	params['pFiltroContexto'] = filtroContexto;
	params['pParametros_adiccionales'] = parametros_adiccionales + '|NumElementosFaceta=10000|';
	params['pUbicacionBusqueda'] = ubicacionBusqueda;
	params['pNumeroFacetas'] = -1;
	params['pUsarMasterParaLectura'] = bool_usarMasterParaLectura;
	params['pFaceta'] = FacetaActual;

	$('.buscador-coleccion .buscar .texto').keyup(function () {				
		that.textoActual = that.eliminarAcentos($(this).val());
		that.paginaActual = 1;
		that.buscarFacetas();
	});



	$.post(obtenerUrl($('input.inpt_UrlServicioFacetas').val()) + "/" + metodo, params, function (data) {
		var htmlRespuesta = $('<div>').html(data);
		that.arrayTotales = new Array($(htmlRespuesta).find('.faceta').length);
		var i = 0;
		$(htmlRespuesta).find('.faceta').each(function () {
			that.arrayTotales[i] = new Array(2);
			that.arrayTotales[i][0] = that.eliminarAcentos($(this).text().toLowerCase());
			that.arrayTotales[i][1] = $(this);
			i++;
		});

		//Ordena por orden alfabético
		if (that.facetasConPopUp[that.IndiceFacetaActual][3]) {
			that.arrayTotales = that.arrayTotales.sort(function (a, b) {
				if (a[0] > b[0]) return 1;
				if (a[0] < b[0]) return -1;
				return 0;
			});
		}

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
	});
};
comportamientoFacetasPopUp.buscarFacetas= function () {
	buscando = true;
	this.textoActual = this.textoActual.toLowerCase();
	$(".indice-lista.no-letra ul").remove();
	
	$(".indice-lista.no-letra.facetas-wrap").append($('<div></div>').attr('class', 'resultados-wrap'));		
	
	var facetaMin = ((this.paginaActual - 1) * (this.numPaginas *this.numResultadosPagina)) + 1;
	var facetaMax = facetaMin + (this.numPaginas *this.numResultadosPagina) -1;

	var facetaActual = 0;
	var facetaPintadoActual = 0;
	var ul = $('<ul>');

	this.fin = true;

	var arrayTextoActual = this.textoActual.split(" ");

	for (i = 0; i < this.arrayTotales.length; i++) {
		var nombre = this.arrayTotales[i][0];

		var mostrar = true;
		for (j = 0; j < arrayTextoActual.length; j++) {
			mostrar = mostrar && nombre.indexOf(arrayTextoActual[j]) >= 0;
		}

		if (facetaPintadoActual < (this.numPaginas *this.numResultadosPagina) && mostrar) {
			facetaActual++;
			if (facetaActual >= facetaMin && facetaActual <= facetaMax) {
				facetaPintadoActual++;
				if (facetaPintadoActual % this.numResultadosPagina == 1) {
					ul = $('<ul>').attr('class', 'listadoFacetas').css('margin-left', '30px;').css('opacity', '1');
					$(".indice-lista.no-letra.facetas-wrap .resultados-wrap").append(ul);						
				} 
				var li = $('<li>');
				li.append(this.arrayTotales[i][1]);					
				ul.append(li);
			}
		}
		if (this.fin && facetaPintadoActual == (this.numPaginas *this.numResultadosPagina) && mostrar) {
			this.fin = false;
		}
	}
	
	$(".indice-lista.no-letra.facetas-wrap ul li a").each(function () {		
		if(!$(this).find('span.resultado').length)
		{
			var resultado=$(this).find('span.textoFaceta').text();
			var numResult=$($(this).find('span')[1]).html();
			$(this).empty();
			$(this).append('<span class="textoFaceta">'+resultado+'</span>');
			$(this).append('<span class="num-resultados">'+numResult+'</span>');
			$(this).attr('onclick',"$('#modal-resultados').modal('hide')");
		}			
	});

	$('.indice-lista .faceta').click(function (e) {
		AgregarFaceta($(this).attr("name").replace('#','%23'));
		$('button.mfp-close').click();
		e.preventDefault();
	});
	
	
	if(this.paginaActual==1)
	{
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-anterior-facetas-modal .texto').addClass('disabled');
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-anterior-facetas-modal .material-icons').addClass('disabled');
	}else
	{
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-anterior-facetas-modal .texto').removeClass('disabled');
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-anterior-facetas-modal .material-icons').removeClass('disabled');
	}
	if(this.fin)
	{
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-siguiente-facetas-modal .texto').addClass('disabled');
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-siguiente-facetas-modal .material-icons').addClass('disabled');
	}else
	{
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-siguiente-facetas-modal .texto').removeClass('disabled');
		$('.modal-body .buscador-coleccion .action-buttons-resultados .no-list-style .js-siguiente-facetas-modal .material-icons').addClass('disabled');
	}
	this.buscando = false;
}

var MontarResultadosScroll = {
    footer: null,
    item: null,
    pagActual: null,
    active: true,
    init: function (idFooterJQuery, idItemJQuery, callback = () => {}) {
        this.pagActual = 1;
        this.footer = $(idFooterJQuery);
        this.item = idItemJQuery;
        this.cargarScroll(callback());
        return;
    },
    cargarScroll: function (callback = () => {}) {
        var that = this;
        that.destroyScroll();
        // opciones del waypoint
        var opts = {
            offset: '100%'
        };
		contarLineasDescripcion.init();
        that.footer.waypoint(function (event, direction) {
            that.launchCallWaypoint(callback())
        }, opts);
        return;
    },
    launchCallWaypoint: function(callback = () => {}) {
    	var that = this;
    	that.peticionScrollResultados().done(function (data) {
	        that.destroyScroll();
	        var htmlRespuesta = document.createElement("div");
	        htmlRespuesta.innerHTML = data;
	        if ($(htmlRespuesta).find(that.item).length > 0) {
	            that.CargarResultadosScroll(data);
	            that.cargarScroll();
	        } else {
	            that.CargarResultadosScroll('');
	        }
	        if ((typeof CompletadaCargaRecursos != 'undefined')) {
	            CompletadaCargaRecursos();
	        }
	        if (typeof (urlCargarAccionesRecursos) != 'undefined') {
	            ObtenerAccionesListadoMVC(urlCargarAccionesRecursos);
	        }
	        callback();
	    });
    },
    destroyScroll: function () {
        this.footer.waypoint('destroy');
        return;
    },
    peticionScrollResultados: function () {
        var defr = $.Deferred();
        //Realizamos la peticion 
        if (this.pagActual == null) {
            this.pagActual = 1;
        }
        this.pagActual++;
        //var servicio = new WS($('input.inpt_UrlServicioResultados').val(), WSDataType.jsonp);
        var filtros = ObtenerHash2().replace(/&/g, '|');

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
        return defr;
    }
}

montarTooltip.lanzar = function (elem, title, classes) {
	if (title != undefined) 
	{
        elem.tooltip({
            html: true,
            placement: 'bottom',
            template: '<div class="tooltip ' + classes + '" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
            title: title
        }).on('mouseenter', function () {
            var _this = this;
            // $(this).tooltip('show');
            $('.tooltip').on('mouseenter', function () {
                $(this).tooltip('show');
            }).on('mouseleave', function () {
                $(this).tooltip('hide')
            })
        }).on('mouseleave', function () {
            var _this = this;
            setTimeout(function () {
                if ($('.tooltip:hover').length < 0) {
                    $(_this).tooltip("hide")
                }
                $('.tooltip').on('mouseleave', function () {
                    $(_this).tooltip("hide");
                });
            });
        });
	}
    this.comportamiento(elem);
};


tooltipsAccionesRecursos.getTooltipQuotes= function () {
	var that = this;	
	this.quotes.each(function () {
			var scopusInt=$(this).data('scopus');
			var wosInt=$(this).data('wos');
			var inrecsInt=$(this).data('inrecs');
			var otrasCitas=$(this).data('otros');

			var htmlScopus = "";
			if(typeof scopusInt !== "undefined" && scopusInt != "" && scopusInt != "0"){
				htmlScopus=`
				<li>					
					<span class="texto">SCOPUS</span>
					<span class="num-resultado">${scopusInt}</span>					
				</li>`;
			}
			
			var htmlWos = "";
			if(typeof wosInt !== "undefined" && wosInt != "" && wosInt != "0"){
				htmlWos=`
				<li>					
					<span class="texto">WOS</span>
					<span class="num-resultado">${wosInt}</span>					
				</li>`;
			}
			
			var htmlInrecs = "";
			if(typeof inrecsInt !== "undefined" && inrecsInt != "" && inrecsInt != "0"){
				htmlInrecs=`
				<li>					
					<span class="texto">INRECS</span>
					<span class="num-resultado">${inrecsInt}</span>					
				</li>`;
			}
			
			var htmlOtros = "";
			if(typeof otrasCitas !== "undefined" && otrasCitas != ""){
				
				var listaSplit = otrasCitas.split("|");
				
				if(listaSplit != null && listaSplit.length > 0)
				{
					listaSplit.forEach( function(valor, indice, array) {
						var nombreCita = valor.split("~")[0];
						var numCita = valor.split("~")[1];
						if(nombreCita != "" && numCita != "")
						{
							htmlOtros +=`
							<li>					
								<span class="texto">${nombreCita}</span>
								<span class="num-resultado">${numCita}</span>					
							</li>`;
						}
					});
				}
			}
			
			var html=`<p class="tooltip-title">Fuente de citas</p>
                <ul class="no-list-style">
				${htmlScopus}				
                ${htmlWos}
                ${htmlInrecs}
				${htmlOtros}
                </ul>`;
				
			if((typeof scopusInt !== "undefined" && scopusInt != "" && scopusInt != "0") || (typeof wosInt !== "undefined" && wosInt != "" && wosInt != "0") || (typeof inrecsInt !== "undefined" && inrecsInt != "" && inrecsInt != "0") || (typeof otrasCitas !== "undefined" && otrasCitas != "" && otrasCitas != "0"))
			{
				$(this).tooltip({
					html: true,
					placement: 'bottom',
					template: '<div class="tooltip background-blanco citas" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
					title: html
				});
			} else {
				$(this).tooltip({
					html: true,
					placement: 'bottom',
					template: '<div class="tooltip background-blanco citas" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
					title: `<p class="tooltip-title">Fuente de citas</p>
		                <span class="material-icons cerrar">close</span>
		                <ul class="no-list-style">
		                    <li>
		                        <span class="texto">No hay datos</span>
		                    </li>
		                </ul>`
				});
			}
	});
}

tooltipsAccionesRecursos.getTooltipHindex= function () {
	var that = this;	
    $(".hindex").each(function () {

			var htmlSources = "";
            var list =$(this).parent().find(".source");
            list.sort((a,b)=>{
                if($(a).data("source")=="WOS"){ return -1}
                if($(b).data("source")=="WOS"){return 1}
                if($(a).data("source")=="SCOPUS"){return -1}
                return 0;
            })
            list.each((i)=>{
                    htmlSources+=`
                    <li>					
                        <span class="texto">${$(list[i]).data('source')}</span>
                        <span class="num-resultado">${$(list[i]).data('value')}</span>					
                    </li>`
                
            });
					
			var html=`<p class="tooltip-title">Fuente de citas</p>
                <ul class="no-list-style">
				${htmlSources}				
       
                </ul>`;
				
			if((typeof htmlSources !== "undefined" && htmlSources != "" && htmlSources != "0"))
			{
				$(this).tooltip({
					html: true,
					placement: 'bottom',
					template: '<div class="tooltip background-blanco citas" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
					title: html
				});
			} else {
				$(this).tooltip({
					html: true,
					placement: 'bottom',
					template: '<div class="tooltip background-blanco citas" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
					title: `<p class="tooltip-title">Fuente de citas</p>
		                <span class="material-icons cerrar">close</span>
		                <ul class="no-list-style">
		                    <li>
		                        <span class="texto">No hay datos</span>
		                    </li>
		                </ul>`
				});
			}
	});
}
var montarTooltipCode = {
	// Init the function
    init: function () {
        this.config();
        this.comportamiento();
    },
    // Get the items to set the tooltip
    config: function () {
        this.body = body;
        this.codeTooltip = this.body.find('.code-tooltip');
    },
    // Create & initialize the tooltip
    comportamiento: function () {
        var that = this;

        // Create the tooltip
        if (this.codeTooltip.length > 0) {
			this.codeTooltip.tooltip({
	            html: true,
	            placement: 'bottom',
	            template: '<div class="tooltip background-blanco code-lang-tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
	            title: that.getTooltipCode()
	        });
        }
        
    },
    // Get the code into the tooltip
    getTooltipCode: function () {
        	var that = this;

        	// Get all data from the element
			var dataLang = this.codeTooltip.data();
			var htmlResData = "";

			// Fill the data into the tooltip
			Object.keys(dataLang).forEach(e => {
				if(typeof dataLang[e] !== "undefined" && dataLang[e] != "" && dataLang[e] != "0"){
					htmlResData += `
					<li>					
						<span class="texto">${e}</span>
						<span class="num-resultado">${dataLang[e]}</span>					
					</li>`;
				}
			});
			
			// Set the final html
			var html=`<p class="tooltip-title">Lenguajes de programación</p>
                <ul class="no-list-style">
				${htmlResData}
                </ul>`;

            return html;
    }
};

function MontarResultados(pFiltros, pPrimeraCarga, pNumeroResultados, pPanelID, pTokenAfinidad) {
    contResultados = contResultados + 1;
    if (document.getElementById('ctl00_ctl00_CPH1_CPHContenido_txtRecursosSeleccionados') != null) {
        document.getElementById('ctl00_ctl00_CPH1_CPHContenido_txtRecursosSeleccionados').value = '';
        document.getElementById('ctl00_ctl00_CPH1_CPHContenido_lblErrorMisRecursos').style.display = 'none';
    }
    var servicio = new WS($('input.inpt_UrlServicioResultados').val(), WSDataType.jsonp);

    var paramAdicional = parametros_adiccionales;

    /*
    if ($('li.mapView').attr('class') == "mapView activeView") {
        paramAdicional += 'busquedaTipoMapa=true';
    }*/
    /*
    if ($('.chartView').attr('class') == "chartView activeView") {
        paramAdicional = 'busquedaTipoChart=' + chartActivo + '|' + paramAdicional;
    }*/

    if ($('li.mapView').hasClass('activeView')) {
        paramAdicional += 'busquedaTipoMapa=true';
    }


    if ($('.chartView').hasClass('activeView')) {
        paramAdicional = 'busquedaTipoChart=' + chartActivo + '|' + paramAdicional;
    }

    var metodo = 'CargarResultados';
    var params = {};

    if (bool_usarMasterParaLectura) {
        if (finUsoMaster == null) {
            finUsoMaster = new Date();
            finUsoMaster.setMinutes(finUsoMaster.getMinutes() + 1);
        }
        else {
            var fechaActual = new Date();
            if (fechaActual > finUsoMaster) {
                bool_usarMasterParaLectura = false;
                finUsoMaster = null;
            }
        }
    }

    params['pUsarMasterParaLectura'] = bool_usarMasterParaLectura;
    params['pProyectoID'] = $('input.inpt_proyID').val();
    params['pEsUsuarioInvitado'] = $('input.inpt_bool_esUsuarioInvitado').val() == 'True';

    if (typeof (identOrg) != 'undefined') {
	 
        params['pIdentidadID'] = identOrg;
    }
    else {
	 
        params['pIdentidadID'] = $('input.inpt_identidadID').val();
    }
    params['pParametros'] = '' + pFiltros.replace('#', '');
    params['pLanguageCode'] = $('input.inpt_Idioma').val();
    params['pPrimeraCarga'] = pPrimeraCarga == "True";
    params['pAdministradorVeTodasPersonas'] = adminVePersonas == "True";
    params['pTipoBusqueda'] = tipoBusqeda;
    params['pNumeroParteResultados'] = pNumeroResultados;
    params['pGrafo'] = grafo;
    params['pFiltroContexto'] = filtroContexto;
    params['pParametros_adiccionales'] = paramAdicional;
    params['cont'] = contResultados;
    params['tokenAfinidad'] = pTokenAfinidad;

    $.post(obtenerUrl($('input.inpt_UrlServicioResultados').val()) + "/" + metodo, params, function (response) {
        if (params['cont'] == contResultados) {
            var data = response
            if (response.Value != null) {
                data = response.Value;
            }

            var vistaMapa = (params['pParametros_adiccionales'].indexOf('busquedaTipoMapa=true') != -1);
            var vistaChart = (params['pParametros_adiccionales'].indexOf('busquedaTipoChart=') != -1);

            var descripcion = data;

            var funcionJS = '';
            if (descripcion.indexOf('###ejecutarFuncion###') != -1) {
                var funcionJS = descripcion.substring(descripcion.indexOf('###ejecutarFuncion###') + '###ejecutarFuncion###'.length);
                funcionJS = funcionJS.substring(0, funcionJS.indexOf('###ejecutarFuncion###'));

                descripcion = descripcion.replace('###ejecutarFuncion###' + funcionJS + '###ejecutarFuncion###', '');
            }

            if (tipoBusqeda == 12) {
                var panelListado = $(pPanelID).parent();
                panelListado.html('<div id="' + pPanelID.replace('#', '') + '"></div><div id="' + panResultados.replace('#', '') + '"></div>')

                var panel = $(pPanelID);
                panel.css('display', 'none');
                panel.html(descripcion);
                panelListado.append(panel.find('.resource-list').html())
                panel.find('.resource-list').html('');
            } else if (!vistaMapa && !vistaChart) {
                $(pPanelID).append(descripcion);
            }
            else {
                var arraydatos = descripcion.split('|||');

                if ($('#panAuxMapa').length == 0) {
                    $(pPanelID).parent().html($(pPanelID).parent().html() + '<div id="panAuxMapa" style="display:none;"></div>');
                }

                if (vistaMapa) {
                    $('#panAuxMapa').html('<div id="numResultadosRemover">' + arraydatos[0] + '</div>');
                }

                if (vistaChart) {
                    datosChartActivo = arraydatos;
                    $(pPanelID).html('<div id="divContChart"></div>');
                    eval(jsChartActivo);
                }
                else {
                    utilMapas.MontarMapaResultados(pPanelID, arraydatos);
                }
            }
            FinalizarMontarResultados(paramAdicional, funcionJS, pNumeroResultados, pPanelID);
        }
        if (MontarResultadosScroll.pagActual != null) {
            MontarResultadosScroll.pagActual = 1;
            MontarResultadosScroll.cargarScroll();
        }
    }, "json");
}


function FiltrarPorFacetasGenerico(filtro) {
    filtro = filtro.replace(/&/g, '|');

    if (typeof (filtroDePag) != 'undefined' && filtroDePag != '') {
        if (filtro != '') {
            filtro = filtroDePag + '|' + filtro;
        }
        else {
            filtro = filtroDePag;
        }
    }
    //Si hay orden por relevancia pero no hay filtro search, quito el orden para que salga el orden por defecto
    //    if(QuitarOrdenReleavanciaSinSearch(filtro))
    //    {
    //        return false;
    //    }
    filtrosPeticionActual = filtro;

    var rdf = false;
    if (filtro.indexOf('?rdf') != -1 && ((filtro.indexOf('?rdf') + 4) == filtro.length)) {
        filtro = filtro.substring(0, filtro.length - 4);
        document.location.hash = document.location.hash.substring(0, document.location.hash.length - 4);
        rdf = true;
    }

    enlazarJavascriptFacetas = true;

    var arg = filtro;

    /*
    var vistaMapa = ($('li.mapView').attr('class') == "mapView activeView");
    var vistaChart = ($('.chartView').attr('class') == "chartView activeView");
    */

    var vistaMapa = $('li.mapView').hasClass('activeView');
    var vistaChart = $('.chartView').hasClass('activeView');

    if (!primeraCargaDeFacetas && !vistaMapa) {
        MostrarUpdateProgress();
    }

    var parametrosFacetas = 'ObtenerResultados';

    var gruposPorTipo = $('#facetedSearch.facetedSearch .listadoAgrupado ').length>0;

    if (cargarFacetas && !gruposPorTipo) {
        if (typeof panFacetas != "undefined" && panFacetas != "" && $('#' + panFacetas).length > 0 && !primeraCargaDeFacetas && !gruposPorTipo) {
            $('#' + panFacetas).html('')
        }
        if (numResultadosBusq != "" && $('#' + numResultadosBusq).length > 0 && !primeraCargaDeFacetas) {
            $('#' + numResultadosBusq).html('')
            $('#' + numResultadosBusq).css('display', 'none');
        }
        if (!clickEnFaceta && panFiltrosPulgarcito != "" && $('#' + panFiltrosPulgarcito).length > 0 && !primeraCargaDeFacetas) {
            $('#' + panFiltrosPulgarcito).html('')
        }
    }

    if (!vistaMapa) {
        SubirPagina();
    }

    if (typeof idNavegadorBusqueda != "undefined") {
        $('#' + idNavegadorBusqueda).html('');
        $('#' + idNavegadorBusqueda).css('display', 'none');
    }

    if (!vistaMapa && !primeraCargaDeFacetas) {
        // Vaciar el contenido actual de resultados - Nuevo Front
        // document.getElementById(updResultados).innerHTML = '';
        $(`#${updResultados}`).html('');
        $('#' + updResultados).attr('style', '');
    }

    clickEnFaceta = false;
    var primeraCarga = false;

    if (filtro.length > 1 || document.location.href.indexOf('/tag/') > -1 || (filtroContexto != null && filtroContexto != '')) {
        parametrosFacetas = 'AgregarFacetas|' + arg;
        var parametrosResultados = 'ObtenerResultados|' + arg;
        if (!cargarFacetas) {
            var parametrosResultados = 'ObtenerResultadosSinFacetas|' + arg;
        }
        //cargarFacetas
        var displayNone = '';
        document.getElementById('query').value = parametrosFacetas;
        if (HayFiltrosActivos(filtro) && (tipoBusqeda != 12 || filtro.indexOf("=") != -1)) {
            $('#' + divFiltros).css('display', '');
            $('#' + divFiltros).css('padding-top', '0px !important');
            //$('#' + divFiltros).css('margin-top', '10px');
        }
        var pLimpiarFilt = $('p', $('#' + divFiltros)[0]);

        if (pLimpiarFilt != null && pLimpiarFilt.length > 0) {
            if (!(filtro.length > 1 || document.location.href.indexOf('/tag/') > -1)) {
                pLimpiarFilt[0].style.display = 'none';
            }
            else {
                pLimpiarFilt[0].style.display = '';
            }
        }
    }
    else {
        primeraCarga = true;
        $('#' + divFiltros).css('display', 'none');
        $('#' + divFiltros).css('padding-top', '0px !important');
        //$('#' + divFiltros).css('margin-top', '10px');
    }

    if (rdf) {
        eval(document.getElementById('rdfHack').href);
    }

    var tokenAfinidad = guidGenerator();

    if (vistaMapa || !primeraCargaDeFacetas) {
        MontarResultados(filtro, primeraCarga, 1, '#' + panResultados, tokenAfinidad);
    }

    if (panFacetas != "" && (cargarFacetas || document.getElementById(panFacetas).innerHTML == '')) {
        var inicioFacetas = 1;

        MontarFacetas(filtro, primeraCarga, inicioFacetas, '#' + panFacetas, null, tokenAfinidad);
    }

    primeraCargaDeFacetas = false;
    cargarFacetas = true;

    var txtBusquedaInt = $('.aaCabecera .searchGroup .text')
    var textoSearch = 'search=';
    if ((filtro.indexOf(textoSearch) > -1) && txtBusquedaInt.length > 0) {
        var filtroSearch = filtro.substring(filtro.indexOf(textoSearch) + textoSearch.length);
        if (filtroSearch.indexOf('|') > -1) {
            filtroSearch = filtroSearch.substring(0, filtroSearch.indexOf('|'));
        }

        txtBusquedaInt.val(decodeURIComponent(filtroSearch));
        txtBusquedaInt.blur();
    }
    CambiarOrden(filtro);
    return false;
}


function AgregarFaceta(faceta,eliminarFiltroAnterior=false) {
    faceta = faceta.replace(/%22/g, '"');
    estamosFiltrando = true;
    //var filtros = ObtenerHash2().replace(/%20/g, ' ');
    var filtros = ObtenerHash2();
    filtros = replaceAll(filtros, '%26', '---AMPERSAND---');
    filtros = decodeURIComponent(filtros);
    filtros = replaceAll(filtros, '---AMPERSAND---', '%26');
	


    var esFacetaTesSem = false;

    if (faceta.indexOf('|TesSem') != -1) {
        esFacetaTesSem = true;
        faceta = faceta.replace('|TesSem', '');
    }

    var eliminarFacetasDeGrupo = '';
    if (faceta.indexOf("rdf:type=") != -1 && filtros.indexOf(faceta) != -1) {
        //Si faceta es RDF:type y filtros la contiene, hay que eliminar las las que empiezen por el tipo+;
        eliminarFacetasDeGrupo = faceta.substring(faceta.indexOf("rdf:type=") + 9) + ";";
    }

    var filtrosArray = filtros.split('&');
    filtros = '';

    var tempNamesPace = '';
    if (faceta.indexOf('|replace') != -1) {
        tempNamesPace = faceta.substring(0, faceta.indexOf('='));
        faceta = faceta.replace('|replace', '');
    }

    var facetaDecode = decodeURIComponent(faceta);
    var contieneFiltro = false;

    for (var i = 0; i < filtrosArray.length; i++) {
        if (filtrosArray[i] != '' && filtrosArray[i].indexOf('pagina=') == -1) {
            if (eliminarFacetasDeGrupo == '' || filtrosArray[i].indexOf(eliminarFacetasDeGrupo) == -1) {
                if (tempNamesPace == '' || (tempNamesPace != '' && filtrosArray[i].indexOf(tempNamesPace) == -1)) {
                    filtros += filtrosArray[i] + '&';
                }
            }
        }

        if (filtrosArray[i] != '' && (filtrosArray[i] == faceta || filtrosArray[i] == facetaDecode)) {
            contieneFiltro = true;
        }
    }

    if (filtros != '') {
        filtros = filtros.substring(0, filtros.length - 1);
    }
    if (faceta.indexOf('search=') == 0) {
	 
        $('h1 span#filtroInicio').remove();
    }

    if (typeof (filtroDePag) != 'undefined' && filtroDePag.indexOf(faceta) != -1) {
        var url = document.location.href;
        //var filtros = '';

        if (filtros != '') {
            filtros = '?' + filtros.replace(/ /g, '%20');
            //filtros = '?' + encodeURIComponent(filtros);
        }

        if (url.indexOf('?') != -1) {
            //filtros = url.substring(url.indexOf('?'));
            url = url.substring(0, url.indexOf('?'));
        }

        if (url.substring(url.length - 1) == '/') {
            url = url.substring(0, (url.length - 1));
        }

        //Quito los dos ultimos trozos:
        url = url.substring(0, url.lastIndexOf('/'));
        url = url.substring(0, url.lastIndexOf('/'));

        if (filtroDePag.indexOf('skos:ConceptID') != -1) {
            var busAvazConCat = false;

            if (typeof (textoCategoria) != 'undefined') {
                //busAvazConCat = (url.indexOf('/' + textoCategoria) == (url.length - textoCategoria.length - 1));
                if (url.indexOf(textoComunidad + '/') != -1) {
                    var trozosUrl = url.substring(url.indexOf(textoComunidad + '/')).split('/');
                    busAvazConCat = (trozosUrl[2] == textoCategoria);
                }
            }

            url = url.substring(0, url.lastIndexOf('/'));

            if (busAvazConCat) {
                url += '/' + textoBusqAvaz;
            }
        }


        MostrarUpdateProgress();

        document.location = url + filtros;
        return;
    }
    else if (!contieneFiltro) {
		if (eliminarFiltroAnterior)
		{
			filtros='';
			var filtroEliminar=faceta.substring(0,faceta.indexOf('=')+1);			
			for (var i = 0; i < filtrosArray.length; i++) {				
				if (filtrosArray[i].indexOf(filtroEliminar)==-1)  {
					filtros += filtrosArray[i] + '&';
				}
			}
		}	
				
		//Si no existe el filtro, lo a?adimos
		if (filtros.length > 0) { filtros += '&'; }
		filtros += faceta;

		if (typeof searchAnalitics != 'undefined') {
			searchAnalitics.facetsSearchAdd(faceta);
		}
    }
    else {
        filtros = '';

        for (var i = 0; i < filtrosArray.length; i++) {
			var filtroEliminar="";
			if (eliminarFiltroAnterior)
			{
				filtroEliminar=faceta.substring(0,faceta.indexOf('=')+1);
			}
            if (filtrosArray[i] != '' && filtrosArray[i] != faceta && filtrosArray[i] != facetaDecode && (!eliminarFiltroAnterior || filtrosArray[i].indexOf(filtroEliminar)==-1) ) {
                filtros += filtrosArray[i] + '&';
            }
        }

        if (filtros != '') {
            filtros = filtros.substring(0, filtros.length - 1);
        }

        if (!esFacetaTesSem && typeof searchAnalitics != 'undefined') {
            searchAnalitics.facetsSearchRemove(faceta);
        }
    }

    filtros = filtros.replace(/&&/g, '&');
    if (filtros.indexOf('&') == 0) {
        filtros = filtros.substr(1, filtros.length);
    }
    if (filtros[filtros.length - 1] == '&') {
        filtros = filtros.substr(0, filtros.length - 1);
    }

    filtros = filtros.replace(/\\\'/g, '\'');
    filtros = filtros.replace('|', '%7C');

    history.pushState('', 'New URL: ' + filtros, '?' + filtros);
    FiltrarPorFacetas(ObtenerHash2());
    EscribirUrlForm(filtros);
}


/**
 * Clase metabuscador para dar funcionalidad al mismo 
*/
var metabuscador = {
    /**
     * Función que inicializa el metaBuscador 
    */
    init: function () {
        this.config();
        // this.loadLastSearchs();
        // Inicializar el mostrado de búsquedas de metaBuscador
        this.drawSearchsFromLocalStorage();
        this.comportamiento();
        return;
    },
    /* loadLastSearchs: function () {
    	var that = this;
    	var baseUlr = "https://localhost:44321/";
    	var url = baseUlr + "Hercules/GetLastSearchs";
    	$.get(url, function (data) {
			that.printDataIntoLastSearchs(data);
		});
        return;
    },*/

    /**
     * Función que determina los elementos en la clase 
    */
    config: function () {
        this.body = body;
        this.header = this.body.find('#header');
        // this.metabuscadorTrigger = this.header.find('.col-buscador');
        this.metabuscadorTrigger = this.body.find('.col-buscador');
        this.metabuscador = this.body.find('#menuLateralMetabuscador');
        this.input = $('#txtBusquedaPrincipal');
        this.resultadosMetabuscador = this.body.find('#resultadosMetabuscador');
        this.verMasEcosistema = this.body.find('#verMasEcosistema');
        this.numMaxSearchs = 10;
        // Panel sin resultados por elementos no encontrados
        this.panelSinResultados = $(`#sinResultadosMetabuscador`);
        //this.timeWaitingForUserToType = 750; // Esperar 0.75 segundos a si el usuario ha dejado de escribir para iniciar búsqueda
		this.timeWaitingForUserToType = 150; // Esperar 0.75 segundos a si el usuario ha dejado de escribir para iniciar búsqueda
        //this.ignoreKeysToBuscador = [37, 38, 39, 40, 46, 8, 32, 91, 17, 18, 20, 36, 18, 27];
		this.ignoreKeysToBuscador = [37, 38, 39, 40, 91, 17, 18, 20, 36, 18, 27];
        // Palabra clave introducida en el metaBuscador para mostrar en el panel de resultados no encontrados
        this.idPalabraBuscadaMetabuscador = `metabuscadorBusqueda`;
        this.sugerenciasMetabuscadorItems = this.body.find('#sugerenciasMetabuscador ul');
        this.KEY_LOCAL_SEARCHS = "metasearchs";
        // Listado de los tipos de resultados existentes
        this.typeSearch = {
        	"persona": {
        		"icon": "icono-persona",
        		"section": "persons",
        		"searchParm": "searcherPersons"
        	},
        	"group": {
        		"icon": "icono-group",
        		"section": "groups",
        		"searchParm": "searcherGroups"
        	},
        	"project": {
        		"icon": "icono-work",
        		"section": "projects",
        		"searchParm": "searcherProjects"
        	},
        	"publicacion": {
        		"icon": "icono-recurso",
        		"section": "documents",
        		"searchParm": "searcherPublications"
        	},
        	"researchObject": {
        		"icon": "icono-recurso",
        		"section": "researchObjects",
        		"searchParm": "searcherPublications"
        	},
			"offer": {
        		"icon": "icono-offer",
        		"section": "offers",
        		"searchParm": "searcherOffers"
        	},
        };
        this.keyInput = "";
        return;
    },
    /* printDataIntoLastSearchs: function (data) {
    	var list = $('#sugerenciasMetabuscador ul');
    	list.html('');
    	var items = data.map(e => {
    		return '<li class="reciente con-icono-before icono-busqueda">\
                <a href="javascript: void(0)" data-search="\'' + e + '\'">' + e + '</a>\
            </li>';
    	});
    	list.append(items);
    	items.on('click', function(event) {
    		event.preventDefault();
    		var searchTxt = $(this).data("search");
    		that.cargarRecursos(searchTxt);
    	});
    }, */

    /**
     * Función principal que determina como funcionará el input y que lanzará los diferentes procesos del objeto 
    */
    comportamiento: function () {
        var that = this;

        that.metabuscadorTrigger.on('click', function (e) {
            that.input.focus();
        });

        // Clear the searchs
        that.input.on('click', function (e) {
        	that.keyInput = that.input.val();
			if (that.keyInput.length <= 1) 
			{
                that.ocultarResultados();
			}
        });

        that.input.on('keyup', function (e) {
            that.keyInput = that.input.val();

			if (that.keyInput.length <= 1) 
			{
                that.ocultarResultados();
			}
			else {

        		if (that.validarKeyPulsada(e) == true) {

	                clearTimeout(that.timer);
	                that.timer = setTimeout(function () {
                        //that.ocultarResultados();
                        // Ocultar panel sin resultados por posible busqueda anterior sin resultados
                        //that.mostrarPanelSinResultados(false);
                        that.cargarResultados();
                        // Guardar búsqueda en localStorage
                        that.saveSearchInLocalStorage(that.keyInput);
	                }, that.timeWaitingForUserToType);
				}           	
            }
        });

        // Botón para cerrar resultados de la Home
        /* this.btnCloseMetabuscador.on("click", function () {
            that.mostrarOculto();
        }); */

        that.input.keydown(function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                that.saveSearchInLocalStorage(that.keyInput);
                that.input.keyup();
                return false;
            }
        });

        // Click en cada item de histórico de metabuscador              
        //this.metabuscador.on('click', this.sugerenciasMetabuscadorItems.children(), function (event) {
        this.sugerenciasMetabuscadorItems.on('click', function (event) {
            // Establecer como búsqueda a realizar
            const search = event.target.text;
            that.input.val(search);
            that.input.trigger("keyup");
        });


        return;
    },

    /**
     * Ocultar los resultados 
    */
    ocultarResultados: function () {
        this.metabuscador.removeClass('mostrarResultados');
    },

    /**
     * Inicia el proceso de cargar los resultados y pintarlos 
    */
    cargarResultados: function () {
        var that = this;
        this.metabuscador.addClass('mostrarResultados');
        // simular la carga de cada sección
        that.cargarRecursos();
    },

    /**
     * Muestra la barra de cargando 
     * @param {any} loader_div: Objeto jquery con el div sobre el que pintar la progressBar
     * @param {int} time: Tiempo límite que debe durar la progressBar
    */
    showLoader: function (loader_div, time) {
        var loaderBar = loader_div.progressBarTimer({
            autostart: false,
            timeLimit: time,
            warningThreshold: 0,
            baseStyle: '',
            warningStyle: '',
            completeStyle: '',
            smooth: true,
            striped: false,
            animated: false,
            height: 12
        });
        return loaderBar;

    },
    /**
     * Inicia la carga de datos 
    */
    cargar: function () {
        var that = this;

        // Inicia la progressBar
        var loader_container = $('#loader-recursos-wrap');
        loader_container.find('.progress-bar').remove();

        var loader_div = $('<div id="#loader-recursos" class="progress-bar"></div>');
        loader_container.append(loader_div);
        loader_container.show();

        var loader = this.showLoader(loader_div, 1);
        loader.start();


        // Get the url
        var uri = that.resultadosMetabuscador.data('url');

        // Compone la url para la llamada
        var url = new URL(url_servicio_externo +  uri);
		
        url.searchParams.set('stringSearch', this.keyInput);
        url.searchParams.set('lang', lang);

        // Llama al servicio y devuelve una promesa
        return new Promise((resolve, reject) => {
            $.get(url.toString(), function (data) {
            	loader.stop();
                loader_div.remove();
                loader_container.hide();
                resolve(data);
            });
        });
    },

    /**
     * Método que inicia la carga de los recursos, se llama a 'cargar' y luego llama al método pintarItems para pintar el resultado 
     * @param {string} searchTxt: Texto alternativo de búsqueda
    */
    cargarRecursos: function (searchTxt = "") {
        var that = this;

        // Oculta la sección de resultados
        //var bloque = that.resultadosMetabuscador.find('.bloque');
        //bloque.hide();

        // Rellena los datos por si se les pasara por parámetro
        if (searchTxt != "") {
        	that.keyInput = searchTxt;
        }

        // Carga los datos
        var procesoCarga = this.cargar();
        procesoCarga.then(function (data) {

        	// Comprueba que hay resultados
        	var totalItems = 0;
        	Object.keys(data).forEach(e => totalItems += data[e].value.length);

        	// Si hay resultados, pinta los items
        	if (totalItems > 0) {
        		that.mostrarPanelSinResultados(false);        		
        	} else {
        		that.mostrarPanelSinResultados(true);
        	}
			that.pintarItems(data);
        });
    },

	/**
     * Método que pintar el resultado de la búsqueda
     * @param {Object} data: Objeto con los datosobtenidos de la llamada ajax
    */
    pintarItems: function (data) {

    	var that = this;

    	Object.keys(data).forEach(ndx => {

    		// Comprueba que hay un bloque de cada tipo devuelto en el objeto
    		if (that.typeSearch[ndx]) {

    			// Busca en el dom la sección
    			var currenTbloque = that.resultadosMetabuscador.find('.bloque.' + that.typeSearch[ndx].section);
    			// Selecciona la lista del bloque correspondiente
    			var listCurrenTbloque = currenTbloque.find('ul');
    			var urlComunidad = currenTbloque.data('urlcomunidad');
    			var textoBloque = currenTbloque.find('p.title');

    			// Elimina los items
    			listCurrenTbloque.children().remove();

    			// Comprueba que tiene resultados los datos para este elemento
    			if (data[ndx].value.length > 0) {

    				// Pinta los elementos dentro de la lista
    				var result = data[ndx].value.map(item => $('<li class="con-icono-before ' + that.typeSearch[ndx].icon + '" data-id="'+item.id+'">\
                        	<a href="'+item.url+'">'+item.title+'</a>\
                    	</li>')
    				);
    				result.forEach(item => listCurrenTbloque.append(item));

    				if (data[ndx].key) {
    					listCurrenTbloque.append($('<li class="con-icono-after ver-mas-icono ver-mas">\
                            <a href="'+urlComunidad+'?' + that.typeSearch[ndx].searchParm + '='+that.keyInput+'">Ver ' + that.keyInput + ' en ' + textoBloque.text().toLowerCase() + '</a>\
                        </li>'));
    				}

    				// Muestra el bloque
    				currenTbloque.show();
    			}else
				{
					//Oculta el bloque
					currenTbloque.hide();
				}
    		}
    	})
    },

	/**
     * Método que pintar el resultado de la búsqueda
    */
    linkVerEnElEcosistema: function () {
        var that = this;
        that.verMasEcosistema.hide();
        setTimeout(() => {
            that.verMasEcosistema.show()
        }, 5000);
    },

    /**
     * Comprobará la tecla pulsada, y si no se encuentra entre las excluidas, dará lo introducido por válido devolviendo true
     * Si se pulsa una tecla de las excluidas, devolverá false y por lo tanto el metabuscador no debería iniciarse
     * @param {any} event: Evento o tecla pulsada en el teclado
     */
    validarKeyPulsada: function (event) {
        const keyPressed = this.ignoreKeysToBuscador.find(key => key == event.keyCode);
        if (keyPressed) {
            return false
        }
        return true;
    },

	/**
     * Método que muestra el panel (o no) 'sin resultados' para cuando no haya resultados en la llamada
     * @param {bool} mostrarPanel: Determina si mostrarlo o no
     */
    mostrarPanelSinResultados: function(mostrarPanel){

        // Vaciar posibles palabras anteriores
        $(`#${this.idPalabraBuscadaMetabuscador}`).text('');
        // Establecer la palabra buscada para ser mostrada en el panel
        const cadenaBuscada = this.input.val();
        // Panel de búsqueda no encontrada
        const panelSinResultadosContent = `
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="errorPlantilla">
                            <h3 style="font-size:18px">Sin resultados</h3>
                            <div class="detallesError"><p class="text-muted">Lo sentimos pero no se ha encontrado ningún resultado con <span id="metabuscadorBusqueda" class="font-weight-bold"></span></p></div>
                        </div>
                    </div>
                </div>
            </div>
        `;
        
        if (mostrarPanel == true) {
            this.panelSinResultados.removeClass("d-none");
            this.panelSinResultados.html(panelSinResultadosContent);
            // Añadir la palabra buscada al panel de no encontrado
            $(`#${this.idPalabraBuscadaMetabuscador}`).text(cadenaBuscada);
        } else {
            this.panelSinResultados.addClass("d-none");            
        }    
    },

    /**
     * Guardar en localStorage una búsqueda realizada. Eliminará el más antiguo si se ha llegado al número de items máximo guardado.
     * @param {string} search
     */
    saveSearchInLocalStorage: function (search) {
        // Comprobar si la búsqueda reciente ya existe en el histórico de búsquedas
        let searchRepeated = false;

        try {
            // Obtener las búsquedas actuales que hay en el localStorage. Si no hay devolverá un array vacío
            const localSearchs = JSON.parse(
                localStorage.getItem(this.KEY_LOCAL_SEARCHS) || "[]"
            );

            // Comprobar que no existe la búsqueda actual en localStorage (como texto parcial)
            localSearchs.forEach((item) => {
                if (item.search.indexOf(search) == 0) {
                    searchRepeated = true;
                }
            });

            if (searchRepeated) {
                return;
            }
			
			// Comprobamos si es la continuación de alguna búsqueda actual, en caso de que lo sea eliminamos esa búsqueda
			let eliminar=[];
			let index=0;
			localSearchs.forEach((item) => {				
                if (search.indexOf(item.search) == 0) {
					eliminar.push(index);
                }else
				{
					index++;
				}
            });
			
			
			eliminar.forEach((item) => {				
                localSearchs.splice(item, 1); 
            });
			

            // Si hay un máximo de X resultados en localStorage, eliminar el Último
            if (localSearchs.length >= this.numMaxSearchs) {
                localSearchs.shift();
            }

            // Crear el nuevo item "search" y guardarlo en localStorage
            const searchObject = { "search": search };
            localSearchs.push(searchObject);
            localStorage.setItem(this.KEY_LOCAL_SEARCHS, JSON.stringify(localSearchs));
        } catch (error) {
            console.log("Error saving metaSearch");
        }

        this.drawSearchsFromLocalStorage();
    },

    /**
    * Creará o dibujará las metabusquedas del localStorage en el HTML para que sean mostradas correctamente
    */
    drawSearchsFromLocalStorage: function () {
        // Listas de las búsquedas en localStorage
        let searchListItems = "";

        // Obtener las búsquedas actuales que hay en el localStorage. Si no hay devolverá array vacío
        const localSearchs = JSON.parse(localStorage.getItem(this.KEY_LOCAL_SEARCHS) || "[]");
        // Si no hay resultados ... no hacer nada
        if (localSearchs.length == 0) {
            return;
        }

        var list = $('#sugerenciasMetabuscador ul');

        // Eliminar posibles items
        this.sugerenciasMetabuscadorItems.children().remove();

        // Construir cada itemList con las búsquedas almacenadas en localStorage
        localSearchs.reverse().forEach((item) => {
            searchListItems += `<li class="reciente con-icono-before icono-busqueda">
                                    <a href="javascript: void(0);">${item.search}</a>
                                </li>`;
        });

        // Añadir los items para el metabuscador
        this.sugerenciasMetabuscadorItems.append(searchListItems);
    },

    /**
     * Eliminar las búsquedas históricas al cerrar sesión
     * */
    removeSearchsFromLocalStorage: function () {

        try {
            localStorage.removeItem(this.KEY_LOCAL_SEARCHS);
        } catch(e) {
            console.log("Error removing localSearchs from localStorage");
        }        
    }
};

function setCacheWithExpiry(key, value, ttl) {
	const now = new Date();
	const item = {
		value: value,
		expiry: now.getTime() + ttl
	}
	localStorage.setItem(key, JSON.stringify(item));
}

function getCacheWithExpiry(key) {
	const itemStr = localStorage.getItem(key);
	if (!itemStr) {
		return null;
	}
	const item = JSON.parse(itemStr);
	const now = new Date();
	if (now.getTime() > item.expiry) {
		localStorage.removeItem(key);
		return null;
	}
	return item.value;
}

function GetFuentesExternas(pIdUsuario) {
    var url = url_servicio_externo + "FuentesExternas/InsertToQueue";
    var arg = {};
    arg.pUserId = pIdUsuario;
    mostrarNotificacion("info", "Obteniendo datos de fuentes externas en proceso. Tardará unos minutos.");
    $.get(url, arg, function (data) {
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.log("Error al obtener las fuentes externas");
        var fecha = jqXHR.responseText.split("-");
        if (fecha.length == 3) {
            var horas_restantes = (24 - (new Date().getHours()));

            mostrarNotificacion("error", "Ya has solicitado la actualización de fuentes externas hoy. Por favor, espera " + horas_restantes + " horas para volver a solicitarla.");

        } else {
            console.log(jqXHR);
            mostrarNotificacion("error", "Error al obtener las fuentes externas");
        }

    }).success(function (data) {
        console.log("Fuentes externas obtenidas");
        
    }
    );
}

function PedirFuentesExternas() {
    GetFuentesExternas($('.inpt_usuarioID').attr('value'));
    menusLateralesManagement.init();
    //mostrarNotificacion("success", "Obteniendo datos de fuentes externas en proceso. Tardará unos minutos.");
}
function GetSexenios(comite, periodo, perfil, subcomite, pIdUsuario) {
    var url = url_servicio_editorcv + "Sexenios/ConseguirSexenios";
    var formData = new FormData();
    formData.append('comite', comite);
    formData.append('periodo', periodo);
    formData.append('perfil_tecnologico', perfil);
    formData.append('subcomite', subcomite);
    formData.append('idInvestigador', pIdUsuario);
    mostrarNotificacion("info", "Obteniendo datos de sexenios en proceso. Tardará unos minutos.");
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        cache: false,
        processData: false,
        enctype: 'multipart/form-data',
        contentType: false,
        success: function (response) {
            console.log("Sexenios obtenidos");
        },
        fail: function (jqXHR, textStatus, errorThrown) {
            console.log("Error al obtener los sexenios");
            var fecha = jqXHR.responseText.split("-");
            if (fecha.length == 3) {
                var horas_restantes = (24 - (new Date().getHours()));
            } else {
                console.log(jqXHR);
                mostrarNotificacion("error", "Error al obtener los sexenios");
            }
        },
        error: function (response) {
            mostrarNotificacion("error", "Error al obtener los sexenios");
        }
    });
    $('#modal-sexenios').modal('hide');
    $("#modal-sexenios #idSelectorComite").val("2").change();
    $("#modal-sexenios #labelPeriodo").val("");
    $("#modal-sexenios #yesRadio").prop("checked", false);
    $("#modal-sexenios #idSelectorSubcomite").val("1").change();
}

function PedirSexenio() {
    var comite = $('#idSelectorComite option:selected').val();
    var periodo = $('#labelPeriodo').val();
    var perfil = $('#idSelectorComite option:selected').val() == "8" ? $("#yesRadio").prop("checked") : "";
    var subcomite = $('#idSelectorComite option:selected').val() == "9" ? $('#idSelectorSubcomite option:selected').val() : "";
    var investigador = $('.inpt_usuarioID').attr('value');
    GetSexenios(comite, periodo, perfil, subcomite, investigador);
    menusLateralesManagement.init();
}

function GetAcreditaciones(comision, tipo, categoria, pIdUsuario) {
    var url = url_servicio_editorcv + "Acreditaciones/ConseguirAcreditaciones";
    var formData = new FormData();
    formData.append('comision', comision);
    formData.append('tipo_acreditacion', tipo);
    formData.append('categoria_acreditacion', categoria);
    formData.append('idInvestigador', pIdUsuario);
    mostrarNotificacion("info", "Obteniendo datos de las acreditaciones en proceso. Tardará unos minutos.");
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        cache: false,
        processData: false,
        enctype: 'multipart/form-data',
        contentType: false,
        success: function (response) {
            console.log("Acreditaciones obtenidas");
        },
        fail: function (jqXHR, textStatus, errorThrown) {
            console.log("Error al obtener las acreditaciones");
            var fecha = jqXHR.responseText.split("-");
            if (fecha.length == 3) {
                var horas_restantes = (24 - (new Date().getHours()));
            } else {
                console.log(jqXHR);
                mostrarNotificacion("error", "Error al obtener las acreditaciones");
            }
        },
        error: function (response) {
            mostrarNotificacion("error", "Error al obtener los acreditaciones");
        }
    });
    $('#modal-acreditaciones').modal('hide');
    $("#modal-acreditaciones #idSelectorComision").val("2").change();
    $("#modal-acreditaciones #idSelectorComision").val("1").change();
    $("#modal-acreditaciones #idSelectorComision").val("1").change();
}

function PedirAcreditacion() {
    var comision = $('#idSelectorComision option:selected').val();
    var tipo = $('#idSelectorTipoAcreditacion option:selected').val();
    var categoria = $('#idSelectorComision option:selected').val() == "21" ? $('#idSelectorCategoria option:selected').val() : "";
    var investigador = $('.inpt_usuarioID').attr('value');
    GetAcreditaciones(comision, tipo, categoria, investigador);
    menusLateralesManagement.init();
}
var comportamientoMenuLateral = {
    init: function () {
        $("#noRadio").prop("checked", true);
        $("#btnPedirSexenio").click(function () {
            PedirSexenio();
        });
        $("#btnPedirAcreditaciones").click(function () {
            PedirAcreditacion();
        });
        $("#idSelectorComite").on("change", function () {
            $("#perfilTecnologicoForm").hide();
            $("#subcomiteForm").hide();
            if ($('#idSelectorComite option:selected').val() == "8") {
                $("#perfilTecnologicoForm").show();
            } else if ($('#idSelectorComite option:selected').val() == "9") {
                $("#subcomiteForm").show();
            }
        });
        $("#idSelectorComision").on("change", function () {
            $("#categoriaForm").hide();
            if ($('#idSelectorComision option:selected').val() == "21") {
                $("#categoriaForm").show();
            }
        });
    }
}
menusLateralesManagement.montarMenuLateralMetabuscador= function () {
	var that = this;
	var container = that.main.find('.container');

	if (!$('#menuLateralMetabuscador').length > 0) return;
	var isHome=$('body').hasClass('homeComunidad');
	var width=740;
	if(isHome)
	{
		width=$('.home-ma .buscador-container').width();
	}
	$('#menuLateralMetabuscador').slideReveal({
		trigger: $('#txtBusquedaPrincipal'),
		width: width,
		overlay: true,
		position: 'left',
		push: false,
		autoEscape: false,
		show: function (slider) {
			that.body.addClass('metabuscador-abierto');

			if(isHome)
			{
				setTimeout(() => {
					var position=$('#txtBusquedaPrincipal').offset().left;
					slider.css('left', position);
					$('.home-ma .buscador-container').css('z-index', 1049);
					slider.css('cssText', slider.attr("style") + ";top:"+($('#txtBusquedaPrincipal').offset().top-$(window).scrollTop()+50)+"px !important");	
				});
				that.timerId = setInterval(() => {						
					var position=$('#txtBusquedaPrincipal').offset().left;
					slider.css('left', position);
					$('.home-ma .buscador-container').css('z-index', 1049);$('#txtBusquedaPrincipal').width()
					slider.css('cssText', slider.attr("style") + ";top:"+($('#txtBusquedaPrincipal').offset().top-$(window).scrollTop()+50)+"px !important");	
				},100);
			}else
			{
				setTimeout(() => {
					var position;
					if (container.offset().left < 177) {
						position = that.buscador.offset().left;
					} else {
						position = container.offset().left;
					}
					slider.css('left', position);
				});
			}
			
			

			// Click to close
			$(document).on('click', '#menuLateralMetabuscador, .row.upper-row, #txtBusquedaPrincipal', function(e) {
				e.stopPropagation();
				var $target = $(e.target);
				if(!$target.closest('#menuLateralMetabuscador').length && $('#menuLateralMetabuscador').is(":visible")) {
					$('#menuLateralMetabuscador').slideReveal('hide');
				}
			});
			$('#txtBusquedaPrincipal').off('click');
			$('#txtBusquedaPrincipal').on('click', function(e){
				e.stopPropagation();
				$('#menuLateralMetabuscador').slideReveal('show');
				$('.col-facetas .cerrar').trigger('click');
			});
		},
		hide: function () {
			that.body.removeClass('metabuscador-abierto');
			clearInterval(that.timerId);
		}
	});
}

function tooltipsImpactFactor()
{
	$('.impactFactorTooltip').each(function() {
		$(this).tooltip({
			html: true,
			placement: 'bottom',
			template: '<div class="tooltip background-blanco impactfactor" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
			title: $(this).find('.tooltipContent').html()
		});
	});	
	$('.quartileTooltip').each(function() {
		$(this).tooltip({
			sanitize: false,
			html: true,
			placement: 'bottom',
			template: '<div class="tooltip background-blanco cuartiles" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
			title: $(this).find('.tooltipContent').html()
		});
	});	
}	
$(document).ready(e=>{
    tooltipsAccionesRecursos.lanzar = function () {
        montarTooltip.lanzar(this.info_resource, '', 'background-gris grupos');
        montarTooltip.lanzar(this.quotes, this.getTooltipQuotes(), 'background-blanco citas');
        montarTooltip.lanzar($(".hindex"),this.getTooltipHindex(),'background-blanco citas');
        montarTooltip.lanzar(this.block, 'Bloqueado', 'background-gris-oscuro');
        montarTooltip.lanzar(this.visible, 'Visible', 'background-gris-oscuro');
        montarTooltip.lanzar(this.oculto, 'Oculto', 'background-gris-oscuro');
    }
    tooltipsAccionesRecursos.lanzar();
});

function tooltipMatching (pTextoMesh, pUrlMesh, pDicSnomed, pElemento) {

    var dic = JSON.parse(pDicSnomed);
	var infoSnomed = "";

	for (const [ key, value ] of Object.entries(dic)) {
	    infoSnomed += `<a href="${key}" target="_blank" class="tooltip-link">SNOMED: ${value}</a><br>`;
	}

	var info = `<p class="tooltip-title">Matching</p>
                <span class="material-icons cerrar">close</span>
                <div class="tooltip-content">
                    <a href="${pUrlMesh}" target="_blank" class="tooltip-link">MESH: ${pTextoMesh}</a><br>
                    ${infoSnomed}
                </div>`;

    $(pElemento).tooltip('dispose');
    montarTooltip.lanzar($(pElemento), info, 'background-blanco link');
    $(pElemento).tooltip('show');
}

comportamientoVerMasVerMenosTags.comportamiento = function() {
    $('.list-wrap .moreResults .ver-mas').off('click').on('click', function () {
        var list = $(this).closest('.list-wrap');
        list.find('ul > .ocultar').show(300);
        list.find('ul > .ocultar').css('display', 'flex');
        list.find('.ver-mas').hide();
        list.find('.ver-menos').show();
    });

    $('.list-wrap .moreResults .ver-menos').off('click').on('click', function () {
        var list = $(this).closest('.list-wrap');
        list.find('ul > .ocultar').hide(300);
        list.find('.ver-menos').hide();
        list.find('.ver-mas').show();
    });
    
    $('#idSelectorFormatoCita').off('change.showcita').on('change.showcita', function () {
        var valor = $(this).find('option:selected').val();
        if (valor == '-') {
            $('#idContenedorResultadoCita').css('display', 'none');
        } else {
            var urlQuote = url_servicio_externo + "Citas/GetQuoteText";
            var argQuote = {};
            argQuote.pIdRecurso = $('.ficha-title-wrap h1[about]').attr('about');
            argQuote.pFormato = valor;
            MostrarUpdateProgress();
            $.get(urlQuote, argQuote, function (data) {
                $('#idTextoCita').text(data);
                $('#idContenedorResultadoCita').css('display', 'block');
                OcultarUpdateProgress();
            });
        }
    });
    
    $('#btnCopiarCita').off('click').on('click', function () {
        var textoCita = $('#idTextoCita').text();
        if (navigator.clipboard) {
            navigator.clipboard.writeText(textoCita);
        } else {
            var temp = $("<input>");
            $(this).append(temp);
            temp.val(textoCita).select();
            document.execCommand("copy");
            temp.remove();
        }
        mostrarNotificacion('info', 'Cita copiada');
    });

    $('#idSelectorDescargaCita').off('change.showdescarga').on('change.showdescarga', function () {
        var valor = $(this).find('option:selected').val();
        if (valor == '-') {
            $('#btnDescargarCita').parent().css('display', 'none');
        } else {
            $('#btnDescargarCita').parent().css('display', 'block');
        }
    });
    $('#btnDescargarCita').off('click').on('click', function () {
        var urlQuote = url_servicio_externo + "Citas/GetQuoteDownload";
        var argQuote = {};
        argQuote.pIdRecurso = $('.ficha-title-wrap h1[about]').attr('about');
        var valor = $('#idSelectorDescargaCita').find('option:selected').val();
        argQuote.pFormato = valor;
        if (valor != '-') {
            MostrarUpdateProgress();
            urlQuote += "?pIdRecurso=" + argQuote.pIdRecurso + "&pFormato=" + argQuote.pFormato;
            document.location.href = urlQuote;
            OcultarUpdateProgress();
        }
    });
};


/**
 * Función que limpia un string como url a semejanza de GNOSS
 */
function cleanStringUrlLikeGnoss (text) {
	let nameUrlRo = ""
    let posToBreak = text.length > 50 ? text.lastIndexOf(" ", 50) : -1

    if (posToBreak != -1) {
        nameUrlRo = cleanStringUrl(text.substring(0, posToBreak))
    } else {
        nameUrlRo = cleanStringUrl(text)
    }
    return nameUrlRo
}


/**
 * Función que limpia un string como un enlace
 */
function cleanStringUrl (text) {
	return removeAccents(text).trim().toLowerCase().replace(/[^a-z0-9 ]+/g,'').replace(/[^a-z0-9]+/g,'-')
}


/**
 * Función que elimina los acentos
 */
function removeAccents (text) {
	var ts = '';
	for (var i = 0; i < text.length; i++) {
		var c = text.charCodeAt(i);
		if (c >= 224 && c <= 230) { ts += 'a'; }
		else if (c >= 232 && c <= 235) { ts += 'e'; }
		else if (c >= 236 && c <= 239) { ts += 'i'; }
		else if (c >= 242 && c <= 246) { ts += 'o'; }
		else if (c >= 249 && c <= 252) { ts += 'u'; }
		else { ts += text.charAt(i); }
	}
	return ts;
}




// Función para hacer la llamada y comprobar si el usuario es un gestor Otri
function CheckIsOtri(idUser) {
	// Comfigura la URL
	let uriIsOtriGestor = "Ofertas/CheckIfIsOtri"
	let urlIOG = new URL(url_servicio_externo +  uriIsOtriGestor)
	urlIOG.searchParams.set('pIdGnossUser', idUser)

	// Realizo la llamada y devuelvo un booleano si es otri o no
	return new Promise((resolve) => {
		$.get(urlIOG.toString(), function (isOtri) {
			resolve(isOtri)
		});
	})
	
}


// Comprobar si el usuario es un gestor Otri y habilitar la url
function EnhableIfIsOtri() {


    let isOtriVal = localStorage.getItem("ISOTRI"); // Obtengo del localStorage si el usuario es otri
	let idUser = $('#inpt_usuarioID').val() // Obtego el id del usuario
	// Comprueba si hacer la llamada ajax
	if (CheckIfCallIsOtri(idUser)) {
		// Realizo la llamada ajax
		CheckIsOtri(idUser).then((isOtri) => {
    		// Actualiza el estado si es el caso
    		localStorage.setItem("ISOTRI", isOtri);
    		isOtriVal = isOtri

    		// Si el usuario no ha cambiado y es un gestor otri, se activa
			if (isOtriVal) {
				$('#menu_is_otri').removeClass("d-none")
				if ($('#menu_ed_is_otri').length > 0) {
					$('#menu_ed_is_otri').removeClass("d-none")
				}
			}
		})
	} else {
		// Si el usuario no ha cambiado y es un gestor otri, se activa
		if (isOtriVal) {
			$('#menu_is_otri').removeClass("d-none")
			if ($('#menu_ed_is_otri').length > 0) {
				$('#menu_ed_is_otri').removeClass("d-none")
			}
		}
	}
}


// Comprobar si llamar al servicio para comprobar si el usuario es un gestor Otri
function CheckIfCallIsOtri(idUser) {

	let currentSavedIdUser = localStorage.getItem("IDUSER")

	// Compruebo si el valor en localSotorage está vacío
	if (currentSavedIdUser == "") {
        localStorage.setItem("IDUSER", idUser);
        return true
    // Compruebo si el id de usuario ha cambiado
	} else if (idUser != currentSavedIdUser) {
        localStorage.setItem("IDUSER", idUser);
        return true
	} else {
        return false
	}
}

function refrescarNumElementosNuevos(){
}