using System.Collections.Generic;

namespace VirsTimer.Core.Constants
{
    /// <summary>
    /// Known WCA events.
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// 2x2x2.
        /// </summary>
        public const string TwoByTwo = "2x2x2";

        /// <summary>
        /// 3x3x3.
        /// </summary>
        public const string ThreeByThree = "3x3x3";

        /// <summary>
        /// 4x4x4.
        /// </summary>
        public const string FourByFour = "4x4x4";

        /// <summary>
        /// 5x5x5.
        /// </summary>
        public const string FiveByFive = "5x5x5";

        /// <summary>
        /// 6x6x6.
        /// </summary>
        public const string SixBySix = "6x6x6";

        /// <summary>
        /// 7x7x7.
        /// </summary>
        public const string SevenBySeven = "7x7x7";

        /// <summary>
        /// 3x3x3 blindfolded.
        /// </summary>
        public const string ThreeByThreeBlindfold = "3x3x3 BlindFolded";

        /// <summary>
        /// 3x3x3 one handed.
        /// </summary>
        public const string ThreeByThreeOneHand = "3x3x3 OH";

        /// <summary>
        /// Clock.
        /// </summary>
        public const string Clock = "Clock";

        /// <summary>
        /// Megaminx.
        /// </summary>
        public const string Megaminx = "Megaminx";

        /// <summary>
        /// Pyraminx.
        /// </summary>
        public const string Pyraminx = "Pyraminx";

        /// <summary>
        /// SKEWB.
        /// </summary>
        public const string Skewb = "Skewb";

        /// <summary>
        /// Square-1.
        /// </summary>
        public const string SquareOne = "Square One";

        /// <summary>
        /// 4x4x4 blindfolded.
        /// </summary>
        public const string FourByFourBlindfold = "4x4x4 BlindFolded";

        /// <summary>
        /// 5x5x5 blindfolded.
        /// </summary>
        public const string FiveByFiveBlindfold = "5x5x5 BlindFolded";

        /// <summary>
        /// All predefined events.
        /// </summary>
        public static readonly IReadOnlyList<string> Predefined = new[]
        {
                ThreeByThree,
                TwoByTwo,
                FourByFour,
                FiveByFive,
                SixBySix,
                SevenBySeven,
                Megaminx,
                Pyraminx,
                Skewb,
                SquareOne,
                Clock,
                ThreeByThreeOneHand,
                ThreeByThreeBlindfold,
                FourByFourBlindfold,
                FiveByFiveBlindfold
        };
    }
}