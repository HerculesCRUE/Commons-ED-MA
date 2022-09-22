![](../Docs/media/CabeceraDocumentosMD.png)

| Fecha                 | 22/09/2022                                |
| --------------------- | ---------------------------------------- |
| Título                | Desarrolladores: Configuración de duplicación de items|
| Descripción           | Formación para desarrolladores |
| Versión               | 1.0                                      |
| Módulo                | Formación                            |
| Tipo                  | Especificación                           |
| Cambios de la Versión | Versión inicial                          |

# Configuración de duplicación de items

Dentro de este módulo  de formación presentado el 20/9/2022 de 90 minutos de duración dirigido a desarrolladores se verán los siguientes apartados:


 - [Gestión de duplicidades del CV](#gestión-de-duplicidades-del-cv)
   - [Flujo](#flujo)
 - [Motor de desambiguación](#motor-de-desambiguación)
   - [Editor de CV. Autores de publicaciones](#editor-de-cv-autores-de-publicaciones) 
   - [Carga de fuentes externas](#carga-de-fuentes-externas) 
 

## Gestión de duplicidades del CV
Se ha implementado una herramienta para que los investigadores desde su CV puedan gestionar sus posibles duplicados. Ya que estas pueden surgir al importar datos de diversos lugares Fuentes externas, carga inicial, carga del CV por el usuario, carga de ítems como coautor, etc.


### Flujo
En el servicio [EdiciónCV](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) se configuran los elementos sobre los cuales se deben comprobar duplicidades, para ello en los [TabTemplates](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV/EditorCV/Config/TabTemplates) se añade el campo "checkDuplicates" con valor true en los ítems a comprobar.

La gestión de duplicados puede darse en varios casos:
 - Al entrar en la edición del CV del usuario se lanzará directamente la gestión de duplicados.
 - Desde el editor de CV, al seleccionar el botón de "GESTIONAR DUPLICADOS", en la que adicionalmente se podrán buscar duplicados más dudosos.
 - Al enviar algun ítem a producción científica, sobre un ítem especifico.

En la gestión de duplicados se hace una llamada ajax al servicio "GetItemDuplicados" del servicio [EdiciónCV](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) pasando como parametro el identificador del CV y el porcentaje de similaridad que servirá como diferenciador de los elementos. Por defecto se enviará un 0.9, 90% de similaridad entre los títulos, y tras terminar de gestionar todos los duplicados se le dará la opción al usuario de disminuirlo, para intentar buscar duplicados más dudosos y lo bajaremos a un 0.7, es decir un 70%. 
Al enviar ítems a producción cientifica, adicionalmente se enviará como parametro extra el identificador del ítem a enviar.
 
El servicio [EdiciónCV](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) comprobará que el usuario de la sesión, es el mismo que el del CV que hemos enviado y posteriormente obtendrá los elementos que presentan duplicidad entre ellos.

Los elementos son guardados en un diccionario, y se recorrerán comprobando la diferencia entre todos ellos, en el caso de encontrar elementos duplicados (A y B), partiendo de que se comprueban similaridades sobre A, se eliminará del listado de comprobación el elemento B, al haberse encontrado similaridad con A.
 
Para comprobar la diferencia entre dos elementos se comprobará la diferencia entre sus títulos, mediante la comprobación de la distancia de Levenshtein (mínimo número de operaciones requeridas para transformar una cadena de caracteres A en otra B).
 
Se eliminará del listado de respuesta aquellos marcados como no duplicados pero que se haya encontrado similaridad entre ambos.

Tras ello se devolverá al navegador un listado de objetos [SimilarityResponse](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.EditorCV/EditorCV/Models/Similarity/SimilarityResponse.cs)

En el navegador se lanzará un popup en cuyo título se mostrará un contador el número de elementos totales que se deben revisar, junto a las opciones a aplicar sobre los mismos ("APLICAR Y SIGUIENTE" e "IGNORAR Y SIGUIENTE"). 
Los elementos se pintarán en medio del pop-up, mostrando en caso de que alguno de los elementos esté bloqueado como principal, si varios son bloqueados se seleccionará como principal el primero. Aquellos elementos secundarios podran pasar a principal pulsando el botón "CAMBIAR A PRINCIPAL", a menos que el principal esté bloqueado y el secundario sea no bloqueado. Las opciones que se pueden aplicar sobre los elementos secundarios son:
 - Fusionar. Se copia en el principal la información del ítem que no esté en el principal
 - Eliminar. Se elimina el ítem y no se volverá a presentar
 - Marcar como no duplicado. Se marca para que no se vuelva a proponer como duplicado del ítem principal

 

## Motor de desambiguación
El [motor de desambiguación](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.DisambiguationEngine) se utiliza en diferentes implementaciones dentro de Hércules, su función es la de comparar diferentes items en base a sus características para comprobar si se trata de los mismo items. 

Dentro de Hércules se ha utilizado en diferentes servicios, a continución se explican diferentes implementaciones de este motor dentro de Hércules.

### Editor de CV. Autores de publicaciones
Dentro del [editor de CV](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) se utiliza cuando se añaden autores a las publicaciones.

El flujo de esta operativa es el siguiente:

  - El usuario introduce las firmas de los autores separadas por ';' y se pulsa sobre 'Buscar'. Esto desencadenará una llamada AJAX al servicio [editor de cv](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) al método 'ValidateSignatures' del controlador 'EdicionCVController'.

  - En primer lugar recuperamos todos los colaboradores del dueño del CV junto con su número de colaboraciones tanto en proyectos como en publicaciones y también los departamentos de todos sus colaboradores.

  - Posteriormente para cada uno de los nombres introducidos buscamos a todos los candidatos con el método 'ObtenerPersonasFirma' de la clase 'AccionesEdicion' y construimos objetos de tipo [Person](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.EditorCV/EditorCV/Models/API/Response/Person.cs) en los cuales cargaremos su nombre en una propiedad de tipo 'algoritmoNombres'.

  - A continuación creamos otro objeto [Person](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.EditorCV/EditorCV/Models/API/Response/Person.cs) con el nombre de la firma tal cual se ha introducido.

  - Con estos datos realizamos una petición al método 'SimilarityBBDDScores' con los siguientes parámetros:
    - pItems: La persona con el nombre introducido por el usuario.
    - pItemBBDD: Las personas candidatas obtenidas.
    - pUmbral: Pasamos '0' porque en este caso queremos obtener el score de cualquier posible candidato.
    - pToleranciaNombres: Pasamos '0.5' porque en este caso permitimos que haya una discrepancia en los nombres de hasta el 50% ya que luego el usuario elegirá de entre los candidatos.

  - Como resultado obtenemos un diccionario con los Identificadores de todos los candidatos junto con una puntuación en función de la similitud de los nombres.
  - Asignamos a la lista de candidatos el score obtenido y eliminamos aquellos para los que no hayamos obtenido ningún score.
  - Nos recorremos cada una de las personas y consideramos su score máximo = person.score + (1 - person.score) * person.score.
  - Consideramos por cada publicación en común con el usuario actual un score adicional de 0.1, por cada proyecto 0.1 y en caso de que coincida el departamento 0.2.
  - Realizamos todos los cálculos, ordenamos los resultados, cogemos las primeras 20 propuestas para cada firma y lo devolvemos al usuario.
  - El usuario puede seleccionar uno de los candidatos propuestos o buscar uno nuevo a través de su ORCID, en el método ValidateORCID del controlador GuardadoCVController, que buscará dentro del sistema alguien con ese ORCID y en caso de no encontrarlo lo buscará a través del API de ORCID.
  - En última instancia instancia, en caso de no encontrar el ORCID se podrá crear la persona introducienco el nombre y los apellidos.

### Carga de fuentes externas
A los investigadores se les cargarán publicaciones provenientes de fuentes externas como [Wos](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.ExternalSources/Hercules.ED.WoSConnect), [Scopus](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.ExternalSources/Hercules.ED.ScopusConnect), y [OpenAire](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.ExternalSources/Hercules.ED.OpenAireConnect) obtenidas a través del servicio [Publication](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.ExternalSources/Hercules.ED.Publication) que generará un JSON para su carga posterior con el [Servicio de carga de datos obtenidos de fuentes externas](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.ResearcherObjectLoad). 

El flujo de esta operativa es el siguiente:
  - El servicio lee los JSON que están en la carpeta que está configurada como directorio de lectura.
  - Obtenemos los datos de la persona propietaria del JSON y cargamos los datos del JSON.
  - Obtenemos los datos candidatos para las publicaciones y los autores leídos del JSON y los cargamos de tipo [DisambiguationPerson](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.ResearcherObjectLoad/Hercules.ED.ResearcherObjectLoad/Models/DisambiguationObjects/DisambiguationPerson.cs) y  [DisambiguationPubilcation](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.ResearcherObjectLoad/Hercules.ED.ResearcherObjectLoad/Models/DisambiguationObjects/DisambiguationPublication.cs) en los cargaremos las propiedades necesarias para realizar la posterior desambiguación.
  - Con estos datos realizamos una petición al método 'Disambiguate' con los siguientes parámetros:
    - pItems: Las personas y publicaciones obtenidas del JSON.
    - pItemBBDD: Las personas y publicaciones candidatas recuperadas de BBDD.
  - Como resultado obtenemos un diccionario cuyas claves son los identificadores de las publicaciones de BBDD y cuyos valores son las publicaciones y personas del JSON
  - A continuación cargamos las personas que no existan en el sistema o actualizamos el ORCID de las que corresponda.
  - Posteriormente creamos los objetos de las publicaciones a cargar y les asignamos las personas que hemos cargado previamente.
  - Creamos los objetos de las notificaciones para informar a los usuarios de la carga/modificación de las publicaciones.
  - Realizamos la carga/modificación de personas y publicaciones.
  - Realizamos la carga de las notificaciones.
  - Creamos la notificación para informar al propietario del JSON que se ha terminado de procesar.
  - En último lugar creamos un ZIP con el JSON comprimido y lo eliminamos de la carpeta de pensientes de procesar.

