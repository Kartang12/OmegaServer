using Microsoft.Extensions.Configuration;
using OmegaSoftTest.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public class ApiService : IApiService
    {
        private readonly IConfiguration _configuration;

        public ApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<List<API>> GetAPIs()
        {
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT * FROM APIs";
                command.Connection = connection;
                var reader = command.ExecuteReader();

                List<API> result = new List<API>();
                while (reader.Read())
                {
                    var api = new API();
                    api.Id = (long)reader["Id"];
                    api.Name = (string)reader["Name"];
                    api.Route= (string)reader["Route"];
                    api.ApiKey = (string)reader["ApiKey"];
                    api.Host = (string)reader["Host"];
                    result.Add(api);
                }
                return Task.FromResult(result);
            }
        }

        public Task<List<ApiTask>> GetApiTasks(long apiId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT * FROM APITasks where APIId = {apiId}";
                command.Connection = connection;
                var reader = command.ExecuteReader();

                List<ApiTask> result = new List<ApiTask>();
                while (reader.Read())
                {
                    var task = new ApiTask();
                    task.Id = (long)reader["Id"];
                    task.ApiId = (long)reader["APIId"];
                    task.Description = (string)reader["Description"];
                    task.QueryString = (string)reader["QueryString"];
                    result.Add(task);
                }
                return Task.FromResult(result);
            }
        }

        public Task<List<TaskParameter>> GetApiTaskParameters(long taskId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT * FROM TaskParameters where TaskId = {taskId}";
                command.Connection = connection;
                var reader = command.ExecuteReader();

                List<TaskParameter> result = new List<TaskParameter>();
                while (reader.Read())
                {
                    var parameter = new TaskParameter();
                    parameter.Id = (long)reader["Id"];
                    parameter.TaskId = (long)reader["TaskId"];
                    parameter.Name = (string)reader["Name"];
                    parameter.Description = (string)reader["Description"];
                    parameter.DefaultValue = (string)reader["DefaultValue"];
                    result.Add(parameter);
                }
                return Task.FromResult(result);
            }
        }

        public Task<API> GetUserAPIs(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
