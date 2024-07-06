using Aspose.Zip.Rar;
using Iwan.Server.Infrastructure.Files;
using Iwan.Shared.Constants;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Iwan.Server.Services.Common
{
    [Injected(ServiceLifetime.Scoped, typeof(IRarFileService))]
    public class RarFileService : IRarFileService
    {
        public ProductAsRar GetProductAsRar(RarArchive archive)
        {
            RarArchiveEntry mainImage = null;
            RarArchiveEntry details = null;
            var images = new List<RarArchiveEntry>();

            foreach (var entry in archive.Entries)
            {
                if (entry.Name.Contains(ProductRarFileNames.MainImage))
                    mainImage = entry;
                else if (entry.Name.Contains(ProductRarFileNames.Details))
                    details = entry;
                else
                    images.Add(entry);
            }

            return new ProductAsRar
            {
                Details = details,
                MainImage = mainImage,
                Images = images
            };
        }
    }
}
