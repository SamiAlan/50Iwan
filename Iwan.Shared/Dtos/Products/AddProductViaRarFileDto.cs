using Microsoft.AspNetCore.Http;

namespace Iwan.Shared.Dtos.Products
{
    public class AddProductViaRarFileDto
    {
        public IFormFile RarFile { get; set; }
    }
}
