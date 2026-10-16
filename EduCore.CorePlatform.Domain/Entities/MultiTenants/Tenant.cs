using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.Tenants;
using EduCore.CorePlatform.Domain.Entities.Localization;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Domain.Entities.Settings;
using EduCore.CorePlatform.Domain.Events.Domain.Tenant;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants;

public class Tenant : AggregateRoot<Guid>
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }    
    public TenantStatus Status { get; private set; } = TenantStatus.Pending;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Address { get; private set; }
    public string? Website { get; private set; }
    public string? LogoUrl { get; private set; }
    public DateTime? SubscriptionStartDate { get; private set; }
    public DateTime? SubscriptionEndDate { get; private set; }
    public int MaxUsers { get; private set; } = 10;
    public int MaxStorageMB { get; private set; } = 1024;
    public string? TimeZone { get; private set; }
    
    // Soft isolation
    public DateTime? SuspendedAt { get; private set; }

    public Guid? DefaultLanguageId { get; private set; }

    // Navigation properties
    public virtual ICollection<UserTenant> UserTenants { get; private set; } = new List<UserTenant>();
    public virtual ICollection<TenantConnectionString> ConnectionStrings { get; private set; } = new List<TenantConnectionString>();
    public virtual ICollection<TenantSubscription> Subscriptions { get; private set; } = new List<TenantSubscription>();    
    public virtual ICollection<TenantSetting> Settings { get; private set; } = new List<TenantSetting>();

    public Language? DefaultLanguage { get; private set; }
    protected Tenant() { }

    public Tenant(string code, string name, string? description = null, 
                    string? email = null, string? phone = null, string? address = null,
                    string? website = null, DateTime? subscriptionStartDate = null, DateTime? subscriptionEndDate = null,
                    int maxUsers = 10, int maxStorageMB = 1024, string? timeZone = null, Guid? defaultLanguageId = null,
                    Guid? createdBy = null)
    {
        Code = code;
        Name = name;
        Description = description;
        Email = email;
        Phone = phone;
        Address = address;
        Website = website;
        SubscriptionStartDate = subscriptionStartDate;
        SubscriptionEndDate = subscriptionEndDate;
        MaxUsers = maxUsers;
        MaxStorageMB = maxStorageMB;
        TimeZone = timeZone;
        DefaultLanguageId = defaultLanguageId;
        Status = TenantStatus.Pending;

        CreatedAt = DateTime.UtcNow;
        CreatedBy = createdBy;

        RaiseDomainEvent(new TenantCreatedEvent(Id, code, name));
    }
    public void Activate()
    {
        Status = TenantStatus.Active;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new TenantStatusChangedEvent(Id, TenantStatus.Active));
    }

    public void Suspend()
    {
        Status = TenantStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new TenantStatusChangedEvent(Id, TenantStatus.Suspended));
    }

    public void Update(string name, string? description = null, 
                    string? email = null, string? phone = null, string? address = null,
                    string? website = null, DateTime? subscriptionStartDate = null, DateTime? subscriptionEndDate = null,
                    int maxUsers = 10, int maxStorageMB = 1024, string? timeZone = null, Guid? defaultLanguageId = null,
                    Guid? updatedBy = null)
    {
        Name = name;
        Description = description;
        Status = TenantStatus.Active;
        Email = email;
        Phone = phone;
        Address = address;
        Website = website;
        SubscriptionStartDate = subscriptionStartDate;
        SubscriptionEndDate = subscriptionEndDate;
        MaxUsers = maxUsers;
        MaxStorageMB = maxStorageMB;
        TimeZone = timeZone;
        DefaultLanguageId = defaultLanguageId;
        UpdatedBy = updatedBy;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new TenantUpdatedEvent(Id));
    }

    public void SetSubscription(DateTime startDate, DateTime endDate, int maxUsers, int maxStorageMB)
    {
        SubscriptionStartDate = startDate;
        SubscriptionEndDate = endDate;
        MaxUsers = maxUsers;
        MaxStorageMB = maxStorageMB;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetLocalization(string timeZone, Guid languageId)
    {
        TimeZone = timeZone;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive()
    {
        return Status == TenantStatus.Active;
    }

    public bool ChangeStatus(TenantStatus status)
    {
        return Status == status;
    }

    public bool IsSubscriptionValid()
    {
        if (SubscriptionEndDate == null)
            return true;

        return DateTime.UtcNow <= SubscriptionEndDate.Value;
    }

    public void AssignAdmin(Guid userId)
    {
        RaiseDomainEvent(new TenantAdminAssignedEvent(Id, userId));
    }
    public void ChangeDefaultLanguage(Guid languageId, Guid? updatedByUserId)
    {
        DefaultLanguageId = languageId;
        UpdatedAt = DateTime.UtcNow;
        UpdatedBy = updatedByUserId;
        RaiseDomainEvent(new TenantDefaultLanguageChangedEvent(Id, languageId, updatedByUserId));
    }
}