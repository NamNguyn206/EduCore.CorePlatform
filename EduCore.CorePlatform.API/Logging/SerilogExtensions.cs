using Serilog;

namespace EduCore.CorePlatform.API.Logging;

public static class SerilogExtensions
{
    public static WebApplicationBuilder UseSerilogLogging(
        this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext();
        });

        return builder;
    }
}
