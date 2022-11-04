using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models.RedesUsuario;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using User = Hercules.CommonsEDMA.ServicioExterno.Models.RedesUsuario.User;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesRedesUsuario : GnossGetMainResourceApiDataBase
    {

        /// <summary>
        /// Obtiene los datos de las fuentes de RO de una persona.
        /// </summary>
        /// <param name="pIdGnossUser">ID del usuario gnoss.</param>
        /// <returns>Diccionario con los datos resultantes.</returns>
        public List<DataUser> GetDataRedesUsuario(string pIdGnossUser)
        {
            List<DataUser> listaData = new();
            listaData.Add(new DataUser() { nombre = "Identificador de FigShare", id = "usuarioFigShare", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "Token de FigShare", id = "tokenFigShare", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "Usuario de GitHub", id = "usuarioGitHub", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "Token de GitHub", id = "tokenGitHub", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "ORCID", id = "orcid", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "Wos ID", id = "researcherId", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "Scopus ID", id = "scopusId", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "Semantic Scholar ID", id = "semanticScholarId", valor = string.Empty });
            listaData.Add(new DataUser() { nombre = "Matching", id = "useMatching", valor = string.Empty });

            string idGnossUser = $@"http://gnoss/{pIdGnossUser.ToUpper()}";
            SparqlObject resultadoQuery = null;
            StringBuilder select = new(), where = new();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT DISTINCT ?s ?usuarioFigShare ?tokenFigShare ?usuarioGitHub ?tokenGitHub ?orcid ?orcidCV ?researcherId ?researcherIdCV ?scopusId ?scopusIdCV ?semanticScholarId ?useMatching");
            where.Append("WHERE { ");
            where.Append($@"?s roh:gnossUser <{idGnossUser}>. ");
            where.Append($@"?c roh:cvOf ?s. ");
            where.Append($@"?c roh:personalData ?personalData.");
            where.Append($@"OPTIONAL{{?s roh:usuarioFigShare ?usuarioFigShare. }} ");
            where.Append($@"OPTIONAL{{?s roh:tokenFigShare ?tokenFigShare. }} ");
            where.Append($@"OPTIONAL{{?s roh:usuarioGitHub ?usuarioGitHub. }} ");
            where.Append($@"OPTIONAL{{?s roh:tokenGitHub ?tokenGitHub. }} ");
            where.Append($@"OPTIONAL{{?s roh:ORCID ?orcid. }} ");
            where.Append($@"OPTIONAL{{?s vivo:researcherId ?researcherId. }} ");
            where.Append($@"OPTIONAL{{?s vivo:scopusId ?scopusId. }} ");
            where.Append($@"OPTIONAL{{?personalData vivo:researcherId ?researcherIdCV. }} ");
            where.Append($@"OPTIONAL{{?personalData roh:ORCID ?orcidCV. }} ");
            where.Append($@"OPTIONAL{{?personalData vivo:scopusId ?scopusIdCV. }} ");
            where.Append($@"OPTIONAL{{?s roh:semanticScholarId ?semanticScholarId. }} ");
            where.Append($@"OPTIONAL{{?s roh:useMatching ?useMatching. }} ");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQueryMultipleGraph(select.ToString(), where.ToString(), new List<string> { "person" , "curriculumvitae" });


            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    // Usuario FigShare
                    listaData = getDataSpqrl(fila, listaData, "usuarioFigShare");

                    // Token FigShare
                    listaData = getDataSpqrl(fila, listaData, "tokenFigShare");

                    // Usuario GitHub
                    listaData = getDataSpqrl(fila, listaData, "usuarioGitHub");

                    // Token GitHub
                    listaData = getDataSpqrl(fila, listaData, "tokenGitHub");

                    // ORCID
                    if (fila.ContainsKey("orcid"))
                    {
                        listaData = getDataSpqrl(fila, listaData, "orcid");
                    }
                    else if (fila.ContainsKey("orcidCV"))
                    {
                        foreach (DataUser userData in listaData)
                        {
                            if (userData.id == "orcid")
                            {
                                userData.valor = fila["orcidCV"].value;
                                break;
                            }
                        }
                    }
                    
                    // Researcher ID
                    if (fila.ContainsKey("researcherId"))
                    {
                        listaData = getDataSpqrl(fila, listaData, "researcherId");
                    }
                    else if (fila.ContainsKey("researcherIdCV"))
                    {
                        foreach (DataUser userData in listaData)
                        {
                            if (userData.id == "researcherId")
                            {
                                userData.valor = fila["researcherIdCV"].value;
                                break;
                            }
                        }
                    }
                    
                    // Scopus ID
                    if (fila.ContainsKey("scopusId"))
                    {
                        listaData = getDataSpqrl(fila, listaData, "scopusId");
                    }
                    else if (fila.ContainsKey("scopusIdCV"))
                    {
                        foreach (DataUser userData in listaData)
                        {
                            if (userData.id == "scopusId")
                            {
                                userData.valor = fila["scopusIdCV"].value;
                                break;
                            }
                        }
                    }

                    // Semantic Scholar ID
                    listaData = getDataSpqrl(fila, listaData, "semanticScholarId");

                    // Matching
                    listaData = getDataSpqrl(fila, listaData, "useMatching");
                }
            }

            return listaData;
        }


        /// <summary>
        /// Modifica los datos de la fuente de RO de una persona.
        /// </summary>
        /// <param name="pIdGnossUser">ID del usuario de gnoss.</param>
        /// <param name="pDataUser">Objeto con los datos nuevos.</param>
        public void SetDataRedesUsuario(string pIdGnossUser, User pDataUser)
        {
            // Obtención de datos antiguos.
            List<DataUser> datosAntiguos = GetDataRedesUsuario(pIdGnossUser);

            string idRecurso = string.Empty;
            string idGnossUser = $@"http://gnoss/{pIdGnossUser.ToUpper()}";
            SparqlObject resultadoQuery = null;
            StringBuilder select = new(), where = new();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT DISTINCT ?s ");
            where.Append("WHERE { ");
            where.Append($@"?s roh:gnossUser <{idGnossUser}>. ");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), "person");

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    if (fila.ContainsKey("s"))
                    {
                        idRecurso = fila["s"].value;
                    }
                }
            }

            // Inserción/Modificación de triples.
            resourceApi.ChangeOntoly("person");
            Guid guid = resourceApi.GetShortGuid(idRecurso);
            Dictionary<Guid, List<TriplesToInclude>> dicInsercion = new();
            List<TriplesToInclude> listaTriplesInsercion = new();
            Dictionary<Guid, List<TriplesToModify>> dicModificacion = new();
            List<TriplesToModify> listaTriplesModificacion = new();
            Dictionary<Guid, List<RemoveTriples>> dicBorrado = new();
            List<RemoveTriples> listaTriplesBorrado = new();

            foreach (DataUser item in datosAntiguos)
            {
                string propiedad = item.id;
                string dataViejo = item.valor;
                var dataNuevoTmp = pDataUser.dataUser.FirstOrDefault(x => x.nombre == item.nombre);
                string dataNuevo = String.Empty;
                if (dataNuevoTmp != null)
                {
                    dataNuevo = dataNuevoTmp.valor;
                }
                if (dataNuevo == null)
                {
                    dataNuevo = string.Empty;
                }
                if (dataViejo == null)
                {
                    dataViejo = string.Empty;
                }

                if (dataViejo != dataNuevo)
                {
                    if (string.IsNullOrEmpty(dataViejo))
                    {
                        // Inserción (Triples).                 
                        TriplesToInclude triple = new();
                        triple.Predicate = $@"http://w3id.org/roh/{propiedad}";
                        triple.NewValue = dataNuevo;
                        listaTriplesInsercion.Add(triple);
                    }
                    else if (string.IsNullOrEmpty(dataNuevo))
                    {
                        // Borrado (Triple).
                        RemoveTriples triple = new();
                        triple.Predicate = $@"http://w3id.org/roh/{propiedad}";
                        triple.Value = dataViejo;
                        triple.Title = false;
                        triple.Description = false;
                        listaTriplesBorrado.Add(triple);
                    }
                    else
                    {
                        // Modificación (Triples).
                        TriplesToModify triple = new();
                        triple.Predicate = $@"http://w3id.org/roh/{propiedad}";
                        triple.NewValue = dataNuevo;
                        triple.OldValue = dataViejo;
                        listaTriplesModificacion.Add(triple);
                    }
                }
            }

            // Inserción.
            dicInsercion.Add(guid, listaTriplesInsercion);
            resourceApi.InsertPropertiesLoadedResources(dicInsercion);

            // Borrado.
            dicBorrado.Add(guid, listaTriplesBorrado);
            resourceApi.DeletePropertiesLoadedResources(dicBorrado);

            // Modificación.
            dicModificacion.Add(guid, listaTriplesModificacion);
            resourceApi.ModifyPropertiesLoadedResources(dicModificacion);
        }


        /// <summary>
        /// Obtiene los datos de las fuentes de RO de una persona.
        /// </summary>
        /// <param name="fila">fila de resultados de la búsqueda.</param>
        /// <param name="listaData">Listado de los usuario sobre el que buscar.</param>
        /// <param name="fieldName">Nombre del campo a buscar y establecer.</param>
        /// <returns>Diccionario con los datos resultantes.</returns>
        private List<DataUser> getDataSpqrl(Dictionary<string, SparqlObject.Data> fila, List<DataUser> listaData, string fieldName)
        {
            if (fila.ContainsKey(fieldName))
            {
                foreach (DataUser userData in listaData)
                {
                    if (userData.id == fieldName)
                    {
                        userData.valor = fila[fieldName].value;
                        break;
                    }
                }
            }
            return listaData;
        }
    }
}
