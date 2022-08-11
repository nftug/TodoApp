using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Users.Services;
using MediatR;
using Domain.Users.Entities;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(ISender mediator, IUserRepository userRepository)
        : base(mediator)
    {
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
        => await HandleResult(() => _userRepository.LoginAsync(command));

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
        => await HandleRequest(new Application.Users.UseCases.Register.Command(command));
}
