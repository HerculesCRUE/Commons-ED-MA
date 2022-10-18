![](../../../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 10/10/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Archivos de personalización del resultado de las facetas|
|Descripción|Vistas para la personalización del resultado de las facetas, la faceta en general y cada item|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|


# _ResultadoRecurso.cshtml
Archivo para la personalización de cada minificha de los recursos en las páginas de búsqueda.

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona las vistas:
- [**Index.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/Busqueda/Index.cshtml) - Vista general de resultados
- [**CargarResultados.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/CargadorResultados/CargarResultados.cshtml) - Vista para cargar los resultados

### Se relaciona con los servicios:
- **Servicio de resultados**

---
# CargarResultados.cshtml
Archivo para cargar los resultaldos de las minifichas de los recursos en las páginas de búsqueda.

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona las vistas:
- [**Index.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/Busqueda/Index.cshtml) - Vista general de resultados
- [**_ResultadoRecurso.cshtml**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Views/Views/CargadorResultados/CargarResultados.cshtml) - Vista de personalización de las minifichas de los recursos

### Se relaciona con los servicios:
- **Servicio de resultados**

---