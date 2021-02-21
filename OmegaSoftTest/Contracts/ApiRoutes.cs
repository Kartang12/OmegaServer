using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Contracts
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
        }

        public static class Apis
        {
            public const string GetAll = Base + "/apis";
            public const string GetById = Base + "/apis/{id}";
            public const string GetApiTasks = Base + "/apis/{id}/tasks";
            public const string GetApiTaskParameters = Base + "/apis/task/{id}/parameters";
        }

        public static class Task
        {
            public const string Save = Base + "/task/save";
            public const string GetByUserId = Base + "/task/user";
            public const string GetApiTasks = Base + "/apis/{id}/tasks";
            public const string GetApiTaskParameters = Base + "/apis/task/{id}/parameters";
        }

    }
}
