using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain;
using API.Models;
using API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : AccountControllerBase
    {
        public UsersController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            TokenService tokenService
        ) : base(userManager, signInManager, tokenService)
        {
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel.Public>> GetUserInfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();

            return new UserModel.Public
            {
                Id = user.Id,
                Username = user.UserName
            };
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<ActionResult<UserModel.Private>> GetMyUserInfo()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null) return NotFound();

            return new UserModel.Private
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email
            };
        }
    }
}