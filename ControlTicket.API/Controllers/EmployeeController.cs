using ControlTicket.API.Base;
using ControlTicket.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace ControlTicket.API.Contollers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : BaseApiClient
{
    
    [HttpGet]
    public async Task<Result<string[]>> Get()
    {
        return await ExecuteAsync(async () =>
        {
            var employees = new[] { "a", "b", "c" };
            return Result<string[]>.Success(employees);
        });
    }
}