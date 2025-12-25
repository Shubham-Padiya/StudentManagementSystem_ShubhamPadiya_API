using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Application.Consts;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces.Services;

namespace StudentManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(IStudentService _studentService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] StudentGetDTO query)
        {
            var (data, totalCount) = await _studentService.GetStudentsAsync(query);

            return Ok(new ApiResponseDTO<object>
            {
                Data = new
                {
                    data,
                    totalCount,
                },
                StatusCode = 200,
                Success = true,
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetStudent(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            return Ok(new ApiResponseDTO<object>
            {
                Data = student,
                StatusCode = 200,
                Success = true,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentCreateDTO dto)
        {
            var student = await _studentService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, student);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, StudentUpdateDTO dto)
        {
            var student = await _studentService.UpdateAsync(id, dto);
            return Ok(new ApiResponseDTO<object>
            {
                Data = student,
                StatusCode = 200,
                Success = true,
            });
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _studentService.DeleteAsync(id);
            return Ok(new ApiResponseDTO<object>
            {
                StatusCode = 200,
                Success = true,
                Message = ConstantMessages.Deleted
            });
        }
    }
}
