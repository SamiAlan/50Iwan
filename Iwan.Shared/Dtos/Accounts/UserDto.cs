using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.Accounts
{
    public class UserDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("canDelete")]
        public bool CanDelete { get; set; }
    }
}
