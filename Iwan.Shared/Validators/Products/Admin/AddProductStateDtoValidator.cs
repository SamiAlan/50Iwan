using FluentValidation;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Products.Admin
{
    public class AddProductStateDtoValidator : AbstractValidator<AddProductStateDto>
    {
        public AddProductStateDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(s => s.ArabicName)
                .CustomNotEmptyOrWhitespaceValueValidation(localizer);
        }
    }
}
