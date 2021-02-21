using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Domain
{
    public class ApiTask
    {
        public long Id { get; set; }
        public long ApiId { get; set; }
        public string QueryString{ get; set; }
        public string Description { get; set; }
    }
}
