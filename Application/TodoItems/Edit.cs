#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Core.Exceptions;
using Persistence;
using Domain;

namespace Application.TodoItems
{
    public class Edit
    {
        public class Command : IRequest<TodoItemDTO>
        {
            public TodoItemDTO TodoItemDTO { get; set; }
            public Guid Id { get; set; }
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
                var inputItem = request.TodoItemDTO.ToRawModel();

                if (request.Id != inputItem.Id)
                    throw new BadRequestException();

                _context.Entry(inputItem).State = EntityState.Modified;

                var item = await _context.TodoItems
                                         .Include(x => x.Comments)
                                         .FirstOrDefaultAsync(x => x.Id == request.Id);
                if (item == null)
                    throw new NotFoundException();

                inputItem.CreatedAt = item.CreatedAt;

                await _context.SaveChangesAsync();

                return item.ToDTO();
            }
        }
    }
}