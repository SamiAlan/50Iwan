using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Common;
using Iwan.Shared.Extensions;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Common
{
    public class AddAddressDtoValidator : AbstractValidator<AddAddressDto>
    {
        public AddAddressDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(a => a.City)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceValueValidation(localizer);

            RuleFor(a => a.Country)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceValueValidation(localizer);

            RuleFor(a => a.Company)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceValueValidation(localizer);

            RuleFor(a => a.Address1)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceValueValidation(localizer);

            RuleFor(a => a.Email)
                .Cascade(CascadeMode.Stop)
                .CustomEmailValidation(localizer);

            RuleFor(a => a.PhoneNumber)
                .Cascade(CascadeMode.Stop)
                .CustomSyrianPhoneNumberValidation(localizer);
        }
    }
}
