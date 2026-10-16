using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.DTOs.Identity;

public class UserTenantDto
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = default!;
    public bool IsTenantAdmin { get; set; }

    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}