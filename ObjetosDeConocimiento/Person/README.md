![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 02/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Persona| 
|Descripción|Descripción del objeto de conocimiento Persona para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Persona

La entidad foaf:Persona (ver Figura 1) representa a todas aquellas personas, externas o afiliadas a la Universidad de Murcia, que tienen cabida en la plataforma Hércules. La especialización de dicha entidad incorpora algunas propiedades de los dominios de investigación, además de las correspondientes a la ontología fundamental empleada en ASIO.
Por otra parte, se han añadido ciertas propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura Hércules EDMA.

Las propiedades son las siguientes:

- eroh:publicationsNumber
- eroh:publicPublicationsNumber
- eroh:projectsNumber
- eroh:publicProjectsNumber
- eroh:publicCollaboratorsNumber
- foaf:gender
- roh:crisIdentifier
- eroh:isOtriManager
- eroh:isGraphicManager
- foaf:firstName
- foaf:lastName
- foaf:nick
- vivo:description
- roh:ORCID
- foaf:name
- roh:hasRole
- eroh:isSynchronized
- vivo:scopusId
- roh:hasPosition
- vivo:departmentOrSchool
- roh:h-index
- vivo:hasResearchArea
- vivo:relates
- foaf:homepage
- vcard:hasTelephone
- vcard:email
- vcard:address
- eroh:lineResearch
- vivo:researchedId
- eroh:semanticScholarId
- eroh:isActive
- eroh:themedAreasNumber
- eroh:lastUpdatedDate
- eroh:gnossUser
- eroh:usuarioFigShare
- eroh:tokenFigShare
- eroh:usuarioGitHub
- eroh:tokenGitHub
- eroh:isIPGroupActually
- eroh:isIPGroupHistorically
- eroh:isIPProjectActually
- eroh:isIPProjectHistorically
- eroh:ipNumber
- eroh:metricPage
- eroh:useMatching

En ROH, foaf:Person está basada en FOAF (Friend of a Friend) y sigue los patrones empleados en VIVO. Ello explica la inclusión de ciertas propiedades FOAF básicas como foaf:firstName, foaf:lastName, foaf:nick, foaf:name y foaf:homepage. Se han adoptado otras relaciones de importancia acordes al formato común de información en materia de la investigación europea (CERIF), como roh:crisIdentifier, roh:hasRole, roh:h-index o roh:ORCID.

Una instancia de foaf:Person se asocia con las siguientes entidades a través de propiedades de objeto:
- foaf:Organization, que vincula a un investigador con una determinada organización.
- vivo:Department, que vincula a un investigador con un departamento o escuela universitaria concreta.
- roh:CategoryPath, que vincula las áreas temáticas en las que un investigador desarrolla su trabajo.
- foaf:Group, que vincula al investigador con uno o varios grupos de investigación.

![](../../Docs/media/ObjetosDeConocimiento/Person.png)

*Figura 1. Diagrama ontológico para la entidad foaf:Person*
