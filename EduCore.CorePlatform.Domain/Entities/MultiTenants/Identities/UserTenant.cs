using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Entities.Identity;
using EduCore.CorePlatform.Domain.Events.Domain.MultiTenants.TenantUser;

namespace EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;

public class UserTenant : MultiTenantEntity<Guid, Guid>
{
    public Guid UserId { get; private set; }    
    public bool IsTenantAdmin { get; private set; }
    public DateTime JoinedAt { get; private set; }
    public DateTime? LeftAt { get; private set; }
   
    // Navigation properties
    public virtual User User { get; private set; } = null!;
    public virtual Tenant Tenant { get; private set; } = null!;

    private readonly List<UserTenantRole> _roles = new();
    public IReadOnlyCollection<UserTenantRole> Roles => _roles;

    protected UserTenant() { }

    public UserTenant(Guid userId, Guid tenantId, 
                        bool isTenantAdmin = false)
    {
        UserId = userId;
        IsTenantAdmin = isTenantAdmin;
        JoinedAt = DateTime.UtcNow;

        SetTenant(tenantId);
    }
    public void AddRole(Guid roleId)
    {
        if (_roles.Any(x => x.RoleId == roleId))
            return;

        _roles.Add(new UserTenantRole(Id, roleId));
    }
    public void LeaveTenant()
    {
        if (LeftAt != null)
            return;

        LeftAt = DateTime.UtcNow;

        _roles.Clear();

        RaiseDomainEvent(new UserLeftTenantEvent(UserId, TenantId));
    }
}