using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Users;
using Application.Core.Exceptions;
using Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Pagination.EntityFrameworkCore.Extensions;

namespace API.Controllers.Account
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO.Public>> GetUserInfo(string id)
        {
            try
            {
                return await Mediator.Send(new Details.Public.Query { Id = id, UserId = _userId });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public async Task<ActionResult<UserDTO.Me>> GetMyUserInfo()
        {
            try
            {
                return await Mediator.Send(new Details.Me.Query { UserId = _userId });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("me")]
        public async Task<ActionResult<UserDTO.Me>> EditMyUserInfo(UserDTO.Me user)
        {
            try
            {
                return await Mediator.Send(new Edit.Command { User = user, UserId = _userId });
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me/todoItems")]
        public async Task<ActionResult<Pagination<Application.TodoItems.TodoItemDTO>>>
            GetMyTodoItems([FromQuery] Application.TodoItems.Query.QueryParameter param)
        {
            param.User = _userId;
            return await Mediator.Send(new Application.TodoItems.List.Query(param, _userId));
        }
    }
}