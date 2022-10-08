using Domain.Comments.Entities;
using Application.Shared.UseCases;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Comments.DTOs;

namespace Application.Comments.UseCases;

public class List : ListBase<Comment, CommentResultDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IFilterQueryService<Comment> repository, IDomainService<Comment> domain)
            : base(repository, domain)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}
