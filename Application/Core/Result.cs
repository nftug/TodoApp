namespace Application.Core;

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public Error? Error { get; set; }
    public bool IsNotFound { get; set; }

    public static Result<T> Success(T value) =>
        new Result<T> { IsSuccess = true, Value = value };

    public static Result<T> Failure(string field, string message) =>
        new Result<T>
        {
            IsSuccess = false,
            Error = new Error(field, message)
        };

    public static Result<T> NotFound() =>
        new Result<T> { IsNotFound = true };
}