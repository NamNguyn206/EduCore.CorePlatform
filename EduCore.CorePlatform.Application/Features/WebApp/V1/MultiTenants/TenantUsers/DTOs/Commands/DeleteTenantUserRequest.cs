using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.DTOs.Commands;

public class DeleteTenantUserRequest
{
    public Guid UserId { get; set; }
}
