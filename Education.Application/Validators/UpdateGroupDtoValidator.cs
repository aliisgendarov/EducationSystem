using Education.Application.DTOs.Group;
using FluentValidation;

namespace Education.Application.Validators
{
    public sealed class UpdateGroupDtoValidator : AbstractValidator<UpdateGroupDto>
    {
        public UpdateGroupDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().Length(2, 100);
        }
    }
}
