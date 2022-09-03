using Domain.Shared.Models;

namespace Client.Services.Api;

public interface IApiService<TResultDTO, TCommandDTO, TQueryParameter>
{
    Task<Pagination<TResultDTO>?> GetList(TQueryParameter param, bool showValidationError = false);

    Task<TResultDTO?> Get(Guid id, bool showValidationError = false);

    Task<TResultDTO?> Create(TCommandDTO command, bool showValidationError = false);

    Task<TResultDTO?> Put(Guid id, TCommandDTO command, bool showValidationError = false);

    Task<TResultDTO?> Delete(Guid id, bool showValidationError = false);
}
