using System.Collections.Generic;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services.Scrambles
{
    public interface ICustomScrambleGeneratorsCollector
    {
        public IReadOnlyList<ICustomScrambleGenerator> GetCustomScrambleGenerators();
    }
}
