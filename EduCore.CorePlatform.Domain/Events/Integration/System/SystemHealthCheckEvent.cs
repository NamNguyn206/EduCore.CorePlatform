using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Integration.System;

public class SystemHealthCheckEvent : IntegrationEvent
{
    public string Component { get; }
    public string Status { get; }
    public decimal ResponseTime { get; }
    public DateTime CheckedAt { get; }
    public string? ErrorMessage { get; }

    public SystemHealthCheckEvent(string component, string status, decimal responseTime,
                                 DateTime checkedAt, string? errorMessage = null)
    {
        Component = component;
        Status = status;
        ResponseTime = responseTime;
        CheckedAt = checkedAt;
        ErrorMessage = errorMessage;
    }
}