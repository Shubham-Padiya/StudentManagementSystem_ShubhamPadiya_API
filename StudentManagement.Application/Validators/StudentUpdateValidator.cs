using FluentValidation;
using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Validators
{
    public class StudentUpdateValidator : AbstractValidator<StudentUpdateDTO>
    {
        public StudentUpdateValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.ClassIds)
                .Must(x => x.Distinct().Count() == x.Count)
                .WithMessage("Duplicate class ids are not allowed");
        }
    }
}
