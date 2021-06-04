namespace VirsTimer.Core.Models
{
    public class LoginResponse
    {
        public bool Succesfull { get; set; }
        public string UserId { get; init; }
        public string Jwt { get; init; }
    }
}