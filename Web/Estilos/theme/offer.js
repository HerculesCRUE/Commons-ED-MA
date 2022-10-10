const uriSaveOffer = "Ofertas/SaveOffer"
const uriLoadOffer = "Ofertas/LoadOffer"
const uriLoadUsersGroup = "Ofertas/LoadUsersGroup"
// const uriSearchTags = "Cluster/SearchTags"
const uriLoadTaxonomiesOffer = "Ofertas/GetThesaurus"
// STEP 3
const uriLoadLineResearchs = "Ofertas/LoadLineResearchs"
const uriLoadFramingSectors = "Ofertas/LoadFramingSectors"
const uriLoadMatureStates = "Ofertas/LoadMatureStates"

// Constantes para el listado de las ofertas
const urlCambioEstado = url_servicio_externo + "Ofertas/CambiarEstado"
const urlCambioEstadoAll = url_servicio_externo + "Ofertas/CambiarEstadoAll"
const urlBorrarOferta = url_servicio_externo + "Ofertas/BorrarOferta"





var urlLT = "";
var urlSOff ="";
var urlSTAGSOffer = "";
var urlLoadUsersGroup ="";

var urlLoadLineResearchs ="";
var urlLoadFramingSectors = "";
var urlLoadMatureStates ="";

/**
 * Crea las urls para las llamadas ajax
 */
$(document).ready(function () {
	servicioExternoBaseUrl=$('#inpt_baseURLContent').val()+'/servicioexterno/';
	urlLT = new URL(url_servicio_externo +  uriLoadTaxonomiesOffer);
	urlSOff = new URL(servicioExternoBaseUrl +  uriSaveOffer);
	urlSTAGSOffer = new URL(servicioExternoBaseUrl +  uriSearchTags);
	urlLoadOffer = new URL(servicioExternoBaseUrl +  uriLoadOffer);
	urlLoadUsersGroup = new URL(servicioExternoBaseUrl +  uriLoadUsersGroup);

	urlLoadLineResearchs = new URL(servicioExternoBaseUrl +  uriLoadLineResearchs);
	urlLoadFramingSectors = new URL(servicioExternoBaseUrl +  uriLoadFramingSectors);
	urlLoadMatureStates = new URL(servicioExternoBaseUrl +  uriLoadMatureStates);
});



/**
 * Constructor de la clase StepsOffer, se encargará de la funcionalidad principal del creador de ofertas tecnológicas, 
 * controlando los 'steps' y todo el proceso de validación, llamadas ajax al servicio externo, guardado y carga de las
 * ofertas tecnológicas, etc...
 */
class StepsOffer {
	/**
	 * Constructor de la clase StepsOffer
	 */
	constructor() {
		var _self = this
		this.body = $('body')

		// Secciones principales
		this.crearOferta = this.body.find('#wrapper-crear-oferta')
		this.stepProgressWrap = this.crearOferta.find(".step-progress-wrap")
		this.stepsCircle = this.stepProgressWrap.find(".step-progress__circle")
		this.stepsBar = this.stepProgressWrap.find(".step-progress__bar")
		this.stepsText = this.stepProgressWrap.find(".step-progress__text")
		this.crearOfertaStep1 = this.crearOferta.find("#wrapper-crear-oferta-step1")
		this.crearOfertaStep2 = this.crearOferta.find("#wrapper-crear-oferta-step2")
		this.crearOfertaStep3 = this.crearOferta.find("#wrapper-crear-oferta-step3")
		this.crearOfertaStep4 = this.crearOferta.find("#wrapper-crear-oferta-step4")
		this.crearOfertaStep5 = this.crearOferta.find("#wrapper-crear-oferta-step5")
		this.ofertaAccordionPerfil = this.crearOferta.find("#accordion_oferta")
		this.errorDiv = this.crearOferta.find("#error-modal-oferta")
		this.errorDivStep2 = this.crearOferta.find("#error-modal-oferta-step2")
		this.errorDivStep2Equals = this.crearOferta.find("#error-modal-oferta-step2-equals")
		this.errorDivServer = this.crearOferta.find("#error-modal-server-oferta")
		this.researchersStep2 = this.crearOfertaStep2.find("#researchers-stp2-result-oferta")

		this.stepContentWrap = this.crearOferta.find(".steps-content-wrap")
		this.stepsContent = this.stepContentWrap.find(".section-steps")

		// Buttons
		this.botoneraSteps = this.crearOferta.find('.botonera')
		this.btnBefore = this.botoneraSteps.find('.beforeStep')
		this.nextStep = this.botoneraSteps.find('.nextStep')
		this.endStep = this.botoneraSteps.find('.endStep')

		// Step 1
		this.listInvestigadoresSTP1 = this.crearOfertaStep1.find(".resource-list-investigadores > div")
		this.researchers = undefined
		this.numSeleccionadosInvestigadores = 0


		// STEP 3
		this.ddlMadurez = this.crearOfertaStep3.find("#ddlMadurez2")
		this.ddlEncuadre = this.crearOfertaStep3.find("#ddlEncuadre2")
		this.seleccionLineasInvestigacion = this.crearOfertaStep3.find("#modal-seleccionar-area-tematica-linv")
		this.maturesStates = undefined
		this.framingsectors = undefined


		// Modal(es) dinámicos
		this.modalesDinamicos = {
			"sectoraplicacion": {
				item: "sectorAplicacionData",
				field: "sectorAplicacion",
				rel: "modal-seleccionar-sector-aplicacion-1",
				modal: undefined,
				cambios: 0,
				obligatorio: true,
			},
			"areaprocedencia": {
				item: "areaProcedenciaData",
				field: "areaProcedencia",
				rel: "modal-seleccionar-areas-procedencia-1",
				modal: undefined,
				cambios: 0,
				obligatorio: true,
			},
		}
		this.dataModalesDinamicos = undefined


		// STEP 4

		// Ids de los campos de la sección 4
		this.listIdsTextsRequired = [
			"descripcion",
			"aplicaciones",
			"destinatarios",
			"ventajasBeneficios",
		]
		this.listIdsTextsOptional = [
			"origen",
			"innovacion",
			"socios",
			"colaboracion",
			"observaciones",
		]
		this.objectsTextEditor = {
			"descripcion": null,
			"aplicaciones": null,
			"destinatarios": null,
			"ventajasBeneficios": null,
			"origen": null,
			"innovacion": null,
			"socios": null,
			"colaboracion": null,
			"observaciones": null,
		}
		

		// STEP 5
		this.ProyectosSelectedStep5 = this.crearOfertaStep5.find("#proyectos-stp5-result-oferta")
		this.DocumentsSelectedStep5 = this.crearOfertaStep5.find("#publicaciones-stp5-result-oferta")
		this.PIISelectedStep5 = this.crearOfertaStep5.find("#pii-stp5-result-oferta")
		this.numSeleccionadosProjects = 0
		this.numSeleccionadosDocuments = 0
		this.numSeleccionadosPII = 0
		

		// Añadir perfil
		this.modalPerfil = this.body.find("#modal-anadir-perfil-oferta")
		this.inputPerfil = this.modalPerfil.find("#input-anadir-perfil")

		// Editar perfil
		this.modalPerfilEditar = this.body.find("#modal-editar-perfil-oferta")
		this.inputPerfilEditar = this.modalPerfilEditar.find("#input-editar-perfil")

		// Areas temáticas Modal
		this.modalAreasTematicas = this.body.find('#modal-seleccionar-area-tematica')
		this.divTesArbol = this.modalAreasTematicas.find('.divTesArbol')
		this.divTesLista = this.modalAreasTematicas.find('.divTesLista')
		this.divTesListaCats = undefined
		this.btnSaveAT = this.modalAreasTematicas.find('.btnsave')
		this.cambiosAreasTematicas = 0

		// Steps info
		this.step = 1
		this.numSteps = this.stepsCircle.length

		// Información para el guardado 
		this.userId = document.getElementById('inpt_usuarioID').value
		this.ofertaId = undefined
		this.data = {
			researchers: {},
			groups: [],
			projects: {},
			documents: {},
			pii: {},
		}
		this.editDataSave = undefined
		this.communityShortName = $(this.crearOferta).data('cshortname')
		this.communityUrl = $(this.crearOferta).data('comurl')
		this.communityResourceUrl = this.communityUrl + '/' + $(this.crearOferta).data('urlrecurso')
		this.communityKey = $(this.crearOferta).data('comkey')
		this.currentLang = document.getElementById('inpt_Idioma').value

		// Tags
		this.topicsM = undefined

		// Textos obtenido de los 'data-'
		this.eliminarText = this.crearOferta.data("eliminartext")
		this.editarOfertaText = this.crearOferta.data("editaroferta")
		this.AnadirOtroPerfilText = this.crearOferta.data("addotherprofile")
		this.AnadirNuevoPerfilText = this.crearOferta.data("addnewprofile")
		this.lineasInvestigacionText = this.crearOferta.data("lineasinvestigaciontext")
		this.descriptoresEspecificosText = this.crearOferta.data("descriptoresespecificostext")
		this.txtProyectos = this.crearOferta.data("proyectos")
		this.txtPublicaciones = this.crearOferta.data("publicaciones")
		this.txtPii = this.crearOferta.data("pii")
		this.sinEspcificarText = this.crearOferta.data("sinespecificar")
	}

	/**
	 * Método que inicia el funcionamiento funcionalidades necesarias para el creador 
	 * de ofertas
	 */
	init() {

		var _self = this


		// Check if we need load the offer
		var currentUrl = new URL(window.location)
		_self.ofertaId = currentUrl.searchParams.get("id")


		// Fill taxonomies data

		MostrarUpdateProgress();
		this.getDataTaxonomies().then((data) => {

			_self.dataModalesDinamicos = data;
			_self.fillDataTaxonomies(data);


			if (_self.ofertaId) {
				// Load the pffer
				_self.loadOffer().then((res) => {



					// Carga los usuarios del grupo al que perteneces 
					_self.LoadUsersGroup().then(() => {
						OcultarUpdateProgress()
					})
				
				})
			} else {

				// Carga los usuarios del grupo al que perteneces 
				_self.LoadUsersGroup().then(() => {
					OcultarUpdateProgress()
				})
			}
		})


		this.topicsM = new ModalSearchTagsOffer()
	}

	/**
	 * Método que carga el ofertas indicado e inicializa los datos con los parámetros 
	 * indicados
	 */
	loadOffer() {
		var _self = this
		return new Promise((resolve, reject) => {
			this.callLoadOffer().then((res) => {
				_self.data = res
				_self.ofertaId = res.entityID
				_self.editDataSave = res
				var nameInput = document.getElementById('nombreofertainput')
				// var descInput = document.getElementById('txtDescripcion')
				// var selectTerms = this.crearOferta.find('#oferta-modal-sec1-tax-wrapper')

				$('h1').text(_self.editarOfertaText)

				// Fill section 1
				nameInput.value = _self.data.name
				// descInput.value = this.data.description
				// this.saveAreasTematicasEvent(selectTerms, this.data.tags)



				// Cargar datos de los modales dinámicos 
				Object.keys(_self.modalesDinamicos).forEach(idDyn => {
					let tmp = undefined
					try {
						if (tmp = _self.data[_self.modalesDinamicos[idDyn].field]) {
							_self.modalesDinamicos[idDyn].modal.saveAreasTematicasEvent($('#' + _self.modalesDinamicos[idDyn].rel), tmp)
						}
					} catch (error) {}
				})


				// CARGAR TAGS
				let selectTags = $('#oferta-modal-seleccionar-tags-stp1')
				
				if (selectTags.length !== 0)
				{
					_self.saveTAGS(selectTags, _self.data.tags)
				}

				resolve(true)
			});
		});
	}

	/**
	 * Método que realiza una llamada ajax para cargar la oferta tecnológica
	 */
	callLoadOffer() {
		MostrarUpdateProgress();
		urlLoadOffer.searchParams.set('pIdOfertaId', this.ofertaId);
		return new Promise((resolve, reject) => {
			
			$.get(urlLoadOffer.toString(), function (res) {
				resolve(res);
				OcultarUpdateProgress();
			});
		})
	}

	/**
	 * Método que carga el personal investigador del grupo al que pertenece el usuario
	 * que está creando la oferta tecnológica actualmente
	 */
	LoadUsersGroup() {
		var _self = this
		// MostrarUpdateProgress();
		return new Promise((resolve, reject) => {
			_self.callLoadUsersGroup().then((res) => {

				if (res && Object.keys(res).length > 0) {

					_self.researchers = res;

					let imgUser = _self.crearOferta.data('imguser')
					let resHtml = ""
					let i = 0
					for (const [idperson, datospersona] of Object.entries(res)) {

						// Comprueba (si en una carga o se ha cargado en el siguiente paso) si el usuario está cargado
						let selectedClass = ""
						let selector = ""

						if (_self.data.researchers && _self.data.researchers[idperson]) {
							selectedClass = "seleccionado"
							selector = `
							<div class="custom-control custom-checkbox-resource done">
								<span class="material-icons">done</span>
							</div>`
						} else {
							selector = `
							<div class="custom-control custom-checkbox-resource add">
								<span class="material-icons">add</span>
							</div>`
						}


						resHtml += `
							<article class="resource ${selectedClass}" id="stp1-res-${idperson}" data-id="${idperson}">
					            ${selector}
					            <div class="wrap">
					                <div class="usuario-wrap">
					                    <div class="user-miniatura">
					                        <div class="imagen-usuario-wrap">
					                            <a href="#">
					                                <div class="imagen con-material-icons">
					                                    <!-- <span style="background-image: url(${imgUser})"></span> -->
    													<span class="material-icons">person</span>
					                                </div>
					                            </a>
					                        </div>
					                        <div class="nombre-usuario-wrap">
					                            <a href="#" target="_blank">
					                                <p class="nombre">${datospersona.name}</p>
					                                <p class="nombre-completo">${datospersona.organization ? datospersona.organization + ',': ''} ${datospersona.hasPosition ? datospersona.hasPosition : ''} ${datospersona.departamento ? datospersona.departamento: ''}</p>
					                            </a>
					                        </div>
					                    </div>
					                </div>
					                <div class="publicaciones-wrap d-none"></div>
					            </div>
					        </article>
						`
					}
					_self.listInvestigadoresSTP1.html(resHtml)
					_self.listenChecks(Object.keys(res).map(e => "stp1-res-" + e))
					checkboxResources.init()
				} else {
					_self.listInvestigadoresSTP1.parent().parent().remove()
					_self.listInvestigadoresSTP1 = _self.crearOfertaStep1.find(".resource-list-investigadores > div")
				}

				// OcultarUpdateProgress()
				resolve(true)
			})
		})
	}

	/**
	 * Comprueba los cambios para el los investigadores del primer nivel
	 * @param ids, array con los ids para 
	 * @param callback, Función para ejecutar después.
	 */
	listenChecks(ids, callback = () => {}) {

		let _self = this

		ids.forEach(idUser => {
			// Crea un evento para los investigadores del primer nivel, para detectar cambios en el dom
			$('#' + idUser).on("DOMSubtreeModified", function(e) {

				// Obtiene el ID del investigador
				let dataId = $(this).data("id")

				// Detecta si se ha seleccionado (texto igual a "done") o no
				let selector = $(this).find(".custom-checkbox-resource")
				if ($(selector).text().trim() == "done")
				{
					
					// Añade el investigador a la lista de investigadores seleccionados
					if(_self.data.researchers == null)					
					{
						_self.data.researchers = {}
					}
					_self.data.researchers[dataId] = _self.researchers[dataId]

				}else
				{
					if(_self.data.researchers == null)					
					{
						// Crea el objeto de investigadores si se encuentra vacío
						_self.data.researchers = {}
					} else {
						// Borrar investigador del objeto
						delete _self.data.researchers[dataId]
					}
				}

				_self.numSeleccionadosInvestigadores = Object.keys(_self.data.researchers).length
				callback()
			})	

		})
	}

	/**
	 * Método que es llamado por "LoadUsersGroup" para la petición ajax de carga del 
	 * personal investigador del grupo al que pertenece el usuario que está creando 
	 * la oferta tecnológica actualmente
	 */
	callLoadUsersGroup() {
		MostrarUpdateProgress()
		urlLoadUsersGroup.searchParams.set('pIdUserId', this.userId)
		return new Promise((resolve, reject) => {
			
			$.get(urlLoadUsersGroup.toString(), function (res) {
				resolve(res)
				OcultarUpdateProgress()
			})
		})
	}

	/**
	 * Método que inicia el proceso de ir al siguiente paso en el stepBar
	 */
	goNext() {
		this.goStep(this.step + 1)
	}

	/**
	 * Método que inicia el proceso de ir al siguiente paso en el stepBar
	 */
	goEnd() {
		if (this.step == this.numSteps && 
				(Object.keys(this.data.projects).length > 0 || Object.keys(this.data.documents).length > 0 || Object.keys(this.data.pii).length > 0)
			) {
			this.goStep(this.step + 1)
		} else {
			this.errorDiv.show()
		}
	}

	/**
	 * Método que inicia el proceso de ir al paso anterior en el stepBar
	 */
	goBack() {
		if (this.step > 1) {
			this.goStep(this.step - 1)
		}
	}
	
	/**
	 * Método que inicia las comprobaciones para pasar a la siguiente sección
	 * @param pos: Posición a la que se quiere pasar
	 */
	async goStep(pos) {
		var _self = this

		// Vuelve atrás
		if (pos > 0 && this.step > pos) {

			this.errorDiv.hide()
			this.errorDivStep2.hide()
			this.errorDivStep2Equals.hide()
			this.errorDivServer.hide()
			this.setStep(pos)

			switch (pos) {
				case 2:
				_self.startStep2()
				break;
			}

		// Siguiente paso
		} else if(pos > 0) {

			this.errorDiv.hide()
			this.errorDivStep2.hide()
			this.errorDivStep2Equals.hide()
			this.errorDivServer.hide()

			let continueStep = true
			switch (this.step) {
				case 1:
				continueStep = this.checkContinue1()
				if (continueStep) {
					_self.startStep2()
				}
				break;
				case 2:
				continueStep = this.checkContinue2()
				if (continueStep) {
					_self.startStep3()
				}
				break;
				case 3:
				continueStep = this.checkContinue3()
				if (continueStep) {
					_self.startStep4()
				}
				break;
				case 4:
				continueStep = this.checkContinue4()
				if (continueStep) {
					_self.startStep5()
				}
				break;
				case 5:
				try {
					continueStep = await this.saveInit()
					if (continueStep) {
						var urlCom = this.communityResourceUrl+"/"+ this.data.name.replace(/[^a-z0-9_]+/gi, '-').replace(/^-|-$/g, '').toLowerCase() +"/"+ this.ofertaId.split('_')[1]
						window.location = urlCom
					}
				} catch(err) {
					// this.errorDiv.show()
					this.errorDivServer.show()
					window.location.hash = '#' + this.errorDivServer.attr('id')
					continueStep = false
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
	 * Método que comprueba que los campos obligatorios de la sección 1 han sido rellenados
	 * También guarda el estado de la sección 1
	 * @return bool: Devuelve true or false dependiendo de si ha pasado la validación
	 */
	checkContinue1() {
		var _self = this

		// Get first screen data
		let name = document.getElementById('nombreofertainput').value
		// let description = document.getElementById('txtDescripcion').value
		let tags = []
		let inputsTagsItms = this.crearOfertaStep1.find('#oferta-modal-seleccionar-tags-stp1').find('input')
		inputsTagsItms.each((i, e) => {tags.push(e.value)})

		// let researchers = {}
		// // Obtener los investigadores seleccionados
		// if (_self.listInvestigadoresSTP1 && _self.listInvestigadoresSTP1.length > 0) {
		// 	let reserchersDom = _self.listInvestigadoresSTP1.find(".resource.seleccionado")
		// 	reserchersDom.each((i, e) => {if (e) {researchers[$(e).data("id")] = _self.researchers[$(e).data("id")]}})
		// }

		this.numSeleccionadosInvestigadores = Object.keys(_self.data.researchers).length


		this.data = {
			...this.data,
			entityID: _self.ofertaId,
			name,
			tags,
			// researchers
		}

		return (name.length > 0 && tags.length > 0)
	}

	/**
	 * Método que comprueba que al menos hay un investigador para la sección 2
	 * También guarda el estado de la sección 2
	 * @return bool: Devuelve true or false dependiendo de si ha pasado la validación
	 */
	checkContinue2() {
		return this.data.researchers && Object.keys(this.data.researchers).length > 0
	}

	/**
	 * Método que comprueba que todos los campos de la sección 3 se han rellenado
	 * También guarda el estado de la sección 3
	 * @return bool: Devuelve true or false dependiendo de si ha pasado la validación
	 */
	checkContinue3() {
		var _self = this


		// Get the second screen
		let lineasInvestigacion = this.crearOfertaStep3.find('.edit-etiquetas.lineasinvestigacion')
		let inputsTermsProf = lineasInvestigacion.find('input')
		let profTerms = {}
		inputsTermsProf.each((i, el) => {profTerms["id_" + el.value] = _self.divTesListaCats[el.value]})


		let dynamicRellenado = true
		// Comprueba si los campos de los modales están cargados (si son obligatorios)
		for (var [i, el] of Object.entries(_self.modalesDinamicos)) {
			
			// Get the second screen
			let item = document.getElementById(el.item)
			let inputsTermsProfDyn = item.querySelectorAll('input')
			let profTermsDyn = {}
			inputsTermsProfDyn.forEach((input, index) => { profTermsDyn[input.value] = _self.dataModalesDinamicos[i].find(e => e.id == input.value).name })

			// Set the post data
			_self.data[el.field] = profTermsDyn;

			if (el.obligatorio) {
				dynamicRellenado = dynamicRellenado && Object.keys(profTermsDyn).length > 0 || _self.dataModalesDinamicos[i].length == 0
			}
		}

		// Set the post data
		this.data = {
			...this.data,
			lineResearchs: profTerms,
			matureState: _self.ddlMadurez.val(),
			// framingSector: _self.ddlEncuadre.val(),
		}
		
		// Comprueba si las líneas de investigación se han rellenado o no hay ninguna disponible
		let existenTerms = Object.keys(profTerms).length > 0 || _self.divTesListaCats.length == 0

		// Comprueba si las estado de madurez se ha seleccionado
		let ematRellenado = _self.ddlMadurez.val() != ""

		// Comprueba si el sector de encuadre se ha seleccionado
		// let sencuaRellenado = _self.ddlEncuadre.val() != ""



		// return existenTerms && ematRellenado && sencuaRellenado
		return existenTerms && ematRellenado && dynamicRellenado
	}

	/**
	 * Método que comprueba que todos los campos obligatorios de la sección 4 se han rellenado
	 * También guarda el estado de la sección 4
	 * @return bool: Devuelve true or false dependiendo de si ha pasado la validación
	 */
	checkContinue4() {
		let _self = this


		// Objeto para guardar los htmls con sus correspondientes ids
		let objectFieldsHtml = {}

		let validado = true

		// Obtengo el HTML de los elementos obligatorios y lo guardo, adiccionalmente compruebo si no está vacío,
		// Si lo está devuelvo false
		this.listIdsTextsRequired.forEach(e => {
			objectFieldsHtml[e] = document.getElementById(e).innerHTML
			// Valida si el item contiene algo
			validado = objectFieldsHtml[e] != "" && validado
		})

		// Obtengo el HTML y lo guardo
		this.listIdsTextsOptional.forEach(e => {
			objectFieldsHtml[e] = document.getElementById(e).innerHTML
		})

		// Relleno el campo data
		this.data = {
			...this.data,
			objectFieldsHtml
		}

		return validado
	}

	/**
	 * Método que genera la petición get para obtener las taxonomías
	 */
	getDataTaxonomies() {
		
		let _self = this
		// Lista de Tesauros semánticos a cargar
		let listThesaurus = Object.keys(this.modalesDinamicos)
		var args = {
			listThesaurus: listThesaurus,
			lang: this.currentLang,
		}
		
		// Carga el idioma
		// urlLT.searchParams.set('lang', this.currentLang)

		// Realizo la llamada ajax y devuelvo la promesa
		// return new Promise((resolve, reject) => {
		// 	_self.postCall(urlLT, args).then((data) => {
		// 		resolve(data)
		// 	})
		// })

		return new Promise((resolve, reject) => {
			$.post(urlLT.toString(), args, function (data) {
				resolve(data)
			})
		})
	}

	/**
	 * Crea un objeto (y lo almacena) de un modal para cargar y seleccionar elementos
	 * @param data, Objeto con los datos resultantes de la petición, que contiene el árbol de opciones del modal
	 */
	fillDataTaxonomies(data) {

		// let resultHtml = this.fillTaxonomiesTree(data['areaprocedencia']);
		// let resultHtml = this.fillTaxonomiesTree(data['sectoraplicacion']);

		for (const [index, itemData] of Object.entries(data)) {
			
			this.modalesDinamicos[index].modal = new ModalCategoryCreator(this.modalesDinamicos[index].item, index, this.crearOferta)
			this.modalesDinamicos[index].modal.create(itemData)

			// this.modalesDinamicos[index].modalObject = modalObject
		}
	}

	/**
	 * Inicia la generación del html para las diferentes taxonomías
	 * @param data, Objeto con los items
	 */
	fillDataTaxonomiesWithoutParent(data) {
		// Set tree
		let resultHtml = this.fillTaxonomiesTreeWithoutParent(data)
		this.divTesArbol.find('.categoria-wrap').remove()
		this.divTesArbol.append(resultHtml)

		// Set list
		/* resultHtml = this.fillTaxonomiesList(data['researcharea'])
		this.divTesLista.append(resultHtml)
		this.divTesListaCats = this.divTesLista.find(".categoria-wrap") */

		// Open & close event trigger
		let desplegables = this.modalAreasTematicas.find('.boton-desplegar')
	
		if (desplegables.length > 0) {
			desplegables.off('click').on('click', function () {
				$(this).toggleClass('mostrar-hijos')
			})
		}

		// Add events when the items are clicked
		this.itemsClickedWithoutParent()
	}

	/**
	 * Add events when the Taxonomies items are clicked
	 */
	itemsClickedWithoutParent() {

		let _self = this

		// Click into the tree
		_self.divTesArbol.off('click').on("click", "input.at-input", function() {
			let dataVal = this.checked
			let dataId = $(this).attr('id')

			// Añadimos un cambio para las areas tematicas
			_self.cambiosAreasTematicas ++

			_self.btnSaveAT.removeClass('disabled')
			
		})
	}
	/**
	 * Crea el html con las taxonomías
	 * @param data, array con los items
	 * @param idParent, id del nodo padre, para generar los hijos
	 * @return string con el texto generado
	 */
	fillTaxonomiesTreeWithoutParent(data) {

		var _self = this

		let resultHtml = ""
		data.forEach((e, id) => {
			
			resultHtml += `<div class="categoria-wrap">
					<div class="categoria investigacion ${id}">
						<div class="custom-control custom-checkbox themed little primary">
							<input type="checkbox" class="custom-control-input at-input" id="${id}" data-id="${id}" data-name="${e}">
							<label class="custom-control-label" for="${id}">${e}</label>
						</div>
					</div>`

			resultHtml += '</div>'
		})

		return resultHtml
	}



	/**
	 * Método que guarda los 2 pasos iniciales
	 */
	saveInit() {

		var _self = this
		this.data.pIdGnossUser = this.userId
		
		MostrarUpdateProgress();
		return new Promise((resolve) => {
			
			$.post(urlSOff, this.data)
				.done(
					function (rdata) {
						_self.ofertaId = rdata
						_self.data.entityID = rdata
						// _self.startStep2()
						resolve(true)
					}
				)
				.fail(
					function (xhr, status, error) {
						resolve(false)
					}
				)
				.always(() => {
					OcultarUpdateProgress();
				})
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
					resolve(err)
				}
			})

			// $.post(url.toString(), theParams, function(rdata) {
			// 	resolve(rdata)
			// }).fail(function(err) {
			// 	resolve(err)
			// })
		})
	}

	/** 
	 * Método que realiza la llamada GET
	 * @param url, objeto URL que contiene la url de la petición POST
	 */
	getCall(url) {
		return new Promise((resolve) => {
			$.get(url.toString(), (rdata) => {
				resolve(rdata)
			})
		})
	}

	/**
	 * Carga todas las áreas temáticas seleccionadas para ese perfil / sección 
	 * @param item, sección donde se encuentra la información para cargar las areas temáticas
	 */
	setAreasTematicas(item) {
		let _self = this

		let relItem = $('#' + $(item).data("rel"))

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
	 * Carga todas las áreas temáticas seleccionadas para ese perfil / sección 
	 * @param item, sección donde se encuentra la información para cargar las areas temáticas
	 */
	setAreasTematicasDynamic(id) {

		this.modalesDinamicos[id].modal.setAreasTematicas(this.modalesDinamicos[id].rel)

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
		let tagsItems = relItem.find('.tag-wrap')
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
		var _self = this

		if (data != null) {
			// Entra aquí por primera vez si el oferta ha sido guardado

			let htmlResWrapper = $('<div class="tag-list mb-4 d-inline"></div>')

			let htmlRes = ''
			let dataWithNames = []

			if (this.divTesListaCats != null) {
				data.forEach(id => {
					dataWithNames.push({id, name: _self.divTesListaCats[id]})
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
	 * Método que genera el evento para añadir las áreas temáticas selecciondas en el popup
	 * @param relItem, elemento relacionado para indicar dónde deben de guardarse las areas temáticas seleccionadas
	 * @param data, array con las areas temáticas seleccionadas
	 */
	saveAreasTematicasEventDynamic(itemId, relItem, data = null) {


		if (data != null) {
			// Entra aquí por primera vez si el cluster ha sido guardado

			let htmlResWrapper = $('<div class="tag-list mb-4 d-inline"></div>')

			let htmlRes = ''
			let dataWithNames = [];

			if (this.dataModalesDinamicos != null && this.dataModalesDinamicos[this.id]) {
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
		let tagsItems = relItem.find('.tag-wrap')
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

		// Set the buttons 
		this.btnBefore.hide()
		this.nextStep.hide()
		this.endStep.hide()
		if (tstep > 1) {
			this.btnBefore.show()
		}
		if (tstep < this.numSteps) {
			this.nextStep.show()
		} else {
			this.endStep.show()
		}
	}

	/**
	 * Inicia el paso 2
	 */
	startStep2() { 
		comportamientoPopupOferta.init(this.data)
		this.PrintSelectedUsersStp2 ()
		$('#sugeridos-oferta-tab').click()
	}

	/**
	 * Inicia el paso 3
	 */
	startStep3() { 
		let _self = this

		MostrarUpdateProgress();
		// Promise.all([this.loadLineResearchs(), this.loadMatureStates(), this.loadFramingSectors()]).then(values => {
		Promise.all([this.loadLineResearchs(), this.loadMatureStates()]).then(values => {

			// Líneas de investigación
			let lineResearchs = undefined
			let groups = undefined
			if (values[0] && values[0].item1) {
				lineResearchs = values[0].item1
				this.data.groups = values[0].item2
			} else {
				lineResearchs = values[0]
			}

			// Carga las taxonomías
			_self.fillDataTaxonomiesWithoutParent(lineResearchs)
			_self.divTesListaCats = lineResearchs


			// Estado de madurez y sectores de encuadre
			// Comprueba si existen y los carga si no se han rellenado ya
			if (!_self.maturesStates || !_self.framingsectors) {

				_self.maturesStates = values[1]
				// _self.framingsectors = values[2]


				// Pintar Estado de madurez
				let htmlMaturesStates = "<option value=\"\" selected=\"selected\">"+ _self.sinEspcificarText +"</option>"
				for (const[i, el] of Object.entries(_self.maturesStates)) {
					htmlMaturesStates += `<option value="${i}">${el}</option>`
				}
				this.ddlMadurez.html(htmlMaturesStates)


				// // Pintar categoría de encuadre
				// let htmlFramingsectors = "<option value=\"\" selected=\"selected\">"+ _self.sinEspcificarText +"</option>"
				// for (const[i, el] of Object.entries(_self.framingsectors)) {
				// 	htmlFramingsectors += `<option value="${i}">${el}</option>`
				// }
				// this.ddlEncuadre.html(htmlFramingsectors)


				if (_self.ofertaId) {
					_self.loadedOfferStep3()
				}

				
				// Start the select2
				this.ddlMadurez.select2();
				// this.ddlEncuadre.select2();
			}

			OcultarUpdateProgress();
		})

		// this.PrintSelectedUsersStp2 ()
		// $('#sugeridos-oferta-tab').click()
	}

	/**
	 * Establece los valores del paso 3 con los valores obtenidos de la carga de la oferta
	*/
	loadedOfferStep3() {
		var _self = this
		// Busca las categorías en las listas y las selecciona
		let resultsCats = Object.keys(_self.divTesListaCats).filter(e => {
			return Object.values(_self.data.lineResearchs).indexOf(_self.divTesListaCats[e]) != -1
		})
		// Pinta las categorías
		_self.saveAreasTematicasEvent(_self.seleccionLineasInvestigacion, resultsCats)

		// Selecciona los sectores de encuadre y el estado de madurez
		this.ddlMadurez.val(_self.data.matureState)
		this.ddlEncuadre.val(_self.data.framingSector)

	}

	/**
	 * Inicia el paso 4
	 */
	startStep4() { 
		let _self = this

		// Cargo los elementos si estoy editando un elemento
		if (_self.ofertaId) {

			// Ids de los campos de la sección 4
			// Todos los ids concatenados
			let concatedRes = _self.listIdsTextsRequired.concat(_self.listIdsTextsOptional)

			// Comprueba si hay contenido cargado e inicia el editor de texto con el contenido
			concatedRes.forEach(e => {
				if (_self.data.objectFieldsHtml[e]) {
					// Añado el Html
					let item = document.getElementById(e)
					item.innerHTML = _self.data.objectFieldsHtml[e]
					// Inicio el editor
					let parent = item.parentElement
					_self.objectsTextEditor[e] = new TextField(parent);
				}
			})

		}



		this.crearOfertaStep4.find('.edmaTextEditor').each((i, el) => {
			var dataId = el.dataset.id
			var cId = el.id

			function initTextEditor(el) {
				if (!el.classList.contains("inicilized")) {
					_self.objectsTextEditor[dataId] = new TextField(el);
				}
			}

			// Inicializo el editor
			$(el).off('click').on('click', (event) => {
				initTextEditor(el)
			})
			$(el).off('contextmenu').on('contextmenu', (event) => {
				initTextEditor(el)
				return false;
			})


			// Detecta si se ha hecho click fuera del editor
			document.addEventListener("click", (evt) => {

				let targetEl = evt.target; // clicked element
			  	if (targetEl.closest('#' + cId)) return


				if (el.querySelector('.visuell-view').innerHTML == "")
				{
					if (_self.objectsTextEditor[dataId] != undefined && _self.objectsTextEditor[dataId] != null) {

						// Eliminamos el objeto del textArea
						_self.objectsTextEditor[dataId].removeTextAreaOfertas()
						delete _self.objectsTextEditor[dataId]
					}
				}
			});
		})

	}

	/**
	 * Inicia el paso 5
	 */
	startStep5() {

		MostrarUpdateProgress();

		if (this.ofertaId) {
			this.PrintSelectedProjStp5();
			this.PrintSelectedDocuStp5();
			this.PrintSelectedPIIStp5();
		}


		let proyectosTab = document.getElementById("proyectos-tab")
		let documentosTab = document.getElementById("publicaciones-tab")
		let piiTab = document.getElementById("prop-industrial-intelectual-tab")

		proyectosTab.addEventListener("click", (event) => {
			comportamientoProyectosOferta.init(this.data)
		})

		documentosTab.addEventListener("click", (event) => {
			comportamientoPublicacionesOferta.init(this.data)
		})

		piiTab.addEventListener("click", (event) => {
			comportamientoPIIOferta.init(this.data)
		})

		proyectosTab.click()

		// comportamientoPublicacionesOferta.init(this.data)

		// OcultarUpdateProgress();

		// this.PrintSelectedUsersStp2 ()
		// $('#sugeridos-oferta-tab').click()
	}

	/** 
	 * Devuelve una nueva promesa con el listado de investigadores
	 */ 
	loadLineResearchs() {
		let _self = this
		return new Promise((resolve, reject) => {

			$.post(urlLoadLineResearchs, {pIdUsersId: Object.keys(_self.data.researchers)})
				.done(
					function (rdata) {
						resolve(rdata)
					}
				)
				.fail(
					function (xhr, status, error) {
						resolve(false)
					}
				)
		})
	}

	/** 
	 * Devuelve los estados de madurez de las ofertas
	 */
	loadMatureStates() {
		let _self = this
		return new Promise((resolve, reject) => {
			// Añado el idioma para obtener los resultados
			urlLoadMatureStates.searchParams.set('lang', _self.currentLang)
			
			_self.getCall(urlLoadMatureStates).then(rdata => {
				resolve(rdata)
			})
		})
	}

	/** 
	 * Devuelve los sectores de encaje de las ofertas
	 * ACTUALMENTE NO SE USA
	 */
	loadFramingSectors() {
		let _self = this
		return new Promise((resolve, reject) => {
			// Añado el idioma para obtener los resultados
			urlLoadFramingSectors.searchParams.set('lang', _self.currentLang)

			_self.getCall(urlLoadFramingSectors).then(rdata => {
				resolve(rdata)
			})
		})
	}

	/**
	 * Pintar los perfiles "finales"
	 */
	PrintSelectedUsersStp2 () {

		let imgUser = this.crearOferta.data('imguser')

		let profiles = Object.keys(this.data.researchers).map((e, i) => {
			let htmlUser = ""

			// Obtenemos el usuario actual
			let user = this.data.researchers[e]

			// Creamos el literal para la información del usuario si esta no existe.
			if (user.info == undefined) {
				user.info = user.organization + ', ' + user.hasPosition + ' ' + user.departamento
			}

			if(user != null)
			{

				htmlUser = `
				<article class="resource">
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
                        <div class="acciones-wrap">
                            <ul class="no-list-style">
                                <li>
                                    <a href="javascript:stepsOffer.removeSelectedUserSelected('`+ e +`')" class="texto-gris-claro">
                                        Eliminar
                                        <span class="material-icons-outlined">delete</span>
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </article>`
			}

			return htmlUser
		})
		// Añadimos el html de los investigadores
		let htmlUsersCont = `
			<div class="resource-list listView resource-list-investigadores">
			    <div class="resource-list-wrap">
			    	${profiles.join('')}
			    </div>
			</div>`

		this.crearOfertaStep2.find('.resource-list-investigadores').remove()
		this.researchersStep2.append(htmlUsersCont)

		// pintamos el número de investigadores
		this.numSeleccionadosInvestigadores = profiles.length
		this.printNumResearchers()
	}

	/**
	 * Pintar los proyectos "finales"
	 */
	PrintSelectedProjStp5 () {

		let imgUser = this.crearOferta.data('imguser')

		let projects = Object.keys(this.data.projects).map((e, i) => {
			let htmlUser = ""

			// Obtenemos el usuario actual
			let proj = this.data.projects[e]

			// Obtenemos el usuario actual
			let researchers = []
			let researchersFin = ""
			if (proj.researchers) {
				researchers = proj.researchers.map(user => {
					return '<li>' + user + '</li>'
				})
				researchersFin = '<ul>' + researchers.join('') + '</ul>'
			}

			if(proj != null)
			{

				htmlUser = `
				<article class="resource proyecto">
		            <div class="wrap">
		                <div class="middle-wrap">
		                    <div class="title-wrap">
		                        <h2 class="resource-title">
		                            <a href="javascript:void(0)">${proj.name}</a>
		                        </h2>
		                        <div class="acciones-wrap">
		                            <ul class="no-list-style">
		                                <li>
		                                    <a href="javascript:stepsOffer.removeSelectedProjectSelected('`+ e +`')" class="texto-gris-claro">
		                                        Eliminar
		                                        <span class="material-icons-outlined">delete</span>
		                                    </a>
		                                </li>
		                            </ul>
		                        </div>
		                    </div>
		                    <div class="content-wrap">
		                        <div class="description-wrap counted">
		                            <div class="list-wrap">
		                                <p class="nombre-completo">`+ proj.info +`</p>
		                            </div>
		                            <div class="list-wrap authors">
		                            	${researchersFin}
		                            </div>
		                            <div class="desc">
		                                <p>`+ proj.description +`</p>
		                            </div>
		                        </div>
		                    </div>
		                </div>
		            </div>
		        </article>`
			}

			return htmlUser
		})
		// Añadimos el html de los investigadores
		let htmlUsersCont = `
			<div class="resource-list listView resource-list-proyectos">
			    <div class="resource-list-wrap">
			    	${projects.join('')}
			    </div>
			</div>`

		this.ProyectosSelectedStep5.find('.resource-list-proyectos').remove()
		this.ProyectosSelectedStep5.append(htmlUsersCont)

		// pintamos el número de investigadores
		this.numSeleccionadosProjects = projects.length
		this.printNumProjects()
	}

	/**
	 * Pintar las publicaciones "finales"
	 */
	PrintSelectedDocuStp5 () {

		// let imgUser = this.crearOferta.data('imguser')

		let documents = Object.keys(this.data.documents).map((e, i) => {
			let htmlUser = ""

			// Obtenemos el usuario actual
			let proj = this.data.documents[e]

			// Obtenemos el usuario actual
			let researchers = []
			let researchersFin = ""
			if (proj.researchers) {
				researchers = proj.researchers.map(user => {
					return '<li>' + user + '</li>'
				})
				researchersFin = '<ul>' + researchers.join('') + '</ul>'
			}

			if(proj != null)
			{

				htmlUser = `
				<article class="resource proyecto">
		            <div class="wrap">
		                <div class="middle-wrap">
		                    <div class="title-wrap">
		                        <h2 class="resource-title">
		                            <a href="javascript:void(0)">${proj.name}</a>
		                        </h2>
		                        <div class="acciones-wrap">
		                            <ul class="no-list-style">
		                                <li>
		                                    <a href="javascript:stepsOffer.removeSelectedDocumentsSelected('`+ e +`')" class="texto-gris-claro">
		                                        Eliminar
		                                        <span class="material-icons-outlined">delete</span>
		                                    </a>
		                                </li>
		                            </ul>
		                        </div>
		                    </div>
		                    <div class="content-wrap">
		                        <div class="description-wrap counted">
		                            <div class="list-wrap">
		                                <p class="nombre-completo">`+ proj.info +`</p>
		                            </div>
		                            <div class="list-wrap authors">
		                            	${researchersFin}
		                            </div>
		                            <div class="desc">
		                                <p>`+ proj.description +`</p>
		                            </div>
		                        </div>
		                    </div>
		                </div>
		            </div>
		        </article>`
			}

			return htmlUser
		})
		// Añadimos el html de los investigadores
		let htmlUsersCont = `
			<div class="resource-list listView resource-list-proyectos">
			    <div class="resource-list-wrap">
			    	${documents.join('')}
			    </div>
			</div>`

		this.DocumentsSelectedStep5.find('.resource-list-proyectos').remove()
		this.DocumentsSelectedStep5.append(htmlUsersCont)

		// pintamos el número de investigadores
		this.numSeleccionadosDocuments = documents.length
		this.printNumDocuments()
	}

	/**
	 * Pintar las publicaciones "finales"
	 */
	PrintSelectedPIIStp5 () {

		// let imgUser = this.crearOferta.data('imguser')

		let pii = Object.keys(this.data.pii).map((e, i) => {
			let htmlUser = ""

			// Obtenemos el usuario actual
			let resource = this.data.pii[e]

			// Obtenemos el usuario actual
			let researchers = []
			let researchersFin = ""
			if (resource.researchers) {
				researchers = resource.researchers.map(user => {
					return '<li>' + user + '</li>'
				})
				researchersFin = '<ul>' + researchers.join('') + '</ul>'
			}

			if(resource != null)
			{

				htmlUser = `
				<article class="resource proyecto">
		            <div class="wrap">
		                <div class="middle-wrap">
		                    <div class="title-wrap">
		                        <h2 class="resource-title">
		                            <a href="javascript:void(0)">${resource.name}</a>
		                        </h2>
		                        <div class="acciones-wrap">
		                            <ul class="no-list-style">
		                                <li>
		                                    <a href="javascript:stepsOffer.removeSelectedPIISelected('`+ e +`')" class="texto-gris-claro">
		                                        Eliminar
		                                        <span class="material-icons-outlined">delete</span>
		                                    </a>
		                                </li>
		                            </ul>
		                        </div>
		                    </div>
		                    <div class="content-wrap">
		                        <div class="description-wrap counted">
		                            <div class="list-wrap">
		                                <p class="nombre-completo">`+ resource.info +`</p>
		                            </div>
		                            <div class="list-wrap authors">
		                            	${researchersFin}
		                            </div>
		                            <div class="desc">
		                                <p>`+ resource.description +`</p>
		                            </div>
		                        </div>
		                    </div>
		                </div>
		            </div>
		        </article>`
			}

			return htmlUser
		})
		// Añadimos el html de los investigadores
		let htmlUsersCont = `
			<div class="resource-list listView resource-list-proyectos">
			    <div class="resource-list-wrap">
			    	${pii.join('')}
			    </div>
			</div>`

		this.PIISelectedStep5.find('.resource-list-proyectos').remove()
		this.PIISelectedStep5.append(htmlUsersCont)

		// pintamos el número de investigadores
		this.numSeleccionadosPII = pii.length
		this.printNumPII()
	}

	/**
	 * Método que pinta el número de investigadores seleccionados
	 */
	printNumResearchers() {

		// Establecemos el número de investigadores
		this.crearOfertaStep2.find('#stp2-num-selected-txt').text(this.numSeleccionadosInvestigadores)
		this.crearOfertaStep2.find('#stp2-num-selected').text('(' + this.numSeleccionadosInvestigadores + ')')
	}

	/**
	 * Método que pinta el número de proyectos seleccionados
	 */
	printNumProjects() {

		// Establecemos el número de investigadores
		this.crearOfertaStep5.find('#stp5-num-selected-proj-txt').text(this.numSeleccionadosProjects)
		this.crearOfertaStep5.find('#stp5-proj-num-selected').text('(' + this.numSeleccionadosProjects + ')')
	}

	/**
	 * Método que pinta el número de proyectos seleccionados
	 */
	printNumDocuments() {

		// Establecemos el número de investigadores
		this.crearOfertaStep5.find('#stp5-num-selected-docs-txt').text(this.numSeleccionadosDocuments)
		this.crearOfertaStep5.find('#stp5-docs-num-selected').text('(' + this.numSeleccionadosDocuments + ')')
	}

	/**
	 * Método que pinta el número de proyectos seleccionados
	 */
	printNumPII() {

		// Establecemos el número de investigadores
		this.crearOfertaStep5.find('#stp5-num-selected-pii-txt').text(this.numSeleccionadosPII)
		this.crearOfertaStep5.find('#stp5-pii-num-selected').text('(' + this.numSeleccionadosPII + ')')
	}

	/**
	 * Borra los investigadores del listado de seleccionado
	 * @param idUser, id del usuario a borrar
	 */
	removeSelectedUserSelected(idUser) {

		delete this.data.researchers[idUser]


		// Desmarcamos el investigador de la lista de búsqueda
		if (!!document.getElementById(idUser)) {
			let item = document.getElementById(idUser)
			item.classList.remove('seleccionado')
			
			try {
				let itemCheck = item.getElementsByClassName('custom-checkbox-resource')[0]
				itemCheck.classList.remove('done')
				itemCheck.classList.add('add')
			} catch (error) {}
			
			try {
				let itemCheckMaterial = item.getElementsByClassName('material-icons')[0]
				itemCheckMaterial.textContent = "add"
			} catch (error) {}
		}

		this.PrintSelectedUsersStp2()
	}

	/**
	 * Borra los proyectos del listado de seleccionado
	 * @param idItem, id del proyecto a borrar
	 */
	removeSelectedProjectSelected(idItem) {

		delete this.data.projects[idItem]

		// Desmarcamos el investigador de la lista de búsqueda
		if (!!document.getElementById("project_" + idItem)) {
			let item = document.getElementById("project_" + idItem)
			item.classList.remove('seleccionado')
			
			try {
				let itemCheck = item.getElementsByClassName('custom-checkbox-resource')[0]
				itemCheck.classList.remove('done')
				itemCheck.classList.add('add')
			} catch (error) {}
			
			try {
				let itemCheckMaterial = item.getElementsByClassName('material-icons')[0]
				itemCheckMaterial.textContent = "add"
			} catch (error) {}
		}


		this.PrintSelectedProjStp5()
	}

	/**
	 * Borra las publicaciones del listado de seleccionado
	 * @param idItem, id de la publicación a borrar
	 */
	removeSelectedDocumentsSelected(idItem) {

		try {
			delete this.data.documents[idItem]
		} catch (error) {console.error("Error al borrar el item")}

		// Desmarcamos el investigador de la lista de búsqueda
		if (!!document.getElementById("resource_" + idItem)) {
			let item = document.getElementById("resource_" + idItem)
			item.classList.remove('seleccionado')
			
			try {
				let itemCheck = item.getElementsByClassName('custom-checkbox-resource')[0]
				itemCheck.classList.remove('done')
				itemCheck.classList.add('add')
			} catch (error) {}
			
			try {
				let itemCheckMaterial = item.getElementsByClassName('material-icons')[0]
				itemCheckMaterial.textContent = "add"
			} catch (error) {}
		}


		this.PrintSelectedDocuStp5()
	}

	/**
	 * Borra las propiedades industrial intelectual del listado de seleccionado
	 * @param idItem, id de la PII a borrar
	 */
	removeSelectedPIISelected(idItem) {

		try {
			delete this.data.pii[idItem]
		} catch (error) {console.error("Error al borrar el item")}

		// Desmarcamos el investigador de la lista de búsqueda
		if (!!document.getElementById("resource_" + idItem)) {
			let item = document.getElementById("resource_" + idItem)
			item.classList.remove('seleccionado')
			
			try {
				let itemCheck = item.getElementsByClassName('custom-checkbox-resource')[0]
				itemCheck.classList.remove('done')
				itemCheck.classList.add('add')
			} catch (error) {}
			
			try {
				let itemCheckMaterial = item.getElementsByClassName('material-icons')[0]
				itemCheckMaterial.textContent = "add"
			} catch (error) {}
		}


		this.PrintSelectedPIIStp5()
	}
}



class ModalCategoryCreator {

	constructor (itemRelatedId, id, crearOferta) {

		this.modal = undefined
		this.divTesArbol = undefined

		this.itemRelatedId = itemRelatedId
		this.crearOferta = crearOferta

		this.id = id
		this.cambios = 0
	}

	/**
	 * Inicia la generación del html para las diferentes taxonomías
	 * @param data, Objeto con los items
	 */
	create (data) {

		this.data = data

		let modal = this.setNewDynamicModal()
		this.modal = modal

		this.divTesArbol = $(modal.querySelector(".divTesArbol"))

		let divTesArbol = modal.querySelector(".divTesArbol")
		let $divTesArbol = $(divTesArbol)


		// Set tree
		let resultHtml = this.fillTaxonomiesTree(data);
		$divTesArbol.find('.categoria-wrap').remove();
		$divTesArbol.append(resultHtml);

		// Set list
		/* resultHtml = this.fillTaxonomiesList(data['researcharea'])
		this.divTesLista.append(resultHtml)
		this.divTesListaCaths = this.divTesLista.find(".categoria-wrap") */

		// Open & close event trigger
		let desplegables = modal.querySelectorAll('.boton-desplegar')
	
		if ($(desplegables).length > 0) {
			$(desplegables).off('click').on('click', function () {
				$(this).toggleClass('mostrar-hijos');
			});
		}

		// Add events when the items are clicked
		this.itemsClicked();


	}

	/**
	 * Método que crea un modal de forma dinámica
	 */
	setNewDynamicModal() {

		let itemRelated = document.getElementById(this.itemRelatedId)

		// Establezco las traducciones
		let traducciones = {}
		if (itemRelated) {
			traducciones = {
				"title": itemRelated.dataset.title,
				"subtitle": itemRelated.dataset.subtitle,
				"small": itemRelated.dataset.small,
				"arbol": itemRelated.dataset.arbol,
				"buscar": itemRelated.dataset.buscar,
				"guardar": itemRelated.dataset.guardar,
			}
		}

		// Creo el modal
		let modal = document.createElement('div')
        modal.classList.add('modal')
        modal.classList.add('modal-top')
        modal.classList.add('fade')
        modal.classList.add('modal-edicion')
        modal.setAttribute("tabindex", -1)
        modal.setAttribute("role", "dialog")
        modal.setAttribute("id", "modal-seleccionar-"+ this.id)
        modal.setAttribute("data-backdrop", "static")

		let html = `
				<div class="modal-dialog" role="document">
					<div class="modal-content">
						<div class="modal-header">
							<p class="modal-title"><span class="material-icons">folder_open</span>${traducciones.title}</p>
							<span class="material-icons cerrar" data-dismiss="modal" aria-label="Close">close</span>
						</div>
						<div class="modal-body">
							<div class="formulario-edicion">
								<div class="form-group">
									<label class="control-label">${traducciones.subtitle}</label>
								</div>
								<small>${traducciones.small}</small>
								<div id="panDesplegableSelCat_${this.id}" class="mt-4">
									<ul class="nav nav-tabs d-none" id="myTab" role="tablist">
										<li class="nav-item">
											<a class="nav-link active" id="ver-arbol-tab" data-toggle="tab" href="#ver-arbol" role="tab" aria-controls="ver-arbol" aria-selected="true">${traducciones.arbol}</a>
										</li>
									</ul>
									<div class="tab-content pt-0">

										<div class="tab-pane fade show active" id="ver-arbol" role="tabpanel" aria-labelledby="ver-arbol-tab">
											<div class="divTesArbol divCategorias clearfix">
												<div class="buscador-categorias">
													<div class="form-group">
														<input class="filtroRapido form-control not-outline" placeholder="${traducciones.buscar}" type="text" onkeydown="javascript:if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {return false;}}" onkeyup="javascript:MVCFiltrarListaSelCatArbol(this, 'panDesplegableSelCat_${this.id}');">
													</div>
												</div>
											</div>
										</div>
									</div>
								</div>
								<div class="form-actions top">
									<a href="javascript:void(0)" class="btn btn-primary pmd-ripple-effect btnsave disabled">${traducciones.guardar}</a>
								</div>
							</div>
						</div>
					</div>
				</div>
		`
		// Le añado el html contenedor
		modal.innerHTML += html

		// Inserto el modal en la página
		this.crearOferta.after(modal)
		// insertAfter(modal, this.crearOferta)

		return modal
	}


	/**
	 * Add events when the Taxonomies items are clicked
	 */
	itemsClicked() {

		let _self = this
		let currentModal = this.modal

		// Click into the tree
		this.divTesArbol.off('click').on("click", "input.at-input", function() {
			let dataVal = this.checked
			let dataId = $(this).attr('id')
			let dataParentId = $(this).data('parentid')
			dataParentId = (dataParentId.length > 0) ? dataParentId.split('/').pop() : dataParentId

			// Añadimos un cambio para las areas tematicas
			_self.cambios ++
			$(currentModal).find('.btnsave').removeClass('disabled')

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

		var _self = this

		let resultHtml = ""
		data.forEach((e, id) => {
			resultHtml += '<div class="categoria-wrap" data-text="' + e + '">\
					<div class="categoria list__' + id + '">\
						<div class="custom-control custom-checkbox themed little primary">\
							<input type="checkbox" class="custom-control-input at-input" id="list__' + id + '" data-id="' + id + '" data-name="' + e + '">\
							<label class="custom-control-label" for="list__' + id + '">' + e + '</label>\
						</div>\
					</div>\
				</div>'
		})

		return resultHtml
	}


	/*  SECTION SAVE METHODS  */



	/**
	 * Carga todas las áreas temáticas seleccionadas para ese perfil / sección 
	 * @param item, sección donde se encuentra la información para cargar las areas temáticas
	 */
	setAreasTematicas(item) {

		let _self = this

		let relItem = $('#' + item)

		if (relItem.length > 0) {

			let dataJson = relItem.data('jsondata')

			// Reestablecer las categorías
			_self.divTesArbol.find('input.at-input').each((i, e) => {
				e.checked = false
				e.classList.remove('selected')
			})

			// Establece las categorías de la sección
			if (typeof dataJson == "object") {

				dataJson.forEach(e => {
					let citem = document.getElementById(e)
					
					if (citem) {
						citem.checked = true
						// Comprueba si tiene padre para establecerlo con habilitado.
						let parentId = citem.getAttribute('data-parentid')
						if (parentId.length > 0) {
							parentId = (parentId.length > 0) ? parentId.split('/').pop() : parentId

							_self.selectParent(parentId)
						}
					} else {
						console.log ("el elemento no existe: #", e)
					}

				})
			}


			// Reestablecemos el botón de guardar las Áreas Temáticas
			this.cambios = 0
			$(_self.modal).find('.btnsave').addClass('disabled')

			// Muestra el modal de las áreas temáticas
			$(_self.modal).modal('show')

			this.saveAreasTematicasEvent(relItem)
		}
	}


	/**
	 * Método que genera el evento para añadir las áreas temáticas selecciondas en el popup
	 * @param relItem, elemento relacionado para indicar dónde deben de guardarse las areas temáticas seleccionadas
	 * @param data, array con las areas temáticas seleccionadas
	 */
	saveAreasTematicasEvent (relItem, data = null) {

		let _self = this

		if (data != null) {

			// Entra aquí por primera vez si la oferta ha sido guardado
			let htmlResWrapper = $('<div class="tag-list mb-4 d-inline"></div>')

			let htmlRes = ''
			let dataWithNames = [];

			Object.keys(data).forEach(id => {
				let item = undefined
				if (item = this.data.find(e => e.id == id)) {
					dataWithNames.push({ id, name: item.name })
				}
			})

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
			$(this.modal).find('.btnsave').off('click').on('click', (e) => {
				e.preventDefault()

				// Oculta el modal de las áreas temáticas
				$(_self.modal).modal('hide')

				// Reestablecemos el botón de guardar las Áreas Temáticas
				_self.cambios = 0
				$(_self.modal).find('.btnsave').addClass('disabled')

				// Selecciona y establece el contenedor de las areas temáticas
				// let relItem = $('#' + $(item).data("rel"))

				let htmlResWrapper = $('<div class="tag-list mb-4 d-inline"></div>')

				let htmlRes = ''
				let arrayItems = [] // Array para guardar los items que se van a usar
				_self.divTesArbol.find('input.at-input').each((i, e) => {
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
		let tagsItems = relItem.find('.tag-wrap')
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

}




/**
 * Función que se llama cuando se cargan los investigadores
 */
function CompletadaCargaRecursosInvestigadoresOfertas()
{	
	let currentsIds = []
	if(typeof stepsOffer != 'undefined' && stepsOffer != null && stepsOffer.data != null)
	{		
		$('#ofertaListUsers article.resource h2.resource-title').attr('tagert','_blank')
		stepsOffer.data.pPersons = $('#ofertaListUsers article.resource').toArray().map(e => {return $(e).attr('id')})
		
		$('#ofertaListUsers article.resource').each((i, e) => {

			currentsIds.push(e.id)

			if ($(e).find(".custom-checkbox-resource .material-icons").length == 0) {

				if (Object.values(stepsOffer.data.researchers).filter(pr => pr.shortId == e.id).length > 0) {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource done">
		                <span class="material-icons">done</span>
		            </div>`)
					$(e).addClass('seleccionado')
				}
				else {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource add">
				        <span class="material-icons">add</span>
				    </div>`)
				}
			}
		})

		checkboxResources.init()

		currentsIds.forEach(idUser => {

			$('#' + idUser).on("DOMSubtreeModified", function(e) {

				let selector = $(this).find(".custom-checkbox-resource")

				if ($(selector).text().trim() == "done")
				{
					let elementUser = $(this)
					let user = {}
					let arrInfo = []
					user.shortId = idUser
					user.name = elementUser.find('h2.resource-title').text().trim()

					// Obtener la descripción
					elementUser.find('.middle-wrap > .content-wrap > .list-wrap li').each((i, elem) => {
						arrInfo.push($(elem).text().trim())
					})
					user.info = arrInfo.join(', ')

					let numPubDOM = $(this).find('.info-resource .texto')
					numPubDOM.each((i, e) => {
						let textPubDom = $(numPubDOM).text()
						if (textPubDom.includes('publicaciones')) {
							let numPub = textPubDom.split('publicaciones')[0].trim()
							user.numPublicacionesTotal = numPub
						}
					})

					// user.ipNumber = elementUser.data('ipNumber')
					if(stepsOffer.data.researchers == null)					
					{
						stepsOffer.data.researchers = {};
					}
					stepsOffer.data.researchers[idUser] = user

				}else
				{
					// Borrar investigador del objeto
					delete stepsOffer.data.researchers[idUser]
				}

				stepsOffer.numSeleccionadosInvestigadores = Object.keys(stepsOffer.data.researchers).length
				stepsOffer.PrintSelectedUsersStp2();
			});	

		})

		

		

		// $('article.resource .user-perfil').remove()
		// let htmlPerfiles=''
		// if(score.numPublicaciones>0)
		// {
		// 	let idProfileEdit = idProfile
		// 	if(idProfileEdit.length!=36)
		// 	{
		// 		idProfileEdit=idProfileEdit.split('_')[2]
		// 	}
		// 	let nombrePerfil = stepsOffer.data.researchers.filter(function (item) {return item.shortEntityID ==idProfileEdit || item.entityID ==idProfileEdit})[0].name
			
		// 	let publicationsPercent = score.numPublicaciones/score.numPublicacionesTotal*100



		// 	htmlPerfiles+=`	<div class="perfil-wrap">
		// 			        <div class="custom-wrap">
		// 			            <div class="custom-control custom-checkbox">
		// 			                <input type="checkbox" class="custom-control-input" id="${idperson}-${idProfileEdit}">
		// 			                <label class="custom-control-label" for="${idperson}-${idProfileEdit}">
		// 			                    ${nombrePerfil}
		// 			                </label>
		// 			            </div>
		// 			            <div class="check-actions-wrap">
		// 			                <a href="javascript: void(0)" class="dropdown-toggle check-actions-toggle" data-toggle="dropdown" aria-expanded="true">
		// 			                    <span class="material-icons">
		// 			                        arrow_drop_down
		// 			                    </span>
		// 			                </a>
		// 			                <div class="dropdown-menu basic-dropdown check-actions" id="checkActions" x-placement="bottom-start">
		// 			                    <div class="barras-progreso-wrapper">
		// 			                        <div class="progreso-wrapper">
		// 			                            <div class="progress">
		// 			                                <div class="progress-bar background-success" role="progressbar" style="width: ${score.ajuste * 100}%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
		// 			                            </div>
		// 			                            <span class="progress-text"><span class="font-weight-bold">${Math.round(score.ajuste * 10000)/100}%</span></span>
		// 			                        </div>
		// 			                        <div class="progreso-wrapper">
		// 			                            <div class="progress">
		// 			                                <div class="progress-bar" role="progressbar" style="width: ${publicationsPercent}%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
		// 			                            </div>
		// 			                            <span class="progress-text"><span class="font-weight-bold">${score.numPublicaciones} /</span> ${score.numPublicacionesTotal}</span>
		// 			                        </div>
		// 			                    </div>
		// 			                    <div class="wrap">
		// 			                        <div class="header-wrap">
		// 			                            <p>Areas temáticas</p>
		// 			                            <p>Publicaciones</p>
		// 			                        </div>
		// 			                        <div class="areas-tematicas-wrap">
		// 			                            <ul class="no-list-style">
		// 			                                ${termsHtml}
		// 			                            </ul>
		// 			                        </div>
		// 			                    </div>
		// 			                    <div class="wrap">
		// 			                        <div class="header-wrap">
		// 			                            <p>Descriptores</p>
		// 			                        </div>
		// 			                        <div class="descriptores-wrap">
		// 			                            <ul class="no-list-style">
		// 			                                ${tagsHtml}
		// 			                            </ul>
		// 			                        </div>
		// 			                    </div>
		// 			                </div>
		// 			            </div>
		// 			        </div>
		// 			        <div class="barras-progreso-wrap">
		// 			            <div class="progreso-wrapper">
		// 			                <div class="progress">
		// 			                    <div class="progress-bar background-success" role="progressbar" style="width: ${score.ajuste * 100}%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
		// 			                </div>
		// 			                <span class="progress-text"><span class="font-weight-bold">${Math.round(score.ajuste * 10000)/100}%</span></span>
		// 			            </div>
		// 			            <div class="progreso-wrapper">
		// 			                <div class="progress">
		// 			                    <div class="progress-bar" role="progressbar" style="width: ${publicationsPercent}%" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100"></div>
		// 			                </div>
		// 			                <span class="progress-text"><span class="font-weight-bold">${score.numPublicaciones} /</span> ${score.numPublicacionesTotal}</span>
		// 			            </div>
		// 			        </div>
		// 			    </div>`


		// }
		// let htmlPerfilesPersona=`	<div class="user-perfil pl-0">
		// 								${htmlPerfiles}
		// 							</div>`				
		// $('#'+idperson+' .content-wrap.flex-column').append(htmlPerfilesPersona)
		// try {
		// 	$('#'+idperson).data('numPublicacionesTotal', Object.values(datospersona)[0].numPublicacionesTotal)
		// 	$('#'+idperson).data('ipNumber', Object.values(datospersona)[0].ipNumber)
		// } catch (e) { }

		// let repintar = false
		// //Marcamos como checkeados los correspondientes
		// stepsOffer.data.researchers.forEach(function(perfil, index) {
		// 	let idProfile= perfil.shortEntityID
		// 	if(perfil.users!=null)
		// 	{
		// 		perfil.users.forEach(function(user, index) {
		// 			var elementUser = $('#'+user.shortUserID)
		// 			$('#'+user.shortUserID+'-'+idProfile).prop('checked', true)
		// 		})					
		// 	}
		// })
		// if (repintar) {
		// 	stepsOffer.PrintPerfilesstp3()
		// }
		
		// //Enganchamos el chek de los chekbox	
		// $('.perfil-wrap .custom-control-input').change(function() {
		// 	let id=$(this).attr('id');
		// 	let idUser=id.substring(0,36);
		// 	let idProfile=id.substring(37);					
		// 	let perfil=stepsOffer.data.researchers.filter(function (perfilInt) {
		// 		return perfilInt.shortEntityID==idProfile || perfilInt.entityID==idProfile ;
		// 	})[0];
		// 	if(this.checked) {
		// 		let elementUser = $(this).closest('.resource.investigador')
		// 		let user = {}
		// 		let arrInfo = []
		// 		user.shortUserID = idUser
		// 		user.name = elementUser.find('h2.resource-title').text().trim()

		// 		// Obtener la descripción
		// 		elementUser.find('.middle-wrap > .content-wrap > .list-wrap li').each((i, elem) => {
		// 			arrInfo.push($(elem).text().trim())
		// 		})
		// 		user.info = arrInfo.join(', ')

		// 		user.numPublicacionesTotal = elementUser.data('numPublicacionesTotal')
		// 		user.ipNumber = elementUser.data('ipNumber')
		// 		if(perfil.users==null)					
		// 		{
		// 			perfil.users=[];
		// 		}
		// 		perfil.users.push(user);
		// 	}else
		// 	{
		// 		perfil.users=perfil.users.filter(function (userInt) {
		// 			return userInt.shortUserID!=idUser;
		// 		});
		// 	}
		// 	stepsOffer.PrintPerfilesstp3();
		// 	newGrafProjClust.CargarGraficaColaboradores(stepsOffer.data, 'colaboratorsgraphCluster', true);
		// });	
	}
}


/**
 * Función que se llama cuando se cargan los proyectos
 */
function CompletadaCargaRecursosProyectosOfertas()
{	
	let currentsIds = []
	if(typeof stepsOffer != 'undefined' && stepsOffer != null && stepsOffer.data != null)
	{		
		$('#ofertaListProyectos article.resource h2.resource-title').attr('tagert','_blank')
		// stepsOffer.data.pPersons = $('#ofertaListProyectos article.resource').toArray().map(e => {return $(e).attr('id')})
		
		$('#ofertaListProyectos article.resource').each((i, e) => {

			currentsIds.push(e.id.split('_')[1])

			if ($(e).find(".custom-checkbox-resource .material-icons").length == 0) {

				if (stepsOffer.data.projects && Object.values(stepsOffer.data.projects).filter(pr => pr.shortId == currentsIds.at(-1)).length > 0) {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource done">
		                <span class="material-icons">done</span>
		            </div>`)
					$(e).addClass('seleccionado')
				}
				else {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource add">
				        <span class="material-icons">add</span>
				    </div>`)
				}
			}
		})

		checkboxResources.init()

		currentsIds.forEach(idProject => {

			$('#project_' + idProject).on("DOMSubtreeModified", function(e) {

				let selector = $(this).find(".custom-checkbox-resource")

				if ($(selector).text().trim() == "done")
				{
					let element = $(this)
					let item = {}
					let arrInfo = []
					let arrUsers = []
					let description = []
					let dates = []
					item.shortId = idProject
					item.name = element.find('h2.resource-title').text().trim()

					// Obtener la información descriptiva
					element.find('.middle-wrap > .content-wrap > .list-wrap:not(.authors) li').each((i, elem) => {
						arrInfo.push($(elem).text().trim())
					})
					item.info = arrInfo.join(', ')

					// Obtener los investigadores
					element.find('.middle-wrap > .content-wrap > .list-wrap.authors li').each((i, elem) => {
						arrUsers.push($(elem).text().trim())
					})
					item.researchers = arrUsers

					// Obtener la descripción
					element.find('.middle-wrap > .content-wrap > .description-wrap .desc').each((i, elem) => {
						description.push($(elem).text().trim())
					})
					item.description = description.join(', ')

					// Obtener la fecha
					element.find('.middle-wrap > .content-wrap > .content-wrap span').each((i, elem) => {
						dates = ($(elem).text().trim())
					})
					item.dates = dates


					// item.ipNumber = element.data('ipNumber')
					if(stepsOffer.data.projects == null)					
					{
						stepsOffer.data.projects = {};
					}
					stepsOffer.data.projects[idProject] = item

				} else
				{
					// Borrar investigador del objeto
					if (stepsOffer.data.projects) {
						delete stepsOffer.data.projects[idProject]
					}
				}

				// Pintamos de nuevo los elementos seleccionados
				let newPNumber = 0
				// Obtengo el número de elementos actuales
				if (stepsOffer.data.projects) {
					newPNumber = Object.keys(stepsOffer.data.projects).length
				} else {
					stepsOffer.numSeleccionadosProjects = 0
				}
				// Pinto únicamente nuevos elementos si el número de elementos ha cambiado.
				if (stepsOffer.numSeleccionadosProjects != newPNumber) {
					stepsOffer.numSeleccionadosProjects = newPNumber
					stepsOffer.PrintSelectedProjStp5();
				}
			});	

		})
	}
}


/**
 * Función que se llama cuando se cargan las publicaciones
 */
function CompletadaCargaRecursosPublicacionesOfertas()
{	
	let currentsIds = []
	if(typeof stepsOffer != 'undefined' && stepsOffer != null && stepsOffer.data != null)
	{		
		$('#ofertaListPublicaciones article.resource h2.resource-title').attr('tagert','_blank')
		// stepsOffer.data.pPersons = $('#ofertaListPublicaciones article.resource').toArray().map(e => {return $(e).attr('id')})
		
		$('#ofertaListPublicaciones article.resource').each((i, e) => {

			currentsIds.push(e.id.split('_')[1])

			if ($(e).find(".custom-checkbox-resource .material-icons").length == 0) {

				if (stepsOffer.data.documents && Object.values(stepsOffer.data.documents).filter(pr => pr.shortId == currentsIds.at(-1)).length > 0) {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource done">
		                <span class="material-icons">done</span>
		            </div>`)
					$(e).addClass('seleccionado')
				}
				else {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource add">
				        <span class="material-icons">add</span>
				    </div>`)
				}
			}
		})

		checkboxResources.init()

		currentsIds.forEach(idDocument => {

			$('#resource_' + idDocument).on("DOMSubtreeModified", function(e) {

				let selector = $(this).find(".custom-checkbox-resource")

				if ($(selector).text().trim() == "done")
				{
					let element = $(this)
					let item = {}
					let arrInfo = []
					let arrUsers = []
					let description = []
					let dates = []
					item.shortId = idDocument
					item.name = element.find('h2.resource-title').text().trim()

					// Obtener la información descriptiva
					element.find('.middle-wrap > .content-wrap > .list-wrap:not(.authors) li').each((i, elem) => {
						arrInfo.push($(elem).text().trim())
					})
					item.info = arrInfo.join(', ')

					// Obtener los investigadores
					element.find('.middle-wrap > .content-wrap > .list-wrap.authors li').each((i, elem) => {
						arrUsers.push($(elem).text().trim())
					})
					item.researchers = arrUsers

					// Obtener la descripción
					element.find('.middle-wrap > .content-wrap > .description-wrap .desc').each((i, elem) => {
						description.push($(elem).text().trim())
					})
					item.description = description.join(', ')

					// Obtener la fecha
					element.find('.middle-wrap > .content-wrap > .content-wrap span').each((i, elem) => {
						dates = ($(elem).text().trim())
					})
					item.dates = dates


					// item.ipNumber = element.data('ipNumber')
					if(stepsOffer.data.documents == null)					
					{
						stepsOffer.data.documents = {};
					}
					stepsOffer.data.documents[idDocument] = item

				} else
				{
					// Borrar investigador del objeto
					if (stepsOffer.data.documents) {
						delete stepsOffer.data.documents[idDocument]
					}
				}

				// Pintamos de nuevo los elementos seleccionados
				let newPNumber = 0
				// Obtengo el número de elementos actuales
				if (stepsOffer.data.documents) {
					newPNumber = Object.keys(stepsOffer.data.documents).length
				} else {
					stepsOffer.numSeleccionadosDocuments = 0
				}
				// Pinto únicamente nuevos elementos si el número de elementos ha cambiado.
				if (stepsOffer.numSeleccionadosDocuments != newPNumber) {
					stepsOffer.numSeleccionadosDocuments = newPNumber
					stepsOffer.PrintSelectedDocuStp5();
				}
			});	

		})
	}
}


/**
 * Función que se llama cuando se cargan las PII
 */
function CompletadaCargaRecursosPIIOfertas()
{	
	let currentsIds = []
	if(typeof stepsOffer != 'undefined' && stepsOffer != null && stepsOffer.data != null)
	{		
		$('#ofertaListPII article.resource h2.resource-title').attr('tagert','_blank')
		// stepsOffer.data.pPersons = $('#ofertaListPII article.resource').toArray().map(e => {return $(e).attr('id')})
		
		$('#ofertaListPII article.resource').each((i, e) => {

			currentsIds.push(e.id.split('_')[1])

			if ($(e).find(".custom-checkbox-resource .material-icons").length == 0) {

				if (stepsOffer.data.pii && Object.values(stepsOffer.data.pii).filter(pr => pr.shortId == currentsIds.at(-1)).length > 0) {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource done">
		                <span class="material-icons">done</span>
		            </div>`)
					$(e).addClass('seleccionado')
				}
				else {
					$(e).prepend(`<div class="custom-control custom-checkbox-resource add">
				        <span class="material-icons">add</span>
				    </div>`)
				}
			}
		})

		checkboxResources.init()

		currentsIds.forEach(idPii => {

			$('#resource_' + idPii).on("DOMSubtreeModified", function(e) {

				let selector = $(this).find(".custom-checkbox-resource")

				if ($(selector).text().trim() == "done")
				{
					let element = $(this)
					let item = {}
					let arrInfo = []
					let arrUsers = []
					let description = []
					let dates = []
					item.shortId = idPii
					item.name = element.find('h2.resource-title').text().trim()

					// Obtener la información descriptiva
					element.find('.middle-wrap > .content-wrap > .list-wrap:not(.authors) li').each((i, elem) => {
						arrInfo.push($(elem).text().trim())
					})
					item.info = arrInfo.join(', ')

					// Obtener los investigadores
					element.find('.middle-wrap > .content-wrap > .list-wrap.authors li').each((i, elem) => {
						arrUsers.push($(elem).text().trim())
					})
					item.researchers = arrUsers

					// Obtener la descripción
					element.find('.middle-wrap > .content-wrap > .description-wrap .desc').each((i, elem) => {
						description.push($(elem).text().trim())
					})
					item.description = description.join(', ')

					// Obtener la fecha
					element.find('.middle-wrap > .content-wrap > .content-wrap span').each((i, elem) => {
						dates = ($(elem).text().trim())
					})
					item.dates = dates


					// item.ipNumber = element.data('ipNumber')
					if(stepsOffer.data.pii == null)					
					{
						stepsOffer.data.pii = {};
					}
					stepsOffer.data.pii[idPii] = item

				}else
				{
					// Borrar investigador del objeto
					delete stepsOffer.data.pii[idPii]
				}

				// Pintamos de nuevo los elementos seleccionados
				let newPNumber = 0
				// Obtengo el número de elementos actuales
				if (stepsOffer.data.pii) {
					newPNumber = Object.keys(stepsOffer.data.pii).length
				} else {
					stepsOffer.numSeleccionadosPII = 0
				}
				// Pinto únicamente nuevos elementos si el número de elementos ha cambiado.
				if (stepsOffer.numSeleccionadosPII != newPNumber) {
					stepsOffer.numSeleccionadosPII = newPNumber
					stepsOffer.PrintSelectedPIIStp5();
				}
			});	

		})
	}
}




// Comportamiento listado investigadores de la oferta
var comportamientoPopupOferta = {
	tabActive: null,

	config: function () {
		var that = this;

		this.printitem = $('#ofertaListUsers')
		this.text_volumen = this.printitem.data('volumen')
		this.text_ajuste = this.printitem.data('ajuste')
		this.text_mixto = this.printitem.data('mixto')

		return;
	},

	init: function (ofertaObj) {
		let that = this
		this.config();
		let paramsCl = this.workCO(ofertaObj)
		let paramsClUrl = this.workCOUrl("searcherPersonsOffers", ofertaObj)
		// let paramsResearchers = this.workCOProfiles(ofertaObj)
		// let researchers = this.setProfiles(ofertaObj)

		buscadorPersonalizado.profile=null;
		buscadorPersonalizado.search='searchOfertaMixto';
		
		// Iniciar el listado de usuarios
		// buscadorPersonalizado.init($('#INVESTIGADORES').val(), "#ofertaListUsers", "searchOfertaMixto=" + paramsCl, null, "profiles=" + JSON.stringify(profiles) + "|viewmode=oferta|rdf:type=person", $('inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		// buscadorPersonalizado.init($('#INVESTIGADORES').val(), "#ofertaListUsers", "searcherPersonsOffers=" + paramsCl, null, "rdf:type=person|roh:isActive=true", $('inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val(), paramsClUrl);
		buscadorPersonalizado.init($('#INVESTIGADORES').val(), "#ofertaListUsers", null, null, "rdf:type=person|roh:isActive=true", $('#inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val(), paramsClUrl);
		
		// Agregamos los ordenes
		// $('.searcherResults .h1-container').after(
		// `<div class="acciones-listado acciones-listado-buscador">
		// 	<div class="wrap">
				
		// 		<div class="ordenar dropdown">
		// 			<a class="dropdown-toggle" data-toggle="dropdown">
		// 				<span class="material-icons">swap_vert</span>
		// 				<span class="texto">${that.text_mixto}</span>
		// 			</a>
		// 			<div class="dropdown-menu basic-dropdown dropdown-menu-right">
		// 				<a href="javascript: void(0)" filter="searchOfertaMixto" class="item-dropdown">${that.text_mixto}</a>
		// 				<a href="javascript: void(0)" filter="searchOfertaVolumen" class="item-dropdown">${that.text_volumen}</a>
		// 				<a href="javascript: void(0)" filter="searchOfertaAjuste" class="item-dropdown">${that.text_ajuste}</a>
		// 			</div>
		// 		</div>
		// 	</div>
		// </div>`);
		
		// $('.acciones-listado-buscador a.item-dropdown').unbind().click(function (e) {
		// 	$('.acciones-listado-buscador .dropdown-toggle .texto').text($(this).text())
		// 	e.preventDefault();
		// 	buscadorPersonalizado.search=$(this).attr('filter');
		// 	if(buscadorPersonalizado.profile==null)
		// 	{
		// 		buscadorPersonalizado.filtro=$(this).attr('filter')+'='+paramsCl;
		// 	}else
		// 	{
		// 		buscadorPersonalizado.filtro=$(this).attr('filter')+'='+paramsResearchers[buscadorPersonalizado.profile];
		// 	}
		// 	FiltrarPorFacetas(ObtenerHash2());
		// });

		//Enganchamos comportamiento grafica seleccionados
		$('#seleccionados-oferta-tab').unbind().click(function (e) {			
			e.preventDefault();
			// newGrafProjClust.CargarGraficaSeleccionados(stepsOffer.data, 'selectedgraphOferta', true);
		});

		return;
	},

	/*
	* Convierte el objeto del oferta a los parámetros de consulta 
	*/
	workCO: function (ofertaObj) {

		let results = null
		if (ofertaObj && ofertaObj.tags) {
			results = ofertaObj.tags.map(itm => "'" + itm + "'").join(',')
		}

		return results
	},
	

	/*
	* Convierte el objeto del oferta a los parámetros de consulta 
	*/
	workCOUrl: function (param, ofertaObj) {

		let results = null
		if (ofertaObj && ofertaObj.tags) {
			results = ofertaObj.tags.map(itm => "'" + itm + "'").join(',')
		}

		return [param + "=" + results]
	},
	
	/*
	* Convierte el objeto del oferta a los parámetros de consulta 
	*/
	workCOProfiles: function (ofertaObj) {
		var dicPerfiles = [];
		stepsOffer.data.profiles.forEach(function(perfil, index) {
			let terms = (perfil.terms.length) ? perfil.terms.map(itm => '<' + itm + '>').join(',') : "<>"
			let tags = (perfil.tags.length) ? perfil.tags.map(itm => "'" + itm + "'").join(',') : "''"
			dicPerfiles[perfil.shortEntityID]=terms + '@@@' + tags
		});
		return dicPerfiles;
	},

	/*
	* Convierte los profiles en json 
	*/
	setProfiles: function (ofertaObj) {

		let results = null
		if (ofertaObj && ofertaObj.profiles) {
			ofertaObj.profiles.forEach((e, i) => {

			})
			results = ofertaObj.profiles.map((e, i) => (
				{
					[e.name.replace(/[^a-z0-9_]+/gi, '-').replace(/^-|-$/g, '').toLowerCase() + "-" + i]: e.name
				}
			))
		}

		return results
	}
};


// Comportamiento listado Proyectos de la oferta
var comportamientoProyectosOferta = {
	tabActive: null,

	config: function () {
		var that = this;
		return;
	},

	init: function (ofertaObj) {
		let that = this
		this.config();
		let paramsCl = this.workCO(ofertaObj)

		buscadorPersonalizado.profile=null;
		
		// Iniciar el listado de usuarios
		// buscadorPersonalizado.init($('#INVESTIGADORES').val(), "#ofertaListProyectos", "searchOfertaMixto=" + paramsCl, null, "profiles=" + JSON.stringify(profiles) + "|viewmode=oferta|rdf:type=person", $('inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		buscadorPersonalizado.init(stepsOffer.txtProyectos, "#ofertaListProyectos", "searcherProyectosValidadasPersonas=" + paramsCl, null, "rdf:type=project|roh:isValidated=true", $('#inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		

		//Enganchamos comportamiento grafica seleccionados
		$('#seleccionados-oferta-tab').unbind().click(function (e) {			
			e.preventDefault();
		});

		return;
	},

	/*
	* Convierte el objeto de la oferta a los parámetros de consulta 
	*/
	workCO: function (ofertaObj) {

		let results = null
		if (ofertaObj && ofertaObj.researchers) {
			results = Object.keys(ofertaObj.researchers).map(itm => "<http://gnoss/" + itm.toUpperCase() + ">").join(',')
		}

		return results
	},
};


// Comportamiento listado publicaciones de la oferta
var comportamientoPublicacionesOferta = {
	tabActive: null,

	config: function () {
		var that = this;
		return;
	},

	init: function (ofertaObj) {
		let that = this
		this.config();
		let paramsCl = this.workCO(ofertaObj)

		buscadorPersonalizado.profile=null;
		
		// Iniciar el listado de usuarios
		// buscadorPersonalizado.init($('#INVESTIGADORES').val(), "#ofertaListPublicaciones", "searchOfertaMixto=" + paramsCl, null, "profiles=" + JSON.stringify(profiles) + "|viewmode=oferta|rdf:type=person", $('inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		buscadorPersonalizado.init(stepsOffer.txtPublicaciones, "#ofertaListPublicaciones", "searcherPublicacionesValidadasPersonas=" + paramsCl, null, "rdf:type=document|roh:isValidated=true", $('#inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		

		return;
	},

	/*
	* Convierte el objeto de la oferta a los parámetros de consulta 
	*/
	workCO: function (ofertaObj) {

		let results = null
		if (ofertaObj && ofertaObj.researchers) {
			results = Object.keys(ofertaObj.researchers).map(itm => "<http://gnoss/" + itm.toUpperCase() + ">").join(',')
		}

		return results
	},
};


// Comportamiento listado PII de la oferta
var comportamientoPIIOferta = {
	tabActive: null,

	config: function () {
		var that = this;
		return;
	},

	init: function (ofertaObj) {
		let that = this
		this.config();
		let paramsCl = this.workCO(ofertaObj)

		buscadorPersonalizado.profile=null;
		
		// Iniciar el listado de usuarios
		// buscadorPersonalizado.init($('#INVESTIGADORES').val(), "#ofertaListPublicaciones", "searchOfertaMixto=" + paramsCl, null, "profiles=" + JSON.stringify(profiles) + "|viewmode=oferta|rdf:type=person", $('inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		buscadorPersonalizado.init(stepsOffer.txtPii, "#ofertaListPII", "searcherPIIPublicosPersonas=" + paramsCl, null, "rdf:type=patent", $('#inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());
		

		return;
	},

	/*
	* Convierte el objeto de la oferta a los parámetros de consulta 
	*/
	workCO: function (ofertaObj) {

		let results = null
		if (ofertaObj && ofertaObj.researchers) {
			results = Object.keys(ofertaObj.researchers).map(itm => "<http://gnoss/" + itm.toUpperCase() + ">").join(',')
		}

		return results
	},
};


// Comportamiento listado Ofertas del gestor otri
var comportamientoOfertasOtri = {
	tabActive: null,

	init: function (pIdUser, idPrint) {

		let that = this

		buscadorPersonalizado.profile=null;

		// Iniciar el listado de ofertas
		buscadorPersonalizado.init(document.getElementById(idPrint).dataset.title, "#"+idPrint, "searchOffersOtri=" + pIdUser, null, "rdf:type=offer", $('inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());

		return;
	}
};


// Comportamiento listado Ofertas del gestor otri
var comportamientoMisOfertas = {
	tabActive: null,

	init: function (pIdUser, idPrint) {

		let that = this

		buscadorPersonalizado.profile=null;

		// Iniciar el listado de ofertas
		buscadorPersonalizado.init(document.getElementById(idPrint).dataset.title, "#"+idPrint, "searchOwnOffers=" + pIdUser, null, "rdf:type=offer", $('#inpt_baseUrlBusqueda').val(), $('#inpt_proyID').val());

		return;
	}
};




/** 
* Objeto que actualiza el estado de las ofertas
*/
cambiarEstadoOfertas = {
	/** 
	* Método de configuración que establece los datos de la próxima llamada
	* @param id, Id (Guid) con la oferta a modificar
	* @param estado, string con el estado al que cambiar
	* @param estadoActual, string con el estado actual al que actualizar
	*/
    config(id, estado, estadoActual) {
        this.arg = {
            pIdOfferId: id,
            estado: estado,
            estadoActual: estadoActual,
            pIdGnossUser: pIdGnossUser,
        };
    },
    borrarOferta() {
        $.post(urlBorrarOferta, this.arg, function (data) {
            location.reload()
        });
    },
    /** 
	* Método que realiza la petición post para la actualización del estado
	*/
    sendPost() {
        $.post(urlCambioEstado, this.arg, function (data) {
            location.reload()
        });
    },
    /** 
	* Método que se llama desde el popup, que realiza el cambio de estado y que contiene el mensaje
	* para la notificación al usuario con el texto introducido en él.
	* @param texto, String con el texto para la notificación
	*/
    sendModal(texto) {
        this.arg.texto = texto
        this.modal.modal('hide')
        this.sendPost()
    },
    /** 
	* Método de inicia el proceso para cambiar un estado 'simple'
	* @param id, Id (Guid) con la oferta a modificar
	* @param estado, string con el estado al que cambiar
	* @param estadoActual, string con el estado actual al que actualizar
	*/
    send(id, estado, estadoActual) {
        this.config(id, estado, estadoActual)
        this.sendPost()
    },
    /** 
	* Método de inicia el proceso para cambiar el estado a un listado de ofertas
	* @param ids, Array de ids (Guid) con la oferta a modificar
	* @param estado, string con el estado al que cambiar
	* @param estadoActual, string con el estado actual al que actualizar
	*/
    sendAll(ids, estado, estadoActual) {
        if (ids && ids.length > 0) {
            // Set init config
            let _arg = {
                pIdOfferIds: ids,
                estado: estado,
                estadoActual: estadoActual,
                pIdGnossUser: pIdGnossUser,
            };
            // Post Call
            $.post(urlCambioEstadoAll, _arg, function (data) {
                location.reload()
            });
        } else {
            alert(ERROR_IDS_VACIO)
        }
    },
    /** 
	* Método de inicia el proceso para cambiar un estado con un mensaje de modal.
	* @param id, Id (Guid) con la oferta a modificar
	* @param estado, string con el estado al que cambiar
	* @param estadoActual, string con el estado actual al que actualizar
	* @param idModal, Id del modal a abrir
	*/
    setModal(id, estado, estadoActual, idModal) {
        this.config(id, estado, estadoActual)
        this.modal = $("#"+idModal)
        this.modal.modal('show')
    }
}

/** 
* Clase que se encarga de añadir los menús de opciones dependiendo de los estados actuales 
*/
class OfferList {

	/** 
	* Constructor de la clase OfferList
	* @param arrLang, Objeto clave valor de literales con traducciones
	*/
	constructor(arrLang) {
		this.arrLang = arrLang
	}

	/** 
	* Método que pinta los menús en cada elemento de búsqueda
	* @param ids, Array con los ids sobre los que pintar el menú
	* @param typeUser, String con el tipo de usuario que accede a la oferta 
	*/
	loadActionsOffer(ids, typeUser = ["otri"]) {

	    ids.forEach(idDocument => {

	        // Obtiene el recurso en el dom
	        let item = document.getElementById("resource_" + idDocument)

	        // Comprueba si existe la oferta
	        if (item) {
	        	// Obtiene el estado del recurso
		        let itemState = item.dataset.estadores
		        // Obtiene el listado de opciones en el menu
		        let selector = item.querySelector(".acciones-recurso-listado .dropdown-menu ul")
		        let menuParent = item.querySelector(".acciones-recurso-listado .dropdown")


		        // Comprueba si devuelve algo para bloquear
		        let htmlRes = this.printMenu(idDocument, itemState, typeUser).join("")
		        if (htmlRes == "") {
		        	menuParent.classList.add("d-none")
		        }
		        // Añade el html al menú el correspondiente elemento
		        selector.innerHTML = htmlRes
	        }
	        
	            
	    })

	}



	/** 
	* Método que obtiene el html de los menús en un elemento
	* @param idDocument, id del documento
	* @param itemState, Estado del elemento en cuestión
	* @param typeUser, tipo/s de usuarios actuales 
	*/
	printMenu(idDocument, itemState, typeUser) {

		let estados = this.setActionsOffer(itemState, typeUser)

		// Devuelve un array con el html de las acciones por cada acción 
	    let htmlRes = estados.map(e => {

            if (e.idEstadoOFerta == "editar") {
                return `
                    <li>
                        <a class="item-dropdown" href="${document.getElementById("inpt_baseUrlBusqueda").value}/nueva-oferta-tecnologica?id=${idDocument}">
                            <span class="material-icons">${e.icono}</span>
                            <span>${e.txtEnviar}</span>
                        </a>
                    </li>`
            } else if (e.targetModalId) {
                return `
                    <li>
                        <a class="item-dropdown" href="javascript: void(0)">
                            <span class="material-icons">${e.icono}</span>
                            <span class="texto" onclick="javascript:cambiarEstadoOfertas.setModal('${idDocument}','${e.idEstadoOFerta}', '${itemState}', '${e.targetModalId}')" >${e.txtEnviar}</span>
                        </a>
                    </li>`
            } else {

                return `
                    <li>
                        <a class="item-dropdown" href="javascript:cambiarEstadoOfertas.send('${idDocument}','${e.idEstadoOFerta}', '${itemState}')">
                            <span class="material-icons">${e.icono}</span>
                            <span class="texto">${e.txtEnviar}</span>
                        </a>
                    </li>`
            }

        })

        return htmlRes

	}



	/** 
	* Método que obtiene un array de acciones dependiendo del estado de una oferta determinada y el usuario logueado actualmente
	* @param estado, String con el estado actual de la oferta
	* @param typeUser, String con el tipo de usuario que accede a la oferta 
	* @return Array, Array de objetos para construir el menú de opciones
	*/
	setActionsOffer(estado, typeUser = ["other"]) {

	    /**
	    * typeUser es el usuario actual, puede contener:
	    * own, es el propio usuario
	    * otri, es el investigador otri
	    * ip, es el investigador principal
	    * other, es otro usuario que no coincide con ninguno de los anteriores
	    */

	    let estados = []
	    let idEstadoOFerta = ""
	    let txtEnviar = ""
	    let icono = "send"
	    let targetModalId = ""

	    if (typeUser.constructor == Array) {


	    	typeUser.forEach(ut => {

			    switch (estado) 
			    {
			        case "http://gnoss.com/items/offerstate_001":
			            // Es el creador de la oferta
			            // Puede pasar la oferta a revisión
			            if (ut == "own") 
			            {
			                idEstadoOFerta = "http://gnoss.com/items/offerstate_002"
			                txtEnviar = this.arrLang["ENVIAR_REVISION"]
			                targetModalId = ""
			                icono = "send"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["ENVIAR_REVISION"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono })
			                }

			            }
			            break;
			        case "http://gnoss.com/items/offerstate_002":
			            // Es el creador de la oferta
			            // Puede pasar la oferta a borrador
			            if (ut == "own" || ut == "otri" || ut == "ip") 
			            {
			                idEstadoOFerta = "http://gnoss.com/items/offerstate_001"
			                txtEnviar = this.arrLang["ENVIAR_BORRADOR"]
			                targetModalId = ""
			                icono = "send"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["ENVIAR_BORRADOR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono })
			                }

			            }
			            // Es el gestor otri
			            // Puede pasar la oferta a borrador
			            if (ut == "otri")
			            {
			                idEstadoOFerta = "http://gnoss.com/items/offerstate_002";
			                txtEnviar = this.arrLang["MEJORAR"]
			                targetModalId = "modal-enviar-comentario"
			                icono = "send"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["MEJORAR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono, targetModalId })
			                }

			                idEstadoOFerta = "http://gnoss.com/items/offerstate_003";
			                txtEnviar = this.arrLang["VALIDAR"]
			                targetModalId = ""
			                icono = "send"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["VALIDAR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono })
			                }

			                idEstadoOFerta = "http://gnoss.com/items/offerstate_004";
			                txtEnviar = this.arrLang["DENEGAR"]
			                targetModalId = "modal-enviar-comentario"
			                icono = "send"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["DENEGAR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono, targetModalId })
			                }

			            }
			            break;
			        case "http://gnoss.com/items/offerstate_003":
			            if (ut == "otri")
			            {
			                idEstadoOFerta = "http://gnoss.com/items/offerstate_005";
			                txtEnviar = this.arrLang["ARCHIVAR"]
			                targetModalId = "modal-enviar-comentario"
			                icono = "delete"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["ARCHIVAR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono, targetModalId })
			                }

			                targetModalId = ""
			                idEstadoOFerta = "http://gnoss.com/items/offerstate_001";
			                txtEnviar = this.arrLang["ENVIAR_BORRADOR"]
			                icono = "send"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["ENVIAR_BORRADOR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono })
			                }
			            }
			            break;
			        case "http://gnoss.com/items/offerstate_004":
			            if (ut == "otri")
			            {
			                idEstadoOFerta = "http://gnoss.com/items/offerstate_005";
			                txtEnviar = this.arrLang["ARCHIVAR"]
			                targetModalId = ""
			                icono = "delete"
			                if
			                 (!estados.find(e => e.txtEnviar == this.arrLang["ARCHIVAR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono })
			                }

			                idEstadoOFerta = "http://gnoss.com/items/offerstate_001"
			                txtEnviar = this.arrLang["ENVIAR_BORRADOR"]
			                targetModalId = ""
			                icono = "send"
			                if (!estados.find(e => e.txtEnviar == this.arrLang["ENVIAR_BORRADOR"])) {
			                	estados.push({ idEstadoOFerta, txtEnviar, icono })
			                }
			            }
			            break;

			    }

			    if (ut == "own" && (estado == "http://gnoss.com/items/offerstate_001" || estado == "http://gnoss.com/items/offerstate_002" || estado == "http://gnoss.com/items/offerstate_003" || estado == "http://gnoss.com/items/offerstate_004"))
			    {
			        idEstadoOFerta = "editar";
			        txtEnviar = this.arrLang["EDITAR"]
			        icono = "edit"
			        targetModalId = ""
	                if (!estados.find(e => e.txtEnviar == this.arrLang["EDITAR"])) {
	                	estados.push({ idEstadoOFerta, txtEnviar, icono, targetModalId })
	                }
			    }

			    if ((ut == "own" || ut == "otri" || ut == "ip") && (estado == "http://gnoss.com/items/offerstate_001" || estado == "http://gnoss.com/items/offerstate_002"))
			    {
			        idEstadoOFerta = "http://gnoss.com/items/offerstate_005";
			        txtEnviar = this.arrLang["BORRAR"]
			        icono = "delete"
			        targetModalId = "modal-eliminar-oferta-confirmacion"
			        if (!estados.find(e => e.txtEnviar == this.arrLang["BORRAR"])) {
			        	console.log("borrar menu añadido", estados.find(e => e.txtEnviar == this.arrLang["BORRAR"]))
	                	estados.push({ idEstadoOFerta, txtEnviar, icono, targetModalId })
	                }
			    } 

	    	})
	    }


	    return estados


	}


}

/**
* Función que añade un elemento después de otro elmento, usando únicamente js
*/
function insertAfter(newNode, existingNode) {
    existingNode.parentNode.insertBefore(newNode, existingNode.nextSibling);
}




/**
* Clase que contiene la funcionalidad del modal de los TAGS para el Oferta
*/
class ModalSearchTagsOffer {
	constructor() {
		this.body = $('body')
		this.modal = this.body.find('#modal-anadir-topicos-oferta')
		this.inputSearch = this.modal.find('#tagsSearchModalOferta')
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
			var urlSTAGSOffer = new URL(servicioExternoBaseUrl + 'servicioexterno/' + uriSearchTags)
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
	 * Método que genera un evento para el botón "guardar" y devuelve el listado de los TAGS añadidas
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
		urlSTAGSOffer.searchParams.set('tagInput', inputVal)

		return new Promise((resolve) => {
			$.get(urlSTAGSOffer.toString(), function (data) {
				resolve(data.filter(itm => !_self.addedTags.includes(itm)))
			});
		})
	}
}




