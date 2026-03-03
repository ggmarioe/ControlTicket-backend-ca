using ControlTicket.Domain.Employees;
using ControlTicket.SharedKernel.Results;
using ControlTicket.Domain.Common;
using ControlTicket.Application.Features.Employees.Dtos;
using ControlTicket.Application.Common.Messaging;
using ControlTicket.Application.Features.Employees.Commands;

namespace ControlTicket.Repository.Employees;

public static class CreateEmployeeErrors
{
    public static Error RutAlreadyUsed(string rut)
        => new("Application.CreateEmployee.RutAlreadyUsed", $"Rut '{rut}' is already used.", ErrorType.Conflict);
}

public sealed class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<string>>
{
    private readonly IRepository<Employee, string> _employeeRepository;

    public CreateEmployeeCommandHandler(IRepository<Employee, string> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public Task<Result<string>> Handle(CreateEmployeeCommand request, CancellationToken ct)
    {
        var dto = new CreateEmployeeDto(
            request.Rut,
            request.FirstName,
            request.LastName,
            request.WorkPlaceId,
            request.Size,
            request.PictureUrl);

        return Handle(dto, ct);
    }

    private async Task<Result<string>> Handle(CreateEmployeeDto cmd, CancellationToken ct)
    {
        // 1) Dominio valida invariantes
        var employee = Employee.Create(
            cmd.Rut,
            cmd.FirstName,
            cmd.LastName,
            cmd.WorkPlaceId,
            cmd.Size,
            cmd.PictureUrl);

        if (employee.IsFailure)
            return Result<string>.Failure(employee.Errors);

        var employeeEntity = employee.Value!;

        // 2) Persistencia
        await _employeeRepository.AddAsync(employeeEntity, ct);

        // 3) Retorno OK
        return Result<string>.Success(employeeEntity.Rut);
    }
}