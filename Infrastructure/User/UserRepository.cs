using Infrastructure.Services.Repository;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Domain.Users.Entities;

namespace Infrastructure.Users;

public class UserRepository : RepositoryBase<User>
{
    protected override IQueryable<IDataModel<User>> Source => throw new NotImplementedException();

    public UserRepository(DataContext context, IQueryService<User> queryService) : base(context, queryService)
    {
    }

    protected override async Task AddEntityAsync(IDataModel<User> entity)
        => await _context.Users.AddAsync((UserDataModel<Guid>)entity);

    protected override void UpdateEntity(IDataModel<User> entity)
        => _context.Users.Update((UserDataModel<Guid>)entity);

    protected override void RemoveEntity(IDataModel<User> entity)
        => _context.Users.Remove((UserDataModel<Guid>)entity);

    protected override IDataModel<User> ToDataModel(User origin)
    {
        throw new NotImplementedException();
    }

    internal override User ToDomain(IDataModel<User> origin, bool recursive = true)
    {
        throw new NotImplementedException();
    }

    protected override void Transfer(User origin, IDataModel<User> dataModel)
    {
        throw new NotImplementedException();
    }
}
