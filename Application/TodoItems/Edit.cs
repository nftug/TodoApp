using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using Persistence;
using AutoMapper;

namespace Application.TodoItems;

public class Edit
{
    public class Command : IRequest<Result<TodoItemDTO>>
    {
        public TodoItemDTO TodoItemDTO { get; set; }
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Command(Guid id, TodoItemDTO todoItemDTO, string userId)
        {
            TodoItemDTO = todoItemDTO;
            Id = id;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Command, Result<TodoItemDTO>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<TodoItemDTO>> Handle(Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.TodoItemDTO;

            if (request.Id != inputItem?.Id)
                return Result<TodoItemDTO>.Failure("id", "Incorrect id");

            var item = await _context.TodoItems
                                     .Include(x => x.Comments)
                                     .FirstOrDefaultAsync(
                                        x => x.Id == request.Id &&
                                        x.CreatedById == request.UserId
                                     );
            if (item == null)
                return Result<TodoItemDTO>.NotFound();

            _mapper.Map(inputItem, item);

            await _context.SaveChangesAsync();

            var result = _mapper.Map<TodoItemDTO>(item);
            return Result<TodoItemDTO>.Success(result);
        }
    }
}
