using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.Identity;

public class RolePermission : AggregateRoot<Guid>
{
    public Guid RoleId { get; private set; }
    public Guid PermissionId { get; private set; }

    // Navigation properties
    public Role Role { get; private set; } = default!;
    public Permission Permission { get; private set; } = default!;

    private RolePermission() { }

    public RolePermission(Guid roleId, Guid permissionId, Guid? createdBy = null)
    {
        RoleId = roleId;
        PermissionId = permissionId;
        CreatedBy = createdBy;
    }
}