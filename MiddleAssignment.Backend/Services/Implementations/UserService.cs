using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MiddleAssignment.Backend.DTOs;
using MiddleAssignment.Backend.DTOs.Auth;
using MiddleAssignment.Backend.DTOs.Login;
using MiddleAssignment.Backend.DTOs.Register;
using MiddleAssignment.Backend.Models;
using MiddleAssignment.Backend.Repository.Interfaces;
using MiddleAssignment.Backend.Server;
using MiddleAssignment.Backend.Services.Interfaces;
using System.Security.Claims;

namespace MiddleAssignment.Backend.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly JwtSettings _jwtSettings;

        public UserService(IUserRepository userRepository, IMapper mapper, IJwtTokenService jwtTokenService, JwtSettings jwtSettings)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtTokenService = jwtTokenService;
            _jwtSettings = jwtSettings;
        }


        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateUser(Guid id, UserDTO userDTO)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
                return null;

            _mapper.Map(userDTO, user);
            var updatedUser = await _userRepository.UpdateUser(user);
            return _mapper.Map<UserDTO>(updatedUser);
        }

        public async Task DeleteUser(Guid id)
        {
            await _userRepository.DeleteUser(id);
        }

        public async Task<RegistrationResponse> RegisterAsync(RegistrationRequest request)
        {
            // Check if the username is already taken
            var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ArgumentException("Username is already been used.");
            }
            if (existingEmail != null)
            {
                throw new ArgumentException("Email is already been used.");
            }

            // Create a new user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password), // Store the hashed password
                Role = UserRole.User // Set the default role to User
            };

            user.PasswordHash = HashPassword(request.Password);


            await _userRepository.CreateUser(user);

            var response = _mapper.Map<RegistrationResponse>(user);
            return response;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            // Check if the user exists
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                throw new ArgumentException("Invalid username or password.");
            }

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new ArgumentException("Invalid username or password.");
            }

            // Generate JWT tokens
            var accessToken = _jwtTokenService.GenerateAccessToken(user);
            var refreshToken = _jwtTokenService.GenerateRefreshToken();

            // Update the user's refresh token
            user.RefreshToken = refreshToken;
            await _userRepository.UpdateUser(user);

            var response = new LoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id
            };

            return response;
        }

        private string HashPassword(string password)
        {

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
