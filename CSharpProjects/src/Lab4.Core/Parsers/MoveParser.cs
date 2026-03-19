using Itmo.ObjectOrientedProgramming.Lab4.Core.Commands;
using Itmo.ObjectOrientedProgramming.Lab4.Core.ResultInfo;

namespace Itmo.ObjectOrientedProgramming.Lab4.Core.Parsers;

public class MoveParser : ICommandParser
{
    public bool CanParse(IReadOnlyList<string> arguments)
    {
        return arguments.Count >= 3 && (arguments[0].Equals("move", StringComparison.OrdinalIgnoreCase) ||
                                        (arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) &&
                                         arguments[1].Equals("move", StringComparison.OrdinalIgnoreCase)));
    }

    public ICommandResult Parse(IReadOnlyList<string> arguments)
    {
        if (!CanParse(arguments))
        {
            return Result.Fail("Команда не относится к move");
        }

        int startIndex = arguments[0].Equals("file", StringComparison.OrdinalIgnoreCase) ? 1 : 0;
        string sourcePath = arguments[startIndex + 1].Trim().Trim('"');
        string destinationPath = arguments[startIndex + 2].Trim().Trim('"');

        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return Result.Fail("Не указан исходный путь для перемещения");
        }

        if (string.IsNullOrWhiteSpace(destinationPath))
        {
            return Result.Fail("Не указан целевой путь для перемещения");
        }

        var command = new FileMoveCommand(sourcePath, destinationPath);
        return ResultType<ICommand>.Success(command, "Команда file move разобрана");
    }
}