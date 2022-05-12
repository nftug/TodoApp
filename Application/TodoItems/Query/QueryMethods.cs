using Domain;
using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Application.Core.Query;

namespace Application.TodoItems.Query
{
    public static class QueryMethods
    {
        private static IQueryable<TodoItem> GetFilteredQuery
            (this IQueryable<TodoItem> query, QueryParameter param)
        {
            query = query.Include(x => x.Comments);

            // qの絞り込み
            if (!string.IsNullOrEmpty(param.q))
            {
                var paramLower = param.q.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(paramLower) ||
                    x.Comments.Where(x => x.Content.ToLower().Contains(paramLower)).Any()
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
                    x.Comments.Where(x => x.Content.ToLower().Contains(paramLower)).Any()
                );
            }

            // 状態での絞り込み
            if (param.IsComplete != null)
            {
                query = query.Where(x => x.IsComplete == param.IsComplete);
            }

            return query;
        }

        /// <summary>
        /// クエリパラメータで処理したページネーション処理済みデータを返す
        /// </summary>
        public async static Task<Pagination<TodoItemDTO>> GetQueryResultsAsync
            (this IQueryable<TodoItem> query, QueryParameter param)
        {
            query = query.GetFilteredQuery(param);
            return await query.GetPaginatedResultsAsync(param);
        }
    }
}