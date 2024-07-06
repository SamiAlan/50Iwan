using Aspose.Zip.Rar;
using FluentValidation;
using FluentValidation.Results;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using System.IO;
using System.Linq;

namespace Iwan.Shared.Validators.Products.Admin
{
    public class AddProductViaRarFileDtoValidator : AbstractValidator<AddProductViaRarFileDto>
    {
        private readonly IStringLocalizer<Localization> _localizer;
        public AddProductViaRarFileDtoValidator(IStringLocalizer<Localization> localizer)
        {
            _localizer = localizer;

            RuleFor(p => p.RarFile)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage(localizer.Localize(ValidationResponses.Products.RarFileRequired))
                .Must(f => Path.GetExtension(f.FileName).Equals(".rar"))
                .WithMessage(localizer.Localize(ValidationResponses.General.OnlyRarFilesSupported))
                .Custom((file, context) =>
                {
                    using (var archive = new RarArchive(file.OpenReadStream()))
                    {
                        if (!HasMainImageFile(archive))
                            context.AddFailure(new ValidationFailure(nameof(AddProductViaRarFileDto.RarFile),
                                localizer.Localize(ValidationResponses.Products.MainImageNotFound)));

                        if (!HasDetailsFileAsync(archive))
                        {
                            context.AddFailure(new ValidationFailure(nameof(AddProductViaRarFileDto.RarFile),
                                localizer.Localize(ValidationResponses.Products.DetailsFileNotFound)));

                            return;
                        }

                        MakeSureDetailsFileHasAllInfoRequired(archive, context);
                    }
                });
        }

        private static bool HasMainImageFile(RarArchive archive)
        {
            return archive.Entries.Any(e => e.Name.Contains(ProductRarFileNames.MainImage));
        }

        private static bool HasDetailsFileAsync(RarArchive archive)
        {
            return archive.Entries.Any(e => e.Name.Contains(ProductRarFileNames.Details));
        }

        private void MakeSureDetailsFileHasAllInfoRequired(RarArchive archive, ValidationContext<AddProductViaRarFileDto> context)
        {
            using (var package = new ExcelPackage(archive.Entries.First(e => e.Name.Contains(ProductRarFileNames.Details)).Open()))
            {
                var worksheet = package.Workbook.Worksheets[0];

                var table = (object[,])worksheet.Cells[1, 1, worksheet.Dimension.Rows, worksheet.Dimension.Columns].Value;
                var firstColumn = table.GetColumn(0);

                if (!firstColumn.Contains(ProductExcelConsts.Number.Text))
                    context.AddFailure(new ValidationFailure(nameof(AddProductViaRarFileDto.RarFile),
                        _localizer.Localize(ValidationResponses.Products.DetailsFileHasNoNumber)));

                if (!firstColumn.Contains(ProductExcelConsts.ArabicName.Text))
                    context.AddFailure(new ValidationFailure(nameof(AddProductViaRarFileDto.RarFile),
                        _localizer.Localize(ValidationResponses.Products.DetailsFileHasNoArabicName)));

                if (!firstColumn.Contains(ProductExcelConsts.EnglishName.Text))
                    context.AddFailure(new ValidationFailure(nameof(AddProductViaRarFileDto.RarFile),
                        _localizer.Localize(ValidationResponses.Products.DetailsFileHasNoEnglishName)));
            }
        }
    }
}
