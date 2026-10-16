using EduCore.CorePlatform.Domain.Entities.System.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Abstractions.Persistence;

public interface ISystemMessageWriteRepository
{
    Task<SystemMessage?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        SystemMessage systemMessage,
        CancellationToken cancellationToken = default);

    void Update(SystemMessage systemMessage,
                    CancellationToken cancellationToken = default);
}
