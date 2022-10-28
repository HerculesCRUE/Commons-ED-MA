using Es.Riam.Gnoss.AD.EntityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gnoss.Web.Login.Open.Controllers
{
    public static class UtilCookiesHercules
    {
        public static Dictionary<string, string> FromLegacyCookieString(string legacyCookie, EntityContext pEntityContext)
        {
            string cookie = Desencriptar(legacyCookie, pEntityContext);
            if (string.IsNullOrEmpty(cookie))
            {
                return new Dictionary<string, string>();
            }
            return cookie.Split('&').Select(s => s.Split('=')).ToDictionary(kvp => kvp[0], kvp => kvp[1]);
        }

        public static string ToLegacyCookieString(IDictionary<string, string> dict, EntityContext pEntityContext)
        {
            return Encriptar(string.Join("&", dict.Select(kvp => string.Join("=", kvp.Key, kvp.Value))), pEntityContext);
        }

        public static string Desencriptar(string cookie, EntityContext pEntityContext)
        {
            if (string.IsNullOrEmpty(cookie))
            {
                return cookie;
            }
            cookie = cookie.Replace("+", "%2b");
            byte[] buf;
            try
            {
                buf = DesencriptarCookie(Convert.FromBase64String(HttpUtility.UrlDecode(cookie)), pEntityContext);
            }
            catch (InvalidCypherTextException ex)
            {
                throw new InvalidCypherTextException("Imposible desencriptar el texto", ex.InnerException);
            }
            catch (FormatException)
            {
                return cookie;
            }
            if (buf == null || buf.Length == 0)
            {
                throw new InvalidCypherTextException("Imposible desencriptar el texto");
            }
            return Encoding.UTF8.GetString(buf, 0, buf.Length);
        }

        public static string Encriptar(string pText, EntityContext pEntityContext)
        {
            if (string.IsNullOrEmpty(pText))
            {
                return pText;
            }
            byte[] buf = Encoding.UTF8.GetBytes(pText);
            byte[] bufEncr = EncriptarCookie(buf, pEntityContext);
            return HttpUtility.UrlEncode(Convert.ToBase64String(bufEncr, 0, bufEncr.Length));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFichero"></param>
        /// <returns></returns>
        public static byte[] DesencriptarCookie(byte[] pFichero, EntityContext entityContext)
        {
            // Generate decryptor from the existing key bytes and initialization 
            // vector. Key size will be defined based on the number of the key 
            // bytes.

            Aes encriptador = Aes.Create();
            encriptador.KeySize = 256;

            var rnd = RandomNumberGenerator.Create();
            byte[] key = new byte[32];
            byte[] iv = new byte[16];

            string claveEncriptado = entityContext.ParametroAplicacion.Where(item => item.Parametro.Equals("ClaveEncriptado")).Select(item => item.Valor).FirstOrDefault();
            if (string.IsNullOrEmpty(claveEncriptado))
            {
                rnd.GetBytes(key);
                ParametroAplicacion paramAplicacion = new ParametroAplicacion("ClaveEncriptado", Convert.ToBase64String(key));
                entityContext.ParametroAplicacion.Add(paramAplicacion);
            }
            else
            {
                key = Convert.FromBase64String(claveEncriptado);
            }

            string claveVectorEncriptado = entityContext.ParametroAplicacion.Where(item => item.Parametro.Equals("ClaveVectorEncriptado")).Select(item => item.Valor).FirstOrDefault();
            if (string.IsNullOrEmpty(claveVectorEncriptado))
            {
                rnd.GetBytes(iv);
                ParametroAplicacion paramAplicacion = new ParametroAplicacion("ClaveVectorEncriptado", Convert.ToBase64String(iv));
                entityContext.ParametroAplicacion.Add(paramAplicacion);
            }
            else
            {
                iv = Convert.FromBase64String(claveVectorEncriptado);
            }
            entityContext.SaveChanges();

            //Establecer la clave secreta para el algoritmo DES. 
            encriptador.Key = key;
            //Establecer el vector de inicialización. 
            encriptador.IV = iv;

            ICryptoTransform decryptor = encriptador.CreateDecryptor();

            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream(pFichero);

            // Define cryptographic stream (always use Read mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] buff = null;

            try
            {
                //Si el fichero no esta encriptado da error, devolvemos el original
                MemoryStream memory = new MemoryStream();
                cryptoStream.CopyTo(memory);
                buff = memory.ToArray();
                // Close both streams.
                cryptoStream.Close();
            }
            catch (Exception)
            {
                buff = pFichero;
                throw new InvalidCypherTextException();
            }
            finally
            {
                memoryStream.Close();
            }

            return buff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pFichero"></param>
        /// <returns></returns>
        public static byte[] EncriptarCookie(byte[] pFichero, EntityContext pEntityContext)
        {
            // Define memory stream which will be used to hold encrypted data.
            MemoryStream memoryStream = new MemoryStream();

            var rnd = RandomNumberGenerator.Create();
            byte[] key = new byte[32];
            byte[] iv = new byte[16];

            string claveEncriptado = pEntityContext.ParametroAplicacion.Where(item => item.Parametro.Equals("ClaveEncriptado")).Select(item => item.Valor).FirstOrDefault();
            if (string.IsNullOrEmpty(claveEncriptado))
            {
                rnd.GetBytes(key);
                ParametroAplicacion paramAplicacion = new ParametroAplicacion("ClaveEncriptado", Convert.ToBase64String(key));
                pEntityContext.ParametroAplicacion.Add(paramAplicacion);
            }
            else
            {
                key = Convert.FromBase64String(claveEncriptado);
            }

            string claveVectorEncriptado = pEntityContext.ParametroAplicacion.Where(item => item.Parametro.Equals("ClaveVectorEncriptado")).Select(item => item.Valor).FirstOrDefault();
            if (string.IsNullOrEmpty(claveVectorEncriptado))
            {
                rnd.GetBytes(iv);
                ParametroAplicacion paramAplicacion = new ParametroAplicacion("ClaveVectorEncriptado", Convert.ToBase64String(iv));
                pEntityContext.ParametroAplicacion.Add(paramAplicacion);
            }
            else
            {
                iv = Convert.FromBase64String(claveVectorEncriptado);
            }
            pEntityContext.SaveChanges();


            Aes encriptador = Aes.Create();
            encriptador.KeySize = 256;


            //Establecer la clave secreta para el algoritmo DES. 
            encriptador.Key = key;
            //Establecer el vector de inicialización. 
            encriptador.IV = iv;

            ICryptoTransform encrypt = encriptador.CreateEncryptor();

            // Define cryptographic stream (always use Write mode for encryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encrypt, CryptoStreamMode.Write);
            // Start encrypting.
            cryptoStream.Write(pFichero, 0, pFichero.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memory stream into a byte array.
            byte[] buff = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            return buff;
        }

        /// <summary>
        /// Representa errores que ocurren cuando un texto no puede ser desencriptado o ha sido manipulado
        /// </summary>
        public class InvalidCypherTextException : Exception
        {

            #region Constructores

            /// <summary>
            /// Constructor
            /// </summary>
            public InvalidCypherTextException()
                : base()
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public InvalidCypherTextException(string pMessage)
                : base(pMessage)
            {
            }

            /// <summary>
            /// Constructor
            /// </summary>
            public InvalidCypherTextException(string pMessage, Exception pInnerException)
                : base(pMessage, pInnerException)
            {
            }

            #endregion

        }
    }
}
