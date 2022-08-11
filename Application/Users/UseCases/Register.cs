using MediatR;
using Domain.Shared.Exceptions;
using Domain.Services;
using Domain.Users.Entities;
using Domain.Users.Services;

namespace Application.Users.UseCases;

public class Register
{
    public class Command : IRequest<TokenModel>
    {
        public RegisterCommand Item { get; init; }

        public Command(RegisterCommand item)
        {
            Item = item;
        }
    }

    public class Handler : IRequestHandler<Command, TokenModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IDomainService<User> _domainService;

        public Handler(IUserRepository userRepository, IDomainService<User> domainService)
        {
            _userRepository = userRepository;
            _domainService = domainService;
        }

        public async Task<TokenModel> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var item = CreateDomain(request);
            if (!await _domainService.CanCreate(item, item.Id))
                throw new ForbiddenException();

            var result = await _userRepository.RegisterAsync(item, request.Item.Password);
            return result;
        }

        private static User CreateDomain(Command request)
            => User.CreateNew(
                userName: new(request.Item.UserName),
                email: new(request.Item.Email)
            );
    }
}
