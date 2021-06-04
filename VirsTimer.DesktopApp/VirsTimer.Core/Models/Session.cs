using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models
{
    /// <summary>
    /// Model representing session.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Session event.
        /// </summary>
        [JsonIgnore]
        public Event Event { get; set; } = null!;

        /// <summary>
        /// Session id.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Session name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        public Session(Event @event, string id, string name)
        {
            Event = @event;
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Session"/> class.
        /// </summary>
        public Session(Event @event, string name)
        {
            Event = @event;
            Name = name;
        }
    }
}
