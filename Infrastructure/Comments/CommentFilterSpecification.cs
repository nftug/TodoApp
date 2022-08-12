using Domain.Comments.Entities;
using Infrastructure.DataModels;
using Domain.Comments.Queries;
using Domain.Shared.Queries;
using Infrastructure.Shared.Specifications.Filter;

namespace Infrastructure.Comments;

internal class CommentFilterSpecification : FilterSpecificationBase<Comment, CommentDataModel>
{
    public CommentFilterSpecification(DataContext context)
        : base(context)
    {
    }

    protected override void AddQueryByParameter(IQueryable<IDataModel<Comment>> source, IQueryParameter<Comment> param)
    {
        var _param = (CommentQueryParameter)param;

        AddQuery(_param.UserId, x => x.OwnerUserId == _param.UserId);

        AddContains(_param.Q, x => x.Content);

        AddContains(_param.Content, x => x.Content);

        AddContains(_param.UserName, x => x.OwnerUser!.UserName);
    }
}
