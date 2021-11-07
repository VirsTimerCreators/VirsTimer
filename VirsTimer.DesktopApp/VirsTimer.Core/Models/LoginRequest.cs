namespace VirsTimer.Core.Models
{
    public class LoginRequest
    {
        public string Name { get; }
        public string Password { get; }
 
        public LoginRequest(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}