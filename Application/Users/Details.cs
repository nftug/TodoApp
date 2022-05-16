using MediatR;
using Application.Core;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.Users;

public class Details
{
    public class Public
    {
        public class Query : IRequest<Result<UserDTO.Public>>
        {
            public string Id { get; set; }
            public string? UserId { get; set; }

            public Query(string id, string? userId)
            {
                Id = id;
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, Result<UserDTO.Public>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<UserDTO.Public>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

                if (user == null)
                    return Result<UserDTO.Public>.NotFound();

                return Result<UserDTO.Public>.Success(
                    new UserDTO.Public { Id = user.Id, Username = user.UserName }
                );
            }
        }
    }

    public class Me
    {
        public class Query : IRequest<Result<UserDTO.Me>>
        {
            public string UserId { get; set; }

            public Query(string userId)
            {
                UserId = userId;
            }
        }

        public class Handler : IRequestHandler<Query, Result<UserDTO.Me>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<UserDTO.Me>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

                if (user == null)
                    return Result<UserDTO.Me>.NotFound();

                return Result<UserDTO.Me>.Success(
                    new UserDTO.Me { Id = user.Id, Username = user.UserName, Email = user.Email }
                );
            }
        }
    }
}
