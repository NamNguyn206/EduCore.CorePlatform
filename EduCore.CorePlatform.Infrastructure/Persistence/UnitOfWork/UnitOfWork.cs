using DocumentFormat.OpenXml.InkML;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using EduCore.CorePlatform.Infrastructure.Persistence.Extensions;
using EduCore.CorePlatform.Infrastructure.Persistence.Repositories;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Infrastructure.Persistence.UnitOfWork;

public sealed class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly Dictionary<Type, object> _repositories = new();
    private IDbContextTransaction? _currentTransaction;
    private bool _disposed;

    public UnitOfWork(
        ApplicationDbContext dbContext,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    // =====================================================
    // SAVE
    // =====================================================
    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> SaveEntitiesAsync(
        CancellationToken cancellationToken = default)
    {
        // 1. Save database changes
        await _dbContext.SaveChangesAsync(cancellationToken);

        // 2. Publish domain events
        await _dbContext.PublishDomainEventAsync(
            _mediator,
            cancellationToken);

        return true;
    }

    // =====================================================
    // TRANSACTION
    // =====================================================
    public async Task BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            return;

        _currentTransaction = await _dbContext.Database
            .BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            throw new InvalidOperationException("No active transaction");

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _dbContext.PublishDomainEventAsync(
                _mediator,
                cancellationToken);

            await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.RollbackAsync(cancellationToken);
        await DisposeTransactionAsync();
    }

    private async Task DisposeTransactionAsync()
    {
        if (_currentTransaction != null)
        {
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }

    // =====================================================
    // REPOSITORY ACCESS
    // =====================================================
    public IRepository<T> Repository<T>() where T : class
    {
        if (_repositories.TryGetValue(typeof(T), out var repo))
            return (IRepository<T>)repo;

        var newRepo = new EfRepository<T>(_dbContext);
        _repositories[typeof(T)] = newRepo;
        return newRepo;
    }
    // =========================================
    // DISPOSE
    // =========================================
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _currentTransaction?.Dispose();
            _dbContext.Dispose();
        }

        _disposed = true;
    }

    public ISystemMessageWriteRepository SystemMessageWriteRepository
        => new SystemMessageWriteRepository(_dbContext);
}
