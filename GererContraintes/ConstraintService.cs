namespace DefaultNamespace
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class ConstraintService
    {
        private static readonly Regex EmailRegex = new Regex(@"^\S+@\S+\.\S+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return EmailRegex.IsMatch(email.Trim());
        }

        public static bool IsValidPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            // Accept 8 digits (local) or 11 digits starting with 509
            if (digits.Length == 8) return true;
            if (digits.Length == 11 && digits.StartsWith("509")) return true;
            return false;
        }

        public static string FormatPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return "(vide)";
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length == 8)
            {
                return $"+509 {digits.Substring(0, 4)}-{digits.Substring(4, 2)}-{digits.Substring(6, 2)}";
            }
            if (digits.Length == 11 && digits.StartsWith("509"))
            {
                var body = digits.Substring(3);
                return $"+509 {body.Substring(0, 4)}-{body.Substring(4, 2)}-{body.Substring(6, 2)}";
            }
            if (digits.Length > 0) return "+" + digits;
            return "(vide)";
        }
    }
}
