![](../Docs/media/CabeceraDocumentosMD.png)

| Fecha                 | 14/09/2022                                |
| --------------------- | ---------------------------------------- |
| Título                | Desarrolladores: Enriquecimiento de información por similitud. Integración de los resultados|
| Descripción           | Formación para desarrolladores |
| Versión               | 1.0                                      |
| Módulo                | Formación                            |
| Tipo                  | Especificación                           |
| Cambios de la Versión | Versión inicial                          |

# Enriquecimiento de información por similitud. Explotación del servicio

Dentro de este módulo  de formación presentado el 13/9/2022 de 30 minutos de duración dirigido a desarrolladores se verán los siguientes apartados:
 - [Carga de datos](#carga-de-datos)
 - [Explotación](#explotación)
   1. [Ficha de publicación](#ficha-de-publicación)
   2. [Servicio externo](#servicio-externo)
   3. [Search personalizado](#search-personalizado)
   4. [Visualización de similitud](#visualización-de-similitud)

## Carga de datos
Esta carga la realiza el [desnormalizador](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.Desnormalizador) dentro del método ActualizadorEDMA.DesnormalizarTodo(), en este servicio se puede configurar una expresión CRON para indicar la frecuencia de esta desnormalización.  
Para ello se obtienen todos los datos de la BBDD y del servicio de similaridad, se comparan y se cargan/editan/eliminan los datos necesarios en el [servicio de similitud](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.Enrichment/Similitud) con los siguientes datos:
 1. Identificador
 2. Nombres de los autores
 3. Descripción
 4. Etiquetas con peso
 5. Categorías con peso
 

## Explotación
Estos datos se explotan dentro de las fichas de las publicaciones dentro de la pestaña de relacionados.

### Ficha de publicación
Al entrar en la ficha o pulsar sobre la pestaña de relacionados se hace una petición al [servicio externo](https://github.com/HerculesCRUE/HerculesMA/tree/main/src/Hercules.MA.ServicioExterno) 

### Servicio externo
La petición llega al método 'GetSimilaritiesDocument' del controlador 'SimilarityController' pasando como parámetro el ID de la publiación {URL_SERVCIO_EXTERNO}/Similarity/GetSimilaritiesDocument?pIdDocument={ID_PUBLICACIÓN}. Esta petición nos devuelve los IDs de las publicaciones relacionadas junto con las etiquetas y su peso en la relación.

### Search personalizado
Una vez obtenidos los IDs de las publicaciones a mostrar se realiza una búsqueda utilizando los servicios de facetas y de resultados utilizadno el Search personalizado 'searchRelacionadosDocumentoIn' con el que obtendremos los datos de las publicaciones ordenadas en función de la respuesta del servicio externo.

### Visualización de similitud
Una vez completada la carga de resultados se ejecuta el javascript 'CompletadaCargaRecursosSimilitud()' que marca en color rojo las etiquetas de las publicaciones de los resultados que guarden relación con la ficha de la publicación que estemos visualizando.
