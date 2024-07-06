using Aspose.Zip.Rar;
using System.Collections.Generic;

namespace Iwan.Server.Infrastructure.Files
{
    public class ProductAsRar
    {
        public RarArchiveEntry MainImage { get; set; }
        public RarArchiveEntry Details { get; set; }
        public List<RarArchiveEntry> Images { get; set; } = new();
    }
}
