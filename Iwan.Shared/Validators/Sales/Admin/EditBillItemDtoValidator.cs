using FluentValidation;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Sales.Admin
{
    public class EditBillItemDtoValidator : AbstractValidator<EditBillItemDto>
    {
        public EditBillItemDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(b => b.NewQuantity).CustomShouldBePositiveValidation(localizer);
            RuleFor(b => b.NewPrice).CustomShouldBePositiveValidation(localizer);
        }
    }
}
