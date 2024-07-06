using System;
using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos
{
    public abstract class TimedDto
    {
        [JsonPropertyName("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonPropertyName("updatedDate")]
        public DateTime UpdatedDate { get; set; }
    }
}
