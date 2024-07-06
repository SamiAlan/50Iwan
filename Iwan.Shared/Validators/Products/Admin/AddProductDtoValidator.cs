using FluentValidation;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace Iwan.Shared.Validators.Products.Admin
{
    public class AddProductDtoValidator : AbstractValidator<AddProductDto>
    {
        public AddProductDtoValidator(IStringLocalizer<Localization> localizer)
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

            RuleFor(p => p.ColorCode)
                .Cascade(CascadeMode.Stop)
                .CustomColorCodeValidation(localizer)
                .When(c => c.ColorTypeId == (int)ColorType.Custom);

            RuleFor(p => p.ColorTypeId)
                .InEnum<ColorType,AddProductDto,Localization>(localizer, ValidationResponses.General.InvalidColorType);

            RuleFor(p => p.MainImage)
                .Cascade(CascadeMode.Stop)
                .CustomAddImageValidation(localizer)
                .When(p => p.IsVisible);

            RuleForEach(p => p.Images)
                .Cascade(CascadeMode.Stop)
                .CustomAddImageValidation(localizer)
                .When(p => p.Images != null && p.Images.Any());

            RuleForEach(p => p.States)
                .SetValidator(new AddProductStateDtoValidator(localizer))
                .When(p => p.States != null && p.States.Any());
        }
    }
}
