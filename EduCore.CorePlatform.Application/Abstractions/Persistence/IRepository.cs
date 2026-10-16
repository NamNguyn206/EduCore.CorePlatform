using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using EduCore.CorePlatform.Domain.Specifications;

namespace EduCore.CorePlatform.Application.Abstractions.Persistence;

public interface IRepository<T>
{
    IQueryable<T> AsQueryable();
    Task<T?> FirstOrDefaultAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default);

    Task<List<T>> ListAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(
        ISpecification<T> spec,
        CancellationToken cancellationToken = default);

    Task<decimal> SumAsync(
    ISpecification<T> spec,
    Expression<Func<T, decimal>> selector,
    CancellationToken cancellationToken = default);

    Task<decimal?> SumAsync(
        ISpecification<T> spec,
        Expression<Func<T, decimal?>> selector,
        CancellationToken cancellationToken = default);

    Task<List<TResult>> ListAsync<TResult>(
        IAggregateSpecification<T, TResult> spec,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync<TResult>(
        IAggregateSpecification<T, TResult> spec,
        CancellationToken cancellationToken = default);

    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Update(T entity);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    void Delete(T entity);
}
