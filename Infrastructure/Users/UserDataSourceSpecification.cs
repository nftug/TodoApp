using Domain.Users.Entities;
using Infrastructure.DataModels;
using Infrastructure.Shared.Specifications.DataSource;

namespace Infrastructure.Users;

internal class UserDataSourceSpecification : DataSourceSpecificationBase<User>
{
    public UserDataSourceSpecification(DataContext context) : base(context)
    {
    }

    public override IQueryable<IDataModel<User>> Source => _context.Users;

    public override User ToDomain(IDataModel<User> origin, bool recursive = false)
    {
        var _origin = (UserDataModel)origin;

        return new User(
            id: _origin.Id,
            createdOn: _origin.CreatedOn,
            updatedOn: _origin.UpdatedOn,
            ownerUserId: _origin.OwnerUserId,
            userName: new(_origin.UserName),
            email: new(_origin.Email)
        );
    }
}
