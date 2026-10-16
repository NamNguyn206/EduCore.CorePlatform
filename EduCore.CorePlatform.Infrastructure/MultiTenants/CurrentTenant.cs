using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Constants.System;

namespace EduCore.CorePlatform.Infrastructure.MultiTenants;

public sealed class CurrentTenant : ICurrentTenant
{
    public Guid? TenantId { get; }

    public CurrentTenant(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context is null)
            return;

        var tenantHeader = context?.Request.Headers[RequestHeaderTypeConstants.XTenantId].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(tenantHeader))
        {
            throw new UnauthorizedAccessException("Tenant header is missing.");
        }

        if (Guid.TryParse(tenantHeader, out var tenantId))
            TenantId = tenantId;
    }
}