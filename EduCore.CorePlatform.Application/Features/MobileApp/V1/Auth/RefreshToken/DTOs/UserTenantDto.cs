using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.DTOs;

public class UserTenantDto
{
    public TenantDto? Tenant { get; set; } = default!;
    public bool IsCurrentTenant { get; set; } = false;
    public bool IsTenantAdmin { get; set; }
    public List<RoleDto> Roles { get; set; } = new();
}