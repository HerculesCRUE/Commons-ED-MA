![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 28/06/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Proceso de carga inicial| 
|Descripción|Desarrollo del proceso de carga de los datos iniciales XML.|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Hércules ED - MA. Desarrollo del proceso de la carga inicial de datos mediante los archivos XML.

Este documento describe el proceso de carga de los datos XML ofrecidos por la universidad de Murcia. Cuando el sistema de gestión de información (SGI) esté implantado por completo, dicho programa de carga estará en desuso.

Carga de entidades secundarias
==============================
En primer lugar, para seguir el correcto funcionamiento del proceso de carga, es necesario empezar por aquellas entidades que son secundarias. 

Se ha de guardar el fichero ReferenceTables.xml y Thesaurus.xml (obtenido por el CVN) en la ruta ./Dataset/CVN

Carga de entidades principales
==============================
Una vez preparada los ficheros necesarios para la carga de entidades secundarias, precedemos a las primarias.

Es necesario que los ficheros XML ofrecidos por la universidad estén en el directorio de ./Dataset/UMU

También hay que guardar el excel de las taxonomias (Hércules-ED_Taxonomías.xlsx) y el de datos adicionales de las personas (personas_hercules.xlsx) en ./Dataset

