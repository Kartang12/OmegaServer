using OmegaSoftTest.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string password, int roleId);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}
