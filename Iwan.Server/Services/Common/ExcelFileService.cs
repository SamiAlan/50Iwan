using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using System;
using System.IO;
using System.Collections.Generic;

namespace Iwan.Server.Services.Common
{
    [Injected(ServiceLifetime.Scoped, typeof(IExcelFileService))]
    public class ExcelFileService : IExcelFileService
    {
        public AddProductDto GetProductDetailsFromExcelFile(Stream excelFileStream)
        {
            var addProductDto = new AddProductDto
            {
                ColorType = ColorType.FromParent,
                IsVisible = false
            };

            using (var package = new ExcelPackage(excelFileStream))
            {
                var workSheet = package.Workbook.Worksheets[0];
                var table = (object[,])workSheet.Cells[1, 1, workSheet.Dimension.Rows, workSheet.Dimension.Columns].Value;
                addProductDto.Number = Convert.ToInt32(table[ProductExcelConsts.Number.RowIndex, ProductExcelConsts.Number.ColumnIndex].ToString().Trim());
                addProductDto.ArabicName = table[ProductExcelConsts.ArabicName.RowIndex, ProductExcelConsts.ArabicName.ColumnIndex].ToString().Trim();
                addProductDto.EnglishName = table[ProductExcelConsts.EnglishName.RowIndex, ProductExcelConsts.EnglishName.ColumnIndex].ToString().Trim();
                addProductDto.ArabicDescription = table[ProductExcelConsts.ArabicDescription.RowIndex, ProductExcelConsts.ArabicDescription.ColumnIndex].ToString();
                addProductDto.EnglishDescription = table[ProductExcelConsts.EnglishDescription.RowIndex, ProductExcelConsts.EnglishDescription.ColumnIndex]?.ToString();
                addProductDto.MakerEnglishName= table[ProductExcelConsts.MakerEnglishName.RowIndex, ProductExcelConsts.MakerEnglishName.ColumnIndex]?.ToString().Trim();
                addProductDto.MakerArabicName = table[ProductExcelConsts.MakerArabicName.RowIndex, ProductExcelConsts.MakerArabicName.ColumnIndex]?.ToString().Trim();
                addProductDto.Age= table[ProductExcelConsts.Age.RowIndex, ProductExcelConsts.Age.ColumnIndex]?.ToString().Trim().ParseOrDefault<int>() ?? 0;
                addProductDto.StockQuantity= table[ProductExcelConsts.StockQuantity.RowIndex, ProductExcelConsts.StockQuantity.ColumnIndex]?.ToString().Trim().ParseOrDefault<int>() ?? 0;
                addProductDto.Price = table[ProductExcelConsts.Price.RowIndex, ProductExcelConsts.Price.ColumnIndex]?.ToString().Trim().ParseOrDefault<decimal>() ?? 0;
                addProductDto.DimensionsInArabic = table[ProductExcelConsts.DimensionsInArabic.RowIndex, ProductExcelConsts.DimensionsInArabic.ColumnIndex]?.ToString();
                addProductDto.DimensionsInEnglish = table[ProductExcelConsts.DimensionsInEnglish.RowIndex, ProductExcelConsts.DimensionsInEnglish.ColumnIndex]?.ToString();
                //addProductDto.Weight= table[ProductExcelConsts.Weight.RowIndex, ProductExcelConsts.Weight.ColumnIndex].ToString().Trim().ParseOrDefault<double>();
                //addProductDto.Length= table[ProductExcelConsts.Length.RowIndex, ProductExcelConsts.Length.ColumnIndex].ToString().Trim().ParseOrDefault<double>();
                //addProductDto.Width= table[ProductExcelConsts.Width.RowIndex, ProductExcelConsts.Width.ColumnIndex].ToString().Trim().ParseOrDefault<double>();
                //addProductDto.Height= table[ProductExcelConsts.Height.RowIndex, ProductExcelConsts.Height.ColumnIndex].ToString().Trim().ParseOrDefault<double>();
                //addProductDto.Depth= table[ProductExcelConsts.Depth.RowIndex, ProductExcelConsts.Depth.ColumnIndex].ToString().Trim().ParseOrDefault<double>();
                //addProductDto.BaseDiameter= table[ProductExcelConsts.BaseDiameter.RowIndex, ProductExcelConsts.BaseDiameter.ColumnIndex].ToString().Trim().ParseOrDefault<double>();
                //addProductDto.NozzleDiameter = table[ProductExcelConsts.NozzleDiameter.RowIndex, ProductExcelConsts.NozzleDiameter.ColumnIndex].ToString().Trim().ParseOrDefault<double>();

                var states = new List<AddProductStateDto>();

                for(int i = 0; true; i++)
                {
                    try
                    {
                        var stateArabicName = table[ProductExcelConsts.ArabicStates.RowIndex, ProductExcelConsts.ArabicStates.ColumnIndex + i]?.ToString().Trim();

                        var stateEnglishName = table[ProductExcelConsts.EnglishStates.RowIndex, ProductExcelConsts.EnglishStates.ColumnIndex + i]?.ToString().Trim();

                        if (stateArabicName.IsNullOrEmptyOrWhiteSpaceSafe() || stateArabicName.IsNullOrEmptyOrWhiteSpaceSafe())
                            break;

                        states.Add(new AddProductStateDto
                        {
                            ArabicName = stateArabicName,
                            EnglishName = stateEnglishName
                        });
                    }
                    catch
                    {
                        break;
                    }
                }

                addProductDto.States = states;
            }

            return addProductDto;
        }
    }
}
