using Gnoss.ApiWrapper.ApiModel;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models.Buscador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesMetaBusqueda : GnossGetMainResourceApiDataBase
    {

        #region --- Constantes

        public static int publicationsCount = 0;
        public static int researchObjectsCount = 0;
        public static int groupsCount = 0;
        public static int projectsCount = 0;
        public static int personsCount = 0;
        public static int offersCount = 0;

        public static Dictionary<string, List<ObjectSearch.Property>> textSearch = new();

        #endregion


        /// <summary>
        /// Busca los elementos necesarios y los guarda en una variable estática para realizar posteriormente la búsqueda en el metabuscador
        /// </summary>
        public void GenerateMetaShearch()
        {
            new Thread(delegate ()
            {
                while (true)
                {
                    try
                    {
                        //Aquí se almacenan los objetos buscables
                        List<Publication> publicationsTemp = new();
                        List<ResearchObject> researchObjectsTemp = new();
                        List<Group> groupsTemp = new();
                        List<Project> projectsTemp = new();
                        List<Person> personsTemp = new();
                        List<Offer> offersTemp = new();

                        Dictionary<string, List<ObjectSearch.Property>> textSearchTemp = new();

                        #region CargarInvestigadores

                        int limitInvestigadores = 10000;
                        int offsetInvestigadores = 0;
                        while (true)
                        {

                            string selectInvestigadores = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?name ?isActive ";
                            string whereInvestigadores = $@"  where
                                            {{
                                                ?id a 'person'.
                                                ?id foaf:name ?name.
                                                OPTIONAL{{?id roh:isActive ?isActive.}}
                                            }}ORDER BY asc(?name) asc(?id) }} LIMIT {limitInvestigadores} OFFSET {offsetInvestigadores}";

                            SparqlObject resultadoQueryInvestigadores = resourceApi.VirtuosoQuery(selectInvestigadores, whereInvestigadores, idComunidad);

                            if (resultadoQueryInvestigadores != null && resultadoQueryInvestigadores.results != null && resultadoQueryInvestigadores.results.bindings != null && resultadoQueryInvestigadores.results.bindings.Count > 0)
                            {
                                offsetInvestigadores += limitInvestigadores;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryInvestigadores.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    string nombre = fila["name"].value;
                                    bool isActive = false;
                                    if (fila.ContainsKey("isActive"))
                                    {
                                        isActive = fila["isActive"].value == "true";
                                    }

                                    Person person = new()
                                    {
                                        id = id,
                                        title = nombre,
                                        properties = new List<ObjectSearch.Property>(),
                                        searchable = isActive,
                                        publications = new List<Publication>(),
                                        researchObjects = new List<ResearchObject>(),
                                        groups = new List<Group>(),
                                        projects = new List<Project>(),
                                        offers = new List<Offer>(),
                                        order = nombre
                                    };

                                    person.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(nombre).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 1, person));
                                    personsTemp.Add(person);
                                }
                                if (resultadoQueryInvestigadores.results.bindings.Count < limitInvestigadores)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        personsCount = personsTemp.Count(x => x.searchable);

                        #endregion

                        #region CargarDocumentos

                        int limitDocumentos = 10000;
                        int offsetDocumentos = 0;
                        while (true)
                        {
                            string selectDocumentos = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?title ?fecha ?description ";
                            string whereDocumentos = $@"  where
                                            {{
                                                ?id a 'document'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                OPTIONAL{{ ?id bibo:abstract ?description}}
                                                OPTIONAL{{ ?id dct:issued ?fecha}}
                                            }}ORDER BY DESC(?fecha) DESC(?id) }} LIMIT {limitDocumentos} OFFSET {offsetDocumentos}";

                            SparqlObject resultadoQueryDocumentos = resourceApi.VirtuosoQuery(selectDocumentos, whereDocumentos, idComunidad);

                            if (resultadoQueryDocumentos != null && resultadoQueryDocumentos.results != null && resultadoQueryDocumentos.results.bindings != null && resultadoQueryDocumentos.results.bindings.Count > 0)
                            {
                                offsetDocumentos += limitDocumentos;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryDocumentos.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    string fecha = "";
                                    if (fila.ContainsKey("fecha"))
                                    {
                                        fecha = fila["fecha"].value;
                                    }

                                    Publication publication = publicationsTemp.FirstOrDefault(x => x.id == id);
                                    if (publication == null)
                                    {
                                        string title = fila["title"].value;
                                        string description = "";
                                        if (fila.ContainsKey("description"))
                                        {
                                            description = fila["description"].value;
                                        }
                                        publication = new Publication()
                                        {
                                            id = id,
                                            title = title,
                                            properties = new List<ObjectSearch.Property>(),
                                            order = fecha
                                        };

                                        publication.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(title).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 10000000, publication));
                                        publication.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(description).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 1000, publication));

                                        publicationsTemp.Add(publication);
                                    }
                                }
                                if (resultadoQueryDocumentos.results.bindings.Count < limitDocumentos)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        limitDocumentos = 10000;
                        offsetDocumentos = 0;
                        while (true)
                        {
                            string selectQuery1 = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?author ";
                            string whereQuery1 = $@"  where
                                            {{
                                                ?id a 'document'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                ?id bibo:authorList ?lista. 
                                                ?lista rdf:member ?author.
                                            }}ORDER BY DESC(?id) DESC(?author) }} LIMIT {limitDocumentos} OFFSET {offsetDocumentos}";

                            SparqlObject resultadoQuery1 = resourceApi.VirtuosoQuery(selectQuery1, whereQuery1, idComunidad);

                            if (resultadoQuery1 != null && resultadoQuery1.results != null && resultadoQuery1.results.bindings != null && resultadoQuery1.results.bindings.Count > 0)
                            {
                                offsetDocumentos += limitDocumentos;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery1.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    Guid author = new(fila["author"].value.Replace("http://gnoss/", ""));

                                    Publication publication = publicationsTemp.FirstOrDefault(x => x.id == id);
                                    if (publication != null)
                                    {
                                        Person person = personsTemp.FirstOrDefault(x => x.id == author);
                                        if (person != null)
                                        {
                                            person.publications.Add(publication);
                                        }
                                    }
                                }
                                if (resultadoQuery1.results.bindings.Count < limitDocumentos)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }


                        limitDocumentos = 10000;
                        offsetDocumentos = 0;
                        while (true)
                        {
                            string selectQuery2 = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?tag ";
                            string whereQuery2 = $@"  where
                                            {{
                                                ?id a 'document'.
                                                ?id roh:isValidated 'true'.
                                                ?id vivo:freeTextKeyword ?tagAux. ?tagAux roh:title ?tag
                                            }}ORDER BY DESC(?id) DESC(?tag) }} LIMIT {limitDocumentos} OFFSET {offsetDocumentos}";

                            SparqlObject resultadoQuery2 = resourceApi.VirtuosoQuery(selectQuery2, whereQuery2, idComunidad);

                            if (resultadoQuery2 != null && resultadoQuery2.results != null && resultadoQuery2.results.bindings != null && resultadoQuery2.results.bindings.Count > 0)
                            {
                                offsetDocumentos += limitDocumentos;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery2.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));

                                    Publication publication = publicationsTemp.FirstOrDefault(x => x.id == id);
                                    string tag = fila["tag"].value;
                                    if (publication != null)
                                    {
                                        publication.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(tag).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 10000, publication));
                                    }
                                }
                                if (resultadoQuery2.results.bindings.Count < limitDocumentos)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        publicationsCount = publicationsTemp.Count;

                        #endregion

                        #region CargarResearchObjects

                        int limitRO = 10000;
                        int offsetRO = 0;
                        while (true)
                        {
                            string selectRO = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?title ?fecha ?description ";
                            string whereRO = $@"  where
                                            {{
                                                ?id a 'researchobject'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                OPTIONAL{{ ?id bibo:abstract ?description}}
                                                OPTIONAL{{ ?id dct:issued ?fecha}}
                                            }}ORDER BY DESC(?fecha) DESC(?id) }} LIMIT {limitRO} OFFSET {offsetRO}";

                            SparqlObject resultadoQueryRO = resourceApi.VirtuosoQuery(selectRO, whereRO, idComunidad);

                            if (resultadoQueryRO != null && resultadoQueryRO.results != null && resultadoQueryRO.results.bindings != null && resultadoQueryRO.results.bindings.Count > 0)
                            {
                                offsetRO += limitRO;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryRO.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    string fecha = "";
                                    if (fila.ContainsKey("fecha"))
                                    {
                                        fecha = fila["fecha"].value;
                                    }

                                    ResearchObject researchObject = researchObjectsTemp.FirstOrDefault(x => x.id == id);
                                    if (researchObject == null)
                                    {
                                        string title = fila["title"].value;
                                        string description = "";
                                        if (fila.ContainsKey("description"))
                                        {
                                            description = fila["description"].value;
                                        }
                                        researchObject = new ResearchObject()
                                        {
                                            id = id,
                                            title = title,
                                            properties = new List<ObjectSearch.Property>(),
                                            order = fecha,
                                        };
                                        researchObject.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(title).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 10000000, researchObject));
                                        researchObject.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(description).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 1000, researchObject));

                                        researchObjectsTemp.Add(researchObject);
                                    }
                                }
                                if (resultadoQueryRO.results.bindings.Count < limitRO)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        limitRO = 10000;
                        offsetRO = 0;
                        while (true)
                        {
                            string selectQuery3 = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?author ";
                            string whereQuery3 = $@"  where
                                            {{
                                                ?id a 'researchobject'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                ?id bibo:authorList ?lista. 
                                                ?lista rdf:member ?author.
                                            }}ORDER BY DESC(?id) DESC(?author) }} LIMIT {limitRO} OFFSET {offsetRO}";

                            SparqlObject resultadoQuery3 = resourceApi.VirtuosoQuery(selectQuery3, whereQuery3, idComunidad);

                            if (resultadoQuery3 != null && resultadoQuery3.results != null && resultadoQuery3.results.bindings != null && resultadoQuery3.results.bindings.Count > 0)
                            {
                                offsetRO += limitRO;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery3.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    Guid author = new(fila["author"].value.Replace("http://gnoss/", ""));

                                    ResearchObject researchObject = researchObjectsTemp.FirstOrDefault(x => x.id == id);
                                    if (researchObject != null)
                                    {
                                        Person person = personsTemp.FirstOrDefault(x => x.id == author);
                                        if (person != null)
                                        {
                                            person.researchObjects.Add(researchObject);
                                        }
                                    }
                                }
                                if (resultadoQuery3.results.bindings.Count < limitRO)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        limitRO = 10000;
                        offsetRO = 0;
                        while (true)
                        {
                            string selectQuery4 = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?tag ";
                            string whereQuery4 = $@"  where
                                            {{
                                                ?id a 'researchobject'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                ?id vivo:freeTextKeyword ?tag
                                            }}ORDER BY DESC(?id) DESC(?tag) }} LIMIT {limitRO} OFFSET {offsetRO}";

                            SparqlObject resultadoQuery4 = resourceApi.VirtuosoQuery(selectQuery4, whereQuery4, idComunidad);

                            if (resultadoQuery4 != null && resultadoQuery4.results != null && resultadoQuery4.results.bindings != null && resultadoQuery4.results.bindings.Count > 0)
                            {
                                offsetRO += limitRO;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));

                                    ResearchObject researchObject = researchObjectsTemp.FirstOrDefault(x => x.id == id);
                                    string tag = fila["tag"].value;
                                    if (researchObject != null)
                                    {
                                        researchObject.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(tag).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 10000, researchObject));
                                    }
                                }
                                if (resultadoQuery4.results.bindings.Count < limitRO)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        researchObjectsCount = researchObjectsTemp.Count;

                        #endregion

                        #region CargarGrupo

                        int limitGrupo = 10000;
                        int offsetGrupo = 0;
                        while (true)
                        {
                            string selectGrupo = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?title ?description ";
                            string whereGrupo = $@"  where
                                            {{
                                                ?id a 'group'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                OPTIONAL{{ ?id vivo:description ?description}}  
                                            }}ORDER BY DESC(?title) DESC(?id) }} LIMIT {limitGrupo} OFFSET {offsetGrupo}";

                            SparqlObject resultadoQueryGrupo = resourceApi.VirtuosoQuery(selectGrupo, whereGrupo, idComunidad);

                            if (resultadoQueryGrupo != null && resultadoQueryGrupo.results != null && resultadoQueryGrupo.results.bindings != null && resultadoQueryGrupo.results.bindings.Count > 0)
                            {
                                offsetGrupo += limitGrupo;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryGrupo.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    Group group = groupsTemp.FirstOrDefault(e => e.id == id);

                                    if (group == null)
                                    {
                                        string title = fila["title"].value;
                                        string description = "";
                                        if (fila.ContainsKey("description"))
                                        {
                                            description = fila["description"].value;
                                        }

                                        group = new Group()
                                        {
                                            id = id,
                                            title = title,
                                            properties = new List<ObjectSearch.Property>(),
                                            order = title
                                        };

                                        group.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(title).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 10000, group));
                                        group.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(description).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 1000, group));

                                        groupsTemp.Add(group);
                                    }
                                }
                                if (resultadoQueryGrupo.results.bindings.Count < limitGrupo)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        limitGrupo = 10000;
                        offsetGrupo = 0;
                        while (true)
                        {
                            string selectQuery5 = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?author ";
                            string whereQuery5 = $@"  where
                                            {{
                                                ?id a 'group'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.   
                                                ?author a 'person'.                                                    
                                                ?id roh:membersGroup ?author.
                                            }}ORDER BY DESC(?id) DESC(?author) }} LIMIT {limitGrupo} OFFSET {offsetGrupo}";

                            SparqlObject resultadoQuery5 = resourceApi.VirtuosoQuery(selectQuery5, whereQuery5, idComunidad);

                            if (resultadoQuery5 != null && resultadoQuery5.results != null && resultadoQuery5.results.bindings != null && resultadoQuery5.results.bindings.Count > 0)
                            {
                                offsetGrupo += limitGrupo;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery5.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));

                                    string autorId = "";
                                    Guid author = new();
                                    if (fila.ContainsKey("author"))
                                    {
                                        autorId = fila["author"].value;
                                        if (autorId.Length > 0)
                                        {
                                            author = new Guid(autorId.Replace("http://gnoss/", ""));
                                        }
                                    }

                                    Group group = groupsTemp.FirstOrDefault(e => e.id == id);
                                    if (group != null)
                                    {
                                        Person person = personsTemp.FirstOrDefault(x => x.id == author);
                                        if (person != null)
                                        {
                                            person.groups.Add(group);
                                        }
                                    }
                                }
                                if (resultadoQuery5.results.bindings.Count < limitGrupo)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        groupsCount = groupsTemp.Count;

                        #endregion

                        #region CargarProyectos

                        int limitProyectos = 10000;
                        int offsetProyectos = 0;
                        while (true)
                        {
                            string selectProyectos = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?title ?description ";
                            string whereProyectos = $@"  where
                                            {{
                                                ?id a 'project'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                OPTIONAL{{ ?id vivo:description ?description}}
                                            }}ORDER BY DESC(?title) DESC(?id)  }} LIMIT {limitProyectos} OFFSET {offsetProyectos}";

                            SparqlObject resultadoQueryProyectos = resourceApi.VirtuosoQuery(selectProyectos, whereProyectos, idComunidad);

                            if (resultadoQueryProyectos != null && resultadoQueryProyectos.results != null &&
                            resultadoQueryProyectos.results.bindings != null && resultadoQueryProyectos.results.bindings.Count > 0)
                            {
                                offsetProyectos += limitProyectos;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryProyectos.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    Project project = projectsTemp.FirstOrDefault(e => e.id == id);

                                    if (project == null)
                                    {
                                        string title = fila["title"].value;
                                        string description = "";
                                        if (fila.ContainsKey("description"))
                                        {
                                            description = fila["description"].value;
                                        }

                                        project = new Project()
                                        {
                                            id = id,
                                            title = title,
                                            properties = new List<ObjectSearch.Property>(),
                                            order = title
                                        };

                                        project.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(title).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 10000, project));
                                        project.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(description).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 1000, project));

                                        projectsTemp.Add(project);
                                    }
                                }
                                if (resultadoQueryProyectos.results.bindings.Count < limitProyectos)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        limitProyectos = 10000;
                        offsetProyectos = 0;
                        while (true)
                        {
                            string selectQuery6 = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?author ";
                            string whereQuery6 = $@"  where
                                            {{
                                                ?id a 'project'.
                                                ?id roh:title ?title.
                                                ?id roh:isValidated 'true'.
                                                ?author a 'person'.
                                                ?id roh:membersProject  ?author.
                                            }}ORDER BY DESC(?id) DESC(?author) }} LIMIT {limitProyectos} OFFSET {offsetProyectos}";

                            SparqlObject resultadoQuery6 = resourceApi.VirtuosoQuery(selectQuery6, whereQuery6, idComunidad);

                            if (resultadoQuery6 != null && resultadoQuery6.results != null && resultadoQuery6.results.bindings != null && resultadoQuery6.results.bindings.Count > 0)
                            {
                                offsetProyectos += limitProyectos;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery6.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));

                                    string autorId = "";
                                    Guid author = new();
                                    if (fila.ContainsKey("author"))
                                    {
                                        autorId = fila["author"].value;
                                        if (autorId.Length > 0)
                                        {
                                            author = new Guid(autorId.Replace("http://gnoss/", ""));
                                        }
                                    }

                                    Project project = projectsTemp.FirstOrDefault(e => e.id == id);
                                    if (project != null)
                                    {
                                        Person person = personsTemp.FirstOrDefault(x => x.id == author);
                                        if (person != null)
                                        {
                                            person.projects.Add(project);
                                        }
                                    }
                                }
                                if (resultadoQuery6.results.bindings.Count < limitProyectos)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        projectsCount = projectsTemp.Count;

                        #endregion

                        #region CargarOfertas

                        int limitOfertas = 10000;
                        int offsetOfertas = 0;
                        while (true)
                        {
                            string selectOfertas = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?title ?fecha ?search ";
                            string whereOfertas = $@"  where
                                            {{
                                                ?id a 'offer'.
                                                ?id schema:name ?title.
                                                ?id schema:availability <http://gnoss.com/items/offerstate_003>.
                                                OPTIONAL{{ ?id roh:search ?search}}
                                                OPTIONAL{{ ?id dct:issued ?fecha}}
                                            }}ORDER BY DESC(?fecha) DESC(?id) }} LIMIT {limitOfertas} OFFSET {offsetOfertas}";

                            SparqlObject resultadoQueryOfertas = resourceApi.VirtuosoQuery(selectOfertas, whereOfertas, idComunidad);

                            if (resultadoQueryOfertas != null && resultadoQueryOfertas.results != null &&
                            resultadoQueryOfertas.results.bindings != null && resultadoQueryOfertas.results.bindings.Count > 0)
                            {
                                offsetOfertas += limitOfertas;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryOfertas.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    string fecha = "";
                                    if (fila.ContainsKey("fecha"))
                                    {
                                        fecha = fila["fecha"].value;
                                    }
                                    Offer offer = offersTemp.FirstOrDefault(x => x.id == id);
                                    if (offer == null)
                                    {
                                        string title = fila["title"].value;
                                        string search = "";
                                        if (fila.ContainsKey("search"))
                                        {
                                            search = fila["search"].value;
                                        }
                                        offer = new Offer()
                                        {
                                            id = id,
                                            title = title,
                                            properties = new List<ObjectSearch.Property>(),
                                            order = fecha
                                        };

                                        offer.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(title).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 10000, offer));
                                        offer.properties.Add(new ObjectSearch.Property(new HashSet<string>(ObtenerTextoNormalizado(search).Split(' ', StringSplitOptions.RemoveEmptyEntries)), 1000, offer));


                                        offersTemp.Add(offer);
                                    }
                                }
                                if (resultadoQueryOfertas.results.bindings.Count < limitOfertas)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        limitOfertas = 10000;
                        offsetOfertas = 0;
                        while (true)
                        {
                            string selectQuery7 = mPrefijos + "SELECT * WHERE { SELECT DISTINCT ?id ?author ";
                            string whereQuery7 = $@"  where
                                            {{
                                                ?id a 'offer'.
                                                ?id schema:availability <http://gnoss.com/items/offerstate_003>.
                                                ?id roh:researchers ?author.
                                            }}ORDER BY DESC(?id) DESC(?author) }} LIMIT {limitOfertas} OFFSET {offsetOfertas}";

                            SparqlObject resultadoQuery7 = resourceApi.VirtuosoQuery(selectQuery7, whereQuery7, idComunidad);

                            if (resultadoQuery7 != null && resultadoQuery7.results != null && resultadoQuery7.results.bindings != null && resultadoQuery7.results.bindings.Count > 0)
                            {
                                offsetOfertas += limitOfertas;
                                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery7.results.bindings)
                                {
                                    Guid id = new(fila["id"].value.Replace("http://gnoss/", ""));
                                    Guid author = new(fila["author"].value.Replace("http://gnoss/", ""));

                                    Offer offer = offersTemp.FirstOrDefault(x => x.id == id);
                                    if (offer != null)
                                    {
                                        Person person = personsTemp.FirstOrDefault(x => x.id == author);
                                        if (person != null)
                                        {
                                            person.offers.Add(offer);
                                        }
                                    }
                                }
                                if (resultadoQuery7.results.bindings.Count < limitOfertas)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        offersCount = offersTemp.Count;

                        #endregion

                        foreach (Person person in personsTemp)
                        {
                            LeerPropiedades(ref textSearchTemp, person.properties);
                        }

                        foreach (Publication publication in publicationsTemp)
                        {
                            LeerPropiedades(ref textSearchTemp, publication.properties);
                        }

                        foreach (ResearchObject researchObject in researchObjectsTemp)
                        {
                            LeerPropiedades(ref textSearchTemp, researchObject.properties);
                        }

                        foreach (Group group in groupsTemp)
                        {
                            LeerPropiedades(ref textSearchTemp, group.properties);
                        }

                        foreach (Project project in projectsTemp)
                        {
                            LeerPropiedades(ref textSearchTemp, project.properties);
                        }

                        foreach (Offer offer in offersTemp)
                        {
                            LeerPropiedades(ref textSearchTemp, offer.properties);
                        }

                        textSearch = textSearchTemp;

                        Thread.Sleep(300000);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(300000);
                    }
                }
            }).Start();
        }

        private static void LeerPropiedades(ref Dictionary<string, List<ObjectSearch.Property>> textSearchTemp, List<ObjectSearch.Property> listadoPropiedades)
        {
            foreach (ObjectSearch.Property prop in listadoPropiedades)
            {
                foreach (string text in prop.texts)
                {
                    if (!textSearchTemp.ContainsKey(text))
                    {
                        textSearchTemp[text] = new List<ObjectSearch.Property>();
                    }
                    textSearchTemp[text].Add(prop);
                }
            }
        }

        /// <summary>
        /// Busca los elementos necesarios y devuelve los resultados
        /// </summary>
        /// <param name="pStringBusqueda">string de búsqueda</param>
        /// <param name="pLang">Idioma</param>
        public Dictionary<string, KeyValuePair<bool, List<ObjectSearch>>> Busqueda(string pStringBusqueda, string pLang)
        {
            int maxItems = 3;
            Dictionary<string, KeyValuePair<bool, List<ObjectSearch>>> respuesta = new();

            pStringBusqueda = ObtenerTextoNormalizado(pStringBusqueda);
            string lastInput = "";
            HashSet<string> inputs = new();
            if (pStringBusqueda.EndsWith(" "))
            {
                inputs = new HashSet<string>(pStringBusqueda.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
            }
            else
            {
                if (pStringBusqueda.Contains(" "))
                {
                    inputs = new HashSet<string>(pStringBusqueda.Substring(0, pStringBusqueda.LastIndexOf(" ")).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                    lastInput = pStringBusqueda.Substring(pStringBusqueda.LastIndexOf(" ")).Trim();
                }
                else
                {
                    lastInput = pStringBusqueda;
                }

            }

            List<ObjectSearch.Property> propertiesAutocomplete = new();
            List<ObjectSearch.Property> propertiesSearch = new();
            foreach (string inputIn in inputs)
            {
                if (textSearch.ContainsKey(inputIn))
                {
                    List<ObjectSearch.Property> propertiesAux = textSearch[inputIn];
                    if (propertiesAux.Count == 0)
                    {
                        propertiesAutocomplete = new List<ObjectSearch.Property>();
                        propertiesSearch = new List<ObjectSearch.Property>();
                        break;
                    }
                    else
                    {
                        if (propertiesAutocomplete.Count == 0)
                        {
                            propertiesAutocomplete = propertiesAux;
                        }
                        else
                        {
                            propertiesAutocomplete = propertiesAutocomplete.Intersect(propertiesAux).ToList();
                        }
                        if (propertiesSearch.Count == 0)
                        {
                            propertiesSearch = propertiesAux;
                        }
                        else
                        {
                            propertiesSearch = propertiesSearch.Intersect(propertiesAux).ToList();
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(lastInput))
            {
                if ((inputs.Count > 0 && propertiesAutocomplete.Count > 0) || inputs.Count == 0)
                {
                    if (propertiesAutocomplete.Count > 0)
                    {
                        propertiesAutocomplete = propertiesAutocomplete.Intersect(textSearch.Where(x => x.Key.StartsWith(lastInput)).ToList().SelectMany(x => x.Value)).ToList();
                    }
                    else
                    {
                        propertiesAutocomplete = textSearch.Where(x => x.Key.StartsWith(lastInput)).ToList().SelectMany(x => x.Value).ToList();
                    }
                }
                if ((inputs.Count > 0 && propertiesSearch.Count > 0) || inputs.Count == 0)
                {
                    if (textSearch.ContainsKey(lastInput))
                    {
                        if (inputs.Count == 0)
                        {
                            propertiesSearch = textSearch[lastInput].ToList();
                        }
                        else
                        {
                            propertiesSearch = propertiesSearch.Intersect(textSearch[lastInput]).ToList();
                        }
                    }
                    else
                    {
                        propertiesSearch = new List<ObjectSearch.Property>();
                    }

                }
            }


            Dictionary<Person, int> personasFilter = new();
            Dictionary<Publication, int> publicacionesFilter = new();
            Dictionary<ResearchObject, int> researchObjectsFilter = new();
            Dictionary<Group, int> groupsFilter = new();
            Dictionary<Project, int> projectsFilter = new();
            Dictionary<Offer, int> offersFilter = new();
            bool personasSearch = false;
            bool publicacionesSearch = false;
            bool researchObjectsSearch = false;
            bool groupsSearch = false;
            bool projectsSearch = false;
            bool offersSearch = false;

            foreach (ObjectSearch.Property property in propertiesAutocomplete)
            {
                //Personas
                if (property.owner is Person)
                {
                    Person item = (Person)property.owner;
                    if (item.searchable)
                    {
                        if (!personasFilter.ContainsKey(item))
                        {
                            personasFilter.Add(item, 0);
                        }
                        personasFilter[item] += property.score;
                    }
                    if (item.publications.Count > 0)
                    {
                        foreach (Publication publication in item.publications)
                        {
                            if (!publicacionesFilter.ContainsKey(publication))
                            {
                                publicacionesFilter.Add(publication, 0);
                            }
                            publicacionesFilter[publication] += property.score;
                        }
                    }
                    if (item.researchObjects.Count > 0)
                    {
                        foreach (ResearchObject researchObject in item.researchObjects)
                        {
                            if (!researchObjectsFilter.ContainsKey(researchObject))
                            {
                                researchObjectsFilter.Add(researchObject, 0);
                            }
                            researchObjectsFilter[researchObject] += property.score;
                        }
                    }
                    if (item.groups.Count > 0)
                    {
                        foreach (Group group in item.groups)
                        {
                            if (!groupsFilter.ContainsKey(group))
                            {
                                groupsFilter.Add(group, 0);
                            }
                            groupsFilter[group] += property.score;
                        }
                    }
                    if (item.projects.Count > 0)
                    {
                        foreach (Project project in item.projects)
                        {
                            if (!projectsFilter.ContainsKey(project))
                            {
                                projectsFilter.Add(project, 0);
                            }
                            projectsFilter[project] += property.score;
                        }
                    }
                    if (item.offers.Count > 0)
                    {
                        foreach (Offer offer in item.offers)
                        {
                            if (!offersFilter.ContainsKey(offer))
                            {
                                offersFilter.Add(offer, 0);
                            }
                            offersFilter[offer] += property.score;
                        }
                    }
                }

                //Publicaciones
                if (property.owner is Publication)
                {
                    Publication itemPublication = (Publication)property.owner;
                    if (!publicacionesFilter.ContainsKey(itemPublication))
                    {
                        publicacionesFilter.Add(itemPublication, 0);
                    }
                    publicacionesFilter[itemPublication] += property.score;
                }

                //ResearchObjects
                if (property.owner is ResearchObject)
                {
                    ResearchObject itemRo = (ResearchObject)property.owner;
                    if (!researchObjectsFilter.ContainsKey(itemRo))
                    {
                        researchObjectsFilter.Add(itemRo, 0);
                    }
                    researchObjectsFilter[itemRo] += property.score;
                }

                //Grupos                
                if (property.owner is Group)
                {
                    Group itemGroup = (Group)property.owner;
                    if (!groupsFilter.ContainsKey(itemGroup))
                    {
                        groupsFilter.Add(itemGroup, 0);
                    }
                    groupsFilter[itemGroup] += property.score;
                }

                //Proyectos
                if (property.owner is Project)
                {
                    Project itemProject = (Project)property.owner;
                    if (!projectsFilter.ContainsKey(itemProject))
                    {
                        projectsFilter.Add(itemProject, 0);
                    }
                    projectsFilter[itemProject] += property.score;
                }

                //Ofertas
                if (property.owner is Offer)
                {
                    Offer itemOffer = (Offer)property.owner;
                    if (!offersFilter.ContainsKey(itemOffer))
                    {
                        offersFilter.Add(itemOffer, 0);
                    }
                    offersFilter[itemOffer] += property.score;
                }
            }

            foreach (ObjectSearch.Property property in propertiesSearch)
            {
                //Personas
                if (property.owner is Person)
                {
                    if (((Person)property.owner).searchable)
                    {
                        personasSearch = true;
                    }
                    if (((Person)property.owner).publications.Any())
                    {
                        publicacionesSearch = true;
                    }
                    if (((Person)property.owner).researchObjects.Any())
                    {
                        researchObjectsSearch = true;
                    }
                    if (((Person)property.owner).groups.Any())
                    {
                        groupsSearch = true;
                    }
                    if (((Person)property.owner).projects.Any())
                    {
                        projectsSearch = true;
                    }
                    if (((Person)property.owner).offers.Any())
                    {
                        offersSearch = true;
                    }
                }

                //Publicaciones
                if (property.owner is Publication)
                {
                    publicacionesSearch = true;
                }

                //ResearchObjects
                if (property.owner is ResearchObject)
                {
                    researchObjectsSearch = true;
                }

                //Grupos
                if (property.owner is Group)
                {
                    groupsSearch = true;
                }

                //Proyectos
                if (property.owner is Project)
                {
                    projectsSearch = true;
                }

                //Ofertas
                if (property.owner is Offer)
                {
                    offersSearch = true;
                }
            }

            personasFilter = personasFilter.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key.order).ToDictionary(x => x.Key, x => x.Value);
            publicacionesFilter = publicacionesFilter.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key.order).ToDictionary(x => x.Key, x => x.Value);
            researchObjectsFilter = researchObjectsFilter.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key.order).ToDictionary(x => x.Key, x => x.Value);
            groupsFilter = groupsFilter.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key.order).ToDictionary(x => x.Key, x => x.Value);
            projectsFilter = projectsFilter.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key.order).ToDictionary(x => x.Key, x => x.Value);
            offersFilter = offersFilter.OrderByDescending(x => x.Value).ThenByDescending(x => x.Key.order).ToDictionary(x => x.Key, x => x.Value);


            //Personas
            {
                int minPersonas = Math.Min(personasFilter.Count, maxItems);
                List<ObjectSearch> listaPersonas = new();
                foreach (Person item in personasFilter.Keys.ToList().GetRange(0, minPersonas))
                {
                    listaPersonas.Add(new Person() { title = item.title, id = item.id });
                }
                respuesta["persona"] = new KeyValuePair<bool, List<ObjectSearch>>(personasSearch, listaPersonas);
            }


            //Publicaciones
            {
                int minPublicaciones = Math.Min(publicacionesFilter.Count, maxItems);
                List<ObjectSearch> listaPublicaciones = new();
                foreach (Publication item in publicacionesFilter.Keys.ToList().GetRange(0, minPublicaciones))
                {
                    listaPublicaciones.Add(new Publication() { title = item.title, id = item.id });
                }
                respuesta["publicacion"] = new KeyValuePair<bool, List<ObjectSearch>>(publicacionesSearch, listaPublicaciones);
            }

            //ResearchObjects
            {
                int minRO = Math.Min(researchObjectsFilter.Count, maxItems);
                List<ObjectSearch> listaRO = new();
                foreach (ResearchObject item in researchObjectsFilter.Keys.ToList().GetRange(0, minRO))
                {
                    listaRO.Add(new ResearchObject() { title = item.title, id = item.id });
                }
                respuesta["researchObject"] = new KeyValuePair<bool, List<ObjectSearch>>(researchObjectsSearch, listaRO);
            }

            //Grupos
            {
                int minGrupos = Math.Min(groupsFilter.Count, maxItems);
                List<ObjectSearch> listaGrupos = new();
                foreach (Group item in groupsFilter.Keys.ToList().GetRange(0, minGrupos))
                {
                    listaGrupos.Add(new Group() { title = item.title, id = item.id });
                }
                respuesta["group"] = new KeyValuePair<bool, List<ObjectSearch>>(groupsSearch, listaGrupos);
            }

            //Proyectos
            {
                int minProyectos = Math.Min(projectsFilter.Count, maxItems);
                List<ObjectSearch> listaProyectos = new();
                foreach (Project item in projectsFilter.Keys.ToList().GetRange(0, minProyectos))
                {
                    listaProyectos.Add(new Project() { title = item.title, id = item.id });
                }
                respuesta["project"] = new KeyValuePair<bool, List<ObjectSearch>>(projectsSearch, listaProyectos);
            }

            //Ofertas
            {
                int minOfertas = Math.Min(offersFilter.Count, maxItems);
                List<ObjectSearch> listaOfertas = new();
                foreach (Offer item in offersFilter.Keys.ToList().GetRange(0, minOfertas))
                {
                    listaOfertas.Add(new Offer() { title = item.title, id = item.id });
                }
                respuesta["offer"] = new KeyValuePair<bool, List<ObjectSearch>>(offersSearch, listaOfertas);
            }

            List<Guid> ids = new();
            foreach (string key in respuesta.Keys)
            {
                ids = ids.Union(respuesta[key].Value.Select(x => x.id).ToList()).ToList();
            }
            List<ResponseGetUrl> enlaces = new();
            if (ids.Count > 0)
            {
                enlaces = resourceApi.GetUrl(ids, pLang);
            }

            foreach (string key in respuesta.Keys)
            {
                foreach (ObjectSearch item in respuesta[key].Value)
                {
                    item.url = enlaces.FirstOrDefault(x => x.resource_id == item.id)?.url;
                }
            }

            return respuesta;
        }

        /// <summary>
        /// Método que obtiene el número total de elementos de cada tipo (basado en el hilo que obtiene los datos de la búsqueda)
        /// </summary>
        /// <returns>Devuelve diccionario de 'tipo de items' => 'número de items'.</returns>
        public Dictionary<string, int> GetNumItems()
        {
            Dictionary<string, int> result = new();
            result["persons"] = personsCount;
            result["documents"] = publicationsCount;
            result["researchObjects"] = researchObjectsCount;
            result["groups"] = groupsCount;
            result["projects"] = projectsCount;
            result["offers"] = offersCount;
            return result;

        }

        private static string ObtenerTextoNormalizado(string pText)
        {
            string normalizedString = pText.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new();
            foreach (char charin in normalizedString)
            {
                if (char.IsLetterOrDigit(charin) || charin == ' ')
                {
                    sb.Append(charin);
                }
            }
            return sb.ToString().ToLower();
        }



    }
}
