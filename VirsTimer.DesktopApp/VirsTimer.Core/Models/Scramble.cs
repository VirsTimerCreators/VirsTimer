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
 
        public static Scramble Empty { get; } = new Scramble
        {
            Svg = "<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 130 98\" width=\"130px\" version=\"1.1\" height=\"98px\"></svg>",
            _value = "Brak generatora scrambli dla danego eventu"
        };
 
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
