using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Shared;

namespace API.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

    protected string _userId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    protected async Task<IActionResult> HandleResult<T>(Func<Task<T>> cb)
    {
        try
        {
            var result = await cb();
            if (result == null) return NotFound();

            return Ok(result);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
        catch (BadRequestException)
        {
            return BadRequest();
        }
        catch (DomainException exc)
        {
            ModelState.AddModelError(exc.Field, exc.Message);
            return ValidationProblem();
        }
    }
}
