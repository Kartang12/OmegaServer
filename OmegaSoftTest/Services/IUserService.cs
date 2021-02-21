using OmegaSoftTest.Contracts.Responses;
using OmegaSoftTest.Domain;
using System.Data.SQLite;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OmegaSoftTest.Services
{
    public interface IUserService
    {
        Task<int> AddUser(User user);
        int GetChanges(SQLiteConnection connection);
        Task<UserInfoResponse> GetUserInfoByEmail(string email);
        Task<User> GetUserByIdAsync(long id);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> UserExists(string email);
        string GetUserRole(string email);
        Task<bool> CheckUserPasswordAsync(string hashedPassword, long userId);
    }
}