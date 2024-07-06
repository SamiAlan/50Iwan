using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Catalog;
using Iwan.Shared.Dtos.Compositions;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Dtos.Products;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Iwan.Shared.Extensions
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<TEntity, string> CustomEmailValidation<TLocalization, TEntity>(this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotEmpty().WithMessage(localizer.Localize(ValidationResponses.Accounts.EmailRequired))
                .EmailAddress().WithMessage(localizer.Localize(ValidationResponses.Accounts.InvalidEmailAddress));

        public static IRuleBuilderOptions<TEntity, string> CustomPasswordValidation<TLocalization, TEntity>(this IRuleBuilder<TEntity, string> rule, int minLength, IStringLocalizer<TLocalization> localizer)
            => rule.NotEmpty().WithMessage(localizer.Localize(ValidationResponses.Accounts.PasswordRequired))
                .MinimumLength(minLength).WithMessage(localizer.Localize(ValidationResponses.Accounts.PasswordMinLength, minLength))
                .Must(p => p.Any(c => !char.IsLetterOrDigit(c))).WithMessage(localizer.Localize(ValidationResponses.Accounts.SpecialCharactersRequired))
                .Must(p => p.Any(c => char.IsDigit(c))).WithMessage(localizer.Localize(ValidationResponses.Accounts.DigitRequired))
                .Must(p => p.Any(c => char.IsUpper(c))).WithMessage(localizer.Localize(ValidationResponses.Accounts.UpperCharacterRequired));
        
        public static IRuleBuilderOptions<TEntity, string> CustomNotEmptyOrWhitespaceNameValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.CustomNotEmptyOrWhitespace().WithMessage(localizer.Localize(ValidationResponses.General.NameNotEmtpyOrWhitespaceRequired));

        public static IRuleBuilderOptions<TEntity, string> CustomNotEmptyOrWhitespaceValueValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.CustomNotEmptyOrWhitespace().WithMessage(localizer.Localize(ValidationResponses.General.FieldRequired));

        public static IRuleBuilderOptions<TEntity, string> CustomSyrianPhoneNumberValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.Matches(AppRegex.SyrianPhoneNumber).WithMessage(localizer.Localize(ValidationResponses.General.InvalidPhoneNumber));

        public static IRuleBuilderOptions<TEntity, string> CustomPasswordsNotEqual<TEntity, TLocalization>(this IRuleBuilder<TEntity, string> rule, Expression<Func<TEntity, string>> expression, IStringLocalizer<TLocalization> localizer)
            => rule.NotEqual(expression).WithMessage(localizer.Localize(ValidationResponses.Accounts.SamePasswords));
        
        public static IRuleBuilderOptions<TEntity, string> CustomPasswordsEqual<TEntity, TLocalization>(this IRuleBuilder<TEntity, string> rule, Expression<Func<TEntity, string>> expression, IStringLocalizer<TLocalization> localizer)
            => rule.Equal(expression).WithMessage(localizer.Localize(ValidationResponses.Accounts.PasswordsNotSame));

        public static IRuleBuilderOptions<TEntity, string> CustomNotEmptyOrWhitespaceArabicNameValidation<TEntity, TLocalization>
            (this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.CustomNotEmptyOrWhitespace().WithMessage(localizer.Localize(ValidationResponses.General.ArabicNameNotEmptyOrWhitespace));

        public static IRuleBuilderOptions<TEntity, string> CustomNotEmptyOrWhitespaceEnglishNameValidation<TEntity, TLocalization>
            (this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.CustomNotEmptyOrWhitespace().WithMessage(localizer.Localize(ValidationResponses.General.ArabicNameNotEmptyOrWhitespace));

        public static IRuleBuilderOptions<TEntity, string> CustomMaximumLength<TEntity, TLocalization>
            (this IRuleBuilder<TEntity, string> rule, int maxLength, IStringLocalizer<TLocalization> localizer)
            => rule.MaximumLength(maxLength).WithMessage(localizer.Localize(ValidationResponses.General.MaxLength, maxLength));

        public static IRuleBuilderOptions<TEntity, AddCategoryImageDto> CustomAddImageValidation<TEntity, TLocalization>
            (this IRuleBuilder<TEntity, AddCategoryImageDto> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.General.ImageRequired))
               .ChildRules(i =>
               {
                   i.RuleFor(i => i.Image).CustomImageNotNullValidation(localizer);
               });

        public static IRuleBuilderOptions<TEntity, AddCompositionImageDto> CustomAddImageValidation<TEntity, TLocalization>
            (this IRuleBuilder<TEntity, AddCompositionImageDto> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.General.ImageRequired))
               .ChildRules(i =>
               {
                   i.RuleFor(i => i.Image).CustomImageNotNullValidation(localizer);
               });

        public static IRuleBuilderOptions<TEntity, AddProductImageDto> CustomAddImageValidation<TEntity, TLocalization>
            (this IRuleBuilder<TEntity, AddProductImageDto> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.General.ImageRequired))
               .ChildRules(i =>
               {
                   i.RuleFor(i => i.Image).CustomImageNotNullValidation(localizer);
               });

        public static IRuleBuilderOptions<TEntity, AddProductMainImageDto> CustomAddImageValidation<TEntity, TLocalization>
            (this IRuleBuilder<TEntity, AddProductMainImageDto> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.General.ImageRequired))
               .ChildRules(i =>
               {
                   i.RuleFor(i => i.Image).CustomImageNotNullValidation(localizer);
               });

        public static IRuleBuilderOptions<TEntity, string> CustomColorCodeValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.Must(ValidHexColorCode).WithMessage(localizer.Localize(ValidationResponses.General.InvalidColorCode));

        public static IRuleBuilderOptions<TEntity, string> CustomSupportedImageExtensionValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, string> rule, IStringLocalizer<TLocalization> localizer)
            => rule.Must(SupportImageExtension).WithMessage(localizer.Localize(ValidationResponses.General.ImageExtensionNotSupported));

        public static IRuleBuilderOptions<TEntity, AddImageDto> CustomImageNotNullValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, AddImageDto> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.General.ImageRequired));

        public static IRuleBuilderOptions<TEntity, IFormFile> CustomImageNotNullValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, IFormFile> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.General.ImageRequired));
        
        public static IRuleBuilderOptions<TEntity, EditImageDto> CustomImageNotNullValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, EditImageDto> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.General.ImageRequired));

        public static IRuleBuilderOptions<TEntity, IList<TItem>> CustomAnyBillItemValidation<TEntity, TLocalization, TItem>(this IRuleBuilder<TEntity, IList<TItem>> rule, IStringLocalizer<TLocalization> localizer)
            => rule.NotNull().WithMessage(localizer.Localize(ValidationResponses.Bills.AtLeastOneBillItem))
                .Must(images => images.Any()).WithMessage(localizer.Localize(ValidationResponses.Bills.AtLeastOneBillItem));

        public static IRuleBuilderOptions<TEntity, int> CustomShouldBePositiveValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, int> rule, IStringLocalizer<TLocalization> localizer)
            => rule.GreaterThanOrEqualTo(0).WithMessage(localizer.Localize(ValidationResponses.General.ValueShouldBePositive));

        public static IRuleBuilderOptions<TEntity, decimal> CustomShouldBePositiveValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, decimal> rule, IStringLocalizer<TLocalization> localizer)
            => rule.GreaterThanOrEqualTo(0).WithMessage(localizer.Localize(ValidationResponses.General.ValueShouldBePositive));

        public static IRuleBuilderOptions<TEntity, decimal> CustomRangePercentValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, decimal> rule, decimal minValue, decimal maxValue, IStringLocalizer<TLocalization> localizer)
            => rule.InclusiveBetween(minValue, maxValue).WithMessage(localizer.Localize(ValidationResponses.General.ValueShouldBeBetweenRangePercent, minValue, maxValue));

        public static IRuleBuilderOptions<TEntity, float> CustomRangePercentValidation<TEntity, TLocalization>(this IRuleBuilder<TEntity, float> rule, float minValue, float maxValue, IStringLocalizer<TLocalization> localizer)
            => rule.InclusiveBetween(minValue, maxValue).WithMessage(localizer.Localize(ValidationResponses.General.ValueShouldBeBetweenRangePercent, minValue, maxValue));

        public static IRuleBuilderOptions<TEnitity, int> InEnum<TEnum, TEnitity, TLocalization>(this IRuleBuilder<TEnitity, int> rule, IStringLocalizer<TLocalization> localizer, string errorMessageKey)
            where TEnum : struct, Enum => rule.Must((value) => Enum.IsDefined(typeof(TEnum), value)).WithMessage(localizer.Localize(errorMessageKey));

        #region Helper Methods

        private static IRuleBuilderOptions<TEntity, string> CustomNotEmptyOrWhitespace<TEntity>(this IRuleBuilder<TEntity, string> rule)
            => rule.Must(NotEmptyOrWhiteSpace);

        private static bool NotEmptyOrWhiteSpace(string value) => !value.IsNullOrEmptyOrWhiteSpaceSafe();

        private static bool ValidHexColorCode(string colorCode) => colorCode.IsHtmlColor();

        private static bool SupportImageExtension(string fileName)
            => SupportedImageExtensions.IsSupported(Path.GetExtension(fileName));
        #endregion
    }
}
