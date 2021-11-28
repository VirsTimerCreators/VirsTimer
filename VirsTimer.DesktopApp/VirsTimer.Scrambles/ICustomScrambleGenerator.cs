namespace VirsTimer.Scrambles
{
    public interface ICustomScrambleGenerator
    {
        public string EventName { get; }
        public Scramble GenerateScramble();
    }
}