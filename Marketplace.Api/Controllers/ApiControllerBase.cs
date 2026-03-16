using Marketplace.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Marketplace.Api.Controllers;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    protected ActionResult HandleFailure<T>(Result<T> result)
    {
        return result.ErrorType switch
        {
            ErrorType.NotFound => NotFound(new { Message = result.ErrorMessage }),
            ErrorType.BadRequest => BadRequest(new { Message = result.ErrorMessage }),
            ErrorType.Unauthorized => Unauthorized(new { Message = result.ErrorMessage }),
            ErrorType.Conflict => Conflict(new { Message = result.ErrorMessage }),
            _ => BadRequest(new { Message = result.ErrorMessage })
        };
    }
}
