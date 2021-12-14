using System.Text.Json.Serialization;

namespace VirsTimer.Core.Multiplayer
{
    public class RoomScramble
    {
        public string Id { get; init; } = string.Empty;

        [JsonPropertyName("scramble")]
        public string Value { get; init; } = string.Empty;

        [JsonPropertyName("scrambleSvg")]
        public string Svg { get; init; } = string.Empty;
    }
}