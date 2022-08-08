using System.Text.Json.Serialization;
using Application.Shared.Interfaces;
using Domain.Users.Entities;

namespace Application.Users.Models;

public class UserResultDTO
{
    public class Me : IResultDTO<User>
    {
        public Guid Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public DateTime CreatedOn { get; init; }
        public DateTime UpdatedOn { get; init; }
        public Guid? OwnerUserId { get; init; }

        public Me(User user)
        {
            Id = user.Id;
            UserName = user.UserName.Value;
            Email = user.Email.Value;
            CreatedOn = user.CreatedOn;
            UpdatedOn = user.UpdatedOn;
            OwnerUserId = user.OwnerUserId;
        }

        [JsonConstructor]
        public Me() { }
    }

    public class Public : IResultDTO<User>
    {
        public Guid Id { get; init; }
        public string UserName { get; init; } = string.Empty;
        public DateTime CreatedOn { get; init; }
        public DateTime UpdatedOn { get; init; }
        public Guid? OwnerUserId { get; init; }

        public Public(User user)
        {
            Id = user.Id;
            UserName = user.UserName.Value;
            CreatedOn = user.CreatedOn;
            UpdatedOn = user.UpdatedOn;
            OwnerUserId = user.OwnerUserId;
        }

        [JsonConstructor]
        public Public() { }
    }
}
