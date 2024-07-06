using FluentValidation;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Products.Admin
{
    public class AddProductMainImageDtoValidator : AbstractValidator<AddProductMainImageDto>
    {
        public AddProductMainImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(i => i.Image).CustomImageNotNullValidation(localizer);
        }
    }
}
