using FluentValidation;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Products.Admin
{
    public class ChangeProductMainImageDtoValidator : AbstractValidator<ChangeProductMainImageDto>
    {
        public ChangeProductMainImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(i => i. Image).CustomImageNotNullValidation(localizer);
        }
    }
}
