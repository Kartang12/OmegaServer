using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmegaSoftTest.Contracts;
using OmegaSoftTest.Contracts.Requests;
using OmegaSoftTest.Services;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            this._taskService = taskService;
        }

        [HttpPost(ApiRoutes.Task.Save)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SaveTask([FromBody] SaveTaskRequest req)
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);
            return Ok(await _taskService.AssignTaskToUser(req, userId));
        }

        [HttpPost(ApiRoutes.Task.GetByUserId)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserTasks()
        {
            int userId = int.Parse(this.User.Claims.First(i => i.Type == "userId").Value);
            return Ok(await _taskService.GetUserTasks(userId));
        }


    }
}
