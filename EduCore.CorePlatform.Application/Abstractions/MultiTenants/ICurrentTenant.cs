using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.MultiTenants;

public interface ICurrentTenant
{
    Guid? TenantId { get; }
}
