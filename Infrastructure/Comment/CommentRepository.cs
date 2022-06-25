using Domain.Comment;
using Domain.Interfaces;
using Infrastructure.Services.Repository;

namespace Infrastructure.Comment;

public class CommentRepository : RepositoryBase<CommentModel>
{
    public CommentRepository
        (DataContext context, IDataSource<CommentModel> source)
        : base(context, source)
    {
    }
}
