using Domain.Comments.Entities;
using Application.Shared.UseCases;
using Domain.Shared.Interfaces;
using Domain.Services;
using Domain.Comments.DTOs;

namespace Application.Comments.UseCases;

public class Create
    : CreateBase<Comment, CommentResultDTO, CommentCommand>
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