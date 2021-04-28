using System;
using System.IO;

namespace VirsTimer.Core.Constants
{
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
        /// Full path to application common directory path.
        /// </summary>
        public static string CommonDirectoryPath => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                Company,
                Name);
    }

}