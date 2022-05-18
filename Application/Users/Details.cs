using MediatR;
using Domain.Shared;
using Domain.Users;

namespace Application.Users;

public class Details
{
    public class Public
    {
        public class Query : IRequest<UserResultDTO.Public>
        {
            public string Id { get; set; }
            public string? UserId { get; set; }

            public Query(string id, string? userId)
            {
                Id = id;
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, UserResultDTO.Public>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public async Task<UserResultDTO.Public> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.FindAsync(request.Id);
                if (user == null)
                    throw new NotFoundException();

                return UserResultDTO.Public.CreateResultDTO(user);
            }
        }
    }

    public class Me
    {
        public class Query : IRequest<UserResultDTO.Me>
        {
            public string UserId { get; set; }

            public Query(string userId)
            {
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, UserResultDTO.Me>
        {
            private readonly IUserRepository _userRepository;

            public Handler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }


            public async Task<UserResultDTO.Me> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.FindAsync(request.UserId);
                if (user == null)
                    throw new NotFoundException();

                return UserResultDTO.Me.CreateResultDTO(user);
            }
        }
    }
}
