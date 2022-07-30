using Application.Shared.UseCases;
using Application.Users.Models;
using Domain.Interfaces;
using Domain.Users.Entities;

namespace Application.Users.UseCases;

public class Details
{
    public class Me : DetailsBase<User, UserResultDTO.Me>
    {
        public class Handler : HandlerBase
        {
            public Handler(
                IRepository<User> repository,
                IDomainService<User> domain
            ) : base(repository, domain)
            {
            }

            protected override UserResultDTO.Me CreateDTO(User item)
                => new(item);
        }
    }

    public class Public : DetailsBase<User, UserResultDTO.Public>
    {
        public class Handler : HandlerBase
        {
            public Handler(
                IRepository<User> repository,
                IDomainService<User> domain
            ) : base(repository, domain)
            {
            }

            protected override UserResultDTO.Public CreateDTO(User item)
                => new(item);
        }
    }
}
