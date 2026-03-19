namespace Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

public record ResultType<T>(bool IsSuccess, T? Value, string? Message) : ICommandResult
{
    public static ResultType<T> Success(T value, string? message = null) => new(true, value, message);

    public static ResultType<T> Fail(string message)
    {
        string finalMessage = string.IsNullOrWhiteSpace(message)
            ? $"Произошла неизвестная ошибка при получении данных типа {typeof(T).Name}"
            : message;
        return new ResultType<T>(false, default, finalMessage);
    }
}