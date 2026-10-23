using Education.Application.DTOs.Manager;
using FluentValidation;

namespace Education.Application.Validators
{
    public sealed class UpdateManagerDtoValidator : AbstractValidator<UpdateManagerDto>
    {
        public UpdateManagerDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().Length(2, 50);
            RuleFor(x => x.Surname).NotEmpty().Length(2, 50);
            RuleFor(x => x.Age).InclusiveBetween(18, 120);
            RuleFor(x => x.Salary).GreaterThanOrEqualTo(1);
            RuleFor(x => x.TeacherId).GreaterThan(0);
        }
    }
}