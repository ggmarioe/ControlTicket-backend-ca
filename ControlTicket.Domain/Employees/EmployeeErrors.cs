using ControlTicket.SharedKernel.Results;
    
namespace ControlTicket.Domain.Employees;

public static class EmployeeErrors
{
    public static Error NameRequired()
        => new("DOM.Employee.NameRequired", "Employee name is required.", ErrorType.Validation);

    public static Error InvalidEmail()
        => new("DOM.Employee.InvalidEmail", "Employee email is invalid.", ErrorType.Validation);
    
    public static Error LastNameRequired()
        => new("DOM.Employee.LastNameRequired", "Employee last name is required.", ErrorType.Validation);   

}