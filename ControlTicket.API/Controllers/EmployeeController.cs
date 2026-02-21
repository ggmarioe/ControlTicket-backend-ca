using Microsoft.AspNetCore.Mvc;

namespace ControlTicket.API.Contollers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
   
    [HttpGet]
    public async Task<List<string>> Get()
    {
       
        return ["a","b","c"];
    }
}