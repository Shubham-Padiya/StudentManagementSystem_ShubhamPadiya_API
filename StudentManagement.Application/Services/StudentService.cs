using Microsoft.EntityFrameworkCore;
using StudentManagement.Application.Consts;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Interfaces.Services;
using StudentManagement.Domain.Entities;

namespace StudentManagement.Application.Services
{
    public class StudentService(IUnitOfWork _unitOfWork) : IStudentService
    {
        public async Task<StudentResponseDTO> CreateAsync(StudentCreateDTO dto)
        {
            if (await _unitOfWork.StudentRepository.ExistWithPhoneAsync(dto.PhoneNumber))
                throw new InvalidOperationException(string.Format(ConstantMessages.AlreadyExist, "Phone Number"));

            if (await _unitOfWork.StudentRepository.ExistWithEmailAsync(dto.EmailId))
                throw new InvalidOperationException(string.Format(ConstantMessages.AlreadyExist, "Email"));

            var student = new Student
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                EmailId = dto.EmailId,
                CreatedAt = DateTime.UtcNow
            };

            foreach (var classId in dto.ClassIds)
            {
                student.StudentClasses.Add(new StudentClass
                {
                    StudentId = student.Id,
                    ClassId = classId
                });
            }

            await _unitOfWork.StudentRepository.AddAsync(student);
            await _unitOfWork.SaveChangesAsync();

            return await MapStudentAsync(student.Id);
        }

        public async Task<StudentResponseDTO> UpdateAsync(Guid id, StudentUpdateDTO dto)
        {
            var student = await _unitOfWork.StudentRepository.GetWithClasses(id)
                ?? throw new KeyNotFoundException(ConstantMessages.StudentNotFound);

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;

            student.StudentClasses.Clear();

            foreach (var classId in dto.ClassIds)
            {
                student.StudentClasses.Add(new StudentClass
                {
                    StudentId = id,
                    ClassId = classId
                });
            }

            _unitOfWork.StudentRepository.Update(student);
            await _unitOfWork.SaveChangesAsync();

            return await MapStudentAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var student = await _unitOfWork.StudentRepository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException(ConstantMessages.StudentNotFound);

            _unitOfWork.StudentRepository.Remove(student);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<StudentResponseDTO> GetByIdAsync(Guid id)
        {
            return await MapStudentAsync(id);
        }

        private async Task<StudentResponseDTO> MapStudentAsync(Guid id)
        {
            var student = await _unitOfWork.StudentRepository.GetWithClasses(id)
                ?? throw new KeyNotFoundException(ConstantMessages.StudentNotFound);

            return new StudentResponseDTO
            {
                Id = student.Id,
                FullName = $"{student.FirstName} {student.LastName}",
                PhoneNumber = student.PhoneNumber,
                EmailId = student.EmailId,
                Classes = student.StudentClasses
                    .Select(sc => sc.Class.Name)
                    .ToList()
            };
        }

        public async Task<(IEnumerable<StudentResponseDTO>, int)> GetStudentsAsync(StudentGetDTO query)
        {
            var allowedSortColumns = new[]
            {
                nameof(Student.FirstName),
                nameof(Student.LastName),
                nameof(Student.EmailId)
            };

            if (!allowedSortColumns.Contains(query.SortBy))
                query.SortBy = nameof(Student.FirstName);

            var studentsQuery = _unitOfWork.StudentRepository.Query();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                studentsQuery = studentsQuery.Where(s =>
                    s.FirstName.Contains(query.Search) ||
                    s.LastName.Contains(query.Search) ||
                    s.EmailId.Contains(query.Search)
                );
            }

            var totalCount = await studentsQuery.CountAsync();

            studentsQuery = query.SortDesc
                ? studentsQuery.OrderByDescending(s => EF.Property<object>(s, query.SortBy!))
                : studentsQuery.OrderBy(s => EF.Property<object>(s, query.SortBy!));

            var students = await studentsQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            var result = new List<StudentResponseDTO>();

            foreach (var student in students)
            {
                result.Add(await MapStudentAsync(student.Id));
            }

            return (result, totalCount);
        }

    }
}
