using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Authorization
{
    public class UserClient : IUserClient
    {
        [JsonPropertyName("id")]
        public string Id { get; init; }

        [JsonPropertyName("accessToken")]
        public string Jwt { get; set; }
    }
}
