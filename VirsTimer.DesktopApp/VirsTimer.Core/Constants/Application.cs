using System;
using System.IO;
using System.Reflection;

namespace VirsTimer.Core.Constants
{
    /// <summary>
    /// Application constatnts.
    /// </summary>
    public static class Application
    {
        /// <summary>
        /// Company name.
        /// </summary>
        public const string Company = "VirsTimerCreators";

        /// <summary>
        /// Application name.
        /// </summary>
        public const string Name = "VirsTimer";

        /// <summary>
        /// Full path to application data directory path.
        /// </summary>
        public static string ApplicationDirectoryPath => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Company,
                Name);

        /// <summary>
        /// Full path to documents directory path.
        /// </summary>
        public static string ApplicationDocumentsPath => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Company,
                Name);

        /// <summary>
        /// Current application directory.
        /// </summary>
        public static string? CurrentDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
    }
}