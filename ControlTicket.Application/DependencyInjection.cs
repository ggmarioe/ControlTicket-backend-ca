using System.Reflection;
using ControlTicket.Application.Common.Messaging;
using ControlTicket.Application.Features.Employees.Queries;
using ControlTicket.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace ControlTicket.Application;

/// <summary>
/// Provides extension methods to register Application-layer services
/// (mediator, request handlers, notification handlers, etc.) into the DI container.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers Application services (mediator and handlers) into the given
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The dependency injection service collection.</param>
    /// <param name="assembliesToScan">
    /// Assemblies that contain implementations of
    /// <see cref="IRequestHandler{TRequest,TResponse}"/> and
    /// <see cref="INotificationHandler{TNotification}"/>.
    /// These assemblies will be scanned via reflection to automatically register handlers.
    /// </param>
    /// <returns>The same <see cref="IServiceCollection"/> to allow chained calls.</returns>
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        params Assembly[] assembliesToScan
    )
    {
        // Core mediator implementation used across the application.
        services.AddScoped<IMediator, Mediator>();

        // Custom query handlers that still use their own interfaces instead of IRequestHandler<,>.
        services.AddScoped<IGetEmployeeByRutQueryHandler, GetEmployeeByRutQueryHandler>();

        // Automatically register IRequestHandler<,> and INotificationHandler<> implementations
        // discovered in the specified assemblies.
        foreach (var asm in assembliesToScan)
        {
            RegisterRequestHandlers(services, asm);
            RegisterNotificationHandlers(services, asm);
        }

        return services;
    }

    /// <summary>
    /// Scans the given assembly and registers all concrete types that implement
    /// <see cref="IRequestHandler{TRequest,TResponse}"/> as scoped services.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="asm">Assembly to scan for request handlers.</param>
    private static void RegisterRequestHandlers(IServiceCollection services, Assembly asm)
    {
        var registrations = asm.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(i => new { Service = i, Impl = t }));

        foreach (var reg in registrations)
        {
            services.AddScoped(reg.Service, reg.Impl);
        }
    }

    /// <summary>
    /// Scans the given assembly and registers all concrete types that implement
    /// <see cref="INotificationHandler{TNotification}"/> as scoped services.
    /// </summary>
    /// <param name="services">The DI service collection.</param>
    /// <param name="asm">Assembly to scan for notification handlers.</param>
    private static void RegisterNotificationHandlers(IServiceCollection services, Assembly asm)
    {
        var registrations = asm.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                .Select(i => new { Service = i, Impl = t }));

        foreach (var reg in registrations)
        {
            services.AddScoped(reg.Service, reg.Impl);
        }
    }
}
