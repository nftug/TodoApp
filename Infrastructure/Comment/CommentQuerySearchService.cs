using Microsoft.EntityFrameworkCore;
using Infrastructure.Shared.QuerySearch;
using Infrastructure.Shared.QuerySearch.Models;
using Infrastructure.Shared.QuerySearch.Extensions;
using Domain.Interfaces;
using Domain.Comment;
using Infrastructure.DataModels;

namespace Infrastructure.Comment;

public class CommentQuerySearchService
    : QuerySearchServiceBase<CommentModel>
{
    public CommentQuerySearchService(DataContext context)
        : base(context)
    {
    }

    public override IQueryable<CommentDataModel> GetFilteredQuery
        (IQueryParameter<CommentModel> param)
    {
        var _param = (CommentQueryParameter)param;
        var query = _context.Comment.Include(x => x.OwnerUser).AsQueryable();

        var expressionGroup = new List<ExpressionGroup<CommentDataModel>>();

        // ユーザーIDで絞り込み
        var userIdField = new SearchField<CommentDataModel>();
        if (_param.UserId != null)
        {
            userIdField.Node.AddExpression(
                x => x.OwnerUserId == _param.UserId,
                Keyword.CreateDummy()
            );
        }
        expressionGroup.AddExpressionNode(userIdField);

        // qで絞り込み
        var qField = new SearchField<CommentDataModel>(_param.Q);
        foreach (var keyword in GetKeyword(_param.Q))
            qField.Node.AddExpression(x =>
                (keyword.InQuotes ? x.Content : x.Content.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(qField);

        // 内容で絞り込み
        var contentField = new SearchField<CommentDataModel>(_param.Content);
        foreach (var keyword in GetKeyword(_param.Content))
            contentField.Node.AddExpression(x =>
                (keyword.InQuotes ? x.Content : x.Content.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(contentField);

        // ユーザー名で絞り込み
        var userNameField = new SearchField<CommentDataModel>(_param.UserName);
        foreach (var keyword in GetKeyword(_param.UserName))
            userNameField.Node.AddExpression(x =>
                (keyword.InQuotes
                    ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower())
                    .Contains(keyword.Value),
                keyword
            );
        expressionGroup.AddExpressionNode(userNameField);

        query = query.ApplyExpressionGroup(expressionGroup);

        return query;
    }
}
