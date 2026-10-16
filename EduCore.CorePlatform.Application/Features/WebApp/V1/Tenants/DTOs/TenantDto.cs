using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Enums.Tenants;

namespace EduCore.CorePlatform.Application.Features.Tenants.DTOs;

public class TenantDto
{
    public Guid? Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TenantStatus? Status { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? LogoUrl { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public int? MaxUsers { get; set; }
    public int? MaxStorageMB { get; set; }
    public string? TimeZone { get; set; }
}
