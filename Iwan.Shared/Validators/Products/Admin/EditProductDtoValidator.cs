using FluentValidation;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Products.Admin
{
    public class EditProductDtoValidator : AbstractValidator<EditProductDto>
    {
        public EditProductDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(p => p.ArabicName)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceArabicNameValidation(localizer);

            RuleFor(p => p.EnglishName)
                .Cascade(CascadeMode.Stop)
                .CustomNotEmptyOrWhitespaceEnglishNameValidation(localizer);

            RuleFor(p => p.Age)
                .Cascade(CascadeMode.Stop)
                .CustomShouldBePositiveValidation(localizer);

            RuleFor(p => p.Number)
               .Cascade(CascadeMode.Stop)
               .CustomShouldBePositiveValidation(localizer);

            RuleFor(p => p.Price)
                .Cascade(CascadeMode.Stop)
                .CustomShouldBePositiveValidation(localizer);

            RuleFor(p => p.StockQuantity)
                .Cascade(CascadeMode.Stop)
                .CustomShouldBePositiveValidation(localizer);

            //RuleFor(p => p.Weight)
            //    .Cascade(CascadeMode.Stop)
            //    .CustomShouldBePositiveValidation(localizer);

            //RuleFor(p => p.Width)
            //    .Cascade(CascadeMode.Stop)
            //    .CustomShouldBePositiveValidation(localizer);

            //RuleFor(p => p.Height)
            //    .Cascade(CascadeMode.Stop)
            //    .CustomShouldBePositiveValidation(localizer);

            //RuleFor(p => p.Length)
            //    .Cascade(CascadeMode.Stop)
            //    .CustomShouldBePositiveValidation(localizer);

            //RuleFor(p => p.Depth)
            //    .Cascade(CascadeMode.Stop)
            //    .CustomShouldBePositiveValidation(localizer);

            //RuleFor(p => p.BaseDiameter)
            //    .Cascade(CascadeMode.Stop)
            //    .CustomShouldBePositiveValidation(localizer);

            //RuleFor(p => p.NozzleDiameter)
            //    .Cascade(CascadeMode.Stop)
            //    .CustomShouldBePositiveValidation(localizer);

            RuleFor(p => p.ColorCode)
                .Cascade(CascadeMode.Stop)
                .CustomColorCodeValidation(localizer)
                .When(c => c.ColorTypeId == (int)ColorType.Custom);
        }
    }
}
