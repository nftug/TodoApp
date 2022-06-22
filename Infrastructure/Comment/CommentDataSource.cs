using AutoMapper;
using Domain.Comment;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Infrastructure.Shared.DataSource;

namespace Infrastructure.Comment;

public class CommentDataSource : DataSourceBase<CommentModel>
{
    public CommentDataSource(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override IQueryable<IEntity<CommentModel>> Source => _context.Comment;

    public override CommentModel MapToDomain(IEntity<CommentModel> entity)
        => _mapper.Map<CommentModel>(entity);

    public override IEntity<CommentModel> MapToEntity(CommentModel item)
        => _mapper.Map<CommentDataModel>(item);

    public override void Transfer(CommentModel item, IEntity<CommentModel> entity)
        => _mapper.Map(item, entity);

    public override async Task AddEntityAsync(IEntity<CommentModel> entity)
        => await _context.Comment.AddAsync((CommentDataModel)entity);

    public override void UpdateEntity(IEntity<CommentModel> entity)
        => _context.Comment.Update((CommentDataModel)entity);

    public override void RemoveEntity(IEntity<CommentModel> entity)
        => _context.Comment.Remove((CommentDataModel)entity);
}
