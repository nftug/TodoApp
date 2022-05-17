using Domain;
using Application.Core.Query;
using Persistence.DataModels;

namespace Application.Comments.Query;

internal static class QueryExtension
{
    internal static IQueryable<CommentDataModel> GetFilteredQuery
        (this IQueryable<CommentDataModel> query, QueryParameter param)
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
