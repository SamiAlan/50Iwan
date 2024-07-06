using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Accounts;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Accounts
{
    [Injected(ServiceLifetime.Scoped, typeof(IAccountService))]
    public class AccountService : IAccountService
    {
        protected readonly HttpClient _client;

        public AccountService(IHttpClientFactory factory)
        {
            _client = factory.CreateClient(HttpClientsNames.Base);
        }

        public async Task<PagedDto<UserDto>> GetUsersAsync(GetUsersOptions options, CancellationToken cancellationToken = default)
        {
            var request = PrepareGetRequest(options, Routes.Api.Admin.Accounts.BASE);

            return await _client.SendAndReturnOrThrowAsync<PagedDto<UserDto>, ApiErrorResponse>
                (request, cancellationToken);
        }

        public async Task<UserDto> GetCurrentUserAsync(CancellationToken cancellationToken = default)
        {
            return await _client.GetAndReturnOrThrowAsync<UserDto, ApiErrorResponse>
                (Routes.Api.Admin.Accounts.Profile, cancellationToken);
        }

        public async Task<UserDto> AddUserAsync(AddUserDto adminUser, CancellationToken cancellationToken = default)
        {
            return await _client.PostAndReturnOrThrowAsync<UserDto, ApiErrorResponse>
                (Routes.Api.Admin.Accounts.AddAdminUser, adminUser, cancellationToken);
        }

        public async Task<UserDto> UpdateProfileAsync(UpdateProfileDto profile, CancellationToken cancellationToken = default)
        {
            return await _client.PutAndReturnOrThrowAsync<UserDto, ApiErrorResponse>
                (Routes.Api.Admin.Accounts.Profile, profile, cancellationToken);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto password, CancellationToken cancellationToken = default)
        {
            await _client.SendOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Accounts.ChangePassword, password, HttpMethod.Put, cancellationToken);
        }

        public async Task DeleteUserAsync(string id, CancellationToken cancellationToken = default)
        {
            await _client.DeleteOrThrowAsync<ApiErrorResponse>
                (Routes.Api.Admin.Accounts.Delete.ReplaceRouteParameters(id), cancellationToken);
        }

        #region Helpers

        private static HttpRequestMessage PrepareGetRequest(GetUsersOptions options, string url)
        {
            string queryParameters = options.ToQueryStringParameters();

            if (!queryParameters.IsNullOrEmptyOrWhiteSpaceSafe()) url += "?" + queryParameters;
            
            return new HttpRequestMessage(HttpMethod.Get, url);
        }

        #endregion
    }
}
