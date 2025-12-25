using FluentValidation;
using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Validators
{
    public class StudentCreateValidator : AbstractValidator<StudentCreateDTO>
    {
        public StudentCreateValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d{10}$")
                .WithMessage("Phone number must be exactly 10 digits");

            RuleFor(x => x.EmailId)
                .EmailAddress();

            RuleFor(x => x.ClassIds)
                .Must(x => x.Distinct().Count() == x.Count)
                .WithMessage("Duplicate class ids are not allowed");
        }
    }
}
