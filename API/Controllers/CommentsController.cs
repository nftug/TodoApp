using Microsoft.AspNetCore.Mvc;
using Application.Comments;
using Application.Core.Exceptions;
using Persistence;
using Application.Comments.Query;
using Pagination.EntityFrameworkCore.Extensions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ApiControllerBase
    {
        private readonly DataContext _context;

        public CommentsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<Pagination<CommentDTO>>>
            GetComments([FromQuery] QueryParameter param)
        {
            return await Mediator.Send(new List.Query(param));
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetComment(Guid id)
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

        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CommentDTO>> PutComment(Guid id, CommentDTO commentDTO)
        {
            try
            {
                var result = await Mediator.Send(new Edit.Command { Id = id, CommentDTO = commentDTO });
                return AcceptedAtAction(nameof(GetComment), new { id = id }, result);
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

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> PostComment(CommentDTO commentDTO)
        {
            try
            {
                var result = await Mediator.Send(new Create.Command { CommentDTO = commentDTO });
                return CreatedAtAction(nameof(GetComment), new { id = commentDTO.Id }, result);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
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
