using Microsoft.EntityFrameworkCore;
using Infrastructure.DataModels;
using Infrastructure.Shared;

namespace Infrastructure.Comments;

public class CommentQuerySearchService : QueryServiceBase<CommentDataModel, CommentQueryParameter>
{
    public CommentQuerySearchService(DataContext context)
        : base(context)
    {
    }

    public override IQueryable<CommentDataModel> GetFilteredQuery(CommentQueryParameter param)
    {
        var query = _context.Comments.Include(x => x.OwnerUser)
                                     .AsQueryable();

        var expressionsNode = new List<QuerySearchExpression<CommentDataModel>>();

        // ユーザーIDで絞り込み
        if (!string.IsNullOrEmpty(param.UserId))
            expressionsNode.AddExpression(
                x => x.OwnerUserId == param.UserId, CombineMode.And, Guid.NewGuid()
            );

        // qで絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.q))
            expressionsNode.AddExpression(
                x => x.Content.ToLower().Contains(keyword),
                combineMode, blockId
            );

        // 内容で絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.Content))
            expressionsNode.AddExpression(
                x => x.Content.ToLower().Contains(keyword),
                combineMode, blockId
            );

        // ユーザー名で絞り込み
        foreach (var (keyword, combineMode, blockId) in Keywords(param.UserName))
            expressionsNode.AddExpression(
                x => x.OwnerUser!.UserName.ToLower().Contains(keyword),
                combineMode, blockId
            );

        query = query.ApplyExpressionsNode(expressionsNode);

        return query;
    }
}
