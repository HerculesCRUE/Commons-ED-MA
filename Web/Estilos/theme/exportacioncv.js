var urlExportacionCV = url_servicio_editorcv+"ExportadoCV/";
var perfilesExportacion;

var exportacionCV = {
    idUsuario: null,
    init: function() {
		$('#containerCV').closest('.block').addClass('no-cms-style');
        this.config();
        this.idUsuario = $('#inpt_usuarioID').val();
		
        return;
    },
	config: function(){
		var that = this;
		that.cargarListadoCV();
		
		//Preparar exportación
		$('.btGenerarExportarCV').off('click').on('click', function(e) {            
			e.preventDefault();
			if($('#exportCvName').val().length == 0){
				window.alert("Debes añadir un nombre al fichero");
				return false;
			}else{
				if($('#todas:checked').length){
					MostrarUpdateProgress();
					//Exportacion completa
					var data = {};
					data.userID= that.idUsuario;
					data.lang= $('#ddlIdioma').val();
					data.nombreCV= $('#exportCvName').val();
					data.tipoCVNExportacion = $('#ddlTipoExportacion').find("option:selected").val();
					data.versionExportacion = $('#ddlVersionExportacion').find("option:selected").val();
					$.post(urlExportacionCV + 'GetCV', data, function(data) {
						OcultarUpdateProgress();
						mostrarNotificacion('success', GetText('CV_EXPORTAR_COMPLETO_COMPLETADO'));
						that.cargarListadoCV();
					});
				}
				else if($('#ultimosCinco:checked').length){
					that.cargarCV(true);
				}
				else if($('#seleccionar:checked').length){
					//Exportacion parcial
					that.cargarCV();
				}				
			}
		});		
		
		//Generar exportación
		$('.btExportarCV').off('click').on('click', function(e) {
            e.preventDefault();

			var tipoCVNExportacion = $('#ddlTipoExportacion').find("option:selected").val();

			var listaId = "";
			$('.resource-list .custom-control-input:checkbox:checked').each(function(){
				listaId += (this.checked ? $(this).val()+"@@@" : "")
			});
			
			listaId = listaId.slice(0,-3);
			
			if(listaId.length == 0){
				window.alert("Debes seleccionar alguna opción");
				return false;
			}
			
			var data = {};
			data.userID= that.idUsuario;
			data.lang= $('#ddlIdioma').val();
			data.listaId= listaId;
			data.nombreCV= $('#exportCvName').val();
			data.tipoCVNExportacion = tipoCVNExportacion;
			data.versionExportacion = $('#ddlVersionExportacion').find("option:selected").val();
			MostrarUpdateProgress();
			$.post(urlExportacionCV + 'GetCV', data, function(data) {
				OcultarUpdateProgress();
				mostrarNotificacion('success', GetText('CV_EXPORTAR_COMPLETADO'));
				that.cargarListadoCV();
			});
        });
		
		cambiarTipoExportacion();
	},
	//Carga los CV exportados
    cargarListadoCV: function() {
		$('.resource-list-wrap.listadoCV article').remove();
		$('.exportacion').hide();
		
        var that = this;
		that.idUsuario = $('#inpt_usuarioID').val();
		$('.col-contenido.listadoExportacion').show();
		$('.col-contenido.exportacion').hide();
		MostrarUpdateProgress();
		$.get(urlExportacionCV + 'GetListadoCV?userID=' + that.idUsuario +"&baseUrl="+$('#inpt_baseURL').val()+"&timezoneOffset="+new Date().getTimezoneOffset(), null, function(data) {
            //recorrer items y por cada uno
			for(var i=0;i<data.length;i++){				
				var ref = '';
				var estado = '';
				
				if(data[i].fichero == '' && data[i].estado=='error'){
					//Añado icono de Warning en caso de producirse error
					ref = '<span class="material-icons warning">warning</span>' + data[i].titulo;
				}
				else if(data[i].fichero == ''){
					ref = data[i].titulo; 
				}
				else{
					ref = '<a href="' + data[i].fichero + '">' + data[i].titulo + '</a>';
				}
				//Cambio los colores del ::before dependiendo del estado del archivo
				if(data[i].estado=='procesado'){
					estado = 'success';
				}
				else if(data[i].estado=='pendiente'){
					estado = 'pending';
				}
				
				var articleHTML = `<article class="resource plegado ${estado}" num="${i}">
										<div class="middle-wrap">
											<div class="title-wrap">
												<h2 class="resource-title">
													${ref}
												</h2>
											</div>
											<div class="content-wrap">
												<div class="description-wrap counted">
													<div class="list-wrap no-oculto">
														<div class="label">Estado</div>
														<ul>
															<li class="entity">
																${GetText("CV_EXPORTAR_"+data[i].estado.toUpperCase())}
															</li>
														</ul>
													</div>
													<div class="list-wrap no-oculto">
														<div class="label">Fecha</div>
														<ul>
															<li>${data[i].fecha}</li>
														</ul>
													</div>
												</div>
											</div>
										</div>
									</article>`;
				$('.listadoCV').append(articleHTML);
			}
			if ($('.panNavegador div.items.dropdown').length == 0) {
				$('.panNavegador').append(`
				<div class="items dropdown">
					<a class="dropdown-toggle" data-toggle="dropdown" aria-expanded="true">
						<span class="texto" items="5">Ver 5 items</span>
					</a>
					<div class="dropdown-menu basic-dropdown dropdown-menu-right" x-placement="bottom-end">
						<a href="javascript: void(0)" class="item-dropdown" items="5">Ver 5 items</a>
						<a href="javascript: void(0)" class="item-dropdown" items="10">Ver 10 items</a>
						<a href="javascript: void(0)" class="item-dropdown" items="20">Ver 20 items</a>
						<a href="javascript: void(0)" class="item-dropdown" items="50">Ver 50 items</a>
						<a href="javascript: void(0)" class="item-dropdown" items="100">Ver 100 items</a>
					</div>
				</div>
				<nav>
					<ul class="pagination arrows">
					</ul>
					<ul class="pagination numbers">
						<li class="actual"><a href="javascript: void(0)" page="1">1</a></li>
					</ul>
				</nav>`);
			}
			that.pintarPaginacion();
			OcultarUpdateProgress();
		});
	},
	pintarPaginacion: function(pagina = 0, numItems = 5) {
		var numResultadosPagina = numItems == 5 ? parseInt($(' .panNavegador .dropdown-toggle span').attr('items')) : parseInt(numItems);
		var paginaActual = pagina == 0 ? parseInt($('.panNavegador .pagination.numbers li.actual a').attr('page')) : parseInt(pagina);
		$('.panNavegador .dropdown-menu a').removeClass('active');
		$('.panNavegador .dropdown-menu a[items="' + numResultadosPagina + '"]').addClass('active');
		var NUM_PAG_INICIO = 3;
        var NUM_PAG_PROX_CENTRO = 2;
        var NUM_PAG_FIN = 3;

        var numTotal = 1;
        var numPaginas = 1;
		$('.listadoCV article').attr('style','display:none');
        $('.listadoCV article').each(function() {
			numPaginas = Math.floor((numTotal - 1 + numResultadosPagina) / numResultadosPagina);
			if (numPaginas == paginaActual) {
				$(this).show();
			}
			numTotal++;
        });
        $('.panNavegador .pagination.numbers').empty();
        $('.panNavegador .pagination.arrows').empty();

        // INICIO
        for (i = 1; i <= NUM_PAG_INICIO; i++) {
            if (i > numPaginas) // Hemos excedido el número máximo de páginas, así que dejamos de pintar.
            {
                break;
            }
            if (i == paginaActual) {
                $('.panNavegador .pagination.numbers').append(`<li class="actual"><a page="${i}" >${i}</a></li>`);
            } else {
                $('.panNavegador .pagination.numbers').append(`<li ><a href="javascript: void(0)" page="${i}" >${i}</a></li>`);
            }
        }

        if (numPaginas > NUM_PAG_INICIO) // Continuamos si hay más páginas que las que ya hemos pintado
        {
            var inicioRango = paginaActual - NUM_PAG_PROX_CENTRO;
            var finRango = paginaActual + NUM_PAG_PROX_CENTRO;

            if (paginaActual < (NUM_PAG_INICIO + NUM_PAG_PROX_CENTRO + 1)) {
                inicioRango = NUM_PAG_INICIO + 1;
                if (paginaActual <= NUM_PAG_INICIO) //En el rango de las primeras
                {
                    finRango = paginaActual + NUM_PAG_INICIO + NUM_PAG_PROX_CENTRO - 1;
                } else {
                    finRango = NUM_PAG_INICIO + (2 * NUM_PAG_PROX_CENTRO) + 1; //Ultimo número de la serie.
                }
            } else if (paginaActual > (numPaginas - NUM_PAG_FIN - NUM_PAG_PROX_CENTRO)) {
                finRango = numPaginas - NUM_PAG_FIN;
                if (paginaActual > numPaginas - NUM_PAG_FIN) //En el rango de las últimas
                {
                    inicioRango = paginaActual - NUM_PAG_FIN - NUM_PAG_PROX_CENTRO + 1; //finRango - (pNumPaginas - paginaActual + 1);
                } else {
                    inicioRango = numPaginas - (NUM_PAG_FIN + (2 * NUM_PAG_PROX_CENTRO)); //Ultimo número de la serie empezando atrás.
                }

                //Avanzamos el inicio de la zona final para que no agrege páginas ya pintadas
                while (inicioRango <= NUM_PAG_INICIO) {
                    inicioRango++;
                }
            }

            if (inicioRango > (NUM_PAG_INICIO + 1)) {
                $('.panNavegador .pagination.numbers').append(`<span>...</span>`);
            }


            for (i = inicioRango; i <= finRango; i++) {
                if (i > numPaginas) // Hemos excedido el número máximo de páginas, así que dejamos de pintar.
                {
                    break;
                }

                if (i == paginaActual) {
                    $('.panNavegador .pagination.numbers').append(`<li class="actual"><a page="${i}" >${i}</a></li>`);
                } else {
                    $('.panNavegador .pagination.numbers').append(`<li><a href="javascript: void(0)" page="${i}" >${i}</a></li>`);
                }
            }

            if (finRango < numPaginas) {
                // Continuamos si hay más páginas que las que ya hemos pintado
                inicioRango = numPaginas - NUM_PAG_FIN + 1;

                if ((inicioRango - 1) > finRango) {
                    $('.panNavegador .pagination.numbers').append(`<span>...</span>`);
                }

                //Avanzamos el inicio de la zona final para que no agrege páginas ya pintadas
                while (inicioRango <= finRango) {
                    inicioRango++;
                }

                finRango = numPaginas;

                for (i = inicioRango; i <= finRango; i++) {
                    if (i > numPaginas) //Hemos excedio el número máximo de páginas, así que dejamos de pintar.
                    {
                        break;
                    }

                    if (i == paginaActual) {
                        $('.panNavegador .pagination.numbers').append(`<li class="actual"><a page="${i}" >${i}</a></li>`);
                    } else {
                        $('.panNavegador .pagination.numbers').append(`<li><a href="javascript: void(0)" page="${i}" >${i}</a></li>`);
                    }
                }
            }
        }

        if (paginaActual == 1) {
            $('.panNavegador .pagination.arrows').append(`<li class="actual"><a class="primeraPagina">Página anterior</a></li>`);
        } else {
            $('.panNavegador .pagination.arrows').append(`<li><a href="javascript: void(0)" page="${(parseInt(paginaActual) - 1)}" class="primeraPagina">Página anterior</a></li>`);
        }

        if (paginaActual == numPaginas) {
            $('.panNavegador .pagination.arrows').append(`<li class="actual"><a class="ultimaPagina">Página siguiente</a></li>`);
        } else {
            $('.panNavegador .pagination.arrows').append(`<li><a href="javascript: void(0)" page="${(parseInt(paginaActual) + 1)}" class="ultimaPagina">Página siguiente</a></li>`);
        }

		// Cambiar de página
		$('.panNavegador ul.pagination li a').off('click').on('click', function(e) {
			var pagina = $(this).attr('page');
			exportacionCV.pintarPaginacion(pagina);
        });
        // Cambiar número de items por página
        $('.panNavegador .item-dropdown').off('click').on('click', function(e) {
			var itemsPagina = parseInt($(this).attr('items'));
			$('.panNavegador .dropdown-toggle span').attr('items', itemsPagina);
			$('.panNavegador .dropdown-toggle span').text($(this).text());
			exportacionCV.pintarPaginacion(1, itemsPagina);
        });
	},
	//Carga los datos del CV para la exportacion
    cargarCV: function(isLast5Years) {
		$('.col-contenido.listadoExportacion').hide();
        var that = this;
		var petition = 'x' + RandomGuid();
		MostrarUpdateProgressTime(0);
		
		if($('#titleMascaraBlanca').length == 0){
			$('#mascaraBlanca').find('.wrap.popup').append('<br><div id="titleMascaraBlanca"></div>');
			$('#mascaraBlanca').find('.wrap.popup').append('<div id="workMascaraBlanca"></div>');
		}
		$('.panel-group.pmd-accordion').remove();
		
		//Actualizo el estado cada 500 milisegundos
		var intervalStatus = setInterval(function() {
			$.ajax({
				url: urlExportacionCV + 'ExportarCVStatus?petitionID=' + petition,
				type: 'GET',
				success: function ( response ) {
					if(response != null && response != ''){
						if(response.subTotalWorks == null || response.subTotalWorks == 0 || response.subActualWork==response.subTotalWorks){
							$('#titleMascaraBlanca').text(`${GetText(response.actualWorkTitle)}`);
						}
						else{
							$('#titleMascaraBlanca').text(`${GetText(response.actualWorkTitle)}` + " (" +  response.subActualWork + '/' + response.subTotalWorks + ")");
						}
						//Si no hay pasos maximos no muestro la lista
						if(response.totalWorks != 0){
							$('#workMascaraBlanca').text('Pasos totales: ' + response.actualWork + '/' + response.totalWorks);
						}
					}
				}
			});	
		}, 500);		
		
		
        $.ajax({
			url: urlExportacionCV + 'GetAllTabs?userID=' + that.idUsuario + "&petitionID=" + petition + "&pLang=" + lang,
			type: 'GET',
			success: function(data) {	
				clearInterval(intervalStatus);			
				$('#titleMascaraBlanca').remove();
				$('#workMascaraBlanca').remove();
				//recorrer items y por cada uno			
				for(var i=0;i<data.length;i++){
					var id = 'x' + RandomGuid();
					var contenedorTab=`<div class="panel-group pmd-accordion" id="datos-accordion${i}" role="tablist" aria-multiselectable="true">
											<div class="panel">
												<div class="panel-heading" role="tab" id="datos-tab">
													<p class="panel-title">
														<a data-toggle="collapse" data-parent="#datos-accordion${i}" href="#${id}" aria-expanded="true" aria-controls="datos-tab" data-expandable="false" class="">
															<span class="texto">${data[i].title}</span>
															<span class="material-icons pmd-accordion-arrow">keyboard_arrow_up</span>
														</a>
													</p>
												</div>
												<div id="${id}" class="collapse show">
													<div class="row cvTab">
														<div class="col-12 col-contenido">
														</div>
													</div>
												</div>
											</div>
										</div>`
					if(i==0){
						$('.contenido-cv').append( $(contenedorTab));
						var html = edicionCV.printPersonalData(id, data[i]);					
						$('div[id="' + id + '"] .col-12.col-contenido').append(html);
						$('#'+id+' input[type="checkbox"]').prop('checked',true);
					}else if(i == 6){
						$('.contenido-cv').append( $(contenedorTab));		
						var html = printFreeText(id, data[i], isLast5Years);
						$('div[id="' + id + '"] .col-12.col-contenido').append(html);				
					}else{
						$('.contenido-cv').append( $(contenedorTab));		
						edicionCV.printTab(id, data[i], isLast5Years);
					}				
				}			
				
				asignarAccionesBotonesPerfil(that.idUsuario);
				var perfiles = getExportationProfile(that.idUsuario);
				if($('#ddlProfile option').length < 1){
					$('.misPerfiles').hide();
				}
				
				$('.resource-list.listView .resource .wrap').css("margin-left", "70px")
				checkAllCVWrapper();
				
				$('.exportacion').show();
				$('.col-contenido.exportacion').show();
				OcultarUpdateProgress();
			},
			error: function(jqXHR, exception){
				clearInterval(intervalStatus);			
				$('#titleMascaraBlanca').remove();
				$('#workMascaraBlanca').remove();
			}
		});
        return;
    }
};

function cambiarTipoExportacion(){
	$('#ddlTipoExportacion').change( function(e){
		e.preventDefault();
		var tipoExportacion = $("#ddlTipoExportacion").find("option:selected").val();
		if(tipoExportacion == "PN2008"){
			//CVN
			// Muestro "Total" y asigno por defecto "Total"
			$($('.ddlSecciones .custom-control.themed.little.custom-radio')[0]).show();
			$($('.ddlSecciones .custom-control.themed.little.custom-radio input')[0]).prop('checked', true);
		}
		else{
			//CV abreviado
			// Oculto "Total" y asigno por defecto "Seleccionar"
			$($('.ddlSecciones .custom-control.themed.little.custom-radio')[0]).hide();
			$($('.ddlSecciones .custom-control.themed.little.custom-radio input')[1]).prop('checked', true);
		}
	});
}


function asignarAccionesBotonesPerfil(userID){
	$('.btn.btn-primary.uppercase.btGuardarCV').off('click').on('click', function(e) {
		var title = $(".nombrePerfilExportacion").val();
		if(title != null && title != ""){
			var checkList = $("input[type='checkbox']:checked");
			var checkValues = [];
			for(var i = 0; i < checkList.length; i++){
				checkValues.push(checkList[i].value);
			}
			addExportationProfile(userID, title, checkValues);
		}else{
			window.alert("Debes añadir un nombre");
		}
	});
	$('.btn.btn-primary.uppercase.btCargarCV').off('click').on('click', function(e) {
		var selectedOption = $("#ddlProfile").find("option:selected").val();
		if(selectedOption == GetText('SELECCIONA_UNA_OPCION')){
			window.alert('No hay un perfil seleccionado');
			return;
		}
		var title = $("#ddlProfile").find("option:selected").text();
		loadExportationProfile(title);
	});
	
	$('.btn.btn-primary.uppercase.btBorrarCV').off('click').on('click', function(e) {
		var selectedOption = $("#ddlProfile").find("option:selected").val();		
		if(selectedOption == GetText('SELECCIONA_UNA_OPCION')){
			window.alert('No hay un perfil seleccionado');
			return;
		}
		var title = $("#ddlProfile").find("option:selected").text();
		deleteExportationProfile(userID, title);
	});
}

function getExportationProfile(userID){	
	$.ajax({
		url: url_servicio_editorcv+"ExportadoCV/GetPerfilExportacion?userID=" + userID,
		type: 'GET',
		success: function ( response ) {
			perfilesExportacion = response;
			var contador = 0;
			var selector = $('#ddlProfile');
			selector.empty();
			var initialOpcion = '<option selected="selected" disabled="disabled">' + GetText('SELECCIONA_UNA_OPCION') + "</option>";
			selector.append(initialOpcion);
			for(var opcion in response){				
				selector.append(new Option(opcion, contador));	
				contador++;
			}
			if(contador==0){
				$('.misPerfiles').hide();
			}else{
				$('.misPerfiles').show();
			}
			
			return response;
		}
	});
}

function loadExportationProfile(title){
	if(title == GetText('SELECCIONA_UNA_OPCION')){
		window.alert('No hay un perfil seleccionado');
		return;
	}
	for(var opcion in perfilesExportacion){
		if(opcion == title){
			for(var check in perfilesExportacion[opcion]){
				var elementoSeleccionar = $('input[value="' + perfilesExportacion[opcion][check] + '"');
				elementoSeleccionar.prop('checked', true);
			}
		}
	}
}

function deleteExportationProfile(userID, title){	
	var formData = new FormData();
	formData.append('userID', userID);
	formData.append('title', title);
	
	$.ajax({
		url: url_servicio_editorcv+"ExportadoCV/DeletePerfilExportacion?userID=" + userID + "&title=" + title,
		type: 'DELETE',
		data: formData,	
		cache: false,
		processData: false,
		enctype: 'multipart/form-data',
		contentType: false,
		success: function ( response ) {
			urlExportationProfilesCV = response;
			mostrarNotificacion('success', GetText('CV_ELIMINAR_PERFIL_EXPORTACION'));
			getExportationProfile(userID);
			OcultarUpdateProgress();
		},
		error: function(jqXHR, exception){
			mostrarNotificacion('warning', GetText('CV_ERROR_ELIMINAR_PERFIL_EXPORTACION'));
			getExportationProfile(userID);
			OcultarUpdateProgress();		
		}
	});
}

function addExportationProfile(userID, title, checks){
	var formData = new FormData();
	formData.append('userID', userID);
	formData.append('title', title);
	for(var i=0; i < checks.length; i++){
		formData.append('checks', checks[i]);
	}
	
	MostrarUpdateProgress();
	$.ajax({
		url: url_servicio_editorcv+"ExportadoCV/AddPerfilExportacion",
		type: 'POST',
		data: formData,	
		cache: false,
		processData: false,
		enctype: 'multipart/form-data',
		contentType: false,
		success: function ( response ) {			
			mostrarNotificacion('success', GetText('CV_ANIADIR_PERFIL_EXPORTACION'));
			getExportationProfile(userID);
			OcultarUpdateProgress();
		},
		error: function(jqXHR, exception){
			mostrarNotificacion('warning', GetText('CV_ERROR_ANIADIR_PERFIL_EXPORTACION'));
			getExportationProfile(userID);
			OcultarUpdateProgress();
		}
	});
}

function checkAllCVWrapper(){
	$('.checkAllCVWrapper input[type="checkbox"]').off('click').on('click', function(e) {
		if(!$(this)[0].checked)
		{
			$(this).closest('.custom-control').find('.custom-control-label').text(GetText('CV_SELECCIONAR_TODOS'));
		}
		else
		{
			$(this).closest('.custom-control').find('.custom-control-label').text(GetText('CV_DESELECCIONAR_TODOS'));
		}
		$(this).closest('.panel-body').find('article div.custom-checkbox input[type="checkbox"]').prop('checked',$(this).prop('checked'));
	});
	
	$('.checkAllCVWrapper input[type="checkbox"]').closest('.panel-body').find('article div.custom-checkbox input[type="checkbox"]').off('change').on('change', function(e) {
		if(!$(this).prop('checked')){			
			$(this).closest('.panel-body').find('.checkAllCVWrapper input[type="checkbox"]').prop('checked', false);
		}
	});
}

function printCientificProduction(id, data, isLast5Years){
	//Pintado sección listado
	//css mas generico
	var id = 'x' + RandomGuid();
	var id2 = 'x' + RandomGuid();
	var isChecked = "";
	var expanded = "";
	var show = "";
	if (data.item != null) {
		if (Object.keys(data.item).length > 0) {
			//Desplegado
			expanded = "true";
			show = "show";
		} else {
			//No desplegado	
			expanded = "false";
		}		
		if(isLast5Years){
			isChecked = "checked";
		}
		
		var htmlSection = `
		<div class="panel-group pmd-accordion" section="${data.identifier}" id="${id}" role="tablist" aria-multiselectable="true">
			<div class="panel">
				<div class="panel-heading" role="tab" id="publicaciones-tab">
					<p class="panel-title">
						<a data-toggle="collapse" data-parent="#${id}" href="#${id2}" aria-expanded="${expanded}" aria-controls="${id2}" data-expandable="false">
							<span class="material-icons pmd-accordion-icon-left">folder_open</span>
							<span class="texto">${data.title}</span>
						</a>
					</p>
				</div>
				<div id="${id2}" class="panel-collapse collapse ${show}" role="tabpanel">
					<div id="situacion-panel" class="panel-collapse collapse show" role="tab-panel" aria-labelledby="situacion-tab" style="">
						<div class="panel-body">
							<div class="resource-list listView">
								<div class="resource-list-wrap">
									<article class="resource success" >
										<div class="custom-control custom-checkbox">
											<input type="checkbox" class="custom-control-input" id="check_resource_${id}"  value="${data.item.entityID}" ${isChecked}>
											<label class="custom-control-label" for="check_resource_${id}"></label>
										</div>
										<div class="wrap">
											<div class="middle-wrap">
												<div class="title-wrap">
													<h2 class="resource-title">${data.title}</h2>
												</div>
												<div class="content-wrap">
													<div class="description-wrap">
													</div>
												</div>
											</div>
										</div>
									</article>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>`;
		return htmlSection;
	}
	return '';
}
	
function printFreeText(id, data, isLast5Years){
	var id2 = 'x' + RandomGuid();
	var expanded = "";
	var show = "";
	var isChecked = "";
	if (data.sections != null) {
		if (Object.keys(data.sections).length > 0) {
			//Desplegado
			expanded = "true";
			show = "show";
		} else {
			//No desplegado	
			expanded = "false";
		}
		if(isLast5Years){
			isChecked = "checked";
		}
		var html = `<div class="panel-group pmd-accordion collapse show" section="${data.sections[0].title}" id="${id}" role="tablist" aria-multiselectable="true">
						<div class="panel">
							<div class="panel-heading" role="tab" id="publicaciones-tab">
								<p class="panel-title">
									<a data-toggle="collapse" data-parent="#${id}" href="#${id2}" aria-expanded="${expanded}" aria-controls="${id2}" data-expandable="false">
										<span class="material-icons pmd-accordion-icon-left">folder_open</span>
										<span class="texto">${data.title}</span>
										<span class="material-icons pmd-accordion-arrow">keyboard_arrow_up</span>
									</a>
								</p>
							</div>
							<div id="${id2}" class="panel-collapse collapse ${show}" role="tabpanel">
								<div id="situacion-panel" class="panel-collapse collapse show" role="tab-panel" aria-labelledby="situacion-tab" style="">
									<div class="panel-body">
										<div class="acciones-listado acciones-listado-cv">
											<div class="wrap">
												<div class="checkAllCVWrapper" id="checkAllCVWrapper">
													<div class="custom-control custom-checkbox">
														<input type="checkbox" class="custom-control-input" id="checkAllResources_${id2}">
														<label class="custom-control-label" for="checkAllResources_${id2}">${GetText('CV_SELECCIONAR_TODOS')}
														</label>
													</div>
												</div>
											</div>
										</div>
										<div class="resource-list listView">
											<div class="resource-list-wrap">
						`
		var secciones = data.sections[0].item.sections[0].rows;
		for (var i = 0; i < secciones.length; i++){
			//Si no hay datos no pinto esa sección
			if(secciones[i].properties[0].values.length>0 && secciones[i].properties[0].values[0].length>0){
				var id = 'x' + RandomGuid();
				var html2 = `<article class="resource success" >
								<div class="custom-control custom-checkbox">
									<input type="checkbox" class="custom-control-input" id="check_resource_${id}"  value="${data.sections[0].item.entityID + "|||" + secciones[i].properties[0].property}"  ${isChecked}>
									<label class="custom-control-label" for="check_resource_${id}"></label>
								</div>
								<div class="wrap">
									<div class="middle-wrap">
										<div class="title-wrap">
											<h2 class="resource-title">${secciones[i].properties[0].title}</h2>
										</div>
										<div class="content-wrap">
											<div class="description-wrap">
											</div>
										</div>
									</div>
								</div>
							</article>`;
				html += html2;
			}
		}			
		html += `						</div>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>`;
					
		 return html;
	}
}	
	
edicionCV.printTab = function(entityID, data, isLast5Years) {
	var that=this;	
	if (data.entityID != null) {
		$('div[id="' + entityID + '"] .col-12.col-contenido').append(this.printPersonalData(data));
	} else {
		for (var i = 0; i < data.sections.length; i++) {	
			if(data.sections[i].identifier=="http://w3id.org/roh/generalQualityIndicators")
			{
				$('div[id="' + entityID + '"] .col-12.col-contenido').append(printCientificProduction(entityID, data.sections[i], isLast5Years));
			}
			else
			{			
				$('div[id="' + entityID + '"] .col-12.col-contenido').append(this.printTabSection(data.sections[i], isLast5Years));
				if (data.sections[i].items != null) {
					this.repintarListadoTab(data.sections[i].identifier,true);
				} else if (data.sections[i].item != null) {
					that.printSectionItem(data.sections[i].item.idContenedor, data.sections[i].item, data.sections[i].identifier, $('div[id="' + entityID + '"]').attr('rdftype'), data.sections[i].item.entityID);
					//Si no tiene ningun campo valor se repliega
					var plegar=true;
					$('div[section="' + data.sections[i].identifier+'"] input').each(function() {
						if($(this).val()!='')
						{
							plegar=false;
						}
					});
					//
					$('div[section="' + data.sections[i].identifier+'"] div.visuell-view').each(function() {
						if($(this).html()!='')
						{
							plegar=false;
						}
					});
					if(plegar)
					{
						$('div[section="' + data.sections[i].identifier+'"] .panel-collapse.collapse').removeClass('show');
						$('div[section="' + data.sections[i].identifier+'"] .panel-heading a').attr('aria-expanded','false');
					}
				}
			}
		}
	}
	accionesPlegarDesplegarModal.init();
	this.engancharComportamientosCV();
	this.mostrarTraducciones();		
};
	
edicionCV.printPersonalData=function(id, data) {	
	var id2 = 'x' + RandomGuid();
	var expanded = "";
	var show = "";
	if (data.sections != null) {
		if (Object.keys(data.sections).length > 0) {
			//Desplegado
			expanded = "true";
			show = "show";
		} else {
			//No desplegado	
			expanded = "false";
		}
		var nombre = '';
		for(var i =0; i < data.sections[0].rows[0].properties.length; i++){
			if(data.sections[0].rows[0].properties[i].values[0] != null){
				nombre += data.sections[0].rows[0].properties[i].values[0];
				nombre += " ";				
			}
		}
		
		var html = `<div class="panel-group pmd-accordion collapse show" section="${data.sections[0].title}" id="${id}" role="tablist" aria-multiselectable="true">
						<div class="panel">
							<div class="panel-heading" role="tab" id="publicaciones-tab">
								<p class="panel-title">
									<a data-toggle="collapse" data-parent="#${id}" href="#${id2}" aria-expanded="${expanded}" aria-controls="${id2}" data-expandable="false">
										<span class="material-icons pmd-accordion-icon-left">folder_open</span>
										<span class="texto">${data.title}</span>
										<span class="material-icons pmd-accordion-arrow">keyboard_arrow_up</span>
									</a>
								</p>
							</div>
							<div id="${id2}" class="panel-collapse collapse ${show}" role="tabpanel">
								<div id="situacion-panel" class="panel-collapse collapse show" role="tab-panel" aria-labelledby="situacion-tab" style="">
									<div class="panel-body">
										<div class="resource-list listView">
											<div class="resource-list-wrap">
												<article class="resource success" >
													<div class="custom-control custom-checkbox">
														<input type="checkbox" class="custom-control-input" id="check_resource_${id}"  value="${data.entityID}">
														<label class="custom-control-label" for="check_resource_${id}"></label>
													</div>
													<div class="wrap">
														<div class="middle-wrap">
															<div class="title-wrap">
																<h2 class="resource-title">${nombre}</h2>
																${this.printHtmlListItemEditable(data)}	
																${this.printHtmlListItemIdiomas(data)}
															</div>
															<div class="content-wrap">
																<div class="description-wrap">
																</div>
															</div>
														</div>
													</div>
												</article>
											</div>
										</div>
									</div>
								</div>
							</div>
						</div>
					</div>	`;
		 return html;
	}
};

edicionCV.printTabSection= function(data, isLast5Years) {
	//Pintado sección listado
	//css mas generico
	var id = 'x' + RandomGuid();
	var id2 = 'x' + RandomGuid();
	var expanded = "";
	var show = "";
	if (data.items != null) {
		if (Object.keys(data.items).length > 0) {
			//Desplegado
			expanded = "true";
			show = "show";
		} else {
			//No desplegado	
			expanded = "false";
		}
				
		//TODO texto ver items
		var htmlSection = `
		<div class="panel-group pmd-accordion" section="${data.identifier}" id="${id}" role="tablist" aria-multiselectable="true">
			<div class="panel">
				<div class="panel-heading" role="tab" id="publicaciones-tab">
					<p class="panel-title">
						<a data-toggle="collapse" data-parent="#${id}" href="#${id2}" aria-expanded="${expanded}" aria-controls="${id2}" data-expandable="false">
							<span class="material-icons pmd-accordion-icon-left">folder_open</span>
							<span class="texto">${data.title}</span>
							<span class="numResultados">(${Object.keys(data.items).length})</span>
							<span class="material-icons pmd-accordion-arrow">keyboard_arrow_up</span>
						</a>
					</p>
				</div>
				<div id="${id2}" class="panel-collapse collapse ${show}" role="tabpanel">
					<div id="situacion-panel" class="panel-collapse collapse show" role="tab-panel" aria-labelledby="situacion-tab" style="">
						<div class="panel-body">
							<div class="acciones-listado acciones-listado-cv">
								<div class="wrap">
									<div class="checkAllCVWrapper" id="checkAllCVWrapper">
										<div class="custom-control custom-checkbox">
											<input type="checkbox" class="custom-control-input" id="checkAllResources_${id2}">
											<label class="custom-control-label" for="checkAllResources_${id2}">${GetText('CV_SELECCIONAR_TODOS')}
										</div>
									</div>
								</div>
								<div class="wrap">
									<div class="ordenar dropdown orders">${this.printOrderTabSection(data.orders)}</div>
									<div class="buscador">
										<div class="fieldsetGroup searchGroup">
											<div class="textoBusquedaPrincipalInput">
												<input type="text" class="not-outline txtBusqueda" placeholder="${GetText('CV_ESCRIBE_ALGO')}" autocomplete="off">
												<span class="botonSearch">
													<span class="material-icons">search</span>
												</span>
											</div>
										</div>
									</div>
								</div>
							</div>
							<div class="resource-list listView">
								<div class="resource-list-wrap">
									${this.printHtmlListItems(data.items, isLast5Years)}
									<div class="panNavegador">
										<div class="items dropdown">
											<a class="dropdown-toggle" data-toggle="dropdown" aria-expanded="true">
												<span class="texto" items="5">Ver 5 items</span>
											</a>
											<div class="dropdown-menu basic-dropdown dropdown-menu-right" x-placement="bottom-end">
												<a href="javascript: void(0)" class="item-dropdown" items="5">Ver 5 items</a>
												<a href="javascript: void(0)" class="item-dropdown" items="10">Ver 10 items</a>
												<a href="javascript: void(0)" class="item-dropdown" items="20">Ver 20 items</a>
												<a href="javascript: void(0)" class="item-dropdown" items="50">Ver 50 items</a>
												<a href="javascript: void(0)" class="item-dropdown" items="100">Ver 100 items</a>
											</div>
										</div>
										<nav>
											<ul class="pagination arrows">
											</ul>
											<ul class="pagination numbers">	
												<li class="actual"><a href="javascript: void(0)" page="1">1</a></li>
											</ul>
										</nav>
									</div>
								</div>
							</div>
						</div>
					</div>
				</div>
			</div>
		</div>`;
		return htmlSection;
	}
};

edicionCV.printHtmlListItems = function(items, isLast5Years) {
	var html = "";
	for (var item in items) {
		html += this.printHtmlListItem(item, items[item], isLast5Years);
	}
	return html;
}

edicionCV.printHtmlListItem = function(id, data, isLast5Years) {
	let openAccess = "";
	let isChecked = "";
	let identifierItem = data.identifier;
	
	if (data.isopenaccess) {
		openAccess = "open-access";
	}
	
	if(isLast5Years && data.isChecked){ 
		isChecked = "checked";
	}
	
	if(data.identifier="00000000-0000-0000-0000-000000000000"){
		identifierItem = "x" + RandomGuid();
	}
	
	var htmlListItem = `<article class="resource success ${openAccess}" >
							<div class="custom-control custom-checkbox">
								<input type="checkbox" class="custom-control-input" id="check_resource_${identifierItem}"  value="${id}" ${isChecked}>
								<label class="custom-control-label" for="check_resource_${identifierItem}"></label>
							</div>
							<div class="wrap">
								<div class="middle-wrap">
									${this.printHtmlListItemOrders(data)}
									<div class="title-wrap">
										<h2 class="resource-title">${data.title}</h2>
										${this.printHtmlListItemEditable(data)}	
										${this.printHtmlListItemIdiomas(data)}
										<span class="material-icons arrow">keyboard_arrow_down</span>
									</div>
									<div class="content-wrap">
										<div class="description-wrap">
											${this.printHtmlListItemPropiedades(data)}
										</div>
									</div>
								</div>
							</div>
						</article>`;
	return htmlListItem;
};