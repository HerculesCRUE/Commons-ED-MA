using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Models
{
    class Resource
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("resourceURI")]
        public string ResourceURI { get; set; }
    }
}
