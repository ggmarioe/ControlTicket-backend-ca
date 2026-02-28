namespace ControlTicket.SharedKernel.Results;

public static class ResultExtensions
{
    // Map: transforma el valor si es éxito, si es fallo mantiene errores
    public static Result<TOut> Map<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
        => result.IsSuccess
            ? Result<TOut>.Success(mapper(result.Value!))
            : Result<TOut>.Failure(result.Errors);

    // Bind: compone operaciones que devuelven Result
    public static async Task<Result<TOut>> Bind<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, Task<Result<TOut>>> next)
        => result.IsSuccess
            ? await next(result.Value!)
            : Result<TOut>.Failure(result.Errors);
}