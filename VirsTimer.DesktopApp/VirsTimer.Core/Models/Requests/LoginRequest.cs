using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Requests
{
    /// <summary>
    /// Server login request.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Login name.
        /// </summary>
        [JsonPropertyName("username")]
        public string Name { get; }

        /// <summary>
        /// Login password.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequest"/> class.
        /// </summary>
        public LoginRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}