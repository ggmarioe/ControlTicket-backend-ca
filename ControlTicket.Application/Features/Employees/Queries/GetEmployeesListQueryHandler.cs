using ControlTicket.Application.Common.Messaging;
using ControlTicket.Application.Features.Employees.Dtos;
using ControlTicket.Domain.Common;
using ControlTicket.Domain.Employees;
using ControlTicket.SharedKernel.Results;

namespace ControlTicket.Application.Features.Employees.Queries;

public sealed record GetEmployeeListQuery : IRequest<Result<IReadOnlyList<EmployeeListItemDto>>>;

public interface IGetEmployeeListQueryHandler
{
    Task<Result<IReadOnlyList<EmployeeListItemDto>>> Handle(
        GetEmployeeListQuery query,
        CancellationToken cancellationToken = default);
}

public sealed class GetEmployeeListQueryHandler : 
IRequestHandler<GetEmployeeListQuery, 
    Result<IReadOnlyList<EmployeeListItemDto>>>
{
    private readonly IRepository<Employee, string> _employeeRepository;

    public GetEmployeeListQueryHandler(IRepository<Employee, string> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<IReadOnlyList<EmployeeListItemDto>>> Handle(
        GetEmployeeListQuery query,
        CancellationToken cancellationToken = default)
    {
        var employees = await _employeeRepository.GetAllAsync(cancellationToken);

        var list = employees
            .Select(employee => new EmployeeListItemDto(
                employee.Rut,
                employee.FirstName,
                employee.LastName,
                employee.WorkPlaceId,
                employee.Size,
                employee.PictureUrl,
                employee.IsDeleted,
                employee.TicketId))
            .ToList();

        return Result<IReadOnlyList<EmployeeListItemDto>>.Success(list);
    }
}
