using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using StudentManagement.Application.Consts;
using StudentManagement.Application.DTOs;
using StudentManagement.Application.Interfaces;
using StudentManagement.Application.Interfaces.Services;
using StudentManagement.Domain.Entities;
using System.Globalization;

namespace StudentManagement.Application.Services
{
    public class ClassService(IUnitOfWork _unitOfWork, IValidator<ClassCreateDTO> _validator) : IClassService
    {
        public async Task<IEnumerable<ClassResponseDTO>> GetAllAsync()
        {
            var classes = await _unitOfWork.ClassRepository.GetAllAsync();

            return classes.Select(c => new ClassResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task BulkImportAsync(Stream csvStream)
        {
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });

            var records = csv.GetRecords<ClassCreateDTO>().ToList();

            if (!records.Any())
                throw new InvalidOperationException(ConstantMessages.EmptyFile);

            foreach (var record in records)
            {
                var validation = await _validator.ValidateAsync(record);
                if (!validation.IsValid)
                    throw new FluentValidation.ValidationException(validation.Errors);

                if (await _unitOfWork.ClassRepository.ExistsByNameAsync(record.Name))
                    throw new InvalidOperationException(string.Format(ConstantMessages.DuplicateClassName, record.Name));

                await _unitOfWork.ClassRepository.AddAsync(new Class
                {
                    Id = Guid.NewGuid(),
                    Name = record.Name,
                    Description = record.Description,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
