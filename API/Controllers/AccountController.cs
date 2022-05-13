using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain;
using API.Extensions;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AccountController : ApiControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login(LoginModel loginModel)
        {
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (result.Succeeded)
                return CreateUserObject(user);
            else
                return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(RegisterModel registerModel)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerModel.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerModel.Username))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();
            }

            var user = new ApplicationUser
            {
                Email = registerModel.Email,
                UserName = registerModel.Username
            };

            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(error.Code, error.Description);

                return ValidationProblem();
            }
        }

        private UserModel CreateUserObject(ApplicationUser user)
            => new UserModel
            {
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                Id = user.Id
            };
    }
}