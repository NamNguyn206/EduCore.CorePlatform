using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using EduCore.CorePlatform.Domain.Entities.System.Messages;
using EduCore.CorePlatform.Application.Abstractions.Persistence;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Repositories;

public class SystemMessageWriteRepository
    : ISystemMessageWriteRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SystemMessageWriteRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SystemMessage?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SystemMessages
                            .Include(x => x.Translations)   // 👈 tracking
                            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }

    public async Task AddAsync(
        SystemMessage systemMessage,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SystemMessages
                        .AddAsync(systemMessage, cancellationToken);
    }
    public void Update(SystemMessage systemMessage,
                        CancellationToken cancellationToken = default)
    {
        _dbContext.SystemMessages.Update(systemMessage);
    }
}
