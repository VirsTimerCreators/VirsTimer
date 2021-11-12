using System.Text.Json.Serialization;

namespace VirsTimer.Core.Models.Responses
{
    /// <summary>
    /// Error response from the server.
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Message of the error.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
        /// </summary>
        [JsonConstructor]
        public ErrorResponse(string message)
        {
            if (message.Length >= 7 && message[0..7] == "Error: ")
                message = message[7..];
            Message = message;
        }
    }
}