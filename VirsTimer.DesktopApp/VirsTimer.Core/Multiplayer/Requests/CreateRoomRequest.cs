using VirsTimer.Core.Constants;

namespace VirsTimer.Core.Multiplayer.Requests
{
    internal class CreateRoomRequest
    {
        public int NumberOfScrambles { get; }
        public string ScrambleType { get; }

        public CreateRoomRequest(int scramblesAmount, string eventName)
        {
            NumberOfScrambles = scramblesAmount;
            ScrambleType = Server.Events.GetServerEventName(eventName);
        }
    }
}