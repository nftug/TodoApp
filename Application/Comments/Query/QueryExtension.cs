using Domain;

namespace Application.Comments.Query
{
    public static class QueryExtension
    {
        public static IQueryable<Comment> GetFilteredQuery
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
    }
}