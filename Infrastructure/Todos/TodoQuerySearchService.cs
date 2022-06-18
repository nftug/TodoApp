using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared.QuerySearch;
using Infrastructure.Shared.QuerySearch.Models;
using Infrastructure.Shared.QuerySearch.Extensions;
using Domain.Interfaces;

namespace Infrastructure.Todos;

public class TodoQuerySearchService
    : QuerySearchServiceBase<TodoDataModel>
{
    public TodoQuerySearchService(DataContext context)
        : base(context)
    {
    }

    public override IQueryable<TodoDataModel> GetFilteredQuery
        (IQueryParameter<TodoDataModel> param)
    {
        var _param = (TodoQueryParameter)param;

        var query = _context.Todos
            .Include(x => x.Comments)
            .Include(x => x.OwnerUser)
            .AsQueryable();

        var expressionsNode = new List<QuerySearchExpression<TodoDataModel>>();

        // ユーザーIDで絞り込み
        if (!string.IsNullOrEmpty(_param.UserId))
            expressionsNode.AddExpression(
                x => x.OwnerUserId == _param.UserId,
                Keyword.CreateDummy()
            );

        // 状態で絞り込み
        if (_param.State != null)
            expressionsNode.AddExpression(
                x => x.State == _param.State,
                Keyword.CreateDummy()
            );

        // qで絞り込み
        foreach (var keyword in GetKeyword(_param.q))
            expressionsNode.AddExpression(x =>
                    x.Title.ToLower().Contains(keyword.Value) ||
                    x.Description!.ToLower().Contains(keyword.Value) ||
                    x.Comments.Any(x => x.Content.ToLower().Contains(keyword.Value)),
                keyword
            );

        // タイトルで絞り込み
        foreach (var keyword in GetKeyword(_param.Title))
            expressionsNode.AddExpression(
                x => x.Title.ToLower().Contains(keyword.Value),
                keyword
            );

        // 説明文で絞り込み
        foreach (var keyword in GetKeyword(_param.Description))
            expressionsNode.AddExpression(
                x => x.Description!.ToLower().Contains(keyword.Value),
                keyword
            );

        // コメントで絞り込み
        foreach (var keyword in GetKeyword(_param.Comment))
            expressionsNode.AddExpression(
                x => x.Comments.Any(x => x.Content.ToLower().Contains(keyword.Value)),
                keyword
            );

        // ユーザー名で絞り込み
        foreach (var keyword in GetKeyword(_param.UserName))
            expressionsNode.AddExpression(
                x => x.OwnerUser!.UserName.ToLower().Contains(keyword.Value),
                keyword
            );

        // クエリ式を作成する
        query = query.ApplyExpressionsNode(expressionsNode);

        return query;
    }
}
