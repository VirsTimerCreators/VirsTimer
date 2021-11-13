namespace VirsTimer.Core.Models.Responses
{
    /// <summary>
    /// Repository response handled status.
    /// </summary>
    public enum RepositoryResponseStatus
    {
        /// <summary>
        /// Status ok.
        /// </summary>
        Ok,

        /// <summary>
        /// Error occured on client application side.
        /// </summary>
        ClientError,

        /// <summary>
        /// Error occured on repository error side.
        /// </summary>
        RepositoryError,

        /// <summary>
        /// Network problem occured.
        /// </summary>
        NetworkError,

        /// <summary>
        /// Unknown problem occured.
        /// </summary>
        UnknownError
    }
}