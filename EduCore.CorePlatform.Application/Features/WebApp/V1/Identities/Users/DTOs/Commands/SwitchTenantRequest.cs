using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Identities.Users.DTOs.Commands;

public sealed class SwitchTenantRequest
{
    public Guid TenantId { get; set; } = default!;

}