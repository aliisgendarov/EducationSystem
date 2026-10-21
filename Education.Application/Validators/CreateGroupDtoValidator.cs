using Education.Application.DTOs.Group;
using FluentValidation;

namespace Education.Application.Validators
{
    public sealed class CreateGroupDtoValidator : AbstractValidator<CreateGroupDto>
    {
        public CreateGroupDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);
        }
    }
}
