using Iwan.Client.Blazor.Constants;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Common;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Services.Authentication
{
    [Injected(ServiceLifetime.Scoped, typeof(IAuthService))]
    public class AuthService : IAuthService
    {
        protected readonly HttpClient _client;

        public AuthService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient(HttpClientsNames.Auth);
        }

        public async Task<ApiResponse<JwtAuthToken>> LoginAsync(LoginDto loginCredentials, CancellationToken cancellationToken = default)
        {
            var requestJson = loginCredentials.ToJson();

            var response = await _client.PostAsync(Routes.Api.Admin.Accounts.Login, new StringContent(requestJson, Encoding.UTF8, MediaTypes.Json), cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnAuthorizedException();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = (await response.Content.ReadAsStringAsync(cancellationToken)).ToObject<ApiErrorResponse>();
                throw new BaseException(errorResponse.Message, response.StatusCode);
            }
            
            var authToken = (await response.Content.ReadAsStringAsync(cancellationToken)).ToObject<JwtAuthToken>();
            return new ApiResponse<JwtAuthToken>(authToken);
        }

        public async Task<ApiResponse<JwtAuthToken>> RefreshTokenAsync(RefreshUserTokenDto refreshToken, CancellationToken cancellationToken = default)
        {
            var requestJson = refreshToken.ToJson();

            var response = await _client.PostAsync(Routes.Api.Admin.Accounts.RefreshToken, new StringContent(requestJson, Encoding.UTF8, MediaTypes.Json), cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnAuthorizedException();

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = (await response.Content.ReadAsStringAsync(cancellationToken)).ToObject<ApiErrorResponse>();
                throw new BaseException(errorResponse.Message, response.StatusCode);
            }

            var authToken = (await response.Content.ReadAsStringAsync(cancellationToken)).ToObject<JwtAuthToken>();
            return new ApiResponse<JwtAuthToken>(authToken);
        }
    }
}
