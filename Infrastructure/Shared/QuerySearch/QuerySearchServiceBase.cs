using Domain.Interfaces;
using Infrastructure.DataModels;
using Infrastructure.Interfaces;
using Infrastructure.Shared.QuerySearch.Models;

namespace Infrastructure.Shared.QuerySearch;

public abstract class QuerySearchServiceBase<TEntity> : IQuerySearch<TEntity>
    where TEntity : DataModelBase
{
    protected readonly DataContext _context;

    public QuerySearchServiceBase(DataContext context)
    {
        _context = context;
    }

    public abstract IQueryable<TEntity> GetFilteredQuery(IQueryParameter<TEntity> param);

    protected static IEnumerable<Keyword> GetKeyword(string? param)
        => Keyword.CreateFromRawString(param);
}
