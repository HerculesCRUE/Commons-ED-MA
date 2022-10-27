using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Hercules.CommonsEDMA.ServicioExterno.Models.Cluster;
using ClusterOntology;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hercules.CommonsEDMA.ServicioExterno.Models.Cluster.Cluster;
using Hercules.CommonsEDMA.ServicioExterno.Models;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Microsoft.AspNetCore.Cors;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    [EnableCors("_myAllowSpecificOrigins")]
    public class AccionesCluster : GnossGetMainResourceApiDataBase
    {


        /// <summary>
        /// Método público que obtiene una lista de thesaurus.
        /// </summary>
        /// <param name="listadoCluster">Listado de thesaurus a obtener.</param>
        /// <returns>Diccionario con las listas de thesaurus.</returns>
        public Dictionary<string, List<ThesaurusItem>> GetListThesaurus(string listadoCluster)
        {

            List<string> thesaurusTypes = new List<string>() { "researcharea" };

            try
            {
                if (listadoCluster != "")
                {
                    thesaurusTypes = JsonConvert.DeserializeObject<List<string>>(listadoCluster);
                }
            }
            catch (Exception ex)
            {
                resourceApi.Log.Error("El texto que ha introducido no corresponde a un json válido");
                resourceApi.Log.Error("Excepcion: " + ex.Message);
            }

            var thesaurus = UtilidadesAPI.GetTesauros(resourceApi, thesaurusTypes);

            return thesaurus;
        }


        /// <summary>
        /// Método público para guardar / editar un cluster
        /// </summary>
        /// <param name="pIdGnossUser">Identificador del usuario</param>
        /// <param name="cluster">Objecto cluster</param>
        /// <returns>Diccionario con las listas de thesaurus.</returns>
        public string SaveCluster(string pIdGnossUser, Models.Cluster.Cluster cluster)
        {
            string idRecurso = cluster.entityID;
            int MAX_INTENTOS = 10;
            bool uploadedR = false;

            // Obtener el id del usuario usando el id de la cuenta
            string select = "select ?s ";
            string where = @$"where {{
                    ?s a <http://xmlns.com/foaf/0.1/Person>.
                    ?s <http://w3id.org/roh/gnossUser> ?idGnoss.
                    FILTER(?idGnoss = <http://gnoss/{pIdGnossUser.ToUpper()}>)
                }}";
            SparqlObject sparqlObject = resourceApi.VirtuosoQuery(select, where, "person");
            var userGnossId = string.Empty;
            sparqlObject.results.bindings.ForEach(e =>
            {
                userGnossId = e["s"].value;
            });

            if (!string.IsNullOrEmpty(userGnossId))
            {
                // Creando el objeto del cluster
                // Creando las categorías
                List<CategoryPath> categorias = new List<CategoryPath>();
                categorias.Add(new CategoryPath() { IdsRoh_categoryNode = cluster.terms });

                List<ClusterPerfil> listClusterPerfil = new();
                // Creando los perfiles del cluster
                if (cluster.profiles != null)
                {

                    // Get the full ID
                    List<string> numMember = new();
                    Dictionary<string, string> relationIDs = new();
                    cluster.profiles.ForEach(e =>
                    {
                        if (e.users != null)
                        {
                            foreach (var us in e.users)
                            {
                                if (us.userID != null && us.shortUserID != null)
                                {
                                    relationIDs["http://gnoss.com/" + us.shortUserID] = us.userID;
                                }
                                else
                                {
                                    numMember.Add("<http://gnoss.com/" + us.shortUserID + ">");
                                }
                            }
                        }
                    });
                    numMember = numMember.Distinct().ToList();

                    // Query to get the full ID
                    if (numMember.Count > 0)
                    {

                        select = "select distinct ?s ?entidad";
                        where = @$"where {{
                            ?s <http://gnoss/hasEntidad> ?entidad.
                            ?entidad a<http://xmlns.com/foaf/0.1/Person>.
                            FILTER(?s in ({string.Join(',', numMember)}))
                        }}
                        ";

                        sparqlObject = resourceApi.VirtuosoQuery(select, where, "person");
                        sparqlObject.results.bindings.ForEach(e =>
                        {
                            relationIDs.Add(e["s"].value, e["entidad"].value);
                        });
                    }
                    try
                    {

                        // Create the list of profiles
                        listClusterPerfil = cluster.profiles.Select(e =>
                        {
                            List<string> theUsersP = new List<string>();
                            if (e.users != null)
                            {
                                theUsersP = e.users.Select(x => relationIDs.ContainsKey(("http://gnoss.com/" + x.shortUserID)) ? relationIDs[("http://gnoss.com/" + x.shortUserID)] : "http://gnoss.com/" + x.shortUserID).ToList();
                            }
                            List<CategoryPath> knowledge = e.terms.Select(term => new CategoryPath() { IdsRoh_categoryNode = new List<string>() { term } }).ToList();
                            var clsp = new ClusterPerfil()
                            {
                                Roh_title = e.name,
                                Roh_hasKnowledgeArea = knowledge,
                                IdsRdf_member = theUsersP,
                                Vivo_freeTextKeyword = e.tags
                            };
                            if (e.entityID.StartsWith("http"))
                            {
                                clsp.GNOSSID = e.entityID;
                            }
                            return clsp;
                        }).ToList();


                    }
                    catch (Exception ex)
                    {
                        resourceApi.Log.Error("Excepcion: " + ex.Message);
                    }
                }


                // creando los cluster
                ClusterOntology.Cluster cRsource = new();
                cRsource.IdRdf_member = userGnossId;
                cRsource.Roh_title = cluster.name;
                cRsource.Vivo_description = cluster.description;
                cRsource.Roh_hasKnowledgeArea = categorias;
                cRsource.Roh_clusterPerfil = listClusterPerfil.ToList();
                cRsource.Dct_issued = DateTime.UtcNow;

                resourceApi.ChangeOntoly("cluster");

                if (idRecurso != null && idRecurso != "")
                {
                    string[] recursoSplit = idRecurso.Split('_');

                    // Modificación.
                    ComplexOntologyResource resource = cRsource.ToGnossApiResource(resourceApi, null, new Guid(recursoSplit[recursoSplit.Length - 2]), new Guid(recursoSplit[recursoSplit.Length - 1]));
                    int numIntentos = 0;
                    while (!resource.Modified)
                    {
                        numIntentos++;
                        if (numIntentos > MAX_INTENTOS)
                        {
                            break;
                        }

                        resourceApi.ModifyComplexOntologyResource(resource, false, false);
                        uploadedR = resource.Modified;
                    }

                }
                else
                {
                    // Inserción.
                    ComplexOntologyResource resource = cRsource.ToGnossApiResource(resourceApi, null);
                    int numIntentos = 0;
                    while (!resource.Uploaded)
                    {
                        numIntentos++;
                        if (numIntentos > MAX_INTENTOS)
                        {
                            break;
                        }
                        idRecurso = resourceApi.LoadComplexSemanticResource(resource, false, true);
                        uploadedR = resource.Uploaded;
                    }
                }
            }

            if (uploadedR)
            {
                return idRecurso;
            }
            else
            {
                throw new Exception("Recurso no creado");
            }
        }



        /// <summary>
        /// Método público para eliminar un cluster
        /// </summary>
        /// <param name="pIdClusterId">Identificador del usuario</param>
        /// <returns>Diccionario con las listas de thesaurus.</returns>
        public bool BorrarCluster(string pIdClusterId)
        {

            if (!string.IsNullOrEmpty(pIdClusterId))
            {
                // Carga los datos del Cluster
                Models.Cluster.Cluster clusterData = LoadCluster(pIdClusterId);

                // Obtengo los perfiles
                List<Guid> perfiles = clusterData.profiles.Select(e => resourceApi.GetShortGuid(e.entityID)).ToList();
                // Establezco las entidades secundarias a borrar
                List<string> urlSecondaryListEntities = new() { "http://w3id.org/roh/categoryNode" };

                resourceApi.ChangeOntoly("cluster");

                if (pIdClusterId != null && pIdClusterId != "")
                {
                    Guid resourceGuid = resourceApi.GetShortGuid(pIdClusterId);

                    try
                    {
                        resourceApi.CommunityShortName = resourceApi.GetCommunityShortNameByResourceID(resourceGuid);

                        // Establece las entidades secundarias a borrar
                        resourceApi.DeleteSecondaryEntitiesList(ref urlSecondaryListEntities);
                        // Borra los perfiles
                        // perfiles.ForEach(e => resourceApi.PersistentDelete(e));
                        // borra el recurso
                        resourceApi.PersistentDelete(resourceGuid);
                    }
                    catch (Exception ex)
                    {
                        resourceApi.Log.Error("Excepcion: " + ex.Message);
                        return false;
                    }

                }
            }
            else
            {
                throw new Exception("Recurso no creado");
            }
            return true;
        }


        /// <summary>
        /// Método público para obtener los datos de un cluster
        /// </summary>
        /// <param name="pIdClusterId">Identificador del cluster</param>
        /// <param name="loadUsuarios"></param>
        /// <returns>Diccionario con las listas de thesaurus.</returns>
        internal Models.Cluster.Cluster LoadCluster(string pIdClusterId, bool loadUsuarios = true)
        {

            // Obtener datos del cluster
            string select = "select ?p ?o ";
            string where = @$"where {{
                    ?s a <http://w3id.org/roh/Cluster>.
                    ?s ?p ?o.
                    FILTER(?s = <{pIdClusterId}>)
                }}";
            SparqlObject sparqlObject = resourceApi.VirtuosoQuery(select, where, "cluster");

            // Inicizalizamos el modelo del Cluster para devolver
            Models.Cluster.Cluster pDataCluster = new();
            pDataCluster.terms = new();
            pDataCluster.profiles = new();

            // Lista de los ids de los perfiles devuelto por la consulta
            List<string> perfiles = new();

            sparqlObject.results.bindings.ForEach(e =>
            {
                pDataCluster.entityID = pIdClusterId;

                switch (e["p"].value)
                {
                    case "http://w3id.org/roh/title":
                        pDataCluster.name = e["o"].value;
                        break;
                    case "http://vivoweb.org/ontology/core#description":
                        pDataCluster.description = e["o"].value;
                        break;
                    case "http://w3id.org/roh/hasKnowledgeArea":
                        pDataCluster.terms.Add(e["o"].value);
                        break;
                    case "http://w3id.org/roh/clusterPerfil":
                        perfiles.Add(e["o"].value);
                        break;
                    case "http://purl.org/dc/terms/issued":
                        pDataCluster.fecha = e["o"].value;
                        break;
                }
            });



            // Obtenemos todos los datos de las areas temáticas
            if (pDataCluster.terms.Count > 0)
            {
                pDataCluster.terms = UtilidadesAPI.LoadCurrentTerms(resourceApi, pDataCluster.terms, "cluster");
            }


            // Obtenemos todos los datos de los perfiles
            foreach (string p in perfiles)
            {
                //Datos del perfil (nombre, categorías y tags)
                select = "select distinct ?s ?title group_concat(distinct ?freeTextKeyword;separator=',') as ?freeTextKeywordGroup group_concat(distinct ?KnowledgeArea;separator=',') as ?knowledgeAreaGroup";
                where = @$"where {{
                    ?s a <http://w3id.org/roh/ClusterPerfil>.
                    ?s <http://w3id.org/roh/title> ?title.
                    OPTIONAL
                    {{
                        ?s <http://vivoweb.org/ontology/core#freeTextKeyword> ?freeTextKeyword.
                    }}
                    OPTIONAL
                    {{
                        ?s <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                        ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?KnowledgeArea.
                    }}
                    FILTER(?s = <{p}>)
                }}";
                sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "cluster" ,"person"});


                PerfilCluster perfilCluster = new();
                perfilCluster.tags = new();
                perfilCluster.terms = new();
                perfilCluster.users = new();

                // Carga los datos en el objeto
                sparqlObject.results.bindings.ForEach(e =>
                {
                    perfilCluster.entityID = e["s"].value;
                    perfilCluster.name = e["title"].value;
                    try
                    {
                        perfilCluster.tags = e["freeTextKeywordGroup"].value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    catch (Exception ex)
                    {
                        resourceApi.Log.Error("Excepcion: " + ex.Message);
                        perfilCluster.tags = new();
                    }
                    try
                    {
                        perfilCluster.terms = e["knowledgeAreaGroup"].value.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();
                    }
                    catch (Exception ex)
                    {
                        resourceApi.Log.Error("Excepcion: " + ex.Message);
                        perfilCluster.terms = new();
                    }
                    perfilCluster.users = new List<PerfilCluster.UserCluster>();
                });


                if (loadUsuarios)
                {

                    //Datos de los miembros
                    select = "select distinct ?memberPerfil ?nombreUser ?hasPosition ?tituloOrg ?departamento (count(distinct ?doc)) as ?numDoc (count(distinct ?proj)) as ?ipNumber ";
                    where = @$"where {{
                    ?s <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?memberPerfil.
                    ?memberPerfil <http://xmlns.com/foaf/0.1/name> ?nombreUser.
                    OPTIONAL {{
                        ?doc a <http://purl.org/ontology/bibo/Document>.
                        ?doc <http://w3id.org/roh/isValidated> 'true'.
                        ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
                        ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?memberPerfil.
                    }}
                    OPTIONAL {{
                        ?proj a <http://vivoweb.org/ontology/core#Project>.
                        ?proj <http://w3id.org/roh/isValidated> 'true'.
                        ?proj <http://vivoweb.org/ontology/core#relates> ?listprojauth.
                        ?listprojauth <http://w3id.org/roh/roleOf> ?memberPerfil.
                        ?listprojauth <http://w3id.org/roh/isIP> 'true'.
                    }}
                    OPTIONAL {{
                        ?memberPerfil <http://w3id.org/roh/hasPosition> ?hasPosition.
                    }}
                    OPTIONAL {{
                        ?memberPerfil <http://vivoweb.org/ontology/core#departmentOrSchool> ?dept.
                        ?dept <http://purl.org/dc/elements/1.1/title> ?departamento
                    }}
                    OPTIONAL {{
                        ?memberPerfil <http://w3id.org/roh/hasRole> ?org.
                        ?org <http://w3id.org/roh/title> ?tituloOrg
                    }}
                    FILTER(?s = <{p}>)
                }}";
                    sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where,new List<string>{ "cluster","person","document","project","organization","department"});

                    // Carga los datos en el objeto
                    sparqlObject.results.bindings.ForEach(e =>
                    {
                        List<string> infoList = new List<string>(); ;
                        if (e.ContainsKey("hasPosition"))
                        {
                            infoList.Add(e["hasPosition"].value);
                        }
                        if (e.ContainsKey("tituloOrg"))
                        {
                            infoList.Add(e["tituloOrg"].value);
                        }
                        if (e.ContainsKey("departamento"))
                        {
                            infoList.Add(e["departamento"].value);
                        }
                        string info = string.Join(", ", infoList);
                        perfilCluster.users.Add(new PerfilCluster.UserCluster()
                        {
                            userID = e["memberPerfil"].value,
                            name = e["nombreUser"].value,
                            shortUserID = resourceApi.GetShortGuid(e["memberPerfil"].value).ToString().ToLower(),
                            numPublicacionesTotal = e.ContainsKey("numDoc") ? int.Parse(e["numDoc"].value) : 0,
                            ipNumber = e.ContainsKey("ipNumber") ? int.Parse(e["ipNumber"].value) : 0,
                            info = info
                        });
                    });

                }


                // Añade el perfil creado a los datos del cluster
                pDataCluster.profiles.Add(perfilCluster);
            }

            return pDataCluster;
        }


        /// <summary>
        /// Método público para cargar los perfiles de cada investigador sugerido del cluster
        /// </summary>
        /// <param name="pDataCluster">Datos del cluster para obtener los perfiles</param>
        /// <param name="pPersons">Listado de personas sobre los que pedir información</param>
        /// <returns>Diccionario con los datos necesarios para cada persona por cluster.</returns>
        public Dictionary<string, Dictionary<string, ScoreCluster>> LoadProfiles(Models.Cluster.Cluster pDataCluster, List<string> pPersons)
        {
            //ID persona/ID perfil/score
            Dictionary<string, Dictionary<string, ScoreCluster>> respuesta = new Dictionary<string, Dictionary<string, ScoreCluster>>();

            List<string> filtrosPerfiles = new List<string>();
            List<string> filtrosPerfilesTerms = new List<string>();
            List<string> filtrosPerfilesTags = new List<string>();

            // Genera la consulta para cada perfil
            foreach (PerfilCluster perfilCluster in pDataCluster.profiles)
            {
                // Inicializa cada respuesta
                foreach (string person in pPersons)
                {
                    if (!respuesta.ContainsKey(person))
                    {
                        respuesta.Add(person, new Dictionary<string, ScoreCluster>());
                    }
                    if (!respuesta[person].ContainsKey(perfilCluster.entityID))
                    {
                        respuesta[person].Add(perfilCluster.entityID, new ScoreCluster());
                    }
                }


                // Establece las variables para la consulta
                // Añade la consulta para las categorías de los perfiles, si no dispone de categorías, usará las del cluster
                string filtroCategorias = "";
                if (perfilCluster.terms != null && perfilCluster.terms.Count > 0)
                {
                    filtroCategorias = $@"  {{
					                            ?doc <http://w3id.org/roh/hasKnowledgeArea> ?area.
					                            ?area <http://w3id.org/roh/categoryNode> ?node.
					                            FILTER(?node in(<{string.Join(">,<", perfilCluster.terms)}>))
				                            }}";
                }
                else if (pDataCluster.terms != null && pDataCluster.terms.Count > 0)
                {
                    filtroCategorias = $@"  {{
					                            ?doc <http://w3id.org/roh/hasKnowledgeArea> ?area.
					                            ?area <http://w3id.org/roh/categoryNode> ?node.
					                            FILTER(?node in(<{string.Join(">,<", pDataCluster.terms)}>))
				                            }}";
                }

                // Añade la consulta para los descriptores de los perfiles
                string filtroTags = "";
                if (perfilCluster.tags != null && perfilCluster.tags.Count > 0)
                {
                    filtroTags = $@"   {{
				                            ?doc <http://vivoweb.org/ontology/core#freeTextKeyword>  ?keywordO.
                                            ?keywordO <http://w3id.org/roh/title> ?tag.
				                            FILTER(?tag in('{string.Join("','", perfilCluster.tags)}'))
				                        }}";
                }

                // Termina de formar la consulta de condiciones del perfil
                string union = "";
                if (!string.IsNullOrEmpty(filtroCategorias) && !string.IsNullOrEmpty(filtroTags))
                {
                    union = "UNION";
                }
                string filtroPerfil = $@"   {{
                                                BIND('{perfilCluster.entityID}' as ?perfil)
                                                {filtroCategorias}
                                                {union}
                                                {filtroTags}
                                            }}";
                filtrosPerfiles.Add(filtroPerfil);


                string filtroPerfilTerm = $@"   {{
                                                BIND('{perfilCluster.entityID}' as ?perfil)
                                                {filtroCategorias}
                                            }}";
                filtrosPerfilesTerms.Add(filtroPerfilTerm);


                string filtroPerfilTag = $@"   {{
                                                BIND('{perfilCluster.entityID}' as ?perfil)
                                                {filtroTags}
                                            }}";
                filtrosPerfilesTags.Add(filtroPerfilTag);



            }

            string select = "select ?person ?perfil (count(distinct ?node) + count(distinct ?tag)) as ?scoreAux ";
            string where = @$"where {{
                    ?doc a 'document'.
                    ?doc <http://w3id.org/roh/isValidated> 'true'.
				    ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
				    ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
				    ?person a 'person'.
                    ?person <http://w3id.org/roh/isActive> 'true'.
                    FILTER(?person in (<http://gnoss/{string.Join(">,<http://gnoss/", pPersons.Select(x => x.ToUpper()))}>))
                    {string.Join("UNION", filtrosPerfiles)}
                }}";
            SparqlObject sparqlObject = resourceApi.VirtuosoQuery(select, where, idComunidad);

            foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
            {
                string person = fila["person"].value.Replace("http://gnoss/", "").ToLower();
                string perfil = fila["perfil"].value;
                PerfilCluster perfilCluster = pDataCluster.profiles.FirstOrDefault(x => x.entityID == perfil);
                float scoreAux = float.Parse(fila["scoreAux"].value);
                float scoreMax = 0;
                if (perfilCluster != null && perfilCluster.tags != null)
                {
                    scoreMax += perfilCluster.tags.Count;
                }
                if (perfilCluster != null && perfilCluster.terms != null)
                {
                    scoreMax += perfilCluster.terms.Count;
                }
                float scoreAjuste = scoreAux / scoreMax;
                respuesta[person][perfil].ajuste = scoreAjuste;
            }

            // Obtener los documentos por cada categoría
            select = "select ?person ?perfil ?node (count(distinct ?doc)) as ?numDoc";
            where = @$"where {{
                    ?doc a 'document'.
                    ?doc <http://w3id.org/roh/isValidated> 'true'.
				    ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
				    ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
				    ?person a 'person'.
                    ?person <http://w3id.org/roh/isActive> 'true'.
                    FILTER(?person in (<http://gnoss/{string.Join(">,<http://gnoss/", pPersons.Select(x => x.ToUpper()))}>))
                    {string.Join("UNION", filtrosPerfilesTerms)}
                }}";
            sparqlObject = resourceApi.VirtuosoQuery(select, where, idComunidad);

            foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
            {
                string person = fila["person"].value.Replace("http://gnoss/", "").ToLower();
                string perfil = fila["perfil"].value;
                int numDoc = int.Parse(fila["numDoc"].value);
                if (respuesta[person][perfil].terms == null)
                {
                    respuesta[person][perfil].terms = new();
                }
                if (fila.ContainsKey("node"))
                {
                    string node = fila["node"].value;
                    respuesta[person][perfil].terms.Add(node, numDoc);
                }
                respuesta[person][perfil].numPublicaciones += numDoc;

            }

            // Obtener los documentos por cada tag
            select = "select ?person ?perfil ?tag (count(distinct ?doc)) as ?numDoc";
            where = @$"where {{
                    ?doc a 'document'.
                    ?doc <http://w3id.org/roh/isValidated> 'true'.
				    ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
				    ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
				    ?person a 'person'.
                    ?person <http://w3id.org/roh/isActive> 'true'.
                    FILTER(?person in (<http://gnoss/{string.Join(">,<http://gnoss/", pPersons.Select(x => x.ToUpper()))}>))
                    {string.Join("UNION", filtrosPerfilesTags)}
                }}";
            sparqlObject = resourceApi.VirtuosoQuery(select, where, idComunidad);

            foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
            {
                string person = fila["person"].value.Replace("http://gnoss/", "").ToLower();
                string perfil = fila["perfil"].value;

                int numDoc = int.Parse(fila["numDoc"].value);
                if (respuesta[person][perfil].tags == null)
                {
                    respuesta[person][perfil].tags = new();
                }
                if (fila.ContainsKey("tag"))
                {
                    string node = fila["tag"].value;
                    respuesta[person][perfil].tags.Add(node, numDoc);
                }
                respuesta[person][perfil].numPublicaciones += numDoc;
            }


            // Obtener el número de veces que aparecen documentos con los diferentes tags y categorías
            select = "select ?person ?perfil (count(distinct ?doc)) as ?numDoc (count(distinct ?proj)) as ?ipNumber ";
            where = @$"where {{
                        ?person a 'person'.
                        FILTER(?person in (<http://gnoss/{string.Join(">,<http://gnoss/", pPersons.Select(x => x.ToUpper()))}>))
                        OPTIONAL {{
                            ?doc a 'document'.
                            ?doc <http://w3id.org/roh/isValidated> 'true'.
                            ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
                            ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                        }}
                        OPTIONAL {{
                            ?proj a 'project'.
                            ?proj <http://w3id.org/roh/isValidated> 'true'.
                            ?proj <http://vivoweb.org/ontology/core#relates> ?listprojauth.
                            ?listprojauth <http://w3id.org/roh/roleOf> ?person.
                            ?listprojauth <http://w3id.org/roh/isIP> 'true'.
                        }}
                        {string.Join("UNION", filtrosPerfiles)}
                }}";
            sparqlObject = resourceApi.VirtuosoQuery(select, where, idComunidad);

            foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
            {
                string person = fila["person"].value.Replace("http://gnoss/", "").ToLower();
                string perfil = fila["perfil"].value;
                respuesta[person][perfil].numPublicaciones = fila.ContainsKey("numDoc") && fila["numDoc"].value != null ? int.Parse(fila["numDoc"].value) : 0;
                respuesta[person][perfil].ipNumber = fila.ContainsKey("ipNumber") && fila["ipNumber"].value != null ? int.Parse(fila["ipNumber"].value) : 0;
            }


            // Obtener el número de documentos totales por autor
            select = "select ?person (count(distinct ?doc)) as ?numDoc ";
            where = @$"where {{
                    ?doc a 'document'.
                    ?doc <http://w3id.org/roh/isValidated> 'true'.
				    ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
				    ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
				    ?person a 'person'.
                    ?person <http://w3id.org/roh/isActive> 'true'.
                    FILTER(?person in (<http://gnoss/{string.Join(">,<http://gnoss/", pPersons.Select(x => x.ToUpper()))}>))
                }}";
            sparqlObject = resourceApi.VirtuosoQuery(select, where, idComunidad);

            foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
            {
                string person = fila["person"].value.Replace("http://gnoss/", "").ToLower();
                int numDoc = int.Parse(fila["numDoc"].value);
                foreach (string perfil in respuesta[person].Keys)
                {
                    respuesta[person][perfil].numPublicacionesTotal = numDoc;
                }
            }
            foreach (string idperson in respuesta.Keys.ToList())
            {
                respuesta[idperson] = respuesta[idperson].OrderByDescending(x => x.Value.ajuste).ToDictionary(x => x.Key, x => x.Value);
            }
            return respuesta;
        }


        /// <summary>
        /// Método público para cargar los perfiles de cada investigador guardados en los diferentes clusters
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <param name="loadResearchers">Booleano que determina si cargamos los investigadores de cada perfil</param>
        /// <returns>Diccionario con los datos necesarios para cada perfil.</returns>
        public List<Models.Cluster.Cluster> loadSavedProfiles(Guid userId, bool loadResearchers = false)
        {
            // Listado de clusters para rellenar.
            List<Models.Cluster.Cluster> clusterList = new();
            List<string> listIdsClusters = new();

            // Obtener el los profiles a través del id de usuario de la cuenta
            string select = "select ?cluster ?titleCluster ?issued ?description ?profiles ?title group_concat(distinct ?clKnowledgeArea;separator=',') as ?clKnowledgeAreaGroup  group_concat(distinct ?freeTextKeyword;separator=',') as ?freeTextKeywordGroup group_concat(distinct ?KnowledgeArea;separator=',') as ?knowledgeAreaGroup";
            string where = @$"where {{
                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                    ?person <http://w3id.org/roh/gnossUser> ?idGnoss.
                    ?cluster <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                    ?cluster <http://w3id.org/roh/title> ?titleCluster.
                    ?cluster <http://purl.org/dc/terms/issued> ?issued.
                    OPTIONAL
                    {{
                        ?cluster <http://vivoweb.org/ontology/core#description> ?description.
                    }}

                    OPTIONAL
                    {{
                        ?cluster <http://w3id.org/roh/hasKnowledgeArea> ?clHasKnowledgeArea.
                        ?clHasKnowledgeArea <http://w3id.org/roh/categoryNode> ?clKnowledgeArea.
                    }}


                    # Profiles cluster

                    ?cluster <http://w3id.org/roh/clusterPerfil> ?profiles.
                    OPTIONAL
                    {{
                        ?profiles <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                        ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?KnowledgeArea.
                    }}

                    ?profiles <http://w3id.org/roh/title> ?title.
                    OPTIONAL
                    {{
                        ?profiles <http://vivoweb.org/ontology/core#freeTextKeyword> ?freeTextKeyword.
                    }}
                    OPTIONAL
                    {{
                        ?profiles <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                        ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?KnowledgeArea.
                    }}

                    FILTER(?idGnoss = <http://gnoss/{userId.ToString().ToUpper()}>)
                }}";
            SparqlObject sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "person", "cluster"});


            // Rellena el los clusters
            sparqlObject.results.bindings.ForEach(e =>
            {
                // Obtiene un nuevo perfil
                PerfilCluster profile = new PerfilCluster();
                profile.terms = new();
                profile.tags = new();
                profile.users = new();
                // Añade las areas temáticas y los descriptores específicos

                if (e["knowledgeAreaGroup"].value != String.Empty)
                {
                    profile.terms = e["knowledgeAreaGroup"].value.Split(",").ToList();
                }
                if (e["freeTextKeywordGroup"].value != String.Empty)
                {
                    profile.tags = e["freeTextKeywordGroup"].value.Split(",").ToList();
                }
                // Añade el nombre del perfil
                profile.name = e["title"].value;
                profile.entityID = e["profiles"].value;

                // Busca si existe el cluster actualmente
                if (clusterList.Find(cl => cl.entityID == e["cluster"].value) != null)
                {
                    // Añade el perfil al cluster que corresponde previamente cargado
                    clusterList.Find(cl => cl.entityID == e["cluster"].value).profiles.Add(profile);
                }
                else
                {
                    // Añade el ID en el listado de IDs
                    listIdsClusters.Add(e["cluster"].value);

                    // Obtengo las areas de conocimiento del cluster
                    List<string> clusterTerms = new List<string>();
                    if (e["clKnowledgeAreaGroup"].value != String.Empty)
                    {
                        clusterTerms = e["clKnowledgeAreaGroup"].value.Split(",").ToList();
                    }

                    // 1. Crea un nuevo cluter
                    // 2. Añade el perfil al cluster
                    // 3. Añade el cluster creado al listado de clusters
                    try
                    {
                        Models.Cluster.Cluster cluster = new Models.Cluster.Cluster()
                        {
                            name = e.ContainsKey("titleCluster") ? e["titleCluster"].value : String.Empty,
                            profiles = new(),
                            entityID = e.ContainsKey("cluster") ? e["cluster"].value : String.Empty,
                            description = e.ContainsKey("description") ? e["description"].value : String.Empty,
                            fecha = e.ContainsKey("issued") ? e["issued"].value : String.Empty,
                            terms = clusterTerms
                        };
                        cluster.profiles.Add(profile);

                        // Añade el cluster al listado
                        clusterList.Add(cluster);

                    }
                    catch (Exception ex) 
                    {
                        resourceApi.Log.Error("Excepcion: " + ex.Message);
                    }

                }

            });


            // Carga los investigadores si se le indica
            if (loadResearchers)
            {

                var listPerfiles = clusterList.SelectMany(e => e.profiles).ToList();

                var listUserProfiles = loadUsersProfiles(listPerfiles.Select(e => e.entityID).ToList());

                listPerfiles.ForEach(e =>
                {
                    e.users = listUserProfiles.FindAll(tp => tp.Item1 == e.entityID).Select(tp => tp.Item2).ToList();
                });

            }



            return clusterList;
        }


        /// <summary>
        /// Método público que obtiene el objeto para crear la gráfica de áreas temáticas
        /// </summary>
        /// <param name="pPersons">Personas sobre las que realizar el filtrado de áreas temáticas.</param>
        /// <returns>Objeto que se trata en JS para contruir la gráfica.</returns>
        public DataGraficaAreasTags DatosGraficaAreasTematicas(List<string> pPersons)
        {
            string filtroElemento = "";

            filtroElemento = $@"?documento bibo:authorList ?lista. ";
            filtroElemento += $@"?lista rdf:member ?person.";
            filtroElemento += $@"FILTER(?person in (<http://gnoss/{string.Join(">,<http://gnoss/", pPersons.Select(x => x.ToUpper()))}>))";

            Dictionary<string, int> dicResultados = new Dictionary<string, int>();
            int numDocumentos = 0;
            {
                //Nº de documentos por categoría
                SparqlObject resultadoQuery = null;
                string select = $"{mPrefijos} Select ?nombreCategoria count(distinct ?documento) as ?numCategorias";
                string where = $@"  where
                                {{
                                    ?documento a 'document'. 
                                    {filtroElemento}
                                    ?documento roh:hasKnowledgeArea ?area.
                                    ?area roh:categoryNode ?categoria.
                                    ?documento <http://w3id.org/roh/isValidated> 'true'.
                                    ?categoria skos:prefLabel ?nombreCategoria.
                                    MINUS
                                    {{
                                        ?categoria skos:narrower ?hijos
                                    }}
                                }}
                                Group by(?nombreCategoria)";

                resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        string nombreCategoria = UtilidadesAPI.GetValorFilaSparqlObject(fila, "nombreCategoria");
                        int numCategoria = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numCategorias"));
                        dicResultados.Add(nombreCategoria + " (" + numCategoria + ")", numCategoria);
                    }
                }
            }
            {
                //Nº total de documentos
                SparqlObject resultadoQuery = null;
                string select = $"{mPrefijos} Select count(distinct ?documento) as ?numDocumentos";
                string where = $@"  where
                                {{
                                    ?documento a 'document'. 
                                    {filtroElemento}
                                }}";

                resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        numDocumentos = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numDocumentos"));
                    }
                }
            }

            //Ordenar diccionario
            var dicionarioOrdenado = dicResultados.OrderByDescending(x => x.Value);

            Dictionary<string, double> dicResultadosPorcentaje = new Dictionary<string, double>();
            foreach (KeyValuePair<string, int> item in dicionarioOrdenado)
            {
                double porcentaje = Math.Round((double)(100 * item.Value) / numDocumentos, 2);
                dicResultadosPorcentaje.Add(item.Key, porcentaje);
            }

            // Contruir el objeto de la gráfica.
            List<string> listaColores = UtilidadesAPI.CrearListaColores(dicionarioOrdenado.Count(), "#6cafe3");
            Datasets datasets = new Datasets(dicResultadosPorcentaje.Values.ToList(), listaColores);
            Models.Graficas.DataGraficaAreasTags.Data data = new Models.Graficas.DataGraficaAreasTags.Data(dicResultadosPorcentaje.Keys.ToList(), new List<Datasets> { datasets });

            // Máximo.
            x xAxes = new x(new Ticks(0, 100), new ScaleLabel(true, "Percentage"));

            Options options = new Options("y", new Plugins(null, new Legend(false)), new Scales(xAxes));
            DataGraficaAreasTags dataGrafica = new DataGraficaAreasTags("bar", data, options);

            return dataGrafica;
        }


        /// <summary>
        /// Método público que obtiene el objeto para crear la gráfica tipo araña de las relaciones entre los perfiles seleccionados en el cluster
        /// </summary>
        /// <param name="pCluster">Cluster con los datos de las personas sobre las que realizar el filtrado de áreas temáticas.</param>
        /// <param name="pPersons">Personas sobre las que realizar el filtrado de áreas temáticas (Por si se envía directamente).</param>
        /// <param name="seleccionados">Determina si se envía el listado de personas desde el cluster o desde las personas</param>
        /// <returns>Objeto que se trata en JS para contruir la gráfica.</returns>
        public List<DataItemRelacion> DatosGraficaColaboradoresCluster(Models.Cluster.Cluster pCluster, List<string> pPersons, bool seleccionados)
        {
            List<string> colaboradores = pPersons.Select(x => "http://gnoss/" + x.ToUpper()).ToList();
            if (seleccionados)
            {
                colaboradores = new List<string>();
            }
            List<string> listSeleccionados = new List<string>();
            if (pCluster != null && pCluster.profiles != null)
            {
                foreach (PerfilCluster perfilCluster in pCluster.profiles)
                {
                    if (perfilCluster.users != null)
                    {
                        foreach (PerfilCluster.UserCluster userCluster in perfilCluster.users)
                        {
                            colaboradores.Add("http://gnoss/" + userCluster.shortUserID.ToUpper());
                            listSeleccionados.Add("http://gnoss/" + userCluster.shortUserID.ToUpper());
                        }
                    }
                }
            }
            colaboradores = colaboradores.Distinct().ToList();

            //Nodos            
            Dictionary<string, string> dicNodos = new Dictionary<string, string>();
            //Relaciones
            Dictionary<string, List<DataQueryRelaciones>> dicRelaciones = new Dictionary<string, List<DataQueryRelaciones>>();
            //Respuesta
            List<DataItemRelacion> items = new List<DataItemRelacion>();

            string select = "";
            string where = "";
            if (colaboradores.Count > 0)
            {
                #region Cargamos los nodos
                {
                    //Miembros
                    select = $@"{mPrefijos}
                                select distinct ?person ?nombre";
                    where = $@" WHERE
	                            {{
                                    ?person a 'person'.
                                    ?person foaf:name ?nombre.
                                    FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
		            
	                            }}";

                    SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                    {
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            if (!dicNodos.ContainsKey(fila["person"].value))
                            {

                                dicNodos.Add(fila["person"].value, fila["nombre"].value);
                            }
                        }
                    }
                }
                #endregion

                #region Relaciones entre miembros DENTRO DEl CLUSTER
                {
                    //Proyectos
                    {
                        select = "SELECT ?person group_concat(distinct ?project;separator=\",\") as ?projects";
                        where = $@"
                    WHERE {{ 
                            ?project a 'project'.
                            ?project ?propRol ?rol.
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaProy = new Dictionary<string, List<string>>();
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string projects = fila["projects"].value;
                            string person = fila["person"].value;
                            personaProy.Add(person, new List<string>(projects.Split(',')));
                        }
                        UtilidadesAPI.ProcessRelations("Proyectos", personaProy, ref dicRelaciones);
                    }
                    //DOCUMENTOS
                    {
                        select = "SELECT ?person group_concat(?document;separator=\",\") as ?documents";
                        where = $@"
                    WHERE {{ 
                            ?document a 'document'.
                            ?document <http://purl.org/ontology/bibo/authorList> ?authorList.
                            ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaDoc = new Dictionary<string, List<string>>();
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string documents = fila["documents"].value;
                            string person = fila["person"].value;
                            personaDoc.Add(person, new List<string>(documents.Split(',')));
                        }
                        UtilidadesAPI.ProcessRelations("Documentos", personaDoc, ref dicRelaciones);
                    }
                }
                #endregion



                int maximasRelaciones = 0;
                foreach (KeyValuePair<string, List<DataQueryRelaciones>> sujeto in dicRelaciones)
                {
                    foreach (DataQueryRelaciones relaciones in sujeto.Value)
                    {
                        foreach (Datos relaciones2 in relaciones.idRelacionados)
                        {
                            maximasRelaciones = Math.Max(maximasRelaciones, relaciones2.numVeces);
                        }
                    }
                }

                // Nodos. 
                if (dicNodos != null && dicNodos.Count > 0)
                {
                    foreach (KeyValuePair<string, string> nodo in dicNodos)
                    {
                        string clave = nodo.Key;
                        Models.Graficas.DataItemRelacion.Data.Type type = Models.Graficas.DataItemRelacion.Data.Type.none;
                        type = Models.Graficas.DataItemRelacion.Data.Type.icon_member;
                        if (listSeleccionados.Contains(nodo.Key))
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_ip;
                        }
                        Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(clave, nodo.Value, null, null, null, "nodes", type);
                        DataItemRelacion dataColabo = new DataItemRelacion(data, true, true);
                        items.Add(dataColabo);
                    }
                }

                // Relaciones.
                if (dicRelaciones != null && dicRelaciones.Count > 0)
                {
                    foreach (KeyValuePair<string, List<DataQueryRelaciones>> sujeto in dicRelaciones)
                    {
                        foreach (DataQueryRelaciones relaciones in sujeto.Value)
                        {
                            foreach (Datos relaciones2 in relaciones.idRelacionados)
                            {
                                string id = $@"{sujeto.Key}~{relaciones.nombreRelacion}~{relaciones2.idRelacionado}~{relaciones2.numVeces}";
                                Models.Graficas.DataItemRelacion.Data.Type type = Models.Graficas.DataItemRelacion.Data.Type.none;
                                if (relaciones.nombreRelacion == "Proyectos")
                                {
                                    type = Models.Graficas.DataItemRelacion.Data.Type.relation_project;
                                }
                                else if (relaciones.nombreRelacion == "Documentos")
                                {
                                    type = Models.Graficas.DataItemRelacion.Data.Type.relation_document;
                                }
                                Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(id, relaciones.nombreRelacion, sujeto.Key, relaciones2.idRelacionado, UtilidadesAPI.CalcularGrosor(maximasRelaciones, relaciones2.numVeces), "edges", type);
                                DataItemRelacion dataColabo = new DataItemRelacion(data, null, null);
                                items.Add(dataColabo);
                            }
                        }
                    }
                }
            }
            return items;



        }

        /// <summary>
        /// Método público buscar diferentes tags
        /// </summary>
        /// <param name="pSearch">parámetro que corresponde a la cadena de búsqueda.</param>
        /// <returns>Listado de etiquetas de resultado.</returns>
        public List<string> memberListFromCluser(string pClusterId)
        {
            string select = "SELECT DISTINCT ?user";
            string where = $@"WHERE{{
                {{
                    <{pClusterId}> <http://w3id.org/roh/clusterPerfil> ?s.
                    ?s <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?a.
                    ?a <http://w3id.org/roh/gnossUser> ?user
                }}
                UNION
                {{
                    <{pClusterId}>  <http://www.w3.org/1999/02/22-rdf-syntax-ns#member>?a.
                    ?a <http://w3id.org/roh/gnossUser> ?user

                }}
            }}
            ";
            SparqlObject sparqlObjectAux = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>{"cluster","person"});
            List<string> resultados = sparqlObjectAux.results.bindings.Select(x => x["user"].value).Distinct().ToList();
            return resultados;
        }
        public string getOwnerFromCluser(string pClusterId)
        {

            string select = "SELECT DISTINCT ?user ";
            string where = $@"WHERE{{
                {{
                    <{pClusterId}>  <http://www.w3.org/1999/02/22-rdf-syntax-ns#member>?a.
                    ?a <http://w3id.org/roh/gnossUser> ?user

                }}
            }}
            ";
            SparqlObject sparqlObjectAux = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "cluster" ,"person"});
            List<string> resultados = sparqlObjectAux.results.bindings.Select(x => x["user"].value).Distinct().ToList();
            string user = resultados.ToList().FirstOrDefault();
            return user;
        }
        public List<string> SearchTags(string pSearch)
        {
            int numMax = 20;
            string searchText = pSearch.Trim();
            string filter = "";
            if (!pSearch.EndsWith(' '))
            {
                string[] splitSearch = searchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (splitSearch.Length > 1)
                {
                    searchText = searchText.Substring(0, searchText.LastIndexOf(' '));
                    if (splitSearch.Last().Length > 3)
                    {
                        searchText += " " + splitSearch.Last() + "*";
                    }
                    else
                    {
                        filter = $" AND lcase(?o) like \"% { splitSearch.Last() }%\" ";
                    }
                }
                else if (searchText.Length > 3)
                {
                    searchText += "*";
                }
                else // Si tiene menos de 4 caracteres y no termina en espacio, buscamos por like
                {
                    filter = $"  lcase(?o) like \"{ searchText }%\" OR lcase(?o) like \"% { searchText }%\" ";
                    searchText = "";
                }
            }
            if (searchText != "")
            {
                filter = $"bif:contains(?o, \"'{ searchText }'\"){filter}";
            }
            string select = "SELECT DISTINCT ?s ?o ";
            string where = $"WHERE {{ ?s a <http://purl.org/ontology/bibo/Document>. ?s <http://vivoweb.org/ontology/core#freeTextKeyword> ?freeTextKeyword. ?freeTextKeyword <http://w3id.org/roh/title> ?o. FILTER( {filter} )    }} ORDER BY ?o";
            SparqlObject sparqlObjectAux = resourceApi.VirtuosoQuery(select, where, "document");
            List<string> resultados = sparqlObjectAux.results.bindings.Select(x => x["o"].value).Distinct().ToList();
            if (resultados.Count() > numMax)
            {
                resultados = resultados.ToList().GetRange(0, numMax);
            }
            return resultados;
        }


        #region Métodos de recolección de datos

        /// <summary>
        /// Método privado para obtener los tesauros.
        /// </summary>
        /// <param name="pListaTesauros">Listado de thesaurus a obtener.</param>
        /// <returns>Diccionario con las listas de thesaurus.</returns>
        private Dictionary<string, List<ThesaurusItem>> GetTesauros(List<string> pListaTesauros)
        {
            Dictionary<string, List<ThesaurusItem>> elementosTesauros = new Dictionary<string, List<ThesaurusItem>>();

            foreach (string tesauro in pListaTesauros)
            {
                string select = "select * ";
                string where = @$"where {{
                    ?s a <http://www.w3.org/2008/05/skos#Concept>.
                    ?s <http://www.w3.org/2008/05/skos#prefLabel> ?nombre.
                    ?s <http://purl.org/dc/elements/1.1/source> '{tesauro}'
                    OPTIONAL {{ ?s <http://www.w3.org/2008/05/skos#broader> ?padre }}
                }} ORDER BY ?padre ?s ";
                SparqlObject sparqlObject = resourceApi.VirtuosoQuery(select, where, "taxonomy");

                List<ThesaurusItem> items = sparqlObject.results.bindings.Select(x => new ThesaurusItem()
                {
                    id = x["s"].value,
                    name = x["nombre"].value,
                    parentId = x.ContainsKey("padre") ? x["padre"].value : ""
                }).ToList();

                elementosTesauros.Add(tesauro, items);
            }

            return elementosTesauros;
        }


        /// <summary>
        /// Método privado para obtener las taxonomías de un 'CategoryPath'.
        /// </summary>
        /// <param name="terms">Listado de la categoría a obtener.</param>
        /// <returns>listado de las categorías.</returns>
        private List<string> LoadCurrentTerms(List<string> terms)
        {

            string termsTxt = String.Join(',', terms.Select(e => "<" + e + ">"));

            string select = "select ?o";
            string where = @$"where {{
                ?s a <http://w3id.org/roh/CategoryPath>.
                ?s <http://w3id.org/roh/categoryNode> ?o.
                FILTER(?s IN ({termsTxt}))
            }}";
            SparqlObject sparqlObject = resourceApi.VirtuosoQuery(select, where, "cluster");

            List<string> termsRes = new();

            sparqlObject.results.bindings.ForEach(e =>
            {
                termsRes.Add(e["o"].value);
            });




            return termsRes;
        }


        /// <summary>
        /// Obtiene la categoría padre.
        /// </summary>
        /// <param name="pIdTesauro">Categoría a consultar.</param>
        /// <returns>Categoría padre.</returns>
        private string ObtenerIdTesauro(string pIdTesauro)
        {
            string idTesauro = pIdTesauro.Split(new[] { "researcharea_" }, StringSplitOptions.None)[1];
            int num1 = Int32.Parse(idTesauro.Split('.')[0]);
            int num2 = Int32.Parse(idTesauro.Split('.')[1]);
            int num3 = Int32.Parse(idTesauro.Split('.')[2]);
            int num4 = Int32.Parse(idTesauro.Split('.')[3]);

            if (num4 != 0)
            {
                idTesauro = $@"{resourceApi.GraphsUrl}items/researcharea_{num1}.{num2}.{num3}.0";
            }
            else if (num3 != 0 && num4 == 0)
            {
                idTesauro = $@"{resourceApi.GraphsUrl}items/researcharea_{num1}.{num2}.0.0";
            }
            else if (num2 != 0 && num3 == 0 && num4 == 0)
            {
                idTesauro = $@"{resourceApi.GraphsUrl}items/researcharea_{num1}.0.0.0";
            }

            return idTesauro;
        }

        /// <summary>
        /// Obtiene las categorías del tesáuro.
        /// </summary>
        /// <returns>Tupla con (clave) diccionario de las idCategorias-idPadre y (valor) diccionario de nombreCategoria-idCategoria.</returns>
        private static Tuple<Dictionary<string, string>, Dictionary<string, string>> ObtenerDatosTesauro()
        {
            Dictionary<string, string> dicAreasBroader = new Dictionary<string, string>();
            Dictionary<string, string> dicAreasNombre = new Dictionary<string, string>();

            string select = @"SELECT DISTINCT * ";
            string where = @$"WHERE {{
                ?concept a <http://www.w3.org/2008/05/skos#Concept>.
                ?concept <http://www.w3.org/2008/05/skos#prefLabel> ?nombre.
                ?concept <http://purl.org/dc/elements/1.1/source> 'researcharea'
                OPTIONAL{{?concept <http://www.w3.org/2008/05/skos#broader> ?broader}}
                }}";
            SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, "taxonomy");

            foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings)
            {
                string concept = fila["concept"].value;
                string nombre = fila["nombre"].value;
                string broader = "";
                if (fila.ContainsKey("broader"))
                {
                    broader = fila["broader"].value;
                }
                dicAreasBroader.Add(concept, broader);
                if (!dicAreasNombre.ContainsKey(nombre.ToLower()))
                {
                    dicAreasNombre.Add(nombre.ToLower(), concept);
                }
            }

            Dictionary<string, string> dicAreasUltimoNivel = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in dicAreasNombre)
            {
                bool tieneHijos = false;
                string id = item.Value.Split(new[] { "researcharea_" }, StringSplitOptions.None)[1];
                int num1 = Int32.Parse(id.Split('.')[0]);
                int num2 = Int32.Parse(id.Split('.')[1]);
                int num3 = Int32.Parse(id.Split('.')[2]);
                int num4 = Int32.Parse(id.Split('.')[3]);

                if (num2 == 0 && num3 == 0 && num4 == 0)
                {
                    tieneHijos = dicAreasNombre.ContainsValue($@"{resourceApi.GraphsUrl}items/researcharea_{num1}.1.0.0");
                }
                else if (num3 == 0 && num4 == 0)
                {
                    tieneHijos = dicAreasNombre.ContainsValue($@"{resourceApi.GraphsUrl}items/researcharea_{num1}.{num2}.1.0");
                }
                else if (num4 == 0)
                {
                    tieneHijos = dicAreasNombre.ContainsValue($@"{resourceApi.GraphsUrl}items/researcharea_{num1}.{num2}.{num3}.1");
                }

                if (!tieneHijos)
                {
                    dicAreasUltimoNivel.Add(item.Key, item.Value);
                }
            }

            return new Tuple<Dictionary<string, string>, Dictionary<string, string>>(dicAreasBroader, dicAreasUltimoNivel);
        }


        /// <summary>
        /// Obtiene los datos de los investigadores de los perfiles indicados.
        /// </summary>
        /// <returns>Listado de los ids de los perfiles sobre los que se van a cargar los investigadores.</returns>
        private List<Tuple<string, PerfilCluster.UserCluster>> loadUsersProfiles(List<string> listProfilesIds)
        {
            // Creamos la variable para cargar los investigadores y el id del perfil al que pertenece 
            List<Tuple<string, PerfilCluster.UserCluster>> result = new();

            //Datos de los miembros
            string select = "select distinct ?s ?memberPerfil ?nombreUser ?hasPosition ?tituloOrg ?departamento (count(distinct ?doc)) as ?numDoc (count(distinct ?proj)) as ?ipNumber ";
            string where = @$"where {{
                    ?s <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?memberPerfil.
                    ?memberPerfil <http://xmlns.com/foaf/0.1/name> ?nombreUser.
                    OPTIONAL {{
                        ?doc a <http://purl.org/ontology/bibo/Document>.
                        ?doc <http://w3id.org/roh/isValidated> 'true'.
                        ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
                        ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?memberPerfil.
                    }}
                    OPTIONAL {{
                        ?proj a <http://vivoweb.org/ontology/core#Project>.
                        ?proj <http://w3id.org/roh/isValidated> 'true'.
                        ?proj <http://vivoweb.org/ontology/core#relates> ?listprojauth.
                        ?listprojauth <http://w3id.org/roh/roleOf> ?memberPerfil.
                        ?listprojauth <http://w3id.org/roh/isIP> 'true'.
                    }}
                    OPTIONAL {{
                        ?memberPerfil <http://w3id.org/roh/hasPosition> ?hasPosition.
                    }}
                    OPTIONAL {{
                        ?memberPerfil <http://vivoweb.org/ontology/core#departmentOrSchool> ?dept.
                        ?dept <http://purl.org/dc/elements/1.1/title> ?departamento
                    }}
                    OPTIONAL {{
                        ?memberPerfil <http://w3id.org/roh/hasRole> ?org.
                        ?org <http://w3id.org/roh/title> ?tituloOrg
                    }}
                    FILTER(?s in ({string.Join(',', listProfilesIds.Select(e => '<' + e + '>'))}))
                }}";
            SparqlObject sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "cluster", "person","document","project","organization", "department" });


            // Carga los datos en el objeto
            sparqlObject.results.bindings.ForEach(e =>
            {
                List<string> infoList = new List<string>();
                if (e.ContainsKey("hasPosition"))
                {
                    infoList.Add(e["hasPosition"].value);
                }
                if (e.ContainsKey("tituloOrg"))
                {
                    infoList.Add(e["tituloOrg"].value);
                }
                if (e.ContainsKey("departamento"))
                {
                    infoList.Add(e["departamento"].value);
                }
                string info = string.Join(", ", infoList);

                // Crea la tupla con el ID del perfil y el usuario correspondiente
                result.Add(new Tuple<string, PerfilCluster.UserCluster>(e["s"].value, new PerfilCluster.UserCluster()
                {
                    userID = e["memberPerfil"].value,
                    name = e["nombreUser"].value,
                    shortUserID = resourceApi.GetShortGuid(e["memberPerfil"].value).ToString().ToLower(),
                    numPublicacionesTotal = e.ContainsKey("numDoc") ? int.Parse(e["numDoc"].value) : 0,
                    ipNumber = e.ContainsKey("ipNumber") ? int.Parse(e["ipNumber"].value) : 0,
                    info = info
                }));
            });

            return result;
        }


        #endregion


    }
}
