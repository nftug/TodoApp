using Domain.Comments.Entities;
using Domain.Interfaces;
using Application.Shared.UseCases;

namespace Application.Comments.UseCases;

public class Delete : DeleteBase<Comment>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Comment> repository,
            IDomainService<Comment> domain
        ) : base(repository, domain)
        {
        }
    }
}
