using System.Collections.Generic;

namespace VirsTimer.Core.Services.Cache
{
    /// <summary>
    /// Standard <see cref="IApplicationCache"/> implementation.
    /// </summary>
    public class ApplicationCache : IApplicationCache
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IDictionary<string, string> EventsById { get; init; } = new Dictionary<string, string>();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IDictionary<string, IDictionary<string, string>> SessionsByEventId { get; init; } = new Dictionary<string, IDictionary<string, string>>();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string LastChoosenEvent { get; set; } = string.Empty;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string LastChoosenSession { get; set; } = string.Empty;
    }
}