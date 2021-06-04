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
        public string Id { get; }

        /// <summary>
        /// Event name.
        /// </summary>
        public string Name { get; }

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
