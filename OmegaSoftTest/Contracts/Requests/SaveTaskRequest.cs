using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Contracts.Requests
{
    public class SaveTaskRequest
    {
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string NextInvoke { get; set; }
        public int DaysInterval { get; set; }
        public string TimeInterval { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class Parameter
    {
        public string Value { get; set; }
        public int ParameterId { get; set; }
    }
}
