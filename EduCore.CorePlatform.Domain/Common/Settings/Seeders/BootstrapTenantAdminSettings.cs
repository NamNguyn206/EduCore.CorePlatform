using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Settings.Seeders;

public class BootstrapTenantAdminSettings
{
    public bool Enabled { get; set; } = false;
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string LanguageCode { get; set; } = "vi";
}