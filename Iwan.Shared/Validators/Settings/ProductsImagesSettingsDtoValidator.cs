using FluentValidation;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Settings
{
    public class ProductsImagesSettingsDtoValidator : AbstractValidator<ProductsImagesSettingsDto>
    {
        public ProductsImagesSettingsDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(s => s.MediumImageWidth).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.MediumImageHeight).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.SmallImageWidth).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.SmallImageHeight).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.MobileImageWidth).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.MobileImageHeight).CustomShouldBePositiveValidation(localizer);
        }
    }
}
