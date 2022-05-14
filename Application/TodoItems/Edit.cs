#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Core.Exceptions;
using Persistence;
using AutoMapper;

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
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<TodoItemDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var inputItem = request.TodoItemDTO;

                if (request.Id != inputItem.Id)
                    throw new BadRequestException();

                var item = await _context.TodoItems
                                         .Include(x => x.Comments)
                                         .FirstOrDefaultAsync(
                                            x => x.Id == request.Id &&
                                            x.CreatedById == request.UserId
                                         );
                if (item == null)
                    throw new NotFoundException();

                _mapper.Map(inputItem, item);

                await _context.SaveChangesAsync();

                return _mapper.Map<TodoItemDTO>(item);
            }
        }
    }
}