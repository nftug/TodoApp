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

    public override UserModel MapToDomain(IEntity<UserModel> entity)
        => _mapper.Map<UserModel>(entity);

    public override IEntity<UserModel> MapToEntity(UserModel item)
        => _mapper.Map<UserDataModel<Guid>>(item);

    public override void Transfer(UserModel item, IEntity<UserModel> entity)
        => _mapper.Map(item, entity);

    public override async Task AddEntityAsync(IEntity<UserModel> entity)
        => await _context.Users.AddAsync((UserDataModel<Guid>)entity);

    public override void UpdateEntity(IEntity<UserModel> entity)
        => _context.Users.Update((UserDataModel<Guid>)entity);

    public override void RemoveEntity(IEntity<UserModel> entity)
        => _context.Users.Remove((UserDataModel<Guid>)entity);
}
