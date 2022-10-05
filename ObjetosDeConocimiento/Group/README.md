![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 09/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objetos de Conocimiento Group| 
|Descripción|Descripción del objeto de conocimiento Group para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Group

La entidad foaf:Group (ver Figura 1) representa a un grupo de investigación. La especialización de dicha entidad incorpora algunas propiedades de los dominios de investigación, además de las correspondientes a la ontología fundamental empleada en ASIO.

Se han añadido ciertas propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura Hércules EDMA.

En ROH, foaf:Group está basada en FOAF (Friend of a Friend) y sigue los patrones empleados en VIVO. Ello explica la inclusión de propiedades como foaf:member, vivo:affiliatedOrganization o vivo:description. Se han adoptado otras relaciones de importancia acordes al formato común de información en materia de la investigación europea (CERIF), como roh:crisIdentifier o roh:title.

Una instancia de foaf:Group se asocia con las siguientes entidades a través de propiedades de objeto:

- obo:BFO_0000023, que contiene información sobre la actividad del investigador en el grupo (fechas de comienzo y fin de su participación, su firma, la persona a la que apunta,
- [foaf:Organization](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Organization), que vincula a un grupo de investigación con una determinada organización.
- [eroh:OrganizationType](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/OrganizationType), representa el tipo de organización.
- [gn:Feature](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Feature), representa el país y la comunidad autónoma o región.
- eroh:PersonAux, representa a los investigadores.
- [foaf:Person](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Person), representa los miembros del grupo.
- roh:CategoryPath, representa las áreas temáticas.


![](../../Docs/media/ObjetosDeConocimiento/Group.png)

*Figura 1. Diagrama ontológico para la entidad foaf:Group*
