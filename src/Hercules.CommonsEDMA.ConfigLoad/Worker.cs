using Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid;
using Hercules.CommonsEDMA.ConfigLoad.Models.DB.Acid.EntityContext;
using Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth;
using Hercules.CommonsEDMA.ConfigLoad.Models.DB.OAuth.EntityContext;
using Hercules.CommonsEDMA.ConfigLoad.Models.OAuth;
using Hercules.CommonsEDMA.ConfigLoad.Models.Proyecto;
using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ConfigLoad
{
    public class Worker : BackgroundService
    {
        private static ConfigService configService;

        private static EntityContextAcid entityContextAcid;
        private static EntityContextOauth entityContextOauth;

        /// <summary>
        /// Contructor.
        /// </summary>
        public Worker() { }

        /// <summary>
        /// Tarea asincrona.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                configService = new ConfigService();

                CargarConfiguracionBBDD();

                //Cambiamos el pass del usuario
                Models.DB.Acid.Usuario filaUsuario = entityContextAcid.Usuario.FirstOrDefault(x => x.Login == configService.ObtenerLoginAdmin());
                filaUsuario.Password = CalcularHash(configService.ObtenerPassAdmin(), true);
                entityContextAcid.SaveChanges();

                //Creamos el proyecto
                Proyecto proyecto = ControladorProyecto.CrearNuevoProyecto(entityContextAcid, configService);
                if (proyecto == null)
                {
                    Console.WriteLine("Se ha producido un error no se puede continuar");
                }

                //Creamos el OAuth
                ControladorOAuth.CrearNuevoOAuth(proyecto, entityContextAcid, entityContextOauth, configService);

                //Subimos las configuraciones
                SubirConfiguraciones();
                Console.WriteLine("Ha finalizado la carga de configuraciones");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error no controlado: " + ex.Message);
                Console.WriteLine("Pila de llamadas: " + ex.StackTrace);
            }
        }

        private static void CargarConfiguracionBBDD()
        {
            string bdType = configService.ObtenerTipoBD();
            entityContextOauth = null;
            if (bdType.Equals("0"))
            {
                DbContextOptionsBuilder<EntityContextOauth> optionsBuilderOauth = new DbContextOptionsBuilder<EntityContextOauth>();
                optionsBuilderOauth.UseSqlServer(configService.ObtenerOauthConnectionString());
                entityContextOauth = new EntityContextOauth(optionsBuilderOauth.Options, configService);

                DbContextOptionsBuilder<EntityContextAcid> optionsBuilderAcid = new DbContextOptionsBuilder<EntityContextAcid>();
                optionsBuilderAcid.UseSqlServer(configService.ObtenerAcidConnectionString());
                entityContextAcid = new EntityContextAcid(optionsBuilderAcid.Options, configService);
            }
            else if (bdType.Equals("1"))
            {
                DbContextOptionsBuilder<EntityContextOauth> optionsBuilderOauth = new DbContextOptionsBuilder<EntityContextOauth>();
                optionsBuilderOauth.UseOracle(configService.ObtenerOauthConnectionString());
                entityContextOauth = new EntityContextOauthOracle(optionsBuilderOauth.Options, configService);

                DbContextOptionsBuilder<EntityContextAcid> optionsBuilderAcid = new DbContextOptionsBuilder<EntityContextAcid>();
                optionsBuilderAcid.UseOracle(configService.ObtenerAcidConnectionString());
                entityContextAcid = new EntityContextAcid(optionsBuilderAcid.Options, configService);
            }
            else if (bdType.Equals("2"))
            {
                DbContextOptionsBuilder<EntityContextOauth> optionsBuilderOauth = new DbContextOptionsBuilder<EntityContextOauth>();
                optionsBuilderOauth.UseNpgsql(configService.ObtenerOauthConnectionString());
                entityContextOauth = new EntityContextOauthOracle(optionsBuilderOauth.Options, configService);

                DbContextOptionsBuilder<EntityContextAcid> optionsBuilderAcid = new DbContextOptionsBuilder<EntityContextAcid>();
                optionsBuilderAcid.UseNpgsql(configService.ObtenerAcidConnectionString());
                entityContextAcid = new EntityContextAcid(optionsBuilderAcid.Options, configService);
            }
        }

        /// <summary>
		/// Calcula la encriptación para una password
		/// </summary>
		/// <param name="password">Password</param>
		/// <param name="p256">Verdad si se desea utilizar el algoritmo de 256 bits</param>
		/// <returns>Password encriptada</returns>
		private static string CalcularHash(string password, bool p256)
        {
            int longitudSalt = 4;
            int longitudPasswordMax = 12;

            byte[] passwordBinaria;
            byte[] salt;
            byte[] passwordHashedYHash;

            // Recorto la password si supera el máximo
            if (password.Length > longitudPasswordMax)
                password = password.Substring(0, longitudPasswordMax);

            passwordBinaria = System.Text.Encoding.Unicode.GetBytes(password);
            salt = CrearSalt(longitudSalt);

            passwordHashedYHash = CalcularHash(passwordBinaria, salt, p256);

            // Convertimos a texto y devolvemos
            return Convert.ToBase64String(passwordHashedYHash);
        }
        /// <summary>
		/// Génera código aleatorio para la encriptación
		/// </summary>
		/// <param name="size">Tamaño</param>
		/// <returns></returns>
		private static byte[] CrearSalt(int size)
        {
            // Generamos un código aleatorio para la encriptación.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];

            rng.GetBytes(buff);
            return buff;
        }

        /// <summary>
		/// Calcula el hash para una password
		/// </summary>
		/// <param name="password">Password para calcular</param>
		/// <param name="salt">Valor</param>
		/// <param name="p256">Verdad si se quiere calcular el HASH con el algoritmo de encriptación de 256 bits</param>
		/// <returns>Hash calculado para password</returns>
		private static byte[] CalcularHash(byte[] password, byte[] salt, bool p256)
        {
            byte[] passwordSalted;
            byte[] passwordHashed;
            byte[] passwordHashedYHash;

            // Concatenamos password y valor salt
            passwordSalted = new byte[password.Length + salt.Length];
            password.CopyTo(passwordSalted, 0);
            salt.CopyTo(passwordSalted, password.Length);

            // Generamos el hash de la unión

            if (p256)
            {
                //Usando SHA256
                SHA256 mySHA256 = SHA256Managed.Create();
                passwordHashed = mySHA256.ComputeHash(passwordSalted);
            }
            else
            {
                HashAlgorithm ha = new SHA1CryptoServiceProvider();
                passwordHashed = ha.ComputeHash(passwordSalted);
            }

            // Añadimos el salt al hash en texto claro
            passwordHashedYHash = new byte[passwordHashed.Length + salt.Length];
            passwordHashed.CopyTo(passwordHashedYHash, 0);
            salt.CopyTo(passwordHashedYHash, passwordHashed.Length);

            return passwordHashedYHash;
        }


        private static void SubirConfiguraciones()
        {
            List<Tuple<string, string, string>> listaPasos = new();
            listaPasos.Add(Tuple.Create("1- Subimos ontologías", "Ontologias.zip", "Ontologias"));
            listaPasos.Add(Tuple.Create("2- Subimos Objetos de conocimiento", "ObjetosConocimiento.zip", "ObjetosConocimiento"));
            listaPasos.Add(Tuple.Create("3- Subimos Facetas", "Facetas.zip", "Facetas"));
            listaPasos.Add(Tuple.Create("4- Subimos Componentes del CMS", "ComponentesCMS.zip", "ComponentesCMS"));
            listaPasos.Add(Tuple.Create("5- Subimos Pestañas", "Pestanyas.zip", "Pestanyas"));
            listaPasos.Add(Tuple.Create("6- Subimos Paginas del CMS", "PaginasCMS.zip", "PaginasCMS"));
            listaPasos.Add(Tuple.Create("7- Subimos Utilidades", "Utilidades.zip", "Utilidades"));
            listaPasos.Add(Tuple.Create("8- Subimos Opciones avanzadas", "OpcionesAvanzadas.zip", "OpcionesAvanzadas"));
            listaPasos.Add(Tuple.Create("9- Subimos Estilos", "Estilos.zip", "Estilos"));
            listaPasos.Add(Tuple.Create("10- Subimos Parámetros de búsqueda personalizados", "SearchPersonalizado.zip", "SearchPersonalizado"));
            listaPasos.Add(Tuple.Create("11- Subimos Vistas", "Vistas.zip", "Vistas"));

            Console.WriteLine("Subimos configuraciones");
            string rutaBase = $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Files{Path.DirectorySeparatorChar}";

            foreach (Tuple<string, string, string> step in listaPasos)
            {
                Console.WriteLine(step.Item1);
                Despliegue(rutaBase + step.Item2, step.Item3, configService.ObtenerLoginAdmin(), configService.ObtenerPassAdmin(), configService.ObtenerUrlDominioServicios());
            }
        }

        private static void Despliegue(string pRutaFichero, string pMetodo, string pLoginAdmin, string pPassAdmin, string pUrlDomainServices)
        {
            string nombreProy = configService.ObtenerNombreCortoComunidad();
            string sWebAddress = $"{configService.ObtenerUrlAPIDespliegues()}Upload?tipoPeticion={pMetodo}&usuario={pLoginAdmin}&password={pPassAdmin}&nombreProy={nombreProy}";

            HttpContent contentData;

            if (pMetodo == "Vistas")
            {
                string rutaCarpeta = pRutaFichero.Substring(0, pRutaFichero.Length - 4);
                if (Directory.Exists(rutaCarpeta))
                {
                    Directory.Delete(rutaCarpeta, true);
                }
                System.IO.Compression.ZipFile.ExtractToDirectory(pRutaFichero, rutaCarpeta);


                string contenidoOriginal = File.ReadAllText(rutaCarpeta + Path.DirectorySeparatorChar + "TextosPersonalizadosPersonalizacion.json");
                string contenidoModificado = contenidoOriginal;
                contenidoModificado = contenidoModificado.Replace("[%%%_URL_CONTENT_%%%]", pUrlDomainServices);
                if (contenidoOriginal != contenidoModificado)
                {
                    System.IO.File.WriteAllText(rutaCarpeta + Path.DirectorySeparatorChar + "TextosPersonalizadosPersonalizacion.json", contenidoModificado);
                }

                if (File.Exists(pRutaFichero))
                {
                    File.Delete(pRutaFichero);
                }
                System.IO.Compression.ZipFile.CreateFromDirectory(rutaCarpeta, pRutaFichero);
                if (Directory.Exists(rutaCarpeta))
                {
                    System.IO.Directory.Delete(rutaCarpeta, true);
                }
            }

            byte[] data = File.ReadAllBytes(pRutaFichero);
            ByteArrayContent bytes = new(data);
            bytes.Headers.Add("Content-Type", "application/zip");
            contentData = new MultipartFormDataContent();
            ((MultipartFormDataContent)contentData).Add(bytes, "ficheroZip", ".zip");

            string result;
            HttpResponseMessage response;

            try
            {
                HttpClient client = new();
                response = client.PostAsync($"{sWebAddress}", contentData).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"{pMetodo} {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{pMetodo} Error:{ex.Message}");
            }
        }
    }
}
