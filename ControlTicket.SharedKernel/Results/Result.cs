namespace ControlTicket.SharedKernel.Results;

public class Result
{
    protected Result(bool isSuccess, IReadOnlyList<Error> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors ?? Array.Empty<Error>();
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    public IReadOnlyList<Error> Errors { get; }

    // Conveniencia: “error principal”
    public Error? PrimaryError => Errors.Count > 0 ? Errors[0] : null;

    public static Result Success() => new(true, Array.Empty<Error>());

    public static Result Failure(Error error)
        => new(false, new[] { error });

    public static Result Failure(IReadOnlyList<Error> errors)
    {
        if (errors is null || errors.Count == 0)
            throw new ArgumentException("errors must contain at least one error.", nameof(errors));

        return new Result(false, errors);
    }
}

public sealed class Result<T> : Result
{
    private Result(bool isSuccess, T? value, IReadOnlyList<Error> errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value)
        => new(true, value, Array.Empty<Error>());

    public static new Result<T> Failure(Error error)
        => new(false, default, new[] { error });

    public static new Result<T> Failure(IReadOnlyList<Error> errors)
    {
        if (errors is null || errors.Count == 0)
            throw new ArgumentException("errors must contain at least one error.", nameof(errors));

        return new Result<T>(false, default, errors);
    }
}