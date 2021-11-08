namespace VirsTimer.Core.Models.Authorization
{
    /// <summary>
    /// Logged user data.
    /// </summary>
    public interface IUserClient
    {
        /// <summary>
        /// Logged user id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Logged user json web token.
        /// </summary>
        public string Jwt { get; set; }
    }
}
