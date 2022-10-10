using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gnoss.Web.Login.Open
{
    public class ConfigServiceLogin
    {
        private string urlServiceInDomain { get; set; }

        private IConfiguration _configuration { get; set; }

        public ConfigServiceLogin(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetUrlServiceInDomain()
        {
            if (string.IsNullOrEmpty(urlServiceInDomain))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlServiceInDomain"))
                {
                    connectionString = environmentVariables["urlServiceInDomain"] as string;
                }
                else
                {
                    connectionString = _configuration["urlServiceInDomain"];
                }

                urlServiceInDomain = connectionString;
            }
            return urlServiceInDomain;
        }
    }
}
