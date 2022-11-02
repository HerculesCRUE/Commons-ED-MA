![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 02/11/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Hércules EDMA. Journals| 
|Descripción|Servicio de carga de revistas|
|Versión|1|
|Módulo|Documentación|
|Tipo|Código|
|Cambios de la Versión|Versión inicial|


[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Journals)

[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Journals&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Journals)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Journals&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Journals)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Journals&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Journals)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Journals&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Journals)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Journals&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Journals)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Journals&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Journals)



# Journals
Este servicio obtendrá las revistas de base de datos (BBDD), leerá del Excel los datos de las revistas de WoS y Scopus, los comparará con los de BBDD y los cargará, modificará o eliminará.

El formato de los excels está especificado en el apartado correspondiente en [Confluence](https://confluence.um.es/confluence/pages/viewpage.action?pageId=556662785).

# Proceso de carga
1-. Lectura del excel. El programa leerá el excel y guardará en memoria los datos de las filas. En esta lectura, comprobará si el formato de las columnas es correcto. En el caso de que no lo sea, el programa se detendrá y parará el proceso.

2-. Preparación del objeto a cargar.

3-. Lectura de las filas del excel y creación del objeto con los datos leidos.

4-. Carga de revistas a la BBDD. Antes de dicha carga, el programa comprobará si hay revistas duplicadas. En el caso de que haya, el programa se detendrá.

5-. Borrado de revistas inecesarias/duplicadas de BBDD. Si la revista que se ha cargado ya existía en BBDD, la borrará y dejará la nueva. 

6-. Modificación de revistas. Sobreescribirá los datos de aquellas revistas que sean iguales.

## Dependencias
- **ExcelDataReader**: v3.6.0
- **ExcelDataReader.DataSet**: v3.6.0
- **GnossApiWrapper.NetCore**: v6.0.7
