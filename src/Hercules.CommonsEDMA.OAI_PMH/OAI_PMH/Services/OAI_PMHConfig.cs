using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Services
{
    public class OAI_PMHConfig
    {
        public IConfigurationRoot Configuration { get; set; }
        private string ConfigUrl { get; set; }

        public string GetConfigUrl()
        {
            if (string.IsNullOrEmpty(ConfigUrl))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                Configuration = builder.Build();
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("ConfigUrl"))
                {
                    ConfigUrl = environmentVariables["ConfigUrl"] as string;
                }
                else
                {
                    ConfigUrl = Configuration["ConfigUrl"];
                }
            }
            return ConfigUrl;
        }
    }
}