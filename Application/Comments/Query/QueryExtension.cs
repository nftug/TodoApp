using Domain;
using Application.Core.Query;

namespace Application.Comments.Query
{
    public static class QueryExtension
    {
        public static IQueryable<Comment> GetFilteredQuery
            (this IQueryable<Comment> query, QueryParameter param)
        {
            // qの絞り込み
            QueryMethods.ForEachKeyword(param.q, q =>
            {
                query = query.Where(x =>
                    x.Content.ToLower().Contains(q)
                );
            });

            // Contentの絞り込み
            QueryMethods.ForEachKeyword(param.Content, content =>
            {
                query = query.Where(x =>
                    x.Content.ToLower().Contains(content)
                );
            });

            return query;
        }
    }
}