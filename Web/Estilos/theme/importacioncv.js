var urlImportacionCV = url_servicio_editorcv+"ImportadoCV/";
var selectorConflictoNoBloqueado = '';
var selectorConflictoBloqueado = '';
var selectorCamposTexto = '';
var dropdownSimilitudes = '';
var contador = 1;
var urlUserCV = '';


function preventBeforeUnload(event){
	// Cancel the event as stated by the standard.
	event.preventDefault();
	// Chrome requires returnValue to be set.
	event.returnValue = '';
}

var dropdownSelectorSeccion = ''; 
var dropdownMostrarSeccion = '';


var importarCVN = {
	idUsuario:  null,
    init: function (){	
		$('#containerCV').closest('.block').addClass('no-cms-style');	
		this.config(),
		this.idUsuario = $('#inpt_usuarioID').val();
		this.fileData = '';
		this.filePreimport = '';

		var userID = $('#inpt_usuarioID').val();
		var lang = $('#inpt_Idioma').val();
		$.ajax({
			url: url_servicio_editorcv+"EdicionCV/GetCVUrl?userID=" + userID + "&lang=" + lang,
			type: 'GET',
			success: function ( response ) {				
				urlUserCV = response;
			}
		});

		dropdownSimilitudes = `<div class="ordenar dropdown selectsimilarity dropdown-select">
									<a class="dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
										<span class="material-icons">swap_vert</span>
										<span class="texto">${GetText('CV_MOSTRAR_TODOS')}</span>
									</a>
									<div class="dropdown-menu basic-dropdown dropdown-menu-right">
										<a class="item-dropdown mostrarTodos"><span class="texto">${GetText('CV_MOSTRAR_TODOS')}</span></a>
										<a class="item-dropdown mostrarSimilitudes"><span class="texto">${GetText('CV_MOSTRAR_SIMILITUDES')}</span></a>
										<a class="item-dropdown mostrarNuevos"><span class="texto">${GetText('CV_MOSTRAR_NUEVOS')}</span></a>
									</div>
								</div>`;

		selectorConflictoNoBloqueado = `<select name="itemConflict" disabled >
											<option value="ig" selected="">${GetText('CV_IGNORAR')}</option>
											<option value="fu">${GetText('CV_FUSIONAR')}</option>
											<option value="so">${GetText('CV_SOBREESCRIBIR')}</option>
											<option value="du">${GetText('CV_DUPLICAR')}</option>
										</select>`;

		selectorConflictoBloqueado = `<select name="itemConflict" disabled>
										<option value="ig" selected="">${GetText('CV_IGNORAR')}</option>
										<option value="fu">${GetText('CV_FUSIONAR')}</option>
										<option value="du">${GetText('CV_DUPLICAR')}</option>
									</select>`;

		selectorCamposTexto = `<select hidden name="itemConflict" class="uniqueItemConflict" disabled>
									<option value="so" selected="">${GetText('CV_SOBREESCRIBIR')}</option>
									<option value="ig">${GetText('CV_IGNORAR')}</option>
								</select>`;	

        return;        
    },
	config: function (){
		var that=this;
		$('#file_cvn').GnossDragAndDrop({
            acceptedFiles: ["pdf"],
			maxSize: 5000,
            onFileAdded: function (plugin, files) {
                $('.col-contenido .botonera').css('display', 'block');
            },
            onFileRemoved: function (plugin, files) {
                $('.col-contenido .botonera').css('display', 'none');
            }
        });
		$('.btProcesarCV').off('click').on('click', function(e) {
			window.addEventListener('beforeunload', preventBeforeUnload);
            e.preventDefault();
			that.cargarCV();
		});

		$('.btImportarCV').off('click').on('click', function(e) {
			e.preventDefault();
			//Compruebo que el usuario ha elegido una opción.
			if(CheckSelectorIgnored())
			{
				return;
			}
			
			
			var listaId = "";
			var listaOpcionSeleccionados = "";
			$('.resource-list .custom-control-input:checkbox:checked').each(function(){
				listaId += (this.checked ? $(this).val()+"@@@" : "")
			});
			$('.resource-list .custom-control-input:checkbox:checked').closest('.resource.success').find(':selected').each(function(){
				listaOpcionSeleccionados += (this.selected ? $(this).closest(".resource.success").find(":checked").val() + "|||" + $(this).val()+"@@@" : "")
			});
			
			listaId = listaId.slice(0,-3);			
			listaOpcionSeleccionados = listaOpcionSeleccionados.slice(0,-3);
			
			that.importarCV(listaId, listaOpcionSeleccionados);
		});
    },
	//Carga los datos del CV para la exportacion
    cargarCV: function() {
		if($('#file_cvn')[0].files[0]==null){
			OcultarUpdateProgress();
			alert("No hay un fichero adjuntado");
			return;
		}
		
		$('#informacionImportacion').hide();
		$('.col-contenido.paso1').hide();
		$('.col-contenido.paso2').show();

		$('#CV_select_all').off('click').on('click', function(e) {
			e.preventDefault();
            checkAllWrappersCV(true);
		});
		$('#CV_unselect_all').off('click').on('click', function(e) {
			e.preventDefault();
            checkAllWrappersCV(false);
		});
		$('.h1-container .mostrarTodos').off('click').on('click', function(e) {
			e.preventDefault();
            dropdownVisibilityCV('mostrarTodos');
		});
		$('.h1-container .mostrarSimilitudes').off('click').on('click', function(e) {
			e.preventDefault();
            dropdownVisibilityCV('mostrarSimilitudes');
		});
		$('.h1-container .mostrarNuevos').off('click').on('click', function(e) {
			e.preventDefault();
            dropdownVisibilityCV('mostrarNuevos');
		});

		MostrarUpdateProgressTime(0);
		
		if($('#titleMascaraBlanca').length == 0){
			$('#mascaraBlanca').find('.wrap.popup').append('<br><div id="titleMascaraBlanca"></div>');
			$('#mascaraBlanca').find('.wrap.popup').append('<div id="workMascaraBlanca"></div>');
			$('#mascaraBlanca').find('.wrap.popup').append('<div id="subworkMascaraBlanca"></div>');
		}
	
		var that=this;
		var formData = new FormData();
		var petition = RandomGuid();
		formData.append('userID', that.idUsuario);
		formData.append('petitionID', petition);
		formData.append('File', $('#file_cvn')[0].files[0]);
		
		//Actualizo el estado cada 500 milisegundos
		var intervalStatus = setInterval(function() {
			$.ajax({
				url: urlImportacionCV + 'ImportarCVStatus?petitionID='+petition+'&accion=PREIMPORTAR',
				type: 'GET',
				success: function ( response ) {
					if(response != null && response != ''){
						if(response.subActualWork > response.subTotalWorks){
							response.subActualWork = response.subTotalWorks;
						}
						if(response.actualSubWorks > response.actualSubTotalWorks){
							response.actualSubWorks = response.actualSubTotalWorks;
						}
						
						if(response.subTotalWorks == null || response.subTotalWorks == 0){
							$('#titleMascaraBlanca').text(`${GetText(response.actualWorkTitle)}`);
						}
						else{
							$('#titleMascaraBlanca').text(`${GetText(response.actualWorkTitle)}` + " (" +  response.subActualWork + '/' + response.subTotalWorks + ")");
							
							if(response.actualWorkSubtitle != null && response.actualSubWorks != null && response.actualSubTotalWorks){
								$('#subworkMascaraBlanca').text(`${GetText(response.actualWorkSubtitle)}` + " (" +  response.actualSubWorks + '/' + response.actualSubTotalWorks + ")");
							}
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
			url: urlImportacionCV + 'PreimportarCV',
			type: 'POST',
			data: formData,	
			cache: false,
			processData: false,
            enctype: 'multipart/form-data',
            contentType: false,
			success: function ( response ) {				
				clearInterval(intervalStatus);
				$('#informacionImportacion').show();
				$('#titleMascaraBlanca').remove();
				$('#workMascaraBlanca').remove();
				$('#subworkMascaraBlanca').remove();
				for(var i=0;i<7;i++){
					var id = 'x' + RandomGuid();
					var dropdowns = '';
					if(i!=0 && i!=6 && i!=7){
						dropdownSelectorSeccion = `
						<div class="seleccionar dropdown dropdown-select seccion">
							<a class="dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
										<span class="material-icons">done_all</span>										
									</a>
									<div class="dropdown-menu basic-dropdown dropdown-menu-right">
										<a class="item-dropdown seleccionar"><span class="texto">${GetText('CV_SELECCIONAR_TODOS')}</span></a>
										<a class="item-dropdown deseleccionar"><span class="texto">${GetText('CV_DESELECCIONAR_TODOS')}</span></a>
									</div>
								</div>`;

						dropdownMostrarSeccion = `
						<div class="seleccionar dropdown dropdown-select seccion">
							<a class="dropdown-toggle" data-toggle="dropdown" aria-expanded="false" >
								<span class="material-icons">preview</span>	
							</a>
							<div class="dropdown-menu basic-dropdown dropdown-menu-right">
								<a class="item-dropdown mostrarTodos"><span class="texto">${GetText('CV_MOSTRAR_TODOS')}</span></a>
								<a class="item-dropdown mostrarSimilitudes"><span class="texto">${GetText('CV_MOSTRAR_SIMILITUDES')}</span></a>
								<a class="item-dropdown mostrarNuevos"><span class="texto">${GetText('CV_MOSTRAR_NUEVOS')}</span></a>
							</div>
						</div>`;
						dropdowns = `${dropdownSelectorSeccion} ${dropdownMostrarSeccion}`;
					}
					else
					{
						dropdowns = ``;
					}
					var contenedorTab=`<div class="panel-group pmd-accordion" id="datos-accordion${i}" role="tablist" aria-multiselectable="true">
											<div class="panel">
												<div class="panel-heading" role="tab" id="datos-tab">
													<p class="panel-title">
														<a data-toggle="collapse" data-parent="#datos-accordion${i}" href="#${id}" aria-expanded="true" aria-controls="datos-tab" data-expandable="false" class="">
															<span class="texto">${response[i].title}</span>
															<span class="material-icons pmd-accordion-arrow">keyboard_arrow_up</span>
														</a>
													</p>
												</div>
												<div id="${id}" class="collapse show">
													<div class="row cvTab">
														${dropdowns}
														<div class="col-12 col-contenido">
														</div>
													</div>
												</div>
											</div>
										</div>`;
					if(i==0){
						$('.contenido-cv').append( $(contenedorTab));
						var html = edicionCV.printPersonalData(id, response[i]);
						$('div[id="' + id + '"] .col-12.col-contenido').append(html);
						$('#'+id+' input[type="checkbox"]').prop('checked',true);
					}else if(i == 6){
						$('.contenido-cv').append( $(contenedorTab));		
						var html = printFreeText(id, response[i]);
						$('div[id="' + id + '"] .col-12.col-contenido').append(html);
					}else{
						$('.contenido-cv').append( $(contenedorTab));
						edicionCV.printTab(id, response[i]);
					}
				};
				that.fileData = response[99].title;
				that.filePreimport = response[100].title;
				
				
				checkAllCVWrapper();
				checkAllConflict();
				checkUniqueItems();
				aniadirComportamientoWrapperSeccion();
				aniadirTooltipsConflict();
				
				
				window.removeEventListener('beforeunload', preventBeforeUnload);
				
				
				
				OcultarUpdateProgress();
			},
			error: function(jqXHR, exception){
				clearInterval(intervalStatus);				
				$('#titleMascaraBlanca').remove();
				$('#workMascaraBlanca').remove();
				$('#subworkMascaraBlanca').remove();
				window.removeEventListener('beforeunload', preventBeforeUnload);
				
				var msg = '';
				if (jqXHR.status === 0) {
					msg = 'Not connect.\n Verify Network.';
				} else if (jqXHR.status == 404) {
					msg = 'Requested page not found. [404]';
				} else if (jqXHR.status == 500) {
					msg = 'Internal Server Error [500].';
				} else if (exception === 'parsererror') {
					msg = 'Requested JSON parse failed.';
				} else if (exception === 'timeout') {
					msg = 'Time out error.';
				} else if (exception === 'abort') {
					msg = 'Ajax request aborted.';
				} else {
					msg = 'Uncaught Error.\n' + jqXHR.responseText;
				}
				alert(msg);
			}
		});		
				
		return;
    },
	importarCV: function(listaId, listaOpcionSeleccionados) {
		MostrarUpdateProgressTime(0);
		var that = this;
		var formData = new FormData();
		var petition = RandomGuid();
		formData.append('userID', that.idUsuario);
		formData.append('petitionID', petition);
		formData.append('fileData', that.fileData);
		formData.append('filePreimport', that.filePreimport);
		formData.append('listaId', listaId);
		formData.append('listaOpcionSeleccionados', listaOpcionSeleccionados);
		
		if($('#titleMascaraBlanca').length == 0){
			$('#mascaraBlanca').find('.wrap.popup').append('<br><div id="titleMascaraBlanca"></div>');
			$('#mascaraBlanca').find('.wrap.popup').append('<div id="workMascaraBlanca"></div>');
			$('#mascaraBlanca').find('.wrap.popup').append('<div id="subworkMascaraBlanca"></div>');
		}
		
		//Actualizo el estado cada 500 milisegundos
		var intervalStatus = setInterval(function() {
			$.ajax({
				url: urlImportacionCV + 'ImportarCVStatus?petitionID='+petition+'&accion=POSTIMPORTAR',
				type: 'GET',
				success: function ( response ) {
					if(response != null && response != ''){
						if(response.subActualWork > response.subTotalWorks){
							response.subActualWork = response.subTotalWorks;
						}
						if(response.actualSubWorks > response.actualSubTotalWorks){
							response.actualSubWorks = response.actualSubTotalWorks;
						}
						
						if(response.subTotalWorks == null || response.subTotalWorks == 0){
							$('#titleMascaraBlanca').text(`${GetText(response.actualWorkTitle)}`);							
						}
						else{
							$('#titleMascaraBlanca').text(`${GetText(response.actualWorkTitle)}` + " (" +  response.subActualWork + '/' + response.subTotalWorks + ")");
							
							if(response.actualWorkSubtitle != null && response.actualSubWorks != null && response.actualSubTotalWorks){
								$('#subworkMascaraBlanca').text(`${GetText(response.actualWorkSubtitle)}` + " (" +  response.actualSubWorks + '/' + response.actualSubTotalWorks + ")");
							}
						}
						//Si no hay pasos maximos no muestro la lista
						if(response.totalWorks != 0){
							$('#workMascaraBlanca').text(response.actualWork + '/' + response.totalWorks);
						}
					}
				}
			});	
		}, 500);
		
		$.ajax({
			url: urlImportacionCV + 'PostimportarCV',
			type: 'POST',
			data: formData,
			cache: false,
			processData: false,
            enctype: 'multipart/form-data',
            contentType: false,
			success: function ( response ) {				
				clearInterval(intervalStatus);
				$('#titleMascaraBlanca').remove();
				$('#workMascaraBlanca').remove();
				$('#subworkMascaraBlanca').remove();
				
				OcultarUpdateProgress();
				if(urlUserCV != null && urlUserCV != '')
				{
					window.location.href = urlUserCV;
				}
			},
			error: function(jqXHR, exception){				
				clearInterval(intervalStatus);
				$('#titleMascaraBlanca').remove();
				$('#workMascaraBlanca').remove();
				$('#subworkMascaraBlanca').remove();
				
				var msg = '';
				if (jqXHR.status === 0) {
					msg = 'Not connect.\n Verify Network.';
				} else if (jqXHR.status == 404) {
					msg = 'Requested page not found. [404]';
				} else if (jqXHR.status == 500) {
					msg = 'Internal Server Error [500].';
				} else if (exception === 'parsererror') {
					msg = 'Requested JSON parse failed.';
				} else if (exception === 'timeout') {
					msg = 'Time out error.';
				} else if (exception === 'abort') {
					msg = 'Ajax request aborted.';
				} else {
					msg = 'Uncaught Error.\n' + jqXHR.responseText;
				}
				alert(msg);
			}
		});
		
	}
};

//True si algun selector no ha cambiado el estado por defecto y está checkeado, false en caso contrario
function CheckSelectorIgnored(){
	var articlesSelected = $("article select:not(:disabled)[name='itemConflict'] option:selected");
	for(var i = 0; i < articlesSelected.length; i++){
		if(articlesSelected[i].value=='ig'){
			mostrarNotificacion("warning", "Alguno de los ítems no ha sido seleccionado");
			return true;
		}
	}
	return false;
}

function changeSelector(selectItem, optionSelected){
	var selectOption = selectItem.closest('article.resource').find('select').prop('disabled');
	var selectIgnored = selectItem.closest('article.resource').find('.wrap option:selected').val();
	if(optionSelected != null){
		selectOption = optionSelected;
	}
	if(selectIgnored!="ig"){
		selectItem.closest('article.resource').find('.wrap select').val(0).change();
	}
	var selector = selectItem.closest('article.resource').find('select');
	selector.prop('disabled', !selectOption);
}

function dropdownVisibilityCV(tipo){
	var dropdownVisibility = $('.seleccionar.dropdown.dropdown-select.seccion .dropdown-menu.basic-dropdown.dropdown-menu-right .'+tipo);
	dropdownVisibility.click();
}

function changeUniqueItem(opcion, itemConflict){
	if(opcion == 'ig')
	{
		itemConflict.find('option[value="so"]').attr("selected", true);
		itemConflict.find('option[value="ig"]').attr("selected", false);
	}
	else if(opcion == 'so')
	{
		itemConflict.find('option[value="so"]').attr("selected", false);
		itemConflict.find('option[value="ig"]').attr("selected", true);
	}
}

function checkUniqueItems(){
	$('.uniqueItemConflict').closest('.resource').find('input[type="checkbox"]').off('click').on('click', function(e){		
		var itemConflict = $(this).closest('.resource').find('.uniqueItemConflict');
		var seleccion = $(this).closest('.resource').find('.uniqueItemConflict option:selected').val();
		changeUniqueItem(seleccion, itemConflict);
	});
}

function checkAllWrappersCV(check){
	var wrappersChecked = $('.checkAllCVWrapper input[type="checkbox"]:checked');
	var wrappersUnchecked = $('.checkAllCVWrapper input[type="checkbox"]:not(:checked)');
	if(!check){
		for(var i = 0; i< wrappersChecked.length; i++)
		{
			$(wrappersChecked[i]).click();
		}
		$('.uniqueItemConflict').each(function(){
			$(this).closest('article').find('input[type="checkbox"]:checked').click();
			$(this).find('option[value="so"]').attr("selected", false);
			$(this).find('option[value="ig"]').attr("selected", true);
		});
		$('input[type=checkbox]').prop('checked', false);
	}
	else{
		for(var i = 0; i< wrappersUnchecked.length; i++)
		{
			$(wrappersUnchecked[i]).click();
		}
		$('.uniqueItemConflict').each(function(){
			$(this).closest('article').find('input[type="checkbox"]:not(:checked)').click();
			$(this).find('option[value="ig"]').attr("selected", false);
			$(this).find('option[value="so"]').attr("selected", true);
		});
	}
}

function aniadirComportamientoWrapperSeccion(){	
	$('.seleccionar.dropdown.dropdown-select.seccion .dropdown-menu.basic-dropdown.dropdown-menu-right .seleccionar').off('click').on('click', function(e) {
		e.preventDefault();
		checkAllWrappersSection(true, $(this).closest('.row.cvTab'));
	});
	$('.seleccionar.dropdown.dropdown-select.seccion .dropdown-menu.basic-dropdown.dropdown-menu-right .deseleccionar').off('click').on('click', function(e) {
		e.preventDefault();
		checkAllWrappersSection(false, $(this).closest('.row.cvTab'));
	});
	$('.seleccionar.dropdown.dropdown-select.seccion .dropdown-menu.basic-dropdown.dropdown-menu-right .mostrarTodos').off('click').on('click', function(e) {
		e.preventDefault();
		wrapperVisibilitySection("mostrarTodos", $(this).closest('.row.cvTab'));
	});
	$('.seleccionar.dropdown.dropdown-select.seccion .dropdown-menu.basic-dropdown.dropdown-menu-right .mostrarSimilitudes').off('click').on('click', function(e) {
		e.preventDefault();
		wrapperVisibilitySection("mostrarSimilitudes", $(this).closest('.row.cvTab'));
	});
	$('.seleccionar.dropdown.dropdown-select.seccion .dropdown-menu.basic-dropdown.dropdown-menu-right .mostrarNuevos').off('click').on('click', function(e) {
		e.preventDefault();
		wrapperVisibilitySection("mostrarNuevos", $(this).closest('.row.cvTab'));
	});
	
};

function aniadirTooltipsConflict(){
	$('select[name="itemConflict"]').tooltip({
		html: true,
		placement: 'left',
		template: '<div class="tooltip background-gris-oscuro" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
		title: GetText('IGNORAR_TOOLTIP')
	});
	$('select[name="itemConflict"]').off('change.toolt').on('change.toolt', function(e) {
		e.preventDefault();
		// Cambio el tooltip en función del valor seleccionado
		var valorSeleccionado = $(this).find('option:selected').val();
		if (valorSeleccionado == 'ig') {
			$(this).attr('data-original-title', GetText('IGNORAR_TOOLTIP'));
		} else if (valorSeleccionado == 'fu') {
			$(this).attr('data-original-title', GetText('FUSIONAR_TOOLTIP'));
		} else if (valorSeleccionado == 'so') {
			$(this).attr('data-original-title', GetText('SOBREESCRIBIR_TOOLTIP'));
		} else if (valorSeleccionado == 'du') {
			$(this).attr('data-original-title', GetText('DUPLICAR_TOOLTIP'));
		}
		$(this).tooltip('hide');
	});
};

function wrapperVisibilitySection(opcion, section){
	var items = section.find('.panel-collapse.collapse .ordenar.dropdown.dropdown-select .'+opcion);
	for(var i = 0; i < items.length ; i++){
		items[i].click();
	}
}

function checkAllWrappersSection(toCheck, section){
	var wrappersChecked = section.find('.checkAllCVWrapper input[type="checkbox"]:checked');
	var wrappersUnchecked = section.find('.checkAllCVWrapper input[type="checkbox"]:not(:checked)');	
	checkAllCVWrapper();
	aniadirTooltipsConflict();
	
	//Si quiero añadir checks
	if(toCheck){		
		for(var i = 0; i< wrappersUnchecked.length; i++)
		{
			wrappersUnchecked[i].click();
		}		
	}
	//Si quiero quitar los checks
	else
	{		
		for(var i = 0; i< wrappersChecked.length; i++)
		{		
			wrappersChecked[i].click();
		}
		
		var inputsChecked = section.find('article input[type="checkbox"]:checked');
		for(var i=0; i<inputsChecked.length; i++){
			inputsChecked[i].click();
		}
	}
	
};

function checkAllConflict(){
	$('.ordenar.dropdown.dropdown-select a.item-dropdown').off('click').on('click', function(e) {
		//Indico el tipo de elección
		var tipo = "";
		if($(this).hasClass('mostrarTodos')){
			tipo = "TODOS";
		}
		else if($(this).hasClass('mostrarSimilitudes')){
			tipo = "SIMILITUDES";
		}
		else if($(this).hasClass('mostrarNuevos')){
			tipo = "NUEVOS";
		}		
		
		var dropdownText = $(this).closest('.ordenar.dropdown.dropdown-select').find('a.dropdown-toggle span.texto');		
		var seccion = $(this).closest('.panel-group.pmd-accordion').attr("section");
		var seleccionar = $(this).closest('.acciones-listado').find('.checkAllCVWrapper input[type="checkbox"]');
		
		// Cambio el texto del checkbox de seleccionar en función de los datos mostrados
		$(seleccionar).prop('checked', false);
		//Edito el nombre mostrado
		$(seleccionar).closest('.custom-control').find('.custom-control-label').text(`${GetText('CV_SELECCIONAR_' + tipo)}`);
		
		//Elimino las posibles clases de seleccion
		seleccionar.removeClass('mostrarTodos');
		seleccionar.removeClass('mostrarSimilitudes');
		seleccionar.removeClass('mostrarNuevos');
		
		if(tipo=='TODOS')
		{
			seleccionar.attr('conflict', '');
			seleccionar.addClass('mostrarTodos');
			dropdownText.text(GetText('CV_MOSTRAR_TODOS'));
			edicionCV.buscarListado(seccion, false, false);	
		}
		else if(tipo=='SIMILITUDES')
		{
			seleccionar.attr('conflict', 'true');
			seleccionar.addClass('mostrarSimilitudes');
			dropdownText.text(GetText('CV_MOSTRAR_SIMILITUDES'));
			edicionCV.buscarListado(seccion, true, false);
		}
		else if(tipo=='NUEVOS')
		{
			seleccionar.attr('conflict', 'false');
			seleccionar.addClass('mostrarNuevos');
			dropdownText.text(GetText('CV_MOSTRAR_NUEVOS'));
			edicionCV.buscarListado(seccion, false, true);
		}
		checkAllCVWrapper();
		aniadirTooltipsConflict();
	});	
};

function checkAllCVWrapper(){	
	$('article.resource input.custom-control-input').off('click').on('click', function(e) {
		//e.preventDefault();
		changeSelector($(this));
	});
				
	$('.checkAllCVWrapper input[type="checkbox"]').off('click').on('click', function(e) {
		var conflictType = $(this).attr('conflict') ? '.conflict-' + $(this).attr('conflict') : '';
		var tipo = "";
		if($(this).hasClass('mostrarTodos'))
		{
			tipo = "TODOS";
		}
		else if($(this).hasClass('mostrarSimilitudes')){
			tipo = "SIMILITUDES";
		}
		else if($(this).hasClass('mostrarNuevos')){
			tipo = "NUEVOS";
		}
				
		if(!$(this)[0].checked)
		{
			$(this).closest('.custom-control').find('.custom-control-label').text(`${GetText('CV_SELECCIONAR_' + tipo)}`);
		}
		else
		{
			$(this).closest('.custom-control').find('.custom-control-label').text(`${GetText('CV_DESELECCIONAR_' + tipo)}`);
		}
		
		$(this).closest('.panel-body').find('article' + conflictType + ' div.custom-checkbox input[type="checkbox"]').prop('checked',$(this).prop('checked'));
		changeSelector($(this).closest('.panel-body').find('article' + conflictType + ' div.custom-checkbox input[type="checkbox"]'),$(this)[0].checked);
	});
	
	$('.checkAllCVWrapper input[type="checkbox"]').closest('.panel-body').find('article div.custom-checkbox input[type="checkbox"]').off('change').on('change', function(e) {
		var tipo = "";
		if($(this).closest('.panel-body').find('.checkAllCVWrapper input').hasClass('mostrarTodos'))
		{
			tipo = "TODOS";
		}
		else if($(this).closest('.panel-body').find('.checkAllCVWrapper input').hasClass('mostrarSimilitudes')){
			tipo = "SIMILITUDES";
		}
		else if($(this).closest('.panel-body').find('.checkAllCVWrapper input').hasClass('mostrarNuevos')){
			tipo = "NUEVOS";
		}
		
		if(!$(this).prop('checked')){
			$(this).closest('.panel-body').find('.checkAllCVWrapper input[type="checkbox"]').prop('checked', false);
			$(this).closest('.panel-body').find('.checkAllCVWrapper label').text(`${GetText('CV_SELECCIONAR_' + tipo)}`);			
		}
	});
};

function printCientificProduction(id, data){
	//Pintado sección listado
	//css mas generico
	var id = 'x' + RandomGuid();
	var id2 = 'x' + RandomGuid();

	var expanded = "";
	var show = "";
	var datos = false;
	if (data.items != null) {
		if (Object.keys(data.items).length > 0) {
			//Desplegado
			expanded = "true";
			show = "show";
		} else {
			//No desplegado	
			expanded = "false";
		}
		for(const seccion in data.items){
			if(data.items[seccion].properties[0].values.length != 0){
				datos = true;
			}
		}
		contador=0;
		for(const seccion in data.items){
			
			if(datos){
				//TODO texto ver items
				var htmlSection = `
				<div class="panel-group pmd-accordion" section="${data.items[seccion].properties[0]}" id="${id}" role="tablist" aria-multiselectable="true">
					<div class="panel">
						<div class="panel-heading" role="tab" id="publicaciones-tab">
							<p class="panel-title">
								<a data-toggle="collapse" data-parent="#${id}" href="#${id2}" aria-expanded="${expanded}" aria-controls="${id2}" data-expandable="false">
									<span class="material-icons pmd-accordion-icon-left">folder_open</span>
									<span class="texto">${data.items[seccion].title}</span>
								</a>
							</p>
						</div>`;
						if(data.items[seccion].properties[0].values.length != 0){
						htmlSection += `
						<div id="${id2}" class="panel-collapse collapse ${show}" role="tabpanel">
							<div id="situacion-panel" class="panel-collapse collapse show" role="tab-panel" aria-labelledby="situacion-tab" style="">
								<div class="panel-body">
									<div class="resource-list listView">
										<div class="resource-list-wrap">
											<article class="resource success" >
												<div class="custom-control custom-checkbox">
													<input type="checkbox" class="custom-control-input" id="check_resource_${data.items[seccion].identifier}"  value="${data.items[seccion].identifier}_${contador}" checked>
													<label class="custom-control-label" for="check_resource_${data.items[seccion].identifier}"></label>
												</div>
												<div class="wrap">
													<div class="middle-wrap">
														<div class="title-wrap">
															<h2 class="resource-title">${GetText('CV_INDICADORES_GENERALES')}</h2>`
															+selectorCamposTexto+														
														`</div>
													</div>
												</div>
											</article>
										</div>
									</div>
								</div>
							</div>
						</div>`;
						
						contador++;
						}
				htmlSection += `
					</div>
				</div>`;
			}
		}
		return htmlSection;
	}
	return '';
}
	
function printFreeText(id, data){
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
											<div id="freeTextListArticle" class="resource-list-wrap">`;
		var secciones = data.sections[0].items;
		contador=0;
		for (const seccion in secciones){
			//Si no hay datos no pinto esa sección
			if(secciones[seccion].properties[0].values.length > 0 && secciones[seccion].properties[0].values[0].length > 0){
				var id = 'x' + RandomGuid();
				var valorSeccion = '';
				if(secciones[seccion].properties[0].values[0]!= null ){
					valorSeccion = secciones[seccion].properties[0].values[0];
				}
				var html2 = `<article class="resource success">
								<div class="custom-control custom-checkbox">
									<input type="checkbox" class="custom-control-input" id="check_resource_${secciones[seccion].identifier}_${contador}"  value="${secciones[seccion].identifier}_${contador}" checked>
									<label class="custom-control-label" for="check_resource_${secciones[seccion].identifier}_${contador}"></label>
								</div>
								<div class="wrap">
									<div class="middle-wrap">
										<div class="title-wrap">
											<h2 class="resource-title">${secciones[seccion].title}</h2>`
											+selectorCamposTexto+`
											<!--span class="material-icons arrow">keyboard_arrow_down</span-->
										</div>	
										<div class="content-wrap">
											<div class="description-wrap">
												<div class="group">
													<p>${valorSeccion}</p>
												</div>
											</div>
										</div>
									</div>
								</div>
							</article>`;
				html += html2;
			}
			contador++;
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
	
edicionCV.printTab= function(entityID, data) {
	var that=this;
	
	for (var i = 0; i < data.sections.length; i++) {	
		if(data.sections[i].title=="Indicadores generales de calidad de la producción científica")
		{
			$('div[id="' + entityID + '"] .col-12.col-contenido').append(printCientificProduction(entityID, data.sections[i]));
		}
		else
		{			
			$('div[id="' + entityID + '"] .col-12.col-contenido').append(this.printTabSection(data.sections[i]));
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
		contador=0;	
		for (const seccion in data.sections[0].items)
		{
			for(var i =0; i<data.sections[0].items[seccion].properties.length; i++){
				if(data.sections[0].items[seccion].properties[i].values[0] != null){
					nombre += data.sections[0].items[seccion].properties[i].values[0];
					nombre += " ";
				}
			}
			
			var html = `<div class="panel-group pmd-accordion collapse show" section="${data.sections[0].items[seccion].title}" id="${id}" role="tablist" aria-multiselectable="true">
							<div class="panel">
								<div class="panel-heading" role="tab" id="publicaciones-tab">
									<p class="panel-title">
										<a data-toggle="collapse" data-parent="#${id}" href="#${id2}" aria-expanded="${expanded}" aria-controls="${id2}" data-expandable="false">
											<span class="material-icons pmd-accordion-icon-left">folder_open</span>
											<span class="texto">${data.title}</span>
											<span class="material-icons pmd-accordion-arrow">keyboard_arrow_up</span>
										</a>
									</p>
								</div>`;
							if(data.sections[0].items[seccion].properties[0].values.length!=0)
							{
							html+=`
								<div id="${id2}" class="panel-collapse collapse ${show}" role="tabpanel">
									<div id="situacion-panel" class="panel-collapse collapse show" role="tab-panel" aria-labelledby="situacion-tab" style="">
										<div class="panel-body">
											<div class="resource-list listView">
												<div class="resource-list-wrap">
													<article class="resource success" >
														<div class="custom-control custom-checkbox">
															<input type="checkbox" class="custom-control-input" id="check_resource_${data.sections[0].items[seccion].identifier}"  value="${data.sections[0].items[seccion].identifier}_${contador}">
															<label class="custom-control-label" for="check_resource_${data.sections[0].items[seccion].identifier}"></label>
														</div>
														<div class="wrap">
															<div class="middle-wrap">
																<div class="title-wrap">
																	<h2 class="resource-title">${GetText('CV_DATOS_IDENTIFICACION')}</h2>`
																	+selectorCamposTexto+
																`</div>
																<div class="content-wrap">
																	<div class="description-wrap">
																		<p>${nombre}</p>
																	</div>
																</div>
															</div>
														</div>
													</article>
												</div>
											</div>
										</div>
									</div>
							</div>`;
							contador++;
							}
						html += `
							</div>
						</div>`;
			 return html;
		 }
	}
};

edicionCV.printTabSection= function(data) {
	//Pintado sección listado
	//css mas generico
	var id = 'x' + RandomGuid();
	var id2 = 'x' + RandomGuid();

	var expanded = "";
	var show = "";
	if (data.items != null) {
		contador=0;
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
											<input type="checkbox" class="custom-control-input mostrarTodos" id="checkAllResources_${id2}">
											<label class="custom-control-label" for="checkAllResources_${id2}">` + GetText("CV_SELECCIONAR_TODOS") + `</label>
										</div>
									</div>
								</div>
								<div class="wrap">
									${dropdownSimilitudes}
									<div class="ordenar dropdown orders">${this.printOrderTabSection(data.orders)}</div>
									<div class="buscador">
										<div class="fieldsetGroup searchGroup">
											<div class="textoBusquedaPrincipalInput">
												<input type="text" class="not-outline txtBusqueda" placeholder="` + GetText('CV_ESCRIBE_ALGO') + `" autocomplete="off">
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
									${this.printHtmlListItems(data.items)}
									<div class="panNavegador">
										<div class="items dropdown">
											<a class="dropdown-toggle" data-toggle="dropdown" aria-expanded="true">
												<span class="texto" items="5">Ver 5 items</span>
											</a>
											<div class="dropdown-menu basic-dropdown dropdown-menu-right" x-placement="bottom-end">
												<a  class="item-dropdown" items="5">Ver 5 items</a>
												<a  class="item-dropdown" items="10">Ver 10 items</a>
												<a  class="item-dropdown" items="20">Ver 20 items</a>
												<a  class="item-dropdown" items="50">Ver 50 items</a>
												<a  class="item-dropdown" items="100">Ver 100 items</a>
											</div>
										</div>
										<nav>
											<ul class="pagination arrows">
											</ul>
											<ul class="pagination numbers">	
												<li class="actual"><a  page="1">1</a></li>
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

edicionCV.printHtmlListItem= function(id, data) {
	let openAccess="";
	let isCheck ="";
	let isConflict = false;
	let isBlockedFE = false;
	if (data.isopenaccess) 
	{
		openAccess = "open-access";
	}
	if(data.idBBDD == null || data.idBBDD == '')
	{
		isCheck = "checked";
	}
	if(data.idBBDD != "")
	{
		isConflict = true;
	}
	else
	{
		isConflict = false;
	}
	
	var htmlListItem = ``;
	if(data.title!= null){
		htmlListItem = `<article class="resource success ${openAccess} conflict-${isConflict}" >
							<div class="custom-control custom-checkbox">
								<input type="checkbox" class="custom-control-input" id="check_resource_${id}" value="${id}_${contador}" ${isCheck}>
								<label class="custom-control-label" for="check_resource_${id}"></label>
							</div>
							<div class="wrap">
								<div class="middle-wrap">
									${this.printHtmlListItemOrders(data)}
									<div class="title-wrap">
										<h2 class="resource-title">${data.title}</h2>
										${this.printHtmlListItemEditable(data)}	`;
		if(data.idBBDD != ""){
			if(data.iseditable){
				htmlListItem += selectorConflictoNoBloqueado;
			}else{
				htmlListItem += selectorConflictoBloqueado;
			}	
		}
		else 
		{
			htmlListItem += `<span class="material-icons-outlined new">fiber_new</span>`;
		}
		
		htmlListItem += `<span class="material-icons arrow">keyboard_arrow_down</span>
									</div>
									<div class="content-wrap">
										<div class="description-wrap">
											${this.printHtmlListItemPropiedades(data)}
										</div>
									</div>
								</div>
							</div>
						</article>`;
	}
	contador++;
	return htmlListItem;
};
