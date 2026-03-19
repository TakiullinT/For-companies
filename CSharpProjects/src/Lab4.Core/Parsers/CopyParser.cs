using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class CopyParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 3 && (arguments[0].Equals("copy", StringComparison.OrdinalIgnoreCase) ||
                                        (arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
                                         arguments[1].Equals("copy", StringComparison.OrdinalIgnoreCase)));
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        if (!CanParse(arguments))
        {
            return Result.Fail("Команда не относится к copy");
        }

        int startIndex = arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        string sourcePath = arguments[startIndex + 1].Trim().Trim('"');
        string destinationPath = arguments[startIndex + 2].Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return Result.Fail("Не указан исходный путь для копирования.");
        }

        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            return Result.Fail("Не указан целевой путь для копирования.");
        }

        var command = new FileCopyCommand(sourcePath, destinationPath);
        return ResultType<ICommand>.Success(command, "Команда file copy разобрана.");
    }
}