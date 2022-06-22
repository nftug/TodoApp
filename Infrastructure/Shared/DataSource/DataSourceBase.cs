using AutoMapper;
using Domain.Interfaces;
using Domain.Shared;

namespace Infrastructure.Shared.DataSource;

public abstract class DataSourceBase<TDomain> : IDataSource<TDomain>
    where TDomain : ModelBase
{
    protected readonly DataContext _context;
    protected readonly IMapper _mapper;

    public DataSourceBase(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public abstract IQueryable<IEntity<TDomain>> Source { get; }
    public abstract IEntity<TDomain> MapToEntity(TDomain item);
    public abstract TDomain MapToDomain(IEntity<TDomain> entity);
    public abstract void Transfer(TDomain item, IEntity<TDomain> entity);

    public abstract Task AddEntityAsync(IEntity<TDomain> entity);
    public abstract void UpdateEntity(IEntity<TDomain> entity);
    public abstract void RemoveEntity(IEntity<TDomain> entity);
}
