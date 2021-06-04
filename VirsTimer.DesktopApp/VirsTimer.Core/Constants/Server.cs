namespace VirsTimer.Core.Constants
{
    /// <summary>
    /// VirsTimer server side constants.
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Base address of server.
        /// </summary>
        public const string Address = "http://localhost:8080/";

        /// <summary>
        /// Scramble endpoint.
        /// </summary>
        public const string ScrambleEndpoint = "scramble/";

        /// <summary>
        /// Predefined events.
        /// </summary>
        public static class Events
        {
            /// <summary>
            /// 2x2x2.
            /// </summary>
            public const string TwoByTwo = "TWO_BY_TWO";

            /// <summary>
            /// 3x3x3.
            /// </summary>
            public const string ThreeByThree = "THREE_BY_THREE";

            /// <summary>
            /// 4x4x4.
            /// </summary>
            public const string FourByFour = "FOUR_BY_FOUR";

            /// <summary>
            /// 5x5x5.
            /// </summary>
            public const string FiveByFive = "FIVE_BY_FIVE";

            /// <summary>
            /// 6x6x6.
            /// </summary>
            public const string SixBySix = "SIX_BY_SIX";

            /// <summary>
            /// 7x7x7.
            /// </summary>
            public const string SevenBySeven = "SEVEN_BY_SEVEN";

            /// <summary>
            /// 3x3x3 blindfolded.
            /// </summary>
            public const string ThreeByThreeBlindfold = "THREE_BY_THREE_BLINDFOLDED";

            /// <summary>
            /// 3x3x3 one handed.
            /// </summary>
            public const string ThreeByThreeOneHand = "THREE_BY_THREE_OH";

            /// <summary>
            /// Clock.
            /// </summary>
            public const string Clock = "CLOCK";

            /// <summary>
            /// Megaminx.
            /// </summary>
            public const string Megaminx = "MEGAMINX";

            /// <summary>
            /// Pyraminx.
            /// </summary>
            public const string Pyraminx = "PYRAMINX";

            /// <summary>
            /// Skweb.
            /// </summary>
            public const string Skewb = "SKWEB";

            /// <summary>
            /// Square-1.
            /// </summary>
            public const string Square_One = "SQUARE_ONE";

            /// <summary>
            /// 4x4x4 blindfolded.
            /// </summary>
            public const string FourByFourBlindfold = "FOUR_BY_FOUR_BLINDFOLDED";

            /// <summary>
            /// 5x5x5 blindfolded.
            /// </summary>
            public const string FiveByFiveBlindfold = "FIVE_BY_FIVE_BLINDFOLDED";

            /// <summary>
            /// All predefined events.
            /// </summary>
            public static readonly string[] All = new[]
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
                Square_One,
                Clock,
                ThreeByThreeOneHand,
                ThreeByThreeBlindfold,
                FourByFourBlindfold,
                FiveByFiveBlindfold,
            };
        }
    }
}
