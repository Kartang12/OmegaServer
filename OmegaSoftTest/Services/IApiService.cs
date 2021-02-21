using OmegaSoftTest.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public interface IApiService
    {
        Task<List<API>> GetAPIs();
        Task<API> GetUserAPIs(int userId);
        Task<List<ApiTask>> GetApiTasks(long apiId);
        Task<List<TaskParameter>> GetApiTaskParameters(long taskId);
    }
}