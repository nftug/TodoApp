using Domain.Comment;
using Domain.Interfaces;
using Application.Shared.UseCase;
using MediatR;
using Domain.Shared;

namespace Application.Comment;

public class Delete : DeleteBase<CommentModel>
{
    public class Handler : HandlerBase
    {
        public Handler(IRepository<CommentModel> repository)
            : base(repository)
        {
        }

        public override async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var item = await _repository.FindAsync(request.Id);

            if (item == null)
                throw new NotFoundException();

            if (item.OwnerUserId != request.UserId &&
                item.Todo.OwnerUserId != request.UserId)
                throw new BadRequestException();

            await _repository.RemoveAsync(request.Id);
            return Unit.Value;
        }
    }
}
