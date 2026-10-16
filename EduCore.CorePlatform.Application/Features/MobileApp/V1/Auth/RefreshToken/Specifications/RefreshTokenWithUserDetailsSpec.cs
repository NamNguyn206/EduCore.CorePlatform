using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.MobileApp.V1.Auth.RefreshToken.Specifications;

public sealed class RefreshTokenWithUserDetailsSpec
    : Specification<Domain.Entities.Identity.RefreshToken>
{
    public RefreshTokenWithUserDetailsSpec(string token)
    {
        AndCriteria(x => x.Token == token);
        AndCriteria(x => !x.IsRevoked);
        AndCriteria(x => x.ExpiresAt > DateTime.UtcNow);

        AddInclude("User");
        AddInclude("User.UserTenants");
        AddInclude("User.UserTenants.Tenant");
        AddInclude("User.UserTenants.Roles");
        AddInclude("User.UserTenants.Roles.Role");
        AddInclude("User.UserTenants.Roles.Role.RolePermissions");
        AddInclude("User.UserTenants.Roles.Role.RolePermissions.Permission");
        AddInclude("User.SystemRoles");

        EnableTracking();
    }
}
