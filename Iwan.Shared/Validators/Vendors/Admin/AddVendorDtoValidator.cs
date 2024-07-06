using FluentValidation;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Extensions;
using Iwan.Shared.Validators.Common;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Vendors.Admin
{
    public class AddVendorDtoValidator : AbstractValidator<AddVendorDto>
    {
        public AddVendorDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(v => v.Name).CustomNotEmptyOrWhitespaceNameValidation(localizer);
            RuleFor(v => v.BenefitPercent).CustomRangePercentValidation(0, 100, localizer);
            RuleFor(v => v.Address).SetValidator(new AddAddressDtoValidator(localizer));
        }
    }
}
