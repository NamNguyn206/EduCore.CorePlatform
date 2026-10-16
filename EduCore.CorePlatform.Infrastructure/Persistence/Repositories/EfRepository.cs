using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EduCore.CorePlatform.Domain.Specifications;
using EduCore.CorePlatform.Infrastructure.Persistence.Specifications;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

public class EfRepository<T> : IRepository<T>
    where T : class
{
    protected readonly DbContext _dbContext;

    public EfRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<T> AsQueryable()
    {
        return _dbContext.Set<T>();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        => SpecificationEvaluator.GetQuery(
            _dbContext.Set<T>().AsQueryable(),
            spec);

    public async Task<T?> FirstOrDefaultAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default
    )
    {
        return await ApplySpecification(spec)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> ListAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default
    )
    {
        return await ApplySpecification(spec)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> AnyAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default
    )
    {
        return await ApplySpecification(spec)
            .AnyAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default
    )
    {
        return await ApplySpecification(spec)
            .CountAsync(cancellationToken);
    }

    public async Task<decimal> SumAsync(
        ISpecification<T> spec,
        Expression<Func<T, decimal>> selector,
        CancellationToken cancellationToken = default
    )
    {
        return await ApplySpecification(spec)
            .SumAsync(selector, cancellationToken);
    }

    public async Task<decimal?> SumAsync(
        ISpecification<T> spec,
        Expression<Func<T, decimal?>> selector,
        CancellationToken cancellationToken = default
    )
    {
        return await ApplySpecification(spec)
            .SumAsync(selector, cancellationToken);
    }
   
    public async Task<List<TResult>> ListAsync<TResult>(
        IAggregateSpecification<T, TResult> spec,
        CancellationToken cancellationToken = default
    )
    {
        var query = spec.Apply(_dbContext.Set<T>().AsQueryable());
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync<TResult>(
        IAggregateSpecification<T, TResult> spec,
        CancellationToken cancellationToken = default
    )
    {
        var query = spec.Apply(_dbContext.Set<T>().AsQueryable());
        return await query.CountAsync(cancellationToken);
    }

    public async Task AddAsync(
        T entity, 
        CancellationToken cancellationToken = default
    )
    {
        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }
    public async Task AddRangeAsync(
        IEnumerable<T> entities, 
        CancellationToken cancellationToken = default
    )
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }
    public Task UpdateRangeAsync(
        IEnumerable<T> entities, 
        CancellationToken cancellationToken = default
    )
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        _dbContext.Set<T>().UpdateRange(entities);

        return Task.CompletedTask;
    }
    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}
