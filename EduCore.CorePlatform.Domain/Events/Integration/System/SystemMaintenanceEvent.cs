using EduCore.CorePlatform.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Events.Integration.System;

public class SystemMaintenanceEvent : IntegrationEvent
{
    public string MaintenanceType { get; }
    public DateTime StartTime { get; }
    public DateTime? EndTime { get; }
    public string Description { get; }
    public bool IsScheduled { get; }

    public SystemMaintenanceEvent(string maintenanceType, DateTime startTime,
                                 DateTime? endTime, string description, bool isScheduled)
    {
        MaintenanceType = maintenanceType;
        StartTime = startTime;
        EndTime = endTime;
        Description = description;
        IsScheduled = isScheduled;
    }
}