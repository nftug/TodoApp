using Domain.Users;
using Domain.Shared;
using Infrastructure.DataModels;

namespace Infrastructure.Users;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<User> UpdateAsync(User user)
    {
        var foundUserDataModel = await _context.Users.FindAsync(user.Id);

        if (foundUserDataModel == null)
            throw new NotFoundException();

        var userDataModel = Transfer(user, foundUserDataModel);

        _context.Users.Update(userDataModel);
        await _context.SaveChangesAsync();

        return ToModel(userDataModel);
    }

    private UserDataModel Transfer(User user, UserDataModel userDataModel)
    {
        userDataModel.UserName = user.UserName.Value;
        userDataModel.Email = user.Email.Value;

        return userDataModel;
    }

    public async Task<User?> FindAsync(string id)
    {
        var UserDataModel = await _context.Users.FindAsync(id);

        return UserDataModel != null ? ToModel(UserDataModel) : null;
    }

    private User ToModel(UserDataModel userDataModel)
    {
        return new User(
            id: userDataModel.Id,
            userName: new UserUserName(userDataModel.UserName),
            email: new UserEmail(userDataModel.Email)
        );
    }

    public async Task RemoveAsync(string id)
    {
        var userDataModel = await _context.Users.FindAsync(id);

        if (userDataModel == null)
            throw new NotFoundException();

        _context.Users.Remove(userDataModel);
        await _context.SaveChangesAsync();
    }
}
