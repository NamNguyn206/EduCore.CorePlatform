using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.DTOs.Role;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs;

public class TenantUserDto
{
    public Guid? UserId { get; set; }
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public bool? IsTenantAdmin { get; set; }
    
    public List<RoleDto>? Roles { get; set; }
    
}
