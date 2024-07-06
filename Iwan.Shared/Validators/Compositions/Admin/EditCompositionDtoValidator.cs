﻿using FluentValidation;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Compositions.Admin
{
    public class EditCompositionDtoValidator : AbstractValidator<EditCompositionDto>
    {
        public EditCompositionDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(c => c.ArabicName)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceArabicNameValidation(localizer);

            RuleFor(c => c.EnglishName)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceEnglishNameValidation(localizer);

            RuleFor(c => c.ColorCode)
                .Cascade(CascadeMode.Stop)
                .CustomColorCodeValidation(localizer)
                .When(c => c.ColorTypeId == (int)ColorType.Custom);
        }
    }
}
