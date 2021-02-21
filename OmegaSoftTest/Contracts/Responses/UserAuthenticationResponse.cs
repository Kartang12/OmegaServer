using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Contracts.Responses
{
    public class UserAuthenticationResponse
    {
        public string Token { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
