using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NestedComments.Api.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("error")]
    public IActionResult HandleError()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return Problem(
            detail: exception?.Message ?? "Unexpected error",
            statusCode: 500,
            title: "Internal Server Error"
        );
    }
}
