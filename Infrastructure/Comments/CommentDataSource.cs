using AutoMapper;
using Domain.Comments;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Infrastructure.Shared.DataSource;

namespace Infrastructure.Comments;

public class CommentDataSource : DataSourceBase<Comment>
{
    public CommentDataSource(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override IQueryable<IEntity<Comment>> Source => _context.Comments;

    public override Comment MapToDomain(IEntity<Comment> entity)
        => _mapper.Map<Comment>(entity);

    public override IEntity<Comment> MapToEntity(Comment item)
        => _mapper.Map<CommentDataModel>(item);

    public override void Transfer(Comment item, IEntity<Comment> entity)
        => _mapper.Map(item, entity);

    public override async Task AddEntityAsync(IEntity<Comment> entity)
        => await _context.Comments.AddAsync((CommentDataModel)entity);

    public override void UpdateEntity(IEntity<Comment> entity)
        => _context.Comments.Update((CommentDataModel)entity);

    public override void RemoveEntity(IEntity<Comment> entity)
        => _context.Comments.Remove((CommentDataModel)entity);
}
