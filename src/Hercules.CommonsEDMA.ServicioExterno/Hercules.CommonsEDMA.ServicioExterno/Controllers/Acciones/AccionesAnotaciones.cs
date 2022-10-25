using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using AnnotationOntology;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesAnotaciones
    {

        #region --- Constantes     
        private static string RUTA_OAUTH = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
        private static ResourceApi mResourceAPI = null;
        private static CommunityApi mCommunityAPI = null;
        private static Guid? mIDComunidad = null;
        private static string RUTA_PREFIJOS = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Models{Path.DirectorySeparatorChar}JSON{Path.DirectorySeparatorChar}prefijos.json";
        private static string mPrefijos = string.Join(" ", JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(RUTA_PREFIJOS)));
        private static string COLOR_GRAFICAS = "#6cafe3";
        private static string COLOR_GRAFICAS_HORIZONTAL = "#6cafe3"; //#1177ff
        #endregion

        private static ResourceApi resourceApi
        {
            get
            {
                while (mResourceAPI == null)
                {
                    try
                    {
                        mResourceAPI = new ResourceApi(RUTA_OAUTH);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido iniciar ResourceApi");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mResourceAPI;
            }
        }

        private static CommunityApi communityApi
        {
            get
            {
                while (mCommunityAPI == null)
                {
                    try
                    {
                        mCommunityAPI = new CommunityApi(RUTA_OAUTH);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido iniciar CommunityApi");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mCommunityAPI;
            }
        }

        private static Guid idComunidad
        {
            get
            {
                while (!mIDComunidad.HasValue)
                {
                    try
                    {
                        mIDComunidad = communityApi.GetCommunityId();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido obtener el ID de la comnunidad");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mIDComunidad.Value;
            }
        }

        public List<Dictionary<string, string>> GetOwnAnnotationsInRO(string idRO, string idUser, string rdfType, string ontology)
        {

            List<Dictionary<string, string>> typesRO = new();

            // "http://purl.org/ontology/bibo/Document", "document"
            // "http://w3id.org/roh/ResearchObject", "researchobject"
            //typesRO.Add("document", "http://purl.org/ontology/bibo/Document");
            //typesRO.Add("researchobject", "http://w3id.org/roh/ResearchObject");


            // Obtengo el id del RO si es Guid
            Guid guidRO = Guid.Empty;
            Dictionary<Guid, string> longsIdRO = new();
            if (Guid.TryParse(idRO, out guidRO))
            {
                longsIdRO = UtilidadesAPI.GetLongIds(new List<Guid>() { guidRO }, resourceApi, rdfType, ontology);
                idRO = longsIdRO[guidRO];
            }
            else
            {
                guidRO = resourceApi.GetShortGuid(idRO);
            }


            // Obtengo el id del usuario si es Guid
            Guid guidUser = Guid.Empty;
            Dictionary<Guid, string> longsIdUs = new();
            if (!Guid.TryParse(idUser, out guidUser))
            {
                guidUser = resourceApi.GetShortGuid(idUser);
            }


            // Obtener el id del usuario usando el id de la cuenta
            string userGnossId = UtilidadesAPI.GetResearcherIdByGnossUser(resourceApi, guidUser);




            // Obtenemos todos los datos de los perfiles y Añadimos el perfil creado a los datos de la oferta
            string select = "select distinct ?s ?date ?texto ";

            //string filterRO = "";
            //switch (ontology)
            //{
            //    case "document":
            //        filterRO = @$"
            //            ?s <http://w3id.org/roh/document> ?document.
            //            FILTER(?document = <{idRO}>)) ";
            //        break;

            //    case "researchobject":
            //        filterRO = @$"
            //            ?s <http://w3id.org/roh/researchobject> ?ro.
            //            FILTER(?ro = <{idRO}>)) ";
            //        break;
            //}

            string where = @$"where {{

                ?s a <http://w3id.org/roh/Annotation>.
                ?s <http://w3id.org/roh/text> ?texto.
                ?s <http://w3id.org/roh/dateIssued> ?date.

                # Filtra por el RO
                ?s ?roRfType ?ro.
                FILTER (?roRfType in (<http://w3id.org/roh/document>, <http://w3id.org/roh/researchobject>))
                FILTER(?ro = <{idRO}>)

                # Filtra por el usuario actual 
                ?s <http://w3id.org/roh/owner> ?user.
                FILTER(?user = <{userGnossId}>)
            }} ORDER BY DESC(?date)";

            SparqlObject sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person" , "annotation" });

            // Carga los datos en el objeto
            sparqlObject.results.bindings.ForEach(e =>
            {
                try
                {
                    // Cargamos las variables
                    var id = e["s"].value;
                    var texto = e["texto"].value;

                    // Cargamos la fecha
                    DateTime fechaDate = DateTime.UtcNow;
                    var fecha = DateTime.UtcNow.ToString("g");
                    try
                    {
                        fechaDate = DateTime.ParseExact(e["date"].value, "yyyyMMddHHmmss", null);
                        fecha = fechaDate.ToString("g");
                    }
                    catch (Exception ex)
                    {
                        resourceApi.Log.Error("Excepcion: " + ex.Message);
                    }

                    // Creamos el diccionario
                    Dictionary<string, string> longs = new Dictionary<string, string>();
                    longs.Add("id", id);
                    longs.Add("fecha", fecha);
                    longs.Add("texto", texto);

                    // Añadimos el diccionario al listado
                    typesRO.Add(longs);

                }
                catch (Exception ext) {
                    throw new ArgumentException("Ha habido un error al procesar los datos de los usuarios: " + ext.Message);
                }

            });



            return typesRO;
        }


        public string CreateNewAnnotation(string idRO, string idUser, string rdfType, string ontology, string texto, string idAnnotation = null)
        {

            // Obtengo el id del RO si es Guid
            Guid guidRO = Guid.Empty;
            Dictionary<Guid, string> longsIdRO = new();
            if (Guid.TryParse(idRO, out guidRO))
            {
                longsIdRO = UtilidadesAPI.GetLongIds(new List<Guid>() { guidRO }, resourceApi, rdfType, ontology);
                idRO = longsIdRO[guidRO];
            }
            else
            {
                guidRO = resourceApi.GetShortGuid(idRO);
            }


            // Obtengo el id del usuario si es Guid
            Guid guidUser = Guid.Empty;
            Dictionary<Guid, string> longsIdUs = new();
            if (!Guid.TryParse(idUser, out guidUser))
            {
                guidUser = resourceApi.GetShortGuid(idUser);
            }

            // Obtener el id del investigador usando el id de la cuenta
            string researcherId = UtilidadesAPI.GetResearcherIdByGnossUser(resourceApi, guidUser);

            int MAX_INTENTOS = 10;
            bool uploadedR = false;


            if (!string.IsNullOrEmpty(researcherId))
            {

                // creando los cluster
                Annotation cRsource = new();

                // Usuario creador
                cRsource.IdRoh_owner = researcherId;

                // Otros campos
                cRsource.Roh_dateIssued = DateTime.UtcNow;

                cRsource.Roh_title = "-";

                // Sección de las descripciones, limpiamos los strings de tags que no queramos
                cRsource.Roh_text = texto != null ? CleanHTML.StripTagsCharArray(texto.Replace("&", "&amp").Replace("<", "&lt").Replace(">", "&gt").Replace("\"", "&quot").Replace("\'", "&apos"), new string[] { }, new string[] { }) : "";// != null ? CleanHTML.StripTagsCharArray(texto, new string[] {}, new string[] {}) : "";

                // Comprobamos si es un documento u otro RO cualquiera
                cRsource.IdsRoh_researchobject = new();
                cRsource.IdsRoh_document = new();

                switch (ontology)
                {
                    case "document":
                        cRsource.IdsRoh_document = new List<string>() { idRO };
                        break;
                    case "researchobject":
                        cRsource.IdsRoh_researchobject = new List<string>() { idRO };
                        break;
                }


                // Guardando o actualizando el recurso
                resourceApi.ChangeOntoly("annotation");
                // Comprueba si es una actualización o no
                if (idAnnotation != null)
                {

                    string[] recursoSplit = idAnnotation.Split('_');

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
                    // Creación.
                    ComplexOntologyResource resource = cRsource.ToGnossApiResource(resourceApi, null);
                    int numIntentos = 0;
                    while (!resource.Uploaded)
                    {
                        numIntentos++;
                        if (numIntentos > MAX_INTENTOS)
                        {
                            break;
                        }
                        idAnnotation = resourceApi.LoadComplexSemanticResource(resource, false, true);
                        uploadedR = resource.Uploaded;
                    }
                }
            }

            if (uploadedR)
            {
                return idAnnotation;
            }
            else
            {
                throw new Exception("Recurso no creado");
            }


        }

        public bool DeleteAnnotation(string idAnnotation)
        {
            // Obtengo el id del RO si es Guid
            Guid guidAnnotation;
            guidAnnotation = resourceApi.GetShortGuid(idAnnotation);
            return resourceApi.PersistentDelete(guidAnnotation);
        }

        public string getUserFromAnnotation(string idAnnotation)
        {
            string select = "SELECT DISTINCT ?usuario ";
            string where = @$"WHERE 
            {{
                <{idAnnotation}> <http://w3id.org/roh/owner> ?person.
                ?person <http://w3id.org/roh/gnossUser> ?usuario.
            }}";

            SparqlObject sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where,new List<string>() { "annotation", "person" });

            // Carga los datos en el objeto
            if (sparqlObject != null && sparqlObject.results != null && sparqlObject.results.bindings != null && sparqlObject.results.bindings.Count > 0)
            {
                Dictionary<string, SparqlObject.Data> bind = sparqlObject.results.bindings.FirstOrDefault();
                if (bind != null && bind.ContainsKey("usuario"))
                {
                    return bind["usuario"].value.Split("/").LastOrDefault();
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            } 
        }
    }

}
