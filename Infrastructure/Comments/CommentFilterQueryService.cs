using Domain.Comments.Entities;
using Infrastructure.DataModels;
using Infrastructure.Shared.Services.FilterQuery;
using Infrastructure.Shared.Specifications.DataSource;
using Infrastructure.Shared.Specifications.Filter;

namespace Infrastructure.Comments;

public class CommentFilterQueryService : FilterQueryServiceBase<Comment, CommentDataModel>
{
    public CommentFilterQueryService(DataContext context) : base(context)
    {
    }

    protected override IDataSourceSpecification<Comment> DataSource
        => new CommentDataSourceSpecification(_context);

    protected override IFilterSpecification<Comment, CommentDataModel> FilterSpecification
        => new CommentFilterSpecification(_context);
}
