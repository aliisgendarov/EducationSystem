using Education.Application.DTOs.Student;
using FluentValidation;

namespace Education.Application.Validators
{
    public sealed class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 50);
            RuleFor(x => x.Surname).NotEmpty().Length(2, 50);
            RuleFor(x => x.Age).InclusiveBetween(1, 120);
            RuleFor(x => x.GroupId).GreaterThan(0);
        }
    }
}
