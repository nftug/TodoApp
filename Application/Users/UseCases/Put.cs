using Application.Shared.UseCases;
using Domain.Users.Entities;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Users.DTOs;

namespace Application.Users.UseCases;

public class Put : EditBase<User, UserResultDTO.Me, UserCommand>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<User> repository, IDomainService<User> domain)
            : base(repository, domain)
        {
        }

        protected override UserResultDTO.Me CreateDTO(User item)
            => new(item);

        protected override void Edit(User origin, UserCommand command)
        {
            origin.Edit(
                userName: new(command.UserName),
                email: new(command.Email)
            );
        }
    }
}
