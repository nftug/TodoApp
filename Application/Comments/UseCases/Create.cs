using Domain.Comments.Entities;
using Application.Shared.UseCases;
using Application.Comments.Models;
using Domain.Shared.Interfaces;
using Domain.Services;

namespace Application.Comments.UseCases;

public class Create
    : CreateBase<Comment, CommentResultDTO, CommentCommandDTO>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<Comment> repository, IDomainService<Comment> domain)
            : base(repository, domain)
        {
        }

        protected override Comment CreateDomain(Command request)
            => Comment.CreateNew(
                content: new(request.Item.Content!),
                todoId: request.Item.TodoId,
                ownerUserId: request.UserId
            );

        protected override CommentResultDTO CreateDTO(Comment item)
            => new(item);
    }
}