namespace Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

public interface ICommandResult
{
    bool IsSuccess { get; }

    string? Message { get; }
}