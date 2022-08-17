![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 09/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Subvención| 
|Descripción|Descripción del objeto de conocimiento Subvención para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Subvención

La entidad vivo:Grant (ver Figura 1) representa todas aquellas ayudas económicas y/o becas de titularidad pública o privada, nacionales e internacionales, de las que pueda ser beneficiario un investigador.
Se han añadido ciertas propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura Hércules EDMA.

Las propiedades son las siguientes:

- eroh:owner
- eroh:cvnCode
- roh:crisIdentifier
- roh:title
- eroh:awardingEntityTitle
- eroh:awardingEntity
- eroh:awardingEntityType
- eroh:awardingEntityTypeOther
- eroh:aims
- eroh:aimsOther
- eroh:conferralDate
- vivo:start
- vivo:end
- eroh:durationYears
- eroh:durationMonths
- eroh:durationDays
- eroh:entityTitle
- eroh:entity
- eroh:center
- vcard:locality
- vcard:hasCountryName
- vcard:hasRegion
- roh:monetaryAmount
- vivo:freeTextKeyword

Una instancia de vivo:Grant se asocia con las siguientes entidades a través de propiedades de objeto:

- foaf:Organization, que vincula la ayuda con la entidad concesionaria (eroh:awardingEntity) y la entidad donde el investigador ha cursado los estudios que han sido objeto de una ayuda económica y/o beca (eroh:entity).
- eroh:GrantAim, que representa la finalidad de la ayuda económica y/o beca recibida.
- roh:CategoryPath, que representa mediante un esquema jerárquico el tesauro con las áreas temáticas descriptoras de la ayuda o beca concedida.


![](../../Docs/media/ObjetosDeConocimiento/Grant.png)

*Figura 1. Diagrama ontológico para la entidad vivo:Grant*
