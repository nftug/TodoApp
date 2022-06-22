using Domain.Todos;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Infrastructure.Shared.Repository;
using Domain.Interfaces;

namespace Infrastructure.Todos;

public class TodoRepository : RepositoryBase<Todo>
{
    public TodoRepository
        (DataContext context, IMapper mapper, IDataSource<Todo> source)
        : base(context, mapper, source)
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