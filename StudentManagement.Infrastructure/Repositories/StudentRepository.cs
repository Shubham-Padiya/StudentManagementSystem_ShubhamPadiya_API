using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Interfaces.Repositories;
using StudentManagement.Domain.Entities;
using StudentManagement.Infrastructure.Data;

namespace StudentManagement.Infrastructure.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        private readonly AppDbContext _context;
        public StudentRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistWithEmailAsync(string email)
        {
            return await _context.Students.AnyAsync(x => x.EmailId == email);
        }

        public async Task<bool> ExistWithPhoneAsync(string phoneNumber)
        {
            return await _context.Students.AnyAsync(x => x.PhoneNumber == phoneNumber);
        }

        public async Task<Student?> GetWithClasses(Guid id)
        {
            return await _context.Students.Include(x => x.StudentClasses).ThenInclude(sc => sc.Class)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<Student> Query()
        {
            return _context.Students.AsNoTracking().AsQueryable();
        }
    }
}
