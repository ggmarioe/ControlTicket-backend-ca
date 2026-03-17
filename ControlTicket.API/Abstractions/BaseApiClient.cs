using ControlTicket.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace ControlTicket.API.Base;

public abstract class BaseApiClient : ControllerBase
{
    protected async Task<Result<T>> ExecuteAsync<T>(Func<Task<Result<T>>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            var error = new Error("UnhandledException", ex.Message, ErrorType.Failure);
            return Result.Failure<T>(error);
        }
    }
}
