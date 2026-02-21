namespace ControlTicket.SharedKernel;

/// <summary>
/// Specialised result that holds multiple validation errors.
/// </summary>
public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}

/// <summary>
/// Specialised generic result that holds multiple validation errors alongside a typed value.
/// </summary>
public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(default, false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
}

/// <summary>
/// Marker interface shared by both validation result types.
/// </summary>
public interface IValidationResult
{
    public static readonly Error ValidationError =
        Error.Validation("General.Validation", "One or more validation errors occurred.");

    Error[] Errors { get; }
}
