using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Vendors;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Vendors
{
    [Injected(ServiceLifetime.Scoped, typeof(IVendorService))]
    public class VendorService : IVendorService
    {
        protected readonly HttpClient _client;

        public VendorService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<VendorDto> AddVendorAsync(AddVendorDto vendorToAdd, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<VendorDto, ApiErrorResponse>(Routes.Api.Admin.Vendors.BASE, vendorToAdd, cancellationToken);
        }

        public async Task DeleteVendorAsync(string vendorId, CancellationToken cancellationToken = default)
        {
            var url = Routes.Api.Admin.Vendors.Delete.ReplaceRouteParameters(vendorId);

            await _client.DeleteOrThrowAsync<ApiErrorResponse>(url, cancellationToken);
        }

        public async Task<VendorDto> EditVendorAsync(EditVendorDto editedVendor, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<VendorDto, ApiErrorResponse>(Routes.Api.Admin.Vendors.BASE, editedVendor, cancellationToken);
        }

        public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<List<VendorDto>, ApiErrorResponse>
                (Routes.Api.Admin.Vendors.GetAll, cancellationToken);
        }

        public async Task<PagedDto<VendorDto>> GetVendorsAsync(GetVendorsOptions options, CancellationToken cancellationToken = default)
        {
            var request = PrepareRequest(options, Routes.Api.Admin.Vendors.BASE);

            return await _client.SendAndReturnOrThrowAsync<PagedDto<VendorDto>, ApiErrorResponse>(request, cancellationToken);
        }

        public async Task<VendorDto> GetVendorAsync(string vendorId, CancellationToken cancellationToken = default)
        {
            var url = Routes.Api.Admin.Vendors.GetVendor.ReplaceRouteParameters(vendorId);

            return await _client.GetAndReturnOrThrowAsync<VendorDto, ApiErrorResponse>(url, cancellationToken);
        }

        #region Helpers

        private static HttpRequestMessage PrepareRequest(GetVendorsOptions options, string url)
        {
            var queryStrings = options.ToQueryStringParameters();

            if (!queryStrings.IsNullOrEmptyOrWhiteSpaceSafe()) url += $"?{queryStrings}";

            return new HttpRequestMessage(HttpMethod.Get, url);
        }
        
        #endregion
    }
}
