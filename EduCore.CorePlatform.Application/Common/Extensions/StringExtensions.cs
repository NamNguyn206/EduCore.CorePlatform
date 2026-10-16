using System;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EduCore.CorePlatform.Application.Common.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNotNullOrEmpty(this string value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static bool IsNotNullOrWhiteSpace(this string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        if (maxLength <= suffix.Length)
            return value.Substring(0, maxLength);

        return value.Substring(0, maxLength - suffix.Length) + suffix;
    }

    public static string ToCamelCase(this string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 2)
            return value;

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }

    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 2)
            return value;

        return char.ToUpperInvariant(value[0]) + value.Substring(1);
    }

    public static string ToSnakeCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return Regex.Replace(value, @"([a-z0-9])([A-Z])", "$1_$2").ToLowerInvariant();
    }

    public static string ToKebabCase(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return Regex.Replace(value, @"([a-z0-9])([A-Z])", "$1-$2").ToLowerInvariant();
    }

    public static string RemoveDiacritics(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var normalizedString = value.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    public static string ToUrlSlug(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Remove diacritics
        var slug = value.RemoveDiacritics().ToLowerInvariant();

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");

        // Remove invalid characters
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        // Remove duplicate hyphens
        slug = Regex.Replace(slug, @"-+", "-");

        // Trim hyphens from start and end
        slug = slug.Trim('-');

        return slug;
    }

    public static string MaskEmail(this string email)
    {
        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            return email;

        var parts = email.Split('@');
        if (parts.Length != 2)
            return email;

        var username = parts[0];
        var domain = parts[1];

        if (username.Length <= 2)
            return $"{username}***@{domain}";

        var maskedUsername = username.Substring(0, 2) +
                           new string('*', Math.Max(0, username.Length - 2));

        return $"{maskedUsername}@{domain}";
    }

    public static string MaskPhoneNumber(this string phoneNumber, int visibleDigits = 4)
    {
        if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length <= visibleDigits)
            return new string('*', phoneNumber.Length);

        var maskedPart = new string('*', phoneNumber.Length - visibleDigits);
        var visiblePart = phoneNumber.Substring(phoneNumber.Length - visibleDigits);

        return maskedPart + visiblePart;
    }

    public static string FirstCharToUpper(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return char.ToUpper(value[0]) + value.Substring(1);
    }

    public static string Reverse(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        var charArray = value.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public static bool ContainsIgnoreCase(this string source, string value)
    {
        if (source == null || value == null)
            return false;

        return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    public static bool EqualsIgnoreCase(this string source, string value)
    {
        if (source == null && value == null)
            return true;

        if (source == null || value == null)
            return false;

        return source.Equals(value, StringComparison.OrdinalIgnoreCase);
    }

    public static string ExtractNumbers(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return new string(value.Where(char.IsDigit).ToArray());
    }

    public static string ExtractLetters(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return new string(value.Where(char.IsLetter).ToArray());
    }

    public static string ToMd5Hash(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = md5.ComputeHash(inputBytes);

            var stringBuilder = new StringBuilder();
            foreach (var b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }

    public static string ToBase64(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    public static string FromBase64(this string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
            return string.Empty;

        try
        {
            var bytes = Convert.FromBase64String(base64String);
            return Encoding.UTF8.GetString(bytes);
        }
        catch
        {
            return string.Empty;
        }
    }

    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPhoneNumber(this string phoneNumber, string pattern = @"^[0-9\-\+\(\)\s]+$")
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        return Regex.IsMatch(phoneNumber, pattern);
    }

    public static string[] SplitLines(this string value, StringSplitOptions options = StringSplitOptions.None)
    {
        if (string.IsNullOrEmpty(value))
            return Array.Empty<string>();

        return value.Split(new[] { "\r\n", "\r", "\n" }, options);
    }

    public static string ToCurrencyFormat(this decimal value, string cultureCode = "vi-VN")
    {
        var culture = new CultureInfo(cultureCode);
        return value.ToString("C", culture);
    }

    public static string ToCurrencyFormat(this string value, string cultureCode = "vi-VN")
    {
        if (decimal.TryParse(value, out var decimalValue))
        {
            return decimalValue.ToCurrencyFormat(cultureCode);
        }
        return value;
    }

    public static string RemoveWhitespace(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return Regex.Replace(value, @"\s+", "");
    }

    public static string RemoveSpecialCharacters(this string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        return Regex.Replace(value, @"[^a-zA-Z0-9\s]", "");
    }

    public static int WordCount(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return 0;

        var words = value.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        return words.Length;
    }

    public static string LimitWords(this string value, int maxWords, string suffix = "...")
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        var words = value.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length <= maxWords)
            return value;

        return string.Join(" ", words.Take(maxWords)) + suffix;
    }
}
