using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaSoftTest.Contracts;
using OmegaSoftTest.Services;
using System.Threading.Tasks;

namespace OmegaSoftTest.Controllers
{
    public class ApiController : Controller
    {
        private readonly IApiService _apiService;

        public ApiController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet(ApiRoutes.Apis.GetAll)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _apiService.GetAPIs());
        }

        [HttpGet(ApiRoutes.Apis.GetApiTasks)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetApiTasks([FromRoute] long id)
        {
            return Ok(await _apiService.GetApiTasks(id));
        }

        [HttpGet(ApiRoutes.Apis.GetApiTaskParameters)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetApiTaskParameters([FromRoute] long id)
        {
            return Ok(await _apiService.GetApiTaskParameters(id));
        }
    }
}
