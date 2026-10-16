using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Settings.Seeders;

public class BootstrapSuperAdminSettings
{
    public bool Enabled { get; set; } = false;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = "vi";
}
