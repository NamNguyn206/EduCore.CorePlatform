using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Specifications;

public interface IAggregateSpecification<TEntity, TResult>
{
    IQueryable<TResult> Apply(IQueryable<TEntity> query);
}