using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion
{
    public class DataItemRelacion
    {        
        public Data data { get; set; }
        public bool? selectable { get; set; }
        public bool? grabbable { get; set; }

        public DataItemRelacion(Data pData, bool? pSelectable, bool? pGrabbable)
        {
            this.data = pData;
            this.selectable = pSelectable;
            this.grabbable = pGrabbable;
        }
    }

    public class Data
    {
        public enum Type
        {
            none,
            icon_member,
            icon_ip,
            icon_area,
            icon_group,
            icon_project,
            relation_document,
            relation_project
        }
        public string id { get; set; }
        public string name { get; set; }
        public string source { get; set; }
        public string target { get; set; }
        public double? weight { get; set; }
        public string group { get; set; }
        public string type { get; set; }
        public double? score { get; set; }

        public Data(string pId, string pName, string pSource, string pTarget, double? pWeight, string pGroup, Type pType)
        {
            this.id = pId;
            this.name = pName;
            this.source = pSource;
            this.target = pTarget;
            this.weight = pWeight;
            this.group = pGroup;
            this.type = pType.ToString();
        }
    }


}
