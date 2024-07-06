using FluentValidation;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Vendors.Admin
{
    public class EditVendorDtoValidator : AbstractValidator<EditVendorDto>
    {
        public EditVendorDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(v => v.Name).CustomNotEmptyOrWhitespaceNameValidation(localizer);
            RuleFor(v => v.BenefitPercent).CustomRangePercentValidation(0, 100, localizer);
        }
    }
}
