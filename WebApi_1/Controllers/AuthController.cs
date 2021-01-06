using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi_1.Data;
using WebApi_1.DTO.User;
using WebApi_1.Models;

namespace WebApi_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO newUser)
        {
            var response = await _authRepository.Register(new User { Username = newUser.Username }, newUser.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {
            var response = await _authRepository.Login(request.Username, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);

            }

            return Ok(response);
        }
    }
}