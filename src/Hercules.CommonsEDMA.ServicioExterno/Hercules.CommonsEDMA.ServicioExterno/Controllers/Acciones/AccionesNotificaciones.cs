using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using System;
using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesNotificaciones : GnossGetMainResourceApiDataBase
    {
        public void readNotification(string pNotificationID)
        {
            if (!isNotificationRead(pNotificationID)) {
                Guid guid = resourceApi.GetShortGuid(pNotificationID);
                Dictionary<Guid, List<TriplesToInclude>> dicInsercion = new Dictionary<Guid, List<TriplesToInclude>>();
                List<TriplesToInclude> listaTriplesInsercion = new List<TriplesToInclude>();
                TriplesToInclude triple = new TriplesToInclude();
                triple.Predicate= "http://w3id.org/roh/read";
                triple.NewValue = "true";
                listaTriplesInsercion.Add(triple);
                dicInsercion.Add(guid, listaTriplesInsercion);
                resourceApi.InsertPropertiesLoadedResources(dicInsercion);
            }

        }
        public bool isNotificationRead(string pNotificationID) {


            string select = "select ?read ";

            string where = @$"where {{

                <{pNotificationID}> <http://w3id.org/roh/read> ?read.
            }}";

            SparqlObject sparqlObject = resourceApi.VirtuosoQuery(select, where, "notification");
            return (sparqlObject != null && sparqlObject.results != null && sparqlObject.results.bindings != null && sparqlObject.results.bindings.Count > 0);
           
        }
    }
}
