using Iwan.Server.Infrastructure.Files;
using Aspose.Zip.Rar;

namespace Iwan.Server.Services.Common
{
    public interface IRarFileService
    {
        ProductAsRar GetProductAsRar(RarArchive archive);
    }
}
