using Application.Shared.UseCase;
using Domain.Interfaces;
using Domain.User;

namespace Application.User;

public class Delete : DeleteBase<UserModel>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<UserModel> repository,
            IDomainService<UserModel> domain
        ) : base(repository, domain)
        {
        }
    }
}
