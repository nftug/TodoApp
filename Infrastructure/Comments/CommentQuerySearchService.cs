using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared.QuerySearch;
using Infrastructure.Shared.QuerySearch.Models;
using Infrastructure.Shared.QuerySearch.Extensions;
using Domain.Interfaces;

namespace Infrastructure.Comments;

public class CommentQuerySearchService
    : QuerySearchServiceBase<CommentDataModel>
{
    public CommentQuerySearchService(DataContext context)
        : base(context)
    {
    }

    public override IQueryable<CommentDataModel> GetFilteredQuery
        (IQueryParameter<CommentDataModel> param)
    {
        var _param = (CommentQueryParameter)param;
        var query = _context.Comments.Include(x => x.OwnerUser).AsQueryable();

        var expressionsNode = new List<QuerySearchExpression<CommentDataModel>>();

        // ユーザーIDで絞り込み
        if (!string.IsNullOrEmpty(_param.UserId))
            expressionsNode.AddExpression(
                x => x.OwnerUserId == _param.UserId,
                Keyword.CreateDummy()
            );

        // qで絞り込み
        foreach (var keyword in GetKeyword(_param.Q))
            expressionsNode.AddExpression(x =>
                (keyword.InQuotes ? x.Content : x.Content.ToLower())
                    .Contains(keyword.Value),
                keyword
            );

        // 内容で絞り込み
        foreach (var keyword in GetKeyword(_param.Content))
            expressionsNode.AddExpression(x =>
                (keyword.InQuotes ? x.Content : x.Content.ToLower())
                    .Contains(keyword.Value),
                keyword
            );

        // ユーザー名で絞り込み
        foreach (var keyword in GetKeyword(_param.UserName))
            expressionsNode.AddExpression(x =>
                (keyword.InQuotes
                    ? x.OwnerUser!.UserName : x.OwnerUser!.UserName.ToLower())
                    .Contains(keyword.Value),
                keyword
            );

        query = query.ApplyExpressionsNode(expressionsNode);

        return query;
    }
}
