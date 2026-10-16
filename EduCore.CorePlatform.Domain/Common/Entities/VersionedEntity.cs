using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Domain.Common.Entities;

public abstract class VersionedEntity<TId> : SoftDeleteEntity<TId>
    where TId : notnull
{
    /// <summary>
    /// Concurrency token (optimistic locking)
    /// </summary>
    public int Version { get; private set; }

    protected VersionedEntity() { }

    protected VersionedEntity(TId id) : base(id) { }

    /// <summary>
    /// Called by Infrastructure (EF Core) when saving
    /// </summary>
    protected void IncrementVersion()
    {
        Version++;
    }
}
