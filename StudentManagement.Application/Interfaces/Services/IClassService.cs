using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Interfaces.Services
{
    public interface IClassService
    {
        Task<IEnumerable<ClassResponseDTO>> GetAllAsync();
        Task BulkImportAsync(Stream csvStream);
    }
}
