namespace ControlTicket.Application.Features.Employees.Dtos;

public sealed record CreateEmployeeDto(
    string Rut,
    string FirstName,
    string LastName,
    int WorkPlaceId,
    string Size,
    string PictureUrl);