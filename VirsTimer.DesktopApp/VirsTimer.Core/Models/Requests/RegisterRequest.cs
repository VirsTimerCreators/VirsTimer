using System.Collections.Generic;

namespace VirsTimer.Core.Models.Requests
{
    /// <summary>
    /// Server register request model.
    /// </summary>
    public class RegisterRequest
    {
        private const string UserRole = "ROLE_USER";

        private static readonly IReadOnlyList<string> NewUserRoles = new[] { UserRole };

        /// <summary>
        /// Username.
        /// </summary>
        public string? Username { get; init; }

        /// <summary>
        /// Email.
        /// </summary>
        public string? Email { get; init; }

        /// <summary>
        /// User roles.
        /// </summary>
        public IReadOnlyList<string> Roles { get; } = NewUserRoles;

        /// <summary>
        /// Password.
        /// </summary>
        public string? Password { get; init; }
    }
}