using Domain;
using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Core.Query;

namespace Application.TodoItems.Query
{
    public static class QueryMethods
    {
        public static IQueryable<TodoItem> GetFilteredQuery
            (this IQueryable<TodoItem> query, QueryParameter param)
        {
            // query = query.Include(x => x.Comments);

            // qの絞り込み
            if (!string.IsNullOrEmpty(param.q))
            {
                var paramLower = param.q.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(paramLower) ||
                    x.Comments.Any(x => x.Content.ToLower().Contains(paramLower))
                );
            }

            // Nameの絞り込み
            if (!string.IsNullOrEmpty(param.Name))
            {
                var paramLower = param.Name.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(paramLower)
                );
            }

            // コメントでの絞り込み
            if (!string.IsNullOrEmpty(param.Comment))
            {
                var paramLower = param.Comment.ToLower();
                query = query.Where(x =>
                    x.Comments.Any(x => x.Content.ToLower().Contains(paramLower))
                );
            }

            // 状態での絞り込み
            if (param.IsComplete != null)
            {
                query = query.Where(x => x.IsComplete == param.IsComplete);
            }

            return query;
        }
    }
}