using Domain;
using Pagination.EntityFrameworkCore.Extensions;
using Application.Core.Query;

namespace Application.TodoItems.Query
{
    public static class QueryMethods
    {
        private static IQueryable<TodoItem> GetFilteredQuery
            (this IQueryable<TodoItem> query, QueryParameter param)
        {
            // qの絞り込み
            if (!string.IsNullOrEmpty(param.q))
            {
                var paramLower = param.q.ToLower();
                query = query.Where(x =>
                    x.Name.ToLower().Contains(paramLower)
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