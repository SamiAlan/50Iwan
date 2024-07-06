using Microsoft.AspNetCore.Http;

namespace Iwan.Shared.Dtos.Media
{
    public class UploadTempImageDto
    {
        public IFormFile Image { get; set; }
    }
}
