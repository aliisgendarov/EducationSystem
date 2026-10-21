using Education.Application.DTOs.TeacherDtos;
using FluentValidation;

namespace Education.Application.Validators
{
    public sealed class UpdateTeacherDtoValidator : AbstractValidator<UpdateTeacherDto>
    {
        public UpdateTeacherDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().Length(2, 50);
            RuleFor(x => x.Surname).NotEmpty().Length(2, 50);
            RuleFor(x => x.Age).InclusiveBetween(1, 120);
            RuleFor(x => x.Salary).GreaterThanOrEqualTo(0);
        }
    }
}
