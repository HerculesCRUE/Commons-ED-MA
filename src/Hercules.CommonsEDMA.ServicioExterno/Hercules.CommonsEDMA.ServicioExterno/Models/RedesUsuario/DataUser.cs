using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.RedesUsuario
{
    public class User
    {
        public List<DataUser> dataUser { get; set; }

    }
    public class DataUser
    {
        public string nombre { get; set; }
        public string id { get; set; }
        public string valor { get; set; }
    }
}
