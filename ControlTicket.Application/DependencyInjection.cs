using System.Reflection;
using ControlTicket.Application.Common.Messaging;
using ControlTicket.Application.Features.Employees.Queries;
using ControlTicket.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace ControlTicket.Application;

/// <summary>
/// Registers Application-layer services into the DI container.
/// Called from Program.cs in the API layer.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        params Assembly[] assembliesToScan
    )
    {
        services.AddScoped<IMediator, Mediator>();

        // Register custom query handlers (they use their own interfaces, not IRequestHandler<,>)
        services.AddScoped<IGetEmployeeByRutQueryHandler, GetEmployeeByRutQueryHandler>();
        services.AddScoped<IGetEmployeeListQueryHandler, GetEmployeeListQueryHandler>();

        foreach (var asm in assembliesToScan)
        {
            RegisterRequestHandlers(services, asm);
            RegisterNotificationHandlers(services, asm);
        }

        return services;
    }

    private static void RegisterRequestHandlers(IServiceCollection services, Assembly asm)
    {
        var registrations = asm.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .Select(i => new { Service = i, Impl = t }));

        foreach (var reg in registrations)
            services.AddScoped(reg.Service, reg.Impl);
    }

    private static void RegisterNotificationHandlers(IServiceCollection services, Assembly asm)
    {
        var registrations = asm.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false })
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                .Select(i => new { Service = i, Impl = t }));

        foreach (var reg in registrations)
            services.AddScoped(reg.Service, reg.Impl);
    }
}
