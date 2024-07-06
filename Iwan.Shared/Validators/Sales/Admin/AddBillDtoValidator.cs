using FluentValidation;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Sales.Admin
{
    public class AddBillDtoValidator : AbstractValidator<AddBillDto>
    {
        public AddBillDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(b => b.CustomerName)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceNameValidation(localizer);

            RuleFor(b => b.CustomerPhone)
                .Cascade(CascadeMode.Stop)
                .CustomSyrianPhoneNumberValidation(localizer);

            RuleFor(b => b.BillItems)
                .Cascade(CascadeMode.Stop)
                .CustomAnyBillItemValidation(localizer)
                .ForEach(b => b.SetValidator(new AddBillItemDtoValidator(localizer)));
        }
    }
}
