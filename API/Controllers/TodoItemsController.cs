using Microsoft.AspNetCore.Mvc;
using Application.TodoItems;
using Application.Core.Exceptions;
using Persistence;
using Application.TodoItems.Query;
using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ApiControllerBase
    {
        private readonly DataContext _context;

        public TodoItemsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<Pagination<TodoItemDTO>>>
            GetTodoItems([FromQuery] QueryParameter param)
        {
            return await Mediator.Send(new List.Query(param, _userId));
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(Guid id)
        {
            try
            {
                return await Mediator.Send(new Details.Query { Id = id, UserId = _userId });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItemDTO>> PutTodoItem(Guid id, TodoItemDTO todoItemDTO)
        {
            try
            {
                var result = await Mediator.Send(
                    new Edit.Command { Id = id, TodoItemDTO = todoItemDTO, UserId = _userId }
                );
                return AcceptedAtAction(nameof(GetTodoItem), new { id = id }, result);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoItemDTO)
        {
            try
            {
                var result = await Mediator.Send(new Create.Command { TodoItemDTO = todoItemDTO });
                return CreatedAtAction(nameof(GetTodoItem), new { id = todoItemDTO.Id }, result);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            try
            {
                await Mediator.Send(new Delete.Command { Id = id, UserId = _userId });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
