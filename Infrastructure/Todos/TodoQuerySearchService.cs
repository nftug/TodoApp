using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared;

namespace Infrastructure.Todos;

public class TodoQuerySearchService
    : QueryServiceBase<TodoDataModel, TodoQueryParameter>
{
    public TodoQuerySearchService(DataContext context)
        : base(context)
    {
    }

    public override IQueryable<TodoDataModel> GetFilteredQuery
        (TodoQueryParameter param)
    {
        var query = _context.Todos
            .Include(x => x.Comments)
            .Include(x => x.OwnerUser)
            .AsQueryable();

        var expressionsNode = new List<QuerySearchExpression<TodoDataModel>>();

        // ユーザーIDで絞り込み
        if (!string.IsNullOrEmpty(param.UserId))
            expressionsNode.AddExpression(
                x => x.OwnerUserId == param.UserId,
                Keyword.CreateDummy()
            );

        // 状態で絞り込み
        if (param.State != null)
            expressionsNode.AddExpression(
                x => x.State == param.State,
                Keyword.CreateDummy()
            );

        // qで絞り込み
        foreach (var keyword in GetKeyword(param.q))
            expressionsNode.AddExpression(x =>
                    x.Title.ToLower().Contains(keyword.Value) ||
                    x.Description!.ToLower().Contains(keyword.Value) ||
                    x.Comments.Any(x => x.Content.ToLower().Contains(keyword.Value)),
                keyword
            );

        // タイトルで絞り込み
        foreach (var keyword in GetKeyword(param.Title))
            expressionsNode.AddExpression(
                x => x.Title.ToLower().Contains(keyword.Value),
                keyword
            );

        // 説明文で絞り込み
        foreach (var keyword in GetKeyword(param.Description))
            expressionsNode.AddExpression(
                x => x.Description!.ToLower().Contains(keyword.Value),
                keyword
            );

        // コメントで絞り込み
        foreach (var keyword in GetKeyword(param.Comment))
            expressionsNode.AddExpression(
                x => x.Comments.Any(x => x.Content.ToLower().Contains(keyword.Value)),
                keyword
            );

        // ユーザー名で絞り込み
        foreach (var keyword in GetKeyword(param.UserName))
            expressionsNode.AddExpression(
                x => x.OwnerUser!.UserName.ToLower().Contains(keyword.Value),
                keyword
            );

        // クエリ式を作成する
        query = query.ApplyExpressionsNode(expressionsNode);

        return query;
    }
}
