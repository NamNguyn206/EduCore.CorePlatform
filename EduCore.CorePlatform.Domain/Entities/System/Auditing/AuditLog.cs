using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.System;
using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.System.Auditing;

public class AuditLog : BaseEntity<Guid>
{
    private readonly List<DomainEvent> _domainEvents = new();    
    public Guid? TenantId { get; private set; }
    public string? UserName { get; private set; }
    public Guid? UserId { get; private set; }
    public string TableName { get; private set; } = string.Empty;    
    public string RecordId { get; private set; } = string.Empty;    
    public AuditAction Action { get; private set; }    
    public DateTime Timestamp { get; private set; }
    public string? OldValues { get; private set; }
    public string? NewValues { get; private set; }
    public string? ChangedColumns { get; private set; }
    public string? IpAddress { get; private set; } = string.Empty;
    public string? UserAgent { get; private set; }
    public string? RequestPath { get; private set; }
    public string? HttpMethod { get; private set; }
    public string? AdditionalData { get; private set; }
    public bool HasDetails { get; private set; }
    public string MachineName { get; private set; } = string.Empty;

    // JSON serialized audit details
    public string OldValuesJson { get; private set; } = string.Empty;
    public string NewValuesJson { get; private set; } = string.Empty;
    public string ChangedColumnsJson { get; private set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<AuditLogDetail> AuditLogDetails { get; private set; } = new List<AuditLogDetail>();

    // Domain events
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AuditLog() { }

    public AuditLog(Guid? tenantId, string? userName, Guid? userId, string tableName,
                   string recordId, AuditAction action, DateTime timestamp)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        UserName = userName;
        UserId = userId;
        TableName = tableName;
        RecordId = recordId;
        Action = action;
        Timestamp = timestamp;
    }

    public void SetHttpContext(string? ipAddress, string? userAgent, string? requestPath, string? httpMethod)
    {
        IpAddress = ipAddress;
        UserAgent = userAgent;
        RequestPath = requestPath;
        HttpMethod = httpMethod;
    }

    public void SetValues(string? oldValues, string? newValues, string? changedColumns)
    {
        OldValues = oldValues;
        NewValues = newValues;
        ChangedColumns = changedColumns;
        HasDetails = !string.IsNullOrEmpty(oldValues) || !string.IsNullOrEmpty(newValues);
    }

    public void SetAdditionalData(string? additionalData)
    {
        AdditionalData = additionalData;
    }

    public void AddDetail(AuditLogDetail detail)
    {
        AuditLogDetails.Add(detail);
    }

    public string GetActionDescription()
    {
        return Action switch
        {
            AuditAction.Create => "Tạo mới",
            AuditAction.Update => "Cập nhật",
            AuditAction.Delete => "Xóa",
            AuditAction.SoftDelete => "Xóa mềm",
            AuditAction.Restore => "Khôi phục",
            AuditAction.Login => "Đăng nhập",
            AuditAction.Logout => "Đăng xuất",
            AuditAction.AccessDenied => "Truy cập bị từ chối",
            AuditAction.PermissionChanged => "Thay đổi quyền",
            AuditAction.TenantChanged => "Thay đổi tenant",
            _ => Action.ToString()
        };
    }

    // Domain events implementation
    public void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}