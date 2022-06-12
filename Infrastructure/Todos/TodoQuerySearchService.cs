using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared;

namespace Infrastructure.Todos;

public class TodoQuerySearchService : QueryServiceBase<TodoDataModel, TodoQueryParameter>
{
    public TodoQuerySearchService(DataContext context)
        : base(context)
    {
    }

    public override IQueryable<TodoDataModel> GetFilteredQuery(TodoQueryParameter param)
    {
        var query = _context.Todos.Include(x => x.Comments)
                                  .Include(x => x.OwnerUser)
                                  .AsQueryable();

        var expressionsNode = new List<QuerySearchExpression<TodoDataModel>>();

        // ユーザーIDで絞り込み
        if (!string.IsNullOrEmpty(param.UserId))
            expressionsNode.AddExpression(
                x => x.OwnerUserId == param.UserId, CombineMode.And, Guid.NewGuid()
            );

        // 状態で絞り込み
        if (param.State != null)
            expressionsNode.AddExpression(
                x => x.State == param.State, CombineMode.And, Guid.NewGuid()
            );

        // qで絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.q))
        {
            expressionsNode.AddExpression(
                x => x.Title.ToLower().Contains(keyword) ||
                         x.Description!.ToLower().Contains(keyword) ||
                         x.Comments.Any(x => x.Content.ToLower().Contains(keyword)) ||
                         x.OwnerUser!.UserName.ToLower().Contains(keyword),
                combineMode, blockId
            );
        }

        // タイトルで絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.Title))
            expressionsNode.AddExpression(
                x => x.Title.ToLower().Contains(keyword),
                combineMode, blockId
            );

        // 説明文で絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.Description))
            expressionsNode.AddExpression(
                x => x.Description!.ToLower().Contains(keyword),
                combineMode, blockId
            );

        // コメントで絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.Comment))
            expressionsNode.AddExpression(
                x => x.Comments.Any(x => x.Content.ToLower().Contains(keyword)),
                combineMode, blockId
            );

        // ユーザー名で絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.UserName))
            expressionsNode.AddExpression(
                x => x.OwnerUser!.UserName.ToLower().Contains(keyword),
                combineMode, blockId
            );

        // クエリ式を作成する
        query = query.ApplyExpressionsNode(expressionsNode);

        return query;
    }
}
