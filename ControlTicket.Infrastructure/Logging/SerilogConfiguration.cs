using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace ControlTicket.Infrastructure.Logging;

/// <summary>
/// Configures Serilog as the application's logging provider.
/// Reads settings from the "Serilog" section of appsettings.json.
/// </summary>
public static class SerilogConfiguration
{
    /// <summary>
    /// Adds Serilog to the host builder, reading configuration from appsettings.json.
    /// </summary>
    public static IHostBuilder AddSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });
    }

    /// <summary>
    /// Creates a bootstrap logger for capturing logs during application startup,
    /// before the full configuration is available.
    /// </summary>
    public static void CreateBootstrapLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();
    }
}
