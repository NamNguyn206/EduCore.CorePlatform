using EduCore.CorePlatform.Application.Common.Events;
using EduCore.CorePlatform.Domain.Common.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Extensions;

/// <summary>
/// Extension methods for ApplicationDbContext
/// Purpose: Publish domain events raised by aggregate roots
/// </summary>
public static class ApplicationDbContextExtensions
{
    /// <summary>
    /// Publish all domain events via MediatR and clear them from entities
    /// </summary>
    public static async Task PublishDomainEventAsync(
        this Microsoft.EntityFrameworkCore.DbContext dbContext,
        IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        if (dbContext is null)
            throw new ArgumentNullException(nameof(dbContext));

        if (mediator is null)
            throw new ArgumentNullException(nameof(mediator));

        // 1. Get all entities derived from EventAwareEntity<Guid>
        var entitiesWithDomainEvents = dbContext.ChangeTracker
            .Entries<EventAwareEntity<Guid>>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        if (!entitiesWithDomainEvents.Any())
            return;

        // 2. Collect domain events
        var domainEvents = entitiesWithDomainEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        // 3. Clear domain events BEFORE publishing
        entitiesWithDomainEvents.ForEach(e => e.ClearDomainEvents());

        // 4. Publish domain events
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());

            var notification = Activator.CreateInstance(notificationType, domainEvent);

            await mediator.Publish((INotification)notification!, cancellationToken);

        }
    }
}
