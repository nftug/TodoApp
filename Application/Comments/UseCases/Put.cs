using Domain.Comments.Entities;
using Application.Shared.UseCases;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Comments.DTOs;

namespace Application.Comments.UseCases;

public class Put
    : EditBase<Comment, CommentResultDTO, CommentCommand>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment> repository, IDomainService<Comment> domain)
             : base(repository, domain)
        {
        }

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);

        protected override void Edit(Comment origin, CommentCommand item)
        {
            origin.Edit(new(item.Content));
        }
    }
}
