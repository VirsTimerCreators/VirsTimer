using System.Text.Json;

namespace VirsTimer.Core.Constants
{
    /// <summary>
    /// Constatns for json type files.
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Server serializer options.
        /// </summary>
        public static readonly JsonSerializerOptions ServerSerializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        /// <summary>
        /// Empty json object.
        /// </summary>
        public const string EmptyObject = "{}";

        /// <summary>
        /// Empty json array.
        /// </summary>
        public const string EmptyArray = "[]";
    }
}