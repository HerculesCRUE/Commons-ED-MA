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
        private string claimMail { get; set; }
        private string claimGroups { get; set; }        
        private string groupAdminIndicadores { get; set; }
        private string groupAdmin { get; set; }
        private string groupGestorOtri { get; set; }

        private string samlEntityID { get; set; }
        private string samlAssertionConsumerService { get; set; }
        private string samlContactPersonMail { get; set; }
        private string samlOrganizationName { get; set; }
        private string samlOrganizationDisplayName { get; set; }
        private string samlOrganizationURL { get; set; }


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

        public string GetClaimMail()
        {
            if (string.IsNullOrEmpty(claimMail))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("claimMail"))
                {
                    connectionString = environmentVariables["claimMail"] as string;
                }
                else
                {
                    connectionString = _configuration["claimMail"];
                }

                claimMail = connectionString;
            }
            return claimMail;
        }

        public string GetClaimGroups()
        {
            if (string.IsNullOrEmpty(claimGroups))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("claimGroups"))
                {
                    connectionString = environmentVariables["claimGroups"] as string;
                }
                else
                {
                    connectionString = _configuration["claimGroups"];
                }

                claimGroups = connectionString;
            }
            return claimGroups;
        }

        public string GetGroupAdminIndicadores()
        {
            if (string.IsNullOrEmpty(groupAdminIndicadores))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("groupAdminIndicadores"))
                {
                    connectionString = environmentVariables["groupAdminIndicadores"] as string;
                }
                else
                {
                    connectionString = _configuration["groupAdminIndicadores"];
                }

                groupAdminIndicadores = connectionString;
            }
            return groupAdminIndicadores;
        }

        public string GetGroupAdmin()
        {
            if (string.IsNullOrEmpty(groupAdmin))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("groupAdmin"))
                {
                    connectionString = environmentVariables["groupAdmin"] as string;
                }
                else
                {
                    connectionString = _configuration["groupAdmin"];
                }

                groupAdmin = connectionString;
            }
            return groupAdmin;
        }

        public string GetGroupGestorOtri()
        {
            if (string.IsNullOrEmpty(groupGestorOtri))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("groupGestorOtri"))
                {
                    connectionString = environmentVariables["groupGestorOtri"] as string;
                }
                else
                {
                    connectionString = _configuration["groupGestorOtri"];
                }

                groupGestorOtri = connectionString;
            }
            return groupGestorOtri;
        }


        public string GetSamlEntityID()
        {
            if (string.IsNullOrEmpty(samlEntityID))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("samlEntityID"))
                {
                    connectionString = environmentVariables["samlEntityID"] as string;
                }
                else
                {
                    connectionString = _configuration["samlEntityID"];
                }
                samlEntityID = connectionString;
            }
            return samlEntityID;
        }

        public string GetSamlAssertionConsumerService()
        {
            if (string.IsNullOrEmpty(samlAssertionConsumerService))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("samlAssertionConsumerService"))
                {
                    connectionString = environmentVariables["samlAssertionConsumerService"] as string;
                }
                else
                {
                    connectionString = _configuration["samlAssertionConsumerService"];
                }

                samlAssertionConsumerService = connectionString;
            }
            return samlAssertionConsumerService;
        }

        public string GetSamlContactPersonMail()
        {
            if (string.IsNullOrEmpty(samlContactPersonMail))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("samlContactPersonMail"))
                {
                    connectionString = environmentVariables["samlContactPersonMail"] as string;
                }
                else
                {
                    connectionString = _configuration["AAsamlContactPersonMailAA"];
                }

                samlContactPersonMail = connectionString;
            }
            return samlContactPersonMail;
        }

        public string GetSamlOrganizationName()
        {
            if (string.IsNullOrEmpty(samlOrganizationName))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("samlOrganizationName"))
                {
                    connectionString = environmentVariables["samlOrganizationName"] as string;
                }
                else
                {
                    connectionString = _configuration["samlOrganizationName"];
                }

                samlOrganizationName = connectionString;
            }
            return samlOrganizationName;
        }

        public string GetSamlOrganizationDisplayName()
        {
            if (string.IsNullOrEmpty(samlOrganizationDisplayName))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("samlOrganizationDisplayName"))
                {
                    connectionString = environmentVariables["samlOrganizationDisplayName"] as string;
                }
                else
                {
                    connectionString = _configuration["samlOrganizationDisplayName"];
                }

                samlOrganizationDisplayName = connectionString;
            }
            return samlOrganizationDisplayName;
        }

        public string GetSamlOrganizationURL()
        {
            if (string.IsNullOrEmpty(samlOrganizationURL))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("samlOrganizationURL"))
                {
                    connectionString = environmentVariables["samlOrganizationURL"] as string;
                }
                else
                {
                    connectionString = _configuration["samlOrganizationURL"];
                }
                samlOrganizationURL = connectionString;
            }
            return samlOrganizationURL;
        }

    }
}
