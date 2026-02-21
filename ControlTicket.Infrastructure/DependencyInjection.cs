using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ControlTicket.Infrastructure;

/// <summary>
/// Registers Infrastructure-layer services into the DI container.
/// Called from Program.cs in the API layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register infrastructure services here as the project grows.
        // Examples: DbContext, repository implementations, external API clients, etc.

        return services;
    }
}
