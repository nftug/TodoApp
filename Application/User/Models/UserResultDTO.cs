using Application.Shared.Interfaces;
using Domain.Users.Entities;

namespace Application.Users.Models;

public class UserResultDTO
{
    public class Me : IResultDTO<User>
    {
        public Guid Id { get; }
        public string Username { get; }
        public string Email { get; }
        public DateTime CreatedOn { get; }
        public DateTime UpdatedOn { get; }
        public Guid? OwnerUserId { get; }

        public Me(User user)
        {
            Id = user.Id;
            Username = user.UserName.Value;
            Email = user.Email.Value;
            CreatedOn = user.CreatedOn;
            UpdatedOn = user.UpdatedOn;
            OwnerUserId = user.OwnerUserId;
        }
    }

    public class Public : IResultDTO<User>
    {
        public Guid Id { get; }
        public string Username { get; }
        public DateTime CreatedOn { get; }
        public DateTime UpdatedOn { get; }
        public Guid? OwnerUserId { get; }

        public Public(User user)
        {
            Id = user.Id;
            Username = user.UserName.Value;
            CreatedOn = user.CreatedOn;
            UpdatedOn = user.UpdatedOn;
            OwnerUserId = user.OwnerUserId;
        }
    }
}
