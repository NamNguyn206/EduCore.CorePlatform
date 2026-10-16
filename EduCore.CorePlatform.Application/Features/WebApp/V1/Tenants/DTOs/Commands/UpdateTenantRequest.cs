using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;

namespace EduCore.CorePlatform.Application.Features.Tenants.DTOs.Commands;

public class UpdateTenantRequest : UpdateRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; private set; }
    public string? Website { get; private set; }
    public string? LogoUrl { get; private set; }
    public DateTime? SubscriptionStartDate { get; private set; }
    public DateTime? SubscriptionEndDate { get; private set; }
    public int MaxUsers { get; private set; } = 10;
    public int MaxStorageMB { get; private set; } = 1024;
    public string? TimeZone { get; private set; }
}
