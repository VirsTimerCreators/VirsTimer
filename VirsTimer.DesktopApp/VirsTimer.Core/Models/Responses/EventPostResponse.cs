using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Responses
{
    internal class EventPostResponse
    {
        public string Id { get; init; } = string.Empty;

        public string UserId { get; init; } = string.Empty;

        [JsonPropertyName("puzzleType")]
        public string Name { get; init; } = string.Empty;
    }
}