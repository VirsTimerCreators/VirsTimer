namespace VirsTimer.Core.Models.Responses
{
    public class ErrorResponse
    {
        public string Message { get; }

        public ErrorResponse(string message)
        {
            if (message.Length >= 7 && message[0..7] == "Error: ")
                message = message[7..];
            Message = message;
        }
    }
}
