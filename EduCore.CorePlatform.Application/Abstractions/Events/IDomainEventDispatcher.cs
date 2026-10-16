using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Domain.Common.Events;

namespace EduCore.CorePlatform.Application.Abstractions.Events;

/// <summary>
/// Dispatch domain events
/// </summary>
public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IEnumerable<DomainEvent> domainEvents,
        CancellationToken cancellationToken = default);
}
