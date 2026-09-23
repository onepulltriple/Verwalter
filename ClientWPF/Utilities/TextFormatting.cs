using System.Globalization;

namespace AnwendungWPF.Utilities
{
    public static class TextFormatting
    {
        public static string ToTitleCase(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var textInfo = CultureInfo.CurrentCulture.TextInfo;

            return textInfo.ToTitleCase(
                value.Trim().ToLower(CultureInfo.CurrentCulture));
        }
    }
}