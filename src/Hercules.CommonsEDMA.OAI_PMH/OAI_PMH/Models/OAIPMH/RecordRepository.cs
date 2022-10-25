using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI;
using OAI_PMH.Models.SGI.Autorizacion;
using OAI_PMH.Models.SGI.GruposInvestigacion;
using OAI_PMH.Models.SGI.Organization;
using OAI_PMH.Models.SGI.PersonalData;
using OAI_PMH.Models.SGI.ProduccionCientifica;
using OAI_PMH.Models.SGI.Project;
using OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual;
using OAI_PMH.Services;
using OaiPmhNet;
using OaiPmhNet.Converters;
using OaiPmhNet.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OAI_PMH.Models.OAIPMH
{
    public class RecordRepository : IRecordRepository
    {
        private readonly IDateConverter _dateConverter;
        private readonly ConfigService _Config;

        public RecordRepository(ConfigService pConfig)
        {
            _dateConverter = new DateConverter();
            _Config = pConfig;
        }

        public RecordContainer GetIdentifiers(ArgumentContainer arguments, IResumptionToken resumptionToken = null)
        {
            return GetRecords(arguments, resumptionToken);
        }

        public Record GetRecord(string identifier, string metadataPrefix)
        {            
            Record record = new();
            try
            {
                List<Record> listaRecords = new List<Record>();
                string set = identifier.Split('_')[0];
                DateTime date = DateTime.UtcNow;

                switch (set)
                {
                    case "Persona":
                        Persona persona = PersonalData.GetPersona(identifier, _Config);
                        record = ToRecord(persona, set, identifier, date, metadataPrefix);
                        break;
                    case "Proyecto":
                        Proyecto proyecto = Project.GetProyecto(identifier, _Config);
                        record = ToRecord(proyecto, set, identifier, date, metadataPrefix);
                        break;
                    case "Organizacion":
                        Empresa organizacion = Organization.GetEmpresa(identifier, _Config);
                        record = ToRecord(organizacion, set, identifier, date, metadataPrefix);
                        break;
                    case "AutorizacionProyecto":
                        Autorizacion autorizacion = Autorizaciones.GetAutorizacion(identifier, _Config);
                        record = ToRecord(autorizacion, set, identifier, date, metadataPrefix);
                        break;
                    case "Invencion":
                        Invencion invencion = Invention.GetInvenciones(identifier, _Config);
                        record = ToRecord(invencion, set, identifier, date, metadataPrefix);
                        break;
                    case "Grupo":
                        Grupo grupo = InvestigationGroup.GetGrupos(identifier, _Config);
                        record = ToRecord(grupo, set, identifier, date, metadataPrefix);
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                return null;
            }
            return record;
        }

        private static Record ToRecord(SGI_Base pObject, string pSet, string pId, DateTime pDate, string pMetadataPrefix)
        {
            Record record = new()
            {
                Header = new RecordHeader()
                {
                    Identifier = pId,
                    SetSpecs = new List<string>() { pSet },
                    Datestamp = pDate
                }
            };

            switch (pMetadataPrefix)
            {
                case "EDMA":
                    try
                    {
                        record.Metadata = new RecordMetadata()
                        {
                            Content = XElement.Parse(pObject.ToXML())
                        };
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
            }
            return record;
        }

        private static Record ToRecord(SGI_Base pObject, string pSet, long? pId, DateTime pDate, string pMetadataPrefix)
        {
            Record record = new()
            {
                Header = new RecordHeader()
                {
                    Identifier = pId.ToString(),
                    SetSpecs = new List<string>() { pSet },
                    Datestamp = pDate
                }
            };

            switch (pMetadataPrefix)
            {
                case "EDMA":
                    try
                    {
                        record.Metadata = new RecordMetadata()
                        {
                            Content = XElement.Parse(pObject.ToXML())
                        };
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                    break;
            }
            return record;
        }

        public RecordContainer GetRecords(ArgumentContainer arguments, IResumptionToken resumptionToken = null)
        {
            RecordContainer container = new RecordContainer();

            try
            {                
                DateTime startDate = DateTime.MinValue;
                if (_dateConverter.TryDecode(arguments.From, out DateTime from))
                {
                    startDate = from;
                }

                List<XML> listxml = new();

                if (arguments.Verb == OaiVerb.ListIdentifiers.ToString())
                {
                    switch (arguments.Set)
                    {
                        case "Persona":
                            Dictionary<string, DateTime> modifiedPeopleIds = PersonalData.GetModifiedPeople(arguments.From, _Config);
                            List<Record> personRecordList = new();
                            foreach (string personId in modifiedPeopleIds.Keys)
                            {
                                personRecordList.Add(ToIdentifiersRecord("Persona", personId, modifiedPeopleIds[personId]));
                            }
                            container.Records = personRecordList;
                            break;
                        case "Organizacion":
                            Dictionary<string, DateTime> modifiedOrganizationsIds = Organization.GetModifiedOrganizations(arguments.From, _Config);
                            List<Record> organizationRecordList = new();
                            foreach (string organizationId in modifiedOrganizationsIds.Keys)
                            {
                                organizationRecordList.Add(ToIdentifiersRecord("Organizacion", organizationId, modifiedOrganizationsIds[organizationId]));
                            }
                            container.Records = organizationRecordList;
                            break;
                        case "Proyecto":
                            Dictionary<string, DateTime> modifiedProjectsIds = Project.GetModifiedProjects(arguments.From, _Config);
                            List<Record> projectRecordList = new();
                            foreach (string projectId in modifiedProjectsIds.Keys)
                            {
                                projectRecordList.Add(ToIdentifiersRecord("Proyecto", projectId, modifiedProjectsIds[projectId]));
                            }
                            container.Records = projectRecordList;
                            break;
                        case "PRC":
                            Dictionary<string, DateTime> modifiedPRC = PRC.GetModifiedPRC(arguments.From, _Config);
                            List<Record> prcRecordList = new();
                            foreach (string prcId in modifiedPRC.Keys)
                            {
                                prcRecordList.Add(ToIdentifiersRecord("PRC", prcId, modifiedPRC[prcId]));
                            }
                            container.Records = prcRecordList;
                            break;
                        case "AutorizacionProyecto":
                            Dictionary<string, DateTime> modifiedAutorizaciones = Autorizaciones.GetModifiedAutorizaciones(arguments.From, _Config);
                            List<Record> autorizacionRecordList = new();
                            foreach (string autorizacionId in modifiedAutorizaciones.Keys)
                            {
                                autorizacionRecordList.Add(ToIdentifiersRecord("AutorizacionProyecto", autorizacionId, modifiedAutorizaciones[autorizacionId]));
                            }
                            container.Records = autorizacionRecordList;
                            break;
                        case "Invencion":
                            Dictionary<string, DateTime> modifiedInvenciones = Invention.GetModifiedInvenciones(arguments.From, _Config);
                            List<Record> invencionRecordList = new();
                            foreach (string invencionnId in modifiedInvenciones.Keys)
                            {
                                invencionRecordList.Add(ToIdentifiersRecord("Invencion", invencionnId, modifiedInvenciones[invencionnId]));
                            }
                            container.Records = invencionRecordList;
                            break;
                        case "Grupo":
                            Dictionary<string, DateTime> modifiedGrupoIds = InvestigationGroup.GetModifiedGrupos(arguments.From, _Config);
                            List<Record> grupoRecordList = new();
                            foreach (string grupoId in modifiedGrupoIds.Keys)
                            {
                                grupoRecordList.Add(ToIdentifiersRecord("Grupo", grupoId, modifiedGrupoIds[grupoId]));
                            }
                            container.Records = grupoRecordList;
                            break;
                    }
                }
                else
                {
                    switch (arguments.Set)
                    {
                        case "Persona":
                            Dictionary<string, DateTime> modifiedPeopleIds = PersonalData.GetModifiedPeople(arguments.From, _Config);
                            List<Persona> peopleList = new();
                            foreach (string personId in modifiedPeopleIds.Keys)
                            {
                                peopleList.Add(PersonalData.GetPersona(personId, _Config));
                            }
                            List<Record> personRecordList = new();
                            foreach (Persona persona in peopleList)
                            {
                                personRecordList.Add(ToRecord(persona, arguments.Set, persona.Id, startDate, arguments.MetadataPrefix));
                            }
                            container.Records = personRecordList;
                            break;
                        case "Organizacion":
                            Dictionary<string, DateTime> modifiedOrganizationsIds = Organization.GetModifiedOrganizations(arguments.From, _Config);
                            List<Empresa> organizationsList = new();
                            foreach (string organizationId in modifiedOrganizationsIds.Keys)
                            {
                                organizationsList.Add(Organization.GetEmpresa(organizationId, _Config));
                            }
                            List<Record> organizationRecordList = new();
                            foreach (Empresa empresa in organizationsList)
                            {
                                organizationRecordList.Add(ToRecord(empresa, arguments.Set, empresa.Id, startDate, arguments.MetadataPrefix));
                            }
                            container.Records = organizationRecordList;
                            break;
                        case "Proyecto":
                            Dictionary<string, DateTime> modifiedProjectsIds = Project.GetModifiedProjects(arguments.From, _Config);
                            List<Proyecto> projectsList = new();
                            foreach (string projectId in modifiedProjectsIds.Keys)
                            {
                                projectsList.Add(Project.GetProyecto(projectId, _Config));
                            }
                            List<Record> projectRecordList = new();
                            foreach (Proyecto proyecto in projectsList)
                            {
                                if (proyecto.Id == null)
                                {
                                    continue;
                                }

                                projectRecordList.Add(ToRecord(proyecto, arguments.Set, proyecto.Id, startDate, arguments.MetadataPrefix));
                            }
                            container.Records = projectRecordList;
                            break;
                        case "PRC":
                            List<ProduccionCientificaEstado> prcList = PRC.GetPRC(arguments.From, _Config);
                            List<Record> prcRecordList = new();
                            foreach (ProduccionCientificaEstado prc in prcList)
                            {
                                prcRecordList.Add(ToRecord(prc, arguments.Set, prc.idRef, startDate, arguments.MetadataPrefix));
                            }
                            container.Records = prcRecordList;
                            break;
                        case "AutorizacionProyecto":
                            Dictionary<string, DateTime> modifiedAutorizacionIds = Autorizaciones.GetModifiedAutorizaciones(arguments.From, _Config);
                            List<Autorizacion> autorizacionList = new();
                            foreach (string autorizacionId in modifiedAutorizacionIds.Keys)
                            {
                                autorizacionList.Add(Autorizaciones.GetAutorizacion(autorizacionId, _Config));
                            }
                            List<Record> autorizacionRecordList = new();
                            foreach (Autorizacion autorizacion in autorizacionList)
                            {
                                autorizacionRecordList.Add(ToRecord(autorizacion, arguments.Set, autorizacion.id.ToString(), startDate, arguments.MetadataPrefix));
                            }
                            container.Records = autorizacionRecordList;
                            break;
                        case "Invencion":
                            Dictionary<string, DateTime> modifiedInvencionIds = Invention.GetModifiedInvenciones(arguments.From, _Config);
                            List<Invencion> invencionList = new();
                            foreach (string invencionId in modifiedInvencionIds.Keys)
                            {
                                invencionList.Add(Invention.GetInvenciones(invencionId, _Config));
                            }
                            List<Record> invencionRecordList = new();
                            foreach (Invencion invencion in invencionList)
                            {
                                invencionRecordList.Add(ToRecord(invencion, arguments.Set, invencion.id.ToString(), startDate, arguments.MetadataPrefix));
                            }
                            container.Records = invencionRecordList;
                            break;
                        case "Grupo":
                            Dictionary<string, DateTime> modifiedGrupoIds = InvestigationGroup.GetModifiedGrupos(arguments.From, _Config);
                            List<Grupo> grupoList = new();
                            foreach (string grupoId in modifiedGrupoIds.Keys)
                            {
                                grupoList.Add(InvestigationGroup.GetGrupos(grupoId, _Config));
                            }
                            List<Record> grupoRecordList = new();
                            foreach (Grupo grupo in grupoList)
                            {
                                grupoRecordList.Add(ToRecord(grupo, arguments.Set, grupo.id.ToString(), startDate, arguments.MetadataPrefix));
                            }
                            container.Records = grupoRecordList;
                            break;
                    }
                }
            }
            catch
            {
                return null;
            }

            return container;
        }

        private static Record ToIdentifiersRecord(string pSet, string pId, DateTime pDate)
        {
            Record record = new()
            {
                Header = new RecordHeader()
                {
                    Identifier = pId,
                    SetSpecs = new List<string>() { pSet },
                    Datestamp = pDate
                }
            };
            return record;
        }
    }
}
