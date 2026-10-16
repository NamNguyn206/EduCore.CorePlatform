using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Entities;

public abstract class MultiTenantEntity<TTenantId, TId> : AggregateRoot<TId>
    where TTenantId : notnull
    where TId : notnull
{
    public TTenantId TenantId { get; private set; } = default!;

    protected MultiTenantEntity() : base() { }

    protected MultiTenantEntity(TId id, TTenantId tenantId) : base(id)
    {
        SetTenant(tenantId);
    }

    protected void SetTenant(TTenantId tenantId)
    {
        if (!EqualityComparer<TTenantId>.Default.Equals(TenantId, default!))
            throw new InvalidOperationException("TenantId cannot be changed.");

        TenantId = tenantId;
    }
}
