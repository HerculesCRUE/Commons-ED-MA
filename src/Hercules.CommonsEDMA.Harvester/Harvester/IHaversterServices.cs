using Harvester.Models;
using Newtonsoft.Json;
using OAI_PMH.Models.SGI.Organization;
using OAI_PMH.Models.SGI.PersonalData;
using OAI_PMH.Models.SGI.Project;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Harvester
{
    public interface IHaversterServices
    {
        public List<IdentifierOAIPMH> ListIdentifiers(string from, ReadConfig pConfig, string until = null, string set = null);
        public List<ListRecordsOAIPMH> ListRecords(string from, ReadConfig pConfig, string until = null, string set = null);
        public string GetRecord(string id, ReadConfig pConfig, string file = null);

    }

    public class IHarvesterServices : IHaversterServices
    {
        public List<IdentifierOAIPMH> ListIdentifiers(string from, ReadConfig pConfig, string until = null, string set = null)
        {
            List<IdentifierOAIPMH> idList = null;
            string uri = $@"{pConfig.GetUrlOaiPmh()}?verb=ListIdentifiers&metadataPrefix=EDMA";
            if (from != null)
            {
                uri += $"&from={from}";
            }
            if (until != null)
            {
                uri += $"&until={until}";
            }
            if (set != null)
            {
                uri += $"&set={set}";
            }

            WebRequest wrGETURL = WebRequest.Create(uri);
            wrGETURL.Timeout = 3600000;
            Stream stream = wrGETURL.GetResponse().GetResponseStream();

            XDocument XMLresponse = XDocument.Load(stream);
            XNamespace nameSpace = XMLresponse.Root.GetDefaultNamespace();
            XElement idListElement = XMLresponse.Root.Element(nameSpace + "ListIdentifiers");

            if (idListElement != null)
            {
                idList = new List<IdentifierOAIPMH>();
                IEnumerable<XElement> headerList = idListElement.Descendants(nameSpace + "header");

                foreach (var header in headerList)
                {
                    header.Attribute(nameSpace + "status");
                    string identifier = header.Element(nameSpace + "identifier").Value;
                    string date = header.Element(nameSpace + "datestamp").Value;
                    string setSpec = header.Element(nameSpace + "setSpec").Value;
                    IdentifierOAIPMH identifierOAIPMH = new()
                    {
                        Date = DateTime.Parse(date),
                        Identifier = identifier,
                        Set = setSpec,
                        Deleted = false
                    };
                    idList.Add(identifierOAIPMH);
                }
            }
            return idList;
        }
        public List<ListRecordsOAIPMH> ListRecords(string from, ReadConfig pConfig, string until = null, string set = null)
        {
            List<ListRecordsOAIPMH> idList = new();
            string uri = $@"{pConfig.GetUrlOaiPmh()}?verb=ListRecords&metadataPrefix=EDMA";
            if (from != null)
            {
                uri += $"&from={from}";
            }
            if (until != null)
            {
                uri += $"&until={until}";
            }
            if (set != null)
            {
                uri += $"&set={set}";
            }

            WebRequest wrGETURL = WebRequest.Create(uri);
            Stream stream = wrGETURL.GetResponse().GetResponseStream();

            XDocument XMLresponse = XDocument.Load(stream);
            XNamespace nameSpace = XMLresponse.Root.GetDefaultNamespace();
            XElement idListElement = XMLresponse.Root.Element(nameSpace + "ListRecords");

            if (idListElement != null)
            {
                IEnumerable<XElement> recordList = idListElement.Descendants(nameSpace + "record");

                foreach (var record in recordList)
                {
                    ListRecordsOAIPMH identifierRecord = new();

                    var headerList = record.Descendants(nameSpace + "header");
                    foreach (var header in headerList)
                    {
                        header.Attribute(nameSpace + "status");
                        identifierRecord.Identifier = header.Element(nameSpace + "identifier").Value;
                        identifierRecord.Date = DateTime.Parse(header.Element(nameSpace + "datestamp").Value);
                        identifierRecord.Set = header.Element(nameSpace + "setSpec").Value;
                    }

                    var metadataList = record.Descendants(nameSpace + "metadata");
                    foreach (var metadata in metadataList)
                    {
                        var prc = metadata.Descendants(nameSpace + "ProduccionCientificaEstado");
                        foreach (var prod in prc)
                        {
                            identifierRecord.Estado = prod.Element(nameSpace + "estado").Value;
                        }
                    }

                    idList.Add(identifierRecord);
                }
            }
            return idList;
        }
        public string GetRecord(string id, ReadConfig pConfig, string file = null)
        {
            string uri = $@"{pConfig.GetUrlOaiPmh()}?verb=GetRecord&identifier=" + id + "&metadataPrefix=EDMA";
            WebRequest wrGETURL = WebRequest.Create(uri);
            wrGETURL.Timeout = 172800000; // 48h
            Stream stream = wrGETURL.GetResponse().GetResponseStream();
            XDocument XMLresponse = XDocument.Load(stream);
            XNamespace nameSpace = XMLresponse.Root.GetDefaultNamespace();
            if (XMLresponse.Root.Element(nameSpace + "GetRecord") != null)
            {
                string record = XMLresponse.Root.Element(nameSpace + "GetRecord").Descendants(nameSpace + "metadata").First().FirstNode.ToString();
                record = record.Replace("xmlns=\"" + nameSpace + "\"", "");
                return record;
            }
            else
            {
                return null;
            }
        }
    }

}

