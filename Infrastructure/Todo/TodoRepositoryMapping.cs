using AutoMapper;
using Domain.Todo;
using Infrastructure.DataModels;

namespace Infrastructure.Todo;

public class TodoRepositoryMapping : Profile
{
    public TodoRepositoryMapping()
    {
        CreateMap<TodoModel, TodoDataModel>()
            .ForMember(
                d => d.Title,
                o => o.MapFrom(s => s.Title.Value)
            )
            .ForMember(
                d => d.Description,
                o => o.MapFrom(s => s.Description.Value)
            )
            .ForMember(
                d => d.StartDate,
                o => o.MapFrom(s => s.Period!.StartDateValue)
            )
            .ForMember(
                d => d.EndDate,
                o => o.MapFrom(s => s.Period!.EndDateValue)
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

        CreateMap<TodoDataModel, TodoModel>()
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
                o => o.MapFrom(s => new TodoPeriod(s.StartDate, s.EndDate))
            )
            .ForMember(
                d => d.State,
                o => o.MapFrom(s => new TodoState(s.State))
            );
    }
}
