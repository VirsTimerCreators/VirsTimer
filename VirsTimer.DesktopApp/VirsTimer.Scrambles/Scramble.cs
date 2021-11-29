using System.Text.Json.Serialization;

namespace VirsTimer.Scrambles
{
    /// <summary>
    /// Represents scramble.
    /// </summary>
    public class Scramble
    {
        /// <summary>
        /// Value of scramble.
        /// </summary>
        [JsonPropertyName("scramble")]
        public string Value { get; init; } = string.Empty;
 
        /// <summary>
        /// Svg generating scramble image.
        /// </summary>
        [JsonPropertyName("svgTag")]
        public string? Svg { get; init; }
    }
}