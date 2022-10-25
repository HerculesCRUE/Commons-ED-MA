![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 28/6/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Servicio de carga de datos desde Hércules SGI| 
|Descripción|Servicio encargado de la  carga de datos (investigadores, publicaciones, ROs, etc) desde Hércules SGI - Sistema de Gestión de la Investigación|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

# Introducción
[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Harvester)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Harvester&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Harvester)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Harvester&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Harvester)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Harvester&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Harvester)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Harvester&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Harvester)


El servicio de carga de datos por parte del SGI se ocupa de recolectar los datos recibidos por medio del [servicio OAI-PMH](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.OAI_PMH) y cargarlos en base de datos (BBDD). 
Este servicio automatiza la lectura de los ficheros recibidos por medio del [servicio OAI-PMH](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.OAI_PMH) y la inserción de los datos en BBDD.

El funcionamiento de los servicios de extracción está documentado en [ED y MA. Integraciones.](https://confluence.um.es/confluence/pages/viewpage.action?pageId=416055407)

## Tipos de datos recibidos
- AutorizacionProyecto: Autorizacion de proyectos para su posterior envio a validación.
- Grupos: Grupo/Equipo de investigación, desarrollo o innovación
- Invencion: Propiedad industrial e intelectual
- Organizacion: Organizacion
- Persona: Persona
- PRC: Relacion de proyecto validado en producción cientifica
- Proyecto: Proyecto

## Funcionamiento
El servicio empezará por pedir los datos de los archivos a partir de la última fecha de actualización, guardando los datos recibidos en ficheros para su posterior carga, tras ello se actualizará el fichero de última actualización con la fecha de actualización.
El programa se quedará leyendo los archivos pendientes de carga, tratando los datos recibidos de estos e insertandolos en BBDD, en caso de que no se encuentren almacenados con anterioridad. Tras terminar de leer los datos de los archivos se almacenará en una carpeta separada de la de lectura el fichero leido.


## Configuración en el appsetting.json
```json
{
  "Url_OAI-PMH": "",
  "LastUpdateDateFile": "",
  "DirLogCarga": ""
}
```
- Url_OAI-PMH: URL dónde está instalado el servicio OAI-PMH.
- LastUpdateDateFile: Ruta del fichero dónde se va a almacenar la fecha de la última actualización
- DirLogCarga: Directorio dónde se van a almacenar los ficheros de pendientes de carga y procesados

## Dependencias
- **GnossApiWrapper.NetCore**: v1.0.6
- **Newtonsoft.Json**: 13.0.1
