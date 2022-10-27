![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 4/3/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Servicio OAI-PMH| 
|Descripción|Servicio OAI-PMH para recolección de datos desde los sistemas de la universidad, SGI y otros|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|



[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.OAI_PMH)

[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.OAI_PMH&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.OAI_PMH)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.OAI_PMH&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.OAI_PMH)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.OAI_PMH&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.OAI_PMH)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.OAI_PMH&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.OAI_PMH)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.OAI_PMH&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.OAI_PMH)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.OAI_PMH&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.OAI_PMH)




# Hercules.CommonsEDMA.OAI_PMH
OAI-PMH es un protocolo para la transmisión de metadatos por internet y su versión actual es la 2.0, de 2002.
El protocolo OAI-PMH presenta las siguientes características:
- Su funcionamiento se basa en una arquitectura cliente-servidor en la que un servicio recolector de metadatos pide información a un proveedor de datos.
- Las peticiones se expresan en HTTP utilizando únicamente los métodos GET o POST.
- Las respuestas tienen que ser documentos XML bien formados y codificados en UTF-8.
- Las fechas y los tiempos se codifican en ISO 8601 y se expresan en UTC.
- Soporta la difusión de registros en diversos formatos de metadatos.
- Tiene control de flujo.
- Cuando hay un error o una excepción los repositorios deben indicarlos distinguiéndolos de los códigos de estado HTTP.
![](../../Docs/media/OAI-PMH/protocolo-peticiones.png)

## Servicio OAI-PMH
El servicio OAI-PMH consta de varios métodos para obtener la información:
- **Identify**, para obtener información sobre el servidor.
- **ListSets**, para obtener registros pertenecientes a una clase determinada creada por el servidor.
- **ListMetadataFormats**, para obtener la lista de los formatos bibliográficos usados por el servidor.
- **ListIdentifiers**, para obtener encabezamientos (IDs) de los registros.
- **GetRecords**, para obtener un registro determinado mediante un encabezado (ID).
- **ListRecords**, para obtener registros completos.

Dichos métodos, por detrás hacen peticiones a un API encargada de obtener y ofrecer los datos pedidos.
Los diversas peticiones a las que se hacen referencia están documentadas en ["Tratamiento de datos"](https://confluence.um.es/confluence/display/HERCULES/Tratamiento+de+datos)

### Obtención del Token Bearer
Antes de hacer las peticiones a los servicios correspondientes, es necesario el acceso por token. Dicho token se pedirá automaticamente, teniendo un tiempo de expiración de cinco minutos. Tras estos cinco minutos se volververá a hacer la petición de obtención de token para refrescarlo.

### Identify
Permite recuperar información sobre un repositorio. No requiere ningún parámetro adicional de uso.

### ListSets
Obtiene el dato que especifica los criterios establecidos para la recolección selectiva. No requiere ningún parámetro adicional de uso.

### ListMetadataFormats
Permite obtener el metadataPrefix utilizado para especificar los encabezados que deben devolverse. No requiere ningún parámetro adicional de uso.

### ListIdentifiers
Devuelve una lista de identificadores de los datos solicitados (setSpec_ID) junto a la hora de actualización y el setSpec del dato solicitado.
Para la utilización de este método, es necesario los siguientes parámetros:
- **metadataPrefix**: Especifica que los encabezados deben devolverse solo si el formato de metadatos que coincide con el metadataPrefix proporcionado está disponible. Los formatos de metadatos admitidos por un repositorio y para un elemento en particular se pueden recuperar mediante la solicitud ListMetadataFormats. Ejemplo: EDMA
- **from**: Fecha de inicio desde la que se desean recuperar las cabeceras de las entidades (Codificado con ISO8601 y expresado en UTC, YYYY-MM-DD o YYYY-MM-DDThh:mm:ssZ). Ejemplo: 2022-01-01T00:00:00Z
- **until**: Fecha de fin hasta la que se desean recuperar las cabeceras de las entidades (Codificado con ISO8601 y expresado en UTC, YYYY-MM-DD o YYYY-MM-DDThh:mm:ssZ). Ejemplo: 2023-01-01T00:00:00Z
- **set**: Argumento con un valor setSpec, que especifica los criterios establecidos para la recolección selectiva. Los formatos de sets admitidos por un repositorio y para un elemento en particular se pueden recuperar mediante la solicitud ListSets. Ejemplo: Persona

### GetRecord
Devuelve los datos con el ID obtenido por el método ListIdentifiers.
Para la utilización de este método, es necesario los siguientes parámetros:
- **identifier**: Identificador de la entidad a recuperar (los identificadores se obtienen con el metodo ListIdentifiers). Ejemplo: Persona_ID-PERSONA
- **metadataPrefix**: Especifica que los encabezados deben devolverse solo si el formato de metadatos que coincide con el metadataPrefix proporcionado está disponible. Los formatos de metadatos admitidos por un repositorio y para un elemento en particular se pueden recuperar mediante la solicitud ListMetadataFormats. Ejemplo: EDMA

### ListRecords
Es una combinación de ListIdentifiers y GetRecords. Obtiene un listado con todos los records solicitados y sus metadatos.
Para la utilización de este método, es necesario los siguientes parámetros:
- **from**: Fecha de inicio desde la que se desean recuperar los datos (Codificado con ISO8601 y expresado en UTC, YYYY-MM-DD o YYYY-MM-DDThh:mm:ssZ). Ejemplo: 2022-01-01T00:00:00Z
- **until**: Fecha de fin hasta la que se desean recuperar los datos (Codificado con ISO8601 y expresado en UTC, YYYY-MM-DD o YYYY-MM-DDThh:mm:ssZ). Ejemplo: 2023-01-01T00:00:00Z
- **set**: Argumento con un valor setSpec, que especifica los criterios establecidos para la recolección selectiva. Los formatos de sets admitidos por un repositorio y para un elemento en particular se pueden recuperar mediante la solicitud ListSets. Ejemplo: Persona
- **metadataPrefix**: Especifica que los encabezados deben devolverse solo si el formato de metadatos que coincide con el metadataPrefix proporcionado está disponible. Los formatos de metadatos admitidos por un repositorio y para un elemento en particular se pueden recuperar mediante la solicitud ListMetadataFormats. Ejemplo: EDMA

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
  "AllowedHosts": "*",
  "UsernameToken": "",
  "PasswordToken": "",
  "UsernameTokenPII": "",
  "PasswordTokenPII": "",
  "ConfigUrl": "",
  "url_sgi": "",
  "CronExpression": ""
}
```
- **LogLevel.Default**: Nivel de error por defecto.
- **LogLevel.Microsoft**: Nivel de error para los errores propios de Microsoft.
- **LogLevel.Microsoft.Hosting.Lifetime**: Nivel de error para los errores de host.
- **UsernameToken**: Nombre de usuario para solicitar el token.
- **PasswordToken**: Contraseña para solicitar el token.
- **UsernameTokenPII**: Nombre de usuario para solicitar el token en PII.
- **PasswordTokenPII**: Contraseña para solicitar el token en PII.
- **ConfigUrl**: Url del servicio OAI-PMH instalado.
- **url_sgi**: Dominio dónde está instalado el sistema SGI. Ej: https://domniosgi.com
- **CronExpression**: Expresión en formato Cron para la ejecución del programa. Ej: 0 0 0 ? * * *

## Dependencias
- **IdentityServer4**: v4.1.2
- **IdentityServer4.AccessTokenValidation**: v3.0.1
- **Microsoft.AspNetCore.HttpOverrides**: v2.2.0
- **Microsoft.AspNetCore.Mvc.Core**: v2.2.5
- **Microsoft.AspNetCore.Mvc.Formatters.Json**: v2.2.0
- **Microsoft.AspNetCore.Mvc.NewtonsoftJson**: v5.0.10
- **OaiPmhNet**: v0.4.1
- **RestSharp**: v106.12.0
- **Swashbuckle.AspNetCore**: v6.2.2
- **Swashbuckle.AspNetCore.Annotations**: v6.2.2
- **System.ServiceModel.Duplex**: v4.8.1
- **System.ServiceModel.Http**: v4.8.1
- **System.ServiceModel.NetTcp**: v4.8.1
- **System.ServiceModel.Security**: v4.8.1
