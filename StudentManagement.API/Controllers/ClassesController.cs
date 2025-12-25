using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Consts;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace StudentManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController(IClassService _classService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _classService.GetAllAsync();
            return Ok(new ApiResponseDTO<object>
            {
                Data = data,
                StatusCode = 200,
                Success = true,
            });
        }

        [HttpPost("import")]
        public async Task<IActionResult> Import(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponseDTO<string>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ConstantMessages.FileRequired
                });

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest(new ApiResponseDTO<string>
                {
                    StatusCode = 400,
                    Success = false,
                    Message = ConstantMessages.FileSizeExceed
                });

            await using var stream = file.OpenReadStream();
            await _classService.BulkImportAsync(stream);

            return Ok(new ApiResponseDTO<string>
            {
                StatusCode = 200,
                Success = true,
                Message = ConstantMessages.FileImported
            });
        }
    }
}
