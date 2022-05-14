using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Domain;
using API.Models;
using API.Extensions;

namespace API.Controllers.Account
{
    public abstract class AccountControllerBase : ControllerBase
    {
        protected readonly UserManager<ApplicationUser> _userManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected readonly TokenService _tokenService;

        public AccountControllerBase(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        protected TokenModel CreateUserObject(ApplicationUser user)
            => new TokenModel
            {
                Token = _tokenService.CreateToken(user)
            };
    }
}