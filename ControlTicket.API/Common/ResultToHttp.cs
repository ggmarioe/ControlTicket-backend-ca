using Microsoft.AspNetCore.Mvc;
using ControlTicket.SharedKernel.Results;

namespace ControlTicket.API.Common;

public static class ResultToHttp
{
    public static IActionResult ToActionResult<T>(this ControllerBase controller, Result<T> result)
    {
        if (result.IsSuccess)
            return controller.Ok(result.Value);

        return controller.ProblemFromResult(result);
    }

    public static IActionResult ProblemFromResult(this ControllerBase controller, Result result)
    {
        var error = result.PrimaryError ?? new Error("APP.Unknown", "Unknown error.");

        var status = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };

        var pd = new ProblemDetails
        {
            Status = status,
            Title = error.Code,
            Detail = error.Message
        };

        if (result.Errors.Count > 1)
            pd.Extensions["errors"] = result.Errors.Select(e => new { e.Code, e.Message, e.Type, e.Metadata });

        return controller.StatusCode(status, pd);
    }
}