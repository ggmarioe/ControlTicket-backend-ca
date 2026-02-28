using ControlTicket.Application.Features.Employees.Commands;
using ControlTicket.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using ControlTicket.API.Common;
using ControlTicket.SharedKernel.Results;
using ControlTicket.Application.Features.Employees.Dtos;
using ControlTicket.Application.Features.Employees.Queries;

namespace ControlTicket.API.Contollers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IGetEmployeeByRutQueryHandler _getEmployeeByRutQueryHandler;
    private readonly IGetEmployeeListQueryHandler _getEmployeeListQueryHandler;

    public EmployeesController(
        IGetEmployeeByRutQueryHandler getEmployeeByRutQueryHandler,
        IGetEmployeeListQueryHandler getEmployeeListQueryHandler)
    {
        
        _getEmployeeByRutQueryHandler = getEmployeeByRutQueryHandler;
        _getEmployeeListQueryHandler = getEmployeeListQueryHandler;
    }

   
    [HttpGet]
    public async Task<Result<IReadOnlyList<EmployeeListItemDto>>> GetAllEmployeeeList()
    {
        var query = await _getEmployeeListQueryHandler.Handle(new GetEmployeeListQuery());
        return query;
    }

    [HttpGet("{rut}")]
    public async Task<Result<EmployeeListItemDto?>> GetEmployeeByRut([FromRoute] string rut)
    {
        var query = await _getEmployeeByRutQueryHandler.Handle(new GetEmployeeByRutQuery(rut));
        return query;
    }

}