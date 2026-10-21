using Education.Application.DTOs.Manager;
using FluentValidation;

namespace Education.Application.Validators
{
    public sealed class CreateManagerDtoValidator : AbstractValidator<CreateManagerDto>
    {
        public CreateManagerDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 50);
            RuleFor(x => x.Surname).NotEmpty().Length(2, 50);
            RuleFor(x => x.Age).InclusiveBetween(1, 120);
            RuleFor(x => x.Salary).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TeacherId).GreaterThan(0);
        }
    }
}
