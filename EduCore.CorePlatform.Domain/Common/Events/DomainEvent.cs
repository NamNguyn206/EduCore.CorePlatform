using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace EduCore.CorePlatform.Domain.Common.Events;

public abstract class DomainEvent
{
    /// <summary>
    /// Event unique identifier
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// When the event occurred
    /// </summary>
    public DateTime OccurredOn { get; }
    /// <summary>
    /// Event type name
    /// </summary>
    public string EventType => GetType().Name;


    /// <summary>
    /// Constructor
    /// </summary>
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredOn = DateTime.UtcNow;
    }
}