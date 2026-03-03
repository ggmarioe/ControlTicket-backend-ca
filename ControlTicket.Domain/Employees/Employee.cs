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

    public string PictureUrl { get; set; } = string.Empty;
    public bool IsDeleted { get; set; } = false;
    public long? TicketId { get; set; } = null;
     public static Result<Employee> Create(
        string rut, 
        string firstName , 
        string lastName,
        int workPlaceId,
        string size,
        string pictureUrl)
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
            Size = size.Trim(),
            PictureUrl = (pictureUrl ?? string.Empty).Trim(),
            IsDeleted = false,
            TicketId = null
        };

        return Result<Employee>.Success(employee);
    }
}