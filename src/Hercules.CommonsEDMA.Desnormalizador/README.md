![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 29/06/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Servicio de desnormalización de datos| 
|Descripción|Proceso encargado de generar datos desnormalizados para su consulta, búsqueda yrepresentación|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Desnormalizador)

[![Reliability](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Desnormalizador&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Desnormalizador)
[![Security](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Desnormalizador&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Desnormalizador)
[![Maintainability](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Desnormalizador&metric=maintainability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Desnormalizador)
[![Security Review](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Desnormalizador&metric=security_review_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Desnormalizador)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Desnormalizador&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Desnormalizador)



# Servicio de desnormalización de datos

El Desnormalizador es un proceso encargado de generar datos desnormalizados para su consulta, búsqueda y representación; tanto en Hércules ED como en [Hércules MA](https://github.com/HerculesCRUE/HerculesMA).
El Desnormalizador tambien es encargado de eliminar los datos innecesarios del sistema, como las notificaciones antiguas.

## Configuración en el appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "ConnectionStrings": {
    "RabbitMQ": ""
  },
  "AllowedHosts": "*",
  "DenormalizerQueueRabbit": "",
  "DirectorioEscritura": "",
  "UrlSimilarity": "",
  "DenormalizerCronExpression": ""
}
```
- **LogLevel.Default**: Nivel de error por defecto.
- **LogLevel.Microsoft**: Nivel de error para los errores propios de Microsoft.
- **LogLevel.Microsoft.Hosting.Lifetime**: Nivel de error para los errores de host.
- **ConnectionStrings.RabbitMQ**: Conexión con Rabbit.
- **DenormalizerQueueRabbit**: Nombre de la cola.
- **DirectorioEscritura**: Ruta de directorio de escritura temporal.
- **UrlSimilarity**: Url del servicio de [similaridad](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.Enrichment/Similitud).
- **DenormalizerCronExpression**: Expresión CRON para una ejecución completa del desnormalizador.


## Dependencias
- **dotNetRDF**: v2.7.2
- **GnossApiWrapper.NetCore**: v1.0.6
- **Quartz**: v3.4.0
- **RabbitMQ.Client**: v6.2.4
- **System.Net.Http.Formatting.Extension**: v5.2.3
