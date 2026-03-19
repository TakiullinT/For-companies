namespace Itmo.ObjectOrientedProgramming.Lab3.ResultInfo;

public record ResultType<T>(bool IsSuccess, T? Value, string? ErrorMessage)
{
    public static ResultType<T> Success(T value) => new(true, value, null);

    public static ResultType<T> Fail(string errorMessage) => new(false, default, errorMessage);
}
