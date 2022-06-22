using Domain.Interfaces;
using Domain.Shared;
using Infrastructure.Shared.QuerySearch.Models;

namespace Infrastructure.Shared.QuerySearch;

public abstract class QuerySearchServiceBase<TDomain> : IQuerySearch<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;

    public QuerySearchServiceBase(DataContext context)
    {
        _context = context;
    }

    public abstract IQueryable<IEntity<TDomain>> GetFilteredQuery(IQueryParameter<TDomain> param);

    protected static IEnumerable<Keyword> GetKeyword(string? param)
        => Keyword.CreateFromRawString(param);

}
