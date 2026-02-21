using Microsoft.Extensions.DependencyInjection;

namespace ControlTicket.Application;

/// <summary>
/// Registers Application-layer services into the DI container.
/// Called from Program.cs in the API layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services here as the project grows.
        // Examples: MediatR handlers, validators, mapping profiles, etc.

        return services;
    }
}
