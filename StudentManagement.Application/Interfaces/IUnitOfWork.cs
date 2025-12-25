using StudentManagement.Application.Interfaces.Repositories;

namespace StudentManagement.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IStudentRepository StudentRepository { get; }
        IClassRepository ClassRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
