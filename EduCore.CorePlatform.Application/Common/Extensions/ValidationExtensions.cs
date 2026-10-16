using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace EduCore.CorePlatform.Application.Common.Extensions;
public static class ValidationExtensions
{
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

    public static bool IsValidUrl(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return false;

        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }

    public static bool IsValidIpAddress(this string ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            return false;

        return System.Net.IPAddress.TryParse(ipAddress, out _);
    }

    public static bool IsValidGuid(this string guidString)
    {
        if (string.IsNullOrWhiteSpace(guidString))
            return false;

        return Guid.TryParse(guidString, out _);
    }

    public static bool IsValidDateTime(this string dateTimeString)
    {
        if (string.IsNullOrWhiteSpace(dateTimeString))
            return false;

        return DateTime.TryParse(dateTimeString, out _);
    }

    public static bool IsValidDate(this string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return false;

        return DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out _) ||
               DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _);
    }

    public static bool IsValidTime(this string timeString)
    {
        if (string.IsNullOrWhiteSpace(timeString))
            return false;

        return TimeSpan.TryParse(timeString, out _);
    }

    public static bool IsValidJson(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        json = json.Trim();
        if ((json.StartsWith("{") && json.EndsWith("}")) ||
            (json.StartsWith("[") && json.EndsWith("]")))
        {
            try
            {
                System.Text.Json.JsonDocument.Parse(json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        return false;
    }

    public static bool IsValidXml(this string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return false;

        try
        {
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(xml);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidBase64(this string base64String)
    {
        if (string.IsNullOrWhiteSpace(base64String))
            return false;

        base64String = base64String.Trim();
        if (base64String.Length % 4 != 0)
            return false;

        try
        {
            Convert.FromBase64String(base64String);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool IsValidPassword(this string password, int minLength = 8)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < minLength)
            return false;

        // At least one uppercase letter
        if (!password.Any(char.IsUpper))
            return false;

        // At least one lowercase letter
        if (!password.Any(char.IsLower))
            return false;

        // At least one digit
        if (!password.Any(char.IsDigit))
            return false;

        // At least one special character
        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            return false;

        return true;
    }

    public static bool IsValidUsername(this string username, int minLength = 3, int maxLength = 50)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        if (username.Length < minLength || username.Length > maxLength)
            return false;

        // Only allow letters, digits, underscores, and hyphens
        var regex = new Regex(@"^[a-zA-Z0-9_-]+$");
        return regex.IsMatch(username);
    }

    public static bool IsValidVietnamesePhoneNumber(this string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        // Remove all non-digit characters
        var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

        // Vietnamese phone numbers start with 0 or +84
        if (digits.StartsWith("0"))
            digits = "84" + digits.Substring(1);

        if (!digits.StartsWith("84"))
            return false;

        // Check length (without country code)
        var nationalNumber = digits.Substring(2);
        if (nationalNumber.Length != 9 && nationalNumber.Length != 10)
            return false;

        // Check mobile prefixes
        var mobilePrefixes = new[] { "32", "33", "34", "35", "36", "37", "38", "39",
                                        "70", "76", "77", "78", "79",
                                        "81", "82", "83", "84", "85", "86", "87", "88", "89",
                                        "90", "91", "92", "93", "94", "96", "97", "98", "99" };

        var prefix = nationalNumber.Substring(0, 2);
        return mobilePrefixes.Contains(prefix);
    }

    public static bool IsValidVietnameseIdCard(this string idCard)
    {
        if (string.IsNullOrWhiteSpace(idCard))
            return false;

        // Remove all non-digit characters
        var digits = new string(idCard.Where(char.IsDigit).ToArray());

        // Vietnamese ID card has 9 or 12 digits
        return digits.Length == 9 || digits.Length == 12;
    }

    public static bool IsValidTaxCode(this string taxCode)
    {
        if (string.IsNullOrWhiteSpace(taxCode))
            return false;

        // Remove all non-digit characters
        var digits = new string(taxCode.Where(char.IsDigit).ToArray());

        // Vietnamese tax code has 10 or 13 digits
        return digits.Length == 10 || digits.Length == 13;
    }

    public static bool IsValidBankAccountNumber(this string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
            return false;

        // Remove all non-digit characters
        var digits = new string(accountNumber.Where(char.IsDigit).ToArray());

        // Bank account number should be between 8 and 19 digits
        return digits.Length >= 8 && digits.Length <= 19;
    }

    public static bool IsValidCreditCardNumber(this string cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        // Remove all non-digit characters
        var digits = new string(cardNumber.Where(char.IsDigit).ToArray());

        // Check length (typically 13-19 digits)
        if (digits.Length < 13 || digits.Length > 19)
            return false;

        // Luhn algorithm
        return IsValidLuhn(digits);
    }

    public static bool IsValidLuhn(this string digits)
    {
        if (string.IsNullOrWhiteSpace(digits) || !digits.All(char.IsDigit))
            return false;

        var sum = 0;
        var alternate = false;

        for (var i = digits.Length - 1; i >= 0; i--)
        {
            var n = int.Parse(digits[i].ToString());

            if (alternate)
            {
                n *= 2;
                if (n > 9)
                    n = (n % 10) + 1;
            }

            sum += n;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    public static bool IsValidHexColor(this string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return false;

        var regex = new Regex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
        return regex.IsMatch(color);
    }

    public static bool IsValidRgbColor(this string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return false;

        var regex = new Regex(@"^rgb\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3})\)$");
        if (!regex.IsMatch(color))
            return false;

        var match = regex.Match(color);
        var r = int.Parse(match.Groups[1].Value);
        var g = int.Parse(match.Groups[2].Value);
        var b = int.Parse(match.Groups[3].Value);

        return r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255;
    }

    public static bool IsValidRgbaColor(this string color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return false;

        var regex = new Regex(@"^rgba\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3}),\s*(0|1|0?\.\d+)\)$");
        if (!regex.IsMatch(color))
            return false;

        var match = regex.Match(color);
        var r = int.Parse(match.Groups[1].Value);
        var g = int.Parse(match.Groups[2].Value);
        var b = int.Parse(match.Groups[3].Value);
        var a = float.Parse(match.Groups[4].Value);

        return r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255 && a >= 0 && a <= 1;
    }

    public static bool IsValidMacAddress(this string macAddress)
    {
        if (string.IsNullOrWhiteSpace(macAddress))
            return false;

        var regex = new Regex(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$");
        return regex.IsMatch(macAddress);
    }

    public static bool IsValidLatitude(this double latitude)
    {
        return latitude >= -90 && latitude <= 90;
    }

    public static bool IsValidLongitude(this double longitude)
    {
        return longitude >= -180 && longitude <= 180;
    }

    public static bool IsValidCoordinate(this (double Latitude, double Longitude) coordinate)
    {
        return coordinate.Latitude.IsValidLatitude() && coordinate.Longitude.IsValidLongitude();
    }

    public static bool IsValidPercentage(this double percentage)
    {
        return percentage >= 0 && percentage <= 100;
    }

    public static bool IsValidPercentage(this int percentage)
    {
        return percentage >= 0 && percentage <= 100;
    }

    public static bool IsValidAge(this DateTime birthDate, int minAge = 0, int maxAge = 150)
    {
        var age = DateTime.Today.Year - birthDate.Year;
        if (birthDate.Date > DateTime.Today.AddYears(-age))
            age--;

        return age >= minAge && age <= maxAge;
    }

    public static bool IsValidFileSize(this long fileSize, long maxSizeInBytes)
    {
        return fileSize >= 0 && fileSize <= maxSizeInBytes;
    }

    public static bool IsValidFileExtension(this string fileName, params string[] allowedExtensions)
    {
        if (string.IsNullOrWhiteSpace(fileName) || allowedExtensions == null || allowedExtensions.Length == 0)
            return false;

        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension))
            return false;

        return allowedExtensions.Any(ext => ext.ToLowerInvariant() == extension);
    }

    public static bool IsValidMimeType(this string mimeType, params string[] allowedMimeTypes)
    {
        if (string.IsNullOrWhiteSpace(mimeType) || allowedMimeTypes == null || allowedMimeTypes.Length == 0)
            return false;

        return allowedMimeTypes.Contains(mimeType, StringComparer.OrdinalIgnoreCase);
    }

    public static List<ValidationResult> ValidateObject(this object obj)
    {
        var results = new List<ValidationResult>();
        if (obj == null)
            return results;

        var context = new ValidationContext(obj);
        Validator.TryValidateObject(obj, context, results, true);
        return results;
    }

    public static bool IsValidObject(this object obj)
    {
        return !obj.ValidateObject().Any();
    }

    public static string GetValidationErrors(this object obj)
    {
        var errors = obj.ValidateObject();
        return string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage));
    }

    public static void ThrowIfInvalid(this object obj, string paramName = null)
    {
        var errors = obj.ValidateObject();
        if (errors.Any())
        {
            var errorMessages = string.Join(Environment.NewLine, errors.Select(e => e.ErrorMessage));
            throw new ValidationException(paramName != null ?
                $"{paramName}: {errorMessages}" : errorMessages);
        }
    }
}