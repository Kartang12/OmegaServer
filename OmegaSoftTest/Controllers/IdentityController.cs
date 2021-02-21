using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OmegaSoftTest.Contracts;
using OmegaSoftTest.Contracts.Requests;
using OmegaSoftTest.Domain;
using OmegaSoftTest.Services;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OmegaSoftTest.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        //private readonly UserService _userService;
        //private readonly IConfiguration _configuration;


        public IdentityController(IIdentityService identityService)
        {
            //_configuration = Configuration;
            this._identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest req)
        {
            var result = new AuthenticationResult();
            if (ModelState.IsValid)
            {
                result = await _identityService.RegisterAsync(req.Email, req.Password, req.RoleId);

            }

            return Ok(result);
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest req)
        {
            var result = await _identityService.LoginAsync(req.Email, req.Password);

            if (!result.Success)
            {
                return BadRequest(new AuthenticationResult
                {
                    Success = false,
                    Errors = result.Errors
                });
            }

            return Ok(result);
        }
    }
}
