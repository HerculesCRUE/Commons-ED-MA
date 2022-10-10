using Es.Riam.AbstractsOpen;
using Es.Riam.Gnoss.AD.EntityModel;
using Es.Riam.Gnoss.AD.EntityModelBASE;
using Es.Riam.Gnoss.AD.Virtuoso;
using Es.Riam.Gnoss.CL;
using Es.Riam.Gnoss.Util.Configuracion;
using Es.Riam.Gnoss.Util.General;
using Es.Riam.Gnoss.Util.Seguridad;
using Es.Riam.Gnoss.UtilServiciosWeb;
using Es.Riam.OpenReplication;
using Es.Riam.Util;
using Gnoss.Web.Login;
using Gnoss.Web.Login.Open;
using ITfoxtec.Identity.Saml2;
using ITfoxtec.Identity.Saml2.MvcCore.Configuration;
using ITfoxtec.Identity.Saml2.Schemas.Metadata;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Threading.Tasks;

namespace Gnoss.Web.Login
{
    public class Startup
    {
        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            Configuration = configuration;
            mEnvironment = environment;
        }

        public IConfiguration Configuration { get; }
        public Microsoft.AspNetCore.Hosting.IHostingEnvironment mEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            services.AddMvc();
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(UtilTelemetry));
            services.AddScoped(typeof(Usuario));
            services.AddScoped(typeof(UtilPeticion));
            services.AddScoped(typeof(Conexion));
            services.AddScoped(typeof(UtilGeneral));
            services.AddScoped(typeof(LoggingService));
            services.AddScoped(typeof(RedisCacheWrapper));
            services.AddScoped(typeof(Configuracion));
            services.AddScoped(typeof(GnossCache));
            services.AddScoped(typeof(VirtuosoAD));
            services.AddScoped(typeof(UtilServicios));
            services.AddScoped<IServicesUtilVirtuosoAndReplication, ServicesVirtuosoAndBidirectionalReplicationOpen>();

            string IdPMetadata = "";
            string Issuer = "";
            string SignatureAlgorithm = "";
            string CertificateValidationMode = "";
            string RevocationMode = "";
            if (environmentVariables.Contains("Saml2_IdPMetadata"))
            {
                IdPMetadata = environmentVariables["Saml2_IdPMetadata"] as string;
            }
            else
            {
                IdPMetadata = Configuration["Saml2:IdPMetadata"];
            }
            if (environmentVariables.Contains("Saml2_Issuer"))
            {
                Issuer = environmentVariables["Saml2_Issuer"] as string;
            }
            else
            {
                Issuer = Configuration["Saml2:Issuer"];
            }
            if (environmentVariables.Contains("Saml2_SignatureAlgorithm"))
            {
                SignatureAlgorithm = environmentVariables["Saml2_SignatureAlgorithm"] as string;
            }
            else
            {
                SignatureAlgorithm = Configuration["Saml2:SignatureAlgorithm"];
            }
            if (environmentVariables.Contains("Saml2_CertificateValidationMode"))
            {
                CertificateValidationMode = environmentVariables["Saml2_CertificateValidationMode"] as string;
            }
            else
            {
                CertificateValidationMode = Configuration["Saml2:CertificateValidationMode"];
            }
            if (environmentVariables.Contains("Saml2_RevocationMode"))
            {
                RevocationMode = environmentVariables["Saml2_RevocationMode"] as string;
            }
            else
            {
                RevocationMode = Configuration["Saml2:RevocationMode"];
            }

            bool useSAML = false;
            if (!string.IsNullOrEmpty(IdPMetadata) && !string.IsNullOrEmpty(Issuer) && !string.IsNullOrEmpty(SignatureAlgorithm) && !string.IsNullOrEmpty(CertificateValidationMode) && !string.IsNullOrEmpty(RevocationMode))
            {
                useSAML = true;
            }

            if (useSAML)
            {
                services.Configure<Saml2Configuration>(saml2Configuration =>
                {
                    saml2Configuration.Issuer = Issuer;
                    saml2Configuration.SignatureAlgorithm = SignatureAlgorithm;
                    saml2Configuration.CertificateValidationMode = (X509CertificateValidationMode)Enum.Parse(typeof(X509CertificateValidationMode), CertificateValidationMode, true);
                    saml2Configuration.RevocationMode = (X509RevocationMode)Enum.Parse(typeof(X509RevocationMode), RevocationMode, true);
                    saml2Configuration.AllowedAudienceUris.Add(Issuer);

                    var entityDescriptor = new EntityDescriptor();
                    entityDescriptor.ReadIdPSsoDescriptorFromUrl(new Uri(IdPMetadata));
                    if (entityDescriptor.IdPSsoDescriptor != null)
                    {
                        saml2Configuration.SingleSignOnDestination = entityDescriptor.IdPSsoDescriptor.SingleSignOnServices.First(x => x.Binding == new Uri("urn:oasis:names:tc:SAML:2.0:bindings:HTTP-Redirect")).Location;
                        saml2Configuration.SingleLogoutDestination = entityDescriptor.IdPSsoDescriptor.SingleLogoutServices.First().Location;
                        saml2Configuration.SignatureValidationCertificates.AddRange(entityDescriptor.IdPSsoDescriptor.SigningCertificates);
                    }
                    else
                    {
                        throw new Exception("IdPSsoDescriptor not loaded from metadata.");
                    }
                });

            }
            services.AddCors(options =>
            {
                options.AddPolicy(name: "_myAllowSpecificOrigins",
                builder =>
                {
                    builder.SetIsOriginAllowed(ComprobarDominioEnBD);
                    //builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });
            string bdType = "";
            if (environmentVariables.Contains("connectionType"))
            {
                bdType = environmentVariables["connectionType"] as string;
            }
            else
            {
                bdType = Configuration.GetConnectionString("connectionType");
            }
            if (bdType.Equals("2"))
            {
                services.AddScoped(typeof(DbContextOptions<EntityContext>));
                services.AddScoped(typeof(DbContextOptions<EntityContextBASE>));
            }
            services.AddSingleton(typeof(ConfigService));
            services.AddSingleton(typeof(ConfigServiceLogin));

            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            Conexion.ServicioWeb = true;
            string acid = "";
            if (environmentVariables.Contains("acid"))
            {
                acid = environmentVariables["acid"] as string;
            }
            else
            {
                acid = Configuration.GetConnectionString("acid");
            }
            string baseConnection = "";
            if (environmentVariables.Contains("base"))
            {
                baseConnection = environmentVariables["base"] as string;
            }
            else
            {
                baseConnection = Configuration.GetConnectionString("base");
            }
            if (bdType.Equals("0"))
            {
                services.AddDbContext<EntityContext>(options =>
                        options.UseSqlServer(acid)
                        );
                services.AddDbContext<EntityContextBASE>(options =>
                        options.UseSqlServer(baseConnection)

                        );
            }
            else if (bdType.Equals("2"))
            {
                services.AddDbContext<EntityContext, EntityContextPostgres>(opt =>
                {
                    var builder = new NpgsqlDbContextOptionsBuilder(opt);
                    builder.SetPostgresVersion(new Version(9, 6));
                    opt.UseNpgsql(acid);

                });
                services.AddDbContext<EntityContextBASE, EntityContextBASEPostgres>(opt =>
                {
                    var builder = new NpgsqlDbContextOptionsBuilder(opt);
                    builder.SetPostgresVersion(new Version(9, 6));
                    opt.UseNpgsql(baseConnection);

                });
            }

            var sp = services.BuildServiceProvider();

            // Resolve the services from the service provider
            var configService = sp.GetService<ConfigService>();


            string configLogStash = configService.ObtenerLogStashConnection();
            if (!string.IsNullOrEmpty(configLogStash))
            {
                LoggingService.InicializarLogstash(configLogStash);
            }
            var entity = sp.GetService<EntityContext>();
            LoggingService.RUTA_DIRECTORIO_ERROR = Path.Combine(mEnvironment.ContentRootPath, "logs");

            EstablecerDominioCache(entity);

            CargarIdiomasPlataforma(configService);

            ConfigurarApplicationInsights(configService);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gnoss.Web.Login", Version = "v1" });
            });
            if (useSAML)
            {
                services.AddSaml2();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gnoss.Web.Login v1"));
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseSaml2();
            app.UseCors();
            app.UseAuthorization();
            app.UseGnossMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private bool ComprobarDominioEnBD(string dominio)
        {
            return true;
        }

        /// <summary>
        /// Establece el dominio de la cache.
        /// </summary>
        private void EstablecerDominioCache(EntityContext entity)
        {
            string dominio = entity.ParametroAplicacion.Where(parametroApp => parametroApp.Parametro.Equals("UrlIntragnoss")).FirstOrDefault().Valor;

            dominio = dominio.Replace("http://", "").Replace("https://", "").Replace("www.", "");

            if (dominio[dominio.Length - 1] == '/')
            {
                dominio = dominio.Substring(0, dominio.Length - 1);
            }

            BaseCL.DominioEstatico = dominio;
        }

        private void CargarIdiomasPlataforma(ConfigService configService)
        {

            configService.ObtenerListaIdiomas().FirstOrDefault();
        }

        private void ConfigurarApplicationInsights(ConfigService configService)
        {
            string valor = configService.ObtenerImplementationKeyLogin();

            if (!string.IsNullOrEmpty(valor))
            {
                Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = valor.ToLower();
            }

            if (UtilTelemetry.EstaConfiguradaTelemetria)
            {
                //Configuración de los logs

                string ubicacionLogs = configService.ObtenerUbicacionLogsLogin();

                int valorInt = 0;
                if (int.TryParse(ubicacionLogs, out valorInt))
                {
                    if (Enum.IsDefined(typeof(UtilTelemetry.UbicacionLogsYTrazas), valorInt))
                    {
                        LoggingService.UBICACIONLOGS = (UtilTelemetry.UbicacionLogsYTrazas)valorInt;
                    }
                }


                //Configuración de las trazas

                string ubicacionTrazas = configService.ObtenerUbicacionTrazasLogin();

                int valorInt2 = 0;
                if (int.TryParse(ubicacionTrazas, out valorInt2))
                {
                    if (Enum.IsDefined(typeof(UtilTelemetry.UbicacionLogsYTrazas), valorInt2))
                    {
                        LoggingService.UBICACIONTRAZA = (UtilTelemetry.UbicacionLogsYTrazas)valorInt2;
                    }
                }

            }

        }
    }
}
