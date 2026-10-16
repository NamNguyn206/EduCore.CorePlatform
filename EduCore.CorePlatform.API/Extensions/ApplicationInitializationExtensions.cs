using EduCore.CorePlatform.Infrastructure.Persistence.DbContext;
using EduCore.CorePlatform.Infrastructure.Persistence.Seeders;

namespace EduCore.CorePlatform.API.Extensions;

public static class ApplicationInitializationExtensions
{
    public static async Task InitAuthorizationAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("AuthorizationInit");

        logger.LogInformation("=== Authorization initialization started ===");

        // 1️⃣ Run authorization seeders (Role, Permission, RolePermission)
        var seederRunner = scope.ServiceProvider
            .GetRequiredService<SeederRunner>();

        await seederRunner.RunAsync();

        logger.LogInformation("=== Authorization initialization completed ===");

    }
}