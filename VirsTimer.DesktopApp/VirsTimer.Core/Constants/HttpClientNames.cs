namespace VirsTimer.Core.Constants
{
    /// <summary>
    /// Names for defined <see cref="System.Net.Http.HttpClient"/>.
    /// </summary>
    public static class HttpClientNames
    {
        /// <summary>
        /// Blank/Default <see cref="System.Net.Http.HttpClient"/>. 
        /// </summary>
        public const string Blank = "";

        /// <summary>
        /// <see cref="System.Net.Http.HttpClient"/> with user authorization header. 
        /// </summary>
        public const string UserAuthorized = "user";

        /// <summary>
        /// <see cref="System.Net.Http.HttpClient"/> with application authorization header. 
        /// </summary>
        public const string ApplicationAuthorized = "application";
    }
}