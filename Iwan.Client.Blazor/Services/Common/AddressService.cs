using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Common;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Common
{
    [Injected(ServiceLifetime.Scoped, typeof(IAddressService))]
    public class AddressService : IAddressService
    {
        protected readonly HttpClient _client;

        public AddressService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<AddressDto> EditAddressAsync(EditAddressDto editedAddress, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<AddressDto, ApiErrorResponse>
                (Routes.Api.Admin.Addresses.BASE, editedAddress, cancellationToken);
        }

        public async Task<AddressDto> GetAddressAsync(string addressId, CancellationToken cancellationToken = default)
        {
            var url = Routes.Api.Admin.Addresses.GetAddress.ReplaceRouteParameters(addressId);
            return await _client.GetAndReturnOrThrowAsync<AddressDto, ApiErrorResponse>(url, cancellationToken);
        }
    }
}
