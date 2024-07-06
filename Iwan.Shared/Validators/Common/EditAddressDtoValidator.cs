using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Common;
using Iwan.Shared.Extensions;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Common
{
    public class EditAddressDtoValidator : AbstractValidator<EditAddressDto>
    {
        public EditAddressDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(a => a.City).CustomNotEmptyOrWhitespaceValueValidation(localizer);
            RuleFor(a => a.Country).CustomNotEmptyOrWhitespaceValueValidation(localizer);
            RuleFor(a => a.Company).CustomNotEmptyOrWhitespaceValueValidation(localizer);
            RuleFor(a => a.Address1).CustomNotEmptyOrWhitespaceValueValidation(localizer);
            RuleFor(a => a.Email).CustomEmailValidation(localizer);
            RuleFor(a => a.PhoneNumber).CustomSyrianPhoneNumberValidation(localizer);
        }
    }
}
