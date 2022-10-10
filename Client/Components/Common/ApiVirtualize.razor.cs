using Client.Services;
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
    [Inject]
    private VirtualizeStoreService<TResultDTO> Store { get; set; } = null!;

    [Parameter]
    public RenderFragment<TResultDTO> ItemContent { get; set; } = null!;
    [Parameter]
    public RenderFragment Placeholder { get; set; } = null!;
    [Parameter]
    public int OverscanCount { get; set; } = 4;
    [Parameter]
    public int ItemSize { get; set; } = 50;

    private int? totalCount;
    private bool _doNotShow;

    private async ValueTask<ItemsProviderResult<TResultDTO>> LoadList(ItemsProviderRequest request)
    {
        if (totalCount == null)
            totalCount = (await ApiService.GetList(new() { Limit = 1 }))!.TotalItems;

        TQueryParameter queryParameter = new()
        {
            Limit = request.Count,
            StartIndex = request.StartIndex
        };

        if (Store.CacheList.Count < request.StartIndex + request.Count)
        {
            var paginated = await ApiService.GetList(queryParameter);
            Store.CacheList = Store.CacheList.GetUnionList(paginated!.Results, request.StartIndex);
        }

        var results = Store.CacheList.GetItems(request.StartIndex, request.Count);

        return new ItemsProviderResult<TResultDTO>(results, totalCount ??= 0);
    }

    public async Task RefreshDataAsync()
    {
        totalCount = null;
        Store.CacheList = new();

        _doNotShow = true;
        StateHasChanged();

        await Task.Delay(100);

        _doNotShow = false;
        StateHasChanged();
    }
}
