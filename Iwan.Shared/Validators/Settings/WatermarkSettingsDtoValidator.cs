using FluentValidation;
using Iwan.Shared.Dtos.Settings;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Settings
{
    public class WatermarkSettingsDtoValidator : AbstractValidator<EditWatermarkSettingsDto>
    {
        public WatermarkSettingsDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(s => s.Opacity).CustomRangePercentValidation(0, 1, localizer);
        }
    }
}
