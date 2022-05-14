using MediatR;
using Application.Core.Exceptions;
using Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Application.Users
{
    public class Details
    {
        public class Public
        {
            public class Query : IRequest<UserDTO.Public>
            {
                public string Id { get; set; } = string.Empty;
                public string? UserId { get; set; }
            }

            public class Handler : IRequestHandler<Query, UserDTO.Public>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public Handler(DataContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<UserDTO.Public> Handle(Query request, CancellationToken cancellationToken)
                {
                    var result = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.Id);

                    if (result == null)
                        throw new NotFoundException();

                    return new UserDTO.Public
                    {
                        Id = result.Id,
                        Username = result.UserName
                    };
                }
            }
        }

        public class Me
        {
            public class Query : IRequest<UserDTO.Me>
            {
                public string? UserId { get; set; }
            }

            public class Handler : IRequestHandler<Query, UserDTO.Me>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public Handler(DataContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<UserDTO.Me> Handle(Query request, CancellationToken cancellationToken)
                {
                    var result = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

                    if (result == null)
                        throw new NotFoundException();

                    return new UserDTO.Me
                    {
                        Id = result.Id,
                        Username = result.UserName,
                        Email = result.Email
                    };
                }
            }
        }
    }
}