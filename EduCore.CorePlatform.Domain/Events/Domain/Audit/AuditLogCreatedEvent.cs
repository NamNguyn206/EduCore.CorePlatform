using EduCore.CorePlatform.Domain.Common.Enums.System;
using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Domain.Audit;

public class AuditLogCreatedEvent : DomainEvent
{
    public Guid AuditLogId { get; }
    public string TableName { get; }
    public string RecordId { get; }
    public AuditAction Action { get; }
    public Guid? UserId { get; }
    public Guid? TenantId { get; }

    public AuditLogCreatedEvent(Guid auditLogId, string tableName, string recordId,
                               AuditAction action, Guid? userId, Guid? tenantId)
    {
        AuditLogId = auditLogId;
        TableName = tableName;
        RecordId = recordId;
        Action = action;
        UserId = userId;
        TenantId = tenantId;
    }
}