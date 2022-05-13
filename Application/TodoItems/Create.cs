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
            public string UserId { get; set; }
        }

        public class Handler : IRequestHandler<Command, TodoItemDTO>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<TodoItemDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = request.TodoItemDTO.ToRawModel();
                item.CreatedAt = DateTime.Now;
                item.CreatedById = request.UserId;

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