using FluentValidation;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Settings
{
    public class AboutUsSectionImagesSettingsDtoValidator : AbstractValidator<AboutUsSectionImagesSettingsDto>
    {
        public AboutUsSectionImagesSettingsDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(s => s.MediumImageWidth).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.MediumImageHeight).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.MobileImageWidth).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.MobileImageHeight).CustomShouldBePositiveValidation(localizer);
        }
    }
}
