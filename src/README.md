![](../Docs/media/CabeceraDocumentosMD.png)

# Servicios Web y Back comunes a Hércules ED y Hércules MA

Contiene los servicios web y back que proporcionan funcionalidad propia de los proyectos [Hércules ED](https://github.com/HerculesCRUE/HerculesED) y [Hércules MA](https://github.com/HerculesCRUE/HerculesMA).

El listado de componentes es:

- [Hercules.CommonsEDMA.Load](./Hercules.CommonsEDMA.Load). Desarrollo del proceso de carga de los datos iniciales, en formato XML.
- [Hercules.CommonsEDMA.DisambiguationEngine](./Hercules.CommonsEDMA.DisambiguationEngine). Servicio encargado de la deduplicación de datos (investigadores, publicaciones, ROs, etc.
- [Hercules.CommonsEDMA.Login](./Hercules.CommonsEDMA.Login). Descripción de la configuración del Servicio de Login con SAML
- [Hercules.CommonsEDMA.OAI_PMH](./Hercules.CommonsEDMA.OAI_PMH). Servicio OAI-PMH para recolección de datos desde los sistemas de la universidad, Hércules SGI y otros.
- [Hercules.CommonsEDMA.Harvester](./Hercules.CommonsEDMA.Harvester). Servicio encargado de la carga de datos (investigadores, publicaciones, ROs, etc) desde Hércules SGI - Sistema de Gestión de Investigación (SGI).
- [Hercules.CommonsEDMA.ServicioExterno](./Hercules.CommonsEDMA.ServicioExterno). Servicio Web con funciones propias del proyecto: ofertas, cluster, gráficas y búsqueda.
