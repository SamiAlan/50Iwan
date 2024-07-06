using FluentValidation;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Sales.Admin
{
    public class AddBillItemDtoValidator : AbstractValidator<AddBillItemDto>
    {
        public AddBillItemDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(b => b.Quantity).CustomShouldBePositiveValidation(localizer);
            RuleFor(b => b.Price).CustomShouldBePositiveValidation(localizer);
        }
    }
}
