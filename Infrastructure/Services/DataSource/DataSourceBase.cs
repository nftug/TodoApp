using AutoMapper;
using Domain.Interfaces;
using Domain.Shared;

namespace Infrastructure.Services.DataSource;

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

    public virtual TDomain MapToDomain(IEntity<TDomain> entity)
        => _mapper.Map<TDomain>(entity);

    public virtual void Transfer(TDomain item, IEntity<TDomain> entity)
        => _mapper.Map(item, entity);

    public abstract Task AddEntityAsync(IEntity<TDomain> entity);

    public abstract void UpdateEntity(IEntity<TDomain> entity);

    public abstract void RemoveEntity(IEntity<TDomain> entity);

    public virtual IQueryable<TDomain> DomainSource
        => _mapper.ProjectTo<TDomain>(Source);
}
