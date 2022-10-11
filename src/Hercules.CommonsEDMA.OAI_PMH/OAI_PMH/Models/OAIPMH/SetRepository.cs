using OaiPmhNet;
using OaiPmhNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.OAIPMH
{
    class SetRepository : ISetRepository
    {
        private readonly IOaiConfiguration _configuration;
        private readonly IList<Set> _sets;

        public SetRepository(IOaiConfiguration configuration)
        {
            _configuration = configuration;
            _sets = new List<Set>();
            List<string> setNameList = new List<string>
            {
                "Persona",
                "Organizacion",
                "Proyecto",
                "PRC",
                "AutorizacionProyecto",
                "Invencion",
                "Grupo"
            };

            foreach (string setName in setNameList)
            {
                Set set = new()
                {
                    Spec = setName,
                    Name = setName,
                    Description = setName
                };
                _sets.Add(set);
            }
        }

        public SetContainer GetSets(ArgumentContainer arguments, IResumptionToken resumptionToken = null)
        {
            SetContainer container = new();
            IQueryable<Set> sets = _sets.AsQueryable().OrderBy(s => s.Name);
            int totalCount = sets.Count();
            container.Sets = sets;
            return container;
        }
    }
}
