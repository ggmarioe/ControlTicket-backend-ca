namespace ControlTicket.Application.Features.Employees.Dtos;
public sealed record EmployeeListItemDto(
    string Rut,
    string FirstName,
    string LastName,
    int WorkPlaceId,
    string Size);

