using EduCore.CorePlatform.Domain.Common.Entities;

namespace EduCore.CorePlatform.Domain.Entities.Identity;

public class Permission : AggregateRoot<Guid>
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; } = default!;

    private readonly List<RolePermission> _rolePermissions = new();
    public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions;
    protected Permission() { }

    public Permission(string code, string name, string? description,
                     Guid? createdBy)
        : base()
    {
        Code = code;
        Name = name;
        Description = description;
        CreatedBy = createdBy;
    }
}