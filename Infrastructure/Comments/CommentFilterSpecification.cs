using Domain.Comments.Entities;
using Infrastructure.DataModels;
using Domain.Comments.Queries;
using Domain.Shared.Queries;
using Infrastructure.Shared.Specifications.Filter.Extensions;
using Infrastructure.Shared.Specifications.Filter;

namespace Infrastructure.Comments;

internal class CommentFilterSpecification : FilterSpecificationBase<Comment, CommentDataModel>
{
    public CommentFilterSpecification(DataContext context)
        : base(context)
    {
    }

    protected override IQueryable<IDataModel<Comment>> GetQueryByParameter(
        IQueryable<IDataModel<Comment>> source,
        IQueryParameter<Comment> param
    )
    {
        var _param = (CommentQueryParameter)param;

        ExpressionGroups.AddSimpleSearch(_param.UserId, x => x.OwnerUserId == _param.UserId);

        ExpressionGroups.AddSearch(_param.Q, k => k.Contains("Content"));

        ExpressionGroups.AddSearch(_param.Content, k => k.Contains("Content"));

        ExpressionGroups.AddSearch(_param.UserName, k => k.ContainsInChild("OwnerUser", "UserName"));

        return source.OfType<CommentDataModel>().ApplyExpressionGroup(ExpressionGroups);
    }
}
