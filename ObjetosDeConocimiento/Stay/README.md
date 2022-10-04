![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 09/12/2021                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Stay| 
|Descripción|Descripción del objeto de conocimiento Stay para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Stay

La entidad eroh:Stay (ver Figura 1) representa estancias de movilidad temporal del titular del CV en centros I+D+i de enseñanza superior y/o de investigación de titularidad pública o privada, nacionales e internacionales.
Se han añadido ciertas propiedades que extienden la ontología fundamental con el fin de dar respuesta a las necesidades de gestión de datos requeridas durante el desarrollo de la infraestructura Hércules EDMA.

Una instancia de eroh:Stay se asocia con las siguientes entidades a través de propiedades de objeto:

- [foaf:Person](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Person), representa la persona asociada a la estancia.
- [eroh:StayGoal](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/StayGoal), que se emplea para representar los objetivos de la estancia.
- [foaf:Organization](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Organization), que relaciona la estancia con la entidad donde se ha realizado (eroh:entity), así como la entidad responsable de su financiación (roh:fundedBy).
- [eroh:OrganizationType](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/OrganizationType), que relaciona la estancia con el tipo de entidad donde se ha realizado (eroh:entity), así como el tipo de entidad responsable de su financiación (roh:fundedBy).
- [gn:Feature](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Feature), representa el país y la comunidad autónoma o región de la entidad de realización y de la entidad financiadora.
- roh:CategoryPath, que representa los códigos UNESCO y las palabras clave.


![](../../Docs/media/ObjetosDeConocimiento/Stay.png)

*Figura 1. Diagrama ontológico para la entidad eroh:Stay*
