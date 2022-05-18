using MediatR;
using Domain.Comments;
using Domain.Todos;
using Domain.Shared;

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
        private readonly ICommentRepository _commentRepository;
        private readonly ITodoRepository _todoRepository;

        public Handler
            (ICommentRepository commentRepository, ITodoRepository todoRepository)
        {
            _commentRepository = commentRepository;
            _todoRepository = todoRepository;
        }

        public async Task<CommentResultDTO> Handle(Command request, CancellationToken cancellationToken)
        {
            var inputItem = request.CommentCommandDTO;

            // 外部キーの存在チェック
            var todoDataModel = await _todoRepository.FindAsync(inputItem.TodoId);
            if (todoDataModel == null)
                throw new DomainException("todoId", "Todoアイテムが存在しません");

            var comment = Comment.CreateNew(
                content: new CommentContent(inputItem.Content),
                todoId: inputItem.TodoId,
                ownerUserId: request.UserId
            );

            var result = await _commentRepository.CreateAsync(comment);
            return CommentResultDTO.CreateResultDTO(result);
        }
    }
}
