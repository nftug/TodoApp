using Domain.User;
using Infrastructure.Services.Repository;
using AutoMapper;
using Domain.Interfaces;
using Infrastructure.DataModels;

namespace Infrastructure.User;

public class UserRepository : RepositoryBase<UserModel>
{
    public UserRepository(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    protected override IQueryable<IEntity<UserModel>> Source => _context.Users;

    protected override IEntity<UserModel> MapToEntity(UserModel item)
        => _mapper.Map<UserDataModel<Guid>>(item);

    protected override async Task AddEntityAsync(IEntity<UserModel> entity)
        => await _context.Users.AddAsync((UserDataModel<Guid>)entity);

    protected override void UpdateEntity(IEntity<UserModel> entity)
        => _context.Users.Update((UserDataModel<Guid>)entity);

    protected override void RemoveEntity(IEntity<UserModel> entity)
        => _context.Users.Remove((UserDataModel<Guid>)entity);
}
