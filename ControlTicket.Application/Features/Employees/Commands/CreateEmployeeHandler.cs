using ControlTicket.Domain.Employees;
using ControlTicket.SharedKernel.Results;

namespace ControlTicket.Application.Features.Employees.Commands;

public static class CreateEmployeeErrors
{
    public static Error RutAlreadyUsed(string rut)
        => new("APP.CreateEmployee.RutAlreadyUsed", $"Rut '{rut}' is already used.", ErrorType.Conflict);   
}

public sealed class CreateEmployeeHandler
{
    private readonly IEmployeeRepository _repo;

    public CreateEmployeeHandler(IEmployeeRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result<string>> Handle(CreateEmployeeCommand cmd, CancellationToken ct)
    {
        // 1) Dominio valida invariantes
        var created = Employee.Create(
            cmd.Rut, 
            cmd.FirstName, 
            cmd.LastName,
            cmd.WorkPlaceId,
            cmd.Size);
        if (created.IsFailure)
            return Result<string>.Failure(created.Errors);

        var employee = created.Value!;

        // 2) Regla de aplicación (caso de uso)
        if (await _repo.RutExistsAsync(employee.Rut, ct))
            return Result<string>.Failure(CreateEmployeeErrors.RutAlreadyUsed(employee.Rut));

        // 3) Persistencia
        await _repo.AddAsync(employee, ct);

        // 4) Retorno OK
        return Result<string>.Success(employee.Rut);
    }
}