![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 4/3/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Motor de desambiguación y deduplicación de datos.| 
|Descripción|Librería encargada de la deduplicación de datos (investigadores, publicaciones, ROs, etc).|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Motor de desambiguación y deduplicación de datos
El motor de desambiguación y deduplicación de datos se ocupa de reconocer y diferenciar de un conjunto de datos aquellos que sean iguales. Dicho proceso beneficia el desarrollo y mantenimiento de la información en aquellos servicios que hagan uso en Hércules CommonsEDMA.
Este proceso tiene varias ventajas como:
- Reducción del tiempo y el espacio del almacenamiento.
- Información más gestionable.
- Generación de un sistema centralizado de información.

## Funcionamiento
Esta librería tiene 3 métodos públicos dentro de la clase 'Disambiguation', todos ellos toman como parámetros de entrada un listado de items con los datos a los que buscar equivalentes (pItems) y un listado de items en los que buscar los equivalentes (pItemsBBDD). Para ello se utiliza la clase ['DisambiguableEntity'](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguableEntity.cs), se trata de una clase abstracta que hay heredar en función de la implementación deseada. En la implementación hay que utilizar las propiedades que se consideren representativas de la entidad y configurar sus pesos postivos y negativos y su tipo dentro de los siguientes [valores](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguationData.cs):
 - equalsIdentifiers: Propiedad de tipo identificador, dos entidades con el mismo valor son la misma entidad, dos entidades con distintos valores son diferentes entidades
 - equalsTitle: Propiedad de tipo título, en caso de estar configurada, dos elementos tienen que tener el mismo valor para ser considerados iguales (sin diferenciar mayúsculas y minúsculas y excluyendo caracteres no alfanumñericos)
 - equalsItem: Propiedad 'normal' se asignan los pesos positivos en caso de que coincidan y los negativos en caso de que sean diferentes.
 - equalsItemList: Igual que la anterior pero para valores múltiples.
 - algoritmoNombres: Utilizado para los nombres de las personas


### SimilarityBBDD
Obtenemos un diccionario cuya clave son los identificadores de los 'pItems' y un valor que especifica el identificador del item de 'pItemBBDD' con el que se ha obtenido la similaridad. 
Parámetros:
 - pItems: Listado de objetos con items del tipo ['DisambiguableEntity'](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguableEntity.cs) a desambiguar.
 - pItemBBDD: Listado de objetos con items obtenidos de la BBDD del tipo ['DisambiguableEntity'](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguableEntity.cs) para realizar la desmbiguación.
 - pUmbral(valor por defecto 0.8f): Valor por el cual se considerará que una similitud es positiva.
 - pToleranciaNombres (valor por defecto 0f): Tolerancia utilizada en los nombres.

### SimilarityBBDDScores
Obtenemos un diccionario cuya clave son los identificadores de los 'pItems' y un valor que es otro diccionario cuyas claves son los identificadores de los items de 'pItemBBDD' junto con una puntuación de similaridad (entre 0 y 1). 
Parámetros:
 - pItems: Listado de objetos con items del tipo ['DisambiguableEntity'](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguableEntity.cs) a desambiguar.
 - pItemBBDD: Listado de objetos con items obtenidos de la BBDD del tipo ['DisambiguableEntity'](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguableEntity.cs) para realizar la desmbiguación.
 - pUmbral(valor por defecto 0.8f): Valor por el cual se considerará que una similitud es positiva.
 - pToleranciaNombres (valor por defecto 0f): Tolerancia utilizada en los nombres.

### Disambiguate
Obtenemos un diccionario cuya clave son los identificadores de los 'pItems' y un valor que es una lista con los identificadores de los items de 'pItemBBDD' con los que se ha obtenido la similaridad. 
Parámetros:
 - pItems: Listado de objetos con items del tipo ['DisambiguableEntity'](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguableEntity.cs) a desambiguar.
 - pItemBBDD: Listado de objetos con items obtenidos de la BBDD del tipo ['DisambiguableEntity'](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.DisambiguationEngine/Hercules.CommonsEDMA.DisambiguationEngine/Models/DisambiguableEntity.cs) para realizar la desmbiguación.
 - pDisambiguateItems: Indica si hay que desambiguiar entre los pItems.
 - pUmbral(valor por defecto 0.8f): Valor por el cual se considerará que una similitud es positiva.
 - pToleranciaNombres (valor por defecto 0f): Tolerancia utilizada en los nombres.

### Usos dentro de Hércules

En Hércules CommonsEDMA, dicho motor de desambiguación es utilizado en varios servicios que trabajan con datos. Estos servicios son los siguientes:
- Edición de CVs.
- Importación de CVN.
- Carga de datos de fuentes externas.


## Edición de CVs
En el proceso de edición de items del CV se utiliza al añadir autores a publicaciones.

## Importación de CVN
En el proceso de carga de datos por medio de los curriculum vitae (CV) de los usuarios, se hace uso para obtener las equivalencias para todos los ítems propios del CV, además de las equivalencias entre las personas almacenadas en base de datos (BBDD) y las cargadas desde el CV. 
Cada ítem tiene diferentes atributos de diferenciación, que se marcarán en el servicio de importación de CV y un score o valor que indicará la similaridad entre diferentes ítems. 
Para considerar similares dos ítems se deberá alcanzar un valor minimo de score, que se conseguirá con la suma de scores de los diferentes atributos.   
Tras ello, por medio del metodo SimilarityBBDD de la clase Disambiguation, se compararán los ítems leidos del CV con los almacenados en BBDD y según los criterios descritos anteriormente se diferenciarán las similaridades, devolviendo un listado de equivalencias.

## Carga de datos de Fuentes Externas
En el proceso de obtención de datos de fuentes externas se hace uso para obtener las equivalencias de investigadores, publicaciones y research objects.
Para ello, se utiliza el método Disambiguate, el cual requiere de dos parametros que son:
- Listado con los items obtenidos de fuentes externas para ser desambiguados.
- Listado de los items ya cargados en BBDD que tengan algún tipo de relación con los items obtenidos de fuentes externas.

En este proceso, se detecta los items iguales o que tengan un alto umbral de similaridad y se relacionan entre ellos mediante IDs.
Como resultado del proceso, el motor devolverá un diccionario cuya clave es el item a desambiguar y como valor una lista de sus equivalencias.
Posteriormente, el servicio encargado de fuentes externas procederá a la carga de datos.
Para más información sobre el servicio de fuentes externas mirar en el siguiente repositorio [Hercules.ED.ResearcherObjectLoad](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.ResearcherObjectLoad).


## Dependencias
- **GnossApiWrapper.NetCore**: v6.0.6
