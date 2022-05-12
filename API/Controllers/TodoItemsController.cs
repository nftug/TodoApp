using Microsoft.AspNetCore.Mvc;
using Application.TodoItems;
using Application.Core.Exceptions;
using Persistence;
using Domain;
using Application.TodoItems.Query;
using Pagination.EntityFrameworkCore.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ApiControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<Pagination<TodoItemDTO>>>
            GetTodoItems([FromQuery] QueryParameter param)
        {
            return await Mediator.Send(new List.Query(param));
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(Guid id)
        {
            try
            {
                return await Mediator.Send(new Details.Query { Id = id });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            try
            {
                await Mediator.Send(new Edit.Command { Id = id, TodoItem = todoItem });
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return AcceptedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem.ItemToDTO());
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            await Mediator.Send(new Create.Command { TodoItem = todoItem });
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem.ItemToDTO());
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            try
            {
                await Mediator.Send(new Delete.Command { Id = id });
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
