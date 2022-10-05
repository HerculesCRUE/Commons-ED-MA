![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 10/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Patent| 
|Descripción|Descripción del objeto de conocimiento Patent para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Patent

La entidad bibo:Patent (ver Figura 1) representa tanto los derechos de propiedad industrial (marcas, patentes, diseño industrial, modelos de utilidad, variedades vegetales, denominaciones de origen) como los derechos de derechos de autor (derechos morales y de explotación)
Se han añadido varias propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura.

Una instancia de bibo:Patent se asocia, a su vez, con las siguientes entidades a través de propiedades de objeto:

- [gn:Feature](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Feature), representa el país y la comunidad autónoma o región.
- roh:PersonAux, que referencia los autores de la propiedad intelectual.
- eroh:Feature, que referencia los paises sobre los cuales se ha extendido/formalizado la explotación.
- [foaf:Organization](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Organization), que referencia la entidad titular de derechos.
- eroh:Organization, que referencia las empresas.
- [eroh:IndustrialPropertyType](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/IndustrialPropertyType), que referencia el tipo de propiedad intelectual.
- roh:CategoryPath, referencia las palabras clave.
- [eroh:ResultType](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/ResultType), que referencia el resultado.

![](../../Docs/media/ObjetosDeConocimiento/Patent.png)

*Figura 1. Diagrama ontológico para la entidad bibo:Patent*
