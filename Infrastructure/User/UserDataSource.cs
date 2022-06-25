using AutoMapper;
using Domain.Interfaces;
using Domain.User;
using Infrastructure.DataModels;
using Infrastructure.Shared.DataSource;

namespace Infrastructure.User;

public class UserDataSource : DataSourceBase<UserModel>
{
    public UserDataSource(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public override IQueryable<IEntity<UserModel>> Source => _context.Users;

    public override IEntity<UserModel> MapToEntity(UserModel item)
        => _mapper.Map<UserDataModel<Guid>>(item);

    public override async Task AddEntityAsync(IEntity<UserModel> entity)
        => await _context.Users.AddAsync((UserDataModel<Guid>)entity);

    public override void UpdateEntity(IEntity<UserModel> entity)
        => _context.Users.Update((UserDataModel<Guid>)entity);

    public override void RemoveEntity(IEntity<UserModel> entity)
        => _context.Users.Remove((UserDataModel<Guid>)entity);
}
