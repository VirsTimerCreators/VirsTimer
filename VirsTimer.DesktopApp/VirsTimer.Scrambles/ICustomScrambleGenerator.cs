namespace VirsTimer.Scrambles
{
    /// <summary>
    /// Intefrace that enables implementing scramble generators for custom events.
    /// </summary>
    public interface ICustomScrambleGenerator
    {
        /// <summary>
        /// Event name that must match name given in main VirsTimer application.
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// Generates scramble for event.
        /// </summary>
        public Scramble GenerateScramble();
    }
}