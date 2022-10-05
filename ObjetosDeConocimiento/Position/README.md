![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 15/03/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Objeto de Conocimiento Position| 
|Descripción|Descripción del objeto de conocimiento Position para Hércules|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED. Objeto de conocimiento Position

La entidad vivo:Position (ver Figura 1) representa ítems de situación profesional actual e ítems de cargos actividades.

Una instancia de vivo:Position se asocia, a su vez, con las siguientes entidades a través de propiedades de objeto:

- [foaf:Person](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Person), representa la persona asociada al ítem.
- [foaf:Organization](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Organization), representa las entidades asociadas al ítem.
- [eroh:OrganizationType](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/OrganizationType), representa el tipo de entidad asociada al ítem.
- [eroh:ContractModality](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/ContractModality), que referencia la modalidad de contrato.
- roh:CategoryPath, que referencia los códigos UNESCO.
- [eroh:DedicationRegime](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/DedicationRegime), que referencia los régimenes de dedicación.
- [eroh:ScopeManagementActivity](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/ScopeManagementActivity), que referencia el ámbito de actividad de gestión.
- [gn:Feature](https://github.com/HerculesCRUE/Commons-ED-MA/tree/main/ObjetosDeConocimiento/Feature), representa el país y la comunidad autónoma o región.
- vcard:TelephoneType, que referencia el teléfono y fax.

![](../../Docs/media/ObjetosDeConocimiento/Position.png)

*Figura 1. Diagrama ontológico para la entidad eroh:Position*
