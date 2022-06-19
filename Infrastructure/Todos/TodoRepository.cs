using Domain.Todos;
using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared.Repository;
using AutoMapper;

namespace Infrastructure.Todos;

public class TodoRepository : RepositoryBase<Todo, TodoDataModel>
{
    public TodoRepository(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override async Task<Todo?> FindAsync(Guid id)
    {
        var data = await _context.Todos
            .Include(x => x.Comments)
            .FirstOrDefaultAsync(x => x.Id == id);

        return data != null ? _mapper.Map<Todo>(data) : null;
    }
}
