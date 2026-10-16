using EduCore.CorePlatform.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Entities.Identity;

public sealed class UserSystemRole : AggregateRoot<Guid>
{
    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    // Navigation properties
    public User User { get; private set; } = default!;

    public Role Role { get; private set; } = default!;

    private UserSystemRole()
    {
    }

    public UserSystemRole(
        Guid userId,
        Guid roleId
    )
    {
        UserId = userId;
        RoleId = roleId;
    }
}