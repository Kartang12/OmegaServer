using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Domain
{
    public class API
    {
        public long Id { get; set; }
        public string Route { get; set; }
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string Host { get; set; }
    }
}
