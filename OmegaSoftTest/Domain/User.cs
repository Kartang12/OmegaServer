using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Domain
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public long RoleID { get; set; }
        public string Token { get; set; }
    }
}
