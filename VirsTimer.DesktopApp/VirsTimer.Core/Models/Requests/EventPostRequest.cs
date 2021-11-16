using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Requests
{
    internal class EventPostRequest
    {
        [JsonPropertyName("puzzleType")]
        public string Name { get; init; } = string.Empty;

        public EventPostRequest(string name)
        {
            Name = name;
        }
    }
}