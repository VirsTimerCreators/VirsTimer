using System.Text.Json.Serialization;
using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Models
{
    /// <summary>
    /// Represents scramble.
    /// </summary>
    public class Scramble
    {
        private string _value = string.Empty;

        /// <summary>
        /// Value of scramble.
        /// </summary>
        [JsonPropertyName("scramble")]
        public string Value
        {
            get => _value;
            init => _value = Regexes.WhiteSpaces.Replace(value, " ");
        }

        /// <summary>
        /// Svg generating scramble image.
        /// </summary>
        [JsonPropertyName("svgTag")]
        public string? Svg { get; init; }
    }
}
