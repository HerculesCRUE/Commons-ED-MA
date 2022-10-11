![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 1/3/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Hércules EDMA. Servicio externo| 
|Descripción|Servicio Web con funciones propias de Hércules Métodos de Análisis|
|Versión|1|
|Módulo|Documentación|
|Tipo|Código|
|Cambios de la Versión|Versión inicial|

# Servicio Externo

Este servicio es un servicio web sobre el que se apoya la web de **Hércules Métodos de Análisis**, a la que realiza diferentes peticiones vía AJAX. 

El servicio externo se compone a su vez de 3 servicios o funcionalidades bien marcadas:

# OfertasController
La documentación funcional del creador de ofertas tecnológicas está en [Ofertas tecnológicas](https://confluence.um.es/confluence/pages/viewpage.action?pageId=468647949).

Los métodos de éste controlador tendrán la siguiente url:
> https://something.com/servicioexterno/Ofertas/[METODO]

Los métodos son los siguientes:


## [POST] GetThesaurus
Controlador para obtener los thesaurus usados en las ofertas

*Parámetros:*
 - **listThesaurus** *(string[])*: Elemento padre que define los thesaurus a devolver
 - **lang** *(string)*: Idioma del literal devuelto del thesaurus, por defecto 'es'
 
*Devuelve:*
*Object* Diccionario con los datos. (Diccionario clave -> listado de thesaurus)



## [POST] BorrarOferta
Borra una oferta

*Parámetros:*
 - **pIdOfferID** *(string)*: Id de la oferta a borrar
 - **pIdGnossUser** *(Guid)*: Id del usuario que realiza la acción

*Devuelve:*
*Boolean* True o false si ha sido borrado.


## [POST] CambiarEstado
Cambia el estado de una oferta

*Parámetros:*
 - **pIdOfferId** *(string)*: Id de la oferta a modificar
 - **estado** *(string)*: Id del estado al que se quiere establecer
 - **estadoActual** *(string)*: Id del estado que tiene actualmente (Necesario para la modificación del mismo)
 - **pIdGnossUser** *(Guid)*: Id del usuario que modifica el estado, necesario para actualizar el historial
 - **text** *(string)*: Texto de la notificación (Opcional) que contiene el mensaje personalizado para la notificación

*Devuelve:*
*String* Id del nuevo estado.


## [POST] CambiarEstadoAll
Cambiar el estado de un listado de ofertas

*Parámetros:*
 - **pIdOfferIds** *(Guid[])*: Ids (array de GUIDs) de las ofertas a modificar
 - **estado** *(string)*: Id del estado al que se quiere establecer
 - **estadoActual** *(string)*: Id del estado que tiene actualmente (Necesario para la modificación del mismo)
 - **pIdGnossUser** *(Guid)*: Id del usuario que modifica el estado, necesario para actualizar el historial
 - **text** *(string)*: Texto de la notificación (Opcional) que contiene el mensaje personalizado para la notificación

*Devuelve:*
*Bool* True o false indicando si se han hecho los cambios o no.


## [GET] LoadOffer
Controlador para cargar los datos de la oferta

*Parámetros:*
 - **pIdOfferId** *(string)*: Id de la oferta a borrar
 
*Devuelve:*
*Object* Objeto "leible" de la oferta.


## [GET] LoadUsers
Controlador para Obtener los usuarios del/los grupos de un investigador

*Parámetros:*
 - **pIdUserId** *(string)*: Usuario investigador
 
*Devuelve:*
*Object* Diccionario con los datos necesarios para cada persona.


## [POST] LoadLineResearchs
Controlador para Obtener las líneas de invetigación de los grupos de los usuarios investigadores dados

*Parámetros:*
 - **pIdUsersId** *(string[])*: Usuarios investigadores
 
*Devuelve:*
*string[]* Listado de las líneas de investigación.



## [GET] LoadFramingSectors
Controlador para Obtener los sectores de encuadre

*Parámetros:*
 - **lang** *(string)*: Idioma a cargar
 
*Devuelve:*
*string[]* Listado de las líneas de investigación.



## [GET] LoadMatureStates
Controlador para Obtener los estados de madurez de las ofertas tecnológicas

*Parámetros:*
 - **lang** *(string)*: Idioma a cargar
 
*Devuelve:*
*string[]* Listado de los estados de madurez.



## [POST] SaveOffer
Controlador para crear/actualizar los datos de la oferta

*Parámetros:*
 - **pIdGnossUser** *(Guid)*: Usuario de gnoss que realiza la acción
 - **oferta** *(Object)*: Objeto con la oferta tecnológica a crear/actualizar
 
*Devuelve:*
*string* Id de la oferta creada o modificada.



## [POST] GetUserProfileInOffer
Controlador que lista el perfil de usuarios al que pertenece el usuario actual respecto a una oferta tecnológica dada 

*Parámetros:*
 - **pIdOfertaId** *(string)*: Id de la oferta tecnológica
 - **userId** *(Guid)*: Usuario de gnoss que realiza la acción
 
*Devuelve:*
*Object* Objeto json




# ClusterController
La documentación funcional del creador de los clusters está en [Asistente para la creación de cluster (equipo de proyecto)](https://confluence.um.es/confluence/pages/viewpage.action?pageId=398786801).

Los métodos de éste controlador tendrán la siguiente url:
> https://something.com/servicioexterno/Cluster/[METODO]


## [GET] GetThesaurus
Controlador para obtener los thesaurus usados en el cluster

*Parámetros:*
 - **listThesaurus** *(string)*: Elemento padre que define el thesaurus, por defecto un texto vacío.
 
*Devuelve:*
*Object* Diccionario con los datos. (Diccionario clave -> listado de thesaurus)




## [POST] SaveCluster
Controlador para crear/actualizar los datos del cluster

*Parámetros:*
 - **pIdGnossUser** *(string)*: Usuario de gnoss que realiza la acción
 - **pDataCluster** *(Object)*: Objeto con el cluster a crear/actualizar
 
*Devuelve:*
*string* Id del cluster creado o modificado.




## [GET] LoadCluster
Controlador para cargar los datos de un cluster

*Parámetros:*
 - **pIdClusterId** *(string)*: Id del cluster
 
*Devuelve:*
*Object* Objeto con el contenido del cluster.



## [POST] LoadProfiles
Controlador para cargar los perfiles de cada investigador sugerido del cluster

*Parámetros:*
 - **pDataCluster** *(Object)*: Datos del cluster
 - **pPersons** *(string[])*: Listado de personas sobre los que pedir información
 
*Devuelve:*
*Object* Diccionario con los datos necesarios para cada persona por cluster.


## [POST] LoadSavedProfiles
Controlador para cargar la configuración de los perfiles de todos los clusters de un usuario de la web

*Parámetros:*
 - **pIdUser** *(Object)*: Id del usuario
 - **loadSavedProfiles** *(bool)*: Booleano que determina si cargamos los investigadores de cada perfil
 
*Devuelve:*
*Objects* Listado con los datos necesarios de los clusters y sus perfiles.




## [POST] DatosGraficaColaboradoresCluster
Controlador que obtiene el objeto para crear la gráfica tipo araña de las relaciones entre los perfiles seleccionados en el cluster

*Parámetros:*
 - **pCluster** *(Object)*: Cluster con los datos de las personas sobre las que realizar el filtrado de áreas temáticas
 - **pPersons** *(string[])*: Personas sobre las que realizar el filtrado de áreas temáticas (Por si se envía directamente)
 - **seleccionados** *(bool)*: Determina si se envía el listado de personas desde el cluster o desde las personas
 
*Devuelve:*
*Object* Objeto que se trata en JS para construir la gráfica.




## [POST] DatosGraficaAreasTematicasCluster
Controlador que obtiene los datos para crear la gráfica de áreas temáticas

*Parámetros:*
 - **pPersons** *(string[])*: Personas sobre las que realizar el filtrado
 
*Devuelve:*
*Object* Objeto que se trata en JS para construir la gráfica.




## [POST] BorrarCluster
Controlador que borra un cluster

*Parámetros:*
 - **pIdClusterId** *(string)*: Id del Cluster a borrar
 
*Devuelve:*
*bool* 'true' o 'false' si ha sido borrado o no.




## [GET] SearchTags
Controlador que sugiere etiquetas con la búsqueda dada

*Parámetros:*
 - **tagInput** *(string)*: Texto para la búsqueda de etiquetas
 
*Devuelve:*
*string[]* Listado de las etiquetas de resultado




# RosVinculadosController
La documentación funcional de los ROs vinculados está en [ROs vinculados](https://confluence.um.es/confluence/display/HERCULES/ROs+vinculados).

Los métodos de éste controlador tendrán la siguiente url:
> https://something.com/servicioexterno/RosVinculados/[METODO]

Los métodos son los siguientes:


## [POST] DeleteLinked
Borra un vínculo

*Parámetros:*
 - **resourceRO** *(string)*: Id (Guid) del RO relacionado
 - **pIdROId** *(string)*: Id (Guid) del RO a eliminar de vinculados
 - **pIdGnossUser** *(string)*: Id del usuario que realiza la acción
 
*Devuelve:*
*Bool* 'true' or 'false' si ha sido borrado



## [GET] LoadRosLinked
Controlador para Obtener los ROs vinculados de un RO en concreto

*Parámetros:*
 - **pIdOfferID** *(string)*: ID del RO a obtener las relaciones
 - **pIdGnossUser** *(string)*: Idioma de los literales para la consulta, por defecto "es"

*Devuelve:*
*Objects* Listado de los RO vinculados.




## [POST] SearchROs
Controlador para Obtener los ROs vinculados de un RO en concreto

*Parámetros:*
 - **text** *(string)*: String a buscar
 - **pIdGnossUser** *(string)*: Id del usuario que modifica el estado, necesario para actualizar el historial
 - **listItemsRelated** *(string[])*: Ids de ROs seleccionados

*Devuelve:*
*Objects* Listado de los RO vinculados.




## [POST] AddLink
Controlador para crear una vinculación

*Parámetros:*
 - **resourceRO** *(string)*: Id (Guid) del RO relacionado
 - **pIdROId** *(string)*: Id (Guid) del RO a añadir a vinculados
 - **pIdGnossUser** *(Guid)*: Id del usuario que realiza la acción

*Devuelve:*
*Objects* Id de la oferta creada o modificada.







# AnotacionesController
La documentación funcional de los ROs vinculados está en [Anotaciones](https://confluence.um.es/confluence/display/HERCULES/Anotaciones+sobre+los+ROs).

Los métodos de éste controlador tendrán la siguiente url:
> https://something.com/servicioexterno/Anotaciones/[METODO]

Los métodos son los siguientes:


## [POST] GetOwnAnnotationsInRO
Controlador para obtener las anotaciones de un investigador en un RO concreto.
Los valores posibles de la ontología serían actualmente:
"http://purl.org/ontology/bibo/Document", "document"
"http://w3id.org/roh/ResearchObject", "researchobject"

*Parámetros:*
 - **idRO** *(string)*: Id del RO
 - **idUser** *(string)*: Id del usuario
 - **rdfType** *(string)*: rdfType de la ontología
 - **ontology** *(string)*: Nombre de la ontología
 
*Devuelve:*
*Objects* Diccionario con los datos



## [POST] CreateNewAnnotation
Controlador para crear una nueva anotación
Los valores posibles de la ontología serían actualmente:
"http://purl.org/ontology/bibo/Document", "document"
"http://w3id.org/roh/ResearchObject", "researchobject"

*Parámetros:*
 - **idRO** *(string)*: Id del RO
 - **idUser** *(string)*: Id del usuario
 - **rdfType** *(string)*: rdfType de la ontología
 - **ontology** *(string)*: Nombre de la ontología
 - **texto** *(string)*: Texto de la anotación
 - **idAnnotation** *(string)*: Id de la anotación (si se guarda) (Puede ser nulo)
 
*Devuelve:*
*Objects* Diccionario con los datos



## [POST] DeleteAnnotation
Metodo para eliminar una anotacion

*Parámetros:*
 - **idAnnotation** *(string)*: Id de la anotacion a eliminar
 
*Devuelve:*
*Bool* 'true' o 'false' si ha sido eliminada



# HerculesController
Es el encargado principalmente de obtener los datos de las gráficas en las diferentes fichas, como puedan ser las fichas de los investigadores, los grupos de investigación, las publicaciones, etc...

## RedesUsuarioController
Controlador para obtener y modificar los datos de las fuentes de *ResearchObjects* (Otros objetos de investigación).

## SearchController
La documentación funcional del metabuscador está en [Hércules MA - Metabuscador](https://confluence.um.es/confluence/display/HERCULES/MA.+Metabuscador).

Este controlador se encarga de las peticiones para realizar una búsqueda de los datos almacenados en memoria de las entidades, o también llamado **metabuscador**:
 - Investigadores
 - Grupos de investigación
 - Proyectos
 - Publicaciones
 - Otros objetos de investigación

Un ejemplo de una petición sería:

> https://something.com/servicioexterno/Search/DoMetaSearch?stringSearch=skarmeta&lang=es

Usando un terminal:
   ```bash
curl -X 'GET' \
  'https://something.com/servicioexterno/Search/DoMetaSearch?stringSearch=skarmeta&lang=es' \
  -H 'accept: */*'
```

Donde los parámetros serían **stringSearch**, que sería la cadena de texto a buscar, y **lang** el idioma de búsqueda.

## Otras funcionalidades

También posee la funcionalidad de cargar en memoria periódicamente un listado con los objetos de las diferentes entidades buscables por el metabuscador. Este proceso se iniciará al iniciar el servicio y periódicamente se volverá a ejecutar cargando los nombres, descripciones, urls, tags y autores (según corresponda a cada tipo de entidad) necesarios para realizar la búsqueda en a través del SearchController.
