![](../../../Docs/media/CabeceraDocumentosMD.png)

| Fecha         | 10/10/2022                                                   |
| ------------- | ------------------------------------------------------------ |
|Título|Archivos de personalización de las consultas SPARQL|
|Descripción|Vistas para la personalización de las secciones de consultas SPARQL en las páginas del CMS|
|Versión|1.0|
|Módulo|Documentación|
|Tipo|Especificación|
|Cambios de la Versión|Versión inicial|


# cluster.cshtml
Archivo que se usa para crear la ficha del cluster.
## Relaciones:
### Se relaciona al siguiente archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados


### Se relaciona con los servicios:
- **Servicio de Clusters**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.

---

# curriculumvitae.cshtml
Archivo que se usa para crear el editor del curriculum del investigador

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**edicioncv.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/edicioncv.js) - Archivo js encargado del editor cv.
- [**editorWysiwyg.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo que crea un editor de texto que permite el formateo basico del texto.

### Se relaciona con los servicios:
- **Servicio de resultados**
- **Servicio de facetas**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.
- [**Hercules.CommonsEDMA.EditorCV**](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV): Servicio encargado del editor CV del investigador.

---

# document.cshtml
Archivo que crea la ficha de recurso de las publicaciones o ROs.
## Relaciones:
### Se relaciona los siguientes archivos js:
- [**annotation.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/annotation.js) - Archivo encargado de la creacion y administracion de anotaciones.
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona con los servicios:
- **Servicio de similitudes**
- **Servicio de facetas**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.
---

# group.cshtml 
Archivo encargado de crear la ficha de los grupos

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona con los servicios:
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.


---

# offer.cshtml
Archivo encargado de la ficah de ofertas tecnológicas.

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**oferta.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/oferta.js) - Archivo encargado de las ofertas tecnológicas
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona con los servicios:
- **Servicio de ofertas**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.
- [**Hercules.CommonsEDMA.EditorCV**](https://github.com/HerculesCRUE/HerculesED/tree/main/src/Hercules.ED.EditorCV): Servicio encargado del editor CV del investigador.

---

# person.cshtml
Archivo que crea la ficha de los investigadores.

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados

### Se relaciona con los servicios:
- **Servicio de resultados**
- **Servicio de facetas**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.

# project.cshtml
Archivo encargado de los proyectos del investigador.

## Relaciones:
### Se relaciona los siguientes archivos js:
- [**ficharecurso.js**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/Web/Estilos/theme/ficharecurso.js) - Archivo para las fichas de los recursos, útil para crear, entre otros, buscadores personalizados


### Se relaciona con los servicios:
- **Servicio de resultados**
- **Servicio de facetas**
- [**Hercules.CommonsEDMA.ServicioExterno**](https://github.com/HerculesCRUE/Commons-ED-MA/blob/main/src/Hercules.CommonsEDMA.ServicioExterno): Servicio encargado de diversas utilidades relacionadas mayormente a peticiones desde las fichas de los diferentes recursos, asistentes en MA y otras funcionalidades.

---
