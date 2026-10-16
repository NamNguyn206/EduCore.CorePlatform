using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.Tenants.DTOs.Queries;

public class GetDefaultLanguageByTenantIdQueryRequest
{
    public Guid TenantId { get; set; } = default!;
}
