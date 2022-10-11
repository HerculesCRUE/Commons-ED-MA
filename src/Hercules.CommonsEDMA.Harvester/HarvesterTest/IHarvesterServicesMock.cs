using Harvester;
using Harvester.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HavesterTest
{
    public class HarvesterServicesMock : IHaversterServices
    {
        public string GetRecord(string id, ReadConfig pConfig, String file = null)
        {
            string uri = $@"{pConfig.GetUrlOaiPmh()}?verb=GetRecord&identifier=" + id + "&metadataPrefix=EDMA";



            XDocument XMLresponse = XDocument.Load(file);

            XNamespace nameSpace = XMLresponse.Root.GetDefaultNamespace();
            string record = XMLresponse.Root.Element(nameSpace + "GetRecord").Descendants(nameSpace + "metadata").First().FirstNode.ToString();
            record = record.Replace("xmlns=\"" + nameSpace + "\"", "");
            return record;

        }

        public List<IdentifierOAIPMH> ListIdentifiers(string from, ReadConfig pConfig, string until = null, string set = null)
        {
            List<IdentifierOAIPMH> idList = new();
            //C: \Users\hsolar\Source\Repos\HerculesED\src\Harvester\HavesterTest\xml_examples\
            XDocument XMLresponse = XDocument.Load(from);
            XNamespace nameSpace = XMLresponse.Root.GetDefaultNamespace();
            XElement idListElement = XMLresponse.Root.Element(nameSpace + "ListIdentifiers");

            if (idListElement != null)
            {
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
            throw new NotImplementedException();
        }
    }
}
