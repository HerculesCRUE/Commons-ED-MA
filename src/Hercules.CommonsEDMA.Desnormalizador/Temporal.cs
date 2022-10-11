using Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores;
using Hercules.CommonsEDMA.Desnormalizador.Models.Services;
using Hercules.CommonsEDMA.Desnormalizador.Models.Similarity;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.Desnormalizador
{
    public static class Temporal
    {
        private readonly static string rutaOauth = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config/ConfigOAuth/OAuthV3.config";
        private static ResourceApi resourceApi = new ResourceApi(rutaOauth);
        private static UserApi userApi = new UserApi(rutaOauth);
        private static CommunityApi communityApi = new CommunityApi(rutaOauth);

        public static void CrearPersonas()
        {
            //Dictionary<Guid, List<TriplesToInclude>> dic = new Dictionary<Guid, List<TriplesToInclude>>();
            //Guid idOtri = resourceApi.GetShortGuid("http://gnoss.com/items/Person_70582872-7f4f-4a76-a150-c0c4b8db1522_34a05e34-641a-4735-88f4-357cca8aaac6");
            //dic.Add(idOtri,new List<TriplesToInclude>() { new TriplesToInclude() { NewValue = "true", Predicate = "http://w3id.org/roh/isOtriManager" } });
            //var x=resourceApi.InsertPropertiesLoadedResources(dic);

            //var resultado = resourceApi.VirtuosoQuery("select *", "where{?s <http://w3id.org/roh/generatedPDFFile> ?o}","curriculumvitae");
            //foreach(Dictionary<string,SparqlObject.Data> fila in resultado.results.bindings)
            //{
            //    Dictionary<Guid, List<RemoveTriples>> dic2 = new Dictionary<Guid, List<RemoveTriples>>();
            //    string id = fila["s"].value;
            //    string pdf = fila["o"].value;
            //    dic2.Add(resourceApi.GetShortGuid(id), new List<RemoveTriples>() { new RemoveTriples() { Predicate = "http://w3id.org/roh/generatedPDFFile", Value = pdf } });
            //    resourceApi.DeletePropertiesLoadedResources(dic2);
            //}



            //Antonio Skaremta 28710458
            AltaUsuarioGnoss("Antonio", "Skarmeta", "antonio--skarmeta@pruebagnoss.com", "skarmeta22", "28710458", "AdrianSaavedra-GNOSS", "12070100");

            //Manuel Campos 34822542
            AltaUsuarioGnoss("Manuel", "Campos", "manuel--campos@pruebagnoss.com", "manuel-camp2", "34822542", "manuelCampos-github", "22222222");

            //Francisco Esquembre 27443184
            AltaUsuarioGnoss("Francisco", "Esquembre", "francisco--esquembre@pruebagnoss.com", "francisco-es", "27443184", "franciscoEsquembre-github", "33333333");

            //José Tomás 8310
            AltaUsuarioGnoss("Jose", "Tomas", "jose---tomas@pruebagnoss.com", "jose-tomas", "31248453", "", "");

            //Diana Castilla 27281387213879
            AltaUsuarioGnoss("Diana", "Castilla", "diana---castilla@pruebagnoss.com", "diana-castil", "27281387213879", "", "");

            //Felix Cesareo Gomez de Leon Hijes
            AltaUsuarioGnoss("Felix", "Cesareo", "Felix--Cesareo@pruebagnoss.com", "felix-cesare", "22463209", "", "");

            //Fernando Jimenez Barrionuevo
            AltaUsuarioGnoss("Fernando", "Jimenez", "Fernando---Jimenez@pruebagnoss.com", "fernando-jim", "29084098", "", "");

            //Gracia Sanchez Carpena
            AltaUsuarioGnoss("Gracia", "Sanchez", "Gracia---Sanchez@pruebagnoss.com", "gracia-sanch", "22144772", "", "");

            //Jose Manuel Juarez Herrero
            AltaUsuarioGnoss("Jose", "Juarez", "Jose---Juarez@pruebagnoss.com", "jose-juarez", "48479115", "", "");

            //Maria Antonia Cardenas Viedma
            AltaUsuarioGnoss("Maria", "Cardenas", "Maria---Cardenas@pruebagnoss.com", "maria-carden", "26476225", "", "");

            //Elena Garcia Barriocanal
            AltaUsuarioGnoss("Elena", "Garcia Barriocanal", "elena--garcia@pruebagnoss.com", "elena-garcia", "11335577992468", "", "");

            //Miguel Ángel Sicilia
            AltaUsuarioGnoss("Miguel Angel", "Sicilia", "miguel--sicilia@pruebagnoss.com", "miguel-angel", "224466880013579", "", "");

            //Marçal Mora Cantallops
            AltaUsuarioGnoss("Marcal", "Mora Cantallops", "marcal--mora@pruebagnoss.com", "marcal-mora-", "113355112223334", "", "");

            //Juan Manuel Dodero
            AltaUsuarioGnoss("Juan", "Manuel Dodero", "juan--manuel@pruebagnoss.com", "juan-manuel-", "31256195", "", "");

            //Andres Muñoz Ortega
            AltaUsuarioGnoss("Andres", "Munoz Ortega", "andres--munoz@pruebagnoss.com", "andres-munoz", "48466315", "", "");

            //Daniela Fernandez
            AltaUsuarioGnoss("Daniela", "Fernandez", "daniela--fernandez@pruebagnoss.com", "daniela-fern", "113636170221114", "", "");

            //Daniela Fernandez
            AltaUsuarioGnoss("Daniela", "Fernandez", "daniela--fernandez@pruebagnoss.com", "daniela-fern", "113636170221114", "", "");

            //Isabel Hernández García
            AltaUsuarioGnoss("Isabel", "Hernandez", "isabel--hernandez@pruebagnoss.com", "isabel-herna", "74336159", "", "");

            //Pedro Martín Peña
            AltaUsuarioGnoss("Pedro", "Martin Pena", "sugerencias.cvn@pruebagnoss.es", "pedro-martin", "46462369", "", "");

            //Test Fecyt Vacio
            AltaUsuarioGnoss("Test", "Fecyt Vacio", "test-fecyt-vacio@pruebagnoss.com", "test-fecyt-v", "22559987903", "", "");

            AltaUsuarioGnoss("Joaquin", "Tribaldos", "joaquin.tribaldos@fecyt.es", "joaquin-trib", "235312462347", "", "");

            AltaUsuarioGnoss("Alfonso", "Lopez", "alfonso.lopez@fecyt.es", "alfonso-lope", "712345634685", "", "");

            AltaUsuarioGnoss("Antonio", "Sanchez", "Antonio.sanchez@fecyt.es", "antonio-sanc", "346244756785", "", "");

            AltaUsuarioGnoss("Estefania", "Guitierrez", "estefania.gutierrez@fecyt.es", "estefania-gu", "324625873546", "", "");

            AltaUsuarioGnoss("Aurelia", "Andres", "aurelia.andres@fecyt.es", "aurelia-and1", "579865434363", "", "");

            AltaUsuarioGnoss("Bernardo", "Canovas", "Bernardo.Canovas@pruebagnoss.es", "bernardo-can", "48487426", "", "");

            AltaUsuarioGnoss("Jose", "Fernandez", "Jose.Fernandez@pruebagnoss.es", "jose-fernand", "29062423", "", "");

            AltaUsuarioGnoss("Rafael", "Valencia", "Rafael.Valencia@pruebagnoss.es", "rafael-valen", "48392732", "", "");



            AltaUsuarioGnoss("Felix", "Gomez", "Felix.Gomez@pruebagnoss.es", "felix-gomez", "48507359", "", "");

            AltaUsuarioGnoss("Juan Antonio", "Madrid", "juan.madrid@pruebagnoss.es", "juan-antonio", "22926115", "", "");

            AltaUsuarioGnoss("Juan Antonio", "Botia", "juan.botia@pruebagnoss.es", "juan-antoni1", "52807499", "", "");

            AltaUsuarioGnoss("Antonio", "Morales", "antonio.morales@pruebagnoss.es", "antonio-mora", "48485751", "", "");

            AltaUsuarioGnoss("Senena", "Corbalan", "senena.corbalan@pruebagnoss.es", "senena-corba", "77562435", "", "", "0000-0003-1840-5578");



        }

        public static User AltaUsuarioGnoss(string pNombre, string pApellidos, string pEmail, string pNombreCorto, string pID, string pUsuarioGitHub, string pUsuarioFigShare, string pOrcid = null)
        {
            User user = userApi.GetUserByShortName(pNombreCorto);

            if (user == null)
            {
                user = new User();
                try
                {
                    user.name = pNombre;
                    user.last_name = pApellidos;
                    user.email = pEmail;
                    user.password = "123gnoss";
                    user.community_short_name = "hercules";
                    user.user_short_name = pNombreCorto;
                    user.user_id = Guid.NewGuid();
                    user = userApi.CreateUser(user);
                }
                catch (Exception ex)
                {
                }
            }
            //Vincular con la persona
            //Obtenemos la persona
            Dictionary<string, SparqlObject.Data> fila = resourceApi.VirtuosoQuery("select *", $@"where
                                                                                            {{
                                                                                                ?s <http://w3id.org/roh/crisIdentifier> '{pID}'. 
                                                                                                OPTIONAL{{?s <http://w3id.org/roh/gnossUser> ?user }}
                                                                                                OPTIONAL{{?s <http://w3id.org/roh/usuarioGitHub> ?userGit }}
                                                                                                OPTIONAL{{?s <http://w3id.org/roh/usuarioFigShare> ?userFigShare }}
                                                                                                OPTIONAL{{?s <http://w3id.org/roh/ORCID> ?ORCID }}
                                                                                            }}", "person").results.bindings.First();
            string idPerona = fila["s"].value;

            Dictionary<string, string> dicPropiedadValorActual = new Dictionary<string, string>();
            Dictionary<string, string> dicPropiedadValorCargar = new Dictionary<string, string>();
            //USER
            dicPropiedadValorActual["http://w3id.org/roh/gnossUser"] = "";
            if (fila.ContainsKey("user"))
            {
                dicPropiedadValorActual["http://w3id.org/roh/gnossUser"] = fila["user"].value;
            }
            dicPropiedadValorCargar["http://w3id.org/roh/gnossUser"] = "http://gnoss/" + user.user_id.ToString().ToUpper();
            //USERGit
            dicPropiedadValorActual["http://w3id.org/roh/usuarioGitHub"] = "";
            if (fila.ContainsKey("userGit"))
            {
                dicPropiedadValorActual["http://w3id.org/roh/usuarioGitHub"] = fila["userGit"].value;
            }
            dicPropiedadValorCargar["http://w3id.org/roh/usuarioGitHub"] = pUsuarioGitHub;
            //USERFigShare
            dicPropiedadValorActual["http://w3id.org/roh/usuarioFigShare"] = "";
            if (fila.ContainsKey("userFigShare"))
            {
                dicPropiedadValorActual["http://w3id.org/roh/usuarioFigShare"] = fila["userFigShare"].value;
            }
            dicPropiedadValorCargar["http://w3id.org/roh/usuarioFigShare"] = pUsuarioFigShare;

            if (!string.IsNullOrEmpty(pOrcid))
            {
                dicPropiedadValorActual["http://w3id.org/roh/ORCID"] = "";
                if (fila.ContainsKey("ORCID"))
                {
                    dicPropiedadValorActual["http://w3id.org/roh/ORCID"] = fila["ORCID"].value;
                }
                dicPropiedadValorCargar["http://w3id.org/roh/ORCID"] = pOrcid;
            }

            foreach (string prop in dicPropiedadValorCargar.Keys)
            {
                if (!string.IsNullOrEmpty(dicPropiedadValorCargar[prop]) && string.IsNullOrEmpty(dicPropiedadValorActual[prop]))
                {
                    //Insertamos
                    Dictionary<Guid, List<TriplesToInclude>> triples = new() { { resourceApi.GetShortGuid(idPerona), new List<TriplesToInclude>() } };
                    TriplesToInclude t = new();
                    t.Predicate = prop;
                    t.NewValue = dicPropiedadValorCargar[prop];
                    triples[resourceApi.GetShortGuid(idPerona)].Add(t);
                    var resultado = resourceApi.InsertPropertiesLoadedResources(triples);
                }
                else if (!string.IsNullOrEmpty(dicPropiedadValorCargar[prop]) &&
                   !string.IsNullOrEmpty(dicPropiedadValorActual[prop]) &&
                   dicPropiedadValorCargar[prop] != dicPropiedadValorActual[prop])
                {
                    //Modificamos
                    //Si el valor nuevo y el viejo no son nulos -->modificamos
                    TriplesToModify t = new();
                    t.NewValue = dicPropiedadValorCargar[prop];
                    t.OldValue = dicPropiedadValorActual[prop];
                    t.Predicate = prop;
                    var resultado = resourceApi.ModifyPropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.TriplesToModify>>() { { resourceApi.GetShortGuid(idPerona), new List<Gnoss.ApiWrapper.Model.TriplesToModify>() { t } } });
                }
            }

            //Cambio correo skarmeta
            if (pID == "28710458")
            {
                Dictionary<Guid, List<TriplesToModify>> triples = new Dictionary<Guid, List<TriplesToModify>>();
                TriplesToModify t = new TriplesToModify("alvaro.palacios@um.es", "skarmeta@um.es", "https://www.w3.org/2006/vcard/ns#email");
                triples.Add(resourceApi.GetShortGuid(idPerona), new List<TriplesToModify>() { t });
                resourceApi.ModifyPropertiesLoadedResources(triples);
            }

            //Cambio correo Isabel Hernández García
            if (pID == "74336159")
            {
                Dictionary<Guid, List<TriplesToModify>> triples = new Dictionary<Guid, List<TriplesToModify>>();
                TriplesToModify t = new TriplesToModify("isabel.m.h@um.es", "isabelhg@um.es", "https://www.w3.org/2006/vcard/ns#email");
                triples.Add(resourceApi.GetShortGuid(idPerona), new List<TriplesToModify>() { t });
                resourceApi.ModifyPropertiesLoadedResources(triples);
            }

            return user;
        }

        /// <summary>
        /// IMPORTANTE!!! esto sólo debe usarse para pruebas, si se eliminan los datos no son recuperables
        /// Elimina los datos desnormalizados
        /// </summary>
        public static void EliminarCVs()
        {
            bool eliminarDatos = false;
            //IMPORTANTE!!!
            //No descomentar, esto sólo debe usarse para pruebas, si se eliminan los datos no son recuperables
            if (eliminarDatos)
            {
                //Eliminamos los CV
                while (true)
                {
                    int limit = 500;
                    String select = @"SELECT ?cv ";
                    String where = @$"  where{{
                                            ?cv a <http://w3id.org/roh/CV>.
                                        }} limit {limit}";

                    SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, "curriculumvitae");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = 10 }, fila =>
                    {
                        resourceApi.PersistentDelete(resourceApi.GetShortGuid(fila["cv"].value));
                    });
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        public static void EliminarEntidadesCV()
        {
            String select = @"SELECT ?cv ?pLvl1 ?oLvl1 ?pLvl2 ?oLvl2 ?entity ?isValidated";
            String where = @$"  where{{
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv ?pLvl1 ?oLvl1.
                                            ?oLvl1 ?pLvl2 ?oLvl2.
                                            ?oLvl2 <http://vivoweb.org/ontology/core#relatedBy> ?entity.
                                            OPTIONAL{{?entity <http://w3id.org/roh/isValidated> ?isValidated}}
                                            FILTER(?cv =<http://gnoss.com/items/CV_d5c72945-6106-4f2d-a91b-fda3f5438054_fcbb2d02-8cfb-49ef-882f-b45c5dde9c0c>)
                                        }}";

            SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, "curriculumvitae");

            List<Guid> eliminar = new List<Guid>();
            Dictionary<Guid, List<RemoveTriples>> dicEliminar = new Dictionary<Guid, List<RemoveTriples>>();
            foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings)
            {
                if (!fila["entity"].value.Contains("Document") && !fila["entity"].value.Contains("Project") && !fila["entity"].value.Contains("Group"))
                {
                    eliminar.Add(resourceApi.GetShortGuid(fila["entity"].value));
                    Guid idCV = resourceApi.GetShortGuid(fila["cv"].value);
                    if (!dicEliminar.ContainsKey(idCV))
                    {
                        dicEliminar.Add(idCV, new List<RemoveTriples>());
                    }
                    RemoveTriples remove = new RemoveTriples(fila["oLvl1"].value + "|" + fila["oLvl2"].value, fila["pLvl1"].value + "|" + fila["pLvl2"].value);
                    dicEliminar[idCV].Add(remove);
                }
                else if(!fila.ContainsKey("isValidated") || fila["isValidated"].value=="false")
                {
                    eliminar.Add(resourceApi.GetShortGuid(fila["entity"].value));
                    Guid idCV = resourceApi.GetShortGuid(fila["cv"].value);
                    if (!dicEliminar.ContainsKey(idCV))
                    {
                        dicEliminar.Add(idCV, new List<RemoveTriples>());
                    }
                    RemoveTriples remove = new RemoveTriples(fila["oLvl1"].value + "|" + fila["oLvl2"].value, fila["pLvl1"].value + "|" + fila["pLvl2"].value);
                    dicEliminar[idCV].Add(remove);
                }
            }
            Dictionary<Guid, bool> resp = resourceApi.DeletePropertiesLoadedResources(dicEliminar);

            foreach (Guid elim in eliminar)
            {
                try
                {
                    resourceApi.PersistentDelete(elim);
                }
                catch (Exception ex)
                {

                }
            }

        }


        public static void ElimnarDatosAdicionales()
        {
            List<Tuple<string, string, string, string>> listaEliminar = new List<Tuple<string, string, string, string>>();
            //Eliminar publicaciones
            listaEliminar.Add(new Tuple<string, string, string, string>("http://purl.org/ontology/bibo/Document", "document", "", ""));
            //Eliminar cvs
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/CV", "curriculumvitae", "", ""));
            //Eliminar proyectos no validados
            listaEliminar.Add(new Tuple<string, string, string, string>("http://vivoweb.org/ontology/core#Project", "project", "http://w3id.org/roh/isValidated", "true"));
            //Eliminar grupos no validados
            listaEliminar.Add(new Tuple<string, string, string, string>("http://xmlns.com/foaf/0.1/Group", "group", "http://w3id.org/roh/isValidated", "true"));
            //Eliminar personas no activas
            listaEliminar.Add(new Tuple<string, string, string, string>("http://xmlns.com/foaf/0.1/Person", "person", "http://w3id.org/roh/isActive", "true"));

            //Otros datos de CVs            
            listaEliminar.Add(new Tuple<string, string, string, string>("http://vivoweb.org/ontology/core#AcademicDegree", "academicdegree", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Accreditation", "accreditation", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Activity", "activity", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Collaboration", "collaboration", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Committee", "committee", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Council", "council", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://vivoweb.org/ontology/core#Grant", "grant", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/ImpartedAcademicTraining", "impartedacademictraining", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/ImpartedCoursesSeminars", "impartedcoursesseminars", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/LanguageCertificate", "languagecertificate", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Network", "network", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://vivoweb.org/ontology/core#Position", "position", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/ScientificProduction", "scientificproduction", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Society", "society", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Stay", "stay", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/SupervisedArtisticProject", "supervisedartisticproject", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/TeachingCongress", "teachingcongress", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/TeachingProject", "teachingproject", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/TeachingPublication", "teachingpublication", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/TechnologicalResult", "technologicalresult", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/ThesisSupervision", "thesissupervision", "", ""));
            listaEliminar.Add(new Tuple<string, string, string, string>("http://w3id.org/roh/Tutorship", "tutorship", "", ""));


            foreach (Tuple<string, string, string, string> item in listaEliminar)
            {
                string filterAux = "";
                if (!string.IsNullOrEmpty(item.Item3) && !string.IsNullOrEmpty(item.Item4))
                {
                    filterAux = $"MINUS{{?s <{item.Item3}> '{item.Item4}'}} ";
                }

                String select = @"SELECT ?s";
                String where = @$"  where{{
                                            ?s a <{item.Item1}>.
                                            {filterAux}
                                        }}";

                SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, item.Item2);
                Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = 100 }, fila =>
                {
                    try
                    {
                        if (resultado.results.bindings.Last() == fila)
                        {
                            resourceApi.PersistentDelete(resourceApi.GetShortGuid(fila["s"].value), true, true);
                        }
                        else
                        {
                            resourceApi.PersistentDelete(resourceApi.GetShortGuid(fila["s"].value));
                        }
                        Console.WriteLine(item.Item2 + " : " + resultado.results.bindings.IndexOf(fila) + "/" + resultado.results.bindings.Count);
                    }
                    catch (Exception)
                    {

                    }
                });
            };

            foreach (Tuple<string, string, string, string> item in listaEliminar)
            {
                string filterAux = "";
                if (!string.IsNullOrEmpty(item.Item3) && !string.IsNullOrEmpty(item.Item4))
                {
                    filterAux = $"MINUS{{?s <{item.Item3}> '{item.Item4}'}} ";
                }

                String select = @"SELECT ?s";
                String where = @$"  where{{
                                            ?s a '{item.Item2}'.
                                            {filterAux}
                                        }}";

                SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, communityApi.GetCommunityId());
                Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = 100 }, fila =>
                {
                    try
                    {
                        if (resultado.results.bindings.Last() == fila)
                        {
                            resourceApi.PersistentDelete(resourceApi.GetShortGuid(fila["s"].value), true, true);
                        }
                        else
                        {
                            resourceApi.PersistentDelete(resourceApi.GetShortGuid(fila["s"].value));
                        }
                        Console.WriteLine(item.Item2 + " : " + resultado.results.bindings.IndexOf(fila) + "/" + resultado.results.bindings.Count);
                    }
                    catch (Exception)
                    {

                    }
                });
            };

            //Actualizar fecha actualizacion personas
            {
                String select = @"SELECT ?s ?date";
                String where = @$"  where{{
                                            ?s a <http://xmlns.com/foaf/0.1/Person>.
                                            ?s <http://w3id.org/roh/lastUpdatedDate> ?date 
                                        }}";

                SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, "person");
                Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = 100 }, fila =>
                {
                    Dictionary<Guid, List<RemoveTriples>> triplesElininar = new Dictionary<Guid, List<RemoveTriples>>();
                    triplesElininar[resourceApi.GetShortGuid(fila["s"].value)] = new List<RemoveTriples>() {
                        new RemoveTriples()
                        {
                             Predicate="http://w3id.org/roh/lastUpdatedDate",
                             Value=fila["date"].value
                        }
                    };
                    resourceApi.DeletePropertiesLoadedResources(triplesElininar);
                });
            }
        }

        public static void InsertarColaDesnormalizador(RabbitServiceWriterDenormalizer rabbitService, string queue)
        {
            List<Tuple<DenormalizerItemQueue.ItemType, string, string>> cargar = new List<Tuple<DenormalizerItemQueue.ItemType, string, string>>();
            cargar.Add(new Tuple<DenormalizerItemQueue.ItemType, string, string>(Models.Services.DenormalizerItemQueue.ItemType.document, "document", "http://purl.org/ontology/bibo/Document"));
            cargar.Add(new Tuple<DenormalizerItemQueue.ItemType, string, string>(Models.Services.DenormalizerItemQueue.ItemType.group, "group", "http://xmlns.com/foaf/0.1/Group"));
            cargar.Add(new Tuple<DenormalizerItemQueue.ItemType, string, string>(Models.Services.DenormalizerItemQueue.ItemType.person, "person", "http://xmlns.com/foaf/0.1/Person"));
            cargar.Add(new Tuple<DenormalizerItemQueue.ItemType, string, string>(Models.Services.DenormalizerItemQueue.ItemType.project, "project", "http://vivoweb.org/ontology/core#Project"));
            cargar.Add(new Tuple<DenormalizerItemQueue.ItemType, string, string>(Models.Services.DenormalizerItemQueue.ItemType.researchobject, "researchobject", "http://w3id.org/roh/ResearchObject"));


            foreach (Tuple<DenormalizerItemQueue.ItemType, string, string> tupla in cargar)
            {
                SparqlObject sparqlObject = resourceApi.VirtuosoQuery("select ?s", $"where{{?s a <{tupla.Item3}>}}", tupla.Item2);

                List<string> items = new List<string>();
                foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
                {
                    items.Add(fila["s"].value);
                }
                List<List<string>> listItems = Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores.ActualizadorBase.SplitList(items, 100).ToList();
                foreach (List<string> itemsIn in listItems)
                {
                    DenormalizerItemQueue item = new DenormalizerItemQueue(tupla.Item1, new HashSet<string>(itemsIn));
                    rabbitService.PublishMessage(item);
                }
            }


        }

        public static void ActualizarPertenenciaDocumentosProyectos()
        {
            ActualizadorDocument actualizadorDocument = new ActualizadorDocument(resourceApi);

            HashSet<string> filters = new HashSet<string>();
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    //Añadimos a documentos
                    int limit = 500;
                    //TODO eliminar from
                    String select = @"select distinct ?doc max(?project) as ?project  from <http://gnoss.com/curriculumvitae.owl>  from <http://gnoss.com/person.owl> from <http://gnoss.com/project.owl>  ";
                    String where = @$"where{{
                                    {filter}
                                    {{
                                        select distinct ?project as ?project ?doc
                                        Where{{
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project <http://vivoweb.org/ontology/core#relates> ?members.
                                            ?members <http://w3id.org/roh/roleOf> ?person.
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                                            OPTIONAL{{?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                            OPTIONAL{{?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                                            BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                            BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                            ?doc a <http://purl.org/ontology/bibo/Document>.
                                            ?doc <http://w3id.org/roh/isValidated> 'true'.
                                            ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                            ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                            ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                            FILTER(xsd:integer(?fechaPublicacion)>= ?fechaPersonaInitAux AND xsd:integer(?fechaPublicacion)<= ?fechaPersonaEndAux)
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?doc a <http://purl.org/ontology/bibo/Document>.
                                        ?doc <http://w3id.org/roh/project> ?X.
                                    }}
                                }}order by desc(?doc) limit {limit}";
                    SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, "document");
                    actualizadorDocument.InsercionMultiple(resultado.results.bindings, "http://w3id.org/roh/project", "doc", "project");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }


        public static void ActualizarPertenenciaProyectosTemporal()
        {
            ActualizadorDocument ac = new ActualizadorDocument(resourceApi);
            HashSet<string> filters = new HashSet<string>();
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    //Añadimos a documentos
                    int limit = 500;
                    //TODO eliminar from
                    String select = @"select ?doc MAX(?project) as ?project  from <http://gnoss.com/curriculumvitae.owl>  from <http://gnoss.com/person.owl> from <http://gnoss.com/project.owl>  ";
                    String where = @$"where{{
                                    {filter}
                                    {{
                                        select distinct ?project ?doc
                                        Where{{
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project <http://vivoweb.org/ontology/core#relates> ?members.
                                            ?members <http://w3id.org/roh/roleOf> ?person.
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                                            OPTIONAL{{?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                            OPTIONAL{{?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                                            BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                            BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                            ?doc a <http://purl.org/ontology/bibo/Document>.
                                            ?doc <http://w3id.org/roh/isValidated> 'true'.
                                            ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                            ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                            ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                            FILTER(xsd:integer(?fechaPublicacion)>= ?fechaPersonaInitAux AND xsd:integer(?fechaPublicacion)<= ?fechaPersonaEndAux)
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?doc a <http://purl.org/ontology/bibo/Document>.
                                        ?doc <http://w3id.org/roh/project> ?projectX.
                                    }}
                                }}group by ?doc order by desc(?doc) limit {limit}";
                    SparqlObject resultado = resourceApi.VirtuosoQuery(select, where, "document");
                    ac.InsercionMultiple(resultado.results.bindings, "http://w3id.org/roh/project", "doc", "project");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }


            }
        }


    }
}
