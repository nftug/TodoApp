using System.Text.Json.Serialization;
using Domain.Todos.ValueObjects;

namespace Application.Todos.Models;

public class TodoStateDTO : TodoState
{
    public TodoStateDTO(int? value) : base(value) { }

    [JsonConstructor]
    public TodoStateDTO() : base(null) { }
}
