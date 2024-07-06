using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Accounts;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Iwan.Shared.Extensions;

namespace Iwan.Shared.Validators.Accounts.Admin
{
    public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(p => p.Name).CustomNotEmptyOrWhitespaceNameValidation(localizer);
        }
    }
}
