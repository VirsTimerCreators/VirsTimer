namespace VirsTimer.Core.Multiplayer.Requests
{
    internal class ChangeRoomStatusRequest
    {
        public string NewStatus { get; init; } = string.Empty;

        public static ChangeRoomStatusRequest Start => new() { NewStatus = "INPROGRESS" };
        public static ChangeRoomStatusRequest Close => new() { NewStatus = "CLOSED" };
    }
}