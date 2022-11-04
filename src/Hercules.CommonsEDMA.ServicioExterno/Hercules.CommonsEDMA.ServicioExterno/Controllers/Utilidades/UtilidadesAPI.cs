using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gnoss.ApiWrapper;
using System.Text;
using System.Web;
using Hercules.CommonsEDMA.ServicioExterno.Models;
using Hercules.CommonsEDMA.ServicioExterno.Models.Cluster;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades
{
    public class UtilidadesAPI
    {
        public static string GetValorFilaSparqlObject(Dictionary<string, SparqlObject.Data> pFila, string pParametro)
        {
            if (pFila.ContainsKey(pParametro) && !string.IsNullOrEmpty(pFila[pParametro].value))
            {
                return pFila[pParametro].value;
            }

            return null;
        }

        public static string GetValorFilaSparqlDia(Dictionary<string, SparqlObject.Data> pFila, string pParametro)
        {
            if (pFila.ContainsKey(pParametro) && !string.IsNullOrEmpty(pFila[pParametro].value))
            {
                return pFila[pParametro].value.Substring(0, 8) + "000000";
            }

            return null;
        }

        public static bool ModificarTriplesRecurso(ResourceApi pResourceApi, Guid pRecursoID, List<TriplesToModify> pTriples)
        {
            List<TriplesToModify> triplesInsertar = new();

            foreach (TriplesToModify triple in pTriples)
            {
                if (triple.NewValue == "")
                {
                    triple.NewValue = null;
                }

                if (triple.OldValue == "")
                {
                    triple.OldValue = null;
                }

                if (triple.OldValue != triple.NewValue)
                {
                    triplesInsertar.Add(triple);
                }
            }

            Dictionary<Guid, List<TriplesToModify>> dicTriplesModificar = new();
            dicTriplesModificar.Add(pRecursoID, triplesInsertar);
            Dictionary<Guid, bool> dicInsertado = pResourceApi.ModifyPropertiesLoadedResources(dicTriplesModificar);

            return (dicInsertado != null && dicInsertado.ContainsKey(pRecursoID) && dicInsertado[pRecursoID]);
        }

        private static string ObtenerQuerySearch(string pQuery, string pParam)
        {
            string[] filtroEspacios = pParam.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            while (pQuery.Contains("|||[PARAMETROESPACIOULTIMODIFERENTE]||"))
            {
                int inicio = pQuery.IndexOf("|||[PARAMETROESPACIOULTIMODIFERENTE]||");
                int fin = pQuery.IndexOf("|||", inicio + 1) + 3;
                string[] querysAuxArray = pQuery.Substring(inicio + 3, fin - inicio - 6).Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

                string queryAuxModificada = querysAuxArray[1];
                string queryAuxModificadaFin = querysAuxArray[2];

                string queryAuxFin = "";
                int i = 0;
                foreach (string palabra in filtroEspacios)
                {
                    i++;
                    if (i == filtroEspacios.Length)
                    {
                        queryAuxFin += " " + queryAuxModificadaFin.Replace("[PARAMETROESPACIOIN]", palabra) + " ";
                    }
                    else
                    {
                        queryAuxFin += " " + queryAuxModificada.Replace("[PARAMETROESPACIOIN]", palabra) + " ";
                    }

                }
                pQuery = pQuery.Substring(0, inicio) + queryAuxFin + pQuery.Substring(fin);
            }

            return pQuery;
        }


        /// <summary>
        /// Crea los filtros en formato sparql.
        /// </summary>
        /// <param name="pDicFiltros">Diccionario con los filtros.</param>
        /// <param name="pVarAnterior">Variable anterior para no repetir nombre.</param>
        /// <param name="pAux">Variable auxiliar para que no se repitan los nombres.</param>
        /// <returns>String con los filtros creados.</returns>
        public static string CrearFiltros(Dictionary<string, List<string>> pDicFiltros, string pVarAnterior, ref int pAux)
        {
            // Edito los espacios y acentos del diccionario
            foreach (KeyValuePair<string, List<string>> keyValue in pDicFiltros)
            {
                for (int i = 0; i < keyValue.Value.Count; i++)
                {
                    keyValue.Value[i] = HttpUtility.UrlDecode(keyValue.Value[i]);

                    //Si el diccionario trae un valor vacío o un espacio elimino ese filtro
                    if (string.IsNullOrEmpty(keyValue.Value[i]))
                    {
                        pDicFiltros.Remove(keyValue.Key);
                    }
                }
            }

            // Filtros de fechas.
            List<string> filtrosFecha = new();
            filtrosFecha.Add("dct:issued");
            filtrosFecha.Add("vivo:start");
            filtrosFecha.Add("vivo:end");

            // Filtros de enteros.
            List<string> filtrosEnteros = new();
            filtrosEnteros.Add("roh:publicationsNumber");
            filtrosEnteros.Add("roh:projectsNumber");
            filtrosEnteros.Add("roh:publicationsNumber");
            filtrosEnteros.Add("roh:citationCount");
            filtrosEnteros.Add("roh:quartile");

            // Filtros de inversas.
            Dictionary<string, int> filtrosReciprocos = new();
            filtrosReciprocos.Add("foaf:member@@@roh:roleOf@@@roh:title", 2);
            filtrosReciprocos.Add("roh:membersGroup@@@roh:title", 1);

            //Filtros personalizados
            Dictionary<string, string> filtrosPersonalizados = new();
            filtrosPersonalizados.Add("searchColaboradoresPorGrupo",
                @$"
                    {{
                        SELECT DISTINCT {pVarAnterior}
	                    WHERE 
	                    {{	
                            {pVarAnterior} a 'person'	
		                    {{
			                    {{
				                    #Documentos
				                    SELECT *
				                    WHERE {{
					                    ?documento <http://w3id.org/roh/isProducedBy> <http://gnoss/[PARAMETRO]>.
					                    ?documento a 'document'.
					                    ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
				                    }}
			                    }} 
			                    UNION 
			                    {{
				                    #Proyectos
				                    SELECT *
				                    WHERE {{
					                    ?proy <http://w3id.org/roh/publicGroupList> <http://gnoss/[PARAMETRO]>.
					                    ?proy a 'project'.
					                    ?proy ?propRol ?role.
					                    FILTER(?propRol in (<http://vivoweb.org/ontology/core#relates>,<http://w3id.org/roh/mainResearchers>))
					                    ?role <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
				                    }}
			                    }}
		                    }}		
		                    MINUS
		                    {{
			                    {pVarAnterior} <http://vivoweb.org/ontology/core#relates> <http://gnoss/[PARAMETRO]>
		                    }}
	                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searchPersonasRelacionadasConProyecto",
                @$"
                    {{
                        SELECT DISTINCT {pVarAnterior}
	                    WHERE 
	                    {{	
                            <http://gnoss/[PARAMETRO]> <http://w3id.org/roh/publicAuthorList> {pVarAnterior}.
	                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searchColaboradoresPorPersona",
                @$"
                    {{
	                    SELECT DISTINCT {pVarAnterior}
	                    WHERE 
	                    {{	
                            {pVarAnterior} a 'person'	
		                    {{
			                    {{
				                    #Documentos
				                    SELECT *
				                    WHERE {{
                                        ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutoresA.
					                    ?listaAutoresA <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> <http://gnoss/[PARAMETRO]>.					                           
					                    ?documento a 'document'.
					                    ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
				                    }}
			                    }}
			                    UNION 
			                    {{
				                    #Proyectos
				                    SELECT *
				                    WHERE {{
					                    ?proy ?propRolA ?roleA.
					                    FILTER(?propRolA in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
					                    ?roleA <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> <http://gnoss/[PARAMETRO]>.
					                    ?proy a 'project'.
					                    ?proy ?propRolB ?roleB.
					                    FILTER(?propRolB in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
					                    ?roleB <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
				                    }}
			                    }}
		                    }}		
		                    FILTER({pVarAnterior}!=<http://gnoss/[PARAMETRO]>)
	                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searchPersonasColaboradoresConProyecto",
                @$"
                    {{
                        SELECT DISTINCT {pVarAnterior}
                        WHERE
                        {{
                            ?doc a 'document'.
                            ?doc <http://w3id.org/roh/isValidated> 'true'.
                            ?doc <http://w3id.org/roh/project> <http://gnoss/[PARAMETRO]>.
                            ?doc <http://purl.org/ontology/bibo/authorList> ?relacion.
                            ?relacion <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
                            MINUS
		                    {{
                                <http://gnoss/[PARAMETRO]> <http://w3id.org/roh/membersProject> {pVarAnterior}  
		                    }}
                        }}
                    }}
                ");

            filtrosPersonalizados.Add("searchPublicacionesPublicasPerson",
                $@"
                    {{
                        SELECT {pVarAnterior}

                        WHERE
	                    {{
		                    {{
                                FILTER(?person=<[PARAMETRO]>)
			                    {pVarAnterior} a 'document'.
			                    ?cv <http://w3id.org/roh/cvOf> ?person.
			                    ?cv  <http://w3id.org/roh/scientificActivity> ?scientificActivity.
			                    ?scientificActivity ?pAux ?oAux.
			                    ?oAux <http://w3id.org/roh/isPublic> 'true'.
			                    ?oAux <http://vivoweb.org/ontology/core#relatedBy> {pVarAnterior}
		                    }}
                            #UNION
		                    #{{
                            #    FILTER(?person=<[PARAMETRO]>)
			                #    {pVarAnterior} a 'document'.
			                #    {pVarAnterior} <http://w3id.org/roh/isValidated> 'true'.
			                #    {pVarAnterior} <http://purl.org/ontology/bibo/authorList> ?list.
			                #    ?list <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
		                    #}}
	                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searchProyectosPublicosPerson",
                $@"
                    {{
	                    SELECT {pVarAnterior}
	                    WHERE
	                    {{		                    
		                    {{
                                FILTER(?person=<[PARAMETRO]>)
			                    {pVarAnterior} a 'project'.
			                    ?cv <http://w3id.org/roh/cvOf> ?person.
			                    ?cv  <http://w3id.org/roh/scientificExperience> ?scientificExperience.
			                    ?scientificExperience ?pAux ?oAux.
			                    ?oAux <http://w3id.org/roh/isPublic> 'true'.
			                    ?oAux <http://vivoweb.org/ontology/core#relatedBy> {pVarAnterior}
		                    }}
                            #UNION
		                    #{{
                            #    FILTER(?person=<[PARAMETRO]>)
			                #    {pVarAnterior} a 'project'.
			                #    {pVarAnterior} <http://w3id.org/roh/isValidated> 'true'.
			                #    {pVarAnterior} ?propRol ?rol.
                            #    FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            #    ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
		                    #}}
	                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searchColaboradoresPersonPublic",
                $@"
                    {{
                        SELECT DISTINCT {pVarAnterior}
	                    WHERE 
	                    {{	
                                    {pVarAnterior} a 'person'	
		                    {{
			                    {{
				                    #Documentos
				                    SELECT *
				                    WHERE {{
					                    ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutoresA.
					                    ?listaAutoresA <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> <[PARAMETRO]>.
					                    ?documento a 'document'.
					                    ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
				                    }}
			                    }}
			                    UNION 
			                    {{
				                    #Proyectos
				                    SELECT *
				                    WHERE {{
					                    ?proy <http://w3id.org/roh/membersProject> <[PARAMETRO]>.
					                    ?proy a 'project'.
					                    ?proy <http://w3id.org/roh/membersProject> {pVarAnterior}.
				                    }}
			                    }}
		                    }}	
		                    FILTER({pVarAnterior} != <[PARAMETRO]>)
	                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searchPersonasRelacionadasConProyectoPublic",
                $@"
                    {{
                    SELECT DISTINCT {pVarAnterior}
                    WHERE
                    {{
                    <[PARAMETRO]> ?propRol ?role.
                    FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                    ?role <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searchProyectosPorGrupo",
                $@"
                    {{
                       SELECT DISTINCT {pVarAnterior}
                       WHERE {{
                           {pVarAnterior} a 'project'.
                           {pVarAnterior} <http://w3id.org/roh/isProducedBy> <[PARAMETRO]>
                       }}
                    }}
                ");

            filtrosPersonalizados.Add("searchMiembrosGrupo",
                $@"
                    {{
                        SELECT {pVarAnterior}
                        WHERE {{
                        <[PARAMETRO]> ?propRol ?rol.
                        FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                        ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
                        }} 
                    }}
                ");

            filtrosPersonalizados.Add("searchColaboradoresGruposExternos",
                $@"
                    {{
                        SELECT DISTINCT {pVarAnterior}
                        WHERE
                            {{
                            {pVarAnterior} a 'person'
                        {{
                            {{
                            #Documentos
                            SELECT {pVarAnterior}
                            WHERE {{
                            ?documento <http://w3id.org/roh/isProducedBy> <[PARAMETRO]>.
                            ?documento a 'document'.
                            ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
                            ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
                            }}
                        }}
                        UNION
                        {{
                            #Proyectos
                            SELECT {pVarAnterior}
                            WHERE {{
                            ?proy <http://w3id.org/roh/isProducedBy> <[PARAMETRO]>.
                            ?proy a 'project'.
                            ?proy ?propRol ?role.
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?role <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
                            }}
                        }}
                        }}
                        MINUS
                        {{
                            <[PARAMETRO]> ?propRol ?rol.
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> {pVarAnterior}.
                        }}
                        }}
                    }}
                ");


            //Filtros searcher buscadores
            filtrosPersonalizados.Add("searcherPublications",
                 $@"
                    {{
                        SELECT DISTINCT {pVarAnterior}
                        WHERE
                        {{
                            {{
			                    {pVarAnterior} <http://w3id.org/roh/title> ?title.
			                    ?title bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and|| '[PARAMETROESPACIOIN]' |||""
                            }}
                            UNION
		                    {{
			                    {pVarAnterior} vivo:freeTextKeyword ?keywordO.
                                ?keywordO <http://w3id.org/roh/title> ?keyword.
			                    ?keyword bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' |||""
		                    }}
		                    UNION
		                    {{
			                    {pVarAnterior} bibo:abstract ?abstract.
			                    ?abstract bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' |||""
                            }}
                            UNION
		                    {{
			                    {pVarAnterior} bibo:authorList ?authorList.
			                    ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
			                    ?person <http://xmlns.com/foaf/0.1/name> ?namePerson.
			                    ?namePerson bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' |||""
                            }}
                        }}
                    }}
                ");

            filtrosPersonalizados.Add("searcherProjects",
                 $@"
                    {{
	                    select {pVarAnterior}
	                    {{
		                    {pVarAnterior} a 'project'
		                    {{
			                    {pVarAnterior} roh:title ?title.
			                    ?title bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' |||""
                            }}
		                    UNION
		                    {{
			                    {pVarAnterior} bibo:abstract ?abstract.
			                    ?abstract bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' |||""
		                    }}
		                    UNION
                            {{
			                    {pVarAnterior} vivo:freeTextKeyword ?keywordO.
                                ?keywordO roh:title ?keyword.
			                    ?keyword bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' |||""
                            }}
		                    UNION
		                    {{
                                ?person a 'person'.
                                {pVarAnterior} roh:membersProject ?person.
			                    ?person foaf:name ?namePerson.
			                    ?namePerson bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' |||""
		                    }}
	                    }}
                    }}
                ");

            filtrosPersonalizados.Add("searcherPersons",
                 $@"
                    {{
	                    select {pVarAnterior} where 
	                    {{
		                    {{
			                    {pVarAnterior} foaf:name ?namePerson .
			                    ?namePerson bif:contains ""|||[PARAMETROESPACIOULTIMODIFERENTE]|| '[PARAMETROESPACIOIN]' and || '[PARAMETROESPACIOIN]' ||| ""
                            }}
	                    }}
                    }}
                ");


            string varInicial = pVarAnterior;
            string pVarAnteriorAux;

            if (pDicFiltros != null && pDicFiltros.Count > 0)
            {
                StringBuilder filtro = new();

                foreach (KeyValuePair<string, List<string>> item in pDicFiltros)
                {

                    if (filtrosPersonalizados.ContainsKey(item.Key))
                    {
                        if (filtrosPersonalizados[item.Key].Contains("[PARAMETRO]"))
                        {
                            string filtroParametros = filtrosPersonalizados[item.Key].Replace("[PARAMETRO]", item.Value.First());
                            filtro.Append(filtroParametros);
                        }
                        else
                        {
                            string filtroParametros = ObtenerQuerySearch(filtrosPersonalizados[item.Key], item.Value.First());
                            filtro.Append(filtroParametros);
                        }
                    }
                    else
                    {

                        foreach (string valorFiltroIn in item.Value)
                        {
                            if (!filtrosReciprocos.ContainsKey(item.Key))
                            {
                                foreach (string parteFiltro in item.Key.Split(new string[] { "@@@" }, StringSplitOptions.RemoveEmptyEntries))
                                {

                                    string varActual = $@"?{parteFiltro.Substring(parteFiltro.IndexOf(":") + 1)}{pAux}";
                                    filtro.Append($@"{pVarAnterior} ");
                                    filtro.Append($@"{parteFiltro} ");
                                    filtro.Append($@"{varActual}. ");
                                    pVarAnterior = varActual;
                                    pAux++;
                                }
                            }
                            else
                            {
                                int index = filtrosReciprocos[item.Key];
                                pVarAnterior = "?varAuxiliar";
                                pVarAnteriorAux = pVarAnterior;
                                foreach (string parteFiltro in item.Key.Split(new string[] { "@@@" }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    if ((pAux + 1) < index)
                                    {
                                        string varActual = $@"?{parteFiltro.Substring(parteFiltro.IndexOf(":") + 1)}{pAux}";
                                        filtro.Append($@"{pVarAnterior} ");
                                        filtro.Append($@"{parteFiltro} ");
                                        filtro.Append($@"{varActual}. ");
                                        pVarAnterior = varActual;
                                        pAux++;
                                    }
                                    else if ((pAux + 1) == index)
                                    {
                                        string varActual = $@"?{parteFiltro.Substring(parteFiltro.IndexOf(":") + 1)}{pAux}";
                                        filtro.Append($@"{pVarAnterior} ");
                                        filtro.Append($@"{parteFiltro} ");
                                        filtro.Append($@"{varInicial}. ");
                                        pAux++;
                                    }
                                    else
                                    {
                                        filtro.Append($@"{pVarAnteriorAux} ");
                                        filtro.Append($@"{parteFiltro} ");
                                        filtro.Append($@"'{HttpUtility.UrlDecode(valorFiltroIn)}'. ");
                                    }
                                }
                            }

                            // Filtro de fechas.
                            if (filtrosFecha.Contains(item.Key))
                            {
                                filtro.Append($@"FILTER({pVarAnterior} >= {valorFiltroIn.Split('-')[0]}000000) ");
                                filtro.Append($@"FILTER({pVarAnterior} <= {valorFiltroIn.Split('-')[1]}000000) ");
                            }
                            else if (filtrosEnteros.Contains(item.Key))
                            {

                                // Comprueba si es un rango
                                if (valorFiltroIn.Contains('-'))
                                {
                                    filtro.Append($@"FILTER({pVarAnterior} >= {valorFiltroIn.Split('-')[0]}) ");
                                    filtro.Append($@"FILTER({pVarAnterior} <= {valorFiltroIn.Split('-')[1]}) ");
                                }
                                else
                                {
                                    // Si no es un rango...
                                    string valorFiltro = string.Empty;
                                    valorFiltro += $@",{valorFiltroIn}";

                                    if (valorFiltro.Length > 0)
                                    {
                                        valorFiltro = valorFiltro.Substring(1);
                                    }

                                    if (!filtrosReciprocos.ContainsKey(item.Key))
                                    {
                                        filtro.Append($@"FILTER({pVarAnterior} IN ({HttpUtility.UrlDecode(valorFiltro)})) ");
                                    }
                                }
                            }
                            else
                            {
                                string valorFiltro = string.Empty;
                                Uri uriAux = null;
                                bool esUri = Uri.TryCreate(valorFiltroIn, UriKind.Absolute, out uriAux);
                                if (esUri)
                                {
                                    valorFiltro += $@",<{valorFiltroIn}>";
                                }
                                else
                                {
                                    //MultiIdioma.
                                    if (valorFiltroIn.Length > 3 && valorFiltroIn[valorFiltroIn.Length - 3] == '@')
                                    {
                                        valorFiltro += $@",'{valorFiltroIn.Substring(0, valorFiltroIn.Length - 3)}'{valorFiltroIn.Substring(valorFiltroIn.Length - 3)}";
                                    }
                                    else
                                    {
                                        valorFiltro += $@",'{valorFiltroIn}'";
                                    }
                                }
                                if (valorFiltro.Length > 0)
                                {
                                    valorFiltro = valorFiltro.Substring(1);
                                }

                                if (!filtrosReciprocos.ContainsKey(item.Key))
                                {
                                    filtro.Append($@"FILTER({pVarAnterior} IN ({HttpUtility.UrlDecode(valorFiltro.Replace("+", "%2B"))})) ");
                                }
                            }
                            pVarAnterior = varInicial;
                        }
                    }

                }
                return filtro.ToString();
            }
            return string.Empty;
        }




        /// <summary>
        /// Obtiene los filtros por los parámetros de la URL.
        /// </summary>
        /// <param name="pParametros">String de filtros.</param>
        /// <returns>Diccionario de filtros.</returns>
        public static Dictionary<string, List<string>> ObtenerParametros(string pParametros)
        {
            if (!string.IsNullOrEmpty(pParametros))
            {
                pParametros = pParametros.Trim().Trim('#');
                if (!string.IsNullOrEmpty(pParametros))
                {
                    Dictionary<string, List<string>> dicFiltros = new();

                    // Agregamos al diccionario los filtros.
                    foreach (string filtro in pParametros.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string keyFiltro = filtro.Split('=')[0];
                        string valorFiltro = filtro.Split('=')[1];
                        if (dicFiltros.ContainsKey(keyFiltro))
                        {
                            dicFiltros[keyFiltro].Add(valorFiltro);
                        }
                        else
                        {
                            dicFiltros.Add(keyFiltro, new List<string> { valorFiltro });
                        }
                    }

                    return dicFiltros;
                }
            }


            return null;
        }

        /// <summary>
        /// Permite calcular el valor del ancho de la línea según el número de colaboraciones que tenga el nodo.
        /// </summary>
        /// <param name="pMax">Valor máximo.</param>
        /// <param name="pColabo">Número de colaboraciones.</param>
        /// <returns>Ancho de la línea en formate double.</returns>
        public static double CalcularGrosor(int pMax, int pColabo)
        {
            return Math.Round(((double)pColabo / (double)pMax) * 10, 2);
        }

        /// <summary>
        /// Obtiene el año de un string.
        /// </summary>
        /// <param name="pFecha">String del año.</param>
        /// <returns>Año en string.</returns>
        public static string ExtraerAny(string pFecha)
        {
            return pFecha.Substring(0, 4);
        }


        /// <summary>
        /// Permite crear la lista con los colores.
        /// </summary>
        /// <param name="pSize">Tamaño de la lista.</param>
        /// <param name="pColorHex">Colores asignados.</param>
        /// <returns>Lista con los colores.</returns>
        public static List<string> CrearListaColores(int pSize, string pColorHex)
        {
            List<string> listaColores = new();
            for (int i = 0; i < pSize; i++)
            {
                listaColores.Add(pColorHex);
            }
            return listaColores;
        }

        /// <summary>
        /// Mediante el ID del recurso en el grafo de búsqueda a través de su ID en el grafo de la ontología
        /// </summary>
        /// <param name="pRsourceApi">API</param>
        /// <param name="pIdOntologia">ID del grafo de la ontología.</param>
        /// <returns>ID del grafo de búsqueda.</returns>
        public static string ObtenerIdBusqueda(ResourceApi pRsourceApi, string pIdOntologia)
        {
            Guid idCorto = pRsourceApi.GetShortGuid(pIdOntologia);
            return $@"http://gnoss/{idCorto.ToString().ToUpper()}";
        }


        /// <summary>
        /// Función que obtiene el ID corto mediante el ID del recurso en el grafo de búsqueda a través de su ID en el grafo de la ontología
        /// </summary>
        /// <param name="pRsourceApi">API</param>
        /// <param name="pIdOntologia">ID del grafo de la ontología.</param>
        /// <returns>ID del grafo de búsqueda.</returns>
        public static Guid ObtenerIdCorto(ResourceApi pRsourceApi, string pIdOntologia)
        {
            Guid idCorto = pRsourceApi.GetShortGuid(pIdOntologia);
            return idCorto;
        }


        public static void ProcessRelations(string pNombreRelacion, Dictionary<string, List<string>> pItems, ref Dictionary<string, List<DataQueryRelaciones>> pDicRelaciones)
        {
            foreach (string itemA in pItems.Keys)
            {
                if (!pDicRelaciones.ContainsKey(itemA))
                {
                    pDicRelaciones.Add(itemA, new List<DataQueryRelaciones>());
                }
                DataQueryRelaciones dataQueryRelaciones = (pDicRelaciones[itemA].FirstOrDefault(x => x.nombreRelacion == pNombreRelacion));
                if (dataQueryRelaciones == null)
                {
                    dataQueryRelaciones = new DataQueryRelaciones()
                    {
                        nombreRelacion = pNombreRelacion,
                        idRelacionados = new List<Datos>()
                    };
                    pDicRelaciones[itemA].Add(dataQueryRelaciones);
                }
                foreach (string itemB in pItems.Keys)
                {
                    if (itemA != itemB && string.Compare(itemA, itemB, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        int num = pItems[itemA].Intersect(pItems[itemB]).Count();
                        if (num > 0)
                        {
                            dataQueryRelaciones.idRelacionados.Add(new Datos()
                            {
                                idRelacionado = itemB,
                                numVeces = num
                            });
                        }
                    }
                }
                if (dataQueryRelaciones.idRelacionados.Count == 0)
                {
                    pDicRelaciones[itemA].Remove(dataQueryRelaciones);
                }
            }
        }


        /// <summary>
        /// Método que obtiene los Ids largos a través de los Ids cortos 
        /// </summary>
        /// <param name="ids">Lista de Ids cortos sobre los que queremos obtener sus IDs largos.</param>
        /// <param name="mResourceApi">Instancia de la clase ResourceApi.</param>
        /// <param name="rdfOntology">RDF de la ontología.</param>
        /// <param name="nameOntology">Nombre de la ontología.</param>
        /// <param name="urlOntology">(Opcional) Url de la ontología.</param>
        /// <returns>Devuelve una relación (diccionario) de los guids enviados con sus correspondientes Ids largos.</returns>
        internal static Dictionary<Guid, string> GetLongIds(List<Guid> ids, ResourceApi mResourceApi, string rdfOntology, string nameOntology, string urlOntology = null)
        {
            // Diccionario de resultados
            Dictionary<Guid, string> relationProjIDs = new();

            // Definimos la variable que va a contener los IDs de los elementos en formato url
            List<string> idsURL = new();
            List<string> graphs = new() {nameOntology};
            if (ids != null)
            {
                // Obtenemos las urls de los iDs cortos
                ids.ForEach(item =>
                {
                    if (item != Guid.Empty)
                    {
                        idsURL.Add("<http://gnoss.com/" + item.ToString() + ">");
                    }
                });

                // Creamos la url de la ontología si ésta está vacía
                if (urlOntology != null)
                {
                    string onto = urlOntology.Split("http://gnoss.com/").Last().Replace(".owl", "");
                    if (onto != graphs.FirstOrDefault())
                    {
                        graphs.Add(onto);
                    }
                }

                // Query to get the full ID
                if (idsURL.Count > 0)
                {

                    string select = @$"select distinct ?s ?entidad FROM <{urlOntology}>";
                    string where = @$"where {{
                            ?s <http://gnoss/hasEntidad> ?entidad.
                            ?entidad a <{rdfOntology}>.
                            FILTER(?s in ({string.Join(',', idsURL)}))
                        }}
                        ";
                    try
                    {
                        // Obtenemos el diccionario con la relación entre id corto (GUID) e id largo
                        SparqlObject sparqlObject = mResourceApi.VirtuosoQueryMultipleGraph(select, where, graphs);
                        sparqlObject.results.bindings.ForEach(e =>
                        {
                            relationProjIDs.Add(new Guid(e["s"].value.Split("http://gnoss.com/").Last()), e["entidad"].value);
                        });
                    }
                    catch (Exception ex)
                    {
                        mResourceApi.Log.Error("Excepcion: " + ex.Message);
                    }

                }
            }

            return relationProjIDs;
        }

        /// <summary>
        /// Método estático para generar notificaciones a los diferentes usuarios
        /// </summary>
        /// <param name="mResourceApi">Objeto ResourceApi.</param>
        /// <param name="idDDBB">Id del recurso sobre el que la notificación es generada.</param>
        /// <param name="idPersonaFrom">Id del usuario que genera la notificación.</param>
        /// <param name="idPersonaTo">Id del usuario que recibe la notificación.</param>
        /// <param name="tipo">String con el tipo de notificación generada (relacionada con los literales). Por defecto 'editOferta'</param>
        /// <param name="texto">Texto generado para la notificación, por defecto está vacío.</param>
        /// <returns>Diccionario con las listas de thesaurus.</returns>
        internal static bool GenerarNotificacion(ResourceApi mResourceApi, string idDDBB, string idPersonaFrom, string idPersonaTo, string tipo = "editOferta", string texto = "")
        {
            ComplexOntologyResource recursoCargar = new();

            // Notificación de fin de la carga
            if (!string.IsNullOrEmpty(idPersonaTo))
            {
                mResourceApi.ChangeOntoly("notification");
                NotificationOntology.Notification notificacion = new();
                notificacion.IdRoh_owner = idPersonaTo;
                notificacion.IdRoh_trigger = idPersonaFrom;
                notificacion.Roh_text = texto;
                notificacion.Roh_entity = idDDBB;
                notificacion.Dct_issued = DateTime.Now;
                notificacion.Roh_type = tipo;
                recursoCargar = notificacion.ToGnossApiResource(mResourceApi);
                int numIntentos = 0;
                while (!recursoCargar.Uploaded)
                {
                    numIntentos++;
                    if (numIntentos > 5)
                    {
                        break;
                    }
                    mResourceApi.LoadComplexSemanticResource(recursoCargar);
                }
            }

            return recursoCargar.Uploaded;
        }

        /// <summary>
        /// Método estático para obtener los tesauros.
        /// </summary>
        /// <param name="mResourceApi">Objeto ResourceApi.</param>
        /// <param name="pListaTesauros">Listado de thesaurus a obtener.</param>
        /// <param name="lang">Idioma para las cargas multiidioma.</param>
        /// <returns>Diccionario con las listas de thesaurus.</returns>
        internal static Dictionary<string, List<ThesaurusItem>> GetTesauros(ResourceApi mResourceApi, List<string> pListaTesauros, string lang = null)
        {
            Dictionary<string, List<ThesaurusItem>> elementosTesauros = new();

            foreach (string tesauro in pListaTesauros)
            {
                string langSelect = string.Empty;
                if (lang != string.Empty)
                {
                    langSelect = $@"FILTER( lang(?nombre) = '{lang}' OR lang(?nombre) = '')";
                }

                string select = "select distinct * ";
                string where = @$"where {{
                    ?s a <http://www.w3.org/2008/05/skos#Concept>.
                    ?s <http://www.w3.org/2008/05/skos#prefLabel> ?nombre.
                    ?s <http://purl.org/dc/elements/1.1/source> '{tesauro}'
                    {langSelect} 
                    OPTIONAL {{ ?s <http://www.w3.org/2008/05/skos#broader> ?padre }}
                }} ORDER BY ?padre ?s ";
                SparqlObject sparqlObject = mResourceApi.VirtuosoQuery(select, where, "taxonomy");

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
        /// Método estático para obtener las taxonomías de un 'CategoryPath'.
        /// </summary>
        /// <param name="mResourceApi">Objeto del ResourceApi necesario.</param>
        /// <param name="terms">Listado de la categoría a obtener.</param>
        /// <param name="ontology">Ontología donde se encuentra el item.</param>
        /// <returns>listado de las categorías.</returns>
        internal static List<string> LoadCurrentTerms(ResourceApi mResourceApi, List<string> terms, string ontology)
        {

            string termsTxt = string.Join(',', terms.Select(e => "<" + e + ">"));

            string select = "select ?o";
            string where = @$"where {{
                ?s a <http://w3id.org/roh/CategoryPath>.
                ?s <http://w3id.org/roh/categoryNode> ?o.
                MINUS
                {{
                    ?o <http://www.w3.org/2008/05/skos#narrower> ?hijos
                }}
                FILTER(?s IN ({termsTxt}))
            }}";
            SparqlObject sparqlObject = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { ontology , "taxonomy" });

            List<string> termsRes = new();

            sparqlObject.results.bindings.ForEach(e =>
            {
                termsRes.Add(e["o"].value);
            });

            return termsRes;
        }

        /// <summary>
        /// Método que obtiene el id investigador del usuario logueado
        /// </summary>
        /// <param name="mResourceApi">Objeto del ResourceApi necesario.</param>
        /// <param name="pIdGnossUser">Id corto del usuario.</param>
        /// <returns>listado del usuario investigador.</returns>
        internal static string GetResearcherIdByGnossUser(ResourceApi mResourceApi, Guid pIdGnossUser)
        {

            // Obtener el id del usuario usando el id de la cuenta
            string select = "select ?s ";
            string where = @$"where {{
                    ?s a <http://xmlns.com/foaf/0.1/Person>.
                    ?s <http://w3id.org/roh/gnossUser> ?idGnoss.
                    FILTER(?idGnoss = <http://gnoss/{pIdGnossUser.ToString().ToUpper()}>)
                }}";
            SparqlObject sparqlObject = mResourceApi.VirtuosoQuery(select, where, "person");
            string userGnossId = string.Empty;

            sparqlObject.results.bindings.ForEach(e =>
            {
                userGnossId = e["s"].value;
            });

            return userGnossId;
        }

    }
}
