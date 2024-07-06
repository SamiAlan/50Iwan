using FluentValidation;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Products.Admin
{
    public class AddProductImageDtoValidator : AbstractValidator<AddProductImageDto>
    {
        public AddProductImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(i => i.Image).CustomImageNotNullValidation(localizer);
        }
    }
}
