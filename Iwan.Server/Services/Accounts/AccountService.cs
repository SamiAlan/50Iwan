using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Users;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Infrastructure.Security.TokenProviders;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Accounts
{
    [Injected(ServiceLifetime.Scoped, typeof(IAccountService))]
    public class AccountService : IAccountService
    {
        /// <summary>
        /// Manger used to do operations on the <see cref="AppUser"/> class
        /// </summary>
        protected readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// The authorization token provider used to provide auth tokens
        /// </summary>
        protected readonly IAuthorizationTokenProvider _tokenProvider;

        /// <summary>
        /// Application context used to do database operations
        /// </summary>
        protected readonly IUnitOfWork _context;

        /// <summary>
        /// The logged in user service
        /// </summary>
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        /// <summary>
        /// The validation parameters used to validate the authenticity of the expired jwt token
        /// </summary>
        protected readonly TokenValidationParameters _validationParameters;

        public AccountService(UserManager<AppUser> userManager,
            IAuthorizationTokenProvider tokenProvider, IUnitOfWork context,
            TokenValidationParameters validationParameters, ILoggedInUserProvider loggedInUserProvider)
        {
            _userManager = userManager;
            _tokenProvider = tokenProvider;
            _context = context;
            _validationParameters = validationParameters;
            _loggedInUserProvider = loggedInUserProvider;
        }

        public async Task<AppUser> AddUserAsync(AddUserDto userToAdd, string roleName, CancellationToken cancellationToken = default)
        {
            // Get the user by email
            var userByEmail = await _userManager.FindByEmailAsync(userToAdd.Email);

            // If the user is not found
            if (userByEmail is not null)
                throw new AlreadyExistException(message: Responses.Accounts.AlreadyExist,
                    userToAdd.GetPropertyPath(u => u.Email));

            // Instantiate a new AppUser and trim it
            var user = new AppUser
            {
                Name = userToAdd.Name,
                Email = userToAdd.Email,
                UserName = userToAdd.Email
            };

            // Add the new user
            await _userManager.CreateAsync(user, userToAdd.Password);

            await _userManager.AddToRoleAsync(user, roleName);

            return user;
        }

        public async Task<IAuthToken> LoginAsync(LoginDto loginCredentials, CancellationToken cancellationToken = default)
        {
            // Get the user by username (since the username in our case is the same as the email)
            var user = await _userManager.FindByNameAsync(loginCredentials.Email);

            // if the user is not found
            if (user is null)
                throw new NotFoundException(Responses.Accounts.InvalidLogin);

            if (!await _userManager.IsInRoleAsync(user, Roles.SuperAdmin) &&
                !await _userManager.IsInRoleAsync(user, Roles.Admin))
                throw new BadRequestException(Responses.Accounts.SystemUserLoginOnly);

            // If the password is incorrect
            if (!await _userManager.CheckPasswordAsync(user, loginCredentials.Password))
                throw new BadRequestException(Responses.Accounts.InvalidLogin);

            // If the user has not confirmed his email
            //if (!user.EmailConfirmed)
            //    throw new BadRequestException(Responses.Account.EmailNotVerified);

            // Generate and return an auth token
            return await _tokenProvider.GenerateAuthTokenForUserAsync(user, cancellationToken);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto, CancellationToken cancellationToken = default)
        {
            var currentUser = await _loggedInUserProvider.GetCurrentUser();

            if (!await _userManager.CheckPasswordAsync(currentUser, changePasswordDto.OldPassword))
                throw new BadRequestException(Responses.Accounts.IncorrectPassword,
                    changePasswordDto.GetPropertyPath(p => p.OldPassword));

            if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
                throw new BadRequestException(Responses.Accounts.SamePasswords,
                    changePasswordDto.GetPropertyPath(p => p.NewPassword));

            await _userManager.ChangePasswordAsync(currentUser, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        }

        public async Task UpdateProfileAsync(UpdateProfileDto profile, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(new object[] { _loggedInUserProvider.UserId }, cancellationToken);

            if (user == null)
                throw new NotFoundException(Responses.Accounts.UserNotFound);

            user.Name = profile.Name;

            await _context.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task<IAuthToken> RefreshTokenAsync(RefreshUserTokenDto refreshUserToken, CancellationToken cancellationToken = default)
        {
            // Get the principal user from the token
            var principal = GetPrincipalFromToken(refreshUserToken.Token);

            // If the token has some errors
            if (principal is null)
                throw new UnAuthorizedException(Responses.Accounts.InvalidRefresh);

            // Get the jwt id for the principal
            var jti = principal.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            // Get the refresh token
            var refreshTokenFromDB = await _context.RefreshTokensRepository.SingleOrDefaultAsync(r => r.Token == refreshUserToken.RefreshToken, cancellationToken: cancellationToken);

            // If token not found or it is expired or it is invalidated or it has been used before or belongs to a different jwt id
            if (refreshTokenFromDB is null || DateTime.UtcNow > refreshTokenFromDB.ExpiryDate ||
                refreshTokenFromDB.Invalidated || refreshTokenFromDB.Used || refreshTokenFromDB.Jid != jti)
                throw new UnAuthorizedException(Responses.Accounts.InvalidRefresh);

            // Mark the refresh token as used
            refreshTokenFromDB.Used = true;

            // Save changes to the db
            await _context.SaveChangesAsync(cancellationToken: cancellationToken);

            // Get the user according to the id contained in the principal
            var user = await _userManager.FindByIdAsync(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            // Generate a new auth token for the retrieved user
            return await _tokenProvider.GenerateAuthTokenForUserAsync(user, cancellationToken);
        }

        public async Task DeleteUserAsync(string id, CancellationToken cancellationToken = default)
        {
            if (id == _loggedInUserProvider.UserId)
                throw new ConflictException(Responses.Accounts.CantDeleteSelf);

            if (!_loggedInUserProvider.IsInRole(Roles.SuperAdmin))
                throw new UnAuthorizedException();

            var userToDelete = await _context.Users.FindAsync(new object[] { id }, cancellationToken: cancellationToken);

            // TODO: Check if the user has permission to delete the intended user

            await _userManager.DeleteAsync(userToDelete);
        }

        #region Helpers

        /// <summary>
        /// Gets a principal user from the jwt token
        /// </summary>
        /// <param name="token">The jwt token to get the principal user from</param>
        protected ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            try
            {
                // Validate token according to the validation parameters and create a principal user
                var principal =
                    new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out var validatedToken);

                // return a principal or null for whether the jwt token has the correct security algorithm
                return IsJwtWithValidSecurityAlgorithm(validatedToken) ? principal : null;
            }
            catch
            {
                // Return null in case there is any error
                return null;
            }
        }

        /// <summary>
        /// Checks if the security token has the correct security algorithm
        /// </summary>
        /// <param name="securityToken">Security token to check</param>
        protected bool IsJwtWithValidSecurityAlgorithm(SecurityToken securityToken)
        {
            // If it is a jwt token and has the correct algorithm
            return securityToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    System.StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion
    }
}
