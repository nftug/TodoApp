using AutoMapper;
using Domain;
using Application.TodoItems;
using Application.Comments;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TodoItem, TodoItem>();
            CreateMap<TodoItem, TodoItemDTO>();
            CreateMap<TodoItemDTO, TodoItem>()
                .ForMember(x => x.CreatedById, opt => opt.UseDestinationValue())
                .ForMember(x => x.CreatedAt, opt => opt.UseDestinationValue())
                .ForMember(x => x.Comments, opt => opt.Ignore());

            CreateMap<Comment, Comment>();
            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>()
                .ForMember(x => x.CreatedAt, opt => opt.UseDestinationValue())
                .ForMember(x => x.TodoItemId, opt => opt.UseDestinationValue());
        }
    }
}