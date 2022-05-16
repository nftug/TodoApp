using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Core;

namespace API.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

    protected string _userId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    protected ActionResult HandleResult<T>(Result<T>? result)
    {
        if (result == null) return NotFound();
        if (result.IsSuccess && result.Value != null)
            return Ok(result.Value);
        if (result.IsSuccess && result.Value == null)
            return NotFound();
        else
            return BadRequest();
    }
}
