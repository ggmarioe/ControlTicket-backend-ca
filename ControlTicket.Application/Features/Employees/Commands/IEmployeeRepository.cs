using ControlTicket.Domain.Employees;

namespace ControlTicket.Application.Features.Employees.Commands;

public interface IEmployeeRepository
{
    Task<bool> RutExistsAsync(string rut, CancellationToken ct);
    Task AddAsync(Employee employee, CancellationToken ct);
}