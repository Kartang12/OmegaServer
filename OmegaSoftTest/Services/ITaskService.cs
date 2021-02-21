using OmegaSoftTest.Contracts.Requests;
using OmegaSoftTest.Contracts.Responses;
using OmegaSoftTest.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public interface ITaskService
    { 
        Task<List<UserTask>> GetUserTasks(int userId);
        Task<int> AssignTaskToUser(SaveTaskRequest req, int userId);
        Task<bool> DismissTask(int userId, int taskId);
    }
}
