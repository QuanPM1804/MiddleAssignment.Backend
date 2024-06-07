using MiddleAssignment.Backend.Models;

namespace MiddleAssignment.Backend.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(Guid id);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task DeleteUser(Guid id);
        //Task<bool> VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
    }
}
