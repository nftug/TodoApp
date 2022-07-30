using Domain.Comments.Entities;
using Application.Shared.UseCases;
using Domain.Shared.Interfaces;
using Domain.Services;

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
