﻿using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Security;
using Iwan.Server.Domain.Users;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Options;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Infrastructure.Security.TokenProviders
{
    /// <summary>
    /// Represents a jwt token provider
    /// </summary>
    [Injected(ServiceLifetime.Scoped, typeof(IAuthorizationTokenProvider))]
    public class JwtAuthorizationTokenProvider : IAuthorizationTokenProvider
    {
        /// <summary>
        /// The auth options to be used for generating the auth token
        /// </summary>
        protected readonly AuthTokenOptions _authOptions;

        /// <summary>
        /// The db context to be used to db operations
        /// </summary>
        protected readonly IUnitOfWork _context;

        /// <summary>
        /// The factory used to create the <see cref="ClaimsPrincipal"/> instance
        /// </summary>
        protected readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;

        /// <summary>
        /// Constructs a new instance of the <see cref="JwtAuthorizationTokenProvider"/> class using the passed parameters
        /// </summary>
        public JwtAuthorizationTokenProvider(AuthTokenOptions authOptions,
            IUnitOfWork context,
            IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory)
        {
            _authOptions = authOptions;
            _context = context;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        /// <summary>
        /// Generates an <see cref="IAuthToken"/> for the passed user
        /// </summary>
        public async Task<IAuthToken> GenerateAuthTokenForUserAsync(AppUser user, CancellationToken token = default)
        {
            // Get the current utc time
            var utcNow = DateTime.UtcNow;

            // Create a jwt security token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create the list of claims to be used for the token creation
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Claims.EmailConfirmed, user.EmailConfirmed.ToString())
            };

            // Create the principal from the user
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            // Add the claims to the principal
            claims.AddRange(principal.Claims);

            // Create the lits of claim names to be removed
            var claimsToRemove = new List<string>
            {
                ClaimTypes.Name,
                Claims.SecurityStamp
            };

            // Remove claims in the remove list
            foreach (var claimName in claimsToRemove)
                claims.RemoveAll(c => c.Type.Equals(claimName));

            // Create the jwt token
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                expires: utcNow.Add(_authOptions.JwtExpiryTime),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(_authOptions.Key.ToUTF8Bytes()), SecurityAlgorithms.HmacSha256));

            // Generate the refresh token
            var refreshToken = new RefreshToken
            {
                Jid = jwtToken.Id,
                UserId = user.Id,
                ExpiryDate = utcNow.Add(_authOptions.RefreshTokenExpiryTime),
            };

            // Save the refresh token
            await _context.RefreshTokensRepository.AddAsync(refreshToken);

            // Save changes to the database
            await _context.SaveChangesAsync(cancellationToken: token);

            // Return a new bearer token
            return new JwtAuthToken(jwtToken: tokenHandler.WriteToken(jwtToken), refreshToken: refreshToken.Token);
        }
    }
}
