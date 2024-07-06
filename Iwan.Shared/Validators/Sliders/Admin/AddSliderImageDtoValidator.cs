using FluentValidation;
using Iwan.Shared.Dtos.Sliders;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Sliders.Admin
{
    public class AddSliderImageDtoValidator : AbstractValidator<AddSliderImageDto>
    {
        public AddSliderImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(s => s.Order).CustomShouldBePositiveValidation(localizer);
            RuleFor(s => s.Image).CustomImageNotNullValidation(localizer);
        }
    }
}
