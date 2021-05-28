namespace VirsTimer.Core.Models
{
    public class Scramble
    {
        public string Value { get; }
        public string? Svg { get; }

        public Scramble(string value)
        {
            Value = value;
        }

        public Scramble(string value, string svg)
            : this(value)
        {
            Svg = svg;
        }
    }
}
