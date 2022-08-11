using Application.Shared.UseCases;
using Domain.Users.Entities;
using Application.Users.Models;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Users.UseCases;

public class Patch : EditBase<User, UserResultDTO.Me, UserPatchCommand>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<User> repository, IDomainService<User> domain)
            : base(repository, domain)
        {
        }

        protected override UserResultDTO.Me CreateDTO(User item)
            => new(item);

        protected override void Edit(User origin, UserPatchCommand command)
        {
            origin.Edit(
                userName: command.UserName != null ? new(command.UserName) : origin.UserName,
                email: command.Email != null ? new(command.Email) : origin.Email
            );
        }
    }
}
