using ControlTicket.Application.Common.Messaging;
using ControlTicket.SharedKernel.Results;

namespace ControlTicket.Application.Features.Employees.Commands;

public sealed record CreateEmployeeCommand(
    string Rut, 
    string FirstName, 
    string LastName,
    int WorkPlaceId,
    string Size,
    string PictureUrl) : IRequest<Result<string>>;