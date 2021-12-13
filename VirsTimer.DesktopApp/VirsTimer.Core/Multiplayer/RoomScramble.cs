namespace VirsTimer.Core.Multiplayer
{
    public class RoomScramble
    {
        public string Id { get; }
        public string Value { get; }
        public string Svg { get; }

        public RoomScramble(string id, string value, string svg)
        {
            Id = id;
            Value = value;
            Svg = svg;
        }
    }
}