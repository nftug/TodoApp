using Domain.Comments.Entities;
using Infrastructure.DataModels;
using Domain.Comments.Queries;
using Domain.Shared.Queries;
using Infrastructure.Shared.Specifications.Filter.Extensions;
using Infrastructure.Shared.Specifications.Filter.Models;
using Infrastructure.Shared.Specifications.Filter;

namespace Infrastructure.Comments;

internal class CommentFilterSpecification : FilterSpecificationBase<Comment>
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

        var expressionGroup = new List<ExpressionGroup<CommentDataModel>>();

        // ユーザーIDで絞り込み
        expressionGroup.AddSimpleSearch(_param.UserId, x => x.OwnerUserId == _param.UserId);

        // qで絞り込み
        expressionGroup.AddSearch(_param.Q, k => x =>
            (k.InQuotes ? x.Content : x.Content.ToLower()).Contains(k.Value));

        // 内容で絞り込み
        expressionGroup.AddSearch(_param.Content, k => x =>
            (k.InQuotes ? x.Content : x.Content.ToLower()).Contains(k.Value));

        // ユーザー名で絞り込み
        expressionGroup.AddSearch(_param.UserName, k => x =>
            (k.InQuotes ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower()).Contains(k.Value));

        return source.OfType<CommentDataModel>().ApplyExpressionGroup(expressionGroup);
    }
}
