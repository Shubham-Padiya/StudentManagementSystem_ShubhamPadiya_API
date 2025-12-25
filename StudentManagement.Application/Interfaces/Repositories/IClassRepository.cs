using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Interfaces.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<bool> ExistsByNameAsync(string name);
    }
}
