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
 

## Gestión de duplicidades del CV
Se ha implementado una herramienta para que los investigadores desde su CV puedan gestionar sus posibles duplicados. Ya que estas pueden surgir al importar datos de diversos lugares Fuentes externas, carga inicial, carga del CV por el usuario, carga de ítems como coautor, etc.


### Flujo
En el servicio [EdiciónCV](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) se configuran los elementos sobre los cuales se deben comprobar duplicidades, para ello en los [TabTemplates](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV/EditorCV/Config/TabTemplates) se añade el campo "checkDuplicates" con valor true en los ítems a comprobar.

Al entrar en la edición del CV del usuario se lanzará directamente la gestión de duplicados. Desde el editor de CV, al seleccionar el botón de "GESTIONAR DUPLICADOS" se lanza la misma gestión, en la que adicionalmente se podrán buscar duplicados más dudosos.

En la gestión de duplicados se hace una llamada ajax al servicio "GetItemDuplicados" del servicio [EdiciónCV](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) pasando como parametro el identificador del CV y el porcentaje de similaridad que servirá como diferenciador de los elementos. Por defecto se enviará un 0.9, 90% de similaridad entre los títulos, y tras terminar de gestionar todos los duplicados se le dará la opción al usuario de disminuirlo, para intentar buscar duplicados más dudosos y lo bajaremos a un 0.7, es decir un 70%.
 
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
Para ello en el CV se introducen las firmas de los autores separadas por ';' y se pulsa sobre 'Buscar'. Esto desencadenará una llamada AJAX al servicio [editor de cv](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV) al método 'ValidateSignatures' del controlador 'EdicionCVController'.

En primer lugar recuperamos todos los colaboradores del dueño del CV junto con su número de colaboraciones tanto en proyectos como en publicaciones y también los departamentos de todos sus colaboradores.

Posteriormente para cada uno de los nombres introducidos buscamos a todos los candidatos con el método 'ObtenerPersonasFirma' de la clase 'AccionesEdicion' y construimos objetos de tipo [Person](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.EditorCV/EditorCV/Models/API/Response/Person.cs) en los cuales cargaremos su nombre en una propiedad de tipo 'algoritmoNombres'.

A continuación creamos otro objeto [Person](https://github.com/HerculesCRUE/HerculesED/blob/main/src/Hercules.ED.EditorCV/EditorCV/Models/API/Response/Person.cs) con el nombre de la firma tal cual se ha introducido.

Con estos datos realizamos una petición al método 'SimilarityBBDDScores' con los siguientes parámetros:
  - pItems: La persona con el nombre introducido por el usuario.
  - pItemBBDD: Las personas candidatas obtenidas.
  - pUmbral: Pasamos '0' porque en este caso queremos obtener el score de cualquier posible candidato.
  - pToleranciaNombres: Pasamos '0.5' porque en este caso permitimos que haya una discrepancia en los nombres de hasta el 50% ya que luego el usuario elegirá de entre los candidatos.

Como resultado obtenemos un diccionario con los Identificadores de todos los candidatos junto con una puntuación en función de la similitud de los nombres.

Asignamos a la lista de candidatos el score obtenido y eliminamos aquellos para los que no hayamos obtenido ningún score.

Nos recorremos cada una de las personas y consideramos su score máximo = person.score + (1 - person.score) * person.score;

Consideramos por cada publicación en común con el usuario actual un score adicional de 0.1, por cada proyecto 0.1 y en caso de que coincida el departamento 0.2.

Realizamos todos los cálculos, ordenamos los resultados, cogemos las primeras 20 propuestas para cada firma y lo devolvemos al usuario.


