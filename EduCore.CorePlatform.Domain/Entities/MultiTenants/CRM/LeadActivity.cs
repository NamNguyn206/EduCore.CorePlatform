using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.MultiTenants.CRM;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.CRM;

public class LeadActivity : AggregateRoot<Guid>
{
    public Guid LeadId { get; private set; }
    public ActivityType ActivityType { get; private set; }
    public string? Details { get; private set; }
    public DateTime OccurredAt { get; private set; }

    // Navigation property
    public Lead Lead { get; private set; } = default!;

    protected LeadActivity() { }

    public LeadActivity(Guid leadId, ActivityType activityType, string? details = null)
    {
        LeadId = leadId;
        ActivityType = activityType;
        Details = details;
        OccurredAt = DateTime.UtcNow;
    }
}