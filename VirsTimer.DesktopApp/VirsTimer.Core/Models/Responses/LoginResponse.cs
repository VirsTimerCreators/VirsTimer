namespace VirsTimer.Core.Models.Responses
{
    public class LoginResponse
    {
        public bool Succesfull { get; set; }
        public string UserId { get; init; }
        public string Jwt { get; init; }
    }
}