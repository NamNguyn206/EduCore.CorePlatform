using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.TenantUsers.Specifications.Queries;

public class TenantUserListSpec
    : Specification<Domain.Entities.Identity.User>
{
    public TenantUserListSpec(
        ICurrentTenant currentTenant,
        string? searchTerm,
        Guid? roleId
    )
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.UserTenants.Any(m => m.TenantId == currentTenant.TenantId!.Value
                                                && m.LeftAt == null));

        if (!string.IsNullOrEmpty(searchTerm))
        {
            AndCriteria(x => x.Email.Contains(searchTerm)
                            || (!string.IsNullOrEmpty(x.FirstName)
                                && x.FirstName.Contains(searchTerm))
                            || (!string.IsNullOrEmpty(x.LastName)
                                && x.LastName.Contains(searchTerm)));
        }

        if (roleId != null)
        {
            AndCriteria(x => x.UserTenants.Any(m => m.TenantId == currentTenant.TenantId!.Value
                                                && m.Roles.Any(r => r.RoleId == roleId.Value)));
        }

        //Includes
        AddInclude(x => x.UserTenants);
        AddInclude("UserTenants.Roles");

        //Sort
        ApplyOrderBy(x => x.Email);
    }
}
