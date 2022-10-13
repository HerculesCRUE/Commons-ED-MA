using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Cluster
{
    public class Cluster
    {
        public class PerfilCluster
        {
            public class UserCluster
            {
                public string userID { get; set; }
                public string shortUserID { get; set; }
                public string name { get; set; }
                public string info { get; set; }
                public int numPublicacionesTotal { get; set; }
                public int ipNumber { get; set; }
            }
            public string entityID { get; set; }
            public string name { get; set; }
            public List<string> terms { get; set; }
            public List<string> tags { get; set; }
            public List<UserCluster> users { get; set; }
        }
        public string entityID { get; set; }
        public string name { get; set; }
        public string fecha { get; set; }
        public string description { get; set; }
        public List<string> terms { get; set; }
        public List<PerfilCluster> profiles { get; set; }
    }

    public class ScoreCluster
    {
        public float ajuste { get; set; }
        public int numPublicaciones { get; set; }
        public int numPublicacionesTotal { get; set; }
        public int ipNumber { get; set; }
        public Dictionary<string, int> terms { get; set; }
        public Dictionary<string, int> tags { get; set; }
    }

}
