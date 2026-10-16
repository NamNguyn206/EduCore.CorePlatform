using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Specifications;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Department.Specifications.Commands;

public sealed class DepartmentByNameForCreateSpec
    : TenantSpecification<Domain.Entities.MultiTenants.Categories.Department, Guid>
{
    public DepartmentByNameForCreateSpec(ICurrentTenant currentTenant, string name, string address)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        AndCriteria(x => x.Name == name && x.Address == address);
    }
}