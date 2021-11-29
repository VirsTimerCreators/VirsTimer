using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VirsTimer.Core.Naming
{
    /// <summary>
    /// Generates names based on given prefix.
    /// </summary>
    public class NameGenerator
    {
        private readonly string _prefix;
        private readonly Regex _regex;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameGenerator"/> class.
        /// </summary>
        public NameGenerator(string prefix)
        {
            _prefix = prefix;
            _regex = new Regex($"{_prefix}([0-9]+)", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Generates name with next number.
        /// </summary>
        public string GenerateNext(IEnumerable<string> taken)
        {
            var s = taken
                .Select(single => _regex.Match(single))
                .ToList();


            var maxEventNumber = taken
                .Select(single => _regex.Match(single))
                .Where(match => match.Success)
                .Select(match => int.Parse(match.Groups[1].Value))
                .DefaultIfEmpty(1)
                .Max() + 1;

            return _prefix + maxEventNumber;
        }
    }
}