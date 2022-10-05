![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 15/03/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento ImpartedCoursesSeminars| 
|Descripción|Descripción del objeto de conocimiento ImpartedCoursesSeminars para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento ImpartedCoursesSeminars

La entidad eroh:ImpartedCoursesSeminars (ver Figura 1) representa los Cursos y seminarios impartidos orientados a la formación docente universitaria en el Curriculum Vitae en la plataforma Hércules.

Una instancia de eroh:ImpartedCoursesSeminars se asocia, a su vez, con las siguientes entidades a través de propiedades de objeto:

- [foaf:Person](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Person), representa la persona asociada a los cursos y seminarios.
- [eroh:EventType](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/EventType), representa el tipo de evento.
- [foaf:Organization](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Organization), representa la entidad organizadora del programa.
- [eroh:OrganizationType](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/OrganizationType), representa el tipo de entidad organizadora del programa.
- [gn:Feature](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Feature), representa el país y la comunidad autónoma o región de la entidad organizadora.
- [eroh:Language](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Language), representa el idioma en el que impartió.
- foaf:Document, representa otros identificadores.
- [eroh:ParticipationTypeDocument](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/ParticipationTypeDocument), representa el tipo de participación.

![](../../Docs/media/ObjetosDeConocimiento/ImpartedCoursesSeminars.png)

*Figura 1. Diagrama ontológico para la entidad eroh:ImpartedCoursesSeminars*
