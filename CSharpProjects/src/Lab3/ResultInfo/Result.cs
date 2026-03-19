namespace Itmo.ObjectOrientedProgramming.Lab3.ResultInfo;

public record class Result(bool IsSuccess, string? ErrorMessage)
{
    public static Result Success() => new(true, null);

    public static Result Fail(string errorMessage) => new(false, errorMessage);
}
