![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 09/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Actividad| 
|Descripción|Descripción del objeto de conocimiento Actividad para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Actividad

La entidad roh:Activity (ver Figura 1) representa cualquier tipo de actividad de I+D+i llevada a cabo por un investigador. La especialización de dicha entidad incorpora algunas propiedades de los dominios de investigación, además de las correspondientes a la ontología fundamental empleada en ASIO.
Por otra parte, se han añadido ciertas propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura Hércules EDMA.

Las propiedades extendidas son las siguientes:
- eroh:geographicFocusOther
- eroh:attendants
- eroh:managementType
- eroh:classificationCVN
- eroh:conductedBy
- eroh:averageAnnualBudget
- eroh:targetGroupProfile
- eroh:activityModality
- eroh:activityModalityOther

Los subtipos de actividad están relacionados con sus correspondientes epígrafes en la norma CVN a través de los valores descritos en la propiedad eroh:classificationCVN.
Una instancia de roh:Activity se asocia con las siguientes entidades a través de propiedades de objeto:

- foaf:Organization, que vincula una actividad con la entidad convocante (roh:promotedBy) y la entidad de realización (eroh:conductedBy).
- vivo:GeographicRegion, que representa el ámbito geográfico de una actividad.
- gn:Feature, que relaciona la actividad con el país (vcard:hasCountryName) y la región (vcard:hasRegion) donde se desarrolla.
- eroh:ManagementTypeActivity, que representa la tipología de la gestión de dicha actividad llevada a cabo por el investigador.
- eroh:TargetGroupProfile, que indica el perfil profesional de los participantes en la actividad.
- roh:CategoryPath, que representa mediante un esquema jerárquico el tesauro con las áreas temáticas descriptoras de la actividad.
- eroh:ActivityModality, que indica el tipo de evaluación/revisión practicada por el investigador sobre una actividad.


![](../../Docs/media/ObjetosDeConocimiento/Activity.png)

*Figura 1. Diagrama ontológico para la entidad roh:Activity*
