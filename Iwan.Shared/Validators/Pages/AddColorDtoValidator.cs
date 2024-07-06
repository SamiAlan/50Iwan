using FluentValidation;
using Iwan.Shared.Dtos.Pages;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Pages
{
    public class AddColorDtoValidator : AbstractValidator<AddColorDto>
    {
        public AddColorDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(c => c.ColorCode).CustomColorCodeValidation(localizer);
        }
    }
}
