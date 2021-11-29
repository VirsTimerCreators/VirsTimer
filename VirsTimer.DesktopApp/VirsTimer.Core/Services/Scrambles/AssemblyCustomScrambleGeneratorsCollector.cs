using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using VirsTimer.Core.Constants;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Services.Scrambles
{
    /// <summary>
    /// <see cref="ICustomScrambleGeneratorsCollector"/> that gathers <see cref="ICustomScrambleGenerator"/> from ScrambleGenerators.dll file.
    /// </summary>
    public class AssemblyCustomScrambleGeneratorsCollector : ICustomScrambleGeneratorsCollector
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IReadOnlyList<ICustomScrambleGenerator> GetCustomScrambleGenerators() => _customScrambleGeneratorsLazy.Value;

        private readonly Lazy<IReadOnlyList<ICustomScrambleGenerator>> _customScrambleGeneratorsLazy = new(() =>
        {
            try
            {
                var result = new List<ICustomScrambleGenerator>();

                var type = typeof(ICustomScrambleGenerator);
                var assemblyPath = Path.Combine(Application.CurrentDirectory, "ScrambleGenerators.dll");
                var k = File.Exists(assemblyPath);
                var generatorsAssembly = Assembly.LoadFile(assemblyPath);
                var generatorsTypes = generatorsAssembly.GetTypes()
                    .Where(p => type.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                    .ToList();

                foreach (var generatorType in generatorsTypes)
                {
                    var instance = CreateInstance(generatorType);
                    if (instance is not null)
                        result.Add(instance);
                }

                return result;
            }
            catch
            {
                return Array.Empty<ICustomScrambleGenerator>();
            }
        });

        private static ICustomScrambleGenerator? CreateInstance(Type type)
        {
            try
            {
                return (ICustomScrambleGenerator?)Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }
    }
}