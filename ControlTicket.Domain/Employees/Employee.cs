using ControlTicket.Domain.Common;
using ControlTicket.SharedKernel.Results;

namespace ControlTicket.Domain.Employees;

public class Employee
{
    public string Rut { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int WorkPlaceId { get; set; } = 0;
    public string Size { get; set; } = string.Empty;
     public static Result<Employee> Create(
        string rut, 
        string firstName , 
        string lastName,
        int workPlaceId,
        string size)
    {
        var errors = new List<Error>();

        if (string.IsNullOrWhiteSpace(firstName))
            errors.Add(EmployeeErrors.NameRequired());

        if (errors.Count > 0)
            return Result<Employee>.Failure(errors);

        var employee = new Employee
        {
            Rut = rut,
            FirstName = firstName.Trim(), 
            LastName = lastName.Trim(),
            WorkPlaceId = workPlaceId,
            Size = size.Trim()
        };

        return Result<Employee>.Success(employee);
    }
}