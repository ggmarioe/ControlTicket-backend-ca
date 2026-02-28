using ControlTicket.Domain.Common;
using ControlTicket.Domain.Employees;
using ControlTicket.Infrastructure.Persistence.Repositories;
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
        // Register repository implementations
        services.AddScoped<IRepository<Employee, string>, EmployeeRepository>();

        return services;
    }
}
    