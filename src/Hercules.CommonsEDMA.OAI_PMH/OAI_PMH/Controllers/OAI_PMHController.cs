using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAI_PMH.Models.OAIPMH;
using OAI_PMH.Services;
using OaiPmhNet;
using OaiPmhNet.Models;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace OAI_PMH.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OAI_PMHController : ControllerBase
    {
        private IOaiConfiguration _configOAI;
        readonly ConfigService _Config;

        public OAI_PMHController(ConfigService OAI_PMHConfig)
        {
            _Config = OAI_PMHConfig;
            _configOAI = OaiConfiguration.Instance;
            _configOAI.SupportSets = true;
            _configOAI.RepositoryName = "OAI_PMH";
            _configOAI.Granularity = "yyyy-MM-ddTHH:mm:ssZ";
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public FileResult Get(OaiVerb verb, string identifier = "", string metadataPrefix = "", string from = "", string until = "", string set = "", string resumptionToken = "")
        {
            try
            {
                _configOAI.BaseUrl = () =>
                {
                    Uri baseUri = new(_Config.GetConfigUrl());
                    return baseUri.AbsoluteUri;
                };
                identifier = identifier.Replace(" ", "+");

                if (string.IsNullOrEmpty(until) && !string.IsNullOrEmpty(from))
                {
                    until = DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ssZ");
                }

                ArgumentContainer arguments = new(verb.ToString(), metadataPrefix, resumptionToken, identifier, from, until, set);
                MetadataFormatRepository metadataFormatRepository = new();
                RecordRepository recordRepository = new(_Config);
                SetRepository setRepository = new(_configOAI);
                DataProvider provider = new(_configOAI, metadataFormatRepository, recordRepository, setRepository);
                XDocument document = provider.ToXDocument(DateTime.UtcNow, arguments);

                var memoryStream = new MemoryStream();
                var xmlWriter = XmlWriter.Create(memoryStream);

                document.WriteTo(xmlWriter);
                xmlWriter.Flush();
                byte[] array = memoryStream.ToArray();
                return File(array, "application/xml");
            }catch(Exception ex)
            {
                throw;
            }
        }
    }
}
