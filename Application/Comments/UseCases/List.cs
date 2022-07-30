using Domain.Comments.Entities;
using Application.Shared.UseCases;
using Application.Comments.Models;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Comments.UseCases;

public class List : ListBase<Comment, CommentResultDTO>
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
