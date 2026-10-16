using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.System.Auditing;

public class AuditLogDetail : BaseEntity<Guid>
{    
    public Guid AuditLogId { get; private set; }
    public string PropertyName { get; private set; } = string.Empty;
    public string? OldValue { get; private set; }
    public string? NewValue { get; private set; }
    public string? PropertyType { get; private set; }

    // Navigation properties
    public virtual AuditLog AuditLog { get; private set; } = null!;

    protected AuditLogDetail() { }

    public AuditLogDetail(Guid auditLogId, string propertyName, string? oldValue, string? newValue, string? propertyType = null)
    {
        Id = Guid.NewGuid();
        AuditLogId = auditLogId;
        PropertyName = propertyName;
        OldValue = oldValue;
        NewValue = newValue;
        PropertyType = propertyType;
    }
}