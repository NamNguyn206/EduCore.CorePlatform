using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants;

public class TenantConnectionString : BaseEntity<Guid>
{    
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;    
    public string ConnectionString { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public bool IsDefault { get; private set; }
    public string DatabaseType { get; private set; } = "SQLServer";
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation properties
    public virtual Tenant Tenant { get; private set; } = null!;

    protected TenantConnectionString() { }

    public TenantConnectionString(Guid tenantId, string name, string connectionString,
                                 string databaseType = "SQLServer", string? description = null)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        Name = name;
        ConnectionString = connectionString;
        DatabaseType = databaseType;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsDefault()
    {
        IsDefault = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string connectionString, string? description)
    {
        Name = name;
        ConnectionString = connectionString;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }
}