using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Contracts.Responses
{
    public class UserInfoResponse
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public long RoleId { get; set; }
    }
}
