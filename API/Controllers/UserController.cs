using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly userService _userService;

        public UserController(userService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLogin request)
        {
            var user = await _userService.GetUserByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Invalid credentials.");
            }
            var token = _jwtService.GenerateJwtToken(user.Username, user.Email, user.Role);
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserRegister request)
        {
            var existingUser = await _userService.GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                return BadRequest("Username already exists.");
            }
            var existingEmail = await _userService.GetUserByEmailAsync(request.Email);
            if (existingEmail != null)
            {
                return BadRequest("Email already exists.");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                Role = "user"
            };
            await _userService.CreateUserAsync(newUser);
            return Ok("User registered successfully.");
        }

    }
}
