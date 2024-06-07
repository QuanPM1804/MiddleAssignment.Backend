using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiddleAssignment.Backend.DTOs.Login;
using MiddleAssignment.Backend.DTOs.Register;
using MiddleAssignment.Backend.Services.Interfaces;

namespace MiddleAssignment.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegistrationResponse>> Register(RegistrationRequest request)
        {
            try
            {
                var response = await _userService.RegisterAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                var response = await _userService.LoginAsync(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}
