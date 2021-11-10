using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Authorization
{
    /// <summary>
    /// Standard <see cref="IUserClient"/> implementation.
    /// </summary>
    public class UserClient : IUserClient
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [JsonPropertyName("accessToken")]
        public string Jwt { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserClient"/> class.
        /// </summary>
        [JsonConstructor]
        public UserClient(string id, string jwt)
        {
            Id = id;
            Jwt = jwt;
        }
    }
}