using FluentValidation;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Compositions.Admin
{
    public class ChangeCompositionImageDtoValidator : AbstractValidator<ChangeCompositionImageDto>
    {
        public ChangeCompositionImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(i => i.Image)
                .Cascade(CascadeMode.Stop)
                .CustomImageNotNullValidation(localizer);
        }
    }
}
