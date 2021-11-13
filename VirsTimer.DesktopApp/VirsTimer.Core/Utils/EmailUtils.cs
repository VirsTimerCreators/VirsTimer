using System.Net.Mail;

namespace VirsTimer.Core.Utils
{
    /// <summary>
    /// Email utilities.
    /// </summary>
    public static class EmailUtils
    {
        /// <summary>
        /// Checks is given <paramref name="email"/> is valid email address.
        /// </summary>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            if (email.Trim().EndsWith("."))
                return false;
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}