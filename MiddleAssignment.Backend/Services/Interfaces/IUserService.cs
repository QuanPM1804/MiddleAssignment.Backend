using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.DTOs.Login;
using MiddleAssignment.Backend.DTOs.Register;

namespace MiddleAssignment.Backend.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<UserDTO> GetUserById(Guid id);
        Task<UserDTO> UpdateUser(Guid id, UserDTO userDTO);
        Task DeleteUser(Guid id);
        Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}
