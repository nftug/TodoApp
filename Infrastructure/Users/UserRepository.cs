using Infrastructure.DataModels;
using Domain.Users.Entities;
using Infrastructure.Shared.Services.Repository;
using Infrastructure.Shared.Specifications.DataSource;

namespace Infrastructure.Users;

public class UserRepository : RepositoryBase<User>
{
    public UserRepository(DataContext context) : base(context)
    {
    }

    protected override IDataSourceSpecification<User> DataSource
        => new UserDataSourceSpecification(_context);

    protected override async Task AddEntityAsync(IDataModel<User> entity)
        => await _context.Users.AddAsync((UserDataModel<Guid>)entity);

    protected override void UpdateEntity(IDataModel<User> entity)
        => _context.Users.Update((UserDataModel<Guid>)entity);

    protected override void RemoveEntity(IDataModel<User> entity)
        => _context.Users.Remove((UserDataModel<Guid>)entity);

    protected override IDataModel<User> ToDataModel(User origin)
        => new UserDataModel<Guid>(origin);

    protected override void Transfer(User origin, IDataModel<User> dataModel)
        => ((UserDataModel<Guid>)dataModel).Transfer(origin);
}
