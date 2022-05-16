using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Core;

namespace API.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

    protected string _userId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsNotFound) return NotFound();
        if (result.IsSuccess && result.Value == null)
            return NotFound();
        if (result.IsSuccess && result.Value is Unit)
            return NoContent();
        if (result.IsSuccess && result.Value != null)
            return Ok(result.Value);
        if (result.Error?.Field != null && result.Error?.Message != null)
        {
            ModelState.AddModelError(result.Error.Field, result.Error.Message);
            return ValidationProblem();
        }

        return BadRequest();
    }
}
