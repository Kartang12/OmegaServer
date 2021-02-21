using Microsoft.Extensions.Configuration;
using OmegaSoftTest.Contracts.Responses;
using OmegaSoftTest.Domain;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> UserExists(string email)
        {
            long result = 0;
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT COUNT(*) FROM Users WHERE Users.Email = \'{email}\'";
                command.Connection = connection;
                result = (long)command.ExecuteScalar();
            }

            return Task.FromResult(result > 0);
        }

        public Task<int> AddUser(User user)
        {
            int rowsAffected = 0;
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = "INSERT INTO Users (Email, Salt, PasswordHash, RoleId) " +
                    $"values (\'{user.Email}\', \'{user.Salt}\', \'{user.PasswordHash}\', {user.RoleID});";
                command.Connection = connection;
                command.ExecuteScalar();
                rowsAffected = GetChanges(connection);
            }
            return Task.FromResult(rowsAffected);
        }

        public int GetChanges(SQLiteConnection connection)
        {
            SQLiteCommand command = new SQLiteCommand();
            command.CommandText = "SELECT changes()";
            command.Connection = connection;
            return (int)(long)command.ExecuteScalar();
        }

        public Task<UserInfoResponse> GetUserInfoByEmail(string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT * FROM Users WHERE Email=\'{email}\'";
                command.Connection = connection;
                var reader = command.ExecuteReader();

                UserInfoResponse result = new UserInfoResponse();
                while (reader.Read())
                {
                    result.Id = (long)reader["Id"];
                    result.Email = (string)reader["Email"];
                    result.RoleId = (long)reader["RoleId"];
                }
                return Task.FromResult(result);
            }
        }

        public Task<User> GetUserByIdAsync(long id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT * FROM Users WHERE Id=\'{id}\'";
                command.Connection = connection;
                var reader = command.ExecuteReader();

                User result = new User();
                while (reader.Read())
                {
                    result.Id = (long)reader["Id"];
                    result.Email = (string)reader["Email"];
                    result.PasswordHash = (string)reader["PasswordHash"];
                    result.Salt = (string)reader["Salt"];
                    result.RoleID = (long)reader["RoleId"];
                }
                return Task.FromResult(result);
            }
        }        
        
        public Task<User> GetUserByEmailAsync(string email)
        {
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = $"SELECT * FROM Users WHERE Email=\'{email}\'";
                command.Connection = connection;
                var reader = command.ExecuteReader();

                User result = new User();
                while (reader.Read())
                {
                    result.Id = (long)reader["Id"];
                    result.Email = (string)reader["Email"];
                    result.PasswordHash = (string)reader["PasswordHash"];
                    result.Salt = (string)reader["Salt"];
                    result.RoleID = (long)reader["RoleId"];
                }
                return Task.FromResult(result);
            }
        }

        public string GetUserRole(string email)
        {
            string result = null;
            using (SQLiteConnection connection = new SQLiteConnection(_configuration.GetConnectionString("Default")))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand($"SELECT Roles.Name FROM Users " +
                $"INNER JOIN Roles " +
                $"ON Users.RoleId = Roles.Id " +
                $"WHERE Users.Email = \'{email}\'; ");
                command.Connection = connection;
                result = (string)command.ExecuteScalar();

                //while (reader.Read())
                //{
                //    result = (string)reader["Name"];
                //};
            }
            return result;
            
        }

        public async Task<bool> CheckUserPasswordAsync(string hashedPassword, long userId)
        {
            var user = await GetUserByIdAsync(userId);

            return user.PasswordHash == hashedPassword;
        }
    }
}
