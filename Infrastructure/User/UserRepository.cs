using Domain.User;
using Infrastructure.Shared.Repository;
using Domain.Interfaces;

namespace Infrastructure.User;

public class UserRepository : RepositoryBase<UserModel>
{
    public UserRepository(DataContext context, IDataSource<UserModel> source)
        : base(context, source)
    {
    }
}
