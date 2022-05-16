using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using Persistence;
using AutoMapper;

namespace Application.Users;

public class Edit
{
    public class Command : IRequest<Result<UserDTO.Me?>?>
    {
        public UserDTO.Me User { get; set; }
        public string UserId { get; set; }

        public Command(UserDTO.Me user, string userId)
        {
            User = user;
            UserId = userId;
        }
    }

    public class Handler : IRequestHandler<Command, Result<UserDTO.Me?>?>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO.Me?>?> Handle(Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.User;

            if (request.UserId != inputItem.Id)
                return Result<UserDTO.Me?>.Failure("id", "Incorrect id");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
                return null;

            user.UserName = inputItem.Username ?? user.UserName;
            user.Email = inputItem.Email ?? user.Email;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Result<UserDTO.Me?>.Success(
                new UserDTO.Me { Id = user.Id, Username = user.UserName, Email = user.Email }
            );
        }
    }
}
