using Iwan.Client.Blazor.Constants;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Client.Blazor.Services.Authentication;
using Iwan.Shared.Dtos.Accounts;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Infrastructure.Security.Providers
{
    [Injected(ServiceLifetime.Scoped, typeof(AuthenticationStateProvider))]
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        protected readonly ILocalStorageService _localStorage;
        protected readonly IAuthService _authService;

        public JwtAuthenticationStateProvider(ILocalStorageService localStorage, IAuthService authService)
        {
            _localStorage = localStorage;
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var unauthenticatedState = new AuthenticationState(new ClaimsPrincipal());

            var thereIsAToken = await _localStorage.ContainKeyAsync(Keys.AccessToken);
            if (!thereIsAToken) return unauthenticatedState;

            var tokenAsString = await _localStorage.GetItemAsStringAsync(Keys.AccessToken);
            var tokenHandler = new JwtSecurityTokenHandler();

            if (!tokenHandler.CanReadToken(tokenAsString)) return unauthenticatedState;

            var token = tokenHandler.ReadJwtToken(tokenAsString);

            if (token.ValidTo.ToLocalTime() < DateTime.Now.AddDays(1))
            {
                if (!await _localStorage.ContainKeyAsync(Keys.RefreshToken))
                    return unauthenticatedState;

                var refreshToken = await _localStorage.GetItemAsStringAsync(Keys.RefreshToken);

                try
                {
                    var authToken = await _authService.RefreshTokenAsync(new RefreshUserTokenDto(tokenAsString, refreshToken));

                    await _localStorage.SetItemAsStringAsync(Keys.AccessToken, authToken.Data.Token);
                    await _localStorage.SetItemAsStringAsync(Keys.RefreshToken, authToken.Data.RefreshToken);

                    tokenAsString = authToken.Data.Token;

                    token = tokenHandler.ReadJwtToken(tokenAsString);
                }
                catch (Exception)
                {
                    return unauthenticatedState;
                }
            }

            var authenticatedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(token.Claims, "Bearer")));
            
            NotifyAuthenticationStateChanged(Task.FromResult(authenticatedState));
            return authenticatedState;
        }
    }
}
