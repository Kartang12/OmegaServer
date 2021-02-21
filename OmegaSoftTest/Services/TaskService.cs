using Microsoft.Extensions.Configuration;
using OmegaSoftTest.Contracts.Requests;
using OmegaSoftTest.Contracts.Responses;
using OmegaSoftTest.Domain;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public class TaskService : ITaskService
    {
        private readonly IConfiguration _configuration;
        private readonly string getLastId = "SELECT last_insert_rowid();";
        public TaskService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AssignTaskToUser(SaveTaskRequest req, int userId)
        {
            //throw new NotImplementedException();
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"INSERT INTO UserTasks " +
                    $"(Name, UserId, TaskId, NextInvoke, DaysInterval, TimeInterval) " +
                    $"VALUES ({req.Name}, {userId}, {req.TaskId}, " +
                    $"{req.NextInvoke.Replace('T', ' ')}, " +
                    $"{req.DaysInterval}, {req.TimeInterval});" +
                    $"{getLastId}";
                command.Connection = connection;
                long userTaskId = (long)(await command.ExecuteScalarAsync());

                foreach (var param in req.parameters)
                {
                    command.CommandText = $"INSERT INTO UserTaskParameters " +
                       $"(UserTaskId, PrameterId, Value)" +
                       $"VALUES ({userTaskId}, {param.ParameterId}, {param.Value});";
                    await command.ExecuteScalarAsync();
                }

                return await GetChangesAsync(connection);
            }
        }

        public Task<bool> DismissTask(int userId, int taskId)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserTask>> GetUserTasks(int userId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT (Id, Name) FROM UserTasks WHERE UserId = {userId}";
                command.Connection = connection;
                var reader = command.ExecuteReader();

                List<UserTask> result = new List<UserTask>();
                while (reader.Read())
                {
                    var parameter = new UserTask();
                    parameter.Id = (int)(long)reader["Id"];
                    parameter.Name = (string)reader["Name"];
                }
                return Task.FromResult(result);
            }
        }

        public async Task<int> GetChangesAsync(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = "SELECT changes()";
            command.Connection = connection;
            return (int)(long)(await command.ExecuteScalarAsync());
        }
    }
}
