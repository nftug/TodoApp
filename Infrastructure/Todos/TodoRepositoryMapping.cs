using AutoMapper;
using Domain.Todos;
using Infrastructure.DataModels;

namespace Infrastructure.Todos;

public class TodoRepositoryMapping : Profile
{
    public TodoRepositoryMapping()
    {
        CreateMap<Todo, TodoDataModel>()
            .ForMember(
                d => d.Title,
                o => o.MapFrom(s => s.Title.Value)
            )
            .ForMember(
                d => d.Description,
                o => o.MapFrom(s => s.Description.Value)
            )
            .ForMember(
                d => d.BeginDateTime,
                o => o.MapFrom(s => s.Period!.BeginDateTimeValue)
            )
            .ForMember(
                d => d.DueDateTime,
                o => o.MapFrom(s => s.Period!.DueDateTimeValue)
            )
            .ForMember(
                d => d.State,
                o => o.MapFrom(s => s.State.Value)
            )
            .ForMember(
                d => d.Comments,
                o => o.Ignore()
            );

        // For Patch:
        // .ForAllMembers(o => o.Condition((src, dest, value) => value != null));

        CreateMap<TodoDataModel, Todo>()
            .ForMember(
                d => d.Title,
                o => o.MapFrom(s => new TodoTitle(s.Title))
            )
            .ForMember(
                d => d.Description,
                o => o.MapFrom(s => new TodoDescription(s.Description))
            )
            .ForMember(
                d => d.Period,
                o => o.MapFrom(s => new TodoPeriod(s.BeginDateTime, s.DueDateTime))
            )
            .ForMember(
                d => d.State,
                o => o.MapFrom(s => new TodoState(s.State))
            );
    }
}
