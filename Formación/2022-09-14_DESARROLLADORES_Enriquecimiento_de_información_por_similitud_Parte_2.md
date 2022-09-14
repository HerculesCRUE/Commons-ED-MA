![](../Docs/media/CabeceraDocumentosMD.png)

| Fecha                 | 13/09/2022                                |
| --------------------- | ---------------------------------------- |
| Título                | Desarrolladores: Enriquecimiento de información por similitud Parte 2|
| Descripción           | Formación para desarrolladores |
| Versión               | 1.0                                      |
| Módulo                | Formación                            |
| Tipo                  | Especificación                           |
| Cambios de la Versión | Versión inicial                          |

# Enriquecimiento de información por similitud Parte 2

Dentro de este módulo  de formación presentado el 13/9/2022 de 30 minutos de duración dirigido a desarrolladores se verán los siguientes apartados:
 1. Carga de datos
 2. Explotación
    1. Ficha
    2. Search personalizado
    3. Visualización de similitud

## Carga de datos
Esta carga la realiza el [desnormalizador](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.Desnormalizador) dentro del método ActualizadorEDMA.DesnormalizarTodo(), en este servicio se puede configurar una expresión CRON para indicar la frecuencia de esta desnormalización.  
Para ello se obtienen todos los datos de la BBDD y del servicio de similaridad, se comparan y se cargan/editan/eliminan los datos necesarios.

https://serviciospreedma.gnoss.com/servicioexterno/Similarity/GetSimilaritiesDocument?pIdDocument=http://gnoss.com/items/Document_5c01909a-b577-4de3-b424-9f89881654bf_7423452d-6435-499c-9df2-0fa73b7d52bd


 1. [Configuración de páginas: Creación de las páginas de la plataforma (buscadores y cms)](https://github.com/HerculesCRUE/HerculesMA/blob/main/Docs/configuracion-de-paginas.md).
 2. [Metabuscador: Funcionamiento del metabuscador](https://github.com/HerculesCRUE/HerculesMA/blob/main/Docs/metabuscador.md)
 3. [Edición de vistas: Edición de las vistas de buscadores, páginas del CMS y fichas de consulta](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Docs/edicion-de-vistas.md)
