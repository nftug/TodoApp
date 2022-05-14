#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain;
using Persistence;
using AutoMapper;

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
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TodoItemDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var item = _mapper.Map<TodoItem>(request.TodoItemDTO);
                item.CreatedAt = DateTime.Now;
                item.CreatedById = request.UserId;

                _context.TodoItems.Add(item);
                await _context.SaveChangesAsync();

                return await _mapper.ProjectTo<TodoItemDTO>(_context.TodoItems)
                                    .FirstOrDefaultAsync(x => x.Id == item.Id);
            }
        }
    }
}