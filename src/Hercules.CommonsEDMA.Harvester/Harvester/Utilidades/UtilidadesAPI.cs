using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gnoss.ApiWrapper;
using System.Text;
using System.Web;

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
    }
}
