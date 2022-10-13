![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 10/10/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Información de los archivos javascript y css|
|Descripción|Información de funcionamiento y utizacíón de los archivos javascript y css del sitio|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# annotations.js
Archivo encargado de la funcionalidad de las anotaciones.

Contiene las siguientes clases y funciones:

- **Clase CargarAnotaciones**: Clase que se encarga de cargar y pintar las anotaciones en los diferentes ROs

## Relaciones:
Este archivo se relaciona con 
- [**document.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/document.cshtml) - Ficha de las publicaciones
- [**researchobject.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/researchobject.cshtml) - Ficha de los Research Objects

---
# cluster.css
Archivo css encargado de los estilos de css para el cluster

---
# cluster.js
Archivo js encargado de la funcionalidad js para el cluster (Mayormente del asistente de creación y edición del mismo).

## Clases y funciones:
- **Clase StepsCluster**: Clase encargada del funcionamiento del creador / editor de los clusters
- **Clase CargarGraficaProjectoClusterObj**: Clase para las trabajar en las gráficas de los colaboradores en el cluster
- **Función actualizarTypesClusterOcultar**: Función a la que se llama para seleccionar qué elementos de las relaciones mostrar
- **Función actualizarTypesClusterOcultarSE**: Función a la que se llama para seleccionar qué elementos de las relaciones mostrar en los investigadores seleccionados
- **Función ActualizarGraficaClusterolaboradoresCluster**: función para actualizar la gráfica de colaboradores
- **Objeto comportamientoPopupCluster**: Objeto para el funcionamiento de los usuarios disponibles para los clusters
- **Clase ModalSearchTags**: Clase que contiene la funcionalidad del modal de los TAGS para el Cluster
- **Función CompletadaCargaRecursosCluster**: Función que se llama cuando se cargan los investigadores en el cluster
- **Función CompletadaCargaFacetasCluster**: Función que se llama cuando se cargan las facetas de los investigadores en el cluster

## Relaciones:
### Se relaciona con las vistas: 
- Vista del asistente de edición y creación de un cluster [**_Destacado_creacioncluster$$$f69da06a-9211-4722-abb6-a70bffb0a41e.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_creacioncluster$$$f69da06a-9211-4722-abb6-a70bffb0a41e.cshtml) - Esta vista posee el html del creador y edición de los clusters, junto sus popups y el iniciador js para el asistente del cluster
- Vista del recurso [**cluster.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/cluster.cshtml) - Página del recurso del cluster

### Se relaciona con los servicios:
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/src/Hercules.CommonsEDMA.ServicioExterno#clustercontroller) - Servicio encargado de varias funcionalidades respecto a la web, entre otras las referentes a la funcionalidad de los clusters
- **Servicio de resultados**
- **Servicio de facetas**

### Se relaciona con otros archivos js:
- Archivo de la ficha de los recursos [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - En este caso, se usa para realizar las peticiones y mostrar los resultados de la búsqueda de resultados de los investigadores.



---
# community_proy.css
Hoja de estilos personalizada sobre los estilos de la comunidad Hércules

---
# community_proy.js
Archivo js encargado de sobrescribir parte del funcionamiento de community.js y añadir alguna otra función transversal al sitio
## Clases y funciones:
- **Función GetText**
- **Objeto cargarCVId**
- **Función enlazarFiltrosBusqueda**
- **Función comportamientoCargaFacetasComunidad**
- **Función setFilter**
- **Función setFilterNumbers**
- **Función setFilterButtons**
- **Función changeSliderVals**
- **Función comportamientoRangosNumeros**
- **Función CompletadaCargaRecursosComunidad**
- **Función comportamientoFacetasPopUp.init**
- **Función comportamientoFacetasPopUp.config**
- **Función comportamientoFacetasPopUp.eliminarAcentos**
- **Función comportamientoFacetasPopUp.cargarFaceta**
- **Función comportamientoFacetasPopUp.buscarFacetas**
- **Objeto MontarResultadosScroll**
- **Función montarTooltip.lanzar**
- **Función tooltipsAccionesRecursos.getTooltipQuotes**
- **Función tooltipsAccionesRecursos.getTooltipHindex**
- **Objeto montarTooltipCode**
- **Función MontarResultados**
- **Función FiltrarPorFacetasGenerico**
- **Función AgregarFaceta**
- **Objeto metabuscador**
- **Función setCacheWithExpiry**
- **Función getCacheWithExpiry**
- **Función GetFuentesExternas**
- **Función PedirFuentesExternas**
- **Función GetSexenios**
- **Función PedirSexenio**
- **Función GetAcreditaciones**
- **Función PedirAcreditacion**
- **Función menusLateralesManagement.montarMenuLateralMetabuscador**
- **Función tooltipsImpactFactor**
- **Función tooltipMatching**
- **Función comportamientoVerMasVerMenosTags.comportamiento**
- **Función cleanStringUrlLikeGnoss**
- **Función cleanStringUrl**
- **Función removeAccents**
- **Función CheckIsOtri**
- **Función EnhableIfIsOtri**
- **Función CheckIfCallIsOtri**


## Relaciones:
### Se relaciona con los servicios:
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.

---
# community.css
Archivo css encargado de los estilos de css para numerosas secciones de la comunidad

---
# community.js
Archivo js encargado de la funcionalidad js la mayoría de las funcionalidades "estándar" del javascript de la comunidad. 

## Clases y funciones:
- **Objeto clonarMenuUsuario**:
- **Objeto accionesBuscadorCabecera**:
- **Objeto menusLateralesManagement**:
- **Objeto metabuscador**:
- **Objeto communityMenuMovil**:
- **Objeto accionesPlegarDesplegarModal**:
- **Objeto iniciarComportamientoImagenUsuario**:
- **Objeto iniciarDatepicker**:
- **Objeto collapseResource**:
- **Objeto comportamientoVerMasVerMenos**:
- **Objeto comportamientoVerMasVerMenosTags**:
- **Objeto edicionListaAutorCV**:
- **Objeto operativaFormularioAutor**:
- **Objeto operativaFormularioTesauro**:
- **Objeto operativaFormularioProduccionCientifica**:
- **Objeto comportamientoTopicosCV**:
- **Objeto mostrarFichaCabeceraFixed**:
- **Objeto clonarNombreFicha**:
- **Objeto montarTooltip**:
- **Objeto tooltipsAccionesRecursos**:
- **Objeto tooltipsCV**:
- **Objeto cambioTraducciones**:
- **Objeto contarLineasDescripcion**:
- **Objeto comportamientoAbrirArbol**:
- **Objeto operativaModalSeleccionarTemas**:
- **Objeto importarCVN**:
- **Objeto checkboxResources**:
- **Objeto pintarGraficos**:
- **Función comportamientoCargaFacetasComunidad**:

---
# communityAlternativa.css
Archivo css con ajustes mínimos. 

---
# edicioncv.css
Archivo css con el css encargado de la edición del CV. 


---
# edicioncv.js
Archivo js encargado del funcionamiento de la sección de la página de edición del CV. 
## Clases y funciones:
- **Objeto edicionCV**
- **Objeto duplicadosCV**
- **Función EliminarAcentos**
- **Función addAutocompletar**
- **Función TransFormData**
- **Función RandomGuid**
- **Función iniciarComportamientoImagenUsuario**
- **Función objectToFormData**
- **Función $.imageDropArea2**
- **Función $.fn.imageDropArea2**
- **Función $.fn.autocomplete**
- **Función $.fn.autocomplete**
- **Función $.fn.result**
- **Función $.fn.search**
- **Función $.fn.flushCache**
- **Función $.fn.setOptions**
- **Función $.fn.unautocomplete**
- **Función $.Autocompleter**
- **Función QuitarContadores**
- **Función $.Autocompleter.defaults**
- **Función $.Autocompleter.Cache**
- **Función $.Autocompleter.Select**
- **Función $.fn.selection**
- **Función PintarTags**
- **Función PosicionarTextBox**
- **Función LimpiarTags**
- **Función ActualizarTag**
- **Función EliminarTag**
- **Función descartarTag**
- **Función posicionarCursor**
- **Función pintarTagsInicio**
- **Función cancelEvent**
- **Función ObtenerUrlMultiple**
- **Función aleatorio**
- **Función getParam**
- **Función tooltipsAccionesRecursos.lanzar**
- **Función montarTooltip.lanzar**
- **Función cambioTraducciones.comportamiento**
- **Función accionesPlegarDesplegarModal.collapse**
- **Función edicionListaAutorCV**
- **Función tooltipsCV.traducciones**
- **Función selectionChange**
- **Función operativaFormularioProduccionCientifica.modal**
- **Función operativaFormularioProduccionCientifica.formProyecto**
- **Función conseguirTesauro**
- **Función pintadoTesauro**
- **Función mostrarNotificacion**
- **Función pintadoEtiquetas**

## Relaciones:
### Se relaciona con los servicios:
- **Servicio de resultados**
- **Servicio de facetas**
- [**Hercules.ED.LoadCVs**](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.LoadCVs) - Carga del CV
- [**Hercules.ED.EditorCV**](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.EditorCV) - Configuración del Editor de CV

---
# editorWysiwyg.css
Hoja de estilos encargada de dar formato al editor de texto WYSIWYG

---
# editorWysiwyg.js
Página javascript encargada de la funcionalidad de los editores wysiwyg que se miestran en la oferta tecnológica y en el editor cv
## Clases y funciones:
- **Clase TextField**: Clase Text field, le da la funcionalidad a cada editor de texto wysiwyg 

## Relaciones:
### Se relaciona con otros archivos js:
- Es llamado desde **offer.js**
- Es llamado desde **editorcv.js**

---
# exportacioncv.js
Archivo de javascript que sirve para la exportación de los currículums
## Clases y funciones:
- **Objeto exportacionCV**
- **Función cambiarTipoExportacion**
- **Función asignarAccionesBotonesPerfil**
- **Función getExportationProfile**
- **Función loadExportationProfile**
- **Función deleteExportationProfile**
- **Función addExportationProfile**
- **Función checkAllCVWrapper**
- **Función printCientificProduction**
- **Función printFreeText**
- **Función edicionCV.printTab**
- **Función edicionCV.printPersonalData**
- **Función edicionCV.printTabSection**
- **Función edicionCV.printHtmlListItems**
- **Función edicionCV.printHtmlListItem**

---
# ficharecurso.js
JS común a todas las fichas con buscadores, es necesario para realizar las peticiones de buscadores personalizados, funciones de las gráficas, personalizaciones en las fichas, etc...

## Clases y funciones:
- **Objeto buscadorPersonalizado**
- **Clase CargarGraficaProjectoObj**
- **Función PintarGraficaPublicaciones**
- **Función PintarGraficaProyectos**
- **Función AjustarGraficaArania**
- **Función zoomCyIn**
- **Función zoomCyOut**
- **Función PintarGraficaArania**
- **Función PintarGraficaAraniaVersionCircle**
- **Función PintarGraficaAreasTematicas**
- **Función filtrarSearch**
- **Función FiltrarPorFacetas**
- **Función VerFaceta**
- **Función comportamientoCargaFacetasComunidad**
- **Función SubirPagina**
- **Función ResetearURL**
- **Función MontarResultadosScroll.peticionScrollResultados**

## Relaciones:
### Se relaciona con las vistas: 
- [**cluster.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/cluster.cshtml) - Ficha de los clusters
- [**group.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/group.cshtml) - Ficha de los grupos de investigación
- [**document.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/document.cshtml) - Ficha de las publicaciones
- [**offer.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/offer.cshtml) - Ficha de los recursos 
- [**person.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/person.cshtml) - Ficha del personal investigador
- [**project.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/project.cshtml) - Ficha de los proyectos
- [**researchobject.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Recursos/researchobject.cshtml) - Ficha de los Research Objects
- [**_ConsultaSPARQL_listadoofertas$$$e4b0f260-6a21-4d2a-8214-517fbd0fa414.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/ConsultaSPARQL/_ConsultaSPARQL_listadoofertas$$$e4b0f260-6a21-4d2a-8214-517fbd0fa414.cshtml) - Personalización de una página del CMS para el listado de las ofertas tecnológicas validadas. Se llama al fichero desde esta vista
- [**_Destacado_listado-mis-ofertas$$$cc429e2d-69b5-4e2f-86f1-8e4589f07e5c.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_listado-mis-ofertas$$$cc429e2d-69b5-4e2f-86f1-8e4589f07e5c.cshtml) - Página para mostrar el listado de las ofertas tecnológicas creadas por ti o por un investigador del que eres IP. Se llama al fichero desde esta vista
- [**_Destacado_gestor-ofertas$$$fb9e53e9-879c-4680-9796-fe5543bd6be4.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_gestor-ofertas$$$fb9e53e9-879c-4680-9796-fe5543bd6be4.cshtml) - Página para gestionar las ofertas tecnológicas como gestor otri. Se llama al fichero desde esta vista


### Se relaciona con los servicios:
- **Servicio de resultados**
- **Servicio de facetas**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.

### Se relaciona con otros archivos js:
- Es llamado desde **offer.js**
- Es llamado desde **cluster.js**
- Hay referencias y llamadas a este archivo en **community_proy.js**
- Hay referencias y llamadas a este archivo en **ficharecurso.js**
- Js llamado desde el javascript establecido en las vistas antes indicadas

---
# graficas_proy.js
Javascript que se encarga de pintar las gráficas "editables" de la página (página de indicadores)
## Clases y funciones:
- **Clase Pagina**
- **Clase GraficaBase**
- **Clase GraficaNodos**
- **Clase GraficaBarras**
- **Clase GraficaHorizontal**
- **Clase GraficaVertical**
- **Clase GraficaCircular**
- **Función getGrafica2**
- **Función getPages**
- **Función getPagesUser**
- **Función pintarGraficaIndividual**
- **Función comportamientos**
- **Función pintarAccionesMapa**
- **Función cerrarModal**


## Relaciones:
### Se relaciona con otros archivos js:
- Se usa **graphic-engine.js** para ser funcional
### Se relaciona con las vistas: 
-  [**_Destacado_indicadorespersonales$$$2cd4955f-e19c-4a32-8530-4cae167796af.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_indicadorespersonales$$$2cd4955f-e19c-4a32-8530-4cae167796af.cshtml) - Página de los indicadores personales

---
# graphic-engine.css
Hoja de estilos de la página de indicadores
## Relaciones:
### Se relaciona con las vistas: 
-  [**_Destacado_indicadorespersonales$$$2cd4955f-e19c-4a32-8530-4cae167796af.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_indicadorespersonales$$$2cd4955f-e19c-4a32-8530-4cae167796af.cshtml) - Página de los indicadores personales

---
# graphic-engine.js
Motor js para las gráficas de indicadores y otras gráficas dónde se use la personalización del mismo.

## Clases y funciones:
- **Objeto metricas**
- **Función comportamientoFacetasPopUp.cargarFaceta**

## Relaciones:
### Se relaciona con las vistas: 
-  [**_Destacado_indicadorespersonales$$$2cd4955f-e19c-4a32-8530-4cae167796af.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_indicadorespersonales$$$2cd4955f-e19c-4a32-8530-4cae167796af.cshtml) - Página de los indicadores personales

### Se relaciona con otros archivos js:
- Es llamado desde **graficas_proy.js**
---
# importacioncv.js
Js para la importación de los CVs en formato CVN y en PDF.

## Clases y funciones:
- **Función preventBeforeUnload**
- **Objeto importarCVN**
- **Función changeSelector**
- **Función dropdownVisibilityCV**
- **Función changeUniqueItem**
- **Función checkUniqueItems**
- **Función checkAllWrappersCV**
- **Función aniadirComportamientoWrapperSeccion**
- **Función aniadirTooltipsConflict**
- **Función wrapperVisibilitySection**
- **Función checkAllWrappersSection**
- **Función checkAllConflict**
- **Función checkAllCVWrapper**
- **Función printCientificProduction**
- **Función edicionCV.printTab**
- **Función edicionCV.printPersonalData**
- **Función edicionCV.printTabSection**
- **Función edicionCV.printHtmlListItem**

## Relaciones:
### Se relaciona con las vistas: 
-  [**_Destacado_importadorcv$$$64a939ed-0716-4b96-b24e-af622af23ce3.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_importadorcv$$$64a939ed-0716-4b96-b24e-af622af23ce3.cshtml) - Página para la importación de los CVs
### Se relaciona con los servicios: 
-  [**Hercules.ED.ImportExportCV**](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.ImportExportCV) - Servicio encargado de importación y exportación de curriculums vitae (CV) en formato CVN

---
# offer.js
Js para crear y editar las ofertas tecnológicas, así mismo servirá para la gestión del control de los estados de las ofertas tecnológicas

## Clases y funciones:

- **Clase StepsOffer**
- **Clase ModalCategoryCreator**
- **Función CompletadaCargaRecursosInvestigadoresOfertas**
- **Función CompletadaCargaRecursosProyectosOfertas**
- **Función CompletadaCargaRecursosPublicacionesOfertas**
- **Función CompletadaCargaRecursosPIIOfertas**
- **Objeto comportamientoPopupOferta**
- **Objeto comportamientoProyectosOferta**
- **Objeto comportamientoPublicacionesOferta**
- **Objeto comportamientoPIIOferta**
- **Objeto comportamientoOfertasOtri**
- **Objeto comportamientoMisOfertas**
- **Objeto cambiarEstadoOfertas**
- **Clase OfferList**
- **Función insertAfter**
- **Clase ModalSearchTagsOffer**
## Relaciones:
### Se relaciona con las vistas: 
- [**_Destacado_gestor-ofertas$$$fb9e53e9-879c-4680-9796-fe5543bd6be4.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_gestor-ofertas$$$fb9e53e9-879c-4680-9796-fe5543bd6be4.cshtml) - Vista encargada de la gestión de las ofertas tecnológicas por el usuario otri
- [**_Destacado_creacionoferta$$$9fb0b25d-3156-417d-a802-4092ab6d2a2c.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_creacionoferta$$$9fb0b25d-3156-417d-a802-4092ab6d2a2c.cshtml) - Vista encargada de asistente de creación y edición de las ofertas tecnológicas
- [**_Destacado_listado-mis-ofertas$$$cc429e2d-69b5-4e2f-86f1-8e4589f07e5c.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_listado-mis-ofertas$$$cc429e2d-69b5-4e2f-86f1-8e4589f07e5c.cshtml) - Vista encargada mostrar el listado de "mis ofertas tecnológicas"

### Se relaciona con los servicios: 
- **Servicio de resultados**
- **Servicio de facetas**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.

---
# redes.js
Js encargado del funcionamiento para la página de configuración de los Research Objects de cada usuario.

## Clases y funciones:
- **Función GuardarDatos**
- **Objeto traducir**

## Relaciones:
### Se relaciona con las vistas: 
- [**_Destacado_redes$$$fd5352c6-5f78-4254-85b9-d7df7976aa9a.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/CMS/Destacado/_Destacado_redes$$$fd5352c6-5f78-4254-85b9-d7df7976aa9a.cshtml#) 

### Se relaciona con los servicios: 
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.

---

# theme.css
Css encargado de pintar los estilos para el tema
---

# theme.js
Js encargado de las numerosas funcionalidades del sitio 

## Clases y funciones:

- **Objeto bodyScrolling**
- **Objeto operativaFullWidth**
- **Objeto menusLateralesManagement**
- **Objeto clonarMenuUsuario**
- **Objeto sacarPrimerasLetrasNombre**
- **Objeto obtenerClaseBackgroundColor**
- **Objeto circulosPersona**
- **Objeto filtrarMovil**
- **Objeto metabuscador**
- **Objeto buscadorSeccion**
- **Objeto listadoMensajesAcciones**
- **Objeto accionDropdownAutofocus**
- **Objeto cambioVistaListado**
- **Objeto masHeaderMensaje**
- **Objeto plegarFacetasCabecera**
- **Objeto facetasVerMasVerTodos**
- **Objeto comportamientosModalFacetas**
- **Objeto plegarSubFacetas**
- **Objeto alturizarBodyTamanoFacetas**
- **Objeto calcularPaddingTopMain**
- **Objeto desplegarDestinatarios**
- **Objeto addCustomScrollBar**
- **Objeto accionesRecurso**
- **Objeto accionHistorial**
- **Objeto accionDesplegarCategorias**
- **Objeto iniciarSelects2**
- **Objeto iniciarDropAreaImagenPerfil**
- **Objeto customizarAvisoCookies**
- **Objeto modificarCabeceraOnScrolling**
- **Función mostrarNotificacion**
- **Función comportamientoCargaFacetasComunidad**
- **Función MostrarUpdateProgress**
- **Función MostrarUpdateProgressTime**
- **Función OcultarUpdateProgress**

---
