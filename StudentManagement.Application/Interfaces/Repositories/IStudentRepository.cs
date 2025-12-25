using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Interfaces.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        IQueryable<Student> Query();
        Task<bool> ExistWithPhoneAsync(string phoneNumber);
        Task<bool> ExistWithEmailAsync(string email);
        Task<Student?> GetWithClasses(Guid id);
    }
}
