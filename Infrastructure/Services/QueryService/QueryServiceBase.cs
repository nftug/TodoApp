using Domain.Interfaces;
using Domain.Shared;
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

    public abstract IQueryable<IEntity<TDomain>> GetFilteredQuery(IQueryParameter<TDomain> param);

    protected static IEnumerable<Keyword> GetKeyword(string? param)
        => Keyword.CreateFromRawString(param);
}
