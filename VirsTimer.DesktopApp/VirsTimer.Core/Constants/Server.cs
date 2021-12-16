using System;
using System.Collections.Generic;
using System.IO;

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
        //public const string Address = "https://virstimer-deployment-test-1-tgo2vtbivq-lm.a.run.app/";

        /// <summary>
        /// Server endpoints.
        /// </summary>
        public static class Endpoints
        {
            /// <summary>
            /// Events resource endpoints.
            /// </summary>
            public static class Event
            {
                /// <summary>
                /// GET all events.
                /// </summary>
                public static string Get => EventsResource;

                /// <summary>
                /// POST event.
                /// </summary>
                public static string Post => EventsResource;

                /// <summary>
                /// PATCH event by id.
                /// </summary>
                public static string Patch(string eventId) => Path.Combine(EventsResource, eventId);

                /// <summary>
                /// DELETE event by id.
                /// </summary>
                public static string Delete(string eventId) => Path.Combine(EventsResource, eventId);
            }


            /// <summary>
            /// Sessions resource endpoints.
            /// </summary>
            public static class Session
            {
                /// <summary>
                /// GET session by event id.
                /// </summary>
                public static string GetByEvent(string eventId) => Path.Combine(SessionsResource, EventsResource, eventId);

                /// <summary>
                /// POST session.
                /// </summary>
                public static string Post => SessionsResource;

                /// <summary>
                /// PATCH session by id.
                /// </summary>
                public static string Patch(string sessionId) => Path.Combine(SessionsResource, sessionId);

                /// <summary>
                /// DELETE session by id.
                /// </summary>
                public static string Delete(string sessionId) => Path.Combine(SessionsResource, sessionId);
            }

            /// <summary>
            /// Solves resource endpoints.
            /// </summary>
            public static class Solve
            {
                /// <summary>
                /// GET solve by session id.
                /// </summary>
                public static string GetBySession(string sessionId) => Path.Combine(SolvesResource, SessionsResource, sessionId);

                /// <summary>
                /// POST solve.
                /// </summary>
                public static string Post => SolvesResource;

                /// <summary>
                /// POST solves.
                /// </summary>
                public static string PostMany => Path.Combine(SolvesResource, "many");

                /// <summary>
                /// PATCH solve by id.
                /// </summary>
                public static string Patch(string solveId) => Path.Combine(SolvesResource, solveId);

                /// <summary>
                /// DELETE solve by id.
                /// </summary>
                public static string Delete(string solveId) => Path.Combine(SolvesResource, solveId);
            }

            /// <summary>
            /// Scrambles resource endpoints.
            /// </summary>
            public static class Scramble
            {
                /// <summary>
                /// GET scramble by event name.
                /// </summary>
                public static string Get(string eventName, int amount = 1) => Path.Combine(ScramblesResource, $"{eventName}?amount={amount}");
            }

            /// <summary>
            /// Rooms resource endpoints.
            /// </summary>
            public static class Room
            {
                /// <summary>
                /// POST room.
                /// </summary>
                public static string Post => RoomsResource;

                /// <summary>
                /// POST room join.
                /// </summary>
                public static string Join => Path.Combine(RoomsResource, "join");

                /// <summary>
                /// POST room leave.
                /// </summary>
                public static string Leave => Path.Combine(RoomsResource, "leave");

                /// <summary>
                /// PATCH room by id.
                /// </summary>
                public static string Patch(string roomId) => Path.Combine(RoomsResource, roomId);

                /// <summary>
                /// POST solve to room.
                /// </summary>
                public static string PostSolve(string roomId) => Path.Combine(RoomsResource, roomId, "solve");

                /// <summary>
                /// POST register.
                /// </summary>
                public static string Notifications(string roomId) => Path.Combine(RoomsResource, roomId, "feed");
            }

            /// <summary>
            /// Auth resource endpoints.
            /// </summary>
            public static class Auth
            {
                /// <summary>
                /// POST login.
                /// </summary>
                public static string Login => Path.Combine(AuthResource, "signin");

                /// <summary>
                /// POST register.
                /// </summary>
                public static string Register => Path.Combine(AuthResource, "signup");
            }

            /// <summary>
            /// Events endpoint.
            /// </summary>
            private const string EventsResource = "event/";

            /// <summary>
            /// Sessions endpoint.
            /// </summary>
            private const string SessionsResource = "session/";

            /// <summary>
            /// Solves endpoint.
            /// </summary>
            private const string SolvesResource = "solve/";

            /// <summary>
            /// Scrambles endpoint.
            /// </summary>
            private const string ScramblesResource = "scramble/";

            /// <summary>
            /// Auth endpoint.
            /// </summary>
            private const string AuthResource = "api/auth/";

            /// <summary>
            /// Rooms endpoint.
            /// </summary>
            private const string RoomsResource = "room/";
        }

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
            /// Skewb.
            /// </summary>
            public const string Skewb = "SKEWB";

            /// <summary>
            /// Square-1.
            /// </summary>
            public const string SquareOne = "SQUARE_ONE";

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
            public static readonly IReadOnlyList<string> All = new[]
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

            /// <summary>
            /// Gets server event name.
            /// </summary>
            /// <exception cref="ArgumentException"></exception>
            public static string GetServerEventName(string eventName)
            {
                return eventName switch
                {
                    Constants.Events.TwoByTwo => TwoByTwo,
                    Constants.Events.ThreeByThree => ThreeByThree,
                    Constants.Events.FourByFour => FourByFour,
                    Constants.Events.FiveByFive => FiveByFive,
                    Constants.Events.SixBySix => SixBySix,
                    Constants.Events.SevenBySeven => SevenBySeven,
                    Constants.Events.Pyraminx => Pyraminx,
                    Constants.Events.Megaminx => Megaminx,
                    Constants.Events.Skewb => Skewb,
                    Constants.Events.SquareOne => SquareOne,
                    Constants.Events.Clock => Clock,
                    Constants.Events.ThreeByThreeBlindfold => ThreeByThreeBlindfold,
                    Constants.Events.ThreeByThreeOneHand => ThreeByThreeOneHand,
                    Constants.Events.FourByFourBlindfold => FourByFourBlindfold,
                    Constants.Events.FiveByFiveBlindfold => FiveByFiveBlindfold,
                    _ => eventName
                };
            }

            /// <summary>
            /// Gets application event name.
            /// </summary>
            /// <exception cref="ArgumentException"></exception>
            public static string GetEventName(string serverEventName)
            {
                return serverEventName switch
                {
                    TwoByTwo => Constants.Events.TwoByTwo,
                    ThreeByThree => Constants.Events.ThreeByThree,
                    FourByFour => Constants.Events.FourByFour,
                    FiveByFive => Constants.Events.FiveByFive,
                    SixBySix => Constants.Events.SixBySix,
                    SevenBySeven => Constants.Events.SevenBySeven,
                    Pyraminx => Constants.Events.Pyraminx,
                    Megaminx => Constants.Events.Megaminx,
                    Skewb => Constants.Events.Skewb,
                    SquareOne => Constants.Events.SquareOne,
                    Clock => Constants.Events.Clock,
                    ThreeByThreeBlindfold => Constants.Events.ThreeByThreeBlindfold,
                    ThreeByThreeOneHand => Constants.Events.ThreeByThreeOneHand,
                    FourByFourBlindfold => Constants.Events.FourByFourBlindfold,
                    FiveByFiveBlindfold => Constants.Events.FiveByFiveBlindfold,
                    _ => serverEventName
                };
            }
        }
    }
}