const uriLoadTaxonomies = "Cluster/GetThesaurus"
const uriSaveCluster = "Cluster/SaveCluster"
const uriSearchTags = "Cluster/SearchTags"
const uriLoadProfiles = "Cluster/LoadProfiles"
const uriLoadSavedProfiles = "Cluster/LoadSavedProfiles"
const uriLoadClst = "Cluster/LoadCluster"

var urlCLT = "";
var urlSC ="";
var urlSTAGS = "";
var urlLoadClst ="";
var urlCargarPerfiles = "";

$(document).ready(function () {
	urlCLT = new URL(url_servicio_externo +  uriLoadTaxonomies);
	urlSC = new URL(url_servicio_externo +  uriSaveCluster);
	urlSTAGS = new URL(url_servicio_externo +  uriSearchTags);
	urlLoadClst = new URL(url_servicio_externo +  uriLoadClst);
	urlCargarPerfiles = new URL(url_servicio_externo +  uriLoadProfiles);
	urlCargarPerfilesGuardados = new URL(url_servicio_externo +  uriLoadSavedProfiles);
});



/**
 * Clase encargada del funcionamiento del creador / editor de los clusters
 */
class StepsCluster {
	/**
	 * Constructor de la clase StepsCluster
	 */
	constructor() {
		var _self = this
		this.step = 1
		this.body = $('body')
		this.dataTaxonomies = null

		// Secciones principales
		this.modalCrearCluster = this.body.find('#wrapper-crear-cluster')
		this.stepProgressWrap = this.modalCrearCluster.find(".step-progress-wrap")
		this.stepsCircle = this.stepProgressWrap.find(".step-progress__circle")
		this.stepsBar = this.stepProgressWrap.find(".step-progress__bar")
		this.stepsText = this.stepProgressWrap.find(".step-progress__text")
		this.modalCrearClusterStep1 = this.modalCrearCluster.find("#wrapper-crear-cluster-step1")
		this.modalCrearClusterStep2 = this.modalCrearCluster.find("#wrapper-crear-cluster-step2")
		this.modalCrearClusterStep3 = this.modalCrearCluster.find("#wrapper-crear-cluster-step3")
		this.clusterAccordionPerfil = this.modalCrearCluster.find("#accordion_cluster")
		this.errorDiv = this.modalCrearCluster.find("#error-modal-cluster")
		this.errorDivStep2 = this.modalCrearCluster.find("#error-modal-cluster-step2")
		this.errorDivStep2Equals = this.modalCrearCluster.find("#error-modal-cluster-step2-equals")
		this.errorDivServer = this.modalCrearCluster.find("#error-modal-server-cluster")
		this.perfilesStep3 = this.modalCrearClusterStep3.find("#perfiles-stp3-result-cluster")

		this.stepContentWrap = this.modalCrearCluster.find(".steps-content-wrap")
		this.stepsContent = this.stepContentWrap.find(".section-steps")

		// Añadir perfil
		this.modalPerfil = this.body.find("#modal-anadir-perfil-cluster")
		this.inputPerfil = this.modalPerfil.find("#input-anadir-perfil")
		this.listadoPerfilesGuardados = this.modalPerfil.find(".modal-apc-listado-saved")
		this.prevSavedCluster = undefined

		// Editar perfil
		this.modalPerfilEditar = this.body.find("#modal-editar-perfil-cluster")
		this.inputPerfilEditar = this.modalPerfilEditar.find("#input-editar-perfil")

		// Areas temáticas Modal
		this.modalAreasTematicas = this.body.find('#modal-seleccionar-area-tematica')
		this.divTesArbol = this.modalAreasTematicas.find('.divTesArbol')
		this.filtroText = this.divTesArbol.find('.filtroRapido')
		this.divTesLista = this.modalAreasTematicas.find('.divTesLista')
		this.divTesListaCaths = undefined
		this.btnSaveAT = this.modalAreasTematicas.find('.btnsave')
		this.cambiosAreasTematicas = 0

		// Información para el guardado 
		this.userId = document.getElementById('inpt_usuarioID').value
		this.clusterId = undefined
		this.data = undefined
		this.editDataSave = undefined
		this.communityShortName = $(this.modalCrearCluster).data('cshortname')
		this.communityUrl = $(this.modalCrearCluster).data('comurl')
		this.communityResourceUrl = this.communityUrl + '/' + $(this.modalCrearCluster).data('urlrecurso')
		this.communityKey = $(this.modalCrearCluster).data('comkey')

		// Tags
		this.topicsM = undefined

		// Textos obtenido de los 'data-'
		this.eliminarText = this.modalCrearCluster.data("eliminartext")
		this.clusterTxt = this.modalCrearCluster.data("clustertxt")
		this.editarClusterText = this.modalCrearCluster.data("editarcluster")
		this.AnadirOtroPerfilText = this.modalCrearCluster.data("addotherprofile")
		this.AnadirNuevoPerfilText = this.modalCrearCluster.data("addnewprofile")
		this.areasTematicasText = this.modalCrearCluster.data("areastematicastext")
		this.descriptoresEspecificosText = this.modalCrearCluster.data("descriptoresespecificostext")

		// Incia las funcionalidades iniciales de modal si este se abre
		// this.modalCrearCluster.on('shown.bs.modal', function () {
		// 	_self.init()
		// });

	}

	/**
	 * Método que inicia el funcionamiento funcionalidades necesarias para el creador de clusters
	 */
	init() {

		var _self = this

		// Fill taxonomies data
		this.getDataTaxonomies().then((data) => {
			_self.fillDataTaxonomies(data);
			_self.dataTaxonomies = data['researcharea'];

			// Check if we need load the cluster (after the taxonomies are loaded)
			var currentUrl = new URL(window.location)
			_self.clusterId = currentUrl.searchParams.get("id")
			if (_self.clusterId) {
				// Load the cluster
				_self.loadCluster()
			}

			// Carga el listado de perfiles guardados previamente
			_self.getDataListClustersSaved().then(res => {
				// Genera el shortId de los cluster y de los perfiles
				res.forEach(e => {e.shortEntityID = e.entityID.substring(37); e.profiles.forEach(p => p.shortEntityID = p.entityID.substring(37))})

				_self.prevSavedCluster = res
				_self.setClustersListOptions(res)
			})


		})


		this.topicsM = new ModalSearchTags()
	}

	/**
	 * Método que carga el clusters indicado e inicializa los datos con los parámetros indicados
	 */
	loadCluster() {
		var _self = this
		this.callLoadCluster().then((res) => {
			this.data = res
			this.editDataSave = res
			var nameInput = document.getElementById('nombreclusterinput')
			var descInput = document.getElementById('txtDescripcion')
			var selectTerms = this.modalCrearCluster.find('#cluster-modal-sec1-tax-wrapper')

			$('h1').text(this.editarClusterText)

			// Fill section 1
			nameInput.value = this.data.name
			descInput.value = this.data.description
			this.saveAreasTematicasEvent(selectTerms, this.data.terms)

			// Fill section 2
			this.data.profiles.forEach(profile => {
				_self.fillProfile(profile);
			})
		});
	}

	/**
	 * Método que rellena un perfil con los datos dados
	 * @param profile, perfil de un cluster
	 */
	fillProfile(profile) {

		let _self = this

		// Fill section 2
		profile.shortEntityID = profile.entityID.split('_')[2]

		// Crea el perfil
		this.addPerfilSearch(profile).then(nameId => {

			// Carga las áreas temáticas
			let selectTerms = $('#modal-seleccionar-area-tematica-' + nameId)

			let timer = setTimeout(function () {
				if (selectTerms.length !== 0)
				{
					_self.saveAreasTematicasEvent(selectTerms, profile.terms)
					clearTimeout(timer)
				}
			}, 100);

			// carga los tags
			let selectTags = $('#modal-seleccionar-tags-' + nameId)
			let timerTerms = setTimeout(function () {
				if (selectTerms.length !== 0)
				{
					_self.saveTAGS(selectTags, profile.tags)
					clearTimeout(timerTerms)
				}
			}, 100);
		})

		// Editamos los IDs de los usuarios
		profile.users.forEach(user => {
			user.shortUserID = user.userID.split('_')[1]
		})
	}

	/**
	 * Método para llamar al servicio externo y cargar los clusters
	 * @return Promise, devuelve una promesa con el resultado de la consulta
	 */
	callLoadCluster() {
		MostrarUpdateProgress();
		urlLoadClst.searchParams.set('pIdClusterId', this.clusterId);
		return new Promise((resolve, reject) => {
			
			$.get(urlLoadClst.toString(), function (res) {
				resolve(res);
				OcultarUpdateProgress();
			});
		})
	}


	/**
	 * Método para llamar al servicio externo y cargar los clusters guardados previamente en otros clusters
	 * @return Promise, devuelve una promesa con el resultado de la consulta
	 */
	getDataListClustersSaved() {
		MostrarUpdateProgress();
		// Pasamos el id del usuario para que los clusters sean del propio usuario
		urlCargarPerfilesGuardados.searchParams.set('pIdUser', this.userId);
		return new Promise((resolve, reject) => {
			
			$.get(urlCargarPerfilesGuardados.toString(), function (res) {
				resolve(res);
				OcultarUpdateProgress();
			});
		})
	}

	/**
	 * Método que crea un listado de opciones con los perfiles guardados previamente
	 * @return Promise, devuelve una promesa con el resultado de la consulta
	 */
	setClustersListOptions(data) {

		let _self = this

		data.forEach(cluster => {
			cluster.profiles.forEach(prof => {
				let htmlProfileList = `
					<div class="categoria-wrap">
					<div class="categoria ${prof.entityID}">
					<div class="custom-control custom-checkbox themed little primary">
					<input type="checkbox" class="custom-control-input at-input" id="${prof.entityID}" data-id="${prof.entityID}" data-clid="${cluster.entityID}" data-name="${prof.name}">
					<label class="custom-control-label" for="${prof.entityID}">${prof.name} <span class="cluster">${_self.clusterTxt}: ${cluster.name}</span></label>
					</div>
					</div>
					</div>
				`
				_self.listadoPerfilesGuardados.append(htmlProfileList)
			})
		})

		_self.listadoPerfilesGuardados.mCustomScrollbar({theme:"minimal-dark"});

		// ... cuando se clique en añadir el perfil
		// fillProfile(profile) 
	}
	
	/**
	 * Método que inicia las comprobaciones para pasar a la siguiente sección
	 * @param pos: Posición a la que se quiere pasar
	 */
	async goStep(pos) {
		var _self = this
		if (pos > 0 && this.step > pos) {

			this.errorDiv.hide()
			this.errorDivStep2.hide()
			this.errorDivStep2Equals.hide()
			this.errorDivServer.hide()
			this.setStep(pos)

		} else if(pos > 0) {

			let continueStep = true
			switch (this.step) {
				case 1:
				continueStep = this.checkContinue1()
				break;
				case 2:
				continueStep = this.checkContinue2()
				if (continueStep) {
					_self.startStep3()
				}
				break;
				case 3:
				try {
					continueStep = await this.saveInit()
					if (continueStep) {
						var urlCom = this.communityResourceUrl+"/"+ this.data.name.replace(/[^a-z0-9_]+/gi, '-').replace(/^-|-$/g, '').toLowerCase() +"/"+ this.clusterId.split('_')[1];
						window.location = urlCom;
					}
				} catch(err) {
					// this.errorDiv.show()
					this.errorDivServer.show()
					window.location.hash = '#' + this.errorDivServer.attr('id')
					continueStep = false;
				}
				break;
			}

			if (continueStep && this.step > (pos - 2)) {
				this.errorDiv.hide()
				this.errorDivStep2.hide()
				this.errorDivStep2Equals.hide()
				this.errorDivServer.hide()
				this.setStep(pos)
			} else {
				if (this.step == 2) {
					this.errorDivStep2.show()
				} else {
					this.errorDiv.show()
				}
				window.location.hash = '#' + this.errorDiv.attr('id')
			}
		}
	}

	/**
	 * Método que obtiene y pinta el número de perfiles en el 'step' de búsquedas de investigadores
	 */
	checkNumberProfiles() {

		let panel = this.clusterAccordionPerfil.find('.panel .panel-heading')

		if (panel.length > 0) {
			this.modalCrearCluster.find("#wrapper-crear-cluster-step2-add-profile").text(this.AnadirOtroPerfilText + ' *')
		} else {
			this.modalCrearCluster.find("#wrapper-crear-cluster-step2-add-profile").text(this.AnadirNuevoPerfilText + ' *')
		}
	}

	/**
	 * Método que borra un perfil
	 * @param head1: Id de la cabecera del collapse
	 * @param head2: Id del contenido del collapse
	 * @param profileId: Id del profile a borrar
	 */
	deletePerfil(head1, head2, profileId) {
		$('#' + head1).remove()
		$('#' + head2).remove()

		// Borrar de los datos
		if (this.data && this.data.profiles) {
			this.data.profiles = this.data.profiles.filter(e => e.entityID != profileId)
		}
		this.checkNumberProfiles()
		// $(item).parent().parent().remove()
	}

	/**
	 * Método que edita un perfil
	 * @param head1: Id de la cabecera del collapse
	 * @param head2: Id del contenido del collapse
	 * @param profileId: Id del profile a borrar
	 */
	editarPerfil(head1, head2, profileId) {
		let _self = this;
		// Texto acstual
		var texto = $('#' + head1).find('.texto').text()
		// Abrimos el popup
		_self.modalPerfilEditar.modal('show')
		// Establecemos el texto por defecto
		_self.inputPerfilEditar.val(texto)
		// Evento de guardar el modal
		_self.modalPerfilEditar.find('.btneditar').off('click').on('click', function() {
			let name = _self.inputPerfilEditar.val()
			
			// Comprueba si el nombre existe
			if (_self.perfilExist(name)) {
				// Muestra un error
				_self.errorDivStep2Equals.show()

			} else {
				_self.errorDivStep2Equals.hide()
				$('#' + head1).find('.texto').text(name)
				$('#' + head2).data('name', name)

				// Esitamos los datos
				if (_self.data && _self.data.profiles) {
					let profile = _self.data.profiles.find(e => e.entityID == profileId)
					profile.name = name
				}
			}

			_self.modalPerfilEditar.modal('hide')
		})

		// this.checkNumberProfiles()
	}

	/**
	 * Método que comprueba que los campos obligatorios de la sección 1 han sido rellenados
	 * También guarda el estado de la sección 1
	 * @return bool: Devuelve true or false dependiendo de si ha pasado la validación
	 */
	checkContinue1() {
		var _self = this

		// Get first screen data
		let name = document.getElementById('nombreclusterinput').value
		let description = document.getElementById('txtDescripcion').value
		let terms = []
		let inputsTermsItms = this.modalCrearClusterStep1.find('#cluster-modal-sec1-tax-wrapper').find('input')
		inputsTermsItms.each((i, e) => {terms.push(e.value)})

		this.data = {
			...this.data,
			entityID: _self.clusterId,
			name,
			description,
			terms
		}

		return (name.length > 0 && terms.length > 0)
	}

	/**
	 * Método que comprueba que al menos hay un perfil con areas temáticas para la sección 2
	 * También guarda el estado de la sección 2
	 * @return bool: Devuelve true or false dependiendo de si ha pasado la validación
	 */
	checkContinue2() {
		var _self = this

		// Get the second screen
		let profiles = this.modalCrearClusterStep2.find('.panel-collapse')
		let profilesObjets = []

		profiles.each((i, e) => {
			let termsSec = $(e).find('.terms-items')
			let inputsTermsProf = termsSec.find('input')
			let profTerms = []
			inputsTermsProf.each((i, el) => {profTerms.push(el.value)})


			let topicsSec = $(e).find('.tags-items')
			let inputsTopicsProf = topicsSec.find('input')
			let profTags = []
			inputsTopicsProf.each((i, e) => {profTags.push(e.value)})

			// Buscar si es un objeto actualizado
			if (this.data.profiles && this.data.profiles.find(prf => prf.shortEntityID == $(e).data('profileid')))
			{
				let profile = this.data.profiles.find(prf => prf.shortEntityID === $(e).data('profileid'))
				profile.terms = profTerms
				profile.tags = profTags
				profilesObjets.push(profile)
			} else {
				profilesObjets.push({
					"name": $(e).data('name'),
					"terms": profTerms,
					"tags": profTags,
					"shortEntityID":$(e).data('profileid'),
					"entityID":$(e).data('profileid')
				})
			}
		})

		// Set the post data
		this.data = {
			...this.data,
			profiles: profilesObjets
		}
		
		let existenPerfiles=this.data.profiles.length > 0;
		let nombresCorrectos=this.data.profiles.every(function (item) {
			return  item.name !=undefined;
		});

		// Comprueba si las etiquetas o las categorías están rellenos
		let categoriasCorrectas=this.data.profiles.every(function (item) {
			return  item.terms!=undefined && item.terms.length>0 || item.tags!=undefined && item.tags.length>0;
		});
		return existenPerfiles && nombresCorrectos && categoriasCorrectas;
	}

	/**
	 * Método que genera la petición get para obtener las taxonomías
	 */
	getDataTaxonomies() {
		
		// https://localhost:44321/Cluster/GetThesaurus?listadoCluster=%5B%22researcharea%22%5D
		let listThesaurus = ["researcharea"];
		urlCLT.searchParams.set('listThesaurus', JSON.stringify(listThesaurus));

		return new Promise((resolve, reject) => {
			$.get(urlCLT.toString(), function (data) {
				resolve(data)
			});
		})
	}

	/**
	 * Inicia la generación del html para las diferentes taxonomías
	 * @param data, Objeto con los items
	 */
	fillDataTaxonomies(data) {
		// Set tree
		let resultHtml = this.fillTaxonomiesTree(data['researcharea']);
		this.divTesArbol.find('.categoria-wrap').remove();
		this.divTesArbol.append(resultHtml);

		// Set list
		/* resultHtml = this.fillTaxonomiesList(data['researcharea'])
		this.divTesLista.append(resultHtml)
		this.divTesListaCaths = this.divTesLista.find(".categoria-wrap") */

		// Open & close event trigger
		let desplegables = this.modalAreasTematicas.find('.boton-desplegar')
	
		if (desplegables.length > 0) {
			desplegables.off('click').on('click', function () {
				$(this).toggleClass('mostrar-hijos');
			});
		}

		// Add events when the items are clicked
		this.itemsClicked();
	}

	/**
	 * Add events when the Taxonomies items are clicked
	 */
	itemsClicked() {

		var _self = this

		// Click into the tree
		this.divTesArbol.off('click').on("click", "input.at-input", function() {
			let dataVal = this.checked
			let dataId = $(this).attr('id')
			let dataParentId = $(this).data('parentid')
			dataParentId = (dataParentId.length > 0) ? dataParentId.split('/').pop() : dataParentId

			// Añadimos un cambio para las areas tematicas
			_self.cambiosAreasTematicas ++
			_self.btnSaveAT.removeClass('disabled')

			if (dataParentId.length > 0) {
				if (!dataVal) {
					let brothers = $(this).parent().parent().parent().parent().find('input.at-input:checked')
					if (brothers.length == 0) {
						_self.selectParent(dataParentId, false)
					} else {
						_self.selectParent(dataParentId)
					}
				} else {
					_self.selectParent(dataParentId)
				}
			}

			
		})

		// Click into the list
		/* this.divTesLista.off('click').on("click","input.at-input", function() {
			let dataVal = this.checked
			let dataId = $(this).attr('id').substring("list__".length)

			if (dataVal) {
				document.getElementById(dataId).checked = true
			} else  {
				document.getElementById(dataId).checked = false
			}
		}) */
	}

	/**
	 * Select the parent in the list and add a class to active the item
	 * @param pId, the parent id
	 * @param addclass, check if should dishabled the item
	 */
	selectParent(pId, addclass = true) {

		let itemP = document.getElementById(pId)
		let $itemP = $(itemP)
		// Check if add or remove class select
		if (addclass) {
			$itemP.addClass('selected')
		} else {
			// Search for childs and remove selected class if not has childs enhabled
			let childs = $itemP.parent().parent().parent().find('input.at-input:checked')
			if (childs.length == 0) {
				$itemP.removeClass('selected')
			} else {
				// Stop the recursive function if it's childs checked
				return null
			}
		}
		// Call to the parent to change the 'selected' class
		let dataParentId = $itemP.data('parentid')
		dataParentId = (dataParentId.length > 0) ? dataParentId.split('/').pop() : dataParentId

		if (dataParentId.length > 0) {this.selectParent(dataParentId, addclass)}
	}

	/**
	 * Crea el html con las taxonomías
	 * @param data, array con los items
	 * @param idParent, id del nodo padre, para generar los hijos
	 * @return string con el texto generado
	 */
	fillTaxonomiesTree(data, idParent = "") {

		var _self = this;

		let resultHtml = "";
		data.filter(e => e.parentId == idParent).forEach(e => {
			let id = e.id.split('/').pop()
			
			let children = _self.fillTaxonomiesTree(data, e.id);

			let disabled = (children != "" && children != undefined) ? 'disabled="disabled"' : ""
			
			resultHtml += '<div class="categoria-wrap">\
					<div class="categoria ' + id + '">\
						<div class="custom-control custom-checkbox themed little primary">\
							<input type="checkbox" class="custom-control-input at-input" id="' + id + '" data-id="' + e.id + '" data-name="' + e.name + '" data-parentid="' + e.parentId + '" ' + disabled +'>\
							<label class="custom-control-label" for="' + id + '">' + e.name + '</label>\
						</div>\
					</div>'


			if (children != "" && children != undefined) {
				resultHtml += '<!--  pintar esto solo cuando tenga hijos -->\
					<div class="boton-desplegar">\
						<span class="material-icons">keyboard_arrow_down</span>\
					</div> \
					<!--  -->'

				resultHtml += '<div class="panHijos">' + children + '</div>'
			}

			resultHtml += '</div>'
		});

		return resultHtml
	}

	/**
	 * Crea el html con las taxonomías en arbol
	 * @param data, array con los items
	 * @return string con el texto generado
	 */
	fillTaxonomiesList(data) {

		var _self = this;

		let resultHtml = "";
		data.forEach(e => {
			let id = e.id.split('/').pop()
			resultHtml += '<div class="categoria-wrap" data-text="' + e.name + '">\
					<div class="categoria list__' + id + '">\
						<div class="custom-control custom-checkbox themed little primary">\
							<input type="checkbox" class="custom-control-input at-input" id="list__' + id + '" data-id="' + e.id + '" data-parentid="' + e.parentId + '" data-name="' + e.name + '">\
							<label class="custom-control-label" for="list__' + id + '">' + e.name + '</label>\
						</div>\
					</div>\
				</div>'
		});

		return resultHtml
	}

	/**
	 * Filtra los items de la lista de categorías
	 * @param input con el texto a filtrar
	 */
	MVCFiltrarListaSelCat(item) {

		// Get the text
		let searchTxt = $(item).val()

		// Set the RegExp
		let matcher = new RegExp(searchTxt, "i");

		// Search the text into the items
		let notFounds = this.divTesListaCaths.each((i, e) => {
			if ($(e).data("text") != null && $(e).data("text") != undefined && $(e).data("text").search(matcher) != -1) {
				$(e).removeClass('d-none');
			} else if($(e).data("text") != null && $(e).data("text") != undefined) {
				$(e).addClass('d-none');
			}
		})

	}

	/**
	 * Método que guarda los 2 pasos iniciales
	 */
	saveInit() {

		var _self = this
		this.data.pIdGnossUser=this.userId;
		
		return new Promise((resolve) => {
			
			$.post(urlSC, this.data)
				.done(
					function (rdata) {
						_self.clusterId = rdata;
						_self.data.entityID=rdata;
						_self.startStep3();
						resolve(true);
					}
				)
				.fail(
					function (xhr, status, error) {
						resolve(false)
					}
				);
		})		

	}

	/** 
	 * Método que realiza la llamada POST
	 * @param url, objeto URL que contiene la url de la petición POST
	 * @param theParams, parámetros para la petición POST
	 */
	postCall(url, theParams) {
		let _self = this
		return new Promise((resolve) => {

			$.ajax({
				url: url.toString(),
				type: "POST",
				dataType: "json",
				crossDomain: true,
				contentType: "application/json; charset=utf-8",
				data: JSON.stringify(theParams),
				traditional: true,
				headers: {
					"accept": "application/json",
					"Access-Control-Allow-Origin":"*"
				},
				success: function(rdata) {
					resolve(rdata)
				},
				failure: function(err) {
					resolve(err);
				}
			});

			// $.post(url.toString(), theParams, function(rdata) {
			// 	resolve(rdata)
			// }).fail(function(err) {
			// 	resolve(err);
			// })
		})
	}

	/**
	 * Carga todas las áreas temáticas seleccionadas para ese perfil / sección 
	 * @param item, sección donde se encuentra la información para cargar las areas temáticas
	 */
	setAreasTematicas(item) {
		let _self = this

		let relItem = $('#' + $(item).data("rel"))

		// Reinicia el filtro del texto de búsqueda
		_self.filtroText.val("");
		MVCFiltrarListaSelCatArbol(_self.filtroText, 'panDesplegableSelCat')

		// Comprueba si hay elementos seleccionados para iniciar la selección
		if (relItem.length > 0) {

			let dataJson = relItem.data('jsondata')

			// Reestablecer las categorías
			this.divTesArbol.find('input.at-input').each((i, e) => {
				e.checked = false
				e.classList.remove('selected')
			})

			// Establece las categorías de la sección
			if (typeof dataJson == "object") {

				dataJson.forEach(e => {
					let item = document.getElementById(e)
					
					if (item) {
						item.checked = true
						// Comprueba si tiene padre para establecerlo con habilitado.
						let parentId = item.getAttribute('data-parentid')
						if (parentId.length > 0) {
							parentId = (parentId.length > 0) ? parentId.split('/').pop() : parentId

							_self.selectParent(parentId)
						}
					} else {
						console.log ("item no existe: #", e)
					}

				})
			}


			// Reestablecemos el botón de guardar las Áreas Temáticas
			this.cambiosAreasTematicas = 0
			this.btnSaveAT.addClass('disabled')

			// Muestra el modal de las áreas temáticas
			this.modalAreasTematicas.modal('show')

			this.saveAreasTematicasEvent(relItem)
		}
	}

	/**
	 * Método que inicia el modal de los tópicos 
	 */
	loadModalTopics(el) {

		let parent = $(el).parent()

		// Establecer los tags de la sección
		this.setTAGS(el)
		// Inicia el funcionamiento de los tags
		this.topicsM.init()

		this.topicsM.closeBtnClick().then((data) => {
			// parent.data('jsondata', data)
			let relItem = $('#' + $(el).data("rel"))
			this.saveTAGS(relItem, data)
		})
	}

	/**
	 * Método que genera el evento para añadir los tags selecciondas en el popup
	 * @param relItem, elemento relacionado para indicar dónde deben de guardarse las areas temátcias seleccionadas
	 */
	saveTAGS(relItem, data) {

		let htmlResWrapper = $('<div class="tag-list mb-4 d-inline"></div>')

		let htmlRes = ''
		let arrayItems = [] // Array para guardar los items que se van a usar
		data.forEach(e => {
			htmlRes += `<div class="tag" title="` + e + `" data-id="` + e + `">
				<div class="tag-wrap">
					<span class="tag-text">` + e + `</span>
					<span class="tag-remove material-icons">close</span>
				</div>
				<input type="hidden" value="` + e + `">
			</div>`
		})

		htmlResWrapper.append(htmlRes)

		relItem.html(htmlResWrapper)

		// Se añade un json para saber qué categorías se han seleccionado
		relItem.data('jsondata', data)

		this.deleteTAGS(relItem)
	}



	/**
	 * Método que elimina las etiquetas del arbol y del listado
	 * @param relItem, contenedor donde se encuentran las áreas temáticas seleccionadas
	 */
	deleteTAGS(relItem) {

		// Selecciona la áreas temáticas seleccionadas dentro de selector
		let tagsItems = relItem.find('.tag-wrap');
		tagsItems.on('click', '.tag-remove', function() {
			// Selecciona el item padre para eliminar
			let itemToDel = $(this).parent().parent()
			let inputShortId = itemToDel.data('id')

			// Remove the current item from array
			let jsonItems = relItem.data('jsondata')
			let myIndex = jsonItems.indexOf(inputShortId)
			if (myIndex !== -1) {
				jsonItems.splice(myIndex, 1)
			}


			// Delete item
			itemToDel.remove()

		})
	}


	/**
	 * Carga todas las áreas temáticas seleccionadas para ese perfil / sección 
	 * @param item, sección donde se encuentra la información para cargar las areas temáticas
	 */
	setTAGS(item) {
		let _self = this

		let relItem = $('#' + $(item).data("rel"))

		if (relItem.length > 0) {

			let dataJson = relItem.data('jsondata')

			// Reestablecer las categorías
			_self.topicsM.removeTags()

			// Establece las categorías de la sección
			if (typeof dataJson == "object") {

				dataJson.forEach(e => {
					_self.topicsM.addTag(e)
				})
			}
		}
	}


	/**
	 * Método que genera el evento para añadir las áreas temáticas selecciondas en el popup
	 * @param relItem, elemento relacionado para indicar dónde deben de guardarse las areas temáticas seleccionadas
	 * @param data, array con las areas temáticas seleccionadas
	 */
	saveAreasTematicasEvent(relItem, data = null) {


		if (data != null) {
			// Entra aquí por primera vez si el cluster ha sido guardado

			let htmlResWrapper = $('<div class="tag-list mb-4 d-inline"></div>')

			let htmlRes = ''
			let dataWithNames = [];

			if (this.dataTaxonomies != null) {
				data.forEach(id => {
					let item = undefined
					if (item = this.dataTaxonomies.find(e => e.id == id)) {
						dataWithNames.push({ id, name: item.name })
					}
				})
			}

			let arrayRes = []
			dataWithNames.forEach(e => {
				htmlRes += '<div class="tag" title="' + e.name + '" data-id="' + e.id.split('/').pop() + '">\
					<div class="tag-wrap">\
						<span class="tag-text">' + e.name + '</span>\
						<span class="tag-remove material-icons">close</span>\
					</div>\
					<input type="hidden" value="' + e.id + '">\
				</div>'
				arrayRes.push(e.id.split('/').pop())
			})

			htmlResWrapper.append(htmlRes)
			relItem.html(htmlResWrapper)

			// Se añade un json para saber qué categorías se han seleccionado
			relItem.data('jsondata', arrayRes)

			this.deleteAreasTematicasEvent(relItem)

		} else {

			// Evento para cuando se seleccione guardar las áreas temáticas desde el popup
			this.btnSaveAT.off('click').on('click', (e) => {
				e.preventDefault()

				// Oculta el modal de las áreas temáticas
				this.modalAreasTematicas.modal('hide')

				// Reestablecemos el botón de guardar las Áreas Temáticas
				this.cambiosAreasTematicas = 0
				this.btnSaveAT.addClass('disabled')

				// Selecciona y establece el contenedor de las areas temáticas
				// let relItem = $('#' + $(item).data("rel"))

				let htmlResWrapper = $('<div class="tag-list mb-4 d-inline"></div>')

				let htmlRes = ''
				let arrayItems = [] // Array para guardar los items que se van a usar
				this.divTesArbol.find('input.at-input').each((i, e) => {
					if (e.checked) {
						htmlRes += '<div class="tag" title="' + $(e).data('name') + '" data-id="' + $(e).data('id') + '">\
							<div class="tag-wrap">\
								<span class="tag-text">' + $(e).data('name') + '</span>\
								<span class="tag-remove material-icons">close</span>\
							</div>\
							<input type="hidden" value="' + $(e).data('id') + '">\
						</div>'
						arrayItems.push(e.id)
					}
				})

				htmlResWrapper.append(htmlRes)

				relItem.html(htmlResWrapper)

				// Se añade un json para saber qué categorías se han seleccionado
				relItem.data('jsondata', arrayItems)

				this.deleteAreasTematicasEvent(relItem)

			})
		}
	}


	/**
	 * Método que elimina las áreas temáticas del arbol y del listado
	 * @param relItem, contenedor donde se encuentran las áreas temáticas seleccionadas
	 */
	deleteAreasTematicasEvent(relItem) {

		// Selecciona la áreas temáticas seleccionadas dentro de selector
		let tagsItems = relItem.find('.tag-wrap');
		tagsItems.on('click', '.tag-remove', function() {
			// Selecciona el item padre para eliminar
			let itemToDel = $(this).parent().parent()
			let inputShortId = itemToDel.data('id')

			if (inputShortId.length > 0) {
				inputShortId = inputShortId.split('/').pop()
			}

			// Set the inputs into the areas temáticas in false
			try {
				document.getElementById(inputShortId).checked = false
				// document.getElementById('list__' + inputShortId).checked = false
			}catch (error) { }

			// Remove the current item from array
			let jsonItems = relItem.data('jsondata')
			let myIndex = jsonItems.indexOf(inputShortId)
			if (myIndex !== -1) {
				jsonItems.splice(myIndex, 1)
			}


			// Delete item
			itemToDel.remove()

		})
	}

	/**
	 * Método que obtiene un perfil por su nombre
	 * @param name, Obtiene un perfil por el nombre
	 */
	perfilExist(name) {
		return this.data && this.data.profiles && this.data.profiles.find(e => e.name == name)
	}


	/**
	 * Método que añade un perfil o varios dependiendo desde dónde se cargue
	 */
	addPerfilSearchBtnEvent() {

		let _self = this

		// Get the name
		name = this.inputPerfil.val()

		if (name == "" && this.prevSavedCluster != undefined) {
			_self.listadoPerfilesGuardados.find("input[selected]")

			let htmlRes = ''
			let arrayItems = [] // Array para guardar los items que se van a usar
			this.listadoPerfilesGuardados.find('input.at-input').each((i, e) => {
				if (e.checked) {
					// deshabilito y coloco y lo pongo como no seleccionado
					e.disabled = true
					e.checked = false

					// Obtengo los ids de los datas para obtener el perfil seleccionado
					let clusterId = e.dataset["clid"]
					let perfilId = e.dataset["id"]
					// Llamo a "fillProfile" para pintar los perfiles seleccionados
					try {
						_self.fillProfile(_self.prevSavedCluster.find(cl => cl.entityID == clusterId).profiles.find(pr => pr.entityID == perfilId))
					} catch (e) { }
					arrayItems.push(perfilId)
				}
			})

		} else {
			this.addPerfilSearch()
		}



	}

	/**
	 * Método que añade un perfil nuevo en el segundo paso
	 * @param name, Nombre opcional para crear un perfil guardado en el cluster que se está cargando
	 * @return string, devuelve un string con el id del profile generado
	 */
	addPerfilSearch(profileObj = null) {


		return new Promise((resolve, reject) => {

			let name = "";
			let profileId = "";

			if (profileObj == null) {
				// Get the name
				name = this.inputPerfil.val()
				profileId = guidGenerator()
			} else {
				name = profileObj.name
				profileId = profileObj.shortEntityID
			}

			this.modalPerfil.modal('hide')
			this.inputPerfil.val("") // Set the name empty

			// Comprueba si el perfil existe y no es la carga inicial cuando se carga un perfil
			if (this.perfilExist(name) && profileObj == null) {
				// Muestra un error
				this.errorDivStep2Equals.show()

			} else {

				this.errorDivStep2Equals.hide()
				// Get the image user url
				let imgUser = this.modalCrearCluster.data('imguser')

				// Set The item id
				let nameId = name.replace(/[^a-z0-9_]+/gi, '-').replace(/^-|-$/g, '').toLowerCase()
				let rand = Math.random() * (100000 - 10000) + 10000
				nameId = nameId + rand.toFixed()

				// Get the panel item
				let panel = this.clusterAccordionPerfil.find('.panel')

				// 
				let item = `<div class="panel-heading" role="tab" id="`+ nameId +`-tab">
						<p class="panel-title">
							<a class="perfil" data-toggle="collapse" data-parent="#accordion_cluster" href="#`+ nameId +`" aria-expanded="true" aria-controls="`+ nameId +`" data-expandable="false">
								<span class="material-icons">keyboard_arrow_down</span>
								<img src="`+ imgUser +`" alt="person">
								<span class="texto">`+ name +`</span>
							</a>
						</p>
						<div class="conteditborrbtn">
							<a href="javascript:void(0)" onclick="stepsCls.editarPerfil('`+ nameId +`-tab', '`+ nameId +`', '`+ profileId +`')" class="btn btn-outline-grey edit">
								<span class="material-icons-outlined px-0">edit</span>
							</a>
							<a href="javascript:void(0)" onclick="stepsCls.deletePerfil('`+ nameId +`-tab', '`+ nameId +`', '`+ profileId +`')" class="btn btn-outline-grey eliminar">
								` + this.eliminarText + `
								<span class="material-icons-outlined">delete</span>
							</a>
						</div>
					</div>
					<div data-profileid="`+profileId+`" id="`+ nameId +`" data-name="`+ name +`" class="panel-collapse collapse show" role="tabpanel" aria-labelledby="`+ nameId +`-tab">
						<div class="panel-body">

							<!-- Areas temáticas -->
							<div class="form-group mb-5 edit-etiquetas terms-items">
								<label class="control-label d-block">`+ this.areasTematicasText +`</label>
								<div class="autocompletar autocompletar-tags form-group" id="modal-seleccionar-area-tematica-`+ nameId +`">
									<div class="tag-list mb-4 d-inline"></div> 
								</div>
								<a class="btn btn-outline-primary" href="javascript: void(0)">
									<span class="material-icons" onclick="stepsCls.setAreasTematicas(this)" data-rel="modal-seleccionar-area-tematica-`+ nameId +`">add</span>
								</a>
							</div>
							<!-- -->

							<!-- Tópicos -->
							<div class="form-group mb-5 edit-etiquetas tags-items">
								<label class="control-label d-block">` + this.descriptoresEspecificosText + `</label>
								<div class="autocompletar autocompletar-tags form-group" id="modal-seleccionar-tags-`+ nameId +`">
									<div class="tag-list mb-4 d-inline"></div> 
								</div>
								<a class="btn btn-outline-primary" href="javascript: void(0)">
									<span class="material-icons" data-rel="modal-seleccionar-tags-`+ nameId +`" onclick="stepsCls.loadModalTopics(this)">add</span>
								</a>
							</div>
							<!-- -->

						</div>
					</div>`

				panel.append(item)

				if (profileObj == null) {
					// Añade un perfil
					if (this.data.profiles) {
						this.data.profiles.push({ name, entityID: profileId })
					} else {
						this.data.profiles = [{ name, entityID: profileId }]
					}
				}

				this.checkNumberProfiles()
				resolve(nameId)
			}

		})

	}


	/**
	 * Establece el "estado" del "step-progress"
	 * @param tstep, posición a establecer
	 */
	setStep(tstep) {
		this.step = tstep

		// Steps Checks
		this.stepsCircle.each((index, step) => {
			$(step).removeClass("done active")
		})

		// Bar step
		this.stepsBar.each((index, step) => {
			$(step).removeClass("active")
		})

		// Title step
		this.stepsText.each((index, step) => {
			$(step).removeClass("current")
		})

		// Content
		this.stepsContent.each((index, step) => {
			$(step).removeClass("show")
		})

		// Add class before
		for (var i = 0; i < (this.step - 1); i++) {
			$(this.stepsCircle[i]).addClass("done active")
			$(this.stepsBar[i]).addClass("active")

		}
		// Add class current
		$(this.stepsCircle[this.step - 1]).addClass("active")
		$(this.stepsContent[this.step - 1]).addClass("show")
		$(this.stepsText[this.step - 1]).addClass("current")
	}

	startStep3() { 
		comportamientoPopupCluster.init(this.data);
		this.PrintPerfilesstp3 ();
		$('#sugeridos-cluster-tab').click();
	}

	/**
	 * Pintar los perfiles "finales"
	 */
	PrintPerfilesstp3 () {

		let imgUser = this.modalCrearCluster.data('imguser')

		let profiles = this.data.profiles.map((e, i) => {
			let idAccordion = (e.name.replace(/[^a-z0-9_]+/gi, '-').replace(/^-|-$/g, '').toLowerCase() + "-" + i);
			let htmlUsers=[];

			// Clases para mostrar o no el listado de usuarios
			let collapseClss1 = ""
			let collapseClss2 = "show"
			if (i > 0) {
				let collapseClss1 = "collapsed"
				let collapseClss2 = ""
			}

			if(e.users!=null)
			{

				htmlUsers=e.users.map((user, nuser) => {
					return `<article class="resource">
                        <div class="wrap">
                            <div class="usuario-wrap">
                                <div class="user-miniatura">
                                    <div class="imagen-usuario-wrap">
                                        <a href="#">
                                            <div class="imagen">
                                                <span style="background-image: url(${imgUser})"></span>
                                            </div>
                                        </a>
                                    </div>
                                    <div class="nombre-usuario-wrap">
                                        <a href="#">
                                            <p class="nombre">${user.name}</p>
                                            <p class="nombre-completo">`+ user.info +`</p>
                                        </a>
                                    </div>
                                </div>
                            </div>
                            <div class="publicaciones-wrap">
                                ${user.numPublicacionesTotal}
                            </div>
                            <div class="principal-wrap">
                                ${user.ipNumber}
                            </div>
                            <div class="acciones-wrap">
                                <ul class="no-list-style">
                                    <li>
                                        <a href="javascript:stepsCls.removeSelectedUserFromProfile('`+e.shortEntityID+`', '`+user.shortUserID+`')" class="texto-gris-claro">
                                            Eliminar
                                            <span class="material-icons-outlined">delete</span>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </article>`;
				});
			}
			let htmlProfile = `<div class="panel-group pmd-accordion" id="accordion`+ idAccordion +`" role="tablist" aria-multiselectable="true">
	            <div class="panel">
	                <div class="panel-heading" role="tab" id="experto-`+ idAccordion +`-tab">
	                    <p class="panel-title">
	                        <a class="perfil `+ collapseClss1 +`" data-toggle="collapse" data-parent="#accordion`+ idAccordion +`" href="#experto-`+ idAccordion +`" aria-expanded="false" aria-controls="experto-`+ idAccordion +`" data-expandable="false">
	                            <span class="material-icons">keyboard_arrow_down</span>
	                            <img src="`+ imgUser +`" alt="image">
	                            <span class="texto">`+ e.name +`</span>
	                        </a>
	                    </p>
	                </div>
	                <div id="experto-`+ idAccordion +`" class="panel-collapse collapse `+ collapseClss2 +`" role="tabpanel" aria-labelledby="experto-`+ idAccordion +`-tab" style="">
	                    <div class="panel-body">

	                        <div class="acciones-listado">
	                            <div class="wrap">
	                                <div class="usuario-wrap"></div>
	                                <div class="publicaciones-wrap">
	                                    Publicaciones
	                                </div>
	                                <div class="principal-wrap">
	                                    Principal
	                                </div>
	                                <div class="acciones-wrap"></div>
	                            </div>
	                        </div>
	                        <div class="resource-list listView resource-list-personas">
	                            <div class="resource-list-wrap">
	                                ${htmlUsers.join('')}
	                            </div>
	                        </div>

	                    </div>
	                </div>
	            </div>
	        </div>
	        `;

			return $(htmlProfile);
		})
		this.perfilesStep3.find('.panel-group.pmd-accordion').remove();
		this.perfilesStep3.append(profiles);
	}

	removeSelectedUserFromProfile(idProfile, idUser) {

		let currentProfile = stepsCls.data.profiles.filter(function (perfilInt) {
			return perfilInt.shortEntityID==idProfile;
		})[0];

		currentProfile.users=currentProfile.users.filter(function (userInt) {
			return userInt.shortUserID!=idUser;
		});

		// Desmarcamos el investigador de la lista de búsqueda
		if (!!document.getElementById(idUser + '-' +idProfile)) {
			document.getElementById(idUser + '-' +idProfile).checked = false
		}

		this.PrintPerfilesstp3();
	}
}


/**
 * Clase para las trabajar en las gráficas de los colaboradores en el cluster
 */
class CargarGraficaProjectoClusterObj {
	dataCB = {};
	dataSE = {};
	idContenedorCB = "";
	idContenedorSE = "";
	typesOcultar = [];
	typesOcultarSE = [];
	showRelation = true;	
	showRelationSE = true;

	actualizarGraficaColaboradores = () => {
		AjustarGraficaArania(this.dataCB, this.idContenedorCB, this.typesOcultar, this.showRelation);
	};
	
	actualizarGraficaSeleccionados = () => {
		AjustarGraficaArania(this.dataSE, this.idContenedorSE, this.typesOcultarSE, this.showRelationSE);
	};

	CargarGraficaColaboradores = (cluster, idContenedor, mostrarCargando = false) => {
		var url = url_servicio_externo + "Cluster/DatosGraficaColaboradoresCluster";
		var self = this;
		$('#' + idContenedor).empty();
		if (mostrarCargando) {
			MostrarUpdateProgress();
		}

		let optionsRelations = ["relation_project", "relation_document"];
		cluster.seleccionados=false;
		$.post(url, cluster, function (data) {
			// Establecer los valores en la variable externa
			self.dataCB = data;
			self.idContenedorCB = idContenedor;

			self.actualizarGraficaColaboradores();
			if (mostrarCargando) {
				OcultarUpdateProgress();
			}
		});
	};
	
	CargarGraficaSeleccionados = (cluster, idContenedor, mostrarCargando = false) => {
		let urlDGCC = url_servicio_externo + "Cluster/DatosGraficaColaboradoresCluster";
		let self = this;
		$('#' + idContenedor).empty();
		if (mostrarCargando) {
			MostrarUpdateProgress();
		}

		let optionsRelations = ["relation_project", "relation_document"];
		cluster.seleccionados=true;
		$.post(urlDGCC, cluster, function (data) {
			// Establecer los valores en la variable externa
			self.dataSE = data;
			self.idContenedorSE = idContenedor;

			self.actualizarGraficaSeleccionados();
			if (mostrarCargando) {
				OcultarUpdateProgress();
			}
		});
	};
};

// Creamos un nuevo objeto
var newGrafProjClust = new CargarGraficaProjectoClusterObj();


// Función a la que se llama para seleccionar qué elementos de las relaciones mostrar
function actualizarTypesClusterOcultar(type) {
	if (type == "relation_todas") {
		newGrafProjClust.typesOcultar = [];
	} else {
		newGrafProjClust.typesOcultar = [type];
	}
	newGrafProjClust.actualizarGraficaColaboradores();
}

// Función a la que se llama para seleccionar qué elementos de las relaciones mostrar en los investigadores seleccionados
function actualizarTypesClusterOcultarSE(type) {
	if (type == "relation_todas") {
		newGrafProjClust.typesOcultarSE = [];
	} else {
		newGrafProjClust.typesOcultarSE = [type];
	}
	newGrafProjClust.actualizarGraficaSeleccionados();
}


// función para actualizar la gráfica de colaboradores
function ActualizarGraficaClusterolaboradoresCluster(typesOcultar = [], showRelation = true) {
	AjustarGraficaArania(dataCB, idContenedorCB, typesOcultar, showRelation);
}

// Objeto para el funcionamiento de los usuarios disponibles para los clusters
var comportamientoPopupCluster = {
	tabActive: null,

	init: function (clusterObj) {
		let that = this
		this.config();
		let paramsCl = this.workCO(clusterObj)
		let paramsProfiles = this.workCOProfiles(clusterObj)
		let profiles = this.setProfiles(clusterObj)

		buscadorPersonalizado.profile=null;
		buscadorPersonalizado.search='searchClusterMixto';
		
		// Iniciar el listado de usuarios
		buscadorPersonalizado.init($('#INVESTIGADORES').val(), "#clusterListUsers", "searchClusterMixto=" + paramsCl, null, "profiles=" + JSON.stringify(profiles) + "|viewmode=cluster|rdf:type=person", $('inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		
		// Agregamos los ordenes
		$('.searcherResults .h1-container').after(
		`<div class="acciones-listado acciones-listado-buscador">
			<div class="wrap">
				
				<div class="ordenar dropdown">
					<a class="dropdown-toggle" data-toggle="dropdown">
						<span class="material-icons">swap_vert</span>
						<span class="texto">${that.text_mixto}</span>
					</a>
					<div class="dropdown-menu basic-dropdown dropdown-menu-right">
						<a href="javascript: void(0)" filter="searchClusterMixto" class="item-dropdown" data-toggle="tooltip" data-placement="top" title="${that.text_mixto_tooltip}">${that.text_mixto}</a>
						<a href="javascript: void(0)" filter="searchClusterVolumen" class="item-dropdown" data-toggle="tooltip" data-placement="top" title="${that.text_volumen_tooltip}">${that.text_volumen}</a>
						<a href="javascript: void(0)" filter="searchClusterAjuste" class="item-dropdown" data-toggle="tooltip" data-placement="top" title="${that.text_ajuste_tooltip}">${that.text_ajuste}</a>
					</div>
				</div>
			</div>
		</div>`);
		
		$('.acciones-listado-buscador a.item-dropdown').unbind().click(function (e) {
			$('.acciones-listado-buscador .dropdown-toggle .texto').text($(this).text())
			e.preventDefault();
			buscadorPersonalizado.search=$(this).attr('filter');
			if(buscadorPersonalizado.profile==null)
			{
				buscadorPersonalizado.filtro=$(this).attr('filter')+'='+paramsCl;
			}else
			{
				buscadorPersonalizado.filtro=$(this).attr('filter')+'='+paramsProfiles[buscadorPersonalizado.profile];
			}
			FiltrarPorFacetas(ObtenerHash2());
		});

		//Enganchamos comportamiento grafica seleccionados
		$('#seleccionados-cluster-tab').unbind().click(function (e) {			
			e.preventDefault();
			newGrafProjClust.CargarGraficaSeleccionados(stepsCls.data, 'selectedgraphCluster', true);
		});

		// Load the tooltips
		$('.searcherResults [data-toggle="tooltip"]').tooltip()

		return;
	},
	config: function () {
		var that = this;

		this.printitem = $('#clusterListUsers')
		this.text_volumen = this.printitem.data('volumen')
		this.text_volumen_tooltip = this.printitem.data('volumentooltip')
		this.text_ajuste = this.printitem.data('ajuste')
		this.text_ajuste_tooltip = this.printitem.data('ajustetooltip')
		this.text_mixto = this.printitem.data('mixto')
		this.text_mixto_tooltip = this.printitem.data('mixtotooltip')

		return;
	},

	/*
	* Convierte el objeto del cluster a los parámetros de consulta 
	*/
	workCO: function (clusterObj) {

		let results = null
		if (clusterObj && clusterObj.profiles) {
			results = clusterObj.profiles.map(e => {
				let terms = (e.terms.length) ? e.terms.map(itm => '<' + itm + '>').join(',') : "<>"
				let tags = (e.tags.length) ? e.tags.map(itm => "'" + itm + "'").join(',') : "''"
				return terms + '@@@' + tags
			}).join('@@@@')
		}

		return results
	},
	
	/*
	* Convierte el objeto del cluster a los parámetros de consulta 
	*/
	workCOProfiles: function (clusterObj) {
		var dicPerfiles = [];
		stepsCls.data.profiles.forEach(function(perfil, index) {
			let terms = (perfil.terms.length) ? perfil.terms.map(itm => '<' + itm + '>').join(',') : "<>"
			let tags = (perfil.tags.length) ? perfil.tags.map(itm => "'" + itm + "'").join(',') : "''"
			dicPerfiles[perfil.shortEntityID]=terms + '@@@' + tags
		});
		return dicPerfiles;
	},

	/*
	* Convierte los profiles en json 
	*/
	setProfiles: function (clusterObj) {

		let results = null
		if (clusterObj && clusterObj.profiles) {
			clusterObj.profiles.forEach((e, i) => {

			})
			results = clusterObj.profiles.map((e, i) => (
				{
					[e.name.replace(/[^a-z0-9_]+/gi, '-').replace(/^-|-$/g, '').toLowerCase() + "-" + i]: e.name
				}
			))
		}

		return results
	}
};

/**
* Clase que contiene la funcionalidad del modal de los TAGS para el Cluster
*/
class ModalSearchTags {
	constructor() {
		this.body = $('body')
		this.modal = this.body.find('#modal-anadir-topicos-cluster')
		this.inputSearch = this.modal.find('#tagsSearchModalCluster')
		this.results = this.modal.find('.ac_results')
		this.resultsUl = this.results.find('ul')
		this.addedTags = []
		this.timeWaitingForUserToType = 750; // Esperar 0.75 segundos a si el usuario ha dejado de escribir para iniciar búsqueda
		this.tagsWrappper = this.modal.find('.tags ul')
		// this.ignoreKeysToBuscador = [37, 38, 39, 40, 46, 8, 32, 91, 17, 18, 20, 36, 18, 27];
		this.ignoreKeysToBuscador = [37, 38, 39, 40, 8, 32, 91, 17, 18, 20, 36, 18, 27];

		// Inicializa la funcionalidad del buscador de TAGS
		this.inputSearchEnter()

		/* if (window.location.hostname == 'depuracion.net' || window.location.hostname.includes("localhost")) {
			var urlSTAGS = new URL(url_servicio_externo + 'servicioexterno/' + uriSearchTags)
		} */
	}

	/**
	 * Inicia la funcionalidad del modal
	 */
	init() {
		this.modal.modal('show') 
	}

	/**
	 * Funcionalidad para cuando se introduce un valor en el buscador
	 */
	inputSearchEnter() {
		let _self = this
		this.inputSearch.off('keyup').on('keyup', function(e) {
			let inputVal = this.value

			if (inputVal.length <= 1) 
			{
				_self.hiddenResults()
			}
			else {
				// Valida si el valor introducirdo en el input es 'válido'
				if (_self.validarKeyPulsada(e) == true) {
					// Limpia el 'time' para reinicializar la espera para iniciar la búsqueda de TAGS
					clearTimeout(_self.timer)
					// Espera 0.5s para realizar la búsqueda de TAGS
					_self.timer = setTimeout(function () {
						_self.hiddenResults()

						_self.searchCall(inputVal).then((data) => {
							// Muestra el selector 
							_self.results.show()
							if (data.length > 0) {
								// Pinta los resultados
								let resultHTML = data.map(e => {
									return '<li class="ac_even">' + e + '</li>'
								})
								_self.resultsUl.html(resultHTML.join(''))
								_self.addTagClick()

							} else {
								_self.mostrarPanelSinResultados()
							}
							
						})

					}, _self.timeWaitingForUserToType);
				}               
			}
		})
	}

	/**
	 * Oculta y borra los resultados del API
	 */
	hiddenResults() {
		this.resultsUl.html('')
		this.results.hide()
	}

	/**
	 * Muestra un texto "sin resultados" cuando no hay resultados del API
	 */
	mostrarPanelSinResultados() {
		this.resultsUl.html('<li>Sin resultados</li>')
	}

	/**
	 * Método que genera un inicia el evento de añadir el tag (al hacer 'click') cuando se 
	 * ha seleccionado un elemento de la lista de opciones disponibles 
	 */
	addTagClick() {
		let _self = this
		// Evento para el click
		this.resultsUl.off('click').on('click', 'li', function(e) {
			let texto = $(this).text()

			if (texto != "Sin resultados") {
				_self.addTag(texto) // Añade el texto
				_self.hiddenResults() // Vacía los resultados de la búsqueda
				_self.inputSearch.val('') // Vacía el campo de búsqueda
			}

		})
		// Delete list item  
	}

	/**
	 * Añade un tag al modal
	 * @param texto, texto del tag a añadir
	 */
	addTag(texto) {
		var _self = this
		let item = $(`<li>
						<a href="javascript: void(0);">
							<span class="texto">` + texto + `</span>
						</a>
						<span class="material-icons cerrar" data-texto="` + texto + `">close</span>
					</li>`)
		this.tagsWrappper.append(item)
		this.addedTags.push(texto)
		
		item.off('click').on('click', '.cerrar', function(e) {
			let texto = $(this).data('texto')
			let li = $(this).parent().remove()
			_self.addedTags = _self.addedTags.filter(txt => txt != texto)
		})
	}

	/**
	 * Remove all tags html
	 */
	removeTags() {
		this.tagsWrappper.html('')
	}

	/**
	 * Método que genera un evento para el botón "guardar" y devuelve el número de TAGS añadidas
	 * @return promise (array) con la lista de resultados 
	 */
	closeBtnClick() {
		let _self = this
		return new Promise((resolve, reject) => {

			this.modal.find('.btnclosemodal').off('click').on('click', function(e) {
			
				let result = new Array()
				_self.tagsWrappper.find('li .texto').each((i, item) => {
					result.push(item.innerText)
				})
				resolve(result)
			})
		})
	}


	/**
	 * Comprobará la tecla pulsada, y si no se encuentra entre las excluidas, dará lo introducido por válido devolviendo true
	 * Si se pulsa una tecla de las excluidas, devolverá false y por lo tanto el metabuscador no debería iniciarse
	 * @param {any} event: Evento o tecla pulsada en el teclado
	 * @return bool, devuelve si la tecla pulsada es válida o no
	 */
	validarKeyPulsada (event) {
		const keyPressed = this.ignoreKeysToBuscador.find(key => key == event.keyCode);
		if (keyPressed) {
			return false
		}
		return true;
	}

	/**
	 * Realiza la petición ajax (GET) para buscar los tags sugeridos en el input
	 * @param {string} inputVal: Texto para la búsqueda
	 * @return promise (array) con la lista de resultados 
	 */
	searchCall (inputVal) {
		var _self = this
		// Set the url parameters
		urlSTAGS.searchParams.set('tagInput', inputVal)

		return new Promise((resolve) => {
			$.get(urlSTAGS.toString(), function (data) {
				resolve(data.filter(itm => !_self.addedTags.includes(itm)))
			});
		})
	}
}

// función que se llama cuando se cargan los investigadores en el cluster
function CompletadaCargaRecursosCluster()
{	
	if(typeof stepsCls != 'undefined' && stepsCls!=null && stepsCls.data!=null)
	{		
		$('#clusterListUsers article.investigador h2.resource-title').attr('tagert','_blank');
		stepsCls.data.pPersons=$('#clusterListUsers article.investigador').toArray().map(e => {return $(e).attr('id')});
		
		newGrafProjClust.CargarGraficaColaboradores(stepsCls.data, 'colaboratorsgraphCluster', true);
		
		$.post(urlCargarPerfiles, stepsCls.data, function(data) {
			$('article.investigador .user-perfil').remove();
			for (const [idperson, datospersona] of Object.entries(data)) {
				let htmlPerfiles='';				
				for (const [idProfile, score] of Object.entries(datospersona)) {
					if(score.numPublicaciones>0)
					{
						let idProfileEdit = idProfile;
						if(idProfileEdit.length!=36)
						{
							idProfileEdit=idProfileEdit.split('_')[2]
						}
						let nombrePerfil = stepsCls.data.profiles.filter(function (item) {return item.shortEntityID ==idProfileEdit || item.entityID ==idProfileEdit;})[0].name;
						
						let publicationsPercent = score.numPublicaciones/score.numPublicacionesTotal*100


      					// Print the terms
      					let termsHtml = ""
      					if (score.terms) {
	      					for (const [termId, count] of Object.entries(score.terms))
	      					{
	      						let titem = stepsCls.dataTaxonomies.find(e => e.id == termId)

	      						if (titem) {
		      						termsHtml += `
		      							<li>
		                                    ${titem.name}
		                                    <span class="numResultados">${count}</span>
		                                </li>
									`
	      						}
	      					}
      					}

      					// Print the tags
      					let tagsHtml = ""
      					if (score.tags) {
	      					for (const [tag, count] of Object.entries(score.tags))
	      					{
	      						tagsHtml += `
	      							<li>
	                                    ${tag}
	                                    <span class="numResultados">${count}</span>
	                                </li>
								`
	      					}
	      				}

						htmlPerfiles+=`	<div class="perfil-wrap">
								        <div class="custom-wrap">
								            <div class="custom-control custom-checkbox">
								                <input type="checkbox" class="custom-control-input" id="${idperson}-${idProfileEdit}">
								                <label class="custom-control-label" for="${idperson}-${idProfileEdit}">
								                    ${nombrePerfil}
								                </label>
								            </div>
								            <div class="check-actions-wrap">
								                <a href="javascript: void(0);" class="dropdown-toggle check-actions-toggle" data-toggle="dropdown" aria-expanded="true">
								                    <span class="material-icons">
								                        arrow_drop_down
								                    </span>
								                </a>
								                <div class="dropdown-menu basic-dropdown check-actions" id="checkActions" x-placement="bottom-start">
								                    <div class="barras-progreso-wrapper">
								                        <div class="progreso-wrapper" data-toggle="tooltip" data-placement="top" title="${comportamientoPopupCluster.text_ajuste_tooltip}">
								                            <div class="progress">
								                                <div class="progress-bar background-success" role="progressbar" style="width: ${score.ajuste * 100}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
								                            </div>
								                            <span class="progress-text"><span class="font-weight-bold">${Math.round(score.ajuste * 10000)/100}%</span></span>
								                        </div>
								                        <div class="progreso-wrapper" data-toggle="tooltip" data-placement="top" title="${comportamientoPopupCluster.text_volumen_tooltip}">
								                            <div class="progress">
								                                <div class="progress-bar" role="progressbar" style="width: ${publicationsPercent}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
								                            </div>
								                            <span class="progress-text"><span class="font-weight-bold">${score.numPublicaciones} /</span> ${score.numPublicacionesTotal}</span>
								                        </div>
								                    </div>
								                    <div class="wrap">
								                        <div class="header-wrap">
								                            <p>Areas temáticas</p>
								                            <p>Publicaciones</p>
								                        </div>
								                        <div class="areas-tematicas-wrap">
								                            <ul class="no-list-style">
								                                ${termsHtml}
								                            </ul>
								                        </div>
								                    </div>
								                    <div class="wrap">
								                        <div class="header-wrap">
								                            <p>Descriptores</p>
								                        </div>
								                        <div class="descriptores-wrap">
								                            <ul class="no-list-style">
								                                ${tagsHtml}
								                            </ul>
								                        </div>
								                    </div>
								                </div>
								            </div>
								        </div>
								        <div class="barras-progreso-wrap">
								            <div class="progreso-wrapper" data-toggle="tooltip" data-placement="top" title="${comportamientoPopupCluster.text_ajuste_tooltip}">
								                <div class="progress">
								                    <div class="progress-bar background-success" role="progressbar" style="width: ${score.ajuste * 100}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
								                </div>
								                <span class="progress-text"><span class="font-weight-bold">${Math.round(score.ajuste * 10000)/100}%</span></span>
								            </div>
								            <div class="progreso-wrapper" data-toggle="tooltip" data-placement="top" title="${comportamientoPopupCluster.text_volumen_tooltip}">
								                <div class="progress">
								                    <div class="progress-bar" role="progressbar" style="width: ${publicationsPercent}%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
								                </div>
								                <span class="progress-text"><span class="font-weight-bold">${score.numPublicaciones} /</span> ${score.numPublicacionesTotal}</span>
								            </div>
								        </div>
								    </div>`;

					}
				}
				let htmlPerfilesPersona=`	<div class="user-perfil pl-0">
												${htmlPerfiles}
											</div>`;				
				$('#'+idperson+' .content-wrap.flex-column').append(htmlPerfilesPersona);
				try {
					$('#'+idperson).data('numPublicacionesTotal', Object.values(datospersona)[0].numPublicacionesTotal);
					$('#'+idperson).data('ipNumber', Object.values(datospersona)[0].ipNumber);
				} catch (e) { }
			}

			let repintar = false
			//Marcamos como checkeados los correspondientes
			stepsCls.data.profiles.forEach(function(perfil, index) {
				let idProfile= perfil.shortEntityID;
				if(perfil.users!=null)
				{
					perfil.users.forEach(function(user, index) {
						var elementUser = $('#'+user.shortUserID)
						$('#'+user.shortUserID+'-'+idProfile).prop('checked', true);
					});					
				}
			});
			if (repintar) {
				stepsCls.PrintPerfilesstp3();
			}
			
			//Enganchamos el chek de los chekbox	
			$('.perfil-wrap .custom-control-input').change(function() {
				let id=$(this).attr('id');
				let idUser=id.substring(0,36);
				let idProfile=id.substring(37);					
				let perfil=stepsCls.data.profiles.filter(function (perfilInt) {
					return perfilInt.shortEntityID==idProfile || perfilInt.entityID==idProfile ;
				})[0];
				if(this.checked) {
					let elementUser = $(this).closest('.resource.investigador')
					let user = {}
					let arrInfo = []
					user.shortUserID = idUser
					user.name = elementUser.find('h2.resource-title').text().trim()

					// Obtener la descripción
					elementUser.find('.middle-wrap > .content-wrap > .list-wrap li').each((i, elem) => {
						arrInfo.push($(elem).text().trim())
					})
					user.info = arrInfo.join(', ')

					user.numPublicacionesTotal = elementUser.data('numPublicacionesTotal')
					user.ipNumber = elementUser.data('ipNumber')
					if(perfil.users==null)					
					{
						perfil.users=[];
					}
					perfil.users.push(user);
				}else
				{
					perfil.users=perfil.users.filter(function (userInt) {
						return userInt.shortUserID!=idUser;
					});
				}
				stepsCls.PrintPerfilesstp3();
				newGrafProjClust.CargarGraficaColaboradores(stepsCls.data, 'colaboratorsgraphCluster', true);
			});	

			// Habilitamos los tooltips
			$('.searcherResults [data-toggle="tooltip"]').tooltip()
			
		});
	}
}


// Función que se llama cuando se cargan las facetas de los investigadores en el cluster
function CompletadaCargaFacetasCluster()
{	
	if(typeof stepsCls != 'undefined' && stepsCls!=null && stepsCls.data!=null)
	{
		if($('#out_roh_profiles').length==0)
		{
			//Creamos la faceta
			let liPerfiles='';
			stepsCls.data.profiles.forEach(function(perfil, index) {
				let idProfile= perfil.shortEntityID;
				let nameProfile= perfil.name;
				let style="";
				if(buscadorPersonalizado.profile==idProfile)
				{
					style='style="font-weight: 700!important;"';
				}
				liPerfiles+=`<li>
						<a rel="nofollow" ${style} href="javascript: void(0);" class="facetaperfil con-subfaceta ocultarSubFaceta" profile="${idProfile}">
							<span class="textoFaceta">${nameProfile}</span>
						</a>
					</li>`;
			});
			let htmlPerfiles=`<div id="out_roh_profiles">
					<div id="in_roh_profiles">
						<div class="box " id="roh_profiles">
							<span class="faceta-title">TODO perfiles</span>
							<ul class="listadoFacetas">
								${liPerfiles}
							</ul>
						</div>
					</div>
				</div>`;
			$('#facetedSearch').prepend(htmlPerfiles);
			//Añadimos el comporamiento a la faceta
			$('.facetedSearch a.facetaperfil')
				.unbind()
				.click(function (e) {	
					if(buscadorPersonalizado.profile==$(this).attr('profile'))
					{
						buscadorPersonalizado.profile=null;
						buscadorPersonalizado.filtro=buscadorPersonalizado.search+'='+comportamientoPopupCluster.workCO(stepsCls.data);
					}
					else
					{
						buscadorPersonalizado.profile=$(this).attr('profile');
						buscadorPersonalizado.filtro=buscadorPersonalizado.search+'='+comportamientoPopupCluster.workCOProfiles(stepsCls.data)[buscadorPersonalizado.profile];
					}
					FiltrarPorFacetas(ObtenerHash2());
				});
		}
		if(buscadorPersonalizado.profile!=null)
		{
			if($('#panFiltros li.profile').length==0)
			{
				let liPerfil='';
				stepsCls.data.profiles.forEach(function(perfil, index) {
					let idProfile= perfil.shortEntityID;
					if(buscadorPersonalizado.profile==idProfile)
					{
						liPerfil+=`<li class="profile">
										${perfil.name}
										<a rel="nofollow" class="remove facetaprofile" profile="idProfile" href="javascript: void(0);">eliminar</a>
									</li>`;
					}					
				});
				$('#panFiltros ul').prepend(liPerfil);
				//Añadimos el comporamiento a la faceta
				$('#panFiltros .profile a').unbind().click(function (e) {	
					buscadorPersonalizado.profile=null;
					buscadorPersonalizado.filtro=buscadorPersonalizado.search+'='+comportamientoPopupCluster.workCO(stepsCls.data);
					FiltrarPorFacetas(ObtenerHash2());
				});
			}
		}
	}
}
