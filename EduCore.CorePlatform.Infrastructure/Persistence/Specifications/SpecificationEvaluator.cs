using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> inputQuery,
        ISpecification<T> spec) where T : class
    {
        var query = inputQuery;

        // Tracking
        if (spec.AsNoTracking)
            query = query.AsNoTracking();

        // Criteria
        if (spec.Criteria != null)
            query = query.Where(spec.Criteria);

        // Includes
        if (spec.Includes != null && spec.Includes.Count > 0)
        {
            query = spec.Includes
                .Aggregate(query,
                    (current, include) => current.Include(include));
        }

        // String-based include
        if (spec.IncludeStrings != null && spec.IncludeStrings.Count > 0)
        {
            query = spec.IncludeStrings
                .Aggregate(query,
                    (current, include) => current.Include(include));
        }

        // Order
        if (spec.OrderBy != null)
            query = query.OrderBy(spec.OrderBy);
        else if (spec.OrderByDescending != null)
            query = query.OrderByDescending(spec.OrderByDescending);

        // Paging
        if (spec.IsPagingEnabled)
            query = query.Skip(spec.Skip!.Value).Take(spec.Take!.Value);

        return query;
    }
}
