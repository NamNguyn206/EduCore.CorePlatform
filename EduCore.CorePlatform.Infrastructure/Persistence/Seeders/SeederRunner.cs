using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EduCore.CorePlatform.Infrastructure.Persistence.Seeders;

public sealed class SeederRunner
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SeederRunner> _logger;

    public SeederRunner(
        IServiceProvider serviceProvider,
        ILogger<SeederRunner> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken ct = default)
    {
        using var scope = _serviceProvider.CreateScope();

        var seeders = scope.ServiceProvider
            .GetServices<IDataSeeder>()
            .OrderBy(x => x.Order)
            .ToList();

        foreach (var seeder in seeders)
        {
            _logger.LogInformation(
                "Running seeder: {Seeder}",
                seeder.GetType().Name);

            await seeder.SeedAsync(ct);
        }
    }
}
