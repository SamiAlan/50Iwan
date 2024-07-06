using FluentValidation;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Settings
{
    public class TempImagesSettingsDtoValidator : AbstractValidator<TempImagesSettingsDto>
    {
        public TempImagesSettingsDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(s => s.DelayInMinutes).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.TempImagesExpirationDays).CustomShouldBePositiveValidation(localizer);
        }
    }
}
