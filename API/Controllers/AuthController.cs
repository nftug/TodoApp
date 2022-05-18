using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using API.Models;
using API.Extensions;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserDataModel> _userManager;
    private readonly SignInManager<UserDataModel> _signInManager;
    private readonly TokenService _tokenService;

    public AuthController(
        UserManager<UserDataModel> userManager,
        SignInManager<UserDataModel> signInManager,
        TokenService tokenService
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenModel>> Login(LoginModel loginModel)
    {
        var user = await _userManager.FindByEmailAsync(loginModel.Email);
        if (user == null) return Unauthorized();

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
        if (result.Succeeded)
            return CreateUserObject(user);
        else
            return Unauthorized();
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<TokenModel>> Register(RegisterModel registerModel)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email == registerModel.Email))
        {
            ModelState.AddModelError("email", "このメールアドレスは既に使用済みです");
            return ValidationProblem();
        }

        var user = new UserDataModel
        {
            Email = registerModel.Email,
            UserName = registerModel.Username
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return ValidationProblem();
        }

        return CreateUserObject(user);
    }

    private TokenModel CreateUserObject(UserDataModel user)
        => new TokenModel
        {
            Token = _tokenService.CreateToken(user)
        };
}
