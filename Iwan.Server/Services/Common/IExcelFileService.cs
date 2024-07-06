using Iwan.Shared.Dtos.Products;
using System.IO;

namespace Iwan.Server.Services.Common
{
    public interface IExcelFileService
    {
        AddProductDto GetProductDetailsFromExcelFile(Stream excelFileStream);
    }
}
