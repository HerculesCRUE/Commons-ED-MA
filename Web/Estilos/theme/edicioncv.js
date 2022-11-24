var urlEdicionCV = url_servicio_editorcv + "EdicionCV/";
var urlImportacionCV = url_servicio_editorcv+"ImportadoCV/";
var urlEnvioDSpaceCV = url_servicio_editorcv + "EnvioDSpace/";
var urlEnvioValidacionCV = url_servicio_editorcv + "EnvioValidacion/";
var urlGuardadoCV = url_servicio_editorcv + "GuardadoCV/";
var languages = ['en', 'ca', 'eu', 'gl', 'fr'];
var tooltips = { section: {}, items: {} };

var edicionCV = {
	idCV: null,
	idPerson: null,
	init: async function () {
		this.idCV = $('.contenido-cv').attr('about');
		this.idPerson = $('.contenido-cv').attr('personid');
		if(await this.importacion()){
			$("#modal-warning-importacion").modal("show");
			$("#modal-warning-importacion").on("hidden.bs.modal",(e)=>{
				this.config();
				duplicadosCV.init();
			})
			return;
		}
		this.config();
		duplicadosCV.init();
		return;
	},
	importacion: async function(){
		var url = urlImportacionCV +"FechaCheck"
		var formData = new FormData();
		var pCVID = $("#recurso_id").val();
		formData.append("pCVID",pCVID)
		var importacion = false;
		await $.ajax({
			url: url,
			type: 'POST',
			data: formData,
			cache: false,
			processData: false,
			enctype: 'multipart/form-data',
			contentType: false,
			success: function (response) {
				if (response) {
					if(response === 'true'){
						importacion = true;
					}
				}
			}
		});
		return importacion;

	},
	config: function () {
		// Quito los tooltips del panel de navegación
		$('#navegacion-cv li.nav-item a').tooltip('dispose');

		$('*').on('shown.bs.modal', function (e) {
			$('.modal-backdrop').last().addClass($(this).attr('id'));
		});

		//Carga de secciones principales
		var that = this;
		$('.cabecera-cv .h1-container .dropdown-menu a').click(function (e) {
			$($(this).attr('href')).click();
		});
		$('#navegacion-cv li.nav-item a').click(function (e) {
			var entityID = $($(this).attr('href')).find('.cvTab').attr('about');
			var rdfType = $($(this).attr('href')).find('.cvTab').attr('rdftype');
			that.loadTab(entityID, rdfType);
		});

		if (getParam('tab') != null) {
			$('a.nav-link[property="' + getParam('tab') + '"]').click();
		} else {
			$('#identificacion-tab').click();
		}



		return;
	},
	//Métodos de pestañas
	loadTab: function (entityID, rdfType) {
		var that = this;
		$('div[about="' + entityID + '"] .col-12.col-contenido').empty();
		MostrarUpdateProgress();

		$.get(urlEdicionCV + 'GetTab?pCVId=' + that.idCV + '&pId=' + entityID + "&pRdfType=" + rdfType + "&pLang=" + lang + "&pSection=0", null, function (data) {
			that.printTab(entityID, data);
			if (!$('div#modal-posible-duplicidad').hasClass('visible')) {
				OcultarUpdateProgress();
			}
			for (var key in tooltips.section) {
				var value = tooltips.section[key];
				$(key).tooltip({
					html: true,
					placement: 'bottom',
					template: '<div class="tooltip background-gris-oscuro infoTooltipMargin" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner tooltipEditor"></div></div>',
					title: value
				});
				montarTooltip.comportamiento($(key));
			}
			if (getParam('tab') != null) {
				if (getParam('section') != null && getParam('id') == null) {
					//Abrimos creacion
					$('div[section="' + getParam('section') + '"] a.aniadirEntidad').click();
					//reseteamos la url
					history.pushState(null, '', document.location.origin + document.location.pathname);
				}
				if (getParam('section') != null && getParam('id') != null) {
					//Si existe el ID abrimos edición
					if ($('a[internal-id="' + getParam('id') + '"]').length) {
						$('a[internal-id="' + getParam('id') + '"]').click();
						history.pushState(null, '', document.location.origin + document.location.pathname);
					} else {
						//Si no existe abrimos la seccion
						$('div.panel-group.pmd-accordion.notLoaded[section="' + getParam('section') + '"]').click();
					}
				}
			}
		});
		return;
	},//Métodos de pestañas
	completeTab: function (entityID, rdfType, section, pOnlyPublic = false) {
		var that = this;
		MostrarUpdateProgress();
		$.get(urlEdicionCV + 'GetTab?pCVId=' + that.idCV + '&pId=' + entityID + "&pRdfType=" + rdfType + "&pLang=" + lang + "&pSection=" + section + "&pOnlyPublic=" + pOnlyPublic, null, function (data) {
			if (rdfType == $('div.tab-pane.active>div.cvTab').attr('rdftype')) {
				that.printTab(entityID, data, section);
				OcultarUpdateProgress();
				for (var key in tooltips.section) {
					var value = tooltips.section[key];
					$(key).tooltip({
						html: true,
						placement: 'bottom',
						template: '<div class="tooltip background-gris-oscuro infoTooltipMargin" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner tooltipEditor"></div></div>',
						title: value
					});
					montarTooltip.comportamiento($(key));
				}
				if (data.sections.length > section + 1) {
					that.completeTab(entityID, rdfType, section + 1);
				}
				if ($('a[internal-id="' + getParam('id') + '"]').length) {
					$('a[internal-id="' + getParam('id') + '"]').click();
					history.pushState(null, '', document.location.origin + document.location.pathname);
				}
			} else if (pOnlyPublic) {
				that.printTabPublic(entityID, data, section);
				OcultarUpdateProgress();
			}
		});
		return;
	},
	printTabPublic: function (entityID, data, section) {
		for (var i = 0; i < data.sections.length; i++) {
			if (data.sections[i].items.length == 0) {
				continue;
			} else if (data.sections[i].identifier == section) {
				$('div[about="' + entityID + '"]').next().find('div[section="' + data.sections[i].identifier + '"]').replaceWith(this.printTabSection(data.sections[i], true));
				$(this).closest('.pmd-accordion').find('.panel-collapse').addClass('show');
				this.repintarListadoTab(data.sections[i].identifier, true);
				accionesPlegarDesplegarModal.init();
				this.engancharComportamientosCV(true);
				$('.resource-title a').removeAttr('href');
				$('div[about="' + entityID + '"]').next().find('div[section="' + data.sections[i].identifier + '"] .panel .panel-collapse').addClass('show')
			}
		}
	},
	printTab: function (entityID, data, section) {
		var that = this;
		if (data.entityID != null) {
			$('div[about="' + entityID + '"] .col-12.col-contenido').append(this.printPersonalData(data));
		} else {
			for (var i = 0; i < data.sections.length; i++) {
				if (section == null) {
					$('div[about="' + entityID + '"] .col-12.col-contenido').append(this.printTabSection(data.sections[i]));
				} else if (data.sections[i].identifier == section) {
					$('div[about="' + entityID + '"] .col-12.col-contenido div[section="' + data.sections[i].identifier + '"]').replaceWith(this.printTabSection(data.sections[i]));
				}
				if (section == null || data.sections[i].identifier == section) {
					if (data.sections[i].items != null) {
						this.repintarListadoTab(data.sections[i].identifier, true);
					} else if (data.sections[i].item != null) {
						that.printSectionItem(data.sections[i].item.idContenedor, data.sections[i].item, data.sections[i].identifier, $('div[about="' + entityID + '"]').attr('rdftype'), data.sections[i].item.entityID);
						//Si no tiene ningun campo valor se repliega
						var plegar = true;
						$('div[section="' + data.sections[i].identifier + '"] input').each(function () {
							if ($(this).val() != '') {
								plegar = false;
							}
						});
						//
						$('div[section="' + data.sections[i].identifier + '"] div.visuell-view').each(function () {
							if ($(this).html() != '') {
								plegar = false;
							}
						});
						if (plegar) {
							$('div[section="' + data.sections[i].identifier + '"] .panel-collapse.collapse').removeClass('show');
							$('div[section="' + data.sections[i].identifier + '"] .panel-heading a').attr('aria-expanded', 'false');
						}
					}
				}
			}
		}
		accionesPlegarDesplegarModal.init();
		this.engancharComportamientosCV();
		this.mostrarTraducciones();
	},
	printSectionItem: function (contenedor, item, identifier, rdftype, entityID) {
		this.printEditItemCV('#' + contenedor, item, identifier, rdftype, entityID);

		var htmlGuardar = `	<div class="form-actions">
							<a class="btn btn-primary">${GetText('CV_GUARDAR')}</a>
						</div>`;
		$('#' + contenedor).append(htmlGuardar);
	}
	//Fin de métodos de pestañas
	, //Métodos de datos personales
	printPersonalData: function (data) {
		var htmlSections = '';
		for (var i = 0; i < data.sections.length; i++) {
			htmlSections += this.printSectionPersonalData(data.sections[i]);
		}
		var html = `	<div class="datos-identificacion  p-4">
						<div class="bloque bloque-title">
							<div class="title">${GetText('CV_DATOSPERSONALES')}</div>
							<div class="actions">
								<ul class="no-list-style d-flex align-items-center">
									<li>
										<a class="btn btn-outline-grey" id="personalDataEdit" personaldataid="${data.entityID}">
											<span class="texto">${GetText('CV_EDITAR')}</span>
											<span class="material-icons">edit</span>
										</a>
									</li>
								</ul>
							</div>
						</div>
						${htmlSections}
					</div>`;
		return html;
	},
	printSectionPersonalData: function (section) {
		var htmlProperties = '';
		for (var i = 0; i < section.rows.length; i++) {
			for (var k = 0; k < section.rows[i].properties.length; k++) {
				htmlProperties += this.printPropertyPersonalData(section.rows[i].properties[k]);
			}
		}
		var html = '';
		if (htmlProperties.trim() != '') {
			html = `	<div class="bloque">
							<p class="pl-4 mb-2 uppercase font-weight-bold">${section.title}</p>
							<div class="table-alike-wrapper">
								<div class="table-alike">
									${htmlProperties}
								</div>
							</div>
						</div>`;
		}
		return html;
	},
	printPropertyPersonalData: function (property) {
		var html = '';
		for (var i = 0; i < property.values.length; i++) {
			if (property.entityAuxData == null) {
				var title = property.title;
				var value = property.values[i];
				if (value != '' && property.type == 'selectCombo') {
					value = property.comboValues[value];
				}
				if (value != '' && property.type == 'date') {
					value = value.substring(6, 8) + "/" + value.substring(4, 6) + "/" + value.substring(0, 4);
				}
				if (value != '') {
					if (property.type == "image") {
						html += `		<div class="item ">
											<div class="item-content">
												<div class="item-title">
													${title}
												</div>
												<div class="item-data">
												&nbsp;
												</div>
												<div class="user-miniatura">
													<div class="imagen-usuario-wrap">
														<div class="imagen">
															<span style="background-image: url('${value}')"></span>
														</div>
													</div>
												</div>
											</div>
										</div>`;
					} else {
						html += `		<div class="item ">
											<div class="item-content">
												<div class="item-title">
													${title}
												</div>
												<div class="item-data">
													${value}
												</div>
											</div>
										</div>`;
					}
				}
			} else {
				if (property.entityAuxData.rdftype == 'http://xmlns.com/foaf/0.1/Document') {
					//Identificadores
					var entity = property.entityAuxData.entities[property.values[i]][0];
					html += `		<div class="item ">
										<div class="item-content">
											<div class="item-title">
												${entity.properties[0].values[0]}
											</div>
											<div class="item-data">
												${entity.properties[1].values[0]}
											</div>
										</div>
									</div>`;
				} else {
					for (var j = 0; j < property.entityAuxData.entities[property.values[i]].length; j++) {
						var entityRows = property.entityAuxData.entities[property.values[i]][j];
						for (var k = 0; k < entityRows.properties.length; k++) {
							var tit = entityRows.properties[k].title;
							var val = '';
							if (entityRows.properties[k].values.length > 0) {
								val = entityRows.properties[k].values[0];
							}
							if (val != '' && entityRows.properties[k].type == 'selectCombo') {
								val = entityRows.properties[k].comboValues[val];
							}
							if (val != '') {
								html += `		<div class="item ">
												<div class="item-content">
													<div class="item-title">
														${tit}
													</div>
													<div class="item-data">
														${val}
													</div>
												</div>
											</div>`;
							}
						}
					}
				}
			}
		}
		return html;
	},
	//Métodos de secciones
	printTabSection: function (data, onlyPublic = false) {
		//Pintado sección listado
		//css mas generico
		var id = 'x' + RandomGuid();
		var id2 = 'x' + RandomGuid();
		var idTooltipSection = "tooltipSection" + RandomGuid();
		tooltips.section['#' + idTooltipSection] = data.information;

		var expanded = "";
		var show = "";
		if (data.items != null) {
			var tieneElementosCargados = Object.values(data.items).some(function (item) {
				return item != null;
			});
			var tieneItems = Object.keys(data.items).length > 0;
			//if (Object.keys(data.items).length > 0) {
			if (tieneElementosCargados && !onlyPublic) {
				//Desplegado
				expanded = "true";
				show = "show";
			} else {
				//No desplegado
				expanded = "false";
			}

			var htmlListItems = '';
			if (tieneElementosCargados) {
				htmlListItems = this.printHtmlListItems(data.items, onlyPublic);
			}

			var notLoaded = '';
			if (tieneItems && !tieneElementosCargados) {
				notLoaded = 'notLoaded';
			}

			//TODO texto ver items
			let aniadirEntidad = `<ul class="no-list-style d-flex align-items-center">
									<li>
										<a class="btn btn-outline-grey aniadirEntidad">
											<span class="texto">${GetText('CV_AGNADIR')}</span>
											<span class="material-icons">post_add</span>
										</a>
									</li>
								</ul>`;
			let tooltip = `<span class="material-icons-outlined informationTooltip" style="width:24px; float:left; margin-left: 5px" id="${idTooltipSection}"></span>`;
			var htmlSection = `
			<div class="panel-group pmd-accordion ${notLoaded}" section="${data.identifier}" id="${id}" role="tablist" aria-multiselectable="true">
				<div class="panel">
					<div class="panel-heading" role="tab" id="publicaciones-tab">
						<p class="panel-title">
							<a style="display: flex;" data-toggle="collapse" data-parent="#${id}" href="#${id2}" aria-expanded="${expanded}" aria-controls="${id2}" data-expandable="false">
								<span class="material-icons pmd-accordion-icon-left">folder_open</span>
								<span class="texto">${data.title}</span>
								<span class="numResultados">(${Object.keys(data.items).length})</span>
								${!onlyPublic ? tooltip : ''}
								<span class="material-icons pmd-accordion-arrow">keyboard_arrow_up</span>
							</a>
						</p>
					</div>
					<div id="${id2}" class="panel-collapse collapse ${show}" role="tabpanel">
						<div class="panel-body">
							<div class="acciones-listado acciones-listado-cv">
								<div class="wrap">
								</div>
								<div class="wrap">
									${!onlyPublic ? aniadirEntidad : ''}
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
									${htmlListItems}
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
			</div>`;
			return htmlSection;
		} else if (data.item != null) {
			var idEntity = '';
			if (data.item.entityID != null) {
				//Desplegado
				expanded = "true";
				show = "show";
			} else {
				//No desplegado
				expanded = "false";
			}
			data.item.idContenedor = 'x' + RandomGuid();
			var htmlSection = `
			<div class="panel-group pmd-accordion" section="${data.identifier}" id="${id}" role="tablist" aria-multiselectable="true">
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
						<div class="panel-body" id="${data.item.idContenedor}">
						</div>
					</div>
				</div>
			</div>`;
			return htmlSection;
		}

	},
	printOrderTabSection: function (orders) {
		if (orders != null && orders.length > 0) {
			let propDefault = "";
			let ascDefault = "";
			for (var item in orders[0].properties) {
				if (propDefault != '') {
					propDefault += "||";
				}
				if (ascDefault != '') {
					ascDefault += "||";
				}
				propDefault += orders[0].properties[item].property;
				ascDefault += orders[0].properties[item].asc;
			}
			var oculto = "";
			if (orders.length == 1) {
				oculto = "oculto";
			}
			return `	<a class="dropdown-toggle ${oculto}" data-toggle="dropdown">
							<span class="material-icons">swap_vert</span>
							<span class="texto" property="${propDefault}" asc="${ascDefault}">${orders[0].name}</span>
						</a>
						<div class="dropdown-menu basic-dropdown dropdown-menu-right">
						${this.printOrderTabSectionItems(orders)}
						</div>`;
		}
		return "";
	},
	printOrderTabSectionItems: function (orders) {
		var html = '';
		for (var ord in orders) {
			html += this.printOrderTabSectionItem(orders[ord]);
		}
		return html;
	},
	printOrderTabSectionItem: function (order) {
		let prop = "";
		let asc = "";
		for (var item in order.properties) {
			if (prop != '') {
				prop += "||";
			}
			if (asc != '') {
				asc += "||";
			}
			prop += order.properties[item].property;
			asc += order.properties[item].asc;
		}
		return `<a href="javascript: void(0)" class="item-dropdown"  property="${prop}" asc="${asc}">${order.name}</a>`;
	},
	printHtmlListItems: function (items, onlyPublic = false) {
		var html = "";
		for (var item in items) {
			html += this.printHtmlListItem(item, items[item], onlyPublic);
		}
		return html;
	},
	printHtmlListItem: function (id, data, onlyPublic = false) {
		let openAccess = "";
		if (data.isopenaccess) {
			openAccess = "open-access";
		}
		var htmlListItem = `<article class="resource success ${openAccess}" >
									<div class="wrap">
										<div class="middle-wrap">
											${this.printHtmlListItemOrders(data)}
											<div class="title-wrap">
											</div>
											<div class="title-wrap">
												<h2 class="resource-title">
													<a href="#" data-id="${id}" internal-id="${data.identifier}">${data.title}</a>
												</h2>
												${!onlyPublic ? this.printHtmlListItemValidacion(data) : ''}
												${!onlyPublic ? this.printHtmlListItemEditable(data) : ''}
												${!onlyPublic ? this.printHtmlListItemVisibilidad(data) : ''}
												${!onlyPublic ? this.printHtmlListItemIdiomas(data) : ''}
												${!onlyPublic ? this.printHtmlListItemAcciones(data, id) : ''}
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
	},
	printHtmlListItemPRC: function (id, data) {
		let openAccess = "";
		if (data.isopenaccess) {
			openAccess = "open-access";
		}
		var htmlListItem = `<article class="resource success ${openAccess}" >
									<div class="wrap">
										<div class="middle-wrap">
											${this.printHtmlListItemOrders(data)}
											<div class="title-wrap">
											</div>
											<div class="title-wrap">
												<h2 class="resource-title">
													<a href="#" data-id="${id}" internal-id="${data.identifier}">${data.title}</a>
												</h2>
												${this.printHtmlListItemValidacion(data)}
												${this.printHtmlListItemEditable(data)}
												${this.printHtmlListItemVisibilidad(data)}
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
	},
	printHtmlListItemDuplicate: function (id, data) {
		let openAccess = "";
		if (data.isopenaccess) {
			openAccess = "open-access";
		}
		var htmlListItem = `<article class="resource success ${openAccess}" >
									<div class="wrap">
										<div class="middle-wrap">
											${this.printHtmlListItemOrders(data)}
											<div class="title-wrap">
											</div>
											<div class="title-wrap">
												<h2 class="resource-title">
													<a href="#" data-id="${id}" internal-id="${data.identifier}">${data.title}</a>
												</h2>
												${this.printHtmlListItemValidacion(data)}
												${this.printHtmlListItemEditable(data)}
												${this.printHtmlListItemIdiomas(data)}
												${this.printHtmlListItemAcciones(data, id)}
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
	},
	printHtmlListItemOrders: function (data) {
		var html = '<div class="orderItems" style="display:none">';
		for (var property in data.orderProperties) {
			var values = "";
			if (data.orderProperties[property].values != null) {
				for (var value in data.orderProperties[property].values) {
					values += data.orderProperties[property].values[value].toLowerCase().trim();
				}
			}
			html += `<div property="${data.orderProperties[property].property}">${values}</div>`;
		}
		html += `<div property="default">${data.title.toLowerCase().trim()}</div>`;
		html += '</div>';
		return html;
	},
	printHtmlListItemValidacion: function (data) {
		if (data.validationStatus == 'pendiente' || data.removePRC) {
			return `<div class="manage-history-wrapper">
						<span class="con-icono-before material-icons">manage_history</span>
					</div>`;
		}
		if (data.validationStatus == 'validado') {
			return `<div class="verified-wrapper">
						<span class="con-icono-before material-icons-outlined">verified_user</span>
					</div>`;
		}
		return ``;
	},
	printHtmlListItemEditable: function (data) {
		if (!data.iseditable) {
			return `<div class="block-wrapper">
						<span class="con-icono-before material-icons">block</span>
					</div>`;
		}
		return '';
	},
	printHtmlListItemVisibilidad: function (data) {
		if (!data.isPublishable) {
			return '';
		}
		if (data.ispublic) {
			return `<div class="visibility-wrapper">
						<div class="con-icono-before eye"></div>
					</div>`;
		} else {
			return `<div class="visibility-wrapper">
						<div class="con-icono-before visibility-activo"></div>
					</div>`;
		}
	},
	printHtmlListItemIdiomas: function (data) {
		if (data.multilang != null && Object.entries(data.multilang).length > 0) {
			let listado = ``;

			for (const [key, value] of Object.entries(data.multilang)) {
				let css = '';
				if (value) {
					css += 'circulo-verde';
				}
				listado += `<div class="translation translation-${key} " data-original-title="" title="">
                                <span class="circulo-color ${css}">
                                    ${key.toUpperCase()}
                                </span>
                            </div>`;
			}

			return `<div class="traducciones-wrapper">
                            ${listado}
                        </div>`;
		}
		return '';
	},
	printHtmlListItemAcciones: function (data, id) {
		var htmlAcciones = "";

		//Si la publicación está en validación o pendiente no se permite el envio a produccion cientifica
		if (data.sendPRC) {
			htmlAcciones += `<li>
							<a class="item-dropdown" data-toggle="modal" data-target="#modal-enviar-produccion-cientifica">
								<span class="material-icons">send</span>
								<span class="texto prodCientItem" data-id="${id}" >${GetText("ENVIAR_PRODUCCION_CIENTIFICA")}</span>
							</a>
						</li>`;
		} else if (data.validationStatus == 'validado' && !data.removePRC) {
			htmlAcciones += `<li>
							<a class="item-dropdown" data-toggle="modal">
								<span class="material-icons">delete</span>
								<span class="texto prodCientBorrarItem" data-id="${id}" >${GetText("ENVIAR_BORRAR_PRODUCCION_CIENTIFICA")}</span>
							</a>
						</li>`;
		}
		// Envío a dspace
		if (data.sendDspace) {
			htmlAcciones += `<li>
							<a class="item-dropdown" data-toggle="modal">
								<span class="material-icons">send</span>
								<span class="texto sendDspace" data-id="${id}" >${GetText("ENVIAR_A_DSPACE")}</span>
							</a>
						</li>`;
		}
		//Si el proyecto está en validación o pendiente no se permite el envio
		if (data.sendValidationProject) {
			htmlAcciones += `<li>
								<a class="item-dropdown" data-toggle="modal">
									<span class="material-icons">send</span>
									<span class="texto validacionItem" data-id="${id}" >${GetText("ENVIAR_VALIDACION")}</span>
								</a>
							</li>`;
		}

		// Si es publicable se muestra el botón de publicar o despublicar
		if (data.isPublishable) {
			if (!data.ispublic) {
				//Si no está publicado siempre se puede publicar
				htmlAcciones += `<li>
									<a class="item-dropdown">
										<span class="material-icons">visibility</span>
										<span class="texto publicaritem" data-id="${id}" property="${data.propertyIspublic}">${GetText("CV_PUBLICAR")}</span>
									</a>
								</li>`;
			} else {//if (data.iseditable) {
				//Si está publicado sólo se puede despublicar si es editable
				htmlAcciones += `<li>
									<a class="item-dropdown">
										<span class="material-icons">visibility_off</span>
										<span class="texto despublicaritem" data-id="${id}">${GetText("CV_DESPUBLICAR")}</span>
									</a>
								</li>`;
			}
		}
		////Si es editable se puede eliminar
		//if (data.iseditable) {
		//    htmlAcciones += `<li>
		//						<a class="item-dropdown">
		//							<span class="material-icons">delete</span>
		//							<span class="texto eliminar" data-id="${id}">${GetText('CV_ELIMINAR')}</span>
		//						</a>
		//					</li>`
		//}
		if (data.iserasable) {
			htmlAcciones += `<li>
								<a class="item-dropdown">
									<span class="material-icons">delete</span>
									<span class="texto eliminar" data-id="${id}">${GetText('CV_ELIMINAR')}</span>
								</a>
							</li>`
		}
		if (htmlAcciones != '') {
			htmlAcciones = `	<div class="acciones-recurso-listado acciones-recurso">
								<div class="dropdown">
									<a href="javascript: void(0)" class="dropdown-toggle no-flecha" role="button" id="dropdownMasOpciones" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
										<span class="material-icons">more_vert</span>
									</a>
									<div class="dropdown-menu basic-dropdown dropdown-icons dropdown-menu-right" aria-labelledby="dropdownMasOpciones">
										<p class="dropdown-title">${GetText('CV_ACCIONES')}</p>
										<ul class="no-list-style">
											${htmlAcciones}
										</ul>
									</div>
								</div>
							</div>`;
		}
		return htmlAcciones;
	},
	printHtmlListItemPropiedades: function (data) {
		var html = ''
		for (var property in data.properties) {
			if (data.properties[property].values.length > 0) {
				html += this.printHtmlListItemPropiedad(data.properties[property]);
			}
		}
		return html;
	},
	printHtmlListItemPropiedad: function (prop) {
		var htmlProp = "";
		if (prop.values.length > 1) {
			htmlProp = `		<div class="list-wrap">
								<ul>`;
			for (var value in prop.values) {
				htmlProp += `<li>${prop.values[value]}</li>`;
			}
			htmlProp += `			</ul>
							</div>`
		} else {
			htmlProp = `<p>${TransFormData(prop.values, prop.type)}</p>`;
		}
		var css = "";
		if (prop.showMiniBold) {
			css = "mini resaltado";
		} else if (prop.showMini) {
			css = "mini";
		}
		return `<div class="group ${css}">
					<p class="title">${prop.name}</p>
					${htmlProp}
				</div>`;
	},
	repintarListadoTab: function (id, noEngancharComportamientosCV, mostrarSoloConflictos, mostrarSoloNuevos) {
		var sectionItem = $('.panel-group[section="' + id + '"]');
		var numResultadosPagina = parseInt(sectionItem.find(' .panNavegador .dropdown-toggle span').attr('items'));
		var texto = sectionItem.find(' .txtBusqueda').val();

		if (mostrarSoloConflictos != false && sectionItem.find('.acciones-listado .checkAllCVWrapper input[type="checkbox"]').hasClass('mostrarSimilitudes')) {
			mostrarSoloConflictos = true;
		}
		else if (mostrarSoloNuevos != false && sectionItem.find('.acciones-listado .checkAllCVWrapper input[type="checkbox"]').hasClass('mostrarNuevos')) {
			mostrarSoloNuevos = true;
		}	
		else {
			mostrarSoloConflictos = false;
			mostrarSoloNuevos = false;
		}

		var paginaActual = parseInt(sectionItem.find(' .panNavegador .pagination.numbers li.actual a').attr('page'));
		var ordenItem = sectionItem.find(' .ordenar.dropdown.orders .texto');
		var ordenProperty = ordenItem.attr('property');
		var ordenAsc = ordenItem.attr('asc');


		//paginaActual
		//orden

		var NUM_PAG_INICIO = 3;
		var NUM_PAG_PROX_CENTRO = 2;
		var NUM_PAG_FIN = 3;

		var articulos = $('div[section="' + id + '"] article');

		var ordenPropertySplit = [];
		var ordenAscSplit = [];
		if (ordenProperty != null) {
			ordenPropertySplit = ordenProperty.split('||');
			ordenAscSplit = ordenAsc.split('||');
		}


		//Ordenes por defecto
		var orderDefaultText = $('div[section="' + id + '"] article div.orderItems div[property="default"]').map((i, e) => {
			return $(e).text();
		});

		//Ordenes auxiliares
		var ordersPropertyText = {};
		for (var i = 0; i < ordenPropertySplit.length; i++) {
			let property = ordenPropertySplit[i];
			let asc = ordenAscSplit[i];
			var orderPropertyText = $('div[section="' + id + '"] article div.orderItems div[property="' + property + '"]').map((i, e) => {
				return $(e).text();
			});
			ordersPropertyText[property] = orderPropertyText;
		}
		var num = 0;
		articulos.each(function () {
			$(this).attr('num', num);
			num++;
		});
		articulos = articulos.sort(function (a, b) {
			let numA = $(a).attr('num');
			let numB = $(b).attr('num');
			if (ordenProperty == null || ordenProperty == '') {
				if (orderDefaultText[numA] > orderDefaultText[numB]) return 1;
				if (orderDefaultText[numA] < orderDefaultText[numB]) return -1;
				return 0;
			} else {
				for (var i = 0; i < ordenPropertySplit.length; i++) {
					let property = ordenPropertySplit[i];
					let asc = ordenAscSplit[i];
					let valueA = ordersPropertyText[property][numA];
					let valueB = ordersPropertyText[property][numB];
					if (asc == 'true') {
						if (valueA > valueB) {
							return 1;
						}
						if (valueA < valueB) {
							return -1;
						}
					} else {
						if (valueA < valueB) {
							return 1;
						}
						if (valueA > valueB) {
							return -1;
						}
					}
				}
				return 0;

			}
		});
		$('div[section="' + id + '"] article').remove();
		$('div[section="' + id + '"] .resource-list .resource-list-wrap').prepend(articulos);

		var numTotal = 1;
		var numPaginas = 1;
		var texto = EliminarAcentos(texto).toLowerCase();
		$('div[section="' + id + '"] article').attr('style', 'display:none');
		$('div[section="' + id + '"] article').each(function () {
			var existe = texto == '';
			var existeEnTitulo = existe || EliminarAcentos($(this).find('h2').text()).toLowerCase().indexOf(texto) > -1;
			var existeEnPropiedad = existe || EliminarAcentos($(this).find('.content-wrap .group p:not(.title),.content-wrap .group li').text()).toLowerCase().indexOf(texto) > -1;

			if (existe || existeEnTitulo || existeEnPropiedad) {
				if (mostrarSoloConflictos) {
					numPaginas = Math.floor((numTotal - 1 + numResultadosPagina) / numResultadosPagina);
					if ($(this).hasClass('conflict-true')) {
						if (numPaginas == paginaActual) {
							$(this).show();
						}
						numTotal++;
					}
				}
				else if (mostrarSoloNuevos) {
					numPaginas = Math.floor((numTotal - 1 + numResultadosPagina) / numResultadosPagina);
					if ($(this).hasClass('conflict-false')) {
						if (numPaginas == paginaActual) {
							$(this).show();
						}
						numTotal++;
					}
				} else {
					numPaginas = Math.floor((numTotal - 1 + numResultadosPagina) / numResultadosPagina);
					if (numPaginas == paginaActual) {
						$(this).show();
					}
					numTotal++;
				}
			}

		});
		$('div[section="' + id + '"] .pagination.numbers').empty();
		$('div[section="' + id + '"] .pagination.arrows').empty();

		///INICIO/
		for (i = 1; i <= NUM_PAG_INICIO; i++) {
			if (i > numPaginas) //Hemos excedio el número máximo de páginas, así que dejamos de pintar.
			{
				break;
			}
			if (i == paginaActual) {
				$('div[section="' + id + '"] .pagination.numbers').append(`<li class="actual"><a page="${i}" >${i}</a></li>`);
			} else {
				$('div[section="' + id + '"] .pagination.numbers').append(`<li ><a href="javascript: void(0)" page="${i}" >${i}</a></li>`);
			}
		}

		if (numPaginas > NUM_PAG_INICIO) //Continuamos si ha más páginas que las que ya hemos pintado
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
				$('div[section="' + id + '"] .pagination.numbers').append(`<span>...</span>`);
			}


			for (i = inicioRango; i <= finRango; i++) {
				if (i > numPaginas) //Hemos excedio el número máximo de páginas, así que dejamos de pintar.
				{
					break;
				}

				if (i == paginaActual) {
					$('div[section="' + id + '"] .pagination.numbers').append(`<li class="actual"><a page="${i}" >${i}</a></li>`);
				} else {
					$('div[section="' + id + '"] .pagination.numbers').append(`<li><a href="javascript: void(0)" page="${i}" >${i}</a></li>`);
				}
			}

			if (finRango < numPaginas) {
				//Continuamos si ha más páginas que las que ya hemos pintado
				inicioRango = numPaginas - NUM_PAG_FIN + 1;

				if ((inicioRango - 1) > finRango) {
					$('div[section="' + id + '"] .pagination.numbers').append(`<span>...</span>`);
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
						$('div[section="' + id + '"] .pagination.numbers').append(`<li class="actual"><a page="${i}" >${i}</a></li>`);
					} else {
						$('div[section="' + id + '"] .pagination.numbers').append(`<li><a href="javascript: void(0)" page="${i}" >${i}</a></li>`);
					}
				}
			}
		}

		if (paginaActual == 1) {
			$('div[section="' + id + '"] .pagination.arrows').append(`<li class="actual"><a class="primeraPagina">Página anterior</a></li>`);
		} else {
			$('div[section="' + id + '"] .pagination.arrows').append(`<li><a href="javascript: void(0)" page="${(paginaActual - 1)}" class="primeraPagina">Página anterior</a></li>`);
		}

		if (paginaActual == numPaginas) {
			$('div[section="' + id + '"] .pagination.arrows').append(`<li class="actual"><a class="ultimaPagina">Página siguiente</a></li>`);
		} else {
			$('div[section="' + id + '"] .pagination.arrows').append(`<li><a href="javascript: void(0)" page="${(paginaActual + 1)}" class="ultimaPagina">Página siguiente</a></li>`);
		}

		if (!$('div[section="' + id + '"]').hasClass('notLoaded')) {
			$('div[section="' + id + '"] .numResultados').text('(' + $('div[section="' + id + '"] article').length + ')');
		}
		if (noEngancharComportamientosCV == null || !noEngancharComportamientosCV) {
			this.engancharComportamientosCV($('div#contenedorOtrosMeritos').length != 0);
		}
		accionesPlegarDesplegarModal.init();
		tooltipsAccionesRecursos.init();
		tooltipsCV.init();
		//Añado comportamientos en importacioncv
		if (typeof checkAllCVWrapper != 'undefined') {
			checkAllCVWrapper();
		}
		if (typeof aniadirComportamientoCompararItems != 'undefined') {
			aniadirComportamientoCompararItems();
		}

		$('#navegacion-cv li.nav-item a').tooltip('dispose');
	},
	paginarListado: function (sectionID, pagina) {
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers .actual').removeClass('actual');
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers li a[page="' + pagina + '"]').parent().addClass('actual');
		this.repintarListadoTab(sectionID);
	},
	cambiarTipoPaginacionListado: function (sectionID, itemsPagina, texto) {
		$('.panel-group[section="' + sectionID + '"] .panNavegador .dropdown-toggle span').attr('items', itemsPagina);
		$('.panel-group[section="' + sectionID + '"] .panNavegador .dropdown-toggle span').text(texto);
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers .actual').removeClass('actual');
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers li a[page="1"]').parent().addClass('actual');
		this.repintarListadoTab(sectionID);
	},
	buscarListado: function (sectionID, mostrarSoloConflictos, mostrarSoloNuevos) {
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers .actual').removeClass('actual');
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers li a[page="1"]').parent().addClass('actual');
		this.repintarListadoTab(sectionID, false, mostrarSoloConflictos, mostrarSoloNuevos);
	},
	ordenarListado: function (sectionID, text, property, asc, dropdown) {
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers .actual').removeClass('actual');
		$('.panel-group[section="' + sectionID + '"] .panNavegador .pagination.numbers li a[page="1"]').parent().addClass('actual');
		if (dropdown == null) {
			$('.panel-group[section="' + sectionID + '"] .ordenar.dropdown.orders .dropdown-toggle .texto').text(text);
			$('.panel-group[section="' + sectionID + '"] .ordenar.dropdown.orders .dropdown-toggle .texto').attr('property', property);
			$('.panel-group[section="' + sectionID + '"] .ordenar.dropdown.orders .dropdown-toggle .texto').attr('asc', asc);
		}
		else {
			dropdown.text(text);
			if (property != null) {
				dropdown.attr('property', property);
				dropdown.attr('asc', asc);
			}
		}
		this.repintarListadoTab(sectionID);
	},
	cambiarPrivacidadItem: function (sectionID, rdfTypeTab, entityID, isPublic, element) {
		var that = this;
		var item = {};
		item.pIdSection = sectionID;
		item.pRdfTypeTab = rdfTypeTab;
		item.pEntity = entityID;
		item.pIsPublic = isPublic;
		var article = $(element).closest('article');
		MostrarUpdateProgress();
		$.post(urlGuardadoCV + 'ChangePrivacityItem', item, function (data) {
			$.get(urlEdicionCV + 'GetItemMini?pCVId=' + that.idCV + '&pIdSection=' + sectionID + "&pRdfTypeTab=" + rdfTypeTab + "&pEntityID=" + item.pEntity + "&pLang=" + lang, null, function (data) {
				article.replaceWith(that.printHtmlListItem(item.pEntity, data));
				that.repintarListadoTab(sectionID);
				OcultarUpdateProgress();
			});
		});
	},
	cargarEdicionItem: function (sectionID, rdfTypeTab, entityID) {
		var that = this;
		MostrarUpdateProgress();
		$('#modal-editar-entidad form').empty();
		$('#modal-editar-entidad .form-actions .ko').remove();
		$.get(urlEdicionCV + 'GetEdit?pCVId=' + that.idCV + '&pIdSection=' + sectionID + "&pRdfTypeTab=" + rdfTypeTab + "&pEntityID=" + entityID + "&pLang=" + lang, null, function (data) {
			that.printEditItemCV('#modal-editar-entidad form', data, sectionID, rdfTypeTab, entityID);
			for (var key in tooltips.items) {
				var value = tooltips.items[key];
				$(key).tooltip({
					html: true,
					placement: 'bottom',
					template: '<div class="tooltip background-gris-oscuro infoTooltipMargin" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner tooltipEditor"></div></div>',
					title: value
				});
				montarTooltip.comportamiento($(key));
			}
			OcultarUpdateProgress();

		});
	}

	//Fin de métodos de secciones
	,
	//Métodos de edición de un item del cv
	printEditItemCV: function (contenedor, data, sectionID, rdfTypeTab, entityID) {
		$(contenedor).empty();
		$(contenedor).attr('entityid', data.entityID);
		$(contenedor).attr('rdftype', data.rdftype);
		$(contenedor).attr('ontology', data.ontology);
		$(contenedor).attr('sectionID', sectionID);
		$(contenedor).attr('rdfTypeTab', rdfTypeTab);
		//TODO ¿se usa, cambiar de nombre?
		$(contenedor).attr('entityload', entityID);

		let multiidioma = data.sections.some(function (section) {
			return section.rows.some(function (row) {
				return row.properties.some(function (property) {
					return property.multilang;
				});
			});
		});

		if (multiidioma) {
			let htmlLangs = '<ul class="nav nav-tabs grey" id="" role="tablist">';
			htmlLangs += `<li class="nav-item">
							<a href="javascript: void(0);" lang="es" class="nav-link active">${GetText('ESPAGNOL')}</a>
						</li>`;
			languages.forEach(function (language, index) {
				var nombreIdioma = "";
				switch (language) {
					case 'en':
						nombreIdioma = GetText('INGLES');
						break;
					case 'ca':
						nombreIdioma = GetText('CATALAN');
						break;
					case 'eu':
						nombreIdioma = GetText('EUSKERA');
						break;
					case 'gl':
						nombreIdioma = GetText('GALLEGO');
						break;
					case 'fr':
						nombreIdioma = GetText('FRANCES');
						break;
				}
				htmlLangs += `	<li class="nav-item">
				<a href="javascript: void(0);" lang=${language} class="nav-link">${nombreIdioma}</a>
			</li>`;
			});
			htmlLangs += '</ul>';
			$(contenedor).append(htmlLangs);
		}

		for (var i = 0; i < data.sections.length; i++) {
			var collapsed = ""
			var ariaexpanded = "true";
			var show = "show";
			if (i > 0 && data.sections[i].title != '' && data.sections[i].title != null) {
				collapsed = "collapsed";
				ariaexpanded = "false";
				show = "";
			}
			var titleSection = '';
			if (data.sections[i].title != '' && data.sections[i].title != null) {
				titleSection = `<a class="collapse-toggle ${collapsed}" data-toggle="collapse" href="#collapse-${i}" role="button" aria-expanded="${ariaexpanded}" aria-controls="collapse-${i}">${data.sections[i].title}</a>`;
			}
			var section =
				`<div class="simple-collapse">
								${titleSection}
								<div class="collapse ${show}" id="collapse-${i}" style="">
									<div class="simple-collapse-content">${this.printRowsEdit(data.iseditable, data.sections[i].rows)}</div>
								</div>
							</div>`;
			$(contenedor).append(section);
		}
		this.repintarListadoEntity(true);
		this.engancharComportamientosCV();
		this.loadPropertiesEntitiesAutocomplete();
	},
	printEditEntity: function (modalContenedor, data, rdfType) {
		$(modalContenedor + ' form').empty();
		$(modalContenedor + ' form').attr('entityid', data.entityID);
		$(modalContenedor + ' form').attr('rdftype', data.rdftype);

		for (var i = 0; i < data.sections.length; i++) {
			var collapsed = ""
			var ariaexpanded = "true";
			var show = "show";
			if (i > 0) {
				collapsed = "collapsed";
				ariaexpanded = "false";
				show = "";
			}
			var section =
				`<div class="simple-collapse">
								<a class="collapse-toggle ${collapsed}" data-toggle="collapse" href="#collapse-${i}" role="button" aria-expanded="${ariaexpanded}" aria-controls="collapse-${i}">${data.sections[i].title}</a>
								<div class="collapse ${show}" id="collapse-${i}" style="">
									<div class="simple-collapse-content">${this.printRowsEdit(data.iseditable, data.sections[i].rows)}</div>
								</div>
							</div>`;
			$(modalContenedor + ' form').append(section);
		}
		this.repintarListadoEntity();
		this.engancharComportamientosCV();
	},
	printRowsEdit: function (iseditable, rows) {
		var rowsHtml = "";
		for (var i = 0; i < rows.length; i++) {
			rowsHtml += this.printRowEdit(iseditable, rows[i]);
		}
		return rowsHtml;
	},
	printRowEdit: function (iseditable, row) {
		var rowHtml = `<div class="custom-form-row">`
		for (var k = 0; k < row.properties.length; k++) {
			var index = RandomGuid();
			rowHtml += this.printPropertyEdit(iseditable, row.properties[k], index);
			if (row.properties[k].information) {
				id = "#tooltip" + index;
				tooltips.items[id] = row.properties[k].information;
			}
		}
		rowHtml += `</div>`;

		return rowHtml;
	},
	printPropertyEdit: function (iseditable, property, index) {
		//Estilos para el contenedor
		var css = "";
		//Tooltip
		// TODO esperar a la maqueta de Felix del tooltip de la sección
		var spanTooltip = property.information ? `<span class="material-icons-outlined informationTooltip" style="width:24px; float:left; margin-left: 5px" id="tooltip${index}"></span>` : '';
		switch (property.width) {
			case 0:
				css = 'oculto';
				break;
			case 1:
				css = '';
				break;
			case 2:
				css = 'expand-2';
				break;
			case 3:
				css = 'full-group';
				break;
			default:
				css = '';
				break;
		}


		var topicsProp = ["http://w3id.org/roh/userKeywords", "http://w3id.org/roh/enrichedKeywords", "http://w3id.org/roh/externalKeywords"];
		if ($.inArray(property.property, topicsProp) >= 0) {
			css += ' topic';
		}

		if (!iseditable) {
			//Si el elemento está bloqueado pero la propiedad está marcada como editable sí que se pude editar
			iseditable = property.editable;
		}

		if (!iseditable || property.blocked) {
			//Si no es editable o esta bloqueado está deshabilitado
			css += ' disabled ';
		}

		var value = "";
		if (!property.multiple) {
			for (var i = 0; i < property.values.length; i++) {
				value = property.values[i];
			}
		}

		var required = "";
		if (property.required) {
			required = " *";
		}

		var rdftype = '';
		if (!property.multiple && property.entityAuxData == null) {
			var htmlInput = '';

			switch (property.type) {
				case 'boolean':
					htmlInput = this.printSelectCombo(property.property, value, property.comboValues, property.comboDependency, property.required, !iseditable || property.blocked, property.entity_cv, property.dependency);
					break;
				case 'image':
					htmlInput = this.printPropertyEditImage(property.property, property.placeholder, value);
					break;
				case "entityautocomplete":
					htmlInput = this.printPropertyEditEntityAutocomplete(property.property, property.placeholder, property.propertyEntityValue, property.required, !iseditable || property.blocked, property.autocomplete, property.dependency, property.autocompleteConfig);
					break;
				case 'text':
					htmlInput = this.printPropertyEditTextInput(property.property, property.placeholder, value, property.required, !iseditable || property.blocked, property.autocomplete, property.dependency, property.autocompleteConfig, property.entity_cv, property.multilang, property.valuesmultilang);
					break;
				case 'number':
					htmlInput = this.printPropertyEditNumberInput(property.property, property.placeholder, value, property.required, !iseditable || property.blocked, property.dependency, property.entity_cv);
					break;
				case 'selectCombo':
					htmlInput = this.printSelectCombo(property.property, value, property.comboValues, property.comboDependency, property.required, !iseditable || property.blocked, property.entity_cv, property.dependency);
					break;
				case 'textarea':
					htmlInput = this.printPropertyEditTextArea(property.property, property.placeholder, value, property.required, !iseditable || property.blocked, property.entity_cv, property.multilang, property.valuesmultilang);
					break;
				case 'date':
					htmlInput = this.printPropertyEditDate(property.property, property.placeholder, value, property.required, !iseditable || property.blocked, property.dependency);
					break;
				case 'auxEntity':
				case 'auxEntityAuthorList':
					if (property.type == 'auxEntityAuthorList') {
						css += " entityauxauthorlist";
					}
					css += " entityauxcontainer";
					if (property.required) {
						css += " obligatorio";
					}
					if (value != '') {
						htmlInput = `<div class='item aux entityaux' propertyrdf='${property.property}' rdftype='${property.entityAuxData.rdftype}' about='${value}'>
							${this.printRowsEdit(iseditable, property.entityAuxData.entities[value])}
							</div>`;
					} else {
						htmlInput = `<div class='item aux entityaux' propertyrdf='${property.property}' rdftype='${property.entityAuxData.rdftype}' about=''>
							${this.printRowsEdit(iseditable, property.entityAuxData.rows)}
							</div>`;
					}
					break;
				case 'entity':
					css += " entitycontainer";
					if (property.required) {
						css += " obligatorio";
					}
					rdftype = ` rdftype='${property.entityData.rdftype}'`;
					htmlInput += `<div class='item added entity' propertyrdf='${property.property}' rdftype='${property.entityData.rdftype}' about='${value}'>`;

					htmlInput += this.printPropertyEditTextInput(property.property, property.placeholder, value, property.required, !iseditable, null, null, null, null);

					//Pintamos el título
					if (property.values.length > 0 && property.values[0] != null && property.entityData.titles[property.values[0]] != null) {
						htmlInput += `
						<span class="title" loaded="true" route="${property.entityData.titles[property.values[0]].route}">${property.entityData.titles[property.values[0]].value}</span> `;
					} else {
						htmlInput += `
						<span class="title" loaded="true" route="${property.entityData.titleConfig.route}"></span> `;
					}


					//Pintamos las propiedades
					if (property.entityData.propertiesConfig != null) {
						for (var prop in property.entityData.propertiesConfig) {
							if (property.values.length > 0 && property.values[0] != null && property.entityData.properties[property.values[0]] != null) {
								htmlInput += `
							<span class="property" loaded="true" name="${property.entityData.properties[property.values[0]][prop].name}" route="${property.entityData.properties[property.values[0]][prop].route}">${property.entityData.properties[property.values[0]][prop].value}</span> `;
							} else {
								htmlInput += `<span class="property" loaded="true" name="${property.entityData.propertiesConfig[prop].name}" route="${property.entityData.propertiesConfig[prop].route}"></span> `;
							}
						}
					}

					htmlInput += `</div>`;

					break;
			}
			if (property.propertyEntity != null) {
				var propertyEntityValue = '';
				if (property.propertyEntityValue != null) {
					propertyEntityValue = property.propertyEntityValue;
				}
				var attrSelectPropertyEntity = "";
				if (property.propertyEntityGraph != null && property.propertyEntityGraph != '' && property.selectPropertyEntity != null && property.selectPropertyEntity.length > 0) {
					attrSelectPropertyEntity += ' graph="' + property.propertyEntityGraph + '"';
					attrSelectPropertyEntity += ' propertyentity="';
					property.selectPropertyEntity.forEach(function (par, index) {
						attrSelectPropertyEntity += par.propertyEntity + "|" + par.propertyCV + "&";
					});
					attrSelectPropertyEntity = attrSelectPropertyEntity.substring(0, attrSelectPropertyEntity.length - 1);
					attrSelectPropertyEntity += '"';
				}
				htmlInput += `<input ${attrSelectPropertyEntity} propertyorigin="${property.property}" propertyrdf="${property.propertyEntity}" value="${propertyEntityValue}" type="hidden" class="form-control not-outline ">`;
			}
			if (property.type == "entityautocomplete") {
				var htmlDependency = '';
				var cssDependency = ''
				if (property.dependency != null) {
					cssDependency += ' hasdependency';
					if (property.dependency.parentDependencyValue != null) {
						htmlDependency = ` dependencyproperty="${property.dependency.parent}" dependencypropertyvalue="${property.dependency.parentDependencyValue}"`;
					} else if (property.dependency.parentDependencyValueDistinct != null) {
						htmlDependency = ` dependencyproperty="${property.dependency.parent}" dependencypropertyvaluedistinct="${property.dependency.parentDependencyValueDistinct}"`;
					}
				}

				htmlInput += `<input propertyorigin="${property.property}_aux" propertyrdf="${property.property}" value="${value}" type="hidden" class="form-control not-outline ${cssDependency} " ${htmlDependency} >`;
			}
			return `<div class="form-group ${css}" ${rdftype}>
					<div style="display: flex;">
						<label class="control-label d-block">${property.title}${required}</label>
						${spanTooltip}
					</div>
					${htmlInput}
				</div>`;
		} else {
			if (property.multiple) {
				css += ' multiple';
			}
			var htmlMultiple = `<div class='item aux'>`;
			if (property.type == 'auxEntity' || property.type == 'auxEntityAuthorList' || property.type == 'thesaurus') {
				htmlMultiple = `<div class='item aux entityaux' propertyrdf='${property.property}' rdftype='${property.entityAuxData.rdftype}' about=''>`;
			} else if (property.type == 'entity') {
				htmlMultiple = `<div class='item aux entity' propertyrdf='${property.property}' rdftype='${property.entityData.rdftype}' about=''>`;
			}
			switch (property.type) {
				case 'text':
					htmlMultiple += this.printPropertyEditTextInput(property.property, property.placeholder, '', property.required, !iseditable || property.blocked, property.autocomplete, property.dependency, property.autocompleteConfig, property.entity_cv, property.multilang, property.valuesmultilang);
					break;
				case 'number':
					htmlMultiple = this.printPropertyEditNumberInput(property.property, property.placeholder, value, property.required, !iseditable || property.blocked, property.dependency, property.entity_cv);
					break;
				case 'selectCombo':
					htmlMultiple += this.printSelectCombo(property.property, '', property.comboValues, property.comboDependency, property.required, !iseditable || property.blocked, property.entity_cv, property.dependency);
					break;
				case 'thesaurus':
					var valuesThesaurus = $.map(property.entityAuxData.entities, function (entity) {
						var values = entity[0].properties[0].values;
						return values;
					});
					htmlMultiple += this.printThesaurus(property.property, property.thesaurusID, valuesThesaurus, property.thesaurus, property.required, !iseditable);
					htmlMultiple += this.printRowsEdit(iseditable, property.entityAuxData.rows);
					break;
				case 'textarea':
					htmlMultiple += this.printPropertyEditTextArea(property.property, property.placeholder, '', property.required, !iseditable || property.blocked, property.entity_cv, property.multilang, property.valuesmultilang);
					break;
				case 'date':
					htmlMultiple += this.printPropertyEditDate(property.property, property.placeholder, '', property.required, !iseditable || property.blocked, property.dependency);
					break;
				case 'auxEntity':
				case 'auxEntityAuthorList':
					htmlMultiple += this.printRowsEdit(iseditable && !property.blocked, property.entityAuxData.rows);
					break;
				case 'entity':
					htmlMultiple += this.printPropertyEditTextInput(property.property, property.placeholder, value, property.required, !iseditable || property.blocked, null, null, null, null, property.multilang);
					break;
			}
			if (property.type == 'auxEntity' || property.type == 'auxEntityAuthorList' || property.type == 'thesaurus') {
				if (property.entityAuxData.propertyOrder != null && property.entityAuxData.propertyOrder != '') {
					htmlMultiple += `
					<input propertyrdf="${property.entityAuxData.propertyOrder}" value="" type="hidden">`;
				}
				if (property.entityAuxData.titleConfig != null) {
					htmlMultiple += `<span class="title" loaded="false" route="${property.entityAuxData.titleConfig.route}"></span> `;
				} else {
					htmlMultiple += `<span class="title" loaded="true"></span> `;
				}

				if (property.entityAuxData.propertiesConfig != null) {
					for (var prop in property.entityAuxData.propertiesConfig) {
						htmlMultiple += `
						<span class="property" loaded="false" name="${property.entityAuxData.propertiesConfig[prop].name}" route="${property.entityAuxData.propertiesConfig[prop].route}"></span> `;
					}
				}
			}
			if (property.type == 'entity') {
				htmlMultiple += `
					<span class="title" loaded="false" route="${property.entityData.titleConfig.route}"></span> `;

				if (property.entityData.propertiesConfig != null) {
					for (var prop in property.entityData.propertiesConfig) {
						htmlMultiple += `
						<span class="property" loaded="false" name="${property.entityData.propertiesConfig[prop].name}" route="${property.entityData.propertiesConfig[prop].route}"></span> `;
					}
				}
			}
			if (iseditable && property.type != 'auxEntity' && property.type != 'entity' && property.type != 'auxEntityAuthorList' && property.type != 'thesaurus') {
				htmlMultiple += this.printAddButton();
			}

			htmlMultiple += '</div>';
			var order = "";
			if (property.type == 'auxEntity' || property.type == 'auxEntityAuthorList' || property.type == 'thesaurus') {
				css += " entityauxcontainer";
				if (property.type == 'auxEntityAuthorList') {
					css += " entityauxauthorlist";
				}
				if (property.type == 'thesaurus') {
					css += " thesaurus";
				}
				if (property.entityAuxData.propertyOrder != null && property.entityAuxData.propertyOrder != '') {
					order = 'order="' + property.entityAuxData.propertyOrder + '"';
				}
			}
			if (property.type == 'entity') {
				css += " entitycontainer";
				rdftype = ` rdftype='${property.entityData.rdftype}'`;
			}
			if (property.required) {
				css += " obligatorio";
			}
			for (var valor in property.values) {
				if (property.type == 'auxEntity' || property.type == 'auxEntityAuthorList' || property.type == 'thesaurus') {
					htmlMultiple += `<div class='item added entityaux' propertyrdf='${property.property}' rdftype='${property.entityAuxData.rdftype}' about='${property.values[valor]}'>`;
				} else if (property.type == 'entity') {
					htmlMultiple += `<div class='item added entity' propertyrdf='${property.property}' rdftype='${property.entityData.rdftype}' about='${property.values[valor]}'>`;
				} else {
					htmlMultiple += `	<div class='item added'>`;
				}
				switch (property.type) {
					case 'text':
						htmlMultiple += this.printPropertyEditTextInput(property.property, property.placeholder, property.values[valor], property.required, true, false, null, null, null, property.multilang, property.valuesmultilang);
						break;
					case 'number':
						htmlMultiple += this.printPropertyEditNumberInput(property.property, property.placeholder, property.values[valor], property.required, true, null, property.entity_cv);
						break;
					case 'selectCombo':
						htmlMultiple += this.printSelectCombo(property.property, property.values[valor], property.comboValues, property.comboDependency, property.required, true, property.entity_cv, property.dependency);
						break;
					case 'textarea':
						htmlMultiple += this.printPropertyEditTextArea(property.property, property.placeholder, property.values[valor], property.required, true, property.entity_cv, property.multilang, property.valuesmultilang);
						break;
					case 'date':
						htmlMultiple += this.printPropertyEditDate(property.property, property.placeholder, property.values[valor], property.required, true);
						break;
					case 'auxEntity':
					case 'auxEntityAuthorList':
					case 'thesaurus':
						htmlMultiple += this.printRowsEdit(iseditable, property.entityAuxData.entities[property.values[valor]]);
						if (property.entityAuxData.propertyOrder != null && property.entityAuxData.propertyOrder != '') {
							//Pintamos el orden
							if (property.values[valor] != null && property.entityAuxData.childsOrder[property.values[valor]] != null) {
								htmlMultiple += `
								<input propertyrdf="${property.entityAuxData.propertyOrder}" value="${property.entityAuxData.childsOrder[property.values[valor]]}" type="hidden">`;
							} else {
								htmlMultiple += `
								<input propertyrdf="${property.entityAuxData.propertyOrder}" value="" type="hidden">`;
							}
						}

						//Pintamos el título
						if (property.values[valor] != null && property.entityAuxData.titles[property.values[valor]] != null) {
							htmlMultiple += `
							<span class="title" loaded="true" route="${property.entityAuxData.titles[property.values[valor]].route}">${property.entityAuxData.titles[property.values[valor]].value}</span> `;
						}

						//Pintamos las propiedades
						if (property.values[valor] != null && property.entityAuxData.properties[property.values[valor]] != null) {
							for (var prop in property.entityAuxData.properties[property.values[valor]]) {
								htmlMultiple += `
								<span class="property" loaded="true" name="${property.entityAuxData.properties[property.values[valor]][prop].name}" route="${property.entityAuxData.properties[property.values[valor]][prop].route}">${property.entityAuxData.properties[property.values[valor]][prop].value}</span> `;
							}
						}


						break;
					case 'entity':
						htmlMultiple += this.printPropertyEditTextInput(property.property, property.placeholder, value, property.required, !iseditable, null, null, null, null);

						//Pintamos el título
						if (property.values[valor] != null && property.entityData.titles[property.values[valor]] != null) {
							htmlMultiple += `
							<span class="title" loaded="true" route="${property.entityData.titles[property.values[valor]].route}">${property.entityData.titles[property.values[valor]].value}</span> `;
						}

						//Pintamos las propiedades
						if (property.values[valor] != null && property.entityData.properties[property.values[valor]] != null) {
							for (var prop in property.entityData.properties[property.values[valor]]) {
								htmlMultiple += `
								<span class="property" loaded="true" name="${property.entityData.properties[property.values[valor]][prop].name}" route="${property.entityData.properties[property.values[valor]][prop].route}">${property.entityData.properties[property.values[valor]][prop].value}</span> `;
							}
						}


						break;
				}
				if (property.type != 'auxEntity' && property.type != 'entity' && property.type != 'auxEntityAuthorList') {
					htmlMultiple += this.printDeleteButton();
				}
				htmlMultiple += '</div>';
			}
			var htmlDependency = '';
			if (property.dependency != null) {
				css += ' hasdependency';

				if (property.dependency.parentDependencyValue != null) {
					htmlDependency = ` dependencyproperty="${property.dependency.parent}" dependencypropertyvalue="${property.dependency.parentDependencyValue}"`;

				} else if (property.dependency.parentDependencyValueDistinct != null) {
					htmlDependency = ` dependencyproperty="${property.dependency.parent}" dependencypropertyvaluedistinct="${property.dependency.parentDependencyValueDistinct}"`;
				}

			}
			return `<div ${htmlDependency} class="form-group ${css}" ${order} ${rdftype}>
						<div style="display: flex;">
						<label class="control-label d-block">${property.title}${required}</label>
						${spanTooltip}
						</div>
					${htmlMultiple}
				</div>`;
		}
	},
	printPropertyEditImage: function (property, placeholder, value) {
		var imagen = "";
		if (value != null) {
			imagen = `<img class="image-uploader__img" src="${value}">`;
		}
		return `<div class="image-uploader js-image-uploader" id="foto-perfil-cv">
					<div class="image-uploader__preview">
						${imagen}
					</div>
					<div class="image-uploader__drop-area">
						<div class="image-uploader__icon">
							<span class="material-icons">backup</span>
						</div>
						<div class="image-uploader__info">
							<p><strong>${placeholder}</strong></p>
							<p>${GetText('CV_FORMATOIMAGENES')}</p>
							<p>${GetText('CV_PESOMAXIMOIMAGENES', '100')}</p>
						</div>
					</div>
					<div class="image-uploader__error">
						<p class="ko"></p>
					</div>
					<input type="file" class="image-uploader__input">
					<input propertyrdf="${property}" type="hidden">
				</div>`;
	},
	printPropertyEditTextInput: function (property, placeholder, value, required, pDisabled, autocomplete, dependency, autocompleteConfig, pEntity_cv, pMultilang, pValuesMultilang) {
		let css = "";
		if (required) {
			css = "obligatorio";
		}
		if (pEntity_cv) {
			css += " entity_cv";
		}
		let prop_property = 'propertyrdf';
		let disabled = '';
		if (pDisabled && !pEntity_cv) {
			disabled = 'disabled';
			css += " disabled ";
		}

		let action = '';
		let atributesAutocomplete = '';
		if (autocomplete) {
			action = 'addAutocompletar(this)';
			if (autocompleteConfig != null) {
				if (autocompleteConfig.property != null) {
					atributesAutocomplete += ' propertyautocomplete="' + autocompleteConfig.property + '" ';
				}
				if (autocompleteConfig.rdftype != null) {
					atributesAutocomplete += ' rdftypeautocomplete="' + autocompleteConfig.rdftype + '" ';
				}
				if (autocompleteConfig.graph != null) {
					atributesAutocomplete += ' graphautocomplete="' + autocompleteConfig.graph + '" ';
				}
				if (autocompleteConfig.cache != null) {
					atributesAutocomplete += ' cache="' + autocompleteConfig.cache + '" ';
				}
				if (autocompleteConfig.getEntityId) {
					atributesAutocomplete += ' entityidautocomplete="true" ';
				}
				if (autocompleteConfig.propertiesAux != null) {
					atributesAutocomplete += ' propertyautocompleteaux="' + autocompleteConfig.propertiesAux.join('|') + '" ';
				}
				if (autocompleteConfig.printAux != null) {
					atributesAutocomplete += ' propertyautocompleteprint="' + autocompleteConfig.printAux + '" ';
				}
			}
		}

		let htmlDependency = '';
		if (dependency != null) {
			css += ' hasdependency';
			if (dependency.parentDependencyValue != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvalue="${dependency.parentDependencyValue}"`;

			} else if (dependency.parentDependencyValueDistinct != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvaluedistinct="${dependency.parentDependencyValueDistinct}"`;
			}

		}

		let html = '';
		if (pMultilang) {
			css += ' multilang ';
			let cssMulti = css.replace('obligatorio', '').replace('disabled', '');
			languages.forEach(function (language, index) {
				let valorIdioma = '';
				if (pValuesMultilang != null && pValuesMultilang[language] != null) {
					valorIdioma = pValuesMultilang[language];
				}
				html += `	<input ${atributesAutocomplete} multilang="${language}" propertyrdf="${property}" 		placeholder="${placeholder}" value="${valorIdioma.replace(/"/g, "&quot;")}" " onfocus="${action}" type="text" class="form-control not-outline ${cssMulti} " style="display:none" ${htmlDependency}>`;
			});
		}
		html += `<input ${disabled} ${atributesAutocomplete} propertyrdf="${property}" placeholder="${placeholder}" value="${value.replace(/"/g, "&quot;")}" onfocus="${action}" type="text" class="form-control not-outline ${css}" ${htmlDependency}>`;
		return html;
	},
	printPropertyEditEntityAutocomplete: function (property, placeholder, value, required, pDisabled, autocomplete, dependency, autocompleteConfig) {
		var css = "";
		if (required) {
			css = "obligatorio";
		}
		var prop_property = 'propertyrdf';
		var disabled = '';
		if (pDisabled) {
			disabled = 'disabled';
			css += " disabled ";
		}

		var action = '';
		var atributesAutocomplete = '';
		if (autocomplete) {
			action = 'addAutocompletar(this)';
			if (autocompleteConfig != null) {
				if (autocompleteConfig.property != null) {
					atributesAutocomplete += ' propertyautocomplete="' + autocompleteConfig.property + '" ';
				}
				if (autocompleteConfig.rdftype != null) {
					atributesAutocomplete += ' rdftypeautocomplete="' + autocompleteConfig.rdftype + '" ';
				}
				if (autocompleteConfig.graph != null) {
					atributesAutocomplete += ' graphautocomplete="' + autocompleteConfig.graph + '" ';
				}
				if (autocompleteConfig.cache != null) {
					atributesAutocomplete += ' cache="' + autocompleteConfig.cache + '" ';
				}
				if (autocompleteConfig.getEntityId) {
					atributesAutocomplete += ' entityidautocomplete="true" ';
				}
				if (autocompleteConfig.propertiesAux != null) {
					atributesAutocomplete += ' propertyautocompleteaux="' + autocompleteConfig.propertiesAux.join('|') + '" ';
				}
				if (autocompleteConfig.printAux != null) {
					atributesAutocomplete += ' propertyautocompleteprint="' + autocompleteConfig.printAux + '" ';
				}
				atributesAutocomplete += ' entityidautocomplete="true" ';
			}
		}

		var htmlDependency = '';
		if (dependency != null) {
			css += ' hasdependency';
			if (dependency.parentDependencyValue != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvalue="${dependency.parentDependencyValue}"`;

			} else if (dependency.parentDependencyValueDistinct != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvaluedistinct="${dependency.parentDependencyValueDistinct}"`;
			}
		}

		return `<input ${disabled} ${atributesAutocomplete} propertyrdf="${property}_aux" placeholder="${placeholder}" value="${value}" value="${value}" onfocus="${action}" type="text" class="form-control not-outline autocompleteentity ${css}" ${htmlDependency}>`;
	},
	printPropertyEditNumberInput: function (property, placeholder, value, required, pDisabled, dependency, pEntity_cv) {
		var css = "";
		if (required) {
			css = "obligatorio";
		}
		if (pEntity_cv) {
			css += " entity_cv";
		}
		var prop_property = 'propertyrdf';
		var disabled = '';
		if (pDisabled) {
			disabled = 'disabled';
			css += " disabled ";
		}
		var htmlDependency = '';
		if (dependency != null) {
			css += ' hasdependency';

			if (dependency.parentDependencyValue != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvalue="${dependency.parentDependencyValue}"`;

			} else if (dependency.parentDependencyValueDistinct != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvaluedistinct="${dependency.parentDependencyValueDistinct}"`;
			}
		}
		return `<input ${disabled} propertyrdf="${property}" placeholder="${placeholder}" value="${value}" type="number" class="form-control not-outline ${css}" ${htmlDependency}>`;
	},
	printPropertyEditDate: function (property, placeholder, value, required, pDisabled, dependency) {
		var valueDate = "";
		if (value != '') {
			valueDate = value.substring(6, 8) + "/" + value.substring(4, 6) + "/" + value.substring(0, 4);
		}
		var css = "";
		if (required) {
			css = "obligatorio";
		}
		var disabled = '';
		if (pDisabled) {
			disabled = 'disabled';
			css += " disabled ";
		}
		var htmlDependency = '';
		if (dependency != null) {
			if (dependency.parentDependencyValue != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvalue="${dependency.parentDependencyValue}" class="hasdependency"`;

			} else if (dependency.parentDependencyValueDistinct != null) {
				htmlDependency = ` dependencyproperty="${dependency.parent}" dependencypropertyvaluedistinct="${dependency.parentDependencyValueDistinct}" class="hasdependency"`;
			}
		}
		return `<input propertyrdf="${property}" value="${value}" type="hidden" ${htmlDependency}>
		<input ${disabled} propertyrdf="${property}" placeholder="${placeholder}" value="${valueDate}" type="text" class="form-control aux not-outline form-group-date datepicker ${css}">
				<span class="material-icons form-group-date">today</span>`;
	},
	printPropertyEditTextArea: function (property, placeholder, value, required, pDisabled, pEntity_cv, pMultilang, pValuesMultilang) {
		var css = "";
		if (required) {
			css = "obligatorio";
		}
		if (pEntity_cv) {
			css += " entity_cv";
		}
		var disabled = '';
		if (pDisabled && !pEntity_cv) {
			disabled = 'disabled';
			css += " disabled ";
		}


		let html = '';
		if (pMultilang) {
			css += ' multilang ';
			let cssMulti = css.replace('obligatorio', '').replace('disabled', '');
			languages.forEach(function (language, index) {
				let valorIdioma = '';
				if (pValuesMultilang != null && pValuesMultilang[language] != null) {
					valorIdioma = pValuesMultilang[language];
				}
				html += `<div class="edmaTextEditor multilangcontainer ${css.includes("disabled") ? "disabled" : ""}" multilangcontainer="${language}" style="display:none">
                            <div class="toolbar">
                                <div class="line">
                                    <div class="box">
                                        <span class="material-icons editor-btn icon smaller" data-action="bold" data-tag-name="b" title="Bold">
											format_bold
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div multilang="${language}" class="form-control not-outline ${css} visuell-view" propertyrdf="${property}" ${css.includes("disabled") ? "" : "contenteditable"} placeholder="${placeholder}" type="text" >${valorIdioma}</div>
                        </div>`;
				//html+=`<textarea multilang="${language}" propertyrdf="${property}" placeholder="${placeholder}" type="text" class="form-control not-outline ${css}" style="display:none">${valorIdioma}</textarea>`;
			});
		}
		html += `<div ${disabled} class="edmaTextEditor ${css.replace('multilang', 'multilangcontainer')}">
                    <div class="toolbar">
                        <div class="line">
                            <div class="box">
                                <span class="material-icons editor-btn icon smaller" data-action="bold" data-tag-name="b" title="Bold">
                                   format_bold
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-control not-outline visuell-view ${css}" propertyrdf="${property}" ${disabled.length == 0 ? "contenteditable" : ""} placeholder="${placeholder}" type="text">${value}</div>
                </div>`;

		//html+=`<textarea ${disabled} propertyrdf="${property}" placeholder="${placeholder}" type="text" class="form-control not-outline ${css}">${value}</textarea>`;
		return html;
	},
	printSelectCombo: function (property, pId, pItems, pComboDependency, required, pDisabled, pEntity_cv, pDependency) {
		var css = "";
		if (required) {
			css = "obligatorio";
		}
		var disabled = '';
		if (pDisabled && !pEntity_cv) {
			disabled = 'disabled';
			css += " disabled ";
		}
		if (pEntity_cv) {
			css += " entity_cv";
		}
		var dependency = "";
		if (pComboDependency != null) {
			css += " hasdependency";
			dependency = pComboDependency.parent;
		}

		var htmlDependency = '';
		if (pDependency != null) {
			css += ' hasdependency';
			if (pDependency.parentDependencyValue != null) {
				htmlDependency = ` dependencyproperty="${pDependency.parent}" dependencypropertyvalue="${pDependency.parentDependencyValue}"`;

			} else if (pDependency.parentDependencyValueDistinct != null) {
				htmlDependency = ` dependencyproperty="${pDependency.parent}" dependencypropertyvaluedistinct="${pDependency.parentDependencyValueDistinct}"`;
			}
		}



		var selector = `<select ${disabled} ${htmlDependency} propertyrdf="${property}" class="js-select2 ${css}" dependency="${dependency}" data-select-search="true">`;
		for (var propiedad in pItems) {
			var propAux = '';
			if (propiedad == pId) {
				propAux = ' selected ';
			}
			if (pComboDependency != null && propiedad != "") {
				propAux += ' disabled data-dependency="' + pComboDependency.parentDependency[propiedad] + '"';
			}

			selector += `<option ${propAux} value="${propiedad}">${pItems[propiedad]}</option>`;
		}
		selector += "</select>";
		return selector;
	},
	printThesaurus: function (property, thesaurusID, values, pItems, required, pDisabled) {
		var css = "";

		if (required) {
			css = "obligatorio";
		}
		var disabled = '';
		if (pDisabled) {
			disabled = 'disabled';
			css += " disabled ";
		}

		var itemsHijo = $.grep(pItems, function (p) { return p.parentId == ''; });

		var selector = `<div class="buscador-coleccion">
                            <div>
                                <span class="buscar">
                                    <input type="text" value="" class="texto">
                                    <span class="material-icons lupa">search</span>
                                </span>
                            </div>
							<script>
								$(document).ready(function () {
									$(".buscador-coleccion .buscar input").on("focus", function () {
										$(this).val("");
									});
								});
							</script>
						</div>
						<div class="action-buttons-resultados">
                            <ul class="no-list-style">
                                <li class="js-plegar-facetas-modal">
                                    <span class="texto">${GetText("CV_PLEGAR")}</span>
                                    <span class="material-icons">expand_less</span>
                                </li>
                                <li class="js-desplegar-facetas-modal">
                                    <span class="texto">${GetText("CV_DESPLEGAR")}</span>
                                    <span class="material-icons">expand_more</span>
                                </li>
                            </ul>
                        </div>
						<ul class="listadoTesauro partial ${disabled}" thesaurusID = "${thesaurusID}">${this.printThesaurusItemsByParent(values, pItems, itemsHijo, 0)}</ul>`;

		return selector;
	},
	printThesaurusItemsByParent: function (values, pItems, itemsPintar, pLevel) {
		var selector = "";

		for (var id in itemsPintar) {
			var propiedad = itemsPintar[id];
			var itemsHijo = $.grep(pItems, function (p) { return p.parentId == propiedad.id; });
			var classAux = '';
			var propAux = '';
			if (Array.isArray(values)) {
				for (var contador = 0; contador < values.length; contador++) {
					if (values[contador].endsWith('|||')) {
						values[contador] = values[contador].substring(0, values[contador].length - 3);
					}
				}
			} else {
				if (values.EndsWith('|||')) {
					values = values.substring(0, values.length - 3);
					values = values.split('|||');
				}
			}
			if (values != '' && values.find(x => x == propiedad.id) != null) {
				classAux = ' selected ';
			}
			if (itemsHijo.length == 0) {
				classAux += ' last-level ';
			}
			if (propiedad.parentId != "") {
				propAux += ' data-parent="' + propiedad.parentId + '"';
			}
			//selector += `<option level="${pLevel}" ${propAux} value="${propiedad.id}">${propiedad.name}</option>`;


			selector += `<li><a rel="nofollow" ${propAux} name="${propiedad.id}" class="faceta con-subfaceta ocultarSubFaceta ${classAux}" title="${propiedad.name}">`;
			if (itemsHijo.length > 0) {
				selector += `<span class="desplegarSubFaceta"><span class="material-icons">expand_more</span></span>`;
			}
			selector += `<span class="textoFaceta">${propiedad.name}</span></a>`;
			if (itemsHijo.length > 0) {
				selector += `<ul>${this.printThesaurusItemsByParent(values, pItems, itemsHijo, pLevel + 1)}</ul>`;
			}
			selector += `</li>`;
		}

		return selector;
	},
	printAddButton: function () {
		return `	<div class="acciones-listado-edicion">
						<div class="wrap">
							<ul class="no-list-style d-flex align-items-center">
								<li>
									<a class="btn btn-outline-grey add">
										<span class="texto">${GetText('CV_AGNADIR')}</span>
										<span class="material-icons">add</span>
									</a>
								</li>
							</ul>
						</div>
					</div>`;
	},
	printDeleteButton: function () {
		return `	<div class="acciones-listado-edicion">
						<div class="wrap">
							<ul class="no-list-style d-flex align-items-center">
								<li>
									<a class="btn btn-outline-grey delete">
										<span class="texto">${GetText('CV_ELIMINAR')}</span>
										<span class="material-icons">delete</span>
									</a>
								</li>
							</ul>
						</div>
					</div>`;
	},
	repintarTopic: function () {
		$('.topic').each(function () {
			var htmlItmes = '';

			$(this).find(".item.added").each(function () {
				var input = $(this).find('input');
				input.attr('data-value', input.val());

				var background = '';
				var deleteButton = `<span class="material-icons cerrar">close</span>`;
				switch (input.attr('propertyrdf')) {
					case 'http://w3id.org/roh/externalKeywords':
						background = 'background-oscuro';
						deleteButton = '';
						break;
					case 'http://w3id.org/roh/title':
						background = 'background-amarillo';
						break;
					case 'http://w3id.org/roh/userKeywords':
						break;
				}

				htmlItmes += `<li class="${background}" about="${input.attr('propertyrdf')}">
					<a href="javascript: void(0);">
						<span class="texto">${input.val()}</span>
					</a>
					${deleteButton}
				</li>`;
			});

			var htmlTopics = `<div class="simple-collapse-content">
								<div class="resource-list listView">
									<div class="list-wrap tags">
										<ul>
											${htmlItmes}
										</ul>
									</div>
								</div>
							</div>`;

			$(this).find('.simple-collapse-content').remove();
			$(this).append(htmlTopics);

		});


		var listadosTopics = $(".topic .simple-collapse-content .resource-list ul")
		listadosTopics.last().append(listadosTopics.find("li"));
	},
	repintarListadoThesaurus: function () {
		var that = this;

		var listadosTesauros = $(".entityauxcontainer.thesaurus .item.aux.entityaux ul.listadoTesauro")
		listadosTesauros.find('a.faceta.selected').removeClass('selected');

		$(".entityauxcontainer.thesaurus").each(function () {
			if ($(this).attr('idtemp') == null) {
				$(this).attr('idtemp', RandomGuid());
			}

			var valoresTesauro = $.map($(this).find('ul.listadoTesauro .faceta'), function (faceta) {
				return { key: $(faceta).attr('name'), value: $(faceta).attr('title') };
			});

			var iconAdd = "add";

			var htmlAgnadir = `		<div class="acciones-listado acciones-listado-edicion">
										<div class="wrap">
											<ul class="no-list-style d-flex align-items-center">
												<li>
													<a class="btn btn-outline-grey add">
														<span class="texto">${GetText('CV_AGNADIR')}</span>
														<span class="material-icons">${iconAdd}</span>
													</a>
												</li>
											</ul>
										</div>
									</div>`;
			if ($(this).find('ul.listadoTesauro').hasClass('disabled')) {
				htmlAgnadir = '';
			}

			var idTemp = $(this).attr('idtemp');
			$(this).children('.simple-collapse-content').remove();
			var items = $(this).children('.item.added.entityaux');

			var htmlAcciones = `
								<div class="simple-collapse-content">
									${htmlAgnadir}
									<div class="resource-list listView">
										<div class="list-wrap tags">
											<ul>
												${that.repintarListadoThesaursItems(items, idTemp, valoresTesauro)}
											</ul>
										</div>
									</div>
								</div>
							</div>`;

			$(this).append(htmlAcciones);

		});

		var tesauroUser = $(".entityauxcontainer.thesaurus div.item.aux[propertyrdf='http://w3id.org/roh/userKnowledgeArea']").closest('.entityauxcontainer.thesaurus');
		var listadosTesauros = $(".entityauxcontainer.thesaurus");

		//Añadimos enriquecidas al principio
		$(listadosTesauros).each(function () {
			if (tesauroUser.length == 1) {
				var propActual = $(this).find('div.item.aux').attr('propertyrdf');
				if (propActual == "http://w3id.org/roh/enrichedKnowledgeArea") {
					tesauroUser.find('.simple-collapse-content .resource-list ul').prepend($(this).find('.simple-collapse-content .resource-list ul li'));
				}
			}

		});

		//Añadimos externas al principio
		$(listadosTesauros).each(function () {

			if (tesauroUser.length == 1) {
				var propActual = $(this).find('div.item.aux').attr('propertyrdf');
				if (propActual == "http://w3id.org/roh/externalKnowledgeArea") {
					tesauroUser.find('.simple-collapse-content .resource-list ul').prepend($(this).find('.simple-collapse-content .resource-list ul li'));
				}
			}

		});



		this.engancharComportamientosCV();
	},
	repintarListadoThesaursItems: function (items, idTemp, valoresTesauro) {
		var that = this;
		var html = '';
		var num = 1;

		$(items).each(function () {
			html += that.repintarListadoThesaurusyItem($(this), num, idTemp, valoresTesauro);
			num++;
		});
		return html;
	},
	repintarListadoThesaurusyItem: function (item, num, idTemp, valoresTesauro) {
		var that = this;
		/*Cargar*/
		var IdItems = $.map(item.find('.item.added input'), function (input) { return $(input).val() }).sort()
		var idItem = IdItems[IdItems.length - 1];

		if (valoresTesauro.length == 0) {
			return;
		}

		var title = valoresTesauro.find(x => x.key == idItem).value;

		//var listado = item.parent().find('.item.aux.entityaux').find('ul.listadoTesauro');
		var background = '';
		var deleteButton = `<span class="material-icons cerrar">close</span>`;
		var lockCheck = false;
		var propRdfTesauro = item.attr('propertyrdf');
		switch (item.attr('propertyrdf')) {
			case 'http://w3id.org/roh/externalKnowledgeArea':
				background = 'background-oscuro';
				deleteButton = '';
				lockCheck = true;
				propRdfTesauro = 'http://w3id.org/roh/userKnowledgeArea';
				break;
			case 'http://w3id.org/roh/enrichedKnowledgeArea':
				background = 'background-amarillo';
				lockCheck = true;
				propRdfTesauro = 'http://w3id.org/roh/userKnowledgeArea';
				break;
			case 'http://w3id.org/roh/userKnowledgeArea':
				break;
		}

		var listado = $(".entityauxcontainer.thesaurus div.item.aux[propertyrdf='" + propRdfTesauro + "']").closest('.entityauxcontainer.thesaurus');
		$.each(IdItems, function (key, value) {
			var check = listado.find('a.faceta[name="' + value + '"]');
			check.addClass('selected');
			if (lockCheck == true) {
				check.addClass('lock');
			}
		});

		return `<li class="${background}" about="${item.attr('about')}" parent-idtemp="${idTemp}" order="${num}">
					<a href="javascript: void(0);">
						<span class="texto">${title}</span>
					</a>
					${deleteButton}
				</li>`;
	},
	repintarListadoEntity: function (collapseAuthors = false) {
		this.repintarTopic();
		this.repintarListadoThesaurus();
		var that = this;

		$(".entityauxcontainer:not(.thesaurus),.entitycontainer").each(function () {
			var aux = true;
			if ($(this).attr('class').indexOf('entitycontainer') > -1) {
				aux = false;
			}
			var multiple = false;
			if ($(this).hasClass('multiple')) {
				multiple = true;
			}
			if ($(this).attr('idtemp') == null) {
				$(this).attr('idtemp', RandomGuid());
			}

			var idTemp = $(this).attr('idtemp');
			var texto = $('div[idtemp="' + idTemp + '"] input.txtBusqueda').val();
			if (texto == null) {
				texto = '';
			}
			$(this).children('.simple-collapse-content').remove();
			var items = $(this).children('.item.added.entityaux');
			if (!aux) {
				items = $(this).children('.item.added.entity');
				$(this).children('.item.aux:not(.entity)').remove();
			}



			if ($(this).attr('order') != null && $(this).attr('order') != '') {
				var ordenProperty = $(this).attr('order');

				//Ordenamos
				var maxOrder = 1;
				$(items).each(function () {
					var orderIn = $(this).children('input[propertyrdf="' + ordenProperty + '"]').val();
					if (orderIn != '' && parseInt(orderIn) >= maxOrder) {
						maxOrder = parseInt(orderIn);
					}
				});

				items = items.sort(function (a, b) {
					var ordenA = $(a).children('input[propertyrdf="' + ordenProperty + '"]').val();
					if (ordenA == '') {
						$(a).children('input[propertyrdf="' + ordenProperty + '"]').val(maxOrder);
						ordenA = maxOrder;
						maxOrder += 1;
					}
					ordenA = parseInt(ordenA);
					var ordenB = $(b).children('input[propertyrdf="' + ordenProperty + '"]').val();
					if (ordenB == '') {
						$(b).children('input[propertyrdf="' + ordenProperty + '"]').val(maxOrder);
						ordenB = maxOrder;
						maxOrder += 1;
					}
					ordenB = parseInt(ordenB);
					if (ordenA > ordenB) {
						return 1;
					} else if (ordenA < ordenB) {
						return -1;
					} else {
						return 0;
					}
				});
				//Asignamos orden a los que no tengan
				var ordenActual = 0;
				$(items).each(function () {
					if (parseInt($(this).children('input[propertyrdf="' + ordenProperty + '"]').val()) <= ordenActual) {
						$(this).children('input[propertyrdf="' + ordenProperty + '"]').val(ordenActual + 1);
					}
					ordenActual = parseInt($(this).children('input[propertyrdf="' + ordenProperty + '"]').val());
				})

				//Reseteamos ordenes
				var orderActual = 1;
				$(items).each(function () {
					$(this).children('input[propertyrdf="' + ordenProperty + '"]').val(orderActual);
					orderActual++;
				});
			}

			var htmAccionesItems = '';
			if (multiple || aux) {
				if (items.length > 0) {
					if ($(this).attr('selecteditem') == null) {
						$(this).attr('selecteditem', $($(items)[0]).attr('about'));
					}

					if (!$(this).hasClass('disabled')) {
						htmAccionesItems += that.pintarListadoEntityOrden($(this).attr('order'));
					}
					if (!$(this).hasClass('entityauxauthorlist')) {
						htmAccionesItems += `<li>
													<a class="btn btn-outline-grey edit">
														<span class="texto">${GetText("CV_EDITAR")}</span>
														<span class="material-icons">edit</span>
													</a>
												</li>`;
					}
					if (!$(this).hasClass('disabled')) {
						htmAccionesItems += `<li>
												<a class="btn btn-outline-grey delete">
													<span class="texto">${GetText("CV_ELIMINAR")}</span>
													<span class="material-icons">delete</span>
												</a>
											</li>`;
					}
				}
				var iconAdd = "add";
				var classList = "";
				var authorList = false;
				var ocultarEdicionAuthorList = '';
				if ($(this).hasClass('entityauxauthorlist')) {
					iconAdd = "person_add";
					classList = " resource-list-autores";
					authorList = true;
					ocultarEdicionAuthorList = `<a class="collapse-toggle" data-toggle="collapse" href="#collapse-autores" role="button" aria-expanded="true" aria-controls="collapse-autores">
                            ${GetText('CV_OCULTAR_EDICION')}
                        </a>`;
				}
				var htmlAdd = "";
				if (multiple || (aux && items.length == 0)) {
					if (!$(this).hasClass('disabled')) {
						htmlAdd = `	<ul class="no-list-style d-flex align-items-center">
										<li>
											<a class="btn btn-outline-grey add">
												<span class="texto">${GetText('CV_AGNADIR')}</span>
												<span class="material-icons">${iconAdd}</span>
											</a>
										</li>
									</ul>`;
					}
				}
				var htmlSearch = "";
				if (items.length > 1) {

					/*htmlSearch = `	<div id="buscador" class="buscador">
										<input type="text" id="txtBusquedaPrincipal" class="not-outline text txtBusqueda autocompletar personalizado ac_input" placeholder="${GetText('CV_ESCRIBE_ALGO')}" value="${texto}" autocomplete="off">
										<span class="botonSearch">
											<span class="material-icons">search</span>
										</span>
									</div>`;*/
				}

				var htmlAcciones = `
									<div class="simple-collapse-content">
										<div class="acciones-listado acciones-listado-edicion">
											<div class="wrap">
												<ul class="no-list-style d-flex align-items-center">
													${htmAccionesItems}
												</ul>
											</div>
											<div class="wrap">
												${htmlAdd}
												${htmlSearch}
											</div>
										</div>
										<div class="resource-list listView list-con-checks ${classList}">
											<div class="resource-list-wrap">
												${that.repintarListadoEntityItems(items, idTemp, $(this).attr('order'), texto)}
											</div>
										</div>
										${ocultarEdicionAuthorList}
									</div>
								</div>`;
				//Si son los autores de un documento hay que pintarlos diferente
				if (authorList) {
					var authorList = '';

					var autores = $(this).find('div.item.added.entityaux');

					autores = autores.sort(function (a, b) {
						var ordenA = $(a).find('input[propertyrdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#comment"]').val();
						ordenA = parseInt(ordenA);
						var ordenB = $(b).children('input[propertyrdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#comment"]').val();
						ordenB = parseInt(ordenB);
						if (ordenA > ordenB) {
							return 1;
						} else if (ordenA < ordenB) {
							return -1;
						} else {
							return 0;
						}
					});


					$(autores).each(function () {
						var nombre = $(this).find('span[route="http://www.w3.org/1999/02/22-rdf-syntax-ns#member||person||http://xmlns.com/foaf/0.1/name"]').text();
						authorList += `<li>${nombre}</li>`;
					});


					htmlAcciones = `	<div class="simple-collapse-content">
										<div class="collapse ${collapseAuthors ? 'show' : ''}" id="collapse-lista-autores" style="">
											<div class="simple-collapse-content">
												<div class="user-list">
													<ul class="no-list-style d-flex flex-wrap align-items-center">
													${authorList}
													</ul>
													<a class="collapse-toggle collapsed" data-toggle="collapse" href="#collapse-autores" role="button" aria-expanded="false" aria-controls="collapse-autores">
														${GetText('CV_EDITAR')}
													</a>
												</div>
											</div>
										</div>
										<div class="collapse ${collapseAuthors ? '' : 'show'}" id="collapse-autores">${htmlAcciones}</div>
									</div>`;
				}
				$(this).append(htmlAcciones);
			} else {
				htmlAcciones = "";

				if ($($(items)[0]).find('input').val() != '') {
					var htmlEliminar = '';
					if (!$(this).hasClass('disabled')) {
						htmlEliminar = `	<li>
											<a class="btn btn-outline-grey delete">
												<span class="texto">${GetText('CV_ELIMINAR')}</span>
												<span class="material-icons">delete</span>
											</a>
										</li>`
					}
					htmlAcciones += `	<div class="item aux">
										${that.repintarEntityItem($(items[0]))}
											<div class="acciones-listado-edicion">
												<div class="wrap">
													<ul class="no-list-style d-flex align-items-center">
														<li>
															<a class="btn btn-outline-grey edit">
																<span class="texto">${GetText('CV_EDITAR')}</span>
																<span class="material-icons">edit</span>
															</a>
														</li>
														${htmlEliminar}
													</ul>
												</div>
											</div>
										</div>`;
				} else {
					if (!$(this).hasClass('disabled')) {
						htmlAcciones += `	<div class="item aux">
											<input disabled="" value="" type="text" class="form-control not-outline ">	<div class="acciones-listado-edicion">
												<div class="wrap">
													<ul class="no-list-style d-flex align-items-center">
														<li>
															<a class="btn btn-outline-grey add">
																<span class="texto">${GetText('CV_AGNADIR')}</span>
																<span class="material-icons">add</span>
															</a>
														</li>
													</ul>
												</div>
											</div>
										</div>`;
					}
				}
				$(this).append(htmlAcciones);
			}
		});
		this.engancharComportamientosCV();
	},
	repintarListadoEntityItems: function (items, idTemp, propOrden, texto) {
		var that = this;
		var html = '';
		var num = 0;
		$(items).each(function () {
			num++;
			var checked = "";
			if ($(this).closest('.entityauxcontainer').attr('selecteditem') != null &&
				$(this).closest('.entityauxcontainer').attr('selecteditem') != '' &&
				$(this).closest('.entityauxcontainer').children('div[about="' + $(this).closest('.entityauxcontainer').attr('selecteditem') + '"]').length == 1) {
				if ($(this).attr('about') == $(this).closest('.entityauxcontainer').attr('selecteditem')) {
					checked = "checked";
				}
			} else if (num == 1) {
				checked = "checked";
				$(this).closest('.entityauxcontainer').attr('selecteditem', $(this).attr('about'));
			}
			if (propOrden != null && propOrden != '') {
				num = parseInt($(this).children('input[propertyrdf="' + propOrden + '"]').val())
			}
			html += that.repintarListadoEntityItem(this, num, checked, idTemp, texto);
		});
		return html;
	},
	repintarListadoEntityItem: function (item, num, checked, idTemp, texto) {
		var that = this;
		/*Cargar*/
		var itemsLoad = {};
		itemsLoad.items = [];
		itemsLoad.pLang = lang;
		if ($(item).children('span.title[loaded="false"]').length > 0) {
			//TODO unificar co la siguiente
			var entityID = $(item).attr('about');
			var spanTitle = $(item).children('span.title[loaded="false"]');
			var routeCompleteSplitTitle = spanTitle.attr('route').split('||');
			var entityAux = $(item);
			for (var j = 0; j < routeCompleteSplitTitle.length; j++) {
				var prop = $(entityAux).children('.custom-form-row').children('.form-group').children('[propertyrdf="' + routeCompleteSplitTitle[j] + '"]');
				if (prop.is('div') && prop.hasClass('enityaux')) {
					entityAux = prop;
				} else {
					if ($(item).hasClass('entity')) {
						var itemLoad = {};
						if (entityID.indexOf('http') == 0) {
							itemLoad.id = entityID;
							itemLoad.about = entityID;
							itemLoad.route = spanTitle.attr('route');
							itemLoad.routeComplete = spanTitle.attr('route');
							itemsLoad.items.push(itemLoad);
							spanTitle.attr('loaded', 'pending');
							break;
						}
					} else if (prop.is('div') && prop.hasClass('entity')) {
						var itemLoad = {};
						if (prop.children('[propertyrdf="' + routeCompleteSplitTitle[j] + '"]').val() != '') {
							itemLoad.id = prop.children('[propertyrdf="' + routeCompleteSplitTitle[j] + '"]').val();
							itemLoad.about = spanTitle.closest('.entityaux').attr('about');
							itemLoad.route = "";
							itemLoad.routeComplete = spanTitle.attr('route');
							for (var j2 = j + 1; j2 < routeCompleteSplitTitle.length; j2++) {
								itemLoad.route += routeCompleteSplitTitle[j2] + "||";
							}
							itemLoad.route = itemLoad.route.substring(0, itemLoad.route.length - 2);
							itemsLoad.items.push(itemLoad);
							spanTitle.attr('loaded', 'pending');
							break;
						}
					} else if (prop.is('select')) {
						if (prop.val() != '') {
							spanTitle.text(prop.find('option:selected').text());
						}
					} else if (j + 1 == routeCompleteSplitTitle.length && prop.val() != '') {
						spanTitle.text(prop.val());
					}
					spanTitle.attr('loaded', 'true');
					break;
				}
			}
		}
		if ($(item).children('span.property[loaded="false"]').length > 0) {
			$(item).children('span.property[loaded="false"]').each(function () {
				var entityID = $(item).attr('about');
				var entityAux = $(item);
				var spanProperty = $(this);
				var routeCompleteSplitProperty = $(this).attr('route').split('||');
				for (var k = 0; k < routeCompleteSplitProperty.length; k++) {
					var prop = $(entityAux).children('.custom-form-row').children('.form-group').children('[propertyrdf="' + routeCompleteSplitProperty[k] + '"]');
					if (prop.is('div') && prop.hasClass('enityaux')) {
						entityAux = prop;
					} else {
						if ($(item).hasClass('entity')) {
							var itemLoad = {};
							if (entityID.indexOf('http') == 0) {
								itemLoad.id = entityID;
								itemLoad.about = entityID;
								itemLoad.route = spanProperty.attr('route')
								itemLoad.routeComplete = spanProperty.attr('route');
								itemsLoad.items.push(itemLoad);
								spanProperty.attr('loaded', 'pending');
								break;
							}
						} else if (prop.is('div') && prop.hasClass('entity')) {
							var itemLoad = {};
							if (prop.children('[propertyrdf="' + routeCompleteSplitProperty[k] + '"]').val() != '') {
								itemLoad.id = prop.children('[propertyrdf="' + routeCompleteSplitProperty[k] + '"]').val();
								itemLoad.about = spanProperty.closest('.entityaux').attr('about');
								itemLoad.route = "";
								itemLoad.routeComplete = spanProperty.attr('route');
								for (var k2 = k + 1; k2 < routeCompleteSplitProperty.length; k2++) {
									itemLoad.route += routeCompleteSplitProperty[k2] + "||";
								}
								itemLoad.route = itemLoad.route.substring(0, itemLoad.route.length - 2);
								itemsLoad.items.push(itemLoad);
								spanProperty.attr('loaded', 'pending');
								break;
							}
						} else if (prop.is('select')) {
							if (prop.val() != '') {
								spanProperty.text(prop.find('option:selected').text());
							}
						} else if (k + 1 == routeCompleteSplitProperty.length && prop.val() != '') {
							spanProperty.text(prop.val());
						}
						spanProperty.attr('loaded', 'true');
						break;
					}
				}

			});
		}
		if (itemsLoad.items.length > 0) {
			MostrarUpdateProgress();
			$.post(urlEdicionCV + 'LoadProps', itemsLoad, function (data) {
				for (var i = 0; i < data.items.length; i++) {
					var contenedor = $('div[about="' + data.items[i].about + '"]').children('span[route="' + data.items[i].routeComplete + '"]');
					contenedor.attr('loaded', 'true');
					contenedor.text(data.items[i].values.join(', '));
				};
				that.repintarListadoEntity();
				OcultarUpdateProgress();
			});
		}


		var title = "";
		var props = "";
		var mostrar = false;
		if (texto == '') {
			mostrar = true;
		} else {
			texto = texto.toLowerCase();
		}
		if ($(item).children('span.title[loaded="true"]').length > 0) {
			title = $(item).children('span.title[loaded="true"]').text();
			if (!mostrar && title.toLowerCase().indexOf(texto) > -1) {
				mostrar = true;
			}
		}
		if ($(item).children('span.property[loaded="true"]').length > 0) {
			$(item).children('span.property[loaded="true"]').each(function () {
				if ($(this).text().trim() != '') {
					props += '<p>';
					if ($(this).attr('name') != null && $(this).attr('name') != '') {
						props += $(this).attr('name') + ": ";
					}
					props += $(this).text().trim();
					props += '</p>';
					if (!mostrar && $(this).text().trim().toLowerCase().indexOf(texto) > -1) {
						mostrar = true;
					}
				}
			});
		}
		if (mostrar) {
			return `<article class="resource" about="${$(item).attr('about')}" order="${num}">
					<div class="custom-control themed little custom-radio">
						<input type="radio" id="edicion-listado-${idTemp}${num}" name="edicion-listado-${idTemp}" class="custom-control-input" ${checked}>
						<label class="custom-control-label" for="edicion-listado-${idTemp}${num}"></label>
					</div>
					<div class="wrap">
						<div class="middle-wrap">
							<div class="title-wrap">
								<h2 class="resource-title">${title}</h2>
							</div>
							<div class="content-wrap">
								<div class="description-wrap">
									<div class="desc">
									${props}
									</div>
								</div>
							</div>
						</div>
					</div>
				</article>`;
		} else {
			return '';
		}
	},
	repintarEntityItem: function (item) {
		var that = this;
		/*Cargar*/
		var itemsLoad = {};
		itemsLoad.items = [];
		itemsLoad.pLang = lang;
		if ($(item).children('span.title[loaded="false"]').length > 0) {
			var spanTitle = $(item).children('span.title[loaded="false"]');

			var itemLoad = {};
			itemLoad.id = $(item).closest('.entity').attr('about');
			itemLoad.about = $(item).closest('.entity').attr('about');
			itemLoad.route = spanTitle.attr('route');
			itemsLoad.items.push(itemLoad);
			spanTitle.attr('loaded', 'pending');
		}
		if ($(item).children('span.property[loaded="false"]').length > 0) {
			$(item).children('span.property[loaded="false"]').each(function () {
				var spanProperty = $(this);

				var itemLoad = {};
				itemLoad.id = $(this).closest('.entity').attr('about');
				itemLoad.about = $(this).closest('.entity').attr('about');
				itemLoad.route = $(this).attr('route');
				itemsLoad.items.push(itemLoad);
				spanProperty.attr('loaded', 'pending');
			});
		}
		if (itemsLoad.items.length > 0) {
			MostrarUpdateProgress();
			$.post(urlEdicionCV + 'LoadProps', itemsLoad, function (data) {
				for (var i = 0; i < data.items.length; i++) {
					var contenedor = $('div[about="' + data.items[i].about + '"]').children('span[loaded="pending"][route="' + data.items[i].route + '"]');
					contenedor.attr('loaded', 'true');
					contenedor.text(data.items[i].values.join(', '));
				};
				that.repintarListadoEntity();
				OcultarUpdateProgress();
			});
		}


		var title = "";
		var props = "";
		if ($(item).children('span.title[loaded="true"]').length > 0) {
			title = $(item).children('span.title[loaded="true"]').text();
		}
		if ($(item).children('span.property[loaded="true"]').length > 0) {
			$(item).children('span.property[loaded="true"]').each(function () {
				if ($(this).text().trim() != '') {
					props += ' ';
					if ($(this).attr('name') != null && $(this).attr('name') != '') {
						props += $(this).attr('name') + ": ";
					}
					props += $(this).text().trim();
				}
			});
		}
		return `<input disabled="" value="${title}${props}" type="text" class="form-control not-outline ">`;
	},
	pintarListadoEntityOrden: function (propOrder) {
		if (propOrder != null && propOrder != '') {
			return `<li>
						<a class="btn btn-outline-grey subir">
							<span class="texto">Subir</span>
							<span class="material-icons">arrow_drop_up</span>
						</a>
					</li>
					<li>
						<a class="btn btn-outline-grey bajar">
							<span class="texto">Bajar</span>
							<span class="material-icons">arrow_drop_down</span>
						</a>
					</li>`;
		}
		return "";
	},
	cargarEdicionEntidad: function (button, rdfType, entityID) {
		var that = this;
		MostrarUpdateProgress();
		var modalPopUp = $(button).closest('.modal-top').attr('id');
		if (modalPopUp == 'modal-editar-entidad') {
			modalPopUp = 'modal-editar-entidad-0'
		} else {
			modalPopUp = 'modal-editar-entidad-' + (parseInt(modalPopUp.substring(21)) + 1);
		}
		modalPopUp = '#' + modalPopUp;
		$(modalPopUp).modal('show');
		$(modalPopUp + ' form').empty();
		$(modalPopUp + ' .form-actions .ko').remove();
		$.get(urlEdicionCV + 'GetEditEntity?pRdfType=' + rdfType + "&pEntityID=" + entityID + "&pLang=" + lang, null, function (data) {
			that.printEditEntity(modalPopUp, data, rdfType);
			OcultarUpdateProgress();
		});
	},
	cargarAreasEtiquetasEnriquecidas: function (titulo, descripcion, documento) {
		var that = this;

		// Objeto con datos a enviar.
		var data = new Object();
		data.pTitulo = titulo;
		data.pDesc = descripcion;
		data.pUrlPdf = documento;

		MostrarUpdateProgress();

		//Pintado tesauro y asignacion de etiquetas asociadas al mismo
		var ul = $('ul.listadoTesauro.partial');
		conseguirTesauro('researcharea', lang, '', ul, false, false, true, that, data);

	},
	cambiarIdiomaEdicion: function (lang, contenedor) {
		if (lang == 'es') {
			//Ocultamos todos los campos multiidioma que tienen idioma
			contenedor.find('input.multilang[multilang],div.multilang[multilang], div.multilangcontainer[multilangcontainer]').hide();
			//Mostramos todos los campos multiidioma que no tienen idioma
			contenedor.find('input.multilang:not([multilang]),div.multilang:not([multilang]), div.multilangcontainer:not([multilangcontainer])').show();
			//Hablitamos todos los campos q(que no sean campos deshabilitados)
			contenedor.find('input:not(.disabled),select:not(.disabled)').removeAttr('disabled');
			contenedor.find('div.edmaTextEditor:not(.disabled)').removeAttr('disabled').find('.visuell-view').attr('contenteditable', "true");
		} else {
			//Ocultamos todos los campos multiidioma
			contenedor.find('input.multilang,div.multilang,div.multilangcontainer').hide();
			//Mostramos todos los campos multiidioma que tienen el idioma seleccionado
			contenedor.find(`input.multilang[multilang="${lang}"],div.multilang[multilang="${lang}"],div.multilangcontainer[multilangcontainer="${lang}"]`).show();
			//Deshablitamos todos los campos que no son multiidioma
			contenedor.find('input:not(.multilang),select,div.edmaTextEditor:not(.multilangcontainer)').attr('disabled', 'disabled');
			contenedor.find('div.edmaTextEditor:not(.multilangcontainer)').find('.visuell-view').removeAttr('contenteditable');
		}
	},
	//Fin de métodos de edición
	engancharComportamientosCV: function (onlyPublic = false) {
		$('select').unbind("change.selectitem").bind("change.selectitem", function () {
			var valor = $(this).val();
			$(this).find('option[value="' + valor + '"]').attr('selected', '');
			$(this).find('option[value!="' + valor + '"]').removeAttr('selected');
		});

		$('.select2-container').remove();
		iniciarSelects2.init();
		iniciarDatepicker.init();
		plegarSubFacetas.init();
		operativaFormularioTesauro.init();
		iniciarComportamientoImagenUsuario.init();
		edicionListaAutorCV.init();
		var that = this;

		if (onlyPublic) {
			//LISTADOS
			//Paginación
			$('.panel-group .panNavegador ul.pagination li a').off('click').on('click', function (e) {
				if ($(this).attr('page') != null) {
					var sectionID = $(this).closest('.panel-group').attr('section');
					var pagina = $(this).attr('page');
					that.paginarListado(sectionID, pagina);
				}
			});
			//Cambiar tipo de paginación
			$('.panel-group .panNavegador .item-dropdown').off('click').on('click', function (e) {
				if ($(this).attr('items') != null) {
					var sectionID = $(this).closest('.panel-group').attr('section');
					var itemsPagina = parseInt($(this).attr('items'));
					var texto = $(this).text();
					that.cambiarTipoPaginacionListado(sectionID, itemsPagina, texto);
				}
			});
			//Buscador
			$('.panel-group input.txtBusqueda').off('keyup').on('keyup', function (e) {
				var sectionID = $(this).closest('.panel-group').attr('section');
				var ignoreKeysToBuscador = [37, 38, 39, 40, 91, 17, 18, 20, 36, 18, 27];
				if (!ignoreKeysToBuscador.find(key => key == e.keyCode)) {
					clearTimeout(that.timer);
					that.timer = setTimeout(function () {
						// Ocultar panel sin resultados por posible busqueda anterior sin resultados
						that.buscarListado(sectionID);
					}, 150);
				}
			});
			//Ordenar
			$('.panel-group .ordenar.dropdown.orders .dropdown-menu a').off('click').on('click', function (e) {
				var sectionID = $(this).closest('.panel-group').attr('section');
				var dropdown = $(this).closest('.ordenar.dropdown.orders').find('.dropdown-toggle .texto');
				that.ordenarListado(sectionID, $(this).text(), $(this).attr('property'), $(this).attr('asc'), dropdown);
			});
			//Fechas hidden
			$('input.datepicker').off('change').on('change', function (e) {
				if ($(this).val().length == 10) {
					$(this).parent().children('input[type="hidden"]').val($(this).val().substring(6, 10) + $(this).val().substring(3, 5) + $(this).val().substring(0, 2) + "000000");
				} else {
					$(this).parent().children('input[type="hidden"]').val('');
				}
			});
			//Seleccion combos
			$('.entityauxcontainer>.simple-collapse-content input').off('change').on('change', function (e) {
				$(this).closest('.entityauxcontainer').attr('selecteditem', $(this).closest('article').attr('about'));
			});
			//Comportamiento desplegar sección no cargada
			$('div.panel-group.pmd-accordion.notLoaded').off('click').on('click', function (e) {
				var about = $(this).parent().prev().attr('about');
				var rdftype = $(this).parent().prev().attr('rdftype');
				var section = $(this).attr('section');
				that.completeTab(about, rdftype, section, true);
			});
			return;
		}
		//Boton duplicados
		$('.btn.gestionar-duplicados').unbind().click(function () {
			duplicadosCV.init(true);
		});
		$('.listadoTesauro .faceta:not(.last-level)').unbind("click").bind("click", function () {
			$(this).find('.desplegarSubFaceta .material-icons').trigger('click');
		});
		$('.listadoTesauro .faceta.last-level:not(.lock)').unbind("click").bind("click", function () {
			var faceta = $(this);

			var addOrRemove = !faceta.hasClass('selected');

			while (faceta != null) {
				if (addOrRemove || faceta.parent().find('ul .faceta.selected').length > 0) {
					faceta.addClass('selected');
				} else {
					faceta.removeClass('selected');
				}

				var listado = faceta.closest('ul');
				if (!listado.hasClass('listadoTesauro')) {
					faceta = listado.prev();
				} else {
					faceta = null;
				}
			}

			//$(this).closest('.formulario-edicion').next().find('a').trigger('click');
		});

		//LISTADOS
		//Paginación
		$('.panel-group .panNavegador ul.pagination li a').off('click').on('click', function (e) {
			if ($(this).attr('page') != null) {
				var sectionID = $(this).closest('.panel-group').attr('section');
				var pagina = $(this).attr('page');
				that.paginarListado(sectionID, pagina);
			}
		});
		//Cambiar tipo de paginación
		$('.panel-group .panNavegador .item-dropdown').off('click').on('click', function (e) {
			if ($(this).attr('items') != null) {
				var sectionID = $(this).closest('.panel-group').attr('section');
				var itemsPagina = parseInt($(this).attr('items'));
				var texto = $(this).text();
				that.cambiarTipoPaginacionListado(sectionID, itemsPagina, texto);
			}
		});
		//Buscador
		$('.panel-group input.txtBusqueda').off('keyup').on('keyup', function (e) {
			var sectionID = $(this).closest('.panel-group').attr('section');
			var ignoreKeysToBuscador = [37, 38, 39, 40, 91, 17, 18, 20, 36, 18, 27];
			if (!ignoreKeysToBuscador.find(key => key == e.keyCode)) {
				clearTimeout(that.timer);
				that.timer = setTimeout(function () {
					// Ocultar panel sin resultados por posible busqueda anterior sin resultados
					that.buscarListado(sectionID);
				}, 150);
			}
		});
		//Ordenar
		$('.panel-group .ordenar.dropdown.orders .dropdown-menu a').off('click').on('click', function (e) {
			var sectionID = $(this).closest('.panel-group').attr('section');
			var dropdown = $(this).closest('.ordenar.dropdown.orders').find('.dropdown-toggle .texto');
			that.ordenarListado(sectionID, $(this).text(), $(this).attr('property'), $(this).attr('asc'), dropdown);
		});
		//Publicar/despublicar
		$('.panel-group .resource-list .publicaritem,.panel-group .resource-list .despublicaritem').parents("li").off('click').on('click', function (e) {
			var textoPublicar = $(this).find('.texto');
			var sectionID = textoPublicar.closest('.panel-group').attr('section');
			var rdfTypeTab = textoPublicar.closest('.cvTab').attr('rdftype');
			var entityID = textoPublicar.attr('data-id');
			var isPublic = true;
			if (textoPublicar.hasClass('despublicaritem')) {
				isPublic = false;
			}
			var element = $(textoPublicar);
			that.cambiarPrivacidadItem(sectionID, rdfTypeTab, entityID, isPublic, element);
		});
		//Publicar/despublicar icono
		$('.panel-group .resource-list .visibility-wrapper').off('click').on('click', function (e) {
			$(this).parent().find('.publicaritem, .despublicaritem').closest('li').click();
		});
		//Eliminar item
		$('.panel-group .resource-list .eliminar').parents("li").off('click').on('click', function (e) {
			//Usa el popup
			$("#modal-eliminar").modal("show");
			var textoEliminar = $(this).find('.texto');
			var sectionID = textoEliminar.closest('.panel-group').attr('section');
			var entityID = textoEliminar.attr('data-id');

			var message = "";
			$("#modal-eliminar").find(".ko").remove();
			if (textoEliminar.closest('article').find('.block-wrapper').length &&
				(
					sectionID == "http://w3id.org/roh/scientificPublications" ||
					sectionID == "http://w3id.org/roh/worksSubmittedConferences" ||
					sectionID == "http://w3id.org/roh/worksSubmittedSeminars"
				)
			) {
				$("#modal-eliminar").find(".form-actions").before(`<div class="ko" style="display:block"><p>${GetText("CV_ALERTA_ELIMINACION_BLOQUEADO")}</p></div>`);
			}

			$('#modal-eliminar .btn-outline-primary').attr('href', 'javascript:edicionCV.eliminarItem("' + sectionID + '","' + entityID + '");$("#modal-eliminar").modal("hide");');
		});


		//Fechas hidden
		$('input.datepicker').off('change').on('change', function (e) {
			if ($(this).val().length == 10) {
				$(this).parent().children('input[type="hidden"]').val($(this).val().substring(6, 10) + $(this).val().substring(3, 5) + $(this).val().substring(0, 2) + "000000");
			} else {
				$(this).parent().children('input[type="hidden"]').val('');
			}
		});
		//Seleccion combos
		$('.entityauxcontainer>.simple-collapse-content input').off('change').on('change', function (e) {
			$(this).closest('.entityauxcontainer').attr('selecteditem', $(this).closest('article').attr('about'));
		});

		//Cargar edición/creación de item
		$('.panel-group .resource-list h2.resource-title a,.panel-group .acciones-listado a.aniadirEntidad').off('click').on('click', function (e) {
			e.preventDefault();
			$('#modal-editar-entidad').modal('show');
			var sectionID = $(this).closest('.panel-group').attr('section');
			var entityID = "";
			if ($(this).attr('data-id') != null) {
				entityID = $(this).attr('data-id');
			}
			var rdfTypeTab = $(this).closest('.cvTab').attr('rdftype');
			that.cargarEdicionItem(sectionID, rdfTypeTab, entityID);
		});


		//Añadir propiedad multiple que no es otra entidad
		$('.multiple:not(.entityauxcontainer ):not(.entitycontainer ) .acciones-listado-edicion .add').off('click').on('click', function (e) {
			var contenedor = $(this).closest('.multiple');
			var item = $(this).closest('.item');
			if (item.find('input, select').val() == '' || item.find('div.visuell-view').html() == '') {
				return;
			}
			var itemClone = item.clone();
			itemClone.find('input, select, div.visuell-view').each(function (index) {
				if ($(this).attr('propertyrdf_aux') != null) {
					$(this).attr('propertyrdf', $(this).attr('propertyrdf_aux'));
					$(this).removeAttr('propertyrdf_aux');
				}
			});
			itemClone.removeClass('aux');
			itemClone.addClass('added');
			itemClone.find('input, select, div.edmaTextEditor').attr('disabled', ''); //TODO TEST EdmaTextEditor TextArea ;
			itemClone.find('div.edmaTextEditor').find('div.visuell-view').removeAttr('contenteditable');
			itemClone.find('.acciones-listado-edicion').replaceWith(that.printDeleteButton());
			contenedor.append(itemClone);
			itemClone.find('input, select').val(item.find('input, select').val());
			itemClone.find('div.visuell-view').html(item.find('div.visuell-view').html());
			item.find('input, select').val('');
			item.find('div.visuell-view').html('');
			item.find('input, select, div.visuell-view').change();


			if (contenedor.hasClass('topic')) {
				edicionCV.repintarTopic();
			}
			if ($(this).closest('div[rdftype="http://w3id.org/roh/CategoryPath"]').length == 0) {
				that.engancharComportamientosCV();
			}
		});

		//Eliminar propiedad multiple que no es una entidad
		$('.multiple:not(.entityauxcontainer ):not(.entitycontainer ) .acciones-listado-edicion .delete').off('click').on('click', function (e) {
			var item = $(this).closest('.item').remove();
		});

		//Mostrar popup entidad nueva/editar auxiliar/editar especiales
		//$('.multiple.entityauxcontainer .acciones-listado-edicion .add,.multiple.entityauxcontainer .acciones-listado-edicion .edit').off('click').on('click', function (e) {
		$('.entityauxcontainer .acciones-listado-edicion .add,.entityauxcontainer .acciones-listado-edicion .edit').off('click').on('click', function (e) {
			MostrarUpdateProgress();
			var edit = $(this).hasClass('edit');

			// Mostrar el tesauro
			var listadoInputSeleccionados = $(this).closest('.form-group.full-group.multiple.entityauxcontainer.thesaurus').find('.item.added').find('input');
			var listadoValoresSeleccionados = "";
			for (var i = 0; i < listadoInputSeleccionados.length; i++) {
				var item = listadoInputSeleccionados[i].value;
				if (item != null && item != '') {
					listadoValoresSeleccionados += listadoInputSeleccionados[i].value + "|||";
				}
			}
			var ul = $(this).closest('.form-group.full-group.multiple.entityauxcontainer.thesaurus').find('ul.listadoTesauro.partial');
			if ($(this).closest('.form-group.full-group.multiple.entityauxcontainer.thesaurus').find('ul.listadoTesauro.partial').length != 0) {
				var tesauro = $(this).closest('.form-group.full-group.multiple.entityauxcontainer.thesaurus').find('ul.listadoTesauro.partial')[0].getAttribute('thesaurusID');
			}
			if (ul.length != 0 && tesauro.length != 0) {
				conseguirTesauro(tesauro, lang, listadoValoresSeleccionados, ul, edit, true, false, null, null);
			}


			if ($(this).closest('.entityauxauthorlist').length > 0) {
				if (!edit) {
					//Creación
					$('#modal-anadir-autor .formulario-edicion .resultados').show();
					$('#modal-anadir-autor .formulario-edicion .form-actions').show();
					$('#modal-anadir-autor').modal('show');
					$('#modal-anadir-autor .ko').hide();
					$('#modal-anadir-autor .resultados .form-group.full-group').remove();
					if ($('div.added.entityaux[propertyrdf="http://purl.org/ontology/bibo/authorList"] div.added.entity[about="' + edicionCV.idPerson + '"]').length) {
						$('#inputsignatures').val('');
					} else {
						$('#inputsignatures').val($('#namePersonCV').val() + ';');
					}
					$('#inputsignatures').removeAttr('disabled');
					$('#modal-anadir-autor .validar').removeAttr('disabled');
					$('#modal-anadir-autor .validar').text(GetText('CV_BUSCAR'));
					$('#modal-anadir-autor .validar').removeAttr('reset');
					$('#modal-anadir-autor').attr('propertyrdf', $(this).closest('.entityauxauthorlist').find('.item.aux.entityaux').attr('propertyrdf'));
				} else {
					//Edición
				}
			} else {
				pintadoTesauro($(this), edit);
			}
			that.repintarListadoEntity();
			OcultarUpdateProgress();
		});


		//Eliminar entidad auxiliar/normal de listado
		//$('.multiple.entityauxcontainer .acciones-listado-edicion .delete,.multiple.entitycontainer .acciones-listado-edicion .delete').off('click').on('click', function (e) {
		$('.entityauxcontainer .acciones-listado-edicion .delete,tiple.entitycontainer .acciones-listado-edicion .delete').off('click').on('click', function (e) {
			$("#modal-eliminar").modal("show");
			//var entityAux=$(this).closest('.multiple').hasClass('entityauxcontainer');
			var entityAux = $(this).closest('.form-group').hasClass('entityauxcontainer');

			//Usa el popup
			var modalID = $(this).closest('.modal').attr('id');
			var contenedor = $(this).closest('.entitycontainer');
			if (entityAux) {
				contenedor = $(this).closest('.entityauxcontainer');
			}
			var idTemp = $(contenedor).attr('idtemp');
			var id = $('input[name="edicion-listado-' + idTemp + '"]:checked').closest('article').attr('about');
			$('#modal-eliminar .btn-outline-primary').attr('href', 'javascript:$("#' + modalID + ' div[idtemp=\'' + idTemp + '\'] div[about=\'' + id + '\']").remove();$("#' + modalID + ' div[idtemp=\'' + idTemp + '\']").attr(\'remove\',$("div[idtemp=\'' + idTemp + '\']").attr(\'remove\')+\'||' + id + '\');edicionCV.repintarListadoEntity();$("#modal-eliminar").modal("hide");');
		});

		//Eliminar entidad normal monovaluada
		$('.entitycontainer>div.item.aux .delete').off('click').on('click', function (e) {
			$("#modal-eliminar").modal("show");
			//Usa el popup
			var modalID = $(this).closest('.modal').attr('id');
			var contenedor = $(this).closest('.entitycontainer');
			var idTemp = $(contenedor).attr('idtemp');
			$('#modal-eliminar .btn-outline-primary').attr('href', 'javascript:$("#' + modalID + ' div[idtemp=\'' + idTemp + '\']>div.item.added.entity").attr("about","");$("#' + modalID + ' div[idtemp=\'' + idTemp + '\']>div.item.added.entity>input").val("");$("#' + modalID + ' div[idtemp=\'' + idTemp + '\']>div.item.added.entity>span").attr("loaded","true");$("#' + modalID + ' div[idtemp=\'' + idTemp + '\']>div.item.added.entity>span").html("");edicionCV.repintarListadoEntity();$("#modal-eliminar").modal("hide");');
		});

		//Eliminar entidad tesauro
		$('.multiple.entityauxcontainer.thesaurus .list-wrap.tags .cerrar').off('click').on('click', function (e) {
			$("#modal-eliminar").modal("show");
			//Usa el popup
			var modalID = $(this).closest('.modal').attr('id');

			var idTemp = $(this).closest('li').attr('parent-idtemp');
			var id = $(this).closest('li').attr('about');
			//$('#modal-eliminar .btn-outline-primary').attr('href', 'javascript:$("#'+modalID+' div[idtemp=\''+idTemp+'\'] div[about=\''+id+'\']").remove();$("#'+modalID+' div[idtemp=\''+idTemp+'\']").attr(\'remove\',$("div[idtemp=\''+idTemp+'\']").attr(\'remove\')+\'||'+id+'\');edicionCV.repintarListadoEntity();$("#modal-eliminar").modal("hide");');

			$('#modal-eliminar .btn-outline-primary').attr('href', 'javascript:edicionCV.eliminarEntidadTesauro("' + modalID + '", "' + idTemp + '", "' + id + '")');
		});

		//Eliminar topic
		$('.topic .list-wrap.tags .cerrar').off('click').on('click', function (e) {
			$("#modal-eliminar").modal("show");
			//Usa el popup
			var modalID = $(this).closest('.modal').attr('id');
			var item = $(this).closest('li');
			var propRdf = item.attr('about');
			var value = item.find('.texto').text();

			$('#modal-eliminar .btn-outline-primary').attr('href', 'javascript:edicionCV.eliminarEntidadTopic("' + modalID + '", "' + propRdf + '", "' + value + '")');
		});

		//Mover orden entidad auxiliar
		$('.multiple.entityauxcontainer .acciones-listado-edicion .subir,.multiple.entityauxcontainer .acciones-listado-edicion .bajar').off('click').on('click', function (e) {
			var contenedor = $(this).closest('.entityauxcontainer');
			var idTemp = $(contenedor).attr('idtemp');

			var subir = $(this).hasClass('subir');
			var idSeleccionado = $('input[name="edicion-listado-' + idTemp + '"]:checked').closest('article').attr('about');
			var ordenSeleccionado = $('input[name="edicion-listado-' + idTemp + '"]:checked').closest('article').attr('order');
			var propertyOrder = $(contenedor).attr('order');
			var valorSeleccionado = parseInt($(contenedor).children('div[about="' + idSeleccionado + '"]').children('input[propertyrdf="' + propertyOrder + '"]').val());
			var idAnterior = null;
			var ordenAnterior = null;
			var actualProcesado = false;
			var idSiguiente = null;
			var ordenSiguiente = null;
			var items = $(contenedor).children('.simple-collapse-content').find('article');
			$(items).each(function () {
				var actual = $(this).attr('about');
				if (actual == idSeleccionado) {
					actualProcesado = true;
					return;
				}
				if (!actualProcesado) {
					idAnterior = actual;
					ordenAnterior = $(this).attr('order');
				}
				if (actualProcesado && idSiguiente == null) {
					idSiguiente = actual;
					ordenSiguiente = $(this).attr('order');
				}
			});
			if (subir && idAnterior != null) {
				$(contenedor).children('.item.added.entityaux[about="' + idAnterior + '"]').children('input[propertyrdf="' + propertyOrder + '"]').val(ordenSeleccionado);
				$(contenedor).children('.item.added.entityaux[about="' + idSeleccionado + '"]').children('input[propertyrdf="' + propertyOrder + '"]').val(ordenAnterior);
			} else if (!subir && idSiguiente != null) {
				$(contenedor).children('.item.added.entityaux[about="' + idSiguiente + '"]').children('input[propertyrdf="' + propertyOrder + '"]').val(ordenSeleccionado);
				$(contenedor).children('.item.added.entityaux[about="' + idSeleccionado + '"]').children('input[propertyrdf="' + propertyOrder + '"]').val(ordenSiguiente);
			}
			$(contenedor).attr('selecteditem', idSeleccionado);
			that.repintarListadoEntity();
		});

		//Mostrar popup entidad nueva/editar listado
		$('.multiple.entitycontainer .acciones-listado-edicion .add,.multiple.entitycontainer .acciones-listado-edicion .edit').off('click').on('click', function (e) {
			var modalPopUp = $(this).closest('.modal-top').attr('id')
			if (modalPopUp == 'modal-editar-entidad') {
				modalPopUp = 'modal-editar-entidad-0'
			} else {
				modalPopUp = 'modal-editar-entidad-' + (parseInt(modalPopUp.substring(21)) + 1);
			}
			modalPopUp = '#' + modalPopUp;
			$(modalPopUp).modal('show');


			var edit = $(this).hasClass('edit');
			//IDS
			var contenedor = $(this).closest('.entitycontainer');
			var idTemp = $(contenedor).attr('idtemp');
			$(modalPopUp).attr('idtemp', idTemp);
			var id = RandomGuid();
			if (edit) {
				id = $('input[name="edicion-listado-' + idTemp + '"]:checked').closest('article').attr('about');
			}
			that.cargarEdicionEntidad($(this), $(contenedor).attr("rdftype"), id);
			that.repintarListadoEntity();
		});

		//Mostrar popup entidad nueva/editar NO listado
		$('.entitycontainer:not(.multiple) .acciones-listado-edicion .add,.entitycontainer:not(.multiple) .acciones-listado-edicion .edit').off('click').on('click', function (e) {
			var modalPopUp = $(this).closest('.modal-top').attr('id')
			if (modalPopUp == 'modal-editar-entidad') {
				modalPopUp = 'modal-editar-entidad-0'
			} else {
				modalPopUp = 'modal-editar-entidad-' + (parseInt(modalPopUp.substring(21)) + 1);
			}
			modalPopUp = '#' + modalPopUp;
			$(modalPopUp).modal('show');

			var edit = $(this).hasClass('edit');
			//IDS
			var contenedor = $(this).closest('.entitycontainer');
			var idTemp = $(contenedor).attr('idtemp');
			$(modalPopUp).attr('idtemp', idTemp);
			var id = RandomGuid();
			if (edit) {
				id = $(contenedor).find('.item.added.entity').attr('about');
			}
			that.cargarEdicionEntidad($(this), $(contenedor).attr("rdftype"), id);
			that.repintarListadoEntity();

			if ($(modalPopUp + ' .formulario-edicion>div>ul.listadoTesauro').length > 0) {
				$(modalPopUp + ' .formulario-edicion>div>div.custom-form-row').hide();
				$(modalPopUp).addClass('modal-con-buscador');
			} else {
				$(modalPopUp + ' .formulario-edicion>div>div.custom-form-row').show();
				$(modalPopUp).removeClass('modal-con-buscador');
			}
		});

		//Buscar personas por firma
		$('#modal-anadir-autor .validar').off('click').on('click', function (e) {
			if ($(this).attr('reset') != 'true') {
				$('#inputsignatures').attr('disabled', 'disabled');
				$('#modal-anadir-autor .validar').text(GetText('CV_RESTABLECER'));
				$('#modal-anadir-autor .validar').attr('reset', 'true');
				that.validarFirmas();
			} else {
				$('#inputsignatures').removeAttr('disabled');
				$(this).removeAttr('reset');
				$('#modal-anadir-autor .validar').text(GetText('CV_BUSCAR'));
				$('#inputsignatures').val('');
				$('#modal-anadir-autor div.custom-form-row.resultados').empty();
			}
		});

		//Enganchamos comportamiento check firmas
		$('input.chksignature').change(function (e) {
			var that = this;
			$(this).closest('.form-group.full-group').find('input.chksignature').each(function () {
				if (that != this) {
					$(this).prop('checked', false);
				}
			});
		});
		operativaFormularioAutor.init();

		//Enganchamos comportamiento buscar	firmas
		$('.coincidencia-wrap a.form-buscar,.collapse-wrap a.form-buscar').off('click').on('click', function (e) {
			$('#modal-anadir-autor .formulario-principal').hide();
			$('#modal-anadir-autor .formulario-codigo').show();
			$('#modal-anadir-autor .formulario-autor').hide();
			$('#signatureorcid').val('');
			$('#modal-anadir-autor .formulario-codigo p.ko').hide();
			$('#modal-anadir-autor .formulario-codigo .firma').text($(this).closest('.simple-collapse').find('.control-label.d-block').text());
		});

		//Validar ORCID firma
		$('#modal-anadir-autor .formulario-codigo a.btn-outline-grey').off('click').on('click', function (e) {
			that.validarORCID($('#modal-anadir-autor #signatureorcid').val());
		});


		//Añadimos firmas
		$('#modal-anadir-autor .addsignatures').off('click').on('click', function (e) {
			var error = "";
			//Error, no se ha seleccionado ninguna persona
			$('#modal-anadir-autor .resultados .form-group.full-group').each(function (index) {
				if ($(this).find('input:checked').length == 0) {
					error += GetText("CV_FIRMANOSELECCIONADOPERSONA") + $(this).find('label.control-label.d-block').text() + "</br>";
				}
			});

			//Error, la persona ya está cargada
			var personasCargadas = []
			$('.entityauxauthorlist .added.entityaux div[propertyrdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#member"]').each(function (index) {
				personasCargadas.push($(this).attr('about'));
			});


			$('#modal-anadir-autor .resultados .form-group.full-group').each(function (index) {
				var personID = $(this).find('input:checked').attr('personid');
				if (personasCargadas.indexOf(personID) > -1) {
					var nombre = $(this).find('input:checked').closest('.user-miniatura').find('.nombre-usuario-wrap .nombre').text();
					error += GetText("CV_ESTACARGADAPERSONAOTRAFIRMA", nombre);
				}

			});


			if (error != '') {
				$('#modal-anadir-autor .ko').show();
				$('#modal-anadir-autor .ko').html(error);
				return;
			}
			$('#modal-anadir-autor .ko').hide();
			$('#modal-anadir-autor .ko').html('');
			var num = 1000;
			$('#modal-anadir-autor .resultados .form-group.full-group').each(function (index) {
				var personID = $(this).find('input:checked').attr('personid');
				var nombre = $(this).find('input:checked').closest('.user-miniatura').find('.nombre-usuario-wrap .nombre').text();
				var firma = $(this).find('label.control-label.d-block').text();
				var orcid = $(this).find('input:checked').closest('.user-miniatura').find('.nombre-usuario-wrap .nombre-completo .orcid').text();
				num++;
				if ($(this).find('input:checked').length > 0) {
					var htmlAuthor = `
						<div class="item added entityaux" propertyrdf="${$('#modal-anadir-autor').attr('propertyrdf')}" rdftype="http://purl.obolibrary.org/obo/BFO_0000023" about="${RandomGuid()}">
							<div class="custom-form-row">
								<div class="form-group full-group">
									<label class="control-label d-block">Firma *</label>
									<input propertyrdf="http://xmlns.com/foaf/0.1/nick" value="${firma}" type="text" class="form-control not-outline obligatorio">
								</div>
								<div class="form-group full-group entitycontainer obligatorio" rdftype="http://xmlns.com/foaf/0.1/Person" idtemp="2a591f6b-5219-4abd-ac48-d1930aae0bc6">
									<label class="control-label d-block">Persona *</label>
									<div class="item added entity" propertyrdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#member" rdftype="http://xmlns.com/foaf/0.1/Person" about="${personID}">
										<input propertyrdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#member" value="${personID}" type="text" class="form-control not-outline obligatorio">
									</div>
								</div>
							</div>
							<input propertyrdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#comment" value="${num}" type="hidden">
							<span class="title" loaded="true" route="http://xmlns.com/foaf/0.1/nick">${firma}</span>
							<span class="property" loaded="true" name="Nombre" route="http://www.w3.org/1999/02/22-rdf-syntax-ns#member||person||http://xmlns.com/foaf/0.1/name">${nombre}</span>
							<span class="property" loaded="true" name="ORCID" route="http://www.w3.org/1999/02/22-rdf-syntax-ns#member||person||http://w3id.org/roh/ORCID">${orcid}</span>
						</div>`;
					$('.entityauxauthorlist').append(htmlAuthor);
				} else {
					error += "No se ha seleccionado persona para la firma " + $(this).find('label.control-label.d-block').text();
				}
			});

			$('#modal-anadir-autor').modal('hide');
			that.repintarListadoEntity();
		});

		//Enganchamos comportamiento 'O teclea los datos de-'
		$('#modal-anadir-autor .formulario-edicion.formulario-codigo a.form-autor').off('click').on('click', function (e) {
			$('#modal-anadir-autor .formulario-principal').hide();
			$('#modal-anadir-autor .formulario-codigo').hide();
			$('#modal-anadir-autor .formulario-autor').show();
			$('#modal-anadir-autor .formulario-autor #cvFirmaAutor').val($(this).find('.firma').html());
			$('#modal-anadir-autor .formulario-autor #cvNombreAutor').val('');
			$('#modal-anadir-autor .formulario-autor #cvApellidosAutor').val('');
			$('#modal-anadir-autor .formulario-autor .form-actions .ko').hide();
			$('#modal-anadir-autor .formulario-autor .form-actions .ko').html('');
		});

		//Enganchamos comportamiento Guardar autor
		$('#modal-anadir-autor a.btEditarAutor').off('click').on('click', function (e) {
			var error = "";
			if ($('#cvNombreAutor').val().trim() == '') {
				error += GetText("CV_NOMBREOBLIGATORIO");
			}
			if ($('#cvApellidosAutor').val().trim() == '') {
				if (error != '') {
					error += "</br>";
				}
				error += GetText("CV_APELLIDOSOBLIGATORIO");
			}
			if (error != '') {
				$('#modal-anadir-autor .formulario-autor p.ko').show();
				$('#modal-anadir-autor .formulario-autor p.ko').html(error);
				return;
			}
			$('#modal-anadir-autor .formulario-autor p.ko').hide();
			$('#modal-anadir-autor .formulario-autor p.ko').html('');
			var item = {};
			item.pName = $('#cvNombreAutor').val();
			item.pSurname = $('#cvApellidosAutor').val();
			MostrarUpdateProgress();
			$.post(urlGuardadoCV + 'CreatePerson', item, function (data) {
				OcultarUpdateProgress();
				$('#modal-anadir-autor .formulario-principal').show();
				$('#modal-anadir-autor .formulario-codigo').hide();
				$('#modal-anadir-autor .formulario-autor').hide();
				var rGuid = RandomGuid();
				var id = data.personid;
				var firma = $('#cvFirmaAutor').val();
				var indice = 1;
				var labelFirma = null;
				$('#modal-anadir-autor .resultados .form-group.full-group .simple-collapse>label').each(function (index) {
					if ($(this).text() == firma) {
						labelFirma = $(this);
						return;
					}
					indice++;
				});

				var htmlAux = "";
				if (data.department != null) {
					htmlAux = data.department;
				}
				if (data.orcid != null) {
					if (htmlAux != '') {
						htmlAux += ' · ';
					}
					htmlAux += `<a target="_blank" class="orcid" href="https://orcid.org/${data.orcid}">${data.orcid}</a>`;
				}

				var htmlPersona = `<div class="user-miniatura">
					<div class="custom-control custom-checkbox">
						<input disabled="disabled" checked="checked" type="checkbox" id="user-${rGuid}" personid="${id}" name="user-${indice}" class="custom-control-input chksignature">
						<label class="custom-control-label" for="user-${rGuid}"></label>
					</div>
					<div class="imagen-usuario-wrap">
						<div class="imagen">
						</div>
					</div>
					<div class="nombre-usuario-wrap">
						<p class="nombre">${data.name}</p>
						<p class="nombre-completo">${htmlAux}</p>
					</div>
				</div>`;
				if (labelFirma != null) {
					labelFirma.parent().find('div').remove();
					labelFirma.parent().find('a').remove();
					labelFirma.after(htmlPersona);
					edicionCV.engancharComportamientosCV()
				}
			});


		});

		//GUARDADOS pop up
		$(`#modal-editar-entidad .btn-primary,
		#modal-editar-entidad-0 .btn-primary,
		#modal-editar-entidad-1 .btn-primary,
		#modal-editar-entidad-2 .btn-primary,
		#modal-editar-entidad-3 .btn-primary,
		#modal-editar-entidad-4 .btn-primary`).off('click').on('click', function (e) {
			//Obtenemos modal y formulario
			var modal = $(this).closest('.modal');
			var formulario = $(modal).find('form.formulario-edicion');
			modal.find('.form-actions .ko').remove();

			//Validamos
			if (that.validarFormulario(formulario, modal)) {
				//Procedemos si supera la validación
				that.guardarEntidad(formulario);
			}
		});

		//GUARDADOS entidad simple
		$(`div.panel-body .btn-primary`).off('click').on('click', function (e) {
			//Obtenemos modal y formulario

			var formulario = $(this).closest('div.panel-body');
			var contenedor = $(formulario).closest('div');
			contenedor.find('.form-actions .ko').remove();

			//Validamos
			if (that.validarFormulario(formulario, contenedor)) {
				//Procedemos si supera la validación
				that.guardarEntidad(formulario);
			}
		});

		//Autocompletar tesauros
		$('.modal-con-buscador div[rdftype="http://w3id.org/roh/CategoryPath"] input.texto').off('keyup').on('keyup', function (e) {
			that.buscarTesauro($(this).val(), $('.modal-con-buscador ul.listadoTesauro'));
		});

		//Cargar edición datos personales
		$('#personalDataEdit').off('click').on('click', function (e) {
			MostrarUpdateProgress();
			$.get(urlEdicionCV + 'GetTab?pCVId=' + that.idCV + '&pId=' + $(this).attr('personaldataid') + "&pRdfType=http://w3id.org/roh/PersonalData&pLang=" + lang, null, function (data) {
				$('#modal-editar-entidad').modal('show');
				that.printEditItemCV('#modal-editar-entidad form', data, data.entityID, 'http://w3id.org/roh/PersonalData', data.entityID);
				OcultarUpdateProgress();
			});
		});


		//Combos dependientes
		$('select.hasdependency').each(function () {
			//Obtenemos el input del que es dependiente
			var dependency = $(this).attr('dependency');
			let contenedor = $(this).closest('.item.added');
			if (contenedor.length == 0) {
				contenedor = $(this).closest('.simple-collapse-content');
			}
			contenedor = $(contenedor[0]);
			if (dependency != '') {
				//Seleccionamos el input del que es dependiente y le añadimos el input sobre el que tiene que actuar
				contenedor.find('select[propertyrdf="' + dependency + '"]').attr('dependencyact', $(this).attr('propertyrdf'));
				contenedor.find('select[propertyrdf="' + dependency + '"]').unbind("change.dependencycombo").bind("change.dependencycombo", function () {
					var valorSeleccionado = $(this).val();
					var comboHijo = $(this).closest('.custom-form-row').find('select[propertyrdf="' + $(this).attr('dependencyact') + '"]');
					comboHijo.find('option').each(function () {
						if ($(this).attr('data-dependency') == valorSeleccionado || $(this).attr('data-dependency') == null) {
							$(this).removeAttr('disabled');
						} else {
							$(this).attr('disabled', 'disabled');
						}
					})
					var opcionHijaSelecccionada = comboHijo.find('option:selected');
					if (opcionHijaSelecccionada.length > 0 && opcionHijaSelecccionada.attr("data-dependency") != valorSeleccionado) {
						comboHijo.find('option:nth-child(1)')
							.prop('selected', true)
							.trigger('change')
					}
				});
				contenedor.find('select[propertyrdf="' + dependency + '"]').trigger('change');
			} else {
				//Obtenemos el input del que es dependiente
				var dependencyproperty = $(this).attr('dependencyproperty');
				//Seleccionamos el input del que es dependiente y le añadimos el/los input sobre el que tiene que actuar
				var lista = [];
				if (contenedor.find('select[propertyrdf="' + dependencyproperty + '"]').attr('dependencyactcombo') != null) {
					lista = contenedor.find('select[propertyrdf="' + dependencyproperty + '"]').attr('dependencyactcombo').split(',');
				}
				if (lista.indexOf($(this).attr('propertyrdf')) == -1) {
					lista.push($(this).attr('propertyrdf'));
				}
				contenedor.find('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]').attr('dependencyactcombo', lista.join(','));
				contenedor.find('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]:not([style])').unbind("change.dependencycombo").bind("change.dependencycombo", function () {
					var valorSeleccionado = $(this).val();
					var that2 = this;
					$.each($(this).attr('dependencyactcombo').split(','), function (ind, elem) {
						var inputDestino = $(that2).closest('.entityaux').find('input[propertyrdf="' + elem + '"],select[propertyrdf="' + elem + '"]');
						if (inputDestino.length == 0) {
							var inputDestino = $(that2).closest('form').find('input[propertyrdf="' + elem + '"],select[propertyrdf="' + elem + '"]');
						}
						if (inputDestino.attr('dependencypropertyvalue') != null) {
							var dependencyDestinoValue = inputDestino.attr('dependencypropertyvalue');
							if ((valorSeleccionado == dependencyDestinoValue) || (dependencyDestinoValue == "*" && valorSeleccionado != '')) {
								$(inputDestino).parent().show();
							} else {
								$(inputDestino).parent().hide();
								$(inputDestino).val('');
								$(inputDestino).trigger('change');
							}
						}
						if (inputDestino.attr('dependencypropertyvaluedistinct') != null) {
							var dependencyDestinoValueDistinct = inputDestino.attr('dependencypropertyvaluedistinct');
							if (valorSeleccionado != dependencyDestinoValueDistinct) {
								$(inputDestino).parent().show();
							} else {
								$(inputDestino).parent().hide();
								$(inputDestino).val('');
								$(inputDestino).trigger('change');
							}
						}
					});


				});
				contenedor.find('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]').trigger('change');
			}
		});

		//Petición de enriquecimiento
		$('form[rdftype="http://purl.org/ontology/bibo/Document"] div.visuell-view[propertyrdf="http://purl.org/ontology/bibo/abstract"]').off('focusout').on('focusout', function (e) {
			//TODO: Revisar tema del documento PDF. De momento, siempre se le pasa NULL.
			edicionCV.cargarAreasEtiquetasEnriquecidas($('form[rdftype="http://purl.org/ontology/bibo/Document"] input[propertyrdf="http://w3id.org/roh/title"]').val(), $('form[rdftype="http://purl.org/ontology/bibo/Document"] div.visuell-view[propertyrdf="http://purl.org/ontology/bibo/abstract"]').html(), null);


		});

		//campos de texto dependientes
		$('input.hasdependency[type=text],input.hasdependency[type=hidden],input.hasdependency[type=number]').each(function () {
			//Obtenemos el input del que es dependiente
			var dependencyproperty = $(this).attr('dependencyproperty');
			//Seleccionamos el input del que es dependiente y le añadimos el/los input sobre el que tiene que actuar
			var lista = [];
			if ($('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]').attr('dependencyact') != null) {
				lista = $('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]').attr('dependencyact').split(',');
			}
			if (lista.indexOf($(this).attr('propertyrdf')) == -1) {
				lista.push($(this).attr('propertyrdf'));
			}
			$('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]').attr('dependencyact', lista.join(','));
			$('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]:not([style])').unbind("change.dependency").bind("change.dependency", function () {
				var valorSeleccionado = $(this).val();
				var that2 = this;
				$.each($(this).attr('dependencyact').split(','), function (ind, elem) {
					var inputDestino = $(that2).closest('.entityaux').find('input[propertyrdf="' + elem + '"],select[propertyrdf="' + elem + '"]');
					if (inputDestino.length == 0) {
						var inputDestino = $(that2).closest('form').find('input[propertyrdf="' + elem + '"],select[propertyrdf="' + elem + '"]');
					}
					if (inputDestino.attr('dependencypropertyvalue') != null) {
						var dependencyDestinoValue = inputDestino.attr('dependencypropertyvalue');
						if ((valorSeleccionado == dependencyDestinoValue) || (dependencyDestinoValue == "*" && valorSeleccionado != '')) {
							$(inputDestino).parent().show();
						} else {
							$(inputDestino).parent().hide();
							$(inputDestino).val('');
							$(inputDestino).trigger('change');

							$('input[propertyorigin="' + $(inputDestino).attr('propertyrdf') + '"]').val('');
							$('input[propertyorigin="' + $(inputDestino).attr('propertyrdf') + '"]').trigger('change');
						}
					}
					if (inputDestino.attr('dependencypropertyvaluedistinct') != null) {
						var dependencyDestinoValueDistinct = inputDestino.attr('dependencypropertyvaluedistinct');
						if (valorSeleccionado != dependencyDestinoValueDistinct) {
							$(inputDestino).parent().show();
						} else {
							$(inputDestino).parent().hide();
							$(inputDestino).val('');
							$(inputDestino).trigger('change');

							$('input[propertyorigin="' + $(inputDestino).attr('propertyrdf') + '"]').val('');
							$('input[propertyorigin="' + $(inputDestino).attr('propertyrdf') + '"]').trigger('change');
						}
					}
				});


			});
			$('select[propertyrdf="' + dependencyproperty + '"],input[propertyrdf="' + dependencyproperty + '"]').trigger('change');
		});

		//entidades auxiliares dependientes
		$('div.entityauxcontainer.hasdependency').each(function () {
			//Obtenemos el input del que es dependiente
			var dependencyproperty = $(this).attr('dependencyproperty');
			//Seleccionamos el input del que es dependiente y le añadimos la/las entidades auxiliares sobre el que tiene que actuar
			var lista = [];
			if ($('select[propertyrdf="' + dependencyproperty + '"]').attr('dependencyactentity') != null) {
				lista = $('select[propertyrdf="' + dependencyproperty + '"]').attr('dependencyactentity').split(',');
			}
			if (lista.indexOf($($(this).find('div.item.aux.entityaux')[0]).attr('propertyrdf')) == -1) {
				lista.push($($(this).find('div.item.aux.entityaux')[0]).attr('propertyrdf'));
			}
			$('select[propertyrdf="' + dependencyproperty + '"]').attr('dependencyactentity', lista.join(','));
			$('select[propertyrdf="' + dependencyproperty + '"]').unbind("change.dependencyentity");
			$('select[propertyrdf="' + dependencyproperty + '"]').bind("change.dependencyentity", function () {
				var valorSeleccionado = $(this).val();
				var that2 = this;
				$.each($(this).attr('dependencyactentity').split(','), function (ind, elem) {
					var entidadDestino = $(that2).closest('form').find('div.item.aux.entityaux[propertyrdf="' + elem + '"]').closest('div.entityauxcontainer');
					var dependencyDestinoValue = entidadDestino.attr('dependencypropertyvalue');
					if (valorSeleccionado == dependencyDestinoValue) {
						$(entidadDestino).parent().show();
					} else {
						$(entidadDestino).parent().hide();
						$(entidadDestino).find('div.item.added.entityaux').remove();
						$(entidadDestino).find('div.simple-collapse-content .resource-list-wrap').empty();
					}
				});
			});
			$('select[propertyrdf="' + dependencyproperty + '"]').trigger('change');
		});


		//Cambiamos de idioma en la edición de item
		$('.formulario-edicion ul.nav-tabs li a').off('click').on('click', function (e) {
			$(this).closest('ul').find('a').removeClass('active');
			$(this).addClass('active');
			that.cambiarIdiomaEdicion($(this).attr('lang'), $(this).closest('.formulario-edicion'));
		});

		//Cambiamos de idioma en la edición de cv
		$('.row.cvTab ul.nav-tabs li a').off('click').on('click', function (e) {
			$(this).closest('ul').find('a').removeClass('active');
			$(this).addClass('active');
			that.cambiarIdiomaEdicion($(this).attr('lang'), $(this).closest('.panel-collapse'));
		});
		//Añadimos los listeners al editor WYSIWYG
		const editores = document.getElementsByClassName('edmaTextEditor');
		for (let i = 0; i < editores.length; i++) {
			new TextField(editores[i]);
		}
		document.removeEventListener('selectionchange', selectionChange);
		document.addEventListener('selectionchange', selectionChange);

		//Comportamiento desplegar sección no cargada
		$('div.panel-group.pmd-accordion.notLoaded').off('click').on('click', function (e) {
			var about = $(this).closest('.cvTab').attr('about');
			var rdftype = $(this).closest('.cvTab').attr('rdftype');
			var section = $(this).attr('section');
			if (rdftype && about) {
				that.completeTab(about, rdftype, section);
			}
		});

		//Obtener datos envio PRC
		$('.texto.prodCientItem').closest('li').off('click').on('click', function (e) {
			$('#modal-enviar-produccion-cientifica .formulario-edicion.formulario-proyecto .resource-list-wrap').empty();
			$('#modal-enviar-produccion-cientifica .resource-list.listView .resource-list-wrap').empty();


			$(this).closest("article").clone().appendTo('#modal-enviar-produccion-cientifica div.modal-body>.resource-list.listView .resource-list-wrap');
			$('#modal-enviar-produccion-cientifica div.modal-body>.resource-list.listView .resource-list-wrap article .acciones-recurso-listado.acciones-recurso').remove();
			$('#modal-enviar-produccion-cientifica div.modal-body>.resource-list.listView .resource-list-wrap article .material-icons.arrow').remove();


			var dataId = $(this)[0].dataset.id;
			var nombreProy = $(this).closest("resource-success");
			var section = $(this).closest(".resource.success").closest(".panel-group.pmd-accordion").attr("section");
			var rdfTypeTab = $(this).closest(".resource.success").closest(".row.cvTab").attr("rdftype");
			var fechaProy = "";
			that.GetDataPRC(dataId, that.idPerson, section, rdfTypeTab);
		});

		//Enviar a borrar en PRC
		$('.texto.prodCientBorrarItem').closest('li').off('click').on('click', function (e) {
			MostrarUpdateProgress();
			var formData = new FormData();
			let idrecurso = $(this).find('[data-id]').attr('data-id')
			formData.append('pIdRecurso', idrecurso);
			$.ajax({
				url: urlEnvioValidacionCV + 'EnvioEliminacionPRC',
				type: 'POST',
				data: formData,
				cache: false,
				processData: false,
				enctype: 'multipart/form-data',
				contentType: false,
				success: function (response) {
					mostrarNotificacion('success', GetText('CV_PUBLICACION_BLOQUEADA_RESUELVA_PROCEDIMIENTO'), 10000);
					OcultarUpdateProgress();
				},
				error: function () {
					mostrarNotificacion('warning', GetText('CV_ERROR_PUBLICACION_PRC'));
					OcultarUpdateProgress();
				}
			});
		});

		//Enviar a DSpace
		$('.texto.sendDspace').closest('li').off('click').on('click', function (e) {


			$("#modal-dspace").modal("show");
			$("#modal-dspace").find(".resource-list").empty();
			$("#modal-dspace").find("a.document-link").remove();
			$(this).parents("article.resource.success").clone().appendTo("#modal-dspace .resource-list").find(".acciones-recurso-listado.acciones-recurso").remove();
			var url = $("#modal-dspace .resource-list").find("p:contains('URL')").parent().find('p:not(".title")').html()
			if(url){
				$("#modal-dspace .resource-list").after(`<a class="document-link" target="_blank" href=${url}>URL del documento</a>`)
			}
			accionesPlegarDesplegarModal.collapse();
			if ($("#file_dspace").data("GnossDragAndDrop")) {
				$("#file_dspace").data("GnossDragAndDrop").resetPlugin()
			}
			$("#file_dspace").GnossDragAndDrop({
				acceptedFiles: '*',
				onFileAdded: function (plugin, files) {
					$('.col-contenido .botonera').css('display', 'block');
				},
				onFileRemoved: function (plugin, files) {
					$('.col-contenido .botonera').css('display', 'none');
				}
			})
			
		});
		$("#enviar-dspace").off("click").on("click", (e) => {

			var files = ($("#file_dspace").prop("files"));
			if (files.length == 0) {
				mostrarNotificacion("warning", "Debes subir un archivo")
			}

			MostrarUpdateProgress();
			var formData = new FormData();
			let idrecurso = $(e.target).parents(".modal-body").find('[data-id]').attr('data-id');
			formData.append('pIdRecurso', idrecurso);
			formData.append("file", files[0]);
			$.ajax({
				url: urlEnvioDSpaceCV + 'EnvioDSpace',
				type: 'POST',
				data: formData,
				cache: false,
				processData: false,
				enctype: 'multipart/form-data',
				contentType: false,
				success: function (response) {
					mostrarNotificacion('success', GetText('CV_ENVIO_DSPACE_CORRECTO'), 10000);
					OcultarUpdateProgress();
					$("#modal-dspace").modal("hide");
				},
				error: function () {
					mostrarNotificacion('warning', GetText('CV_ENVIO_DSPACE_ERROR'), 10000);
					OcultarUpdateProgress();
				}
			})


		})

		$('.texto.validacionItem').off('click').on('click', function (e) {
			var dataId = $(this)[0].dataset.id;

			that.EnvioValidacion(dataId, that.idPerson);
		});

		$('input[propertyentity][graph]').off('change').on('change', function (e) {
			that.loadPropertiesEntitiesAutocomplete();
		});

		return;
	},
	loadPropertiesEntitiesAutocomplete: function () {
		$('input[propertyentity][graph]').each(function () {
			let graph = $(this).attr('graph');
			let propertyEntity = $(this).attr('propertyentity');
			let propertyOrigin = $(this).attr('propertyorigin');
			if (graph != null && propertyEntity != null) {
				let propertyEntitySplit = propertyEntity.split('&');
				if ($(this).val() != '') {
					var sendData = {};
					sendData.pGraph = graph;
					sendData.pEntity = $(this).val();
					sendData.pProperties = [];
					for (var i = 0; i < propertyEntitySplit.length; i++) {
						sendData.pProperties.push(propertyEntitySplit[i].split('|')[0]);
					}
					$.post(urlEdicionCV + 'GetPropertyEntityData', sendData, function (data) {
						for (var i = 0; i < propertyEntitySplit.length; i++) {
							let propEntity = propertyEntitySplit[i].split('|')[0];
							let propCV = propertyEntitySplit[i].split('|')[1];
							if (propCV != propertyOrigin) {
								$('.formulario-edicion input[propertyrdf="' + propCV + '"]').attr('disabled', 'disabled');
							}
							$('.formulario-edicion input[propertyrdf="' + propCV + '"]').val(data[propEntity]);
						}
					});
				} else {
					for (var i = 0; i < propertyEntitySplit.length; i++) {
						let propEntity = propertyEntitySplit[i].split('|')[0];
						let propCV = propertyEntitySplit[i].split('|')[1];
						$('.formulario-edicion input[propertyrdf="' + propCV + '"]').removeAttr('disabled');
					}
				}
			}
		});
	},
	sendPRC: function (idrecurso, idproyecto, section, rdfTypeTab) {
		var that = this;
		var formData = new FormData();
		formData.append('pIdRecurso', idrecurso);
		if (idproyecto.length > 0) {
			for (var indice in idproyecto) {
				formData.append('pIdProyecto', idproyecto[indice]);
			}
		} else {
			formData.append('pIdProyecto', "");
		}
		MostrarUpdateProgress();
		$.ajax({
			url: urlEnvioValidacionCV + 'EnvioPRC',
			type: 'POST',
			data: formData,
			cache: false,
			processData: false,
			enctype: 'multipart/form-data',
			contentType: false,
			success: function (response) {
				mostrarNotificacion('success', GetText('CV_PUBLICACION_BLOQUEADA_RESUELVA_PROCEDIMIENTO'), 10000);
				$.get(urlEdicionCV + 'GetItemMini?pCVId=' + that.idCV + '&pIdSection=' + section + "&pRdfTypeTab=" + rdfTypeTab + "&pEntityID=" + idrecurso + "&pLang=" + lang, null, function (data) {
					$('a[data-id="' + idrecurso + '"]').closest('article').replaceWith(that.printHtmlListItem(idrecurso, data));
					that.repintarListadoTab(section);
					OcultarUpdateProgress();
				});
			},
			error: function () {
				mostrarNotificacion('warning', GetText('CV_ERROR_PUBLICACION_PRC'));
				OcultarUpdateProgress();
			}
		});
	},
	GetDataPRC: function (dataId, idPerson, section, rdfTypeTab) {
		MostrarUpdateProgress();
		var that = this;
		$('#modal-enviar-produccion-cientifica .formulario-edicion.formulario-proyecto .resource-list-wrap').empty();
		$('#modal-enviar-produccion-cientifica .formulario-edicion.formulario-proyecto .form-actions .btn.btn-primary.uppercase.btnEnvioPRC').removeClass("disabled");
		$('#modal-enviar-produccion-cientifica .modal-body > .alert').css('display', 'none');
		$('#modal-enviar-produccion-cientifica .modal-body .formulario-edicion.formulario-publicacion').css('display', 'none');
		$('#modal-enviar-produccion-cientifica .modal-body .formulario-edicion.formulario-proyecto').removeAttr('style');
		let itemId = $('#modal-enviar-produccion-cientifica div.modal-body>.resource-list.listView .resource-list-wrap').find('a[data-id]').attr('data-id');
		var urlDuplicados = urlEdicionCV + "GetItemsDuplicados?pCVId=" + this.idCV + "&pMinSimilarity=0.9&pItemId=" + itemId;
		$.get(urlDuplicados, null, function (data) {
			if (data && data.length > 0) {
				$('#modal-enviar-produccion-cientifica .modal-body > .alert').removeAttr('style');
				$('#modal-enviar-produccion-cientifica .modal-body .formulario-edicion.formulario-publicacion').removeAttr('style');
				$('#modal-enviar-produccion-cientifica .modal-body .formulario-edicion.formulario-proyecto').css('display', 'none');
				$('#modal-enviar-produccion-cientifica .modal-body .form-actions .btn-aplicar').attr('dataId', dataId);
				$('#modal-enviar-produccion-cientifica .modal-body .form-actions .btn-aplicar').attr('section', section);
				$('#modal-enviar-produccion-cientifica .modal-body .form-actions .btn-aplicar').attr('rdfTypeTab', rdfTypeTab);
				$('#modal-enviar-produccion-cientifica .modal-body .form-actions .btn-aplicar').attr('idPerson', idPerson);
				var items = data;
				duplicadosCV.items = data;
				let principal = true;
				for (var itemIn in items[0].items) {
					if (!principal) {
						let aux = itemIn;
						$.get(urlEdicionCV + 'GetItemMini?pCVId=' + that.idCV + '&pIdSection=' + items[0].idSection + "&pRdfTypeTab=" + items[0].rdfTypeTab + "&pEntityID=" + items[0].items[itemIn] + "&pLang=" + lang, null, function (data) {
							var htmlItem = edicionCV.printHtmlListItemPRC(items[0].items[aux], data);
							$('#modal-enviar-produccion-cientifica .formulario-publicacion .resource-list-wrap').append(htmlItem);
							duplicadosCV.engancharComportamientos(true);
						}).done(() => { OcultarUpdateProgress() });
					}
					principal = false;
				}
			} else {
				edicionCV.PintarDataPRC(dataId, idPerson, section, rdfTypeTab);
				OcultarUpdateProgress();
			}
		});
	},
	PintarDataPRC: function (dataId, idPerson, section, rdfTypeTab) {
		$('#modal-enviar-produccion-cientifica .modal-body > .alert').css('display', 'none');
		$('#modal-enviar-produccion-cientifica .modal-body .formulario-edicion.formulario-publicacion').css('display', 'none');
		$('#modal-enviar-produccion-cientifica .modal-body .formulario-edicion.formulario-proyecto').removeAttr('style');
		$.ajax({
			url: urlEnvioValidacionCV + 'ObtenerDatosEnvioPRC',
			type: 'GET',
			data: {
				pIdPersona: idPerson
			},
			success: function (response) {
				var contador = 0;
				var html = '';
				for (const seccion in response) {
					html += `<article class="resource folder">
								<div class="form-group">
									<div class="form-check form-check-inline">
										<input class="form-check-input" type="checkbox" name="proyecto" id="proyecto-${contador}" projectid="${seccion}">
										<label class="form-check-label" for="proyecto-${contador}"></label>
									</div>
								</div>
								<div class="wrap">
									<div class="middle-wrap">
										<div class="title-wrap">
											<h2 class="resource-title">
												${response[seccion].titulo}
											</h2>
										</div>
										<div class="content-wrap">
											<div class="description-wrap counted">`;
					if (response[seccion].fechaInicio != null) {
						html += `
												<div class="group fecha">
													<p class="title">Fecha inicio</p>
													<p>${response[seccion].fechaInicio}</p>
												</div>`;
					}
					if (response[seccion].fechaFin != null) {
						html += `<div class="group fecha">
													<p class="title">Fecha fin</p>
													<p>${response[seccion].fechaFin}</p>
												</div>`;
					}
					if (response[seccion].organizacion != null) {
						html += `<div class="group publicacion">
													<p class="title">Organización</p>
													<p>${response[seccion].organizacion}</p>
												</div>`;
					}
					html += `
											</div>
										</div>
									</div>
								</div>
							</article>`;
					contador++;
				}

				$('#modal-enviar-produccion-cientifica .formulario-edicion.formulario-proyecto .resource-list-wrap').append(html);
				operativaFormularioProduccionCientifica.formProyecto(section, rdfTypeTab);

			}
		});
	},
	EnvioValidacion: function (dataId, idPerson) {
		var formData = new FormData();
		formData.append('pIdRecurso', dataId);
		formData.append('pIdPersona', idPerson);
		formData.append('pIdAutorizacion', '');

		$.ajax({
			url: urlEnvioValidacionCV + 'EnvioProyecto',
			type: 'POST',
			data: formData,
			cache: false,
			processData: false,
			enctype: 'multipart/form-data',
			contentType: false,
			success: function (response) {
				mostrarNotificacion('success', GetText('CV_DOCUMENTO_VALIDACION'), 10000);
			},
			error: function (response) {
				mostrarNotificacion('warning', GetText('CV_ERROR_DOCUMENTO_VALIDACION'));
			}
		});
		return;
	},
	validarFormulario: function (formulario, contenedor) {
		var that = this;
		//Validamos los campos obligatorios
		var camposObligatorios = [];
		//Validamos inputs que no pertenezcan a una entidad auxiliar (ni sean multiples)
		$(formulario).find('input.obligatorio, select.obligatorio, div.visuell-view.obligatorio').each(function (index) {
			if ($(this).closest('.entityauxcontainer,.entitycontainer,.multiple').length == 0) {
				if ($(this).val() == null || $(this).val() == '') {
					if ($(this).is('input') || $(this).is('div.visuell-view')) {
						//Si es un input o textarea sólo es obligatorio si no tiene idioma y está visible
						if (($(this).closest('.form-group').attr('style') == null || $(this).closest('.form-group').attr('style').replaceAll(' ', '').replaceAll(';', '') != 'display:none')
							&& $(this).attr('multilang') == null && $(this).closest('div.visuell-view').length == 0) {
							camposObligatorios.push($(this).closest('.form-group').find('.control-label').text());
						}
					}
					else {
						camposObligatorios.push($(this).closest('.form-group').find('.control-label').text());
					}
				}
			}
		});
		//Validamos inputs que no pertenezcan a una entidad auxiliar (y sean multiples)
		$(formulario).find('.multiple.obligatorio:not(.entityauxcontainer ):not(.entitycontainer )').each(function (index) {
			if ($(this).parent().closest('.entityauxcontainer,.entitycontainer,.multiple').length == 0) {
				if ($(this).children('.added').length == 0 || $($(this).children('.added')[0]).attr('about') == '') {
					camposObligatorios.push($(this).children('label').text());
				}
			}
		});
		//Validamos propiedades de entidades auxiliares y entidades que no pertenezcan a una entidad auxiliar
		$(formulario).find('.entityauxcontainer.obligatorio,.entitycontainer.obligatorio').each(function (index) {
			if ($(this).parent().closest('.entityauxcontainer,.entitycontainer').length == 0) {
				if ($(this).children('.added').length == 0 || $($(this).children('.added')[0]).attr('about') == '') {
					camposObligatorios.push($(this).children('label').text());
				}
			}
		});
		$(contenedor).find('.form-actions .ko').remove();
		if (camposObligatorios.length > 0) {
			var error = "";
			for (var indice in camposObligatorios) {
				error += '<p>El campo ' + camposObligatorios[indice] + ' es obligatorio</p>';
			};
			$(contenedor).find(' .form-actions').append('<div class="ko" style="display:block"><p>' + error + '</p></div>');
			return false;
		}
		return true;
	},
	eliminarEntidadTesauro: function (modalID, idTemp, id) {
		$('#' + modalID + ' div[idtemp="' + idTemp + '"] div[about="' + id + '"]').remove();
		var contenedor = $('#' + modalID + ' div[idtemp="' + idTemp + '"]');
		var valorRemoveAnterior = contenedor.attr('remove');
		if (valorRemoveAnterior != null) {
			valorRemoveAnterior += '||';
		} else {
			valorRemoveAnterior = '';
		}

		contenedor.attr('remove', valorRemoveAnterior + id);
		this.repintarListadoThesaurus();
		$("#modal-eliminar").modal("hide");
	},
	eliminarEntidadTopic: function (modalID, propRDF, value) {
		if ($('#' + modalID + ' input[propertyrdf="' + propRDF + '"][data-value="' + value + '"]').closest('div[propertyrdf="http://w3id.org/roh/enrichedKeywords"]').length) {
			$('#' + modalID + ' input[propertyrdf="' + propRDF + '"][data-value="' + value + '"]').closest('div[propertyrdf="http://w3id.org/roh/enrichedKeywords"]').remove();
		} else {
			$('#' + modalID + ' input[propertyrdf="' + propRDF + '"][data-value="' + value + '"]').parent().remove();
		}

		this.repintarTopic();
		this.engancharComportamientosCV();
		$("#modal-eliminar").modal("hide");
	},
	eliminarItem: function (sectionID, entityID) {
		var that = this;
		var item = {};
		item.pEntity = entityID;
		var article = $('a[data-id="' + entityID + '"]').closest('article');
		MostrarUpdateProgress();
		$.post(urlGuardadoCV + 'RemoveItem', item, function (data) {
			article.remove();
			that.repintarListadoTab(sectionID);
			OcultarUpdateProgress();
		});
	},
	guardarEntidad: function (pFormulario) {
		var that = this;
		$('#modal-editar-entidad .modal-body>.form-actions>.ko').remove();

		//Los modales son de 3 tipos
		//Modal principal (item del CV)
		//Entidad auxiliar
		//Entidad principal

		var modal = pFormulario.closest('.modal');
		//Auxiliar
		if (pFormulario.attr('entityid') == null && pFormulario.attr('entityload') == null) {
			MostrarUpdateProgress();
			if (modal.attr('new') == 'true') {
				if (modal.hasClass('modal-tesauro')) {
					var panThesaurus = $('.entityauxcontainer[idtemp="' + modal.attr('idtemp') + '"]');
					panThesaurus.children('.item.added.entityaux').remove();

					var items = pFormulario.find('a.faceta.last-level.selected:not(.lock)');

					items.each(function () {
						var añadidos = pFormulario.find('.custom-form-row .item.added').remove();

						var parentPanel = $(this).closest('.item.entityaux');
						var newCategoryInput = parentPanel.find('.custom-form-row input[type="text"]');
						var newCategoryAddButton = parentPanel.find('.custom-form-row .btn.add');

						faceta = $(this);

						while (faceta != null) {
							//Seleccionar la categoría del panel principal

							newCategoryInput.val(faceta.attr("name"));
							newCategoryAddButton.trigger('click');
							var listado = faceta.closest('ul');
							if (!listado.hasClass('listadoTesauro')) {
								faceta = listado.prev();
							} else {
								faceta = null;
							}
						}

						var formEdicionClone = modal.find('.formulario-edicion div[about="' + modal.attr('about') + '"]').clone();
						formEdicionClone.attr('about', RandomGuid());
						formEdicionClone.find('.buscador-coleccion').remove();
						formEdicionClone.find('.action-buttons-resultados').remove();
						formEdicionClone.find('.listadoTesauro').remove();

						panThesaurus.append(formEdicionClone);
					});

				} else {
					$('.entityauxcontainer[idtemp="' + $(modal).attr('idtemp') + '"]').append($(modal).find('.formulario-edicion div[about="' + $(modal).attr('about') + '"]'));
				}
			} else {
				$('.entityauxcontainer[idtemp="' + $(modal).attr('idtemp') + '"] div[about="' + $(modal).attr('about') + '"]').replaceWith($(modal).find('.formulario-edicion div[about="' + $(modal).attr('about') + '"]'));
			}
			$(modal).modal('hide');
			that.repintarListadoEntity();
			OcultarUpdateProgress();
		} else {
			//Modal principal (item del CV)
			//o
			//Entidad principal
			var entidad = {};
			var entidadPrincipal = false;
			if ($(pFormulario).attr('entityid') != null && $(pFormulario).attr('entityload') == null) {
				entidadPrincipal = true;
			}
			entidad.id = $(pFormulario).attr('entityid');
			entidad.rdfType = $(pFormulario).attr('rdftype');
			entidad.sectionID = $(pFormulario).attr('sectionid');
			entidad.rdfTypeTab = $(pFormulario).attr('rdftypetab');
			entidad.cvID = this.idCV;
			entidad.pLang = lang;
			entidad.properties = [];
			entidad.properties_cv = [];
			$(pFormulario).find('input, select, div.visuell-view').each(function (index) {
				if ($(this).attr('type') != 'file' && !$(this).hasClass('autocompleteentity')) {
					var entityCV = $(this).hasClass('entity_cv');
					if ($(this).attr('propertyrdf') != null) {
						if ($(this).closest('.item').hasClass('aux') || $(this).hasClass('aux')) {
							//Si es multiple y no es una entidad auxiliar y no tiene otros valores añadidos continua pero con valor vacío
							if ($(this).closest('.multiple:not(.entityauxcontainer )').length > 0 && $(this).closest('.multiple:not(.entityauxcontainer )').find('.added').length == 0) {
								if ($(this).is('div.visuell-viewn')) {
									$(this).html('');
								} else {
									$(this).val('');
								}
							} else {
								return;
							}
						}
						var property = $(this).attr('propertyrdf');
						//Cargar propiedades padre
						if ($(this).closest('.entityaux').length == 1) {
							var propertyParent = $(this).closest('.entityaux').attr('propertyrdf');
							var rdfTypeEntity = $(this).closest('.entityaux').attr('rdftype');
							property = propertyParent + "@@@" + rdfTypeEntity + "|" + property;
						}

						var prop = null;
						if (entityCV) {
							for (var indice in entidad.properties_cv) {
								if (entidad.properties_cv[indice].prop == property) {
									prop = entidad.properties_cv[indice];
								}
							};
						} else {
							for (var indice in entidad.properties) {
								if (entidad.properties[indice].prop == property) {
									prop = entidad.properties[indice];
								}
							};
						}
						if (prop == null) {
							prop = {};
							prop.prop = property;
							prop.values = [];
							prop.valuesmultilang = {};
							if (entityCV) {
								entidad.properties_cv.push(prop);
							} else {
								entidad.properties.push(prop);
							}
						}
						var valor = $(this).val();
						if ($(this).is("div")) {
							valor = $(this).html();
						}
						if ($(this).closest('.entityaux').length == 1) {
							var entityParent = $(this).closest('.entityaux').attr('about');
							valor = entityParent + "@@@" + valor;
						}
						if ($(this).attr('multilang') != null) {
							prop.valuesmultilang[$(this).attr('multilang')] = valor;
						} else {
							prop.values.push(valor);
						}
					}
				}
			});

			entidad.auxEntityRemove = [];
			$(pFormulario).find('.entityauxcontainer').each(function (index) {
				if ($(this).attr('remove') != null) {
					var remove = $(this).attr('remove').split('||');
					for (var i = 0; i < remove.length; i++) {
						entidad.auxEntityRemove.push(remove[i]);
					}
				}
			});

			MostrarUpdateProgress();
			if (entidadPrincipal) {
				//Entidad principal
				$.post(urlGuardadoCV + 'updateEntity', entidad, function (data) {
					if (data.ok) {
						$(modal).modal('hide');
						if (entidad.id.indexOf("http") == -1) {
							//nueva
							if ($('.entitycontainer[idtemp="' + $(modal).attr('idtemp') + '"]').hasClass('multiple')) {
								//Clonamos la auxiliar
								var itemAux = $('.entitycontainer[idtemp="' + $(modal).attr('idtemp') + '"] div.item.aux.entity');
								var itemClone = itemAux.clone();
								itemClone.removeClass('aux');
								itemClone.addClass('added');
								itemClone.attr('about', data.id);
								itemClone.find('input').val(data.id);
								itemAux.after(itemClone);
							} else {
								$('.entitycontainer[idtemp="' + $(modal).attr('idtemp') + '"] .item.added.entity').attr('about', data.id);
								$('.entitycontainer[idtemp="' + $(modal).attr('idtemp') + '"] .item.added.entity input').val(data.id);
							}
						}
						$('.item.added.entity[about="' + data.id + '"]').children('span.title').attr('loaded', 'false');
						$('.item.added.entity[about="' + data.id + '"]').children('span.property').attr('loaded', 'false');
						$('.item.added.entity[about="' + data.id + '"]').children('span.title').html('');
						$('.item.added.entity[about="' + data.id + '"]').children('span.property').html('');
						that.repintarListadoEntity();
						OcultarUpdateProgress();
					} else {
						alert("Error: " + data.error);
						OcultarUpdateProgress();
					}
				});
			} else {
				//Modal principal (item del CV)
				$.post(urlGuardadoCV + 'updateEntity', entidad, function (data) {
					if (data.ok) {
						$(modal).modal('hide');
						var entityLoad = $(pFormulario).attr('entityload');
						if (entityLoad != null && entityLoad != '') {
							//Si son los datos personales refrescamos
							if (entidad.rdfTypeTab == 'http://w3id.org/roh/PersonalData') {
								$('#identificacion-tab').click();
							} else {
								//Si viene entityLoad actualiza el item
								if ($(modal).length) {
									//Item NO CV
									$.get(urlEdicionCV + 'GetItemMini?pCVId=' + that.idCV + '&pIdSection=' + entidad.sectionID + "&pRdfTypeTab=" + entidad.rdfTypeTab + "&pEntityID=" + entityLoad + "&pLang=" + lang, null, function (data) {
										$('a[data-id="' + entityLoad + '"]').closest('article').replaceWith(that.printHtmlListItem(entityLoad, data));
										that.repintarListadoTab(entidad.sectionID);
										OcultarUpdateProgress();
									});
								} else {
									//Item CV
									$.get(urlEdicionCV + 'GetEdit?pCVId=' + that.idCV + '&pIdSection=' + entidad.sectionID + "&pRdfTypeTab=" + entidad.rdfTypeTab + "&pEntityID=" + data.id + "&pLang=" + lang, null, function (data2) {
										that.printSectionItem($('div[sectionid="' + entidad.sectionID + '"]').attr('id'), data2, entidad.sectionID, entidad.rdfTypeTab, data2.entityID);
										that.engancharComportamientosCV();
										OcultarUpdateProgress();
									});
								}

							}
						} else {
							//Si no viene entityLoad carga el item
							$.get(urlEdicionCV + 'GetItemMini?pCVId=' + that.idCV + '&pIdSection=' + entidad.sectionID + "&pRdfTypeTab=" + entidad.rdfTypeTab + "&pEntityID=" + data.id + "&pLang=" + lang, null, function (data2) {
								$('div[section="' + entidad.sectionID + '"] .resource-list-wrap').append(that.printHtmlListItem(data.id, data2));
								that.repintarListadoTab(entidad.sectionID);
								OcultarUpdateProgress();
							});
						}

					} else {
						if (data.error != null && data.error.startsWith("PROPREPETIDA")) {
							var msg = GetText("CV_PROPIEDADIDENTIFICADORREPETIDA", data.error.replace("PROPREPETIDA|", ""));
							$('#modal-editar-entidad .modal-body>.form-actions').append('<p class="ko" style="display:block">' + msg + '</p>');
						} else {
							$(modal).find('.form-actions .ko').remove();
							$(modal).find(' .form-actions').append('<div class="ko" style="display:block"><p>' + data.error + '</p></div>');
			
						}
						OcultarUpdateProgress();
					}
				});
			}
		}
	},
	validarFirmas: function () {
		$('#modal-anadir-autor .formulario-edicion .resultados').hide();
		$('#modal-anadir-autor .formulario-edicion .form-actions').hide();
		$('#modal-anadir-autor .formulario-edicion .resultados .form-group.full-group').remove();
		$('#modal-anadir-autor .formulario-edicion .form-actions .ko').hide();
		$('#modal-anadir-autor .formulario-edicion .form-actions .ko').html("");
		var error = "";
		//Comprobamos que en el texto introducido no haya firmas duplicadas
		var signatures = $('#inputsignatures').val().toLowerCase().split(';');

		var signaturesArray = [];
		signatures.forEach(function (signature) {
			var actual = signature.toLowerCase().trim();
			if (actual != '') {
				signaturesArray.push(actual);
			}
		});

		var signaturesProcessed = [];
		signaturesArray.forEach(function (signature) {
			if (signaturesProcessed.indexOf(signature) > -1) {
				if (error != '') {
					error += "</br>";
				}
				error += GetText("CV_LAFIRMAXESTADUPLICADA", signature);
			}
			signaturesProcessed.push(signature);
		});
		//Comprobamos que en el texto introducido no haya firmas duplicadas (de las cargadas anteriormente)
		$('.entityauxauthorlist .added.entityaux input[propertyrdf="http://xmlns.com/foaf/0.1/nick"]').each(function () {
			var firmaActual = $(this).val().trim();
			if (signaturesProcessed.indexOf(firmaActual.toLowerCase()) > -1) {
				if (error != '') {
					error += "</br>";
				}
				error += GetText("CV_LAFIRMAXYAESTAINTRODUCIDA", firmaActual);
			}
		});
		if (error != '') {
			$('#modal-anadir-autor .formulario-edicion .form-actions .ko').show();
			$('#modal-anadir-autor .formulario-edicion .form-actions .ko').html(error);
			return;
		}

		var that = this;
		var item = {};
		item.pSignatures = $('#inputsignatures').val();
		item.pCVID = this.idCV;
		item.pPersonID = this.idPerson;
		item.pLang = lang;
		MostrarUpdateProgress();
		$.post(urlEdicionCV + 'ValidateSignatures', item, function (data) {
			OcultarUpdateProgress();
			var i = 0;
			var htmlSinCandidatos = `<div class="user-miniatura">
                                <div class="imagen-usuario-wrap">
									<div class="imagen">
									</div>
                                </div>
                                <div class="nombre-usuario-wrap">
									<p class="nombre">Ninguna sugerencia</p>
                                </div>
                                <div class="coincidencia-wrap">
                                    <a href="javascript: void(0);" class="form-buscar">Buscar</a>
                                </div>
                            </div>`;
			var vacio = true;
			for (var firma in data) {
				vacio = false;
				var numCandidatos = data[firma].length;
				i++;

				var htmlCandidatos = htmlSinCandidatos;
				if (numCandidatos > 0) {
					var score = (data[firma][0].score * 100).toFixed(0);
					htmlCandidatos = that.htmlCandidatoFirma(data[firma][0], i, score);
					var otrosCandidatos = '';
					for (var j = 1; j < numCandidatos; j++) {
						var scoreIn = (data[firma][j].score * 100).toFixed(0);
						if (scoreIn >= 80) {
							htmlCandidatos += that.htmlCandidatoFirma(data[firma][j], i, scoreIn);
						} else {
							otrosCandidatos += that.htmlCandidatoFirma(data[firma][j], i, scoreIn);
						}
					}
					htmlCandidatos += `	<a href="#groupCollapse${i}" data-toggle="collapse" aria-expanded="true" class="collapse-toggle collapsed">${GetText('CV_MASRESULTADOS')}</a>
										<div id="groupCollapse${i}" class="collapse-wrap collapse">
											${otrosCandidatos}
											<div class="form-actions">
												<a href="javascript: void(0);" class="form-buscar">${GetText('CV_BUSCAR')}</a>
										 	</div>
										</div>`;


				}
				var autor = `	<div class="form-group full-group">
								<div class="simple-collapse">
									<label class="control-label d-block">${firma}</label>
										${htmlCandidatos}
								</div>
							</div>`;
				$('#modal-anadir-autor .formulario-edicion .resultados').append(autor);
			};
			$('#modal-anadir-autor .formulario-edicion .resultados').show();
			$('#modal-anadir-autor .formulario-edicion .form-actions').show();


			that.engancharComportamientosCV();
			if (vacio) {
				$('#modal-anadir-autor .formulario-edicion .form-actions .ko').show();
				$('#modal-anadir-autor .formulario-edicion .form-actions .ko').html(GetText('CV_NOHASINTRODUCIDONINGUNAFIRMA'));
			}

		});
	},
	htmlCandidatoFirma: function (candidato, indice, score) {
		//TODO imagen
		var color = "red";
		if (score >= 90) {
			color = "green";
		} else if (score >= 80) {
			color = "orange";
		} else {
			color = "red";
		}
		var id = RandomGuid();
		var htmlAux = "";
		if (candidato.department != null) {
			htmlAux = candidato.department;
		}
		if (candidato.orcid != null) {
			if (htmlAux != '') {
				htmlAux += ' · ';
			}
			htmlAux += `<a target="_blank" class="orcid" href="https://orcid.org/${candidato.orcid}">${candidato.orcid}</a>`;
		}

		var html = `<div class="user-miniatura">
					<div class="custom-control custom-checkbox">
						<input type="checkbox" id="user-${id}" personid="${candidato.personid}" name="user-${indice}" class="custom-control-input chksignature">
						<label class="custom-control-label" for="user-${id}"></label>
					</div>
					<div class="imagen-usuario-wrap">
						<div class="imagen">
						</div>
					</div>
					<div class="nombre-usuario-wrap">
						<p class="nombre"><a target="_blank" href="${candidato.url}">${candidato.name}</a></p>
						<p class="nombre-completo">${htmlAux}</p>
					</div>
					<div class="coincidencia-wrap">
						<p class="label">${GetText('CV_COINCIDENCIA')}</p>
						<p class="numResultado" style="color: ${color};">${score}%</p>
					</div>
				</div>`;
		return html;
	},
	validarORCID: function (orcid) {
		$('#modal-anadir-autor .formulario-edicion .form-actions .ko').hide();
		$('#modal-anadir-autor .formulario-edicion .form-actions .ko').html('');
		var that = this;
		var item = {};
		item.pOrcid = orcid;
		MostrarUpdateProgress();
		$.post(urlGuardadoCV + 'ValidateORCID', item, function (data) {
			OcultarUpdateProgress();
			$('#modal-anadir-autor .formulario-codigo p.ko').hide();
			if (data.ok == false) {
				$('#modal-anadir-autor .formulario-codigo p.ko').show();
				$('#modal-anadir-autor .formulario-codigo p.ko').html(GetText('CV_ELCODIGOINTRODUCIDONOESVALIDO'));
				return;
			}
			$('#modal-anadir-autor .formulario-principal').show();
			$('#modal-anadir-autor .formulario-codigo').hide();
			$('#modal-anadir-autor .formulario-autor').hide();
			var rGuid = RandomGuid();
			var id = data.personid;
			var firma = $('.formulario-codigo .form-autor .firma').text();
			var indice = 1;
			var labelFirma = null;
			$('#modal-anadir-autor .resultados .form-group.full-group .simple-collapse>label').each(function (index) {
				if ($(this).text() == firma) {
					labelFirma = $(this);
					return;
				}
				indice++;
			});

			var htmlAux = "";
			if (data.department != null) {
				htmlAux = data.department;
			}
			if (data.orcid != null) {
				if (htmlAux != '') {
					htmlAux += ' · ';
				}
				htmlAux += `<a target="_blank" class="orcid" href="https://orcid.org/${data.orcid}">${data.orcid}</a>`;
			}

			var htmlPersona = `<div class="user-miniatura">
				<div class="custom-control custom-checkbox">
					<input disabled="disabled" checked="checked" type="checkbox" id="user-${rGuid}" personid="${id}" name="user-${indice}" class="custom-control-input chksignature">
					<label class="custom-control-label" for="user-${rGuid}"></label>
				</div>
				<div class="imagen-usuario-wrap">
					<div class="imagen">
					</div>
				</div>
				<div class="nombre-usuario-wrap">
					<p class="nombre">${data.name}</p>
					<p class="nombre-completo">${htmlAux}</p>
				</div>
			</div>`;

			if (labelFirma != null) {
				labelFirma.parent().find('div').remove();
				labelFirma.parent().find('a').remove();
				labelFirma.after(htmlPersona);
				edicionCV.engancharComportamientosCV()
			}
		});
	},
	buscarTesauro: function (valor, tesauro) {
		var lista = tesauro.find('li');
		lista.each(function (indice) {
			var item = $(this);
			var enlaceItem = item.children('a');
			var itemText = enlaceItem.text();
			item.removeClass('oculto');
			if (itemText.toLowerCase().indexOf(valor.toLowerCase()) < 0) {
				item.addClass('oculto');
			} else {
				item.removeHighlight().highlight(valor);
				item.parents('.oculto').removeClass('oculto');
			}
		});
	},
	mostrarTraducciones: function () {
		let clase = $('.acciones-listado.acciones-curriculum .traducciones a.activeView').attr('data-class-resource-list');
		var resourceList = body.find('.resource-list');
		resourceList.removeClass('translationEnView translationCaView translationEuView translationGlView translationFrView');
		resourceList.addClass(clase);
	}
};


var duplicadosCV = {
	idCV: null,
	items: null,
	pasoActual: 0,
	pasosTotales: 0,
	init: function (botonPulsado = false) {
		this.idCV = $('.contenido-cv').attr('about');
		if (!(window.location.href.indexOf('?tab=') > -1)) {
			// Si viene redireccionado de alertas, no carga duplicados.
			this.cargarDuplicados(botonPulsado);
		}
		return;
	},
	engancharComportamientos: function (envioPRC = false) {
		var that = this;
		if (envioPRC) {
			//Eliminamos desplegable acciones-curriculum
			$('#modal-enviar-produccion-cientifica .acciones-recurso-listado').remove();
			$('#modal-enviar-produccion-cientifica .itemConflict').remove();
			$('#modal-enviar-produccion-cientifica .btn-principal').remove();

			//Agregamos desplegable en items
			$('#modal-enviar-produccion-cientifica .formulario-edicion.formulario-publicacion article h2').after(`
				<select class="itemConflict" name="itemConflict">
					<option value="" selected ></option>
					<option value="0">${GetText("CV_DUPLICADO_FUSIONAR")}</option>
					<option value="1">${GetText("CV_DUPLICADO_ELIMINAR")}</option>
					<option value="2" >${GetText("CV_DUPLICADO_NO_DUPLICADO")}</option>
				</select>
			`);
			//Publicar/despublicar duplicado
			$('#modal-enviar-produccion-cientifica .resource-list .visibility-wrapper').off('click').on('click', function (e) {
				var element = $(this);
				var item = {};
				var isPublic = !$(this).find('.con-icono-before').hasClass('eye');
				item.pIdSection = that.items[that.pasoActual].idSection;
				item.pRdfTypeTab = that.items[that.pasoActual].rdfTypeTab;
				item.pEntity = $(this).parent().find('a[data-id]').attr('data-id');
				item.pIsPublic = isPublic;
				MostrarUpdateProgress();
				if (isPublic) {
					$(this).find('.con-icono-before').addClass('eye');
					$(this).find('.con-icono-before').removeClass('visibility-activo');
				} else {
					$(this).find('.con-icono-before').addClass('visibility-activo');
					$(this).find('.con-icono-before').removeClass('eye');
				}
				$.post(urlGuardadoCV + 'ChangePrivacityItem', item, function (data) {
					OcultarUpdateProgress();
				})
			});
			// Botón continuar aplicando cambios
			$('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').off('click').on('click', function (e) {
				var validar = true;
				var url = urlGuardadoCV + 'ProcesarItemsDuplicados';
				var args = {};
				args.idCV = that.idCV;
				args.idSection = that.items[that.pasoActual].idSection;
				args.rdfTypeTab = that.items[that.pasoActual].rdfTypeTab;
				args.principal = $("#modal-enviar-produccion-cientifica .resource-list.listView .middle-wrap h2 a").attr("data-id");
				args.secundarios = {};
				$("#modal-enviar-produccion-cientifica .formulario-edicion.formulario-publicacion article.resource").each(function (index) {
					var opcion = $(this).find('.itemConflict').val();
					var id = $(this).find('h2 a').attr('data-id');
					if (opcion === "") {
						validar = false;
						return false;
					} else {
						args.secundarios[id] = opcion;
					}
				});
				if (!validar) {
					mostrarNotificacion("error", GetText("DUPLICADOS_SELECCIONAR_TODAS_OPCIONES"));
				} else {
					$("#modal-enviar-produccion-cientifica .formulario-edicion.formulario-publicacion article.resource .itemConflict").each(function (index) { });
					$.post(url, args, function (data) { });
					let dataId = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('dataId');
					let idPerson = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('idPerson');
					let section = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('section');
					let rdfTypeTab = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('rdfTypeTab');
					edicionCV.PintarDataPRC(dataId, idPerson, section, rdfTypeTab);
				}
			});
			// Botón continuar sin cambios
			$('#modal-enviar-produccion-cientifica .form-actions .btn-continuar').off('click').on('click', function (e) {
				let dataId = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('dataId');
				let idPerson = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('idPerson');
				let section = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('section');
				let rdfTypeTab = $('#modal-enviar-produccion-cientifica .form-actions .btn-aplicar').attr('rdfTypeTab');
				edicionCV.PintarDataPRC(dataId, idPerson, section, rdfTypeTab);
			});
			return;
		}
		//Eliminamos desplegable acciones-curriculum
		$('#modal-posible-duplicidad .acciones-recurso-listado').remove();
		$('#modal-posible-duplicidad .itemConflict').remove();
		$('#modal-posible-duplicidad .btn-principal').remove();


		//Agregamos desplegable en items
		$('#modal-posible-duplicidad .resource-list-wrap.secundarios article h2').after(`
					   <select class="itemConflict" name="itemConflict">
							<option value="" selected ></option>
							<option value="0">${GetText("CV_DUPLICADO_FUSIONAR")}</option>
							<option value="1">${GetText("CV_DUPLICADO_ELIMINAR")}</option>
							<option value="2" >${GetText("CV_DUPLICADO_NO_DUPLICADO")}</option>
						</select>
					`);

		////Agregamos botón de convertir en principal	en items si el primero no está bloqueado
		//if(!$('#modal-posible-duplicidad .resource-list-wrap.principal article .title-wrap .block-wrapper').length)
		//{
		//	$('#modal-posible-duplicidad .resource-list-wrap.secundarios article h2').after(`<a class="btn btn-secondary uppercase btn-principal">${GetText("CV_CAMBIAR_A_PRINCIPAL")}</a>`);
		//}

		//Agregamos botón de convertir a todos los items si no hay ninguno bloqueado
		//o es un item bloqueado
		if (!$('#modal-posible-duplicidad .resource-list-wrap article .title-wrap .block-wrapper').length) {
			$('#modal-posible-duplicidad .resource-list-wrap.secundarios article h2').after(`<a class="btn btn-secondary uppercase btn-principal">${GetText("CV_CAMBIAR_A_PRINCIPAL")}</a>`);
		}
		$("#modal-posible-duplicidad .resource-list-wrap.secundarios article .title-wrap .block-wrapper").each(function (index) {
			$(this).closest('article').find('h2').after(`<a class="btn btn-secondary uppercase btn-principal">${GetText("CV_CAMBIAR_A_PRINCIPAL")}</a>`);
		});

		//Si dentro de los items hay alguno bloquedao mostramos un texto adicional
		$("#modal-posible-duplicidad .ko").remove();
		if ($('#modal-posible-duplicidad .resource-list-wrap.secundarios article .title-wrap .block-wrapper').length > 0) {
			$("#modal-posible-duplicidad .form-actions").before(`<div class="ko" style="display:block"><p>${GetText("CV_ALERTA_FUSION_ELIMINACION_BLOQUEADO")}</p></div>`);
		}

		//Botón convertir en principal
		$('#modal-posible-duplicidad .btn-principal').unbind().click(function () {
			var idActual = $(this).closest('.title-wrap').find('h2 a').attr('data-id')
			//Elimina del array
			var index = that.items[that.pasoActual].items.indexOf(idActual);
			if (index !== -1) {
				that.items[that.pasoActual].items.splice(index, 1);
			}
			//Introduce en primer lugar
			that.items[that.pasoActual].items.splice(0, 0, idActual);
			that.pintarAgrupacionDuplicados();
		});

		//Botón omitir
		$('#modal-posible-duplicidad .btn-omitir').unbind("click").bind("click", function () {
			if (that.pasoActual + 1 < that.pasosTotales) {
				that.pasoActual++;
				that.pintarAgrupacionDuplicados();
			} else {
				var minSimilarity = $('#modal-repetir-duplicidad').attr('minSimilarity');
				$('#modal-posible-duplicidad').modal('hide');
				if (minSimilarity > 0.7) {
					$('#modal-repetir-duplicidad').modal('show');
				} else {
					mostrarNotificacion("success", GetText("DUPLICADOS_DUPLICIDAD_RESUELTA"), 10000);
				}
			}
		});
		// Botón para continuar la gestión de duplicados
		$('.continuarduplicidad').unbind("click").bind("click", function () {
			$('#modal-repetir-duplicidad').modal('hide');
			that.cargarDuplicados(false, 0.7);
		});
		// Botón para cerrar la gestión de duplicados
		$('a.btn.cerrarduplicidad').unbind("click").bind("click", function () {
			$('#modal-repetir-duplicidad').modal('hide');
			mostrarNotificacion("success", GetText("DUPLICADOS_DUPLICIDAD_RESUELTA"), 10000);
		});
		//Publicar/despublicar duplicado
		$('#modal-posible-duplicidad .resource-list .visibility-wrapper').off('click').on('click', function (e) {
			var element = $(this);
			var item = {};
			var isPublic = !$(this).find('.con-icono-before').hasClass('eye');
			item.pIdSection = that.items[that.pasoActual].idSection;
			item.pRdfTypeTab = that.items[that.pasoActual].rdfTypeTab;
			item.pEntity = $(this).parent().find('a[data-id]').attr('data-id');
			item.pIsPublic = isPublic;
			MostrarUpdateProgress();
			if (isPublic) {
				$(this).find('.con-icono-before').addClass('eye');
				$(this).find('.con-icono-before').removeClass('visibility-activo');
			} else {
				$(this).find('.con-icono-before').addClass('visibility-activo');
				$(this).find('.con-icono-before').removeClass('eye');
			}
			$.post(urlGuardadoCV + 'ChangePrivacityItem', item, function (data) {
				OcultarUpdateProgress();
			})
		});
		//Botón aplicar y siguiente
		$('#modal-posible-duplicidad .btn-continuar').unbind("click").bind("click", function () {
			var validar = true;
			var url = urlGuardadoCV + 'ProcesarItemsDuplicados';
			var args = {};
			args.idCV = that.idCV;
			args.idSection = that.items[that.pasoActual].idSection;
			args.rdfTypeTab = that.items[that.pasoActual].rdfTypeTab;
			args.principal = $("#modal-posible-duplicidad .resource-list-wrap.principal article h2 a").attr("data-id");
			args.secundarios = {};

			$("#modal-posible-duplicidad .secundarios article.resource").each(function (index) {
				var opcion = $(this).find('.itemConflict').val();
				var id = $(this).find('h2 a').attr('data-id');
				if (opcion === "") {
					validar = false;
					return false;
				} else {
					args.secundarios[id] = opcion;
				}
			});
			if (!validar) {
				mostrarNotificacion("error", GetText("DUPLICADOS_SELECCIONAR_TODAS_OPCIONES"));
			} else {
				$("#modal-posible-duplicidad .secundarios article.resource .itemConflict").each(function (index) { });
				$.post(url, args, function (data) { });
				if (that.pasoActual + 1 < that.pasosTotales) {
					that.pasoActual++;
					that.pintarAgrupacionDuplicados();
				} else {
					$('#modal-posible-duplicidad').modal('hide');
					mostrarNotificacion("success", GetText("DUPLICADOS_DUPLICIDAD_RESUELTA"));
				}
			}
		});

		accionesPlegarDesplegarModal.init();
		tooltipsAccionesRecursos.init();
	},
	cargarDuplicados: function (botonPulsado, minSimilarity = 0.9) {
		if (this.idCV != null) {
			var that = this;
			var url = urlEdicionCV + "GetItemsDuplicados?pCVId=" + this.idCV + "&pMinSimilarity=" + minSimilarity;
			$('#modal-repetir-duplicidad').attr('minSimilarity', minSimilarity);
			MostrarUpdateProgress();
			$.get(url, null, function (data) {
				that.items = data;
				that.pasoActual = 0;
				that.pasosTotales = that.items.length;
				if (that.pasosTotales > 0) {
					that.pintarItemsDuplicados();
					$('#modal-repetir-duplicidad').addClass('visible');
				} else {
					if (botonPulsado) {
						$('#modal-repetir-duplicidad').modal('show');
						$('.continuarduplicidad').unbind("click").bind("click", function () {
							$('#modal-repetir-duplicidad').modal('hide');
							that.cargarDuplicados(false, 0.7);
						});
						$('.cerrarduplicidad').unbind("click").bind("click", function () {
							$('#modal-repetir-duplicidad').modal('hide');
							mostrarNotificacion("success", GetText("DUPLICADOS_DUPLICIDAD_RESUELTA"), 10000);
						});
					} else if (minSimilarity == 0.7) {
						mostrarNotificacion("success", GetText("DUPLICADOS_DUPLICIDAD_RESUELTA"), 10000);
					}
					$('#modal-repetir-duplicidad').removeClass('visible');
					OcultarUpdateProgress();
				}
			});
		}
	}
	,
	pintarItemsDuplicados: function () {
		if (this.items.length > 0) {
			var modal = $("#modal-posible-duplicidad");
			modal.modal('show');
			this.pintarAgrupacionDuplicados();
		}
	}
	,
	pintarAgrupacionDuplicados: async function () {
		var that = this;
		$('#modal-posible-duplicidad .resource-list-wrap').empty();
		var principal = true;
		$('#modal-posible-duplicidad .numpasos').html(' (' + (this.pasoActual + 1) + "/" + this.pasosTotales + ')');
		MostrarUpdateProgress();
		$("#modal-posible-duplicidad").find(".btn.btn-primary").addClass("disabled");
		var numActual = 0;
		for (var itemIn in this.items[this.pasoActual].items) {
			if (principal) {
				let aux = itemIn;
				$.get(urlEdicionCV + 'GetItemMini?pCVId=' + that.idCV + '&pIdSection=' + this.items[this.pasoActual].idSection + "&pRdfTypeTab=" + this.items[this.pasoActual].rdfTypeTab + "&pEntityID=" + this.items[this.pasoActual].items[itemIn] + "&pLang=" + lang, null, function (data) {
					var htmlItem = edicionCV.printHtmlListItem(that.items[that.pasoActual].items[aux], data);
					$('#modal-posible-duplicidad .resource-list-wrap.principal').append(htmlItem);
					numActual++;
					if (numActual == that.items[that.pasoActual].items.length) {
						OcultarUpdateProgress();
						$("#modal-posible-duplicidad").find(".btn.btn-primary").removeClass("disabled");
					}
					that.engancharComportamientos();
				});
			} else {
				let aux = itemIn;
				$.get(urlEdicionCV + 'GetItemMini?pCVId=' + that.idCV + '&pIdSection=' + this.items[this.pasoActual].idSection + "&pRdfTypeTab=" + this.items[this.pasoActual].rdfTypeTab + "&pEntityID=" + this.items[this.pasoActual].items[itemIn] + "&pLang=" + lang, null, function (data) {
					var htmlItem = edicionCV.printHtmlListItem(that.items[that.pasoActual].items[aux], data);
					$('#modal-posible-duplicidad .resource-list-wrap.secundarios').append(htmlItem);
					numActual++;
					if (numActual == that.items[that.pasoActual].items.length) {
						OcultarUpdateProgress();
						$("#modal-posible-duplicidad").find(".btn.btn-primary").removeClass("disabled");
					}
					that.engancharComportamientos();
				});
			}
			principal = false;
		}

	}
}


//Métodos auxiliares
function EliminarAcentos(texto) {
	if (texto == null) {
		texto = "";
	}
	texto = texto.toLowerCase();
	var ts = '';
	for (var i = 0; i < texto.length; i++) {
		var c = texto.charCodeAt(i);
		if (c >= 224 && c <= 230) { ts += 'a'; } else if (c >= 232 && c <= 235) { ts += 'e'; } else if (c >= 236 && c <= 239) { ts += 'i'; } else if (c >= 242 && c <= 246) { ts += 'o'; } else if (c >= 249 && c <= 252) { ts += 'u'; } else { ts += texto.charAt(i); }
	}
	return ts;
}

function addAutocompletar(control) {
	$(control).on("keyup", e => {
		if ($(e.target).val() != $(e.target).data('previousValue')) {
			if ($(e.target).attr('multilang') == null) {
				$(control).parent().find('input[propertyorigin="' + $(control).attr('propertyrdf') + '"]').val('');
				$(control).parent().find('input[propertyorigin="' + $(control).attr('propertyrdf') + '"]').change();
			}
		}
	});

	$(control).on('keydown', e => {
		$(e.target).data('previousValue', $(e.target).val());
	})

	if ($(control).hasClass('autocompleteentity')) {
		$(control).on("change", e => {
			//Si no está la entidad se elimina el valor
			if ($(control).parent().find('input[propertyorigin="' + $(control).attr('propertyrdf') + '"]').val() == '') {
				$(control).val('');
			}
		});
	}

	var urlAutocomplete = null
	var pGetEntityID = false;
	//Por defecto busca en los valores introducidos en esa propiedad en otras entidades iguales
	urlAutocomplete = urlEdicionCV + "GetAutocomplete";
	var pProperty = $(control).attr('propertyrdf');
	var pRdfType = $('#modal-editar-entidad form').attr('rdftype');
	var pGraph = $('#modal-editar-entidad form').attr('ontology');
	var pLang = 'es';
	var pPrint = null;
	var pPropertiesAux = null;
	if ($(control).attr('multilang') != null) {
		pLang = $(control).attr('multilang');
	}
	//Si hay alguna configuración lo coge de la configuración
	if ($(control).attr('propertyautocomplete') != null) {
		pProperty = $(control).attr('propertyautocomplete');
	}
	if ($(control).attr('propertyautocompleteaux') != null) {
		pPropertiesAux = $(control).attr('propertyautocompleteaux');
	}
	if ($(control).attr('propertyautocompleteprint') != null) {
		pPrint = $(control).attr('propertyautocompleteprint');
	}
	if ($(control).attr('rdftypeautocomplete') != null) {
		pRdfType = $(control).attr('rdftypeautocomplete');
	}
	if ($(control).attr('graphautocomplete') != null) {
		pGraph = $(control).attr('graphautocomplete');
	}
	if ($(control).attr('cache') != null) {
		pCache = $(control).attr('cache') == 'true';
	} else {
		pCache = false;
	}
	if ($(control).attr('entityidautocomplete') == 'true') {
		pGetEntityID = true;
	}


	var btnID = 'add_' + pProperty;
	$(control).parent().find('.acciones-listado-edicion .add').attr('id', btnID);

	$(control).autocomplete(
		null, {
		url: urlAutocomplete,
		type: 'post',
		delay: 0,
		multiple: false,
		scroll: false,
		selectFirst: false,
		minChars: 3,
		width: 300,
		cacheLength: 0,
		parse: function (data) {
			var parsed = [];
			try {
				if (data.length == null) {
					for (var i = 0; i < Object.keys(data).length; i++) {
						var id = Object.keys(data)[i];
						var txt = data[id];

						parsed[parsed.length] = {
							data: txt,
							value: id,
							result: txt
						};
					}
				} else {
					for (var i = 0; i < data.length; i++) {
						var row = data[i];

						parsed[parsed.length] = {
							data: row,
							value: row,
							result: row
						};
					}
				}
			} catch (ex) { }
			return parsed;
		},
		formatItem: function (data) { return data; },
		extraParams: {
			pPropertiesAux: function () {
				var lista = [];
				if (pPropertiesAux != null && pPropertiesAux != '') {
					var pPropertiesAuxSplit = pPropertiesAux.split('|');
					for (var i = 0; i < pPropertiesAuxSplit.length; i++) {
						lista.push(pPropertiesAuxSplit[i]);
					}
				}
				return lista;
			},
			pPrint: pPrint,
			lista: function () {
				var lista = '';
				$('.added input[propertyrdf="' + pProperty + '"]').each(function () {
					lista += $(this).val().trim() + ',';
				});
				if (pProperty == 'http://w3id.org/roh/userKeywords') {
					$('.added input[propertyrdf="http://w3id.org/roh/enrichedKeywords"]').each(function () {
						lista += $(this).val().trim() + ',';
					});
					$('.added input[propertyrdf="http://w3id.org/roh/externalKeywords"]').each(function () {
						lista += $(this).val().trim() + ',';
					});
				}
				return lista;
			},
			pProperty: pProperty,
			pRdfType: pRdfType,
			pGraph: pGraph,
			pLang: pLang,
			pCache: pCache,
			pGetEntityID: pGetEntityID,
			botonBuscar: btnID,

		}
	}
	);
	$(control).removeAttr('onfocus')
}

function TransFormData(data, type) {
	switch (type) {
		case 'date':
			var aux = [];
			data.forEach(function (valor, indice, array) {
				aux.push(valor.substring(6, 8) + "/" + valor.substring(4, 6) + "/" + valor.substring(0, 4));
			})
			return aux;
			//Declaraciones ejecutadas cuando el resultado de expresión coincide con el valor1
			break;
		default:
			return data;
			break;
	}
}

function RandomGuid() {
	return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
		var r = Math.random() * 16 | 0,
			v = c == 'x' ? r : (r & 0x3 | 0x8);
		return v.toString(16);
	});
}

var iniciarComportamientoImagenUsuario = {
	init: function () {
		this.config();
		this.montarPlugin();
	},
	config: function () {
		this.droparea = $('#foto-perfil-cv');
	},
	montarPlugin: function () {
		options = {
			sizeLimit: 100,
		};
		this.droparea.imageDropArea2(options);
	},
};

function objectToFormData(obj, form, namespace) {

	var fd = form || new FormData();
	var formKey;

	for (var property in obj) {
		if (obj.hasOwnProperty(property)) {
			if (namespace) {
				formKey = namespace + '[' + property + ']';
			} else {
				formKey = property;
			}

			if (Array.isArray(obj[property])) {
				for (var itemArray in obj[property]) {
					if (typeof obj[property][itemArray] === 'object') {
						objectToFormData(obj[property][itemArray], fd, formKey + '[' + itemArray + ']');
					} else {
						fd.append(formKey + '[' + itemArray + ']', obj[property]);
					}
				}
			} else if (typeof obj[property] === 'object' && !(obj[property] instanceof File)) {
				objectToFormData(obj[property], fd, property);
			} else {
				if (obj[property] != null) {
					// if it's a string or a File object
					fd.append(formKey, obj[property]);
				}
			}

		}
	}

	return fd;

};

//Fin de métodos auxiliares

//Imágenes
$.imageDropArea2 = function (element, options) {
	var defaults = {
		sizeLimit: false,
		ajax: false,
		inputSelector: "#foto-perfil-cv .image-uploader__input",
		dropAreaSelector: ".image-uploader__drop-area",
		preview: ".image-uploader__preview",
		previewImg: ".image-uploader__img",
		errorDisplay: ".image-uploader__error",
		// funcionPrueba: function() {}
	};
	var plugin = this;

	plugin.settings = {};

	var $element = $(element);
	var element = element;

	plugin.init = function () {
		plugin.settings = $.extend({}, defaults, options);
		plugin["input"] = $(plugin.settings.inputSelector);
		plugin["dropAreaSelector"] = $(plugin.settings.dropAreaSelector);
		plugin["preview"] = $(plugin.settings.preview);
		plugin["previewImg"] = $(plugin.settings.previewImg);
		plugin["errorDisplay"] = $(plugin.settings.errorDisplay);
		addInputChangeEvent();
		addDragAndDropEvents();
		initialImageCheck();
	};

	/**
	 * Comprueba si en el inicio del plugin ya hay una imagen
	 * para añadirla al input file
	 */
	var initialImageCheck = async function () {
		const image_url = plugin.previewImg.attr("src");
		if (image_url && image_url !== "") {
			const dT = new ClipboardEvent('').clipboardData || new DataTransfer();
			const response = await fetch(image_url);
			const data = await response.blob();
			const metadata = {
				type: 'image/jpeg'
			};
			const file = new File([data], image_url, metadata);
			dT.items.add(file);
			plugin.input.get(0).files = dT.files;
			plugin.input.trigger('change');
		};
	};

	var addInputChangeEvent = function () {
		plugin.input.unbind('change').change(function () {
			$(this).parent().find('input[type="hidden"]').val('');
			if (!isFileImage()) {
				return;
			}

			if (!imageSizeAllowed()) {
				displayError(GetText('CV_IMAGENPESADEMASIADO', plugin.settings.sizeLimit));
				return;
			}

			const reader = new FileReader();
			var that = this;
			reader.addEventListener("load", function () {
				// convert image file to base64 string
				$(that).parent().find('input[type="hidden"]').val(reader.result);
			}, false);
			reader.readAsDataURL(plugin.input.get(0).files[0]);

			if (plugin.settings.ajax) {
				uploadImageWithAjax();
			} else {
				showImageTemporalPreview();
			}
		});
	};

	/**
	 * Muestra la imagen que se ha añadido al input file
	 */
	var showImageTemporalPreview = function () {

		const [file] = plugin.input.get(0).files
		if (file) {
			showImagePreview(URL.createObjectURL(file))
		}
	};

	/**
	 * Incia lógica para llamada ajax
	 */
	var uploadImageWithAjax = function () {

		if (checkAjaxSettings()) {
			return;
		};

		var data = new FormData();
		var files = plugin.input.get(0).files;
		if (files.length > 0) {
			data.append("ImagenRegistroUsuario", files[0]);
		}
		upload(data);
	};

	/**
	 * Llamada ajax
	 */
	var upload = function () {
		$.ajax({
			url: document.location.href,
			type: "POST",
			processData: false,
			contentType: false,
			data: data,
			success: function (response) {
				onSuccesResponse(response);
			},
			error: function (er) {
				displayError(er.statusText);
			},
		});
	}

	/**
	 * @return {boolean}
	 * true: Las opciones de ajax se han configurado
	 * false: Las opciones de ajax se no han configurado
	 */
	var checkAjaxSettings = function () {
		if (plugin.settings.ajax.hasProperty('param_name')) {
			console.log('La opción "ajax.param_name" no está configurada')
			return false;
		}
		if (plugin.settings.ajax.hasProperty('url')) {
			console.log('La opción de "ajax.url" no está configurada')
			return false;
		}
		return true;
	};

	/**
	 * @return {string} error
	 * Shows error message
	 */
	var displayError = function (error) {
		plugin.errorDisplay.find(".ko").text(error);
		plugin.errorDisplay.find(".ko").show();
		plugin.preview.removeClass("show-preview");
	};

	/**
	 * Hides error message
	 */
	var hideError = function () {
		plugin.errorDisplay.find(".ko").hide();
	};

	/**
	 * @param {string} response
	 */
	var onSuccesResponse = function (response) {
		if (response.indexOf("imagenes/") === 0) {
			// Es una url de imagen
			showImagePreview(response);
			showLoadingImagePreview(false);
		} else {
			// No es una url de imagen
			displayError(response);
			showLoadingImagePreview(false);
		}
	};

	/**
	 * @param {boolean} showLoading: Indicar si ha iniciado la carga y por lo tanto, es necesario mostrar un "loading".
	 * true: Mostrará el "loading"
	 * false: Quitar ese "loading" -> Fin carga de imagen
	 */
	var showLoadingImagePreview = function (showLoading) {
		// Mostrar loading
		if (showLoading) {
			// Quitar la imagen actual del preview
			plugin.preview.attr("src", "");
		} else {
			// Quitar loading
			$(".spinner-border").remove();
		}
	};

	/**
	 * @param {string} : url de la imagen
	 */
	var showImagePreview = function (url) {
		let image_url = url;
		if (plugin.settings.ajax) {
			image_url = "http://serviciospruebas.gnoss.net" + "/" + url;
		}
		hideError();
		plugin.previewImg.attr("src", image_url);
		plugin.preview.addClass("show-preview");
	};

	var addDragAndDropEvents = function () {
		plugin.dropAreaSelector
			.off("dragenter dragover")
			.on("dragenter dragover", function (e) {
				e.preventDefault();
				e.stopPropagation();
			});

		plugin.dropAreaSelector.off("click").on("click", function (e) {
			e.preventDefault();
			e.stopPropagation();
			plugin.input.trigger("click");
		});

		plugin.dropAreaSelector.off("dragleave").on("dragleave", function (e) {
			e.preventDefault();
			e.stopPropagation();
		});

		plugin.dropAreaSelector.off("drop").on("drop", function (e) {
			e.preventDefault();
			e.stopPropagation();
			let dt = e.originalEvent.dataTransfer;
			let files = dt.files;
			plugin.input.get(0).files = files;
			plugin.input.trigger("change");
		});
	};

	/**
	 * @return {boolean} return if file is a valid image
	 */
	var isFileImage = function () {
		const acceptedImageTypes = ['image/jpeg', 'image/png'];
		const file = plugin.input.get(0).files[0];
		return file && acceptedImageTypes.includes(file['type'])
	}

	/**
	 * @return {boolean} return if file is valid size
	 */
	var imageSizeAllowed = function () {
		if (plugin.settings.sizeLimit) {
			const file = plugin.input.get(0).files[0];
			return (file.size / 1024) < plugin.settings.sizeLimit;
		} else {
			return true;
		}
	}

	plugin.init();
};

// add the plugin to the jQuery.fn object
$.fn.imageDropArea2 = function (options) {
	return this.each(function () {
		if (undefined == $(this).data("imageDropArea2")) {
			var plugin = new $.imageDropArea2(this, options);
			$(this).data("imageDropArea2", plugin);
		}
	});
};

/*
 * jQuery Autocomplete plugin 1.1
 *
 * Copyright (c) 2009 JÃ¶rn Zaefferer
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 *
 * Revision: $Id: jquery.autocomplete.js 15 2009-08-22 10:30:27Z joern.zaefferer $
 */

; (function ($) {

	$.fn.extend({
		autocomplete: function (urlOrData, options) {
			var isUrl = typeof urlOrData == "string";
			options = $.extend({}, $.Autocompleter.defaults, {
				url: isUrl ? urlOrData : null,
				data: isUrl ? null : urlOrData,
				delay: isUrl ? $.Autocompleter.defaults.delay : 10,
				max: options && !options.scroll ? 10 : 150,
				urlmultiple: null,
				urlActual: 0,
				urlParteAsmx: null,
				urlServicio: function () {
					if (this.urlmultiple != null) {
						var urlServ = this.urlmultiple[this.urlActual] + this.urlParteAsmx;
						this.urlActual++;
						if (this.urlActual == this.urlmultiple.length) { this.urlActual = 0 }
						return urlServ;
					}
					if (this.servicio != null) {
						return this.servicio.service;
					}
					else {
						return this.url;
					}
				}
			}, options);

			// if highlight is set to false, replace it with a do-nothing function
			options.highlight = options.highlight || function (value) { return value; };

			// if the formatMatch option is not specified, then use formatItem for backwards compatibility
			options.formatMatch = options.formatMatch || options.formatItem;

			//Cargo las urls multiples en caso de haberlas:
			ObtenerUrlMultiple(options);

			return this.each(function () {
				new $.Autocompleter(this, options);
			});
		},
		result: function (handler) {
			return this.bind("result", handler);
		},
		search: function (handler) {
			return this.trigger("search", [handler]);
		},
		flushCache: function () {
			return this.trigger("flushCache");
		},
		setOptions: function (options) {
			return this.trigger("setOptions", [options]);
		},
		unautocomplete: function () {
			return this.trigger("unautocomplete");
		}
	});

	$.Autocompleter = function (input, options) {

		var KEY = {
			LEFT: 37,
			UP: 38,
			RIGHT: 39,
			DOWN: 40,
			DEL: 46,
			TAB: 9,
			RETURN: 13,
			ESC: 27,
			COMMA: 188,
			PAGEUP: 33,
			PAGEDOWN: 34,
			BACKSPACE: 8
		};

		// Create $ object for input element
		var $input = $(input).attr("autocomplete", "off").addClass(options.inputClass);

		var cont = 0;
		var timeout;
		var previousValue = "";
		var cache = $.Autocompleter.Cache(options);
		var hasFocus = 0;
		var lastKeyPressCode;
		var config = {
			mouseDownOnSelect: false
		};
		var select = $.Autocompleter.Select(options, input, selectCurrent, pintarSeleccionado, config);

		var blockSubmit;

		// prevent form submit in opera when selecting with return key
		//	$.browser.opera && $(input.form).bind("submit.autocomplete", function() {
		//		if (blockSubmit) {
		//			blockSubmit = false;
		//			return false;
		//		}
		//	});

		// only opera doesn't trigger keydown multiple times while pressed, others don't work with keypress at all
		$input.bind((/*$.browser.opera ? "keypress" : */"keyup") + ".autocomplete", function (event) {
			// a keypress means the input has focus
			// avoids issue where input had focus before the autocomplete was applied
			hasFocus = 1;
			// track last key pressed
			lastKeyPressCode = event.keyCode;
			switch (event.keyCode) {

				case KEY.UP:
					event.preventDefault();
					if (select.visible()) {
						select.prev();
					} else {
						onChange(0, true);
					}
					break;

				case KEY.DOWN:
					event.preventDefault();
					if (select.visible()) {
						select.next();
					} else {
						onChange(0, true);
					}
					break;

				case KEY.PAGEUP:
					event.preventDefault();
					if (select.visible()) {
						select.pageUp();
					} else {
						onChange(0, true);
					}
					break;

				case KEY.PAGEDOWN:
					event.preventDefault();
					if (select.visible()) {
						select.pageDown();
					} else {
						onChange(0, true);
					}
					break;
				// matches also semicolon
				case options.multiple && $.trim(options.multipleSeparator) == "," && KEY.COMMA:
					select.hide();
					PintarTags($input);
					break;
				case KEY.TAB:
				case KEY.RETURN:
					cancelEvent(event);
					if (selectCurrent()) {
						// stop default to prevent a form submit, Opera needs special handling
						event.preventDefault();
						blockSubmit = true;
						return false;
					}
					select.hide();
					PintarTags($input);
					break;
				case KEY.LEFT:
				case KEY.RIGHT:
				case KEY.ESC:
					select.hide();
					break;
				default:
					clearTimeout(timeout);
					timeout = setTimeout(onChange, options.delay);
					break;
			}
		}).focus(function () {
			// track whether the field has focus, we shouldn't process any
			// results if the field no longer has focus
			hasFocus++;
		}).blur(function () {
			hasFocus = 0;
			if (!config.mouseDownOnSelect) {
				hideResults();
			}
		}).click(function () {
			// show select when clicking in a focused field
			if (hasFocus++ > 1 && !select.visible()) {
				onChange(0, true);
			}
		}).bind("search", function () {
			// TODO why not just specifying both arguments?
			var fn = (arguments.length > 1) ? arguments[1] : null;
			function findValueCallback(q, data) {
				var result;
				if (data && data.length) {
					for (var i = 0; i < data.length; i++) {
						if (data[i].result.toLowerCase() == q.toLowerCase()) {
							result = data[i];
							break;
						}
					}
				}
				if (typeof fn == "function") fn(result);
				else $input.trigger("result", result && [result.data, result.value]);
			}
			$.each(trimWords($input.val()), function (i, value) {
				request(value, findValueCallback, findValueCallback);
			});
		}).bind("flushCache", function () {
			cache.flush();
		}).bind("setOptions", function () {
			$.extend(options, arguments[1]);
			// if we've updated the data, repopulate
			if ("data" in arguments[1])
				cache.populate();
		}).bind("unautocomplete", function () {
			select.unbind();
			$input.unbind();
			$(input.form).unbind(".autocomplete");
		});


		function selectCurrent() {
			var selected = select.selected();
			pintarSeleccionado($input, selected.result);
			if ($input.parent().find('input[propertyorigin="' + $input.attr('propertyrdf') + '"]').length > 0) {
				let entidadDestino = $input.parent().find('input[propertyorigin="' + $input.attr('propertyrdf') + '"]');
				entidadDestino.val(selected.value);
				entidadDestino.change();
			}
			hideResultsNow();
			return true;
		}

		function pintarSeleccionado(textbox, resultado) {
			if (!options.pintarConcatenadores) {
				resultado = QuitarContadores(resultado);
			}

			if (textbox.attr('id') == 'finderSection' || textbox.attr('id') == 'txtBusquedaPrincipal') {
				/*Si es el buscador de una pÃ¡gina de busqueda o el metabuscador superior, autocompleta con "" */
				resultado = '"' + resultado + '"';
			}


			var v = resultado;
			previousValue = v;
			var cursorAt = textbox.selection().start;
			if (cursorAt < 0) {
				cursorAt = textbox.val().indexOf(v) + resultado.length;
			}

			if (options.multiple) {
				var words = trimWords(textbox.val());
				if (words.length > 1) {
					var seperator = options.multipleSeparator.length;
					var wordAt, progress = 0;
					$.each(words, function (i, word) {
						progress += word.length;
						if (cursorAt <= progress) {
							wordAt = i;
							return false;
						}
						progress += seperator;
					});
					words[wordAt] = v;
					v = words.join(options.multipleSeparator);
				}
			}

			if (options.NoPintarSeleccionado == null || !options.NoPintarSeleccionado) {
				textbox.val(v);
				PintarTags(textbox);
			}
		}

		function forzarClickBoton(pFaceta, result) {
			//envia los datos al seleccionar una fila del autocompletar
			var objecte = document.getElementById(options.extraParams.botonBuscar);
			if (objecte != null) {
				if (pFaceta != '') {
					if (objecte.attributes['onclick'].value.indexOf('= url + parametros') != -1) {
						eval(objecte.attributes['onclick'].value.replace('= url + parametros', '= url.replace("search=","' + pFaceta + '=") + parametros'));
					}
					else {
						eval(objecte.attributes['onclick'].value.replace('search=', pFaceta + '=').replace('return false;', ''));
					}
				}
				/*else if(document.createEvent)
				{
					var evObj = document.createEvent('MouseEvents');
					evObj.initEvent( 'click', true, false );
					objecte.dispatchEvent(evObj);
				}*/
				else {
					if ($input.val().indexOf("\"") != 0 && $input.val().lastIndexOf("\"") != $input.val().length - 1 && typeof (result) != 'undefined' && result == '' && typeof ($input[0]) != 'undefined' && typeof ($input[0].className) != 'undefined' && $input[0].className.indexOf("filtroFaceta") >= 0) {
						pintarSeleccionado($input, '>>' + $input.val())
					}
					objecte.click();
				}
			}
		}

		function onChange(crap, skipPrevCheck) {
			if (lastKeyPressCode == KEY.DEL) {
				select.hide();
				return;
			}

			var currentValue = $input.val();

			if (!skipPrevCheck && currentValue == previousValue)
				return;

			previousValue = currentValue;

			currentValue = lastWord(currentValue);
			if (currentValue.length >= options.minChars) {
				$input.addClass(options.loadingClass);
				if (!options.matchCase)
					currentValue = currentValue.toLowerCase();
				request(currentValue, receiveData, hideResultsNow);
			} else {
				stopLoading();
				select.hide();
			}
		};

		function trimWords(value) {
			if (!value)
				return [""];
			if (!options.multiple)
				return [$.trim(value)];
			return $.map(value.split(options.multipleSeparator), function (word) {
				return $.trim(value).length ? $.trim(word) : null;
			});
		}

		function lastWord(value) {
			if (!options.multiple)
				return value;
			var words = trimWords(value);
			if (words.length == 1)
				return words[0];
			var cursorAt = $(input).selection().start;
			if (cursorAt == value.length) {
				words = trimWords(value)
			} else {
				words = trimWords(value.replace(value.substring(cursorAt), ""));
			}
			return words[words.length - 1];
		}

		// fills in the input box w/the first match (assumed to be the best match)
		// q: the term entered
		// sValue: the first matching result
		function autoFill(q, sValue) {
			// autofill in the complete box w/the first match as long as the user hasn't entered in more data
			// if the last user key pressed was backspace, don't autofill
			if (options.autoFill && (lastWord($input.val()).toLowerCase() == q.toLowerCase()) && lastKeyPressCode != KEY.BACKSPACE) {
				// fill in the value (keep the case the user has typed)
				$input.val($input.val() + sValue.substring(lastWord(previousValue).length));
				// select the portion of the value not typed by the user (so the next character will erase)
				$(input).selection(previousValue.length, previousValue.length + sValue.length);
			}
		};

		function hideResults() {
			clearTimeout(timeout);
			timeout = setTimeout(hideResultsNow, 200);
		};

		function hideResultsNow() {
			var wasVisible = select.visible();
			select.hide();
			clearTimeout(timeout);
			stopLoading();
			if (options.mustMatch) {
				// call search and run callback
				$input.search(
					function (result) {
						// if no value found, clear the input box
						if (!result) {
							if (options.multiple) {
								var words = trimWords($input.val()).slice(0, -1);
								$input.val(words.join(options.multipleSeparator) + (words.length ? options.multipleSeparator : ""));
							}
							else {
								$input.val("");
								$input.trigger("result", null);
							}
						}
					}
				);
			}
		};

		function receiveData(q, data) {
			if (data && data.length && hasFocus) {
				stopLoading();
				select.display(data, q);
				autoFill(q, data[0].value);

				if (typeof (completadaCargaAutocompletar) != "undefined") {
					completadaCargaAutocompletar();
				}

				select.show();
			} else {
				hideResultsNow();
			}
		};

		function request(term, success, failure) {
			if (typeof (requestAutocompletarPersonalizado) != 'undefined') {
				return requestAutocompletarPersonalizado(term, success, failure, options, cache, cont, lastWord, parse, normalize);
			}

			term = replaceAll(replaceAll(replaceAll(term, '%', '%25'), '#', '%23'), '+', "%2B");
			if (!options.matchCase)
				term = term.toLowerCase();
			var data = null;

			if (options.data == null) {
				data = cache.load(term);
			}

			cont = cont + 1;
			var extraParams = {
				q: lastWord(term),
				limit: options.max,
				cont: cont,
				lista: '',
				callback: 'autocomplete'
			};
			if (options.multiple) {
				if (options.classTxtValoresSelecc != null) {
					var valorLista = '';

					$('.' + options.classTxtValoresSelecc).each(function () {
						valorLista += $(this).val().replace(/&/g, ',');
					});

					extraParams["lista"] = valorLista;
				}
				else if (options.txtValoresSeleccID == null) {
					extraParams["lista"] = $('#' + input.id + '_Hack').val().trim() + $input.val().trim();
					//extraParams["lista"] = previousValue.trim();
				}
				else {
					if (options.txtValoresSeleccID.indexOf('|') != -1) {
						var idtxthacks = options.txtValoresSeleccID.split('|');
						var valorLista = '';
						for (var i = 0; i < idtxthacks.length; i++) {
							valorLista += document.getElementById(idtxthacks[i]).value.replace(/&/g, ',');
						}
						extraParams["lista"] = valorLista;
					}
					else {
						extraParams["lista"] = document.getElementById(options.txtValoresSeleccID).value.replace(/&/g, ',');
					}
				}
			}
			$.each(options.extraParams, function (key, param) {
				extraParams[key] = typeof param == "function" ? param() : param;
			});

			// recieve the cached data
			if (data && data.length) {
				success(term, data);
				// if an AJAX url has been supplied, try loading the data now
			} else if ((typeof options.url == "string") && (options.url.length > 0)) {
				var urlPost = options.urlServicio(options);

				$.ajax({
					type: "POST",
					url: urlPost,
					data: extraParams,
					cache: false
				}).done(function (response) {
					var data = response;
					if (response.d != null) {
						data = response.d;
					}

					var parsed = options.parse && options.parse(data) || parse(data);
					cache.add(term, parsed);
					success(term, parsed);
				}).fail(function (data) {

				});
				//$.post(options.url, extraParams, function (response) {
				//    var parsed = options.parse && options.parse(response) || parse(response);
				//    cache.add(term, parsed);
				//    success(term, parsed);
				//}, "json");
			}
			else if (options.servicio != null) {


				options.servicio.service = options.urlServicio(options);

				options.servicio.call(options.metodo, extraParams, function (data) {
					if (extraParams.cont == cont && $('#' + extraParams.botonBuscar).prev().parent().css('display') != 'none') {
						var parsed = options.parse && options.parse(data) || parse(data);
						cache.add(term, parsed);
						success(term, parsed);
					}
				});
			}
			else if (options.data != null && options.data.length > 0) {
				var parsed = [];
				var termNorm = normalize(term.toLowerCase());

				for (var i = 0; i < options.data.length; i++) {
					var nombreBuscar = normalize(options.data[i][0].toLowerCase());
					if (nombreBuscar.indexOf(termNorm) == 0 || nombreBuscar.indexOf(' ' + termNorm) != -1) {
						parsed.push({ 'data': options.data[i], 'value': options.data[i][0], 'result': options.data[i][0] });
					}
				}

				success(term, parsed);
			}
			else {
				// if we have a failure, we need to empty the list -- this prevents the the [TAB] key from selecting the last successful match
				select.emptyList();
				failure(term);
			}
		};


		var normalize = (function () {
			var from = "ÃƒÃ€ÃÃ„Ã‚ÃˆÃ‰Ã‹ÃŠÃŒÃÃÃŽÃ’Ã“Ã–Ã”Ã™ÃšÃœÃ›Ã£Ã Ã¡Ã¤Ã¢Ã¨Ã©Ã«ÃªÃ¬Ã­Ã¯Ã®Ã²Ã³Ã¶Ã´Ã¹ÃºÃ¼Ã»Ã‘Ã±Ã‡Ã§",
				to = "AAAAAEEEEIIIIOOOOUUUUaaaaaeeeeiiiioooouuuunncc",
				mapping = {};

			for (var i = 0, j = from.length; i < j; i++)
				mapping[from.charAt(i)] = to.charAt(i);

			return function (str) {
				var ret = [];
				for (var i = 0, j = str.length; i < j; i++) {
					var c = str.charAt(i);
					if (mapping.hasOwnProperty(str.charAt(i)))
						ret.push(mapping[c]);
					else
						ret.push(c);
				}
				return ret.join('');
			}

		})();

		function parse(data) {
			var parsed = [];
			try {
				var rows = data.split("\n");
				for (var i = 0; i < rows.length; i++) {
					var row = $.trim(rows[i]);
					if (row) {
						if (row.indexOf('|||') != -1) {
							row = row.split("|||");
						}
						else if (row.indexOf('|') != -1) {
							var valor = row.substring(0, row.lastIndexOf('|'));
							var attControl = row.substring(row.lastIndexOf('|') + 1);
							row = [valor, attControl];
						}
						else {
							row = row.split("|");//Para que cree un array de un elemento.
						}

						parsed[parsed.length] = {
							data: row,
							value: row[0],
							result: options.formatResult && options.formatResult(row, row[0]) || row[0]
						};
					}
				}
			}
			catch (ex) { }
			return parsed;
		};

		function stopLoading() {
			$input.removeClass(options.loadingClass);
		};
	};

	function QuitarContadores(cadena) {
		var resultado = cadena;

		// Obtener la cadena entre parÃ©ntesis
		var contenidoParentesis = resultado.substring(resultado.lastIndexOf('(') + 1);
		contenidoParentesis = contenidoParentesis.substring(0, contenidoParentesis.lastIndexOf(')'));

		// Si es un entero, quitamos los parÃ©ntesis
		if (contenidoParentesis == parseInt(contenidoParentesis)) {
			if (cadena.lastIndexOf('(') > -1) {
				if (cadena.lastIndexOf(')') > -1 && cadena.lastIndexOf(')') > cadena.lastIndexOf('(')) {
					resultado = cadena.substring(0, cadena.lastIndexOf('(') - 1);
				}
			}
		}

		return resultado;
	}

	$.Autocompleter.defaults = {
		inputClass: "ac_input",
		resultsClass: "ac_results",
		loadingClass: "ac_loading",
		minChars: 1,
		delay: 400,
		matchCase: false,
		matchSubset: true,
		matchContains: false,
		cacheLength: 10,
		max: 100,
		mustMatch: false,
		extraParams: {},
		selectFirst: true,
		formatItem: function (row) { return row[0]; },
		formatMatch: null,
		autoFill: false,
		width: 0,
		multiple: false,
		multipleSeparator: ", ",
		/*highlight: function(value, term) {
			return value.replace(new RegExp("(?![^&;]+;)(?!<[^<>]*)(" + term.replace(/([\^\$\(\)\[\]\{\}\*\.\+\?\|\\])/gi, "\\$1") + ")(?![^<>]*>)(?![^&;]+;)", "gi"), "<strong>$1</strong>");
		},*/
		scroll: true,
		scrollHeight: 180
	};

	$.Autocompleter.Cache = function (options) {

		var data = {};
		var length = 0;

		function matchSubset(s, sub) {
			if (!options.matchCase)
				s = s.toLowerCase();
			var i = s.indexOf(sub);
			if (options.matchContains == "word") {
				i = s.toLowerCase().search("\\b" + sub.toLowerCase());
			}
			if (i == -1) return false;
			return i == 0 || options.matchContains;
		};

		function add(q, value) {
			if (length > options.cacheLength) {
				flush();
			}
			if (!data[q]) {
				length++;
			}
			data[q] = value;
		}

		function populate() {
			if (!options.data) return false;
			// track the matches
			var stMatchSets = {},
				nullData = 0;

			// no url was specified, we need to adjust the cache length to make sure it fits the local data store
			if (!options.url) options.cacheLength = 1;

			// track all options for minChars = 0
			stMatchSets[""] = [];

			// loop through the array and create a lookup structure
			for (var i = 0, ol = options.data.length; i < ol; i++) {
				var rawValue = options.data[i];
				// if rawValue is a string, make an array otherwise just reference the array
				rawValue = (typeof rawValue == "string") ? [rawValue] : rawValue;

				var value = options.formatMatch(rawValue, i + 1, options.data.length);
				if (value === false)
					continue;

				var firstChar = value.charAt(0).toLowerCase();
				// if no lookup array for this character exists, look it up now
				if (!stMatchSets[firstChar])
					stMatchSets[firstChar] = [];

				// if the match is a string
				var row = {
					value: value,
					data: rawValue,
					result: options.formatResult && options.formatResult(rawValue) || value
				};

				// push the current match into the set list
				stMatchSets[firstChar].push(row);

				// keep track of minChars zero items
				if (nullData++ < options.max) {
					stMatchSets[""].push(row);
				}
			};

			// add the data items to the cache
			$.each(stMatchSets, function (i, value) {
				// increase the cache size
				options.cacheLength++;
				// add to the cache
				add(i, value);
			});
		}

		// populate any existing data
		setTimeout(populate, 25);

		function flush() {
			data = {};
			length = 0;
		}

		return {
			flush: flush,
			add: add,
			populate: populate,
			load: function (q) {
				if (!options.cacheLength || !length)
					return null;
				/*
				 * if dealing w/local data and matchContains than we must make sure
				 * to loop through all the data collections looking for matches
				 */
				if (!options.url && options.matchContains) {
					// track all matches
					var csub = [];
					// loop through all the data grids for matches
					for (var k in data) {
						// don't search through the stMatchSets[""] (minChars: 0) cache
						// this prevents duplicates
						if (k.length > 0) {
							var c = data[k];
							$.each(c, function (i, x) {
								// if we've got a match, add it to the array
								if (matchSubset(x.value, q)) {
									csub.push(x);
								}
							});
						}
					}
					return csub;
				} else
					// if the exact item exists, use it
					if (data[q]) {
						return data[q];
					} else
						if (options.matchSubset) {
							for (var i = q.length - 1; i >= options.minChars; i--) {
								var c = data[q.substr(0, i)];
								if (c) {
									var csub = [];
									$.each(c, function (i, x) {
										if (matchSubset(x.value, q)) {
											csub[csub.length] = x;
										}
									});
									return csub;
								}
							}
						}
				return null;
			}
		};
	};

	$.Autocompleter.Select = function (options, input, select, pintar, config) {
		var CLASSES = {
			ACTIVE: "ac_over"
		};

		var listItems,
			active = -1,
			data,
			term = "",
			needsInit = true,
			element,
			list;

		// Create results
		function init() {
			if (!needsInit)
				return;
			element = $("<div/>")
				.hide()
				.addClass(options.resultsClass)
				.css("position", "absolute")
			//.appendTo(document.body);

			if (typeof panelContAutoComplet != 'undefined') {
				element.appendTo($("#" + panelContAutoComplet));
			}
			else {
				element.appendTo($(input).parent());
				//element.appendTo(document.body);
			}

			list = $("<ul/>").appendTo(element).mouseover(function (event) {
				if (target(event).nodeName && target(event).nodeName.toUpperCase() == 'LI') {
					active = $("li", list).removeClass(CLASSES.ACTIVE).index(target(event));
					$(target(event)).addClass(CLASSES.ACTIVE);
				}
			}).click(function (event) {
				$(target(event)).addClass(CLASSES.ACTIVE);
				select();
				// TODO provide option to avoid setting focus again after selection? useful for cleanup-on-focus
				input.focus();
				return false;
			}).mousedown(function (event) {
				cancelEvent(event);
				config.mouseDownOnSelect = true;
			}).mouseup(function () {
				config.mouseDownOnSelect = false;
			});

			if (options.width > 0)
				element.css("width", options.width);

			if (options.extraParams.maxwidth)
				element.css("max-width", options.extraParams.maxwidth);

			needsInit = false;
		}

		function target(event) {
			var element = event.target;
			while (element && element.tagName != "LI")
				element = element.parentNode;
			// more fun with IE, sometimes event.target is empty, just ignore it then
			if (!element)
				return [];
			return element;
		}

		function moveSelect(step) {
			listItems.slice(active, active + 1).removeClass(CLASSES.ACTIVE);
			movePosition(step);
			var activeItem = listItems.slice(active, active + 1).addClass(CLASSES.ACTIVE);
			if (options.scroll) {
				var offset = 0;
				listItems.slice(0, active).each(function () {
					offset += this.offsetHeight;
				});
				if ((offset + activeItem[0].offsetHeight - list.scrollTop()) > list[0].clientHeight) {
					list.scrollTop(offset + activeItem[0].offsetHeight - list.innerHeight());
				} else if (offset < list.scrollTop()) {
					list.scrollTop(offset);
				}
			}
		};

		function movePosition(step) {
			active += step;
			if (active < 0) {
				active = listItems.size() - 1;
			} else if (active >= listItems.size()) {
				active = 0;
			}
		}

		function limitNumberOfItems(available) {
			return options.max && options.max < available
				? options.max
				: available;
		}

		function fillList() {
			list.empty();
			var max = limitNumberOfItems(data.length);
			for (var i = 0; i < max; i++) {
				if (!data[i])
					continue;
				var formatted = options.formatItem(data[i].data, i + 1, max, data[i].value, term);
				if (formatted === false)
					continue;
				var li = $("<li/>").html(options.highlight(formatted, term)).addClass(i % 2 == 0 ? "ac_even" : "ac_odd").appendTo(list)[0];
				$.data(li, "ac_data", data[i]);
			}
			listItems = list.find("li");
			if (options.selectFirst) {
				listItems.slice(0, 1).addClass(CLASSES.ACTIVE);
				active = 0;
			}
			// apply bgiframe if available
			if ($.fn.bgiframe)
				list.bgiframe();
		}

		return {
			display: function (d, q) {
				init();
				data = d;
				term = q;
				fillList();
			},
			next: function () {
				moveSelect(1);
			},
			prev: function () {
				moveSelect(-1);
			},
			pageUp: function () {
				if (active != 0 && active - 8 < 0) {
					moveSelect(-active);
				} else {
					moveSelect(-8);
				}
			},
			pageDown: function () {
				if (active != listItems.size() - 1 && active + 8 > listItems.size()) {
					moveSelect(listItems.size() - 1 - active);
				} else {
					moveSelect(8);
				}
			},
			hide: function () {
				$('body').removeClass('autocompletandoPrincipal');
				element && element.hide();
				listItems && listItems.removeClass(CLASSES.ACTIVE);
				active = -1;
			},
			visible: function () {
				return element && element.is(":visible");
			},
			current: function () {
				return this.visible() && (listItems.filter("." + CLASSES.ACTIVE)[0] || options.selectFirst && listItems[0]);
			},
			show: function () {
				if (input.id == "txtBusquedaPrincipal") $('body').addClass('autocompletandoPrincipal');
				if (typeof panelContAutoComplet != 'undefined') {
					element.css('display', 'block');
				}
				else {
					var offset = $(input).offset();
					element.css({
						width: typeof options.width == "string" || options.width > 0 ? options.width : $(input).width(),
						top: offset.top + input.offsetHeight,
						left: offset.left
					}).show();
				}
				if (options.scroll) {
					list.scrollTop(0);
					list.css({
						maxHeight: options.scrollHeight,
						overflow: 'auto'
					});
				}
			},
			selected: function () {
				var selected = listItems && listItems.filter("." + CLASSES.ACTIVE).removeClass(CLASSES.ACTIVE);
				return selected && selected.length && $.data(selected[0], "ac_data");
			},
			emptyList: function () {
				list && list.empty();
			},
			unbind: function () {
				element && element.remove();
			}
		};
	};

	$.fn.selection = function (start, end) {
		if (start !== undefined) {
			return this.each(function () {
				if (this.createTextRange) {
					var selRange = this.createTextRange();
					if (end === undefined || start == end) {
						selRange.move("character", start);
						selRange.select();
					} else {
						selRange.collapse(true);
						selRange.moveStart("character", start);
						selRange.moveEnd("character", end);
						selRange.select();
					}
				} else if (this.setSelectionRange) {
					this.setSelectionRange(start, end);
				} else if (this.selectionStart) {
					this.selectionStart = start;
					this.selectionEnd = end;
				}
			});
		}
		var field = this[0];
		if (field.createTextRange && document.selection && document.selection.createRange) {
			var range = document.selection.createRange(),
				orig = field.value,
				teststring = "<->",
				textLength = range.text.length;
			range.text = teststring;
			var caretAt = field.value.indexOf(teststring);
			field.value = orig;
			this.selection(caretAt, caretAt + textLength);
			return {
				start: caretAt,
				end: caretAt + textLength
			}
		} else if (field.selectionStart !== undefined) {
			return {
				start: field.selectionStart,
				end: field.selectionEnd
			}
		}
	};
})(jQuery);


function PintarTags(textBox) {
	if (textBox.val().trim() != "") {
		var tags = textBox.val().replace(';', ',').split(',');

		var contenedor = textBox.parents('.autocompletar').find('.contenedor');
		var textBoxHack = textBox.parents('.autocompletar').find('input').last();

		if (textBoxHack.length > 0) {
			for (var i = 0; i < tags.length; i++) {
				var tagNombre = tags[i].trim();
				var tagNombreEncode = Encoder.htmlEncode(tagNombre);

				var estaYaAgregada = textBoxHack.val().trim().indexOf(',' + tagNombre + ',') != -1;
				estaYaAgregada = estaYaAgregada || textBoxHack.val().trim().substring(0, tagNombre.length + 1) == tagNombre + ',';

				if (tagNombre != '' && (!estaYaAgregada || textBox.parents('.tag').length > 0)) {
					var html = "<div class=\"tag\" title=\"" + tagNombreEncode + "\"><div>" + tagNombre + "<a class=\"remove\" ></a></div><input type=\"text\" value=\"" + tagNombreEncode + "\"></div>";
					if (textBox.parents('.tag').length > 0) {
						textBox.parents('.tag').before(html);
					}
					else {
						contenedor.append(html);
					}

					textBoxHack.val(textBoxHack.val() + tagNombre.toLowerCase() + ',')
				}
			}

			textBox.val('');

			if (textBox.parents('.tag').length == 0) {
				PosicionarTextBox(textBoxHack.prev());
			}

			if (!textBoxHack.prev().hasClass("no-edit")) {
				textBox.parents('.autocompletar').find('.tag').each(function () {
					$(this).bind('click', function (evento) {
						cancelEvent(evento);

						var divTag = $(this).children('div');
						var textBox = divTag.parent().find('input');
						if (textBox.css('display') == 'none') {
							textBox.width(textBox.parent().width());
							divTag.css('display', 'none');
							textBox.css('display', 'block');
							textBox.focus();
							posicionarCursor(textBox, textBox.val().length);
							textBox.blur(function () { ActualizarTag(textBox, divTag, textBoxHack) });
							textBox.keydown(function (evento) {
								$(this).attr('size', $(this).val().length + 5);
								if (evento.which || evento.keyCode) {
									if ((evento.which == 13) || (evento.keyCode == 13)) {
										ActualizarTag(textBox, divTag, textBoxHack);
										return false;
									}
								}
							});
							textBox.keyup(function (evento) {
								if (evento.which || evento.keyCode) {
									if ((evento.which == 188) || (evento.keyCode == 188)) {
										ActualizarTag(textBox, divTag, textBoxHack);
									}
								}
							});
						}
					});
				});
			}

			textBox.parents('.autocompletar').find('.tag .remove').each(function () {
				if ($(this).data("events") == null) {
					$(this).bind('click', function (evento) {
						cancelEvent(evento);
						EliminarTag($(this).parents('.tag'), evento)
					});
				}
			});
		}
	}
	tagYaPintado = true;
}

function PosicionarTextBox(textBox) {
	textBox.width(150);
	textBox.css('top', '0px');
	textBox.css('left', '0px');

	if (textBox.parent().find('.tag').length == 0 || textBox.position().top > textBox.parent().find('.tag').last().position().top) {
		textBox.css('width', '100%');
	}
	else {
		var tbLeft = textBox.parent().find('.tag').last().position().left + textBox.parent().find('.tag').last().width() + 5;
		textBox.width(textBox.parent().width() - (tbLeft - textBox.parent().position().left));
	}
}

function LimpiarTags(textBox) {
	$('#' + txtTagsID + '_Hack').val('');
	$('#' + txtTagsID).parent().find('.tag').remove();
}

function ActualizarTag(textBox, divTag, textBoxHack) {
	var ultimoElemento = textBoxHack.parent();
	if (ultimoElemento.next().hasClass('propuestos')) {
		ultimoElemento = ultimoElemento.next();
	}
	descartarTag(textBox.parents('.tag'), ultimoElemento);

	textBox.css('display', '');
	PintarTags(textBox);

	var valorAnterior = textBox.parents('.tag').attr('title').toLowerCase();
	var hack = textBoxHack.val().trim();

	if (hack.indexOf(',' + valorAnterior + ',') != -1) {
		hack = hack.replace(',' + valorAnterior + ',', ',');
	}
	else if (hack.substring(0, valorAnterior.length + 1) == valorAnterior + ',') {
		hack = hack.replace(valorAnterior + ',', '');
	}

	textBoxHack.val(hack.trim());
	textBox.parent().remove();
	PosicionarTextBox(textBoxHack.prev());
}

function EliminarTag(elemento, evento) {
	var divAutocompletar = elemento.parents('.autocompletar');
	if (divAutocompletar.find('input').length > 0) {
		var valorAnterior = elemento.attr('title');
		var textBoxHack = divAutocompletar.find('input').last();
		textBoxHack.val(textBoxHack.val().replace(valorAnterior.toLowerCase() + ',', ''));
		var ultimoElemento = divAutocompletar;
		if (divAutocompletar.next().hasClass('propuestos')) {
			var listaPropuestos = divAutocompletar.next().find('.tag');

			for (var i = 0; i < listaPropuestos.length; i++) {
				if ($(listaPropuestos[i]).attr('title') == valorAnterior) {
					$(listaPropuestos[i]).css('display', '');
				}
			}
			ultimoElemento = divAutocompletar.next();
		}

		descartarTag(elemento, ultimoElemento);

		elemento.remove();
		PosicionarTextBox(textBoxHack.prev());
	}
}

function descartarTag(elemento, ultimoElemento) {
	if (!ultimoElemento.next().hasClass('descartados')) {
		ultimoElemento.after("<div class='descartados' style='display:none;'><input id='txtHackDescartados' type='text'/></div>");
		ultimoElemento.next().find('#txtHackDescartados').val(elemento.attr('title').toLowerCase() + ',');
	}
	else {
		var descartados = ultimoElemento.next().find('#txtHackDescartados').val();

		var estaYaAgregada = descartados.indexOf(',' + elemento.attr('title') + ',') != -1;
		estaYaAgregada = estaYaAgregada || descartados.substring(0, elemento.attr('title').length + 1) == elemento.attr('title') + ',';

		if (!estaYaAgregada) {
			descartados += elemento.attr('title').toLowerCase() + ','
			ultimoElemento.next().find('#txtHackDescartados').val(descartados);
		}
	}
}

function posicionarCursor(textbox, pos) {
	if (textbox.get(0).setSelectionRange) {
		textbox.get(0).setSelectionRange(pos, pos);
	} else if (textbox.get(0).createTextRange) {
		var range = textbox.get(0).createTextRange();
		range.collapse(true);
		range.moveEnd('character', pos);
		range.moveStart('character', pos);
		range.select();
	}
}

$(document).ready(function () {
	pintarTagsInicio();

	// // Para que se selecione la pestaña del editor CV si esta seleccionada
	// var hash = location.hash.replace(/^#/, '');  // ^ means starting, meaning only match the first hash
	// if (hash) {
	//     $('.nav-tabs a[href="#' + hash + '"]').tab('show');
	// }

	// // Change hash for page-reload
	// $('.nav-tabs a').on('shown.bs.tab', function (e) {
	//     window.location.hash = e.target.hash;
	// })
});

function pintarTagsInicio() {
	$('.autocompletar').each(function () {
		$(this).bind('click', function (evento) {
			cancelEvent(evento);
			$(this).find('input.txtAutocomplete').focus();
		});
	});

	$('.autocompletar input.txtAutocomplete').each(function () {
		PosicionarTextBox($(this));
		$(this).bind('keydown', function (evento) {
			if ((evento.which == 8) || (evento.keyCode == 8)) {
				if ($(this).val() == "") {
					if ($(this).parent().find('.tag').last().hasClass("selected")) {
						EliminarTag($(this).parent().find('.tag').last(), evento);
					}
					else {
						$(this).parent().find('.tag').last().addClass("selected");
					}
					return false;
				}
			}
			else if ((evento.which == 9) || (evento.keyCode == 9) || (evento.which == 13) || (evento.keyCode == 13)) {
				//Tabulador o Intro
				return false;
			}
			else {
				if ($(this).parent().find('.tag').last().hasClass("selected")) {
					$(this).parent().find('.tag').last().removeClass("selected");
				}
			}
		});
		$(this).bind('blur', function (evento) {
			PintarTags($(this));
		});
		$(this).bind('click', function (evento) {
			cancelEvent(evento);
		});
		PintarTags($(this));
	});
}


function cancelEvent(e) {
	if (!e) e = window.event;
	if (e.preventDefault) {
		e.preventDefault();
	} else {
		e.returnValue = false;
	}

	if (!e) e = window.event;
	if (e.stopPropagation) {
		e.stopPropagation();
	} else {
		e.cancelBubble = true;
	}
}

function ObtenerUrlMultiple(pOptions) {
	if (pOptions.servicio != null && pOptions.servicio.service.indexOf(',') != -1) {
		pOptions.urlParteAsmx = pOptions.servicio.service.substring(pOptions.servicio.service.lastIndexOf(',') + 1);
		pOptions.urlmultiple = pOptions.servicio.service.substring(0, pOptions.servicio.service.lastIndexOf(',')).split(',');
		pOptions.urlActual = aleatorio(0, pOptions.urlmultiple.length - 1);
	}
	else if (pOptions.url != null && pOptions.url.indexOf(',') != -1) {
		pOptions.urlParteAsmx = pOptions.url.substring(pOptions.url.lastIndexOf(',') + 1);
		pOptions.urlmultiple = pOptions.url.substring(0, pOptions.url.lastIndexOf(',')).split(',');
		pOptions.urlActual = aleatorio(0, pOptions.urlmultiple.length - 1);
	}
}

function aleatorio(inferior, superior) {
	numPosibilidades = superior - inferior;
	aleat = Math.random() * numPosibilidades;
	aleat = Math.round(aleat);
	return parseInt(inferior) + aleat;
}


function getParam(param) {
	var url = window.location.href;
	url = String(url.match(/\?+.+/));
	url = url.replace("?", "");
	url = url.split("&");
	x = 0;
	while (x < url.length) {
		p = url[x].split("=");
		if (p[0] == param) {
			return decodeURIComponent(p[1]);
		}
		x++;
	}
}


tooltipsAccionesRecursos.lanzar = function () {
	montarTooltip.lanzar(this.block, GetText('CV_BLOQUEADO'), 'background-gris-oscuro');
	montarTooltip.lanzar(this.visible, GetText('CV_VISIBLE'), 'background-gris-oscuro');
	montarTooltip.lanzar(this.oculto, GetText('CV_OCULTO'), 'background-gris-oscuro');
	montarTooltip.lanzar(this.body.find('.manage-history-wrapper'), GetText('CV_PENDIENTE'), 'background-gris-oscuro');
	montarTooltip.lanzar(this.body.find('.verified-wrapper'), GetText('CV_VALIDADO'), 'background-gris-oscuro');
};

montarTooltip.lanzar = function (elem, title, classes) {
	elem.tooltip({
		html: true,
		placement: 'bottom',
		template: '<div class="tooltip infoTooltipMargin ' + classes + '" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
		title: title
	});
	this.comportamiento(elem);
};

cambioTraducciones.comportamiento = function () {
	var that = this;
	var accionesListado = this.body.find('.acciones-listado');
	var traducciones = accionesListado.find('.traducciones');
	var dropdownMenu = traducciones.find('.dropdown-menu');
	var dropdownToggle = traducciones.find('.dropdown-toggle');
	var dropdownToggleIcon = dropdownToggle.find('.material-icons');
	var modosTraducciones = dropdownMenu.find('a');

	modosTraducciones.on('click', function (e) {
		e.preventDefault();
		var resourceList = that.body.find('.resource-list');
		var item = $(this);
		var clase = item.data('class-resource-list');

		modosTraducciones.removeClass('activeView');
		item.addClass('activeView');

		if (clase != "") {
			edicionCV.mostrarTraducciones();
		}
	});
};

accionesPlegarDesplegarModal.collapse = function () {
	var button = $('.arrow');
	button.off('click').on('click', function () {
		var resource = $(this).parents('.resource');
		if (resource.hasClass('activo')) {
			resource.removeClass('activo');
		} else {
			resource.addClass('activo');
		}
	});
};

var edicionListaAutorCV = {
	init: function () {
		this.config();
		this.comportamiento();
	},
	config: function () {
		this.collapse_lista_autores = $('#collapse-lista-autores');
		this.collapse_autores = $('#collapse-autores');
	},
	comportamiento: function () {
		var that = this;
		this.collapse_lista_autores.find('.collapse-toggle').off('click').on('click', function () {
			if (that.collapse_lista_autores.hasClass('show')) {
				that.collapse_lista_autores.collapse('hide');
			}
		});

		this.collapse_autores.find('.collapse-toggle').off('click').on('click', function () {
			if (that.collapse_autores.hasClass('show')) {
				that.collapse_lista_autores.collapse('show');
			}
		});
	}
};


tooltipsCV.traducciones = function () {
	var item = this.traduccionesWrap.find('.translation');
	$.each(item, function (i) {
		var circulo = $(item[i]).find('.circulo-color');
		var texto;
		if ($(item[i]).hasClass('translation-es')) {
			texto = GetText('ESPAGNOL') + ' (ES)';
		} else if ($(item[i]).hasClass('translation-en')) {
			texto = GetText('INGLES') + ' (EN)';
		} else if ($(item[i]).hasClass('translation-ca')) {
			texto = GetText('CATALAN') + ' (CA)';
		} else if ($(item[i]).hasClass('translation-eu')) {
			texto = GetText('EUSKERA') + ' (EU)';
		} else if ($(item[i]).hasClass('translation-gl')) {
			texto = GetText('GALLEGO') + ' (GL)';
		} else if ($(item[i]).hasClass('translation-fr')) {
			texto = GetText('FRANCES') + ' (FR)';
		}

		if (circulo.hasClass('circulo-verde')) {
			montarTooltip.lanzar($(item[i]), GetText('CV_CONTRADUCCION') + ': ' + texto, 'background-gris-oscuro');
		} else {
			montarTooltip.lanzar($(item[i]), GetText('CV_SINTRADUCCION') + ': ' + texto, 'background-gris-oscuro');
		}
	});
};

function selectionChange(e) {
	let anchor = window.getSelection().anchorNode;
	//En elementos desabilitados no existe el anchorNode;
	if (!anchor) return;
	let elem = anchor.parentNode;
	if (!(elem.classList.contains("visuell-view") || elem.parentNode.classList.contains("visuell-view"))) return;
	let button = elem.tagName == "DIV" ? elem.parentNode.querySelector('.editor-btn') : elem.parentNode.parentNode.querySelector('.editor-btn');
	button.classList.remove('active');
	if (elem.tagName == "B") {
		button.classList.add('active');
	}
}

operativaFormularioProduccionCientifica.modal = function () {
	var that = this;
	this.modal_prod_cientifica.on('hide.bs.modal', function () {
		// Status initial
		//that.modal_prod_cientifica.find('.modal-body > .alert').show();
		//that.formularioPublicacion.show();
		//that.formularioPublicacion.find('.resource .form-check-input').prop('checked', false);
		//that.formularioProyecto.hide();
		that.formularioProyecto.find('> .alert').hide();
		//that.formularioProyecto.find('.btn').removeClass('disabled').attr('data-dismiss', '');
		//that.formularioProyecto.find('.resource .form-check-input').prop('checked', false);
	});
}

operativaFormularioProduccionCientifica.formProyecto = function (section, rdfTypeTab) {
	var that = this;
	that.formularioProyecto.find('> .alert').hide();
	this.formularioProyecto.find('.btn').off('click').on('click', function () {
		$("#modal-enviar-produccion-cientifica .modal-body").scrollTop(0);

		if (that.resourceList.find('.resource .form-check-input').is(':checked')) {
			that.formularioProyecto.find('> .alert').hide();
			$(this).attr('data-dismiss', 'modal');

			if ($('input[name="proyecto"]:checked').length) {
				var idproyecto = [];
				$('input[name="proyecto"]:checked').each(function () {
					idproyecto.push($(this).attr('projectId'));
				});
			}
			var idrecurso = $('.modal-content>.modal-body>.resource-list.listView h2 a').attr("data-id");
			edicionCV.sendPRC(idrecurso, idproyecto, section, rdfTypeTab);

		} else {
			$(this).removeAttr('data-dismiss');
			$("#modal-enviar-produccion-cientifica .modal-body").scrollTop(0);
			that.formularioProyecto.find('> .alert').show();
			$(this).addClass('disabled');
		}
	});

	this.formularioProyecto.find('.alert-title a').off('click').on('click', function () {
		$(this).attr('data-dismiss', 'modal');
		var idrecurso = $('.modal-content>.modal-body>.resource-list.listView h2 a').attr("data-id");
		var idproyecto = [];
		edicionCV.sendPRC(idrecurso, idproyecto, section, rdfTypeTab);
	});

	this.resourceList.find('.resource .form-check-inline').on('change', function () {
		that.formularioProyecto.find('.btn').removeClass('disabled');
		that.formularioProyecto.find('> .alert').hide();
	});
}

function conseguirTesauro(tesaurus, pLang, listadoValoresSeleccionados, ul, edit, mostrarModal, etiquetas, that, data) {
	if (ul.length == 0 && data != null) {
		pintadoEtiquetas(that, data);
	}
	$.ajax({
		url: urlEdicionCV + "GetTesaurus",
		data: {
			"tesaurus": tesaurus,
			"pLang": pLang
		},
		type: "GET",
		success: function (response) {
			var itemsHijo = $.grep(response[0], function (p) { return p.parentId == ''; });
			ul.removeClass('partial');
			ul.empty();
			ul.append(edicionCV.printThesaurusItemsByParent(listadoValoresSeleccionados, response[0], itemsHijo, 0));
			pintadoTesauro(ul, edit, mostrarModal);
			edicionCV.engancharComportamientosCV();
			if (data != null) {
				pintadoEtiquetas(that, data);
			}
		},
		error: function (response) {
		}
	});
}

function pintadoTesauro(elementoActual, edit, mostrarModal) {
	var modalPopUp = elementoActual.closest('.modal-top').attr('id')
	if (modalPopUp == null) {
		return;
	}
	if (modalPopUp == 'modal-editar-entidad') {
		modalPopUp = 'modal-editar-entidad-0'
	} else {
		modalPopUp = 'modal-editar-entidad-' + (parseInt(modalPopUp.substring(21)) + 1);
	}
	modalPopUp = '#' + modalPopUp;
	if (mostrarModal != false) {
		$(modalPopUp).modal('show');
	}

	//IDS
	var contenedor = elementoActual.closest('.entityauxcontainer');
	var idTemp = $(contenedor).attr('idtemp');
	var id = RandomGuid();
	if (edit) {
		id = $('input[name="edicion-listado-' + idTemp + '"]:checked').closest('article').attr('about');
	}
	//Clonamos la entidad auxiliar vacía
	var entityAux = $(contenedor).children('.item.aux.entityaux[about=""]').clone();
	if (edit) {
		entityAux = $(contenedor).children('.item.added.entityaux[about="' + id + '"]').clone();
	}
	if (!edit) {
		//Cambiamos cosas del clon
		entityAux.attr('about', id);
		entityAux.removeClass('aux');
		entityAux.addClass('added');
	}
	//Rellenamos el popup
	$(modalPopUp + ' .formulario-edicion').empty();
	$(modalPopUp + ' .form-actions .ko').remove();
	$(modalPopUp + ' .formulario-edicion').append(entityAux);
	$(modalPopUp).attr('idtemp', idTemp);
	$(modalPopUp).attr('about', id);
	$(modalPopUp).attr('new', 'true');
	if (edit) {
		$(modalPopUp).attr('new', 'false');
		//Reseteamos los campos para mostrar en el listado
		$(modalPopUp + ' .formulario-edicion').children('.entityaux').children('span.title').attr('loaded', 'false');
		$(modalPopUp + ' .formulario-edicion').children('.entityaux').children('span.title').html('');
		$(modalPopUp + ' .formulario-edicion').children('.entityaux').children('span.property').attr('loaded', 'false');
		$(modalPopUp + ' .formulario-edicion').children('.entityaux').children('span.property').html('');
	}
	//Cambiamos los id temporales del clon
	$(entityAux.find('div.entityauxcontainer,div.entitycontainer')).each(function () {
		if (elementoActual.attr('idtemp') != null && $(this).attr('idtemp') != '') {
			elementoActual.attr('idtemp', RandomGuid());
		}
	});

	if ($(modalPopUp + ' .formulario-edicion>div>ul.listadoTesauro').length > 0) {
		$(modalPopUp + ' .formulario-edicion>div>div.custom-form-row').hide();
		$(modalPopUp).addClass('modal-con-buscador');
		$(modalPopUp).addClass('modal-tesauro');
	} else {
		$(modalPopUp + ' .formulario-edicion>div>div.custom-form-row').show();
		$(modalPopUp).removeClass('modal-con-buscador');
		$(modalPopUp).removeClass('modal-tesauro');
	}
}

var mostrarNotificacion = function (tipo, contenido, time) {
	var timeO = 5000;
	if (time != null) {
		timeO = time;
	}
	toastr[tipo](contenido, 'Mensaje de la plataforma', {
		toastClass: 'toast themed',
		positionClass: "toast-bottom-center",
		target: 'body',
		closeHtml: '<span class="material-icons">close</span>',
		showMethod: 'slideDown',
		timeOut: timeO,
		escapeHtml: false,
		closeButton: true,
	});
};

function pintadoEtiquetas(that, data) {
	$.post(urlEdicionCV + 'EnrichmentTopics', data, function (data) {
		// Borrar TAGS y ETIQUETAS enriquecidas precargadas.
		$('div[propertyrdf="http://w3id.org/roh/enrichedKnowledgeArea"].added').remove();
		$('div[propertyrdf="http://w3id.org/roh/enrichedKeywords"].added').remove();

		that.repintarListadoEntity();

		// Lista para comprobar repeticiones.
		var listaEtiquetasCargadas = [];
		var listaCategoriasCargadas = [];

		// Listas para los TAGS
		$('.form-group.full-group.topic.multiple .list-wrap.tags li a span.texto').each(function () {
			listaEtiquetasCargadas.push($(this).text().trim());
		});

		// Lista para las CATEGORIAS
		var listaNombreCat = ["userKnowledgeArea", "enrichedKnowledgeArea", "externalKnowledgeArea"]
		listaNombreCat.forEach(function (item, index) {
			$("#modal-editar-entidad  div.item.entityaux.added[propertyrdf='http://w3id.org/roh/" + item + "']").each(function () {
				var dicNumDesordenado = [];

				$(this).find("input[propertyrdf='http://w3id.org/roh/categoryNode']").each(function () {
					var valActual = $(this).val();
					if (valActual != "") {
						dicNumDesordenado.push({ key: parseInt(valActual.split('_')[1].replaceAll('.', '')), value: valActual });
					}
				});

				// Ordenación
				var idUltimoNivel = "";
				var idInt = 0;
				for (const [key, value] of Object.entries(dicNumDesordenado)) {
					if (value.key > idInt) {
						idInt = value.key;
						idUltimoNivel = value.value;
					}
				}

				listaCategoriasCargadas.push(idUltimoNivel);
			});
		});

		// TAGS
		data.tags.topics.forEach(function (element) {
			if (!listaEtiquetasCargadas.includes(element.word.trim())) {
				var auxKeyword = $('div.entityaux.aux[propertyrdf="http://w3id.org/roh/enrichedKeywords"]');
				var clon = auxKeyword.clone();
				$(clon).removeClass('aux');
				$(clon).addClass('added');
				$(clon).attr('about', RandomGuid());
				$(clon).find('input[propertyrdf="http://w3id.org/roh/title"]').val(element.word.trim());
				$(clon).find('input[propertyrdf="http://w3id.org/roh/score"]').val(element.porcentaje);
				auxKeyword.parent().append(clon);
				listaEtiquetasCargadas.push(element.word);
			}
		});

		// CATEGORIAS
		data.categories.topics.forEach(function (element) {

			var dicNumDesordenado = [];

			element.forEach(function (val) {
				dicNumDesordenado.push({ key: parseInt(val.id.split('_')[1].replaceAll('.', '')), value: val });
			});

			// Ordenación
			var idUltimoNivel = "";
			var idInt = 0;
			for (const [key, value] of Object.entries(dicNumDesordenado)) {
				if (value.key > idInt) {
					idInt = value.key;
					idUltimoNivel = value.value;
				}
			}

			if (!listaCategoriasCargadas.includes(idUltimoNivel.id)) {
				var parentPanel = $('div.item.aux.entityaux[propertyrdf="http://w3id.org/roh/enrichedKnowledgeArea"]').clone();
				var rngId = RandomGuid();

				parentPanel.attr('about', rngId);
				parentPanel.removeClass('aux');
				parentPanel.addClass('added');
				parentPanel.remove('.buscador-coleccion');
				parentPanel.remove('.action-buttons-resultados');
				parentPanel.remove('.listadoTesauro');

				$('div.item.aux.entityaux[propertyrdf="http://w3id.org/roh/enrichedKnowledgeArea"]').parent().append(parentPanel);
				element.forEach(function (id) {

					var idTesauro = id.id;

					var panelTesauro = parentPanel.find('div.custom-form-row div.item.aux').clone();
					$(panelTesauro).removeClass('aux');
					$(panelTesauro).addClass('added');
					$(panelTesauro).find('input').val(idTesauro);
					$(panelTesauro).find('a').removeClass('add');
					$(panelTesauro).find('a').addClass('delete');

					parentPanel.find('div.custom-form-row div.item.aux').parent().append(panelTesauro);

				});

				listaCategoriasCargadas.push(idUltimoNivel);
			}

		});

		that.repintarListadoEntity();
		OcultarUpdateProgress();
	});
}