#nullable disable
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Core.Exceptions;
using Persistence;
using AutoMapper;

namespace Application.Users;

public class Edit
{
    public class Command : IRequest<UserDTO.Me>
    {
        public UserDTO.Me User { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Command, UserDTO.Me>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDTO.Me> Handle(Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.User;

            if (request.UserId != inputItem.Id)
                throw new BadRequestException();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);

            if (user == null)
                throw new BadRequestException();

            user.UserName = inputItem.Username ?? user.UserName;
            user.Email = inputItem.Email ?? user.Email;

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return request.User;
        }
    }
}
