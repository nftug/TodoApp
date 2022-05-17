using Pagination.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Shared;

internal static class QuerySearchManager
{
    internal static void ForEachKeyword(string? param, Action<string> func)
    {
        if (string.IsNullOrEmpty(param)) return;

        string paramLower = param.ToLower().Replace("　", " ");
        string[] paramArray = paramLower.Split(' ');

        foreach (var _param in paramArray)
        {
            if (!string.IsNullOrEmpty(_param))
            {
                func(_param);
            }
        }
    }

    internal async static Task<Pagination<T>> GetPaginatedResultsAsync<T>
        (this IQueryable<T> query, PaginationQueryParameterBase param)
    {
        int page = param.Page ?? 1;
        int limit = param.Limit ?? 10;
        var results = await query.Skip((page - 1) * limit).Take(limit)
                                 .ToListAsync();
        int count = await query.CountAsync();

        return new Pagination<T>(results, count, page, limit);
    }
}
