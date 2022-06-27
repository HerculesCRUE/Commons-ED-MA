using Gnoss.ApiWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.Commons.Load
{
    class Program
    {
        private static ResourceApi mResourceApi = new ResourceApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config\configOAuth\OAuthV3.config");
        private static CommunityApi mCommunityApi = new CommunityApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config\configOAuth\OAuthV3.config");
        static void Main(string[] args)
        {
            CargaNormaCVN.mResourceApi = mResourceApi;
            CargaXMLMurcia.mResourceApi = mResourceApi;
            CargaXMLMurcia.mIdComunidad = mCommunityApi.GetCommunityId();
            //CargaNormaCVN.CargarEntidadesSecundarias();
            CargaXMLMurcia.CargarEntidadesPrincipales();
        }
    }
}
