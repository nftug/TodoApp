using Domain.Interfaces;
using Application.Shared.UseCases;
using Domain.Comments.Entities;
using Application.Comments.Models;

namespace Application.Comments.UseCases;

public class Details : DetailsBase<Comment, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(
            IRepository<Comment> repository,
            IDomainService<Comment> domain
        ) : base(repository, domain)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}
