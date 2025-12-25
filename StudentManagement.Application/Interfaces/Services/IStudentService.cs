using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Interfaces.Services
{
    public interface IStudentService
    {
        Task<StudentResponseDTO> CreateAsync(StudentCreateDTO dto);
        Task<StudentResponseDTO> UpdateAsync(Guid id, StudentUpdateDTO dto);
        Task DeleteAsync(Guid id);
        Task<StudentResponseDTO> GetByIdAsync(Guid id);
        Task<(IEnumerable<StudentResponseDTO> Data, int TotalCount)> GetStudentsAsync(StudentGetDTO query);
    }
}
