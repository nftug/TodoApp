using Application.Shared.UseCases;
using Domain.Users.Entities;
using Application.Users.Models;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Users.UseCases;

public class Edit : EditBase<User, UserResultDTO.Me, UserCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<User> repository,
            IDomainService<User> domain
        ) : base(repository, domain)
        {
        }

        protected override UserResultDTO.Me CreateDTO(User item)
            => new(item);

        protected override void Put(User origin, UserCommandDTO command)
        {
            origin.Edit(
                userName: new(command.Username),
                email: new(command.Email)
            );
        }

        protected override void Patch(User origin, UserCommandDTO command)
        {
            origin.Edit(
                userName: command.Username != null ? new(command.Username) : origin.UserName,
                email: command.Email != null ? new(command.Email) : origin.Email
            );
        }
    }
}
