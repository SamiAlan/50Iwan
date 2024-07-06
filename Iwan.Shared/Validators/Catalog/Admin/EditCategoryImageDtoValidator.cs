using FluentValidation;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Catalog.Admin
{
    public class EditCategoryImageDtoValidator : AbstractValidator<EditCategoryImageDto>
    {
        public EditCategoryImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(c => c.ColorCode)
                .Cascade(CascadeMode.Stop)
                .CustomColorCodeValidation(localizer)
                .When(c => !c.ColorCode.IsNullOrEmptyOrWhiteSpaceSafe());
        }
    }
}
