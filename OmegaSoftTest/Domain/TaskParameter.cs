using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Domain
{
    public class TaskParameter
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string Name{ get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
    }
}
