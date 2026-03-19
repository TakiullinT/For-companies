namespace Itmo.ObjectOrientedProgramming.Lab2.ResultInfo;

public record Result(bool IsSuccess, string? ErrorMessage)
{
    public static Result Success() => new(true, null);

    public static Result Fail(string errorMessage) => new(false, errorMessage);
}