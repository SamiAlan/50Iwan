using Microsoft.Extensions.Localization;
using System.Linq;

namespace Iwan.Shared.Extensions
{
    public static class StringLocalizerExtensions
    {
        public static string Localize<T>(this IStringLocalizer<T> localizer, string key, params object[] values)
        {
            if (values?.Any() ?? false)
                return localizer[key, values].Value;

            return localizer[key].Value;
        }
    }
}
