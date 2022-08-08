using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Shared.Exceptions;

namespace API.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

    protected Guid _userId
    {
        get
        {
            var idString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return idString != null ? Guid.Parse(idString) : new Guid();
        }
    }

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

    protected async Task<IActionResult> HandleRequest<T>(IRequest<T> request)
    {
        return await HandleResult(() => Mediator.Send(request));
    }
}
