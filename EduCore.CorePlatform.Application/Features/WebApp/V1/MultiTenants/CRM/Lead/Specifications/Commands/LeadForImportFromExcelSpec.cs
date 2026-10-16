using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Application.Common.Specifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Specifications.Commands;

public sealed class LeadForImportFromExcelSpec
    : TenantSpecification<Domain.Entities.MultiTenants.CRM.Lead, Guid>
{
    public LeadForImportFromExcelSpec(ICurrentTenant currentTenant)
        : base(currentTenant)
    {
        AndCriteria(x => !x.IsDeleted);
        EnableTracking(); // Cần để tracking update hàng loạt
    }
}