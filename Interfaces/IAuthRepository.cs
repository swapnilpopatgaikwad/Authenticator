using Authenticator.DTO;
using Authenticator.Model;

namespace Authenticator.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> Register(UserDto user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<bool> UserExistsById(int id);
        Task<List<User>> GetAllUsers();
        Task<User> FindByIdAsync(int id);
        Task<int> UpdateUser(User user);
        Task<int> AddUser(User user);
        Task<int> DeleteUser(User user);
    }
}
