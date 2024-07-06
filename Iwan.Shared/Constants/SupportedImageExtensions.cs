using System.Collections.Generic;

namespace Iwan.Shared.Constants
{
    public static class SupportedImageExtensions
    {
        public static List<string> Extensions = new() { ".jpg", ".JPG", ".JPEG", ".PNG", ".jpeg", ".png", ".gif", ".jfif" };

        public static bool IsSupported(string extension) => Extensions.Contains(extension);

        public static string All() => string.Join(",", Extensions);
    }
}
