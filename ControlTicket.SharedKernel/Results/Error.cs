namespace ControlTicket.SharedKernel.Results;

public sealed record Error(
    string Code,
    string Message,
    ErrorType Type = ErrorType.Failure,
    IReadOnlyDictionary<string, object?>? Metadata = null
);