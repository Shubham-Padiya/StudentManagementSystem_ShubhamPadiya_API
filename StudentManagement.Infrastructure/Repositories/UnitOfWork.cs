using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IStudentRepository StudentRepository { get; }
        public IClassRepository ClassRepository { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            StudentRepository = new StudentRepository(context);
            ClassRepository = new ClassRepository(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
