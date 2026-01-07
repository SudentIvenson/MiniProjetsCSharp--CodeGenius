namespace DefaultNamespace
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class FormattingService
    {
        private static readonly Regex EmailRegex = new Regex(@"^\S+@\S+\.\S+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email.Trim());
        }

        public static string FormatPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return "(vide)";
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            // Client enters xxxx-xx-xx (8 digits), we add +509 prefix
            if (digits.Length == 8)
            {
                return $"+509 {digits.Substring(0, 4)}-{digits.Substring(4, 2)}-{digits.Substring(6, 2)}";
            }
            // If user included country code (509 + 8 digits = 11)
            if (digits.Length == 11 && digits.StartsWith("509"))
            {
                var body = digits.Substring(3);
                return $"+509 {body.Substring(0, 4)}-{body.Substring(4, 2)}-{body.Substring(6, 2)}";
            }
            // Fallback: show digits with + prefix
            if (digits.Length > 0) return "+" + digits;
            return "(vide)";
        }
    }
}
