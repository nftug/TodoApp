using Domain.User;
using Infrastructure.Services.Repository;
using Domain.Interfaces;

namespace Infrastructure.User;

public class UserRepository : RepositoryBase<UserModel>
{
    public UserRepository(DataContext context, IDataSource<UserModel> source)
        : base(context, source)
    {
    }
}
