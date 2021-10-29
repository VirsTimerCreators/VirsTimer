using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Requests
{
    public class LoginRequest
    {
        [JsonPropertyName("username")]
        public string Name { get; }

        [JsonPropertyName("password")]
        public string Password { get; }

        public LoginRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}