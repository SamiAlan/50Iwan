using FluentValidation;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Sales.Admin
{
    public class EditBillDtoValidator : AbstractValidator<EditBillDto>
    {
        public EditBillDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(b => b.CustomerName).CustomNotEmptyOrWhitespaceNameValidation(localizer);
            RuleFor(b => b.CustomerPhone).CustomSyrianPhoneNumberValidation(localizer);
            RuleFor(b => b.Total).CustomShouldBePositiveValidation(localizer);
        }
    }
}
