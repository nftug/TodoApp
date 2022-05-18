namespace Domain.Users;

public interface IUserRepository
{
    Task<User> UpdateAsync(User user);
    Task<User?> FindAsync(string id);
    Task RemoveAsync(string id);
}
