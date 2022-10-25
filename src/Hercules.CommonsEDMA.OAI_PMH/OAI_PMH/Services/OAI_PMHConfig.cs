using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

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