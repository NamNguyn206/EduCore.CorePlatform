using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using EduCore.CorePlatform.Application.Abstractions.MultiTenants;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Domain.Common.Entities;

namespace EduCore.CorePlatform.Application.Common.Specifications;

/// <summary>
/// Base specification cho entity đa tenant
/// - Tự động filter theo TenantId
/// - System tenant sẽ bỏ qua filter
/// </summary>
public abstract class TenantSpecification<TEntity, TId>
    : Specification<TEntity>
    where TEntity : MultiTenantEntity<Guid, TId>
    where TId : notnull
{
    protected TenantSpecification(ICurrentTenant currentTenant)
    {
        if (currentTenant == null)
            throw new ArgumentNullException(nameof(currentTenant));

        if (currentTenant.TenantId == null)
            throw new InvalidOperationException("TenantId is required.");

        AndCriteria(x => x.TenantId == currentTenant.TenantId);
    }

    /// <summary>
    /// Ghép thêm điều kiện AND (an toàn cho EF Core)
    /// </summary>
    protected void AddTenantCriteria(Expression<Func<TEntity, bool>> expression)
    {
        AndCriteria(expression);
    }
}