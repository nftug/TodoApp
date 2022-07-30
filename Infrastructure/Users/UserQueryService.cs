using Domain.Shared.Interfaces;
using Domain.Shared.Queries;
using Domain.Users.Entities;
using Infrastructure.DataModels;

namespace Infrastructure.Users;

public class UserQueryService : IQueryService<User>
{
    public IQueryable<IDataModel<User>> GetFilteredQuery(IQueryable<IDataModel<User>> source, IQueryParameter<User> param)
    {
        throw new NotImplementedException();
    }
}
