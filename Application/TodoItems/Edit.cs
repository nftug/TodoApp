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
                var inputItem = request.TodoItemDTO.ToRawModel();

                if (request.Id != inputItem.Id)
                    throw new BadRequestException();

                _context.Entry(inputItem).State = EntityState.Modified;

                var item = await _context.TodoItems
                                         .Include(x => x.Comments)
                                         .FirstOrDefaultAsync(
                                                 x => x.Id == request.Id &&
                                                 x.CreatedById == request.UserId
                                         );
                if (item == null)
                    throw new NotFoundException();

                inputItem.CreatedAt = item.CreatedAt;
                inputItem.CreatedById = request.UserId;

                await _context.SaveChangesAsync();

                return item.ToDTO();
            }
        }
    }
}