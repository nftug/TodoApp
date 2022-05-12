using Domain;
using Pagination.EntityFrameworkCore.Extensions;
using Application.Core.Query;

namespace Application.Comments.Query
{
    public static class QueryMethods
    {
        private static IQueryable<Comment> GetFilteredQuery
            (this IQueryable<Comment> query, QueryParameter param)
        {
            // qの絞り込み
            if (!string.IsNullOrEmpty(param.q))
            {
                var paramLower = param.q.ToLower();
                query = query.Where(x =>
                    x.Content.ToLower().Contains(paramLower)
                );
            }

            // Contentの絞り込み
            if (!string.IsNullOrEmpty(param.Content))
            {
                var paramLower = param.Content.ToLower();
                query = query.Where(x =>
                    x.Content.ToLower().Contains(paramLower)
                );
            }

            return query;
        }

        /// <summary>
        /// クエリパラメータで処理したページネーション処理済みデータを返す
        /// </summary>
        public async static Task<Pagination<CommentDTO>> GetQueryResultsAsync
            (this IQueryable<Comment> query, QueryParameter param)
        {
            query = query.GetFilteredQuery(param);
            return await query.GetPaginatedResultsAsync(param);
        }
    }
}