using System.Net.Mail;

namespace VirsTimer.Core.Utils
{
    public static class EmailUtils
    {
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