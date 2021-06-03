namespace VirsTimer.Core.Models
{
    /// <summary>
    /// Model representing cube event.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Event name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        public Event(string name)
        {
            Name = name;
        }
    }
}
