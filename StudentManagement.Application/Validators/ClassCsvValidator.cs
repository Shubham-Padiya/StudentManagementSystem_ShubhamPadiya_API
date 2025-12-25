using FluentValidation;
using StudentManagement.Application.DTOs;

namespace StudentManagement.Application.Validators
{
    public class ClassCsvValidator : AbstractValidator<ClassCreateDTO>
    {
        public ClassCsvValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Description)
                .MaximumLength(100);
        }
    }
}
