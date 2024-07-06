using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Accounts;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Iwan.Shared.Extensions;

namespace Iwan.Shared.Validators.Accounts.Admin
{
    public class AddUserDtoValidator : AbstractValidator<AddUserDto>
    {
        public AddUserDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(u => u.Email)
                .Cascade(CascadeMode.Stop)
                .CustomEmailValidation(localizer);

            RuleFor(u => u.Password)
                .Cascade(CascadeMode.Stop)
                .CustomPasswordValidation(6, localizer);

            RuleFor(u => u.Name)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceNameValidation(localizer);

            RuleFor(u => u.Role)
                .Cascade(CascadeMode.Stop)
                .Must(BeValidRole).WithMessage(localizer.Localize(ValidationResponses.Accounts.InvalidRole));
        }

        public static bool BeValidRole(string role)
        {
            return Roles.Contains(role);
        }
    }
}
