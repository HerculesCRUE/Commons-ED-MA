![](./media/CabeceraDocumentosMD.png)

| Fecha                 | 05/09/2022                                |
| --------------------- | ---------------------------------------- |
| Título                | Edición de vistas                        |
| Descripción           | Guía de funcionamiento de la edición de vista|
| Versión               | 1.0                                      |
| Módulo                | Documentación                            |
| Tipo                  | Especificación                           |
| Cambios de la Versión | Versión inicial                          |

# Edición de vistas 

 - [Introducción](#introducción)
 - [Administración de vistas](#administración-de-vistas)
 - [Administración de traducciones](#administración-de-traducciones)
 - [Compilador de vistas](#compilador-de-vistas)


## Introducción
En esta documentación se explican la forma de depurar las vistas usadas en la plataforma (buscadores, páginas del CMS, fichas de consulta...)

## Administración de vistas
La creación/edición/eliminación de vistas se realiza desde la página {URL_COMUNIDAD}/administrar-vistas.  
Dentro de esta página hay varias secciones:
 - Plantillas de la web: Dentro de esta sección se encuentran las vistas de la web excepto las del CMS
   - Páginas generales: Aquí se encuentran las vistas generales (todas excepto las de las fichas de los recursos)
   - Fichas de recursos: Aqí se encuentran las vistas de las fichas de los recursos 
 - Plantillas del servicio de resultados: Dentro de esta sección se encuentran las vistas utilizadas en el servicio de resultados
 - Plantillas del servicio de facetas: Dentro de esta sección se encuentran las vistas utilizadas en el servicio de facetas
 - Plantillas de los componentes del CMS: Dentro de esta sección se encuentran las vistas de los componentes del CMS

![](./media/EdicionVistas/AdministrarVistas.jpg)

## Administración de traducciones
Desde esta página se pudeden editar los textos de las traducciones incluidos en las vistas desde la página {URL_COMUNIDAD}/administrar-traducciones.  

![](./media/EdicionVistas/AdministrarTraducciones.jpg)

## Compilador de vistas
El compilador de vistas nos servirá para depurar vistas y poder editar/crear vistas.  
Se encuentra en la siguiente URL: [Compilador de vistas](https://github.com/equipognoss/Gnoss.DevTools.ViewMaker)

Para depurar las vistas hay que copiar las vistas del repositorio [https://github.com/HerculesCRUE/HerculesED/tree/main/Web/Views](https://github.com/HerculesCRUE/HerculesED/tree/main/Web/Views) (Carpetas CMS, Recursos y Views) dentro de la solución dentro en /Views/hercules/

![](./media/EdicionVistas/CompiladorVistas.jpg)

Para cargar una página hay que configurar lo siguiente:
 - Url: La URL de la página que queremos depurar.
 - Autenticación Básica/Usuario: Usuario para la autenticación HTTP en caso de que esté configurada.
 - Autenticación Básica/Password: Contraseña para la autenticación HTTP en caso de que esté configurada.
 - SessionID: Si no se introduce se comportaría como si navegase un usuario invitado. Se puede introducir el ID de sesión de un usuario para que la página se cargue como si fuese ese usuario el que está navegando, para ello hay que recuperarlo de la cookie '.AspNetCore.Session'
![](./media/EdicionVistas/SessionID.jpg)
