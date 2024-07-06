using FluentValidation;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Accounts.Admin
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(l => l.Email)
                .Cascade(CascadeMode.Stop)
                .CustomEmailValidation(localizer);

            RuleFor(l => l.Password)
                .Cascade(CascadeMode.Stop)
                .CustomPasswordValidation(6, localizer);
        }
    }
}
