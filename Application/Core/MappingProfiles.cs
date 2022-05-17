using AutoMapper;
using Domain;
using Application.Todos;
using Application.Comments;
using Persistence.DataModels;

namespace Application.Core;

// TODO: あとで消去

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        /*         CreateMap<TodoItem, TodoItem>();
                CreateMap<TodoItem, TodoItemDTO>();
                CreateMap<TodoItemDTO, TodoItem>()
                    .ForMember(x => x.CreatedById, opt => opt.UseDestinationValue())
                    .ForMember(x => x.CreatedAt, opt => opt.UseDestinationValue())
                    .ForMember(x => x.Comments, opt => opt.Ignore()); */

        CreateMap<CommentDataModel, CommentDataModel>();
        CreateMap<CommentDataModel, CommentDTO>();
        CreateMap<CommentDTO, CommentDataModel>()
            .ForMember(x => x.CreatedAt, opt => opt.UseDestinationValue())
            .ForMember(x => x.TodoId, opt => opt.UseDestinationValue());
    }
}
