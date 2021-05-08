using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.Core.Services
{
    public class RandomScrambleGenerator : IScrambleGenerator
    {
        public Task<IReadOnlyList<Scramble>> GenerateScrambles(Event @event, int scramblesAmount)
        {
            var result = new List<Scramble>();
            for (var i = 0; i < scramblesAmount; i++)
                result.Add(new Scramble(Generate3x3x3Scramble()));

            return Task.FromResult<IReadOnlyList<Scramble>>(result);
        }

        public static string Generate3x3x3Scramble()
        {
            var result = new string[20];

            var allMoves = Enum.GetValues(typeof(CubeMove)).Cast<CubeMove>().Except(new[] { CubeMove.None }).ToArray();
            var allAdditions = Enum.GetValues(typeof(Addition)).Cast<Addition>().ToArray();
            var lastMove = CubeMove.None;

            for (var i = 0; i < 20; i++)
            {
                var randomMove = GetRandomItem(allMoves.Except(new[] { lastMove }).ToArray());
                var randomAddition = GetRandomAdditionString(GetRandomItem(allAdditions));
                result[i] = randomMove + randomAddition;
                lastMove = randomMove;
            }

            return string.Join(" ", result);

            static string GetRandomAdditionString(Addition addition)
            {
                return addition switch
                {
                    Addition.Empty => string.Empty,
                    Addition.Double => "2",
                    Addition.Prim => "'",
                    _ => throw new NotImplementedException()
                };
            }
        }

        private static T GetRandomItem<T>(IReadOnlyList<T> list)
        {
            var random = new Random();
            var itemIndex = random.Next(list.Count);
            return list[itemIndex];
        }

        private enum CubeMove
        {
            R,
            L,
            U,
            D,
            F,
            B,
            None
        }

        private enum Addition
        {
            Empty,
            Prim,
            Double
        }
    }
}
