using Pagination.EntityFrameworkCore.Extensions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.Query
{
    public static class QueryMethodsBase
    {
        // <summary>
        // クエリからページネーション処理を行った結果を返す
        // </summary>
        public async static Task<Pagination<T>> GetPaginatedResultsAsync<T>
            (this IQueryable<IModel<T>> query, QueryParameterBase param)
        {
            var page = param.Page ?? 1;
            var limit = param.Limit ?? 10;
            var results = await query.Select(x => x.ToDTO())
                                     .Skip((page - 1) * limit).Take(limit)
                                     .ToListAsync();
            var count = await query.CountAsync();

            return new Pagination<T>(results, count, page, limit);
        }
    }
}