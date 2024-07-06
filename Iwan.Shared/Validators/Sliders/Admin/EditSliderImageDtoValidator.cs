using FluentValidation;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Sliders.Admin
{
    public class EditSliderImageDtoValidator : AbstractValidator<EditSliderImageDto>
    {
        public EditSliderImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(s => s.Order).CustomShouldBePositiveValidation(localizer);
        }
    }
}
