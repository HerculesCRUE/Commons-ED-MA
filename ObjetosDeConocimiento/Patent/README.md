![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 10/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Patente| 
|Descripción|Descripción del objeto de conocimiento Patente para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Patente

La entidad bibo:Patent (ver Figura 1) representa tanto los derechos de propiedad industrial (marcas, patentes, diseño industrial, modelos de utilidad, variedades vegetales, denominaciones de origen) como los derechos de derechos de autor (derechos morales y de explotación)
Se han añadido varias propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura.

Las propiedades extendidas son las siguientes:

- eroh:applicationNumber
- eroh:operatingCountries
- eroh:operatingCompanies
- eroh:industrialPropertyType
- eroh:industrialPropertyTypeOther
- eroh:authorsRights
- eroh:relatedRights
- eroh:patentNumber
- eroh:tradeSecret
- eroh:knowHow
- eroh:referenceCode
- eroh:spanishPatent
- eroh:europeanPatent
- eroh:internationalPatent
- eroh:pctPatent
- eroh:relevantResults
- eroh:licenses
- eroh:qualityDescription
- eroh:exclusiveUse
- eroh:innovativeEnterprise
- eroh:results
- eroh:products

Una instancia de bibo:Patent se asocia, a su vez, con las siguientes entidades a través de propiedades de objeto:

- obo:BFO_0000023, que contiene el listado de inventores, autores u obtentores relacionados con la patente.
- gn:Feature, que vincula la patente con el país (vcard:hasCountryName) y la región (vcard:hasRegion) de radicación.
- vcard:Address, que localiza los países y regiones donde se ha extendido o formalizado la explotación de la patente.
- foaf:Organization, que representa la organización poseedora de los derechos de patente (roh:ownerOrganization), así como todas aquellas que poseen los derechos para su explotación (eroh:operatingCompanies).
- eroh:IndustrialPropertyType, que señala el tipo de patente.


![](../../Docs/media/ObjetosDeConocimiento/Patent.png)

*Figura 1. Diagrama ontológico para la entidad bibo:Patent*
