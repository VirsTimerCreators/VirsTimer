using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Requests
{
    internal class EventPatchRequest
    {
        [JsonPropertyName("puzzleType")]
        public string Name { get; init; } = string.Empty;

        public EventPatchRequest(string name)
        {
            Name = name;
        }
    }
}
