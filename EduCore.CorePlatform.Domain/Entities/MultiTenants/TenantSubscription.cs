using EduCore.CorePlatform.Domain.Common.Constants;
using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants;

public class TenantSubscription : BaseEntity<Guid>
{    
    public Guid TenantId { get; private set; }
    public string PlanName { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsAutoRenew { get; private set; }
    public bool IsActive { get; private set; } = true;
    public int MaxUsers { get; private set; }
    public int MaxStorageMB { get; private set; }
    public decimal Price { get; private set; }
    public string Currency { get; private set; } = "USD";
    public string? Features { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }

    // Navigation properties
    public virtual Tenant Tenant { get; private set; } = null!;

    protected TenantSubscription() { }

    public TenantSubscription(Guid tenantId, string planName, DateTime startDate, DateTime endDate,
                            int maxUsers, int maxStorageMB, decimal price, string currency = "USD",
                            bool isAutoRenew = true, string? features = null)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        PlanName = planName;
        StartDate = startDate;
        EndDate = endDate;
        MaxUsers = maxUsers;
        MaxStorageMB = maxStorageMB;
        Price = price;
        Currency = currency;
        IsAutoRenew = isAutoRenew;
        Features = features;
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }

    public void UpdatePlan(string planName, int maxUsers, int maxStorageMB, decimal price,
                          string? features = null)
    {
        PlanName = planName;
        MaxUsers = maxUsers;
        MaxStorageMB = maxStorageMB;
        Price = price;
        Features = features;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ExtendSubscription(DateTime newEndDate)
    {
        EndDate = newEndDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        IsActive = false;
        CancelledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        CancelledAt = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetAutoRenew(bool autoRenew)
    {
        IsAutoRenew = autoRenew;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsExpired()
    {
        return DateTime.UtcNow > EndDate;
    }

    public bool WillExpireSoon(int days = 7)
    {
        return EndDate <= DateTime.UtcNow.AddDays(days) && EndDate > DateTime.UtcNow;
    }
}
