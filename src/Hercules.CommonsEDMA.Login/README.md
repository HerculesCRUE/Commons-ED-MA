![](../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 21/06/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Configuración del Servicio de Login con SAML| 
|Descripción|Descripción de la configuración del Servicio de Login con SAML|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|


[![SonarCloud](https://sonarcloud.io/images/project_badges/sonarcloud-white.svg)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Login)

[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Login&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Login)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Login&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Login)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Login&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Login)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hercules.CommonsEDMA.Login&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hercules.CommonsEDMA.Login)




# Acerca de SAML

SAML es un tipo de servicio SSO (Single Sign On) capaz de intercambiar información del usuario mediante un identity provider y un service provider. 
La función principal es de dar seguridad y permisos a los distintos usuarios que naveguen por la web sin comprometer sus datos.

## Configuración en el appsettings.json
    {
	  "Saml2": {
		"IdPMetadata": "https://sso.um.es/cas/idp/metadata",
		"Issuer": "EDMA-SAML-DES",
		"SignatureAlgorithm": "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
		"CertificateValidationMode": "ChainTrust",
		"RevocationMode": "NoCheck"
	  }
    }
  
- Saml2.IdPMetadata: URL del metadata proporcionado por el Identity Provier.
- Saml2.Issuer: Identificador del proveedor.
- Saml2.SignatureAlgorithm: Algoritmo de encriptación.
- Saml2.CertificateValidationMode: Especifica el modo de validación del certificado.
- Saml2.RevocationMode: Establece el modo de revocación que especifica si se produce una comprobación de revocación.
