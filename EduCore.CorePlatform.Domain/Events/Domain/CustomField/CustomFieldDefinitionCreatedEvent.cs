using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.CustomField;

public class CustomFieldDefinitionCreatedEvent : DomainEvent
{
    public Guid FieldDefinitionId { get; }
    public string FieldCode { get; }
    public string EntityType { get; }
    public Guid TenantId { get; }

    public CustomFieldDefinitionCreatedEvent(Guid fieldDefinitionId, string fieldCode,
                                           string entityType, Guid tenantId)
    {
        FieldDefinitionId = fieldDefinitionId;
        FieldCode = fieldCode;
        EntityType = entityType;
        TenantId = tenantId;
    }
}