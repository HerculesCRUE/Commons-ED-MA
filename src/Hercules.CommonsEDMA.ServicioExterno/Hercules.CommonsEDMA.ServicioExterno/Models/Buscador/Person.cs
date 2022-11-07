using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Buscador
{
    public class Person : ObjectSearch
    {
        //Título-->1

        public bool searchable { get; set; }

        public List<Publication> publications { get; set; }
        public List<ResearchObject> researchObjects { get; set; }
        public List<Project> projects { get; set; }
        public List<Group> groups { get; set; }
        public List<Offer> offers { get; set; }
    }

}
