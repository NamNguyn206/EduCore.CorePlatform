using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.Persistence;

public interface IUnitOfWork : IDisposable
{
    // Save
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);

    // Transaction
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

    // Repository access
    IRepository<T> Repository<T>() where T : class;

    ISystemMessageWriteRepository SystemMessageWriteRepository { get; }
}