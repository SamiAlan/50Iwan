namespace Iwan.Shared.Dtos.Accounts
{
    public class RefreshUserTokenDto
    {
        /// <summary>
        /// The token string
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The refresh token string
        /// </summary>
        public string RefreshToken { get; set; }

        public RefreshUserTokenDto(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
