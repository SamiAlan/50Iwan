using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Extensions;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Accounts.Admin
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(c => c.OldPassword)
                .Cascade(CascadeMode.Stop)
                .CustomPasswordsNotEqual(c => c.NewPassword, localizer);

            RuleFor(c => c.NewPassword)
                .Cascade(CascadeMode.Stop)
                .CustomPasswordValidation(6, localizer);

            RuleFor(c => c.ConfirmNewPassword)
                .Cascade(CascadeMode.Stop)
                .CustomPasswordsEqual(c => c.NewPassword, localizer);
        }
    }
}
