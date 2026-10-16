using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders;

public interface IDataSeeder
{
    int Order { get; }
    Task SeedAsync(CancellationToken ct);
}

