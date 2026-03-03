using ControlTicket.Application.Features.Employees.Commands;
using ControlTicket.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using ControlTicket.API.Common;
using ControlTicket.SharedKernel.Results;
using ControlTicket.Application.Features.Employees.Dtos;
using ControlTicket.Application.Features.Employees.Queries;
using ControlTicket.Application.Common.Messaging;


namespace ControlTicket.API.Contollers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IGetEmployeeByRutQueryHandler _getEmployeeByRutQueryHandler;
    private readonly IMediator _mediator;

    public EmployeesController(
        IGetEmployeeByRutQueryHandler getEmployeeByRutQueryHandler,
        IMediator mediator)
    {
        
        _getEmployeeByRutQueryHandler = getEmployeeByRutQueryHandler;
        _mediator = mediator;
    }

   
    [HttpGet]
    public async Task<Result<IReadOnlyList<EmployeeListItemDto>>> GetAllEmployeeeList()
    {
        return await _mediator.Send(new GetEmployeeListQuery(), CancellationToken.None);
    }

    [HttpGet("{rut}")]
    public async Task<Result<EmployeeListItemDto?>> GetEmployeeByRut([FromRoute] string rut)
    {
        var query = await _getEmployeeByRutQueryHandler.Handle(new GetEmployeeByRutQuery(rut));
        return query;
    }

    [HttpPost]
    public async Task<Result<string>> CreateEmployee([FromBody] CreateEmployeeCommand request)
    {
        var command =  new CreateEmployeeCommand(
            request.Rut,
            request.FirstName,
            request.LastName,
            request.WorkPlaceId,
            request.Size,
            request.PictureUrl);
        return await _mediator.Send(command, CancellationToken.None);
    }

}