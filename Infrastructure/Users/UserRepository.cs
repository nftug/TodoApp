using Infrastructure.DataModels;
using Domain.Users.Entities;
using Infrastructure.Shared.Services.Repository;
using Infrastructure.Shared.Specifications.DataSource;
using Domain.Users.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Shared.Services.AuthToken;
using Domain.Shared.Exceptions;

namespace Infrastructure.Users;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly UserManager<UserDataModel> _userManager;
    private readonly SignInManager<UserDataModel> _signInManager;
    private readonly AuthTokenService _authTokenService;

    public UserRepository(
        DataContext context,
        UserManager<UserDataModel> userManager,
        SignInManager<UserDataModel> signInManager,
        AuthTokenService authTokenService
    ) : base(context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authTokenService = authTokenService;
    }

    private TokenModel CreateUserObject(UserDataModel userDataModel)
        => new()
        {
            Token = _authTokenService.CreateToken(userDataModel),
            UserName = userDataModel.UserName,
            UserId = userDataModel.Id
        };

    public override Task<User> CreateAsync(User item)
        => throw new NotImplementedException();

    public async Task<TokenModel> RegisterAsync(User item, string password)
    {
        var data = (UserDataModel)ToDataModel(item);

        var result = await _userManager.CreateAsync(data, password);
        if (!result.Succeeded)
        {
            var exception = new DomainException();
            foreach (var error in result.Errors)
                exception.Add(error.Code, error.Description);
            throw exception;
        }

        return CreateUserObject(data);
    }

    public async Task<TokenModel> LoginAsync(LoginCommand command)
    {
        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user == null) throw new UnauthorizedException();

        var result = await _signInManager.CheckPasswordSignInAsync(user, command.Password, false);
        if (result.Succeeded)
            return CreateUserObject(user);
        else
            throw new UnauthorizedException();
    }

    public async Task<User?> FindAsync(string email, string userName)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Email == email || x.UserName == userName);
        return data != null ? DataSource.ToDomain(data, recursive: false) : null;
    }

    public async Task<User?> FindByEmail(string email)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return data != null ? DataSource.ToDomain(data, recursive: false) : null;
    }

    public async Task<User?> FindByUserName(string userName)
    {
        var data = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        return data != null ? DataSource.ToDomain(data, recursive: false) : null;
    }

    protected override IDataSourceSpecification<User> DataSource
        => new UserDataSourceSpecification(_context);

    protected override Task AddEntityAsync(IDataModel<User> entity)
        => throw new NotImplementedException();

    protected override void UpdateEntity(IDataModel<User> entity)
        => _context.Users.Update((UserDataModel)entity);

    protected override void RemoveEntity(IDataModel<User> entity)
        => _context.Users.Remove((UserDataModel)entity);

    protected override IDataModel<User> ToDataModel(User origin)
        => new UserDataModel(origin);

    protected override void Transfer(User origin, IDataModel<User> dataModel)
        => ((UserDataModel)dataModel).Transfer(origin);
}
