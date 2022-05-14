using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender? _mediator;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>()!;

        protected string _userId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
    }
}