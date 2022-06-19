using AutoMapper;
using Domain.Comments;
using Infrastructure.DataModels;

namespace Infrastructure.Todos;

public class CommentRepositoryMapping : Profile
{
    public CommentRepositoryMapping()
    {
        CreateMap<Comment, CommentDataModel>()
            .ForMember(
                d => d.Content,
                o => o.MapFrom(s => s.Content.Value)
            );

        CreateMap<CommentDataModel, Comment>()
            .ForMember(
                d => d.Content,
                o => o.MapFrom(s => new CommentContent(s.Content))
            );
    }
}