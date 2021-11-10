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
        public string? Id { get; set; }
 
        /// <summary>
        /// Event name.
        /// </summary>
        [JsonPropertyName("puzzleType")]
        public string Name { get; set; }
 
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        [JsonConstructor]
        public Event(string id, string name)
        {
            Id = id;
            Name = name;
        }
 
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event(string name)
        {
            Name = name;
        }
    }
}

