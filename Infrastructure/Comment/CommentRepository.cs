using AutoMapper;
using Domain.Comment;
using Domain.Interfaces;
using Infrastructure.DataModels;
using Infrastructure.Services.Repository;

namespace Infrastructure.Comment;

public class CommentRepository : RepositoryBase<CommentModel>
{
    public CommentRepository(DataContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    protected override IQueryable<CommentDataModel> Source => _context.Comment;

    protected override IEntity<CommentModel> MapToEntity(CommentModel item)
        => _mapper.Map<CommentDataModel>(item);

    protected override async Task AddEntityAsync(IEntity<CommentModel> entity)
        => await _context.Comment.AddAsync((CommentDataModel)entity);

    protected override void UpdateEntity(IEntity<CommentModel> entity)
        => _context.Comment.Update((CommentDataModel)entity);

    protected override void RemoveEntity(IEntity<CommentModel> entity)
        => _context.Comment.Remove((CommentDataModel)entity);
}
