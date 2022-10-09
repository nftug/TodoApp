using Client.Services.Api;
using Client.Shared.Models;
using Domain.Todos.DTOs;
using Domain.Todos.Queries;
using Domain.Todos.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor;

namespace Client.Pages;

public partial class VirtualizationTestPage : ComponentBase
{
    [Inject]
    private IApiService<TodoResultDTO, TodoCommand, TodoQueryParameter> TodoApiService { get; set; } = null!;

    private int? totalCount;

    private List<VirtualizedItem<TodoResultDTO>> listCache = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        TodoQueryParameter queryParameter = new()
        {
            Limit = 1,
            Page = 1
        };
        totalCount = (int)(await TodoApiService.GetList(queryParameter))!.TotalItems;
    }

    private async ValueTask<ItemsProviderResult<TodoResultDTO>> LoadTodos(ItemsProviderRequest request)
    {
        TodoQueryParameter queryParameter = new()
        {
            Limit = request.Count,
            Page = null,
            StartIndex = request.StartIndex
        };

        if (listCache.Count < request.StartIndex + request.Count)
        {
            var paginated = await TodoApiService.GetList(queryParameter);

            var _results = paginated!.Results
                .Select((x, i) => new VirtualizedItem<TodoResultDTO>
                {
                    Result = x,
                    Index = i + request.StartIndex
                })
                .OrderBy(x => x.Index);

            listCache = listCache.Union(_results).DistinctBy(x => x.Index).OrderBy(x => x.Index).ToList();
        }

        var results = listCache.Skip(request.StartIndex).Take(request.Count).Select(x => x.Result);

        return new ItemsProviderResult<TodoResultDTO>(results, totalCount ??= 0);
    }

    private static Color GetTodoChipColor(TodoResultDTO item)
    {
        var state = new TodoState(item.State);
        return state == TodoState.Doing
            ? Color.Tertiary
            : state == TodoState.Done
            ? Color.Success
            : Color.Primary;
    }
}
