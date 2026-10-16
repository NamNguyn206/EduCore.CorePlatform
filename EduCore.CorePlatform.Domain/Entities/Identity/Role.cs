using EduCore.CorePlatform.Domain.Common.Entities;
using EduCore.CorePlatform.Domain.Common.Enums.Identities;
using EduCore.CorePlatform.Domain.Entities.MultiTenants.Identities;
using EduCore.CorePlatform.Domain.Events.Domain.Role;

namespace EduCore.CorePlatform.Domain.Entities.Identity;

public class Role : AggregateRoot<Guid>
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public RoleScope Scope { get; private set; }
    public Guid? TenantId { get; private set; }   // NULL = system role    
    public string? Description { get; private set; }    
    public bool IsDefault { get; private set; }

    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions;
    private readonly List<UserTenantRole> _userTenantRole = new();
    public IReadOnlyCollection<UserTenantRole> UserTenantRoles => _userTenantRole;
    private readonly List<UserSystemRole> _systemRoles = new();
    public IReadOnlyCollection<UserSystemRole> SystemRoles => _systemRoles;

    private Role() { }

    public Role(string code, string name, string? description, 
                RoleScope scope, Guid? tenantId = null, 
                bool isDefault = false)
        : base(Guid.NewGuid())
    {
        TenantId = tenantId;
        Code = code;
        Name = name;        
        Description = description;
        Scope = scope;
        IsDefault = isDefault;
        CreatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new RoleCreatedEvent(Id, Name, TenantId));
    }

    public static Role CreateSystemRole(string code, string name, string? description = null, 
                                        bool isDefault = false)
    {
        return new Role(
            code,
            name,
            description,
            RoleScope.System,
            null,
            isDefault
        );
    }
    public static Role CreateTenantRole(string code, string name, Guid? tenantId, 
                                        string? description = null, bool isDefault = false)
    {
        return new Role(
            code,
            name,
            description,
            RoleScope.Tenant,
            tenantId,
            isDefault
        );
    }
    public void Update(string code, string name, string? description, RoleScope scope,
                       bool isDefault)
    {
        Code = code;
        Name = name;
        Description = description;
        Scope = scope;
        IsDefault = isDefault;
        UpdatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new RolePermissionsUpdatedEvent(Id, Name));
    }
    public void MarkAsDefault()
    {
        IsDefault = true;
        UpdatedAt = DateTime.UtcNow;
    }
}