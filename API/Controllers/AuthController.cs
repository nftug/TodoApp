using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Users.Services;
using MediatR;
using Domain.Users.Entities;
using Domain.Shared.Interfaces;

namespace API.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IUserRepository _userRepository;

    public AuthController(ISender mediator, IRepository<User> userRepository)
        : base(mediator)
    {
        _userRepository = (IUserRepository)userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command)
        => await HandleResult(() => _userRepository.LoginAsync(command));

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
        => await HandleRequest(new Application.Users.UseCases.Register.Command(command));
}
