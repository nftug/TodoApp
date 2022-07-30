using Application.Shared.UseCases;
using Domain.Services;
using Domain.Shared.Interfaces;
using Domain.Users.Entities;

namespace Application.Users.UseCases;

public class Delete : DeleteBase<User>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<User> repository,
            IDomainService<User> domain
        ) : base(repository, domain)
        {
        }
    }
}
