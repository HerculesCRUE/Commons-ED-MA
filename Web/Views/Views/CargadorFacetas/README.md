![](../../../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 10/10/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Archivos de personalización del resultado de las facetas|
|Descripción|Vistas para la personalización del resultado de las facetas, la faceta en general y cada item|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|


# _Faceta.cshtml
Archivo para personalizar las facetas del resultado de las búsquedas.

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona las vistas:
- [**Index.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/Busqueda/Index.cshtml) - Vista general de resultados
- [**_ItemFaceta.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/CargadorFacetas/_ItemFaceta.cshtml) - Detalle de cada item de las facetas

### Se relaciona con los servicios:
- **Servicio de facetas**

---
# _ItemFaceta.cshtml
Archivo para personalizar los items de facetas del resultado de las búsquedas.

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona las vistas:
- [**Index.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/Busqueda/Index.cshtml) - Vista general de resultados
- [**_Faceta.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/CargadorFacetas/_Faceta.cshtml) - Personalización de las facetas

### Se relaciona con los servicios:
- **Servicio de facetas**

---