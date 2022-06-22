using AutoMapper;
using Domain.User;
using Infrastructure.DataModels;

namespace Infrastructure.User;

public class UserRepositoryMapping : Profile
{
    public UserRepositoryMapping()
    {
        CreateMap<UserModel, UserDataModel<Guid>>()
            .ForMember(
                d => d.UserName,
                o => o.MapFrom(s => s.UserName.Value)
            )
            .ForMember(
                d => d.Email,
                o => o.MapFrom(s => s.Email.Value)
            );

        CreateMap<UserDataModel<Guid>, UserModel>()
            .ForMember(
                d => d.UserName,
                o => o.MapFrom(s => new UserName(s.UserName))
            )
            .ForMember(
                d => d.Email,
                o => o.MapFrom(s => new UserEmail(s.Email))
            );
    }
}
