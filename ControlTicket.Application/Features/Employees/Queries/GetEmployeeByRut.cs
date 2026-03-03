using System.Reflection.Metadata.Ecma335;
using ControlTicket.Application.Features.Employees.Dtos;
using ControlTicket.Domain.Common;
using ControlTicket.Domain.Employees;
using ControlTicket.SharedKernel.Results;

using Microsoft.EntityFrameworkCore.Metadata;

namespace ControlTicket.Application.Features.Employees.Queries;

public sealed record GetEmployeeByRutQuery(string Rut);

public interface IGetEmployeeByRutQueryHandler
{
    Task<Result<EmployeeListItemDto?>> Handle(
        GetEmployeeByRutQuery query,
        CancellationToken cancellationToken = default);
}

public sealed class GetEmployeeByRutQueryHandler(IRepository<Employee, string> employeeRepository) : IGetEmployeeByRutQueryHandler
{
    private readonly IRepository<Employee, string> _employeeRepository = employeeRepository;

    public async Task<Result<EmployeeListItemDto?>> Handle(
        GetEmployeeByRutQuery query, 
        CancellationToken cancellationToken)
    {
        var employee = await _employeeRepository.GetByIdAsync(query.Rut, cancellationToken);

        if(employee == null)
        {
            return Result<EmployeeListItemDto?>.Failure(new Error(
                "employee_not_found",
                $"No employee found with RUT {query.Rut}",
                ErrorType.NotFound)
            );
        }

        return Result<EmployeeListItemDto?>.Success(new EmployeeListItemDto(
            employee.Rut,
            employee.FirstName,
            employee.LastName,
            employee.WorkPlaceId,
            employee.Size,
            employee.PictureUrl,
            employee.IsDeleted,
            employee.TicketId));
    }
}
