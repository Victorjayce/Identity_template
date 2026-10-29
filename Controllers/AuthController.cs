using Identity_template.Interface;
using Identity_template.Data;
using Identity_template.Dtos.AuthDtos;
using Identity_template.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Identity_template.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IAuth _authService;

        public AuthController(UserManager<User> userManager, IConfiguration configuration, DataContext context, TokenValidationParameters tokenValidationParameters, IAuth authService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
            _authService = authService;
        }

        [HttpPost("user/signup")]
        public async Task<ActionResult> Register([FromBody] RegisterDto payload)
        {
            return ToActionResult(await _authService.SignUp(payload));
        }

        [HttpPost("user/login")]
        public async Task<ActionResult> Login([FromBody] LoginDto payload)
        {
            if (!ModelState.IsValid) return BadRequest("provide all required details");
            return ToActionResult(await _authService.Login(payload));
        }

        [HttpPost("user/refresh-token")]
        public async Task<ActionResult> TokenRefresh([FromBody] TokenRequestDto payload)
        {
            if (!ModelState.IsValid) return BadRequest("provide all required details");

            return ToActionResult(await _authService.RefreshToken(payload));
        }

        [HttpPost("logout/{refreshToken}"), Authorize]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            var userId = UserId;
            await _authService.Logout(userId, refreshToken);
            return NoContent();
        }
    }
}
