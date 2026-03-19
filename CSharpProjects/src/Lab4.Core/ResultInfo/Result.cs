namespace Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

public record Result(bool IsSuccess, string? Message) : ICommandResult
{
    public static Result Success(string? message = null) => new(true, message);

    public static Result Fail(string message)
    {
        string finalMessage = string.IsNullOrWhiteSpace(message)
            ? "Произошла неизвестная ошибка при выполнении операции"
            : message;
        return new Result(false, finalMessage);
    }
}