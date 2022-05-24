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

        // qで絞り込み
        foreach (string keyword in Keywords(param.q))
        {
            query = query.Where(x =>
                x.Title.ToLower().Contains(keyword) ||
                x.Description!.ToLower().Contains(keyword) ||
                x.Comments.Any(x => x.Content.ToLower().Contains(keyword)) ||
                x.OwnerUser!.UserName.ToLower().Contains(keyword)
            );
        }

        // タイトルで絞り込み
        foreach (string keyword in Keywords(param.Title))
        {
            query = query.Where(x => x.Title.ToLower().Contains(keyword));
        }

        // 説明文で絞り込み
        foreach (string keyword in Keywords(param.Description))
        {
            query = query.Where(x => x.Description!.ToLower().Contains(keyword));
        }

        // コメントで絞り込み
        foreach (string keyword in Keywords(param.Comment))
        {
            query = query.Where(x => x.Comments.Any(x => x.Content.ToLower().Contains(keyword)));
        }

        // ユーザー名で絞り込み
        foreach (string keyword in Keywords(param.UserName))
        {
            query = query.Where(x => x.OwnerUser!.UserName.ToLower().Contains(keyword));
        }

        // ユーザーIDで絞り込み
        if (!string.IsNullOrEmpty(param.UserId))
            query = query.Where(x => x.OwnerUserId == param.UserId);

        // 状態で絞り込み
        if (param.State != null)
            query = query.Where(x => x.State == param.State);

        return query;
    }
}
