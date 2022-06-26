using System.Text.RegularExpressions;
using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.Services.QueryService.Extensions;
using Infrastructure.Services.QueryService.Models;

namespace Infrastructure.Services.QueryService;

public abstract class QueryServiceBase<TDomain> : IQueryService<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;

    public QueryServiceBase(DataContext context)
    {
        _context = context;
    }

    public virtual IQueryable<IEntity<TDomain>> GetFilteredQuery
        (IQueryParameter<TDomain> param)
    {
        var query = GetQueryByParameter(param);
        return OrderQuery(query, param);
    }

    protected abstract IQueryable<IEntity<TDomain>> GetQueryByParameter
        (IQueryParameter<TDomain> param);

    protected static IEnumerable<Keyword> GetKeyword(string? param)
        => Keyword.CreateFromRawString(param);

    protected virtual IQueryable<IEntity<TDomain>> OrderQuery(
        IQueryable<IEntity<TDomain>> query,
        IQueryParameter<TDomain> param
    )
    {
        bool isDescending = Regex.IsMatch(param.Sort, "^-");
        string orderBy = Regex.Replace(param.Sort, "^-", "");

        return orderBy switch
        {
            "createdOn" => query.OrderByAscDesc(x => x.CreatedOn, isDescending),
            "updatedOn" => query.OrderByAscDesc(x => x.UpdatedOn, isDescending),
            _ => query
        };
    }
}
