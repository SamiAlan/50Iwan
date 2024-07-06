using System.Text.Json.Serialization;

namespace Iwan.Shared.Dtos.RealTime
{
    public class EntityImagesManipulationInitializationDto
    {
        [JsonPropertyName("numberOfEntitiesToProcess")]
        public int NumberOfEntitiesToProcess { get; set; }
    }

    public class EntityImagesManipulationProgressDto
    {
        [JsonPropertyName("numberOfEntitiesToProcess")]
        public int NumberOfEntitiesToProcess { get; set; }

        [JsonPropertyName("doneEntities")]
        public int DoneEntities { get; set; }

        [JsonPropertyName("entitiesLeft")]
        public int EntitiesLeft { get; set; }
    }
}
