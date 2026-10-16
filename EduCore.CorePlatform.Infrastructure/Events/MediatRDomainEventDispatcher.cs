using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Abstractions.Events;
using EduCore.CorePlatform.Domain.Common.Events;
using MediatR;

namespace EduCore.CorePlatform.Infrastructure.Events;

/// <summary>
/// Dispatch domain events via MediatR
/// </summary>
internal sealed class MediatRDomainEventDispatcher
    : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public MediatRDomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAsync(
        IEnumerable<DomainEvent> domainEvents,
        CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
