using ControlTicket.Domain.Common; 
using ControlTicket.Domain.Employees;

namespace ControlTicket.Infrastructure.Persistence.Repositories;

public sealed class EmployeeRepository(ApplicationDbContext context) 
: IRepository<Employee, string>
{
    private readonly ApplicationDbContext ctx = context;

    public Task<Employee?> GetByIdAsync(string rut, CancellationToken cancellationToken = default)
    {
        var result = ctx.Employees.FirstOrDefault(e => e.Rut == rut);
        return Task.FromResult<Employee?>(result);
    }

    public Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var employees = ctx.Employees.ToList(); 
        return Task.FromResult<IReadOnlyList<Employee>>(employees);
    }

    public Task<Employee> AddAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        ctx.Employees.Add(entity);
        ctx.SaveChanges();
        return Task.FromResult(entity);
    }

    public Task UpdateAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        ctx.Employees.Remove(entity);
        ctx.SaveChanges();
        return Task.CompletedTask;
    }


}



