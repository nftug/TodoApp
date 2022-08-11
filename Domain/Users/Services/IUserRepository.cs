using Domain.Shared.Interfaces;
using Domain.Users.Entities;

namespace Domain.Users.Services;

public interface IUserRepository : IRepository<User>
{
    Task<TokenModel> RegisterAsync(User item, string password);
    Task<TokenModel> LoginAsync(LoginCommand command);
    Task<User?> FindAsync(string email, string userName);
    Task<User?> FindByEmail(string email);
    Task<User?> FindByUserName(string userName);
}
