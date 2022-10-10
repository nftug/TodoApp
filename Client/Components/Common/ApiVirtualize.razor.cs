using Client.Services.Api;
using Client.Shared.Models;
using Domain.Shared.DTOs;
using Domain.Shared.Entities;
using Domain.Shared.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Client.Components.Common;

public partial class ApiVirtualize<TModel, TResultDTO, TCommandDTO, TQueryParameter> : ComponentBase
    where TModel : ModelBase
    where TResultDTO : IResultDTO<TModel>
    where TCommandDTO : ICommand<TModel>
    where TQueryParameter : IQueryParameter<TModel>, new()
{
    [Inject]
    private IApiService<TResultDTO, TCommandDTO, TQueryParameter> ApiService { get; set; } = null!;

    [Parameter]
    public RenderFragment<TResultDTO> ItemContent { get; set; } = null!;
    [Parameter]
    public RenderFragment Placeholder { get; set; } = null!;
    [Parameter]
    public int OverscanCount { get; set; } = 4;
    [Parameter]
    public int ItemSize { get; set; } = 50;
    [Parameter]
    public List<VirtualizedItem<TResultDTO>> CacheList { get; set; } = new();
    [Parameter]
    public EventCallback<List<VirtualizedItem<TResultDTO>>> OnCacheListChanged { get; set; }

    private int? totalCount;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        TQueryParameter queryParameter = new() { Limit = 1 };
        totalCount = (await ApiService.GetList(queryParameter))!.TotalItems;
    }

    private async ValueTask<ItemsProviderResult<TResultDTO>> LoadList(ItemsProviderRequest request)
    {
        TQueryParameter queryParameter = new()
        {
            Limit = request.Count,
            StartIndex = request.StartIndex
        };

        if (CacheList.Count < request.StartIndex + request.Count)
        {
            var paginated = await ApiService.GetList(queryParameter);
            CacheList = CacheList.GetUnionList(paginated!.Results, request.StartIndex);
            await OnCacheListChanged.InvokeAsync(CacheList);
        }

        var results = CacheList.GetItems(request.StartIndex, request.Count);

        return new ItemsProviderResult<TResultDTO>(results, totalCount ??= 0);
    }
}
