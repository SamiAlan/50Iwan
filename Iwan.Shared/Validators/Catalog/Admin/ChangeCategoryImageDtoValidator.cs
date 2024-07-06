using FluentValidation;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Catalog.Admin
{
    public class ChangeCategoryImageDtoValidator : AbstractValidator<ChangeCategoryImageDto>
    {
        public ChangeCategoryImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(i => i.Image)
                .Cascade(CascadeMode.Stop)
                .CustomImageNotNullValidation(localizer);
        }
    }
}
