![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 24/10/2022                                                  |
| ------------- | ------------------------------------------------------------ |
|Titulo|API Carga de configuración| 
|Descripción|Manual del servicio Carga de configuración|
|Versión|1.0|
|Módulo|HerculesED|
|Tipo|Manual|
|Cambios de la Versión| Versión inicial |

## Sobre Hercules.CommonsEDMA.ConfigLoad (Carga inicial)

[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.ConfigLoad)

[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.ConfigLoad&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.ConfigLoad)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.ConfigLoad&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.ConfigLoad)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.ConfigLoad&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.ConfigLoad)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.ConfigLoad&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.ConfigLoad)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.ConfigLoad&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.ConfigLoad)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.ConfigLoad&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.ConfigLoad)

# Servicio de Carga de configuración

Este servicio se encarga de la carga de configuración del proyecto (páginas del CMS, facetas...) enviando paquetes al API de despliegues.

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
  "urlAPIDespliegues": "",
}
```
- **LogLevel.Default**: Nivel de error por defecto.
- **LogLevel.Microsoft**: Nivel de error para los errores propios de Microsoft.
- **LogLevel.Microsoft.Hosting.Lifetime**: Nivel de error para los errores de host.
- **urlAPIDespliegues**: Url del API de despliegues.

## Dependencias
- **Serilog.AspNetCore**: v6.0.1
- **Micrisoft.Extensions.Hosting.Abstractions**: 6.0.0
