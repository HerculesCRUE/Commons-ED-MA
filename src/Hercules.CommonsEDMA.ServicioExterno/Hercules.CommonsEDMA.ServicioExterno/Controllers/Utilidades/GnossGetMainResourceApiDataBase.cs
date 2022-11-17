using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Gnoss.ApiWrapper;
using Newtonsoft.Json;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades
{
    public class GnossGetMainResourceApiDataBase
    {
        #region --- Constantes   
        protected static string RUTA_OAUTH = $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
        protected static ResourceApi mResourceAPI = null;
        protected static CommunityApi mCommunityAPI = null;
        protected static Guid? mIDComunidad = null;
        protected static string RUTA_PREFIJOS = $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Models{Path.DirectorySeparatorChar}JSON{Path.DirectorySeparatorChar}prefijos.json";
        protected static string mPrefijos = string.Join(" ", JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(RUTA_PREFIJOS)));
        #endregion

        protected static ResourceApi resourceApi
        {
            get
            {
                while (mResourceAPI == null)
                {
                    try
                    {
                        mResourceAPI = new ResourceApi(RUTA_OAUTH);
                        if (string.IsNullOrEmpty(mResourceAPI.GraphsUrl))
                        {
                            mResourceAPI = null;
                            Console.WriteLine("No se ha podido iniciar ResourceApi mResourceApi.GraphsUrl es nulo o vacío");
                            Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                            Thread.Sleep(10000);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("No se ha podido iniciar ResourceApi");
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mResourceAPI;
            }
        }

        protected static CommunityApi communityApi
        {
            get
            {
                while (mCommunityAPI == null)
                {
                    try
                    {
                        mCommunityAPI = new CommunityApi(RUTA_OAUTH);
                        mCommunityAPI.GetCommunityId();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("No se ha podido iniciar CommunityApi");
                        Console.WriteLine("Error: " + ex.Message);
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mCommunityAPI;
            }
        }

        protected static Guid idComunidad
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

    }
}
