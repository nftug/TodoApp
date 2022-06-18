using MediatR;
using Domain.Comments;
using Domain.Interfaces;
using Infrastructure.DataModels;

namespace Application.Comments;

public class Create
{
    public class Command : IRequest<CommentResultDTO>
    {
        public CommentCommandDTO CommentCommandDTO { get; set; }
        public string UserId { get; set; }

        public Command(CommentCommandDTO CommentItemDTO, string usedId)
        {
            CommentCommandDTO = CommentItemDTO;
            UserId = usedId;
        }
    }

    public class Handler : IRequestHandler<Command, CommentResultDTO>
    {
        private readonly IRepository<Comment, CommentDataModel> _commentRepository;

        public Handler
            (IRepository<Comment, CommentDataModel> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<CommentResultDTO> Handle
            (Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.CommentCommandDTO;

            var comment = Comment.CreateNew(
                content: new(inputItem.Content!),
                todoId: inputItem.TodoId,
                ownerUserId: request.UserId
            );

            var result = await _commentRepository.CreateAsync(comment);
            return new CommentResultDTO(result);
        }
    }
}
