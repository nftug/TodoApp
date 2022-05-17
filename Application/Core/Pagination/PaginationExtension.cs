using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.Pagination;

// TODO: あとで消去

internal static class PaginationExtension
{
    // <summary>
    // クエリからページネーション処理を行った結果を返す
    // </summary>
    internal async static Task<Pagination<T>> GetPaginatedResultsAsync<T>
        (this IQueryable<T> query, QueryParameterBase param)
    {
        int page = param.Page ?? 1;
        int limit = param.Limit ?? 10;
        var results = await query.Skip((page - 1) * limit).Take(limit)
                                 .ToListAsync();
        int count = await query.CountAsync();

        return new Pagination<T>(results, count, page, limit);
    }
}
