using Client.Shared.Extensions;

namespace Client.Shared.Exceptions;

public class HttpValidationErrorException : Exception
{
    public IDictionary<string, string[]>? Errors { get; }

    private HttpValidationErrorException(IDictionary<string, string[]>? errors)
    {
        Errors = errors;
    }

    public static async Task<HttpValidationErrorException> CreateAsync(HttpResponseMessage response)
    {
        var errorDetails = await response.GetErrorDetailsAsync();
        return new(errorDetails?.Errors);
    }
}
