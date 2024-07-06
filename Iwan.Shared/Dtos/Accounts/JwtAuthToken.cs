using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Accounts
{
    public class JwtAuthToken : IAuthToken
    {
        /// <summary>
        /// The json web token string
        /// </summary>
        [JsonPropertyName("token")]
        public string Token { get; set; }

        /// <summary>
        /// The related refresh token string
        /// </summary>
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public JwtAuthToken() { }

        /// <summary>
        /// Constructs a new instance of the <see cref="JwtAuthToken"/> class using the passed parameters
        /// </summary>
        public JwtAuthToken(string jwtToken, string refreshToken)
            => (Token, RefreshToken) = (jwtToken, refreshToken);

        public override string ToString()
        {
            return $"Token: {Token}\nRefresh: {RefreshToken}";
        }
    }
}
