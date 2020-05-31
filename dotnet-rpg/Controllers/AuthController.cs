
using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos;
using dotnet_rpg.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthRepository _authRepo;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepo = authRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto registerDto)
        {
            ServiceResponse<int> response=await _authRepo.Register(new User{UserName=registerDto.Username},registerDto.Password);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserRegisterDto registerDto)
        {
            ServiceResponse<string> response=await _authRepo.Login(registerDto.Username,registerDto.Password);
            if(!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}