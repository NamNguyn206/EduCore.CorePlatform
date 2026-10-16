using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.Auth.Login.Specifications;

public sealed class UserByEmailWithDetailsSpec : Specification<EduCore.CorePlatform.Domain.Entities.Identity.User>
{
    public UserByEmailWithDetailsSpec(string email)
    {
        // Criteria
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Email == email);

        // Includes 
        AddInclude("UserTenants");
        AddInclude("UserTenants.Tenant");
        AddInclude("UserTenants.Roles");
        AddInclude("UserTenants.Roles.Role");
        AddInclude("UserTenants.Roles.Role.RolePermissions");
        AddInclude("UserTenants.Roles.Role.RolePermissions.Permission");
        AddInclude("SystemRoles");

        // Vì login có update user
        EnableTracking();
    }
}
