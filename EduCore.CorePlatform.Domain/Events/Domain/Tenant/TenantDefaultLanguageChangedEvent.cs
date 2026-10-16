using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Events;

namespace EduCore.CorePlatform.Domain.Events.Domain.Tenant;

public class TenantDefaultLanguageChangedEvent : DomainEvent
{
    public Guid TenantId { get; }
    public Guid LanguageId { get; }
    public Guid? UpdatedByUserId { get; }

    public TenantDefaultLanguageChangedEvent(Guid tenantId, Guid languageId, Guid? updatedByUserId)
    {
        TenantId = tenantId;
        LanguageId = languageId;
        UpdatedByUserId = updatedByUserId;
    }
}