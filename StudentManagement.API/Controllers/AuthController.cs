using Microsoft.AspNetCore.Mvc;
using StudentManagement.API.Auth;
using StudentManagement.Application.Consts;
using StudentManagement.Application.DTOs;

namespace StudentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration _config) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login(LoginDTO request)
        {
            // Hardcoded user (allowed by requirement)
            if (request.Username != "admin" || request.Password != "admin123")
                return Unauthorized(new { message = ConstantMessages.InvalidCred });

            var token = JWTGenerate.GenerateToken(_config, request);

            return Ok(new ApiResponseDTO<string>
            {
                Data = token,
                StatusCode = 200,
                Success = true,
                Message = ConstantMessages.LoginSuccess
            });
        }
    }
}
