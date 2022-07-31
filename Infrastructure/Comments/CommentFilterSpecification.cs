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
        var userIdField = new SearchField<CommentDataModel>();
        if (_param.UserId != null)
            userIdField.Node.AddExpression(
                Keyword.CreateDummy(),
                x => x.OwnerUserId == _param.UserId
            );
        expressionGroup.AddExpressionNode(userIdField);

        // qで絞り込み
        var qField = new SearchField<CommentDataModel>(_param.Q);
        foreach (var k in GetKeyword(_param.Q))
            qField.Node.AddExpression(k, x =>
                (k.InQuotes ? x.Content : x.Content.ToLower()).Contains(k.Value)
            );
        expressionGroup.AddExpressionNode(qField);

        // 内容で絞り込み
        var contentField = new SearchField<CommentDataModel>(_param.Content);
        foreach (var k in GetKeyword(_param.Content))
            contentField.Node.AddExpression(k, x =>
                (k.InQuotes ? x.Content : x.Content.ToLower()).Contains(k.Value)
            );
        expressionGroup.AddExpressionNode(contentField);

        // ユーザー名で絞り込み
        var userNameField = new SearchField<CommentDataModel>(_param.UserName);
        foreach (var k in GetKeyword(_param.UserName))
            userNameField.Node.AddExpression(k, x =>
                (k.InQuotes
                    ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower())
                .Contains(k.Value)
            );
        expressionGroup.AddExpressionNode(userNameField);

        return source.OfType<CommentDataModel>().ApplyExpressionGroup(expressionGroup);
    }
}
