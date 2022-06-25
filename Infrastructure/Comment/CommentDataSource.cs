using AutoMapper;
using Domain.Comment;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Infrastructure.Services.DataSource;

namespace Infrastructure.Comment;

public class CommentDataSource : DataSourceBase<CommentModel>
{
    public CommentDataSource(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override IQueryable<CommentDataModel> Source => _context.Comment;

    public override IEntity<CommentModel> MapToEntity(CommentModel item)
        => _mapper.Map<CommentDataModel>(item);

    public override async Task AddEntityAsync(IEntity<CommentModel> entity)
        => await _context.Comment.AddAsync((CommentDataModel)entity);

    public override void UpdateEntity(IEntity<CommentModel> entity)
        => _context.Comment.Update((CommentDataModel)entity);

    public override void RemoveEntity(IEntity<CommentModel> entity)
        => _context.Comment.Remove((CommentDataModel)entity);
}
