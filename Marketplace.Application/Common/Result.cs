namespace Marketplace.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }
    public ErrorType ErrorType { get; }

    private Result(bool isSuccess, T? value, string? errorMessage, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Value = value;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value) =>
        new(true, value, null, ErrorType.None);

    public static Result<T> Failure(string errorMessage, ErrorType errorType) =>
        new(false, default, errorMessage, errorType);
}
