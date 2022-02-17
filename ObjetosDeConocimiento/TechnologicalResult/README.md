![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 10/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Resultado Tecnológico| 
|Descripción|Descripción del objeto de conocimiento Resultado Tecnológico para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Resultado Tecnológico

La entidad eroh:TechnologicalResult (ver Figura 1) representa cualquier resultado tecnológico derivado de actividades especializadas y de transferencia.
Se han añadido varias propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura.

Las propiedades extendidas son las siguientes:

- eroh:participates
- eroh:principalInvestigator
- eroh:coprincipalInvestigator
- eroh:durationYears
- eroh:durationMonths
- eroh:durationDays
- eroh:newTechniques
- eroh:spinoffCompanies
- eroh:activityResults
- eroh:standardisation
- eroh:collaborationAgreements
- eroh:targetOrganizations
- eroh:relevantResults

Una instancia de eroh:TechnologicalResult se asocia, a su vez, con las siguientes entidades a través de propiedades de objeto:

- foaf:Organization, que referencia las distintas organizaciones que han colaborado en la actividad experta y/o de transferencia (eroh:participates), además de aquellas organizaciones destinatarias de dicha actividad (eroh:targetOrganizations).
- obo:BFO_0000023, contiene tanto al investigador principal o responsable del equipo en el que se ha realizado la actividad tecnológica (eroh:principalInvestigator), como al investigador corresponsable (eroh:coprincipalInvestigator).
- roh:CategoryPath, que representa mediante un esquema jerárquico el tesauro con las áreas temáticas descriptoras de la actividad.
- vivo:GeographicRegion, representa el ámbito geográfico de la actividad experta.


![](../../Docs/media/ObjetosDeConocimiento/TechnologicalResult.png)
*Figura 1. Diagrama ontológico para la entidad eroh:TechnologicalResult*
