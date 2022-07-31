using Domain.Comments.Entities;
using Infrastructure.DataModels;
using Infrastructure.Shared.Services.Repository;
using Infrastructure.Shared.Specifications.DataSource;

namespace Infrastructure.Comments;

public class CommentRepository : RepositoryBase<Comment>
{
    public CommentRepository(DataContext context) : base(context)
    {
    }

    protected override IDataSourceSpecification<Comment> DataSource
        => new CommentDataSourceSpecification(_context);

    protected override async Task AddEntityAsync(IDataModel<Comment> entity)
        => await _context.Comment.AddAsync((CommentDataModel)entity);

    protected override void UpdateEntity(IDataModel<Comment> entity)
        => _context.Comment.Update((CommentDataModel)entity);

    protected override void RemoveEntity(IDataModel<Comment> entity)
        => _context.Comment.Remove((CommentDataModel)entity);

    protected override IDataModel<Comment> ToDataModel(Comment origin)
        => new CommentDataModel(origin);

    protected override void Transfer(Comment origin, IDataModel<Comment> dataModel)
        => ((CommentDataModel)dataModel).Transfer(origin);
}
