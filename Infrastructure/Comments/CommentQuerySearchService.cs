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

        // qで絞り込み
        foreach (string keyword in Keywords(param.q))
        {
            query = query.Where(x => x.Content.ToLower().Contains(keyword));
        }

        // 内容で絞り込み
        foreach (string keyword in Keywords(param.Content))
        {
            query = query.Where(x => x.Content.ToLower().Contains(keyword));
        }

        // ユーザー名で絞り込み
        foreach (string keyword in Keywords(param.Content))
        {
            query = query.Where(x => x.OwnerUser!.UserName.ToLower().Contains(keyword));
        }

        // ユーザーIDで絞り込み
        if (!string.IsNullOrEmpty(param.UserId))
            query = query.Where(x => x.OwnerUserId == param.UserId);

        return query;
    }
}
