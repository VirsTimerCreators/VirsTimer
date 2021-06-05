using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models
{
    /// <summary>
    /// Model representing cube event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Event id.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// Event name.
        /// </summary>
        [JsonPropertyName("puzzleType")]
        public string Name { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
