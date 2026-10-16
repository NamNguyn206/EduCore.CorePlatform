using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs.Commands;

public sealed class InviteExistingUserToTenantRequest
{
    public List<Guid> UserIds { get; set; } = new();
    public List<Guid> RoleIds { get; set; } = new();
    public bool IsTenantAdmin { get; set; }    
}
