using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Constants.System;

public static class ValidationConstants
{
    public const int MaxNameLength = 256;
    public const int MaxEmailLength = 256;
    public const int MaxPhoneLength = 20;
    public const int MaxDescriptionLength = 2000;
    public const int MaxCodeLength = 100;
    public const int MaxUrlLength = 2048;
    public const int MaxJsonLength = 4000;

    public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public const string PhoneRegex = @"^[\+]?[(]?[0-9]{1,4}[)]?[-\s\.]?[0-9]{1,4}[-\s\.]?[0-9]{1,9}$";

    public const string TenantNameRegex = @"^[a-zA-Z0-9\s\-_\.]{3,100}$";

    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 128;

    public const int MaxFileSize = 10 * 1024 * 1024; // 10MB
    public const string AllowedImageExtensions = ".jpg,.jpeg,.png,.gif,.bmp";
    public const string AllowedDocumentExtensions = ".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx,.txt";
}