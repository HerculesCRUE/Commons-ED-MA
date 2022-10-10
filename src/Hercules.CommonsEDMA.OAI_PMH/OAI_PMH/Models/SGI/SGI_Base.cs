using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OAI_PMH.Models.SGI
{
    public class SGI_Base
    {
        public string ToXML()
        {
            using var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(GetType());
            serializer.Serialize(stringwriter, this);
            return stringwriter.ToString();
        }
    }
}
