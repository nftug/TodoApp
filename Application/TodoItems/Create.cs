#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;

namespace Application.TodoItems
{
    public class Create
    {
        public class Command : IRequest<TodoItemDTO>
        {
            public TodoItemDTO TodoItemDTO { get; set; }
        }

        public class Handler : IRequestHandler<Command, TodoItemDTO>
        {
            private readonly TodoContext _context;

            public Handler(TodoContext context)
            {
                _context = context;
            }

            public async Task<TodoItemDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = request.TodoItemDTO.ToRawModel();
                item.CreatedAt = DateTime.Now;
                _context.TodoItems.Add(item);
                await _context.SaveChangesAsync();

                item = await _context.TodoItems
                                     .Include(x => x.Comments)
                                     .FirstOrDefaultAsync(x => x.Id == item.Id);

                return item.ToDTO();
            }
        }
    }
}