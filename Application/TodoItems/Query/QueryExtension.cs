using Domain;
using Application.Core.Query;

namespace Application.TodoItems.Query;

internal static class QueryExtension
{
    internal static IQueryable<TodoItem> GetFilteredQuery
        (this IQueryable<TodoItem> query, QueryParameter param)
    {
        // qの絞り込み
        QueryMethods.ForEachKeyword(param.q, q =>
        {
            query = query.Where(x =>
                x.Name.ToLower().Contains(q) ||
                x.Comments.Any(x => x.Content.ToLower().Contains(q)) ||
                x.CreatedBy!.UserName.ToLower().Contains(q)
            );
        });

        // Nameの絞り込み
        QueryMethods.ForEachKeyword(param.Name, name =>
        {
            query = query.Where(x =>
                x.Name.ToLower().Contains(name)
            );
        });

        // コメントでの絞り込み
        QueryMethods.ForEachKeyword(param.Comment, comment =>
        {
            query = query.Where(x =>
                x.Comments.Any(x => x.Content.ToLower().Contains(comment))
            );
        });

        // ユーザー名での絞り込み
        QueryMethods.ForEachKeyword(param.UserName, username =>
        {
            query = query.Where(x =>
                x.CreatedBy!.UserName.ToLower().Contains(username)
            );
        });

        // ユーザーIDでの絞り込み
        if (!string.IsNullOrEmpty(param.User))
            query = query.Where(x => x.CreatedBy!.Id == param.User);

        // 状態での絞り込み
        if (param.IsComplete != null)
            query = query.Where(x => x.IsComplete == param.IsComplete);

        return query;
    }
}
