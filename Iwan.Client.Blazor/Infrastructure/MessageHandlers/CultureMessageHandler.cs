using Iwan.Client.Blazor.Constants;
using Iwan.Shared.Constants;
using Blazored.LocalStorage;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Infrastructure.MessageHandlers
{
    public class CultureMessageHandler : DelegatingHandler
    {
        protected readonly ILocalStorageService _storage;

        public CultureMessageHandler(ILocalStorageService storage)
        {
            _storage = storage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var currentCultureCode = await _storage.GetItemAsStringAsync(Keys.CultureCode);

            if (string.IsNullOrEmpty(currentCultureCode) || !AppLanguages.All().Contains(currentCultureCode))
            {
                currentCultureCode = AppLanguages.English.Culture;
            }

            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue(currentCultureCode));
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
