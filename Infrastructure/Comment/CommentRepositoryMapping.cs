using AutoMapper;
using Domain.Comment;
using Infrastructure.DataModels;

namespace Infrastructure.Todo;

public class CommentRepositoryMapping : Profile
{
    public CommentRepositoryMapping()
    {
        CreateMap<CommentModel, CommentDataModel>()
            .ForMember(
                d => d.Content,
                o => o.MapFrom(s => s.Content.Value)
            );

        CreateMap<CommentDataModel, CommentModel>()
            .ForMember(
                d => d.Content,
                o => o.MapFrom(s => new CommentContent(s.Content))
            );
    }
}