using EduCore.CorePlatform.Domain.Common.Events;
using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Entities.System.Auditing;

public class EntityChangeLog : BaseEntity<Guid>
{    
    public Guid? TenantId { get; private set; }
    public string EntityName { get; private set; } = string.Empty;    
    public string EntityId { get; private set; } = string.Empty;    
    public EntityChangeType ChangeType { get; private set; }    
    public DateTime ChangeTime { get; private set; }
    public Guid? ChangedBy { get; private set; }
    public string? ChangedByName { get; private set; }
    public string? ChangeDetails { get; private set; }
    public string? Description { get; private set; }
    public string? IpAddress { get; private set; }

    protected EntityChangeLog() { }

    public EntityChangeLog(Guid? tenantId, string entityName, string entityId,
                          EntityChangeType changeType, Guid? changedBy, string? changedByName)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        EntityName = entityName;
        EntityId = entityId;
        ChangeType = changeType;
        ChangeTime = DateTime.UtcNow;
        ChangedBy = changedBy;
        ChangedByName = changedByName;
    }

    public void SetChangeDetails(string? changeDetails, string? description)
    {
        ChangeDetails = changeDetails;
        Description = description;
    }

    public void SetIpAddress(string? ipAddress)
    {
        IpAddress = ipAddress;
    }
}

public enum EntityChangeType
{
    Created = 1,
    Updated = 2,
    Deleted = 3,
    StatusChanged = 4,
    PermissionsChanged = 5,
    Assigned = 6,
    Unassigned = 7
}