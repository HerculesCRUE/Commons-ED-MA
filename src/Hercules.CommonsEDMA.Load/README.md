![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 28/06/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Proceso de carga inicial| 
|Descripción|Desarrollo del proceso de carga de los datos iniciales XML.|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|


[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Load)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Load&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Load)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Load&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Load)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Load&metric=ncloc)](https://sonarcloud.io/dashboard?id=Hercules.CommonsEDMA.Load)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Load&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Load)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Load&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Load)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Load&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Load)



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

También hay que guardar el excel de las taxonomias (Hércules-CommonsEDMA_Taxonomías.xlsx) y el de datos adicionales de las personas (personas_hercules.xlsx) en ./Dataset

Dependencias
============
- EnterpriseLibrary.Data.NetCore: v6.3.2
- ExcelDataReader: v3.6.0
- ExcelDataReader.DataSet: v3.6.0
- GnossApiWrapper.NetCore: v1.0.8
- Newtonsoft.Json: v13.0.1
- System.Text.Encoding.CodePages: v6.0.0
