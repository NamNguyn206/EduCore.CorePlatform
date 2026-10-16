using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.CustomField;

public class CustomFieldValueChangedEvent : DomainEvent
{
    public Guid FieldValueId { get; }
    public string EntityType { get; }
    public Guid EntityId { get; }
    public string FieldCode { get; }
    public Guid TenantId { get; }

    public CustomFieldValueChangedEvent(Guid fieldValueId, string entityType, Guid entityId,
                                       string fieldCode, Guid tenantId)
    {
        FieldValueId = fieldValueId;
        EntityType = entityType;
        EntityId = entityId;
        FieldCode = fieldCode;
        TenantId = tenantId;
    }
}