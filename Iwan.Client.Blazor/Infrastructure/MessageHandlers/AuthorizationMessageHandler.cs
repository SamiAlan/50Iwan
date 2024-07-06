using Iwan.Client.Blazor.Constants;
using Blazored.LocalStorage;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Infrastructure.MessageHandlers
{
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        protected readonly ILocalStorageService _storage;

        public AuthorizationMessageHandler(ILocalStorageService storage)
        {
            _storage = storage;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var unauthorizedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            if (!await _storage.ContainKeyAsync(Keys.AccessToken))
                return unauthorizedResponse;

            var tokenAsString = await _storage.GetItemAsStringAsync(Keys.AccessToken);

            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(tokenAsString))
                return unauthorizedResponse;

            var token = tokenHandler.ReadJwtToken(tokenAsString);

            if (token.ValidTo.ToLocalTime() < DateTime.Now)
                return unauthorizedResponse;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenAsString);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
