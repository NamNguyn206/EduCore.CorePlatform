using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Login.DTOs;

public class TenantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}
